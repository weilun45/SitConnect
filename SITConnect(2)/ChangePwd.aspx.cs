using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect_2_
{


    public partial class ChangePwd : System.Web.UI.Page
    {
        string MYDBConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            lblMsg.Text = "";
            lbl_Success.Text = "";
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {

            bool AllOk = Validated();
            if (AllOk)
            {
                PwdHash();
                PasswordUpdate();
                lbl_Success.Text += "Password changed!";
                Response.Redirect("Home.aspx");
            }
            
        }

        public bool Validated()
        {
            string email = Session["LoggedIn"].ToString();
            if (!PasswordValidation(email,tb_CurrentPwd.Text))
            {
                lblMsg.Text += "Password is incorrectly entered! Try again. </br>";
            }
            if (!PwdCheck(tb_NewPwd.Text))
            {
                lblMsg.Text += "Password is invalid,please try again and follow the requirements! </br>";
            }
            if (PassAgeTooYoung(email))
            {
                lblMsg.Text += "Password was just changed a moment ago. Please change password at a later time. </br>";
            }
            if (tb_CurrentPwd == tb_NewPwd)
            {
                lblMsg.Text += "New password is the same as Current password, please re-enter a new password. </br>";
            }
            if (lblMsg.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PwdHash()
        {
            string pwd = tb_NewPwd.Text.ToString().Trim(); ;
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
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
        }

        public bool PasswordValidation(string email, string pass)
        {
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
            {
                string pwdWithSalt = pass + dbSalt;
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                string userHash = Convert.ToBase64String(hashWithSalt);
                if (userHash.Equals(dbHash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            else
            {
                return false;
            }
        }

        protected string getDBHash(string email)
        {
            string l = null;
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            string sqlstmt = "Select PasswordHash from Accounts where email = @Email";
            SqlCommand cmd = new SqlCommand(sqlstmt, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["PasswordHash"] != null)
                    {
                        if (reader["PasswordHash"] != DBNull.Value)
                        {
                            l = reader["PasswordHash"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return l;
        }

        protected string getDBSalt(string email)
        {
            string k = null;
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            string sqlstmt = "Select PasswordSalt from Accounts where email = @Email";
            SqlCommand cmd = new SqlCommand(sqlstmt, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["PasswordSalt"] != null)
                    {
                        if (reader["PasswordSalt"] != DBNull.Value)
                        {
                            k = reader["PasswordSalt"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            return k;
        }

        public bool PwdCheck(string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
                return false;

            try
            {
                return Regex.IsMatch(pwd, @"(?-i)(?=^.{8,}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))(?=(1)(?=.*\d)|.*[^A-Za-z0-9])^.*$", RegexOptions.None);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public bool PassAgeTooYoung(string email)
        {
            DateTime pwdAge;
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            string sqlStmt = "SELECT PasswordAge from Accounts where email = @Email";
            SqlCommand cmd = new SqlCommand(sqlStmt, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordAge"] != DBNull.Value)
                        {
                            pwdAge = Convert.ToDateTime(reader["PasswordAge"]);

                            DateTime now = DateTime.Now;
                            TimeSpan interval = now - pwdAge;
                            if (interval.TotalMinutes < 5)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else { return false; }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return false;
        }
        public void PasswordUpdate()
        {
            try
            {
                SqlConnection con = new SqlConnection(MYDBConnectionString);
                string sqlStmt = "UPDATE Accounts SET passwordHash = @PasswordHash, passwordSalt = @PasswordSalt, [key] = @Key, IV = @IV " +
                    "WHERE email = @Email";
                SqlCommand cmd = new SqlCommand(sqlStmt, con);

                cmd.Parameters.AddWithValue("@Email", Session["LoggedIn"].ToString());
                cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}