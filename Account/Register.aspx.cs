using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using SistemiProvimeveOnline.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace SistemiProvimeveOnline.Account
{
    public partial class Register : Page
    {
        protected SqlConnection connection;

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");
                RegisterToDb();
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
                return;
            }

            
        }

        void RegisterToDb()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CSMainDb"].ConnectionString;
                connection = new SqlConnection(connectionString);

                connection.Open();

                SqlCommand cmd = new SqlCommand("sp_RegisterStudent", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@email", Email.Text));
                cmd.Parameters.Add(new SqlParameter("@emri", Emri.Text));
                cmd.Parameters.Add(new SqlParameter("@mbiemri", Mbiemri.Text));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally
            {
                connection.Close();
            }
        }
    }
}