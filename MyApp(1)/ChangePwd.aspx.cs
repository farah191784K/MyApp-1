using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyApp_1_
{
    public partial class ForgotPwd : System.Web.UI.Page
    {
        int attempts;
        string isLocked;

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        string str = null;
        SqlCommand com;
        byte up;
        static string finalHash;
        static string justHash;
        static string salt;

        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_pwdchecker.Visible = false;
            lblMessage.Visible = false;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            lbl_pwdchecker.Visible = true;
            lblMessage.Visible = true;
            lbl_pwdchecker.Text = "";

            int scores = checkPassword(tb_pwd.Text);

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
            string password = tb_CurrPwd.Text.ToString().Trim();

            SHA512Managed hash = new SHA512Managed();

            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            string pwdWithSalt = password + salt;
            byte[] plainHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] hashWithSalt = hash.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            justHash = Convert.ToBase64String(plainHash);
            finalHash = Convert.ToBase64String(hashWithSalt);

            lblMessage.Visible = true;

            attempts = Convert.ToInt32(ViewState["attemptcount"]);
            isLocked = Convert.ToString(ViewState["isLocked"]);

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            using (SqlConnection connection = new SqlConnection(MYDBConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from Account where Email=@Email", connection);
                cmd.Parameters.AddWithValue("@Email", tb_email.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                        SqlConnection con = new SqlConnection(MYDBConnectionString);
                        con.Open();
                        str = "select * from Account ";
                        com = new SqlCommand(str, con);
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            if(isLocked == reader["isLocked"].ToString())
                            {
                                lblMessage.Text += "Account is already locked.";
                            }

                            if (justHash.ToString() == reader["JustHash"].ToString())
                            {
                                up = 1;
                            }
                            else
                            {
                                lbl_pwdchecker.Text += "Please enter correct Current password" + "<br/>";
                                lbl_pwdchecker.ForeColor = Color.Red;
                            }
                        }
                        reader.Close();
                        con.Close();
                        if (up == 1)
                        {

                            lbl_pwdchecker.Text = "Status:" + status;
                            if (tb_pwd.Text != tb_Cpwd.Text)
                            {
                                lbl_pwdchecker.Text += "Passwords are not the same." + "<br/>";
                                lbl_pwdchecker.ForeColor = Color.Red;
                            }
                            else
                            {
                                if (scores < 4)
                                {
                                    lbl_pwdchecker.ForeColor = Color.Green;
                                    return;
                                }
                                else
                                {

                                    con.Open();
                                    str = "update Account set PasswordHash='" + finalHash+",PasswordSalt='"+salt+",JustHash='"+ justHash+"where Email='" + Session["LoggedIn"].ToString() + "'";
                                    com = new SqlCommand(str, con);
                                    com.Parameters.Add(new SqlParameter("@PasswordHash", finalHash));
                                    com.Parameters.Add(new SqlParameter("@PasswordSalt", salt));
                                    com.Parameters.Add(new SqlParameter("@JustHash", justHash));
                                    com.ExecuteNonQuery();
                                    con.Close();
                                    lbl_pwdchecker.Text += "Password changed Successfully" + "<br/>";
                                    lbl_pwdchecker.ForeColor = Color.Green;
                                    Response.Redirect("HomePage.aspx", false);

                                }
                            }

                        }

                        }
                        else
                        {
                            lblMessage.Text = "UserName does not exist.";
                            lblMessage.ForeColor = Color.Red;
                        }
                    }
                connection.Close();
            }

            
            
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

    }
}