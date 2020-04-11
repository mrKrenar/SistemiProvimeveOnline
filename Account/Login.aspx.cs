using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using SistemiProvimeveOnline.Models;

namespace SistemiProvimeveOnline.Account
{
    public partial class Login : Page
    {

        SqlConnection connection;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
            Email.Focus();
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        LoginMydb();
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
                                                        Request.QueryString["ReturnUrl"],
                                                        RememberMe.Checked),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "Invalid login attempt";
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }

        void LoginMydb()
        {
            //request from database user with ID: Email
            DataSet ds = new DataSet();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CSMainDb"].ConnectionString;
                connection = new SqlConnection(connectionString);

                connection.Open();

                SqlCommand cmd = new SqlCommand("sp_SelectStudent", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@email", Email.Text));
                sqlDA.SelectCommand = cmd;
                sqlDA.Fill(ds, "student");

                Session.Add("email", ds.Tables[0].DefaultView[0]["email"].ToString());
                System.Diagnostics.Debug.WriteLine("Active session: "+Session["email"].ToString());
            }
            catch { }
            finally
            {
                connection.Close();
            }



            //start session
            //in session save email
        }

    }
}