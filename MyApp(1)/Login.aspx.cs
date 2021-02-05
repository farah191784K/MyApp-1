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

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Configuration;
using System.Drawing;

namespace MyApp_1_
{
    public partial class Login1 : System.Web.UI.Page
    {
        int attempts;

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;


        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void LoginMe(object sender, EventArgs e)
        {
            if (tb_userid.Text == "" || tb_pwd.Text == "")
            {
                lblMessage.Text = "Fields are required.";
            }

            string password = tb_pwd.Text.ToString().Trim();

            SHA512Managed hash = new SHA512Managed();

            byte[] plainHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            finalHash = Convert.ToBase64String(plainHash);

            lblMessage.Visible = true;

            attempts = Convert.ToInt32(ViewState["attempts"]);
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select Email,attemptcount from Account where Email=@Email", con);
                cmd.Parameters.AddWithValue("@Email", tb_userid.Text);
                cmd.Parameters.AddWithValue("@JustHash", finalHash);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ValidateCaptcha())
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            attempts = Convert.ToInt32(ds.Tables[0].Rows[0]["attemptcount"]);
                            if (attempts == 3)
                            {
                                lblMessage.Text = "Your account has already been locked.";
                                lblMessage.ForeColor = Color.Red;
                            }
                            else
                            {
                                cmd = new SqlCommand("select Email,attemptcount from Account where Email=@Email and JustHash=@JustHash", con);
                                cmd.Parameters.AddWithValue("@Email", tb_userid.Text);
                                cmd.Parameters.AddWithValue("@JustHash", finalHash);
                                da = new SqlDataAdapter(cmd);
                                da.Fill(ds1);


                                if (ds1 != null)
                                {
                                    if (ds1.Tables[0].Rows.Count > 0)
                                    {
                                        ViewState["attempts"] = ds1.Tables[0].Rows[0]["attemptcount"];
                                        if (Convert.ToInt32(ViewState["attempts"]) != 3)
                                        {
                                            Session["LoggedIn"] = tb_userid.Text.Trim();

                                            //Create a new GUID and save into the session
                                            string guid = Guid.NewGuid().ToString();
                                            Session["AuthToken"] = guid;

                                            //Now create a new cookie with this guid value
                                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                            cmd = new SqlCommand("update Account set attemptcount=0 where Email=@Email and JustHash=@JustHash", con);
                                            cmd.Parameters.AddWithValue("@Email", tb_userid.Text);
                                            cmd.Parameters.AddWithValue("@JustHash", finalHash);
                                            cmd.ExecuteNonQuery();
                                            lblMessage.Text = "Login Successful!";
                                            lblMessage.ForeColor = Color.Green;
                                            Response.Redirect("HomePage.aspx");
                                        }
                                        else
                                        {
                                            lblMessage.Text = "Your account has already been locked...Please contact the administrator.";
                                            lblMessage.ForeColor = Color.Red;
                                        }
                                    }
                                    else
                                    {
                                        string strquery = string.Empty;
                                        if (attempts > 2)
                                        {
                                            strquery = "update Account set islocked=1, attemptcount=@attempts where Email=@Email and JustHash=@JustHash";
                                            lblMessage.Text = "You have reached the no.of maximum attempts. Your account has been locked.";
                                        }
                                        else
                                        {
                                            attempts = attempts + 1;
                                            ViewState["attempts"] = attempts;
                                            strquery = "update Account set isLocked=1,attemptcount=@attempts where Email=@Email";
                                            if (attempts == 3)
                                            {
                                                lblMessage.Text = "Your account is locked";
                                            }
                                            else
                                                lblMessage.Text = "Your password is wrong, you have only " + (3 - attempts) + " attempts left.";
                                        }
                                        cmd = new SqlCommand(strquery, con);
                                        cmd.Parameters.AddWithValue("@Email", tb_userid.Text);
                                        cmd.Parameters.AddWithValue("@JustHash", finalHash);
                                        cmd.Parameters.AddWithValue("@attempts", attempts);
                                        cmd.ExecuteNonQuery();
                                        lblMessage.ForeColor = Color.Red;
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
                con.Close();
            }
        }
            protected string getDBSalt(string userid)
            {

                string s = null;

                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", userid);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["PASSWORDSALT"] != null)
                            {
                                if (reader["PASSWORDSALT"] != DBNull.Value)
                                {
                                    s = reader["PASSWORDSALT"].ToString();
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { connection.Close(); }
                return s;

            }

            protected string getDBHash(string userid)
            {

                string h = null;

                SqlConnection connection = new SqlConnection(MYDBConnectionString);
                string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", userid);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            if (reader["PasswordHash"] != null)
                            {
                                if (reader["PasswordHash"] != DBNull.Value)
                                {
                                    h = reader["PasswordHash"].ToString();
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { connection.Close(); }
                return h;
            }
        

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter.
            //captchaResponse consist of the user click pattern. Behavious analytics AI
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(" https://www.google.com/recaptcha/api/siteverify?secret=6LdmHiQaAAAAAHF4Ueke3RfELIwdwO4shywtHxrE &response=" + captchaResponse);

            try
            {
                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //the response is in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //to show the JSON response string for learning purposes
                        //lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //create jsonObject to handle the response e.g. success or error
                        //deserializer Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }

        }

        protected void Reset(object sender, EventArgs e)
        {
            Response.Redirect("ChangePwd.aspx");
        }

        protected void Register(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx");
        }
    }
}