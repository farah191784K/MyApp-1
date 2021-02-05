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
using System.IO;

namespace MyApp_1_
{
    public partial class HomePage : System.Web.UI.Page
    {
        //string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        //byte[] Key;
        //byte[] IV;
        //byte[] ccInfo = null;
        //string userID = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (Session["UserID"] != null)
            //{
            //    userID = (string)Session["userID"];

            //    displayUserProfile(userID);
            //}

            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    lblMessage.Text = "Congratulations!, you are logged in.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    btnLogout.Visible = true;
                }

            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }

        }


        protected void LogoutMe(object sender, EventArgs e)
        {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();

                Response.Redirect("Login.aspx", false);

                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                }

                if (Request.Cookies["AuthToken"] != null)
                {
                    Response.Cookies["AuthToken"].Value = string.Empty;
                    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                }

            }

        protected void btn_change_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePwd.aspx", false);
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Response.Redirect("display.aspx?Comment=" + HttpUtility.UrlEncode(tb_comments.Text));
        }

        //protected string decryptData(byte[] cipherText)
        //{
        //    string plainText = null;

        //    try
        //    {
        //        RijndaelManaged cipher = new RijndaelManaged();
        //        cipher.IV = IV;
        //        cipher.Key = Key;

        //        Create a decrytor to perform the stream transform.
        //        ICryptoTransform decryptTransform = cipher.CreateDecryptor();

        //        Create the streams used for decryption
        //        using (MemoryStream msDecrypt = new MemoryStream(cipherText))
        //            {
        //                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
        //                {
        //                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //                    {
        //                        Read the decrypted bytes from the decrypting stream
        //                    and place them in a string
        //                    plainText = srDecrypt.ReadToEnd();
        //                    }
        //                }
        //            }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //    finally { }
        //    return plainText;
        //}

        //protected void displayUserProfile(string userid)
        //{
        //    SqlConnection connection = new SqlConnection(MYDBConnectionString);
        //    string sql = "SELECT * FROM Account WHERE Email=@userId";
        //    SqlCommand command = new SqlCommand(sql, connection);
        //    command.Parameters.AddWithValue("@userId", userid);

        //    try
        //    {
        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                if (reader["Email"] != DBNull.Value)
        //                {
        //                    lbl_userID.Text = reader["Email"].ToString());
        //                }

        //                if (reader["CreditCardInfo"] != DBNull.Value)
        //                {
        //                    convert base64 in db to byte[]
        //                    ccInfo = Convert.FromBase64String(reader["CreditCardInfo"].ToString());
        //                }

        //                if (reader["IV"] != DBNull.Value)
        //                {
        //                    IV = Convert.FromBase64String(reader["IV"].ToString());
        //                }

        //                if (reader["Key"] != DBNull.Value)
        //                {
        //                    IV = Convert.FromBase64String(reader["Key"].ToString());
        //                }
        //            }
        //            lbl_ccInfo.Text = decryptData(ccInfo);
        //        }
        //    }//try
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

    }
}