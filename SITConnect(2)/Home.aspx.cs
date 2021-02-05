using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect_2_
{
    public partial class _Home : Page
    {
        string MYDBConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            string email = Session["LoggedIn"].ToString();
            if (PwdAgeTooOld(email))
            {
                BannerPwdExpired.Visible = true;
            }
           
        }
       protected void LogoutMe(object sender, EventArgs e)
       {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Request.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Request.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Request.Cookies["AuthToken"].Value = string.Empty;
                Request.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }

            Response.Redirect("Login.aspx", false);
        }

        public bool PwdAgeTooOld(string email)
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
                            if (interval.TotalMinutes > 15)
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
    }
}

        
