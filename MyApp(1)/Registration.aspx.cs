using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;

namespace MyApp_1_
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string justHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            lb_error1.Visible = true;
            if(tb_CCinfo.Text.Length != 16)
            {
                lb_error1.Text = "Invalid card number";
                lb_error1.ForeColor = Color.Red;
            }

            if(tb_FName.Text == "" || tb_LName.Text == "" || tb_email.Text == "" || tb_CCinfo.Text == "" || tb_password.Text == "" || tb_LName.Text == "")
            {
                lb_error1.Text = "All fields required.";
                lb_error1.ForeColor = Color.Red;
            }

            ValidateEmail();

            string pwd = tb_password.Text.ToString().Trim(); 

            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            
            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            
            SHA512Managed hashing = new SHA512Managed();
            
            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            
            finalHash = Convert.ToBase64String(hashWithSalt);
            justHash = Convert.ToBase64String(plainHash);
            
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            
            createAccount();


            int scores = checkPassword(tb_password.Text);
            string status = "";
            switch (scores)
            {

                case 1:
                    status = "Password stringth is very weak";
                    break;

                case 2:
                    status = "Password strength is weak";
                    break;

                case 3:
                    status = "Password strength is medium";
                    break;

                case 4:
                    status = "Password strength is strong";
                    break;

                case 5:
                    status = "Password strength is very strong";
                    break;

                default:
                    break;
            }
            lbl_pwdchecker.Text = "Status:" + status;
            if (scores < 4)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                return;
            }
            else 
                lbl_pwdchecker.ForeColor = Color.Green;

            Response.Redirect("HomePage.aspx", false);

        }

        private int checkPassword(string password)
        {
            int score = 0;

            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                score++;
            }

            return score;
        }

        public void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName, @LastName, @CreditCardInfo, @Email, @JustHash, @PasswordHash, @PasswordSalt, @DateOfBirth, @IV, @Key, @isLocked, @attemptcount)"))
                {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_FName.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_LName.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCardInfo", Convert.ToBase64String(encryptData(tb_CCinfo.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@JustHash", justHash);
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DateOfBirth", tb_DOB.Text.Trim());
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@isLocked", "0");
                            cmd.Parameters.AddWithValue("@attemptcount", "0");
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();

                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        protected void btn_login(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        private void ValidateEmail()
        {
            string email = tb_email.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                lbl_pwdchecker.Text = email + " is Valid Email Address";
            else
                lbl_pwdchecker.Text = email + " is Invalid Email Address";
        }

    }
}