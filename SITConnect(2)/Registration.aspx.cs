using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Globalization;
using System;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Drawing;

namespace SITConnect_2_
{
    public partial class Registration : System.Web.UI.Page
    {

        string MYDBConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lbl_fname.Text = "";
            lbl_lname.Text = "";
            lbl_lname.Text = "";
            lbl_ccno.Text = "";
            lbl_cvv.Text = "";
            lbl_ccexp.Text = "";
            lbl_email.Text = "";
            lbl_pwd.Text = "";
            lbl_dob.Text = "";
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool Validated = Validation();
            if (Validated)
            {
                PwdHash();
                ValidateCaptcha();
                createAccount();
                Response.Redirect("Login.aspx");
            }
           
        }
        //Settle - Call function Validation() before calling PwdHash(), then Call CreateAccount()
        public bool Validation()
        {
            //if (tb_fname.Text == "" || tb_lname.Text == "" || tb_ccno.Text == "" || tb_cvv.Text == "" || tb_exp.Text == "" ||
            //    tb_email.Text == "" || tb_pwd.Text == "" || tb_dob.Text == "")
            //{
            //    lblMsg.Text += "Not all inputs are filled, please double-check before resubmitting! </br>";
            //}
            if (tb_fname.Text == "")
            {
                lbl_fname.Text += "First Name is required. <br/>";
            }
            if (tb_lname.Text == "")
            {
                lbl_lname.Text += "Last Name is required. <br/>";
            }
            if (tb_ccno.Text == "")
            {
                lbl_ccno.Text += "Credit Card No. is required. <br/>";
            }
            if (tb_cvv.Text == "")
            {
                lbl_cvv.Text += "Credit Card CVV is required. <br/>";
            }
            if (tb_exp.Text == "")
            {
                lbl_ccexp.Text += "Credit Card Expiry is required. <br/>";
            }
            if (tb_email.Text == "")
            {
                lbl_email.Text += "Email is required. <br/>";
            }
            if (tb_pwd.Text == "")
            {
                lbl_pwd.Text += "Password is required. <br/>";
            }
            if (tb_dob.Text == "")
            {
                lbl_dob.Text += "Date of Birth is required. <br/>";
            }
            if (!ValidateNames(tb_fname.Text))
            {
                lblMsg.Text = "First name is not valid, please re-enter! </br>";
            }
            if (!ValidateNames(tb_lname.Text))
            {
                lblMsg.Text += "Last name is not valid, please re-enter! </br>";
            }
            if (!ValidateCCNo(tb_ccno.Text))
            {
                lblMsg.Text += "Credit Card No. is invalid,please check and re-enter if necessary! </br>";
            }
            if (!ValidateCVV(tb_cvv.Text))
            {
                lblMsg.Text += "Credit Card's CVV Code is invalid,please check and re-enter if necessary! </br>";
            }
            if (!ValidateCCExp(tb_exp.Text))
            {
                lblMsg.Text += "Credit Card's Expiry is invalid!Please re-select! </br>";
            }
            if (!ValidateEmail(tb_email.Text))
            {
                lblMsg.Text += "Email is invalid!Please check and re-enter if necessary! </br>";
            }
            if (ExistingEmailCheck(tb_email.Text))
            {
                lblMsg.Text += "Email already taken,please enter an alternative email! </br>";
            }
            if (!PwdValidation(tb_pwd.Text))
            {
                lblMsg.Text += "Password does not follow requirements,please check and re-enter following requirements! </br>";
            }
            if (!ValidateDOB(tb_dob.Text))
            {
                lblMsg.Text += "Date of Birth is not valid,please check and re-enter if necessary. </br>";
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
                
                //string guid = Guid.NewGuid().ToString();
                //Session["AuthToken"] = guid;

                //Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                /// <summary>
                ///  Add Redirect here
                /// </summary>
        //public bool ValidatePwd(string pwd)
        //{
        //    if (string.IsNullOrEmpty(pwd))
        //        return false;
        //    else if (pwd.Length <8)
        //        return false;
        //    else
        //    {
        //        try
        //       {
        //            return Regex.IsMatch(pwd,
        //                @"^\d{5}$");
        //        }
        //        catch (RegexMatchTimeoutException)
        //        {
        //            return false;
        //        }
        //    }
        //}

        //settle,follows practical
        public void PwdHash()
        {
            string pwd = tb_pwd.Text.ToString().Trim(); ;
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
     
        
        //Settle
        public bool PwdValidation(string pwd)
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
        //settle
        public bool ValidateCCNo(string ccno)
        {
            if (string.IsNullOrEmpty(ccno))
                return false;
            else if (ccno.Length > 16)
                return false;
            else
            {
                try
                {
                    return Regex.IsMatch(ccno,
                        @"^\d{16}$");
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }


        }
        //settle
        public bool ValidateCVV(string cvv)
        {
            if (string.IsNullOrEmpty(cvv))
                return false;
            else if (cvv.Length > 3)
                return false;
            else
            {
                try
                {
                    return Regex.IsMatch(cvv,
                        @"^\d{3}$");
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }


        }
        //settle
        public bool ValidateNames(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            try
            {
                return Regex.IsMatch(name,
                    @"^[A-Za-z.\s_-]+$",
                    RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }


        }
        public bool ExistingEmailCheck(string email)
        {
            string em = null;
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            string sqlstmt = "Select Email from Accounts where email = @Email";
            SqlCommand cmd = new SqlCommand(sqlstmt, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Email"] != null)
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
                            em = reader["Email"].ToString();
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
            if (em != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //settle
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                        RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        //settle
        public bool ValidateDOB(string dob)
        {
            bool result;
            result = DateTime.TryParse(dob, out _);
            if (!result)
            {
                return false;
            }
            DateTime DOB = Convert.ToDateTime(dob);
            DateTime now = DateTime.Today;
            if (now <= DOB)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
        //settle
        public bool ValidateCCExp(string exp)
        {
            bool result;
            result = DateTime.TryParse(exp, out _);
            if (!result)
            {
                return false;
            }
            DateTime Exp = Convert.ToDateTime(exp);
            DateTime now = DateTime.Today;
            if (Exp <= now)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        //Call captcha validation before Create Account 
        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-captcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Ld7JEUaAAAAAIjQIFeTtCAD7tAmRZ1VUd1STz41 &response" + captchaResponse);


            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

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
            
        //settle
        public void createAccount()
        {
            try
            {
                SqlConnection conn = new SqlConnection(MYDBConnectionString);
                string sqlStmt = "INSERT INTO Accounts (fname, lname,email,dob,passwordHash,passwordSalt,CreditCard,CVV,CCExp,[key],IV,PasswordAge)" 
                + "VALUES(@fname,@lname,@email,@dob, @PasswordHash, @PasswordSalt, @CreditCard, @CVV, @CCExp, @Key, @IV, @PasswordAge)";
                SqlCommand cmd = new SqlCommand(sqlStmt, conn);

               
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@fname", tb_fname.Text.Trim());
                cmd.Parameters.AddWithValue("@lname", tb_lname.Text.Trim());
                cmd.Parameters.AddWithValue("@email", tb_email.Text);
                cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(tb_dob.Text));
                cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                cmd.Parameters.AddWithValue("@CreditCard", Convert.ToBase64String(encryptData(tb_ccno.Text)));
                cmd.Parameters.AddWithValue("@CVV", Convert.ToBase64String(encryptData(tb_cvv.Text)));
                cmd.Parameters.AddWithValue("@CCExp", Convert.ToDateTime(tb_exp.Text));
                cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                cmd.Parameters.AddWithValue("@PasswordAge", DateTime.Now);


                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
            
        //settle
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

        
    }
}
