using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DoctorDiaryAPI.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            using (var db = new ddiarydbEntities())
            {
                var user = new usr();

                if (Regex.IsMatch(context.UserName, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", RegexOptions.IgnoreCase))
                {
                    user = db.usrs.FirstOrDefault(x => x.Email == context.UserName);
                }
                else if (Regex.IsMatch(context.UserName, @"(\d*-)?\d{10}", RegexOptions.IgnoreCase))
                {
                    user = (from x in db.Doctor_Master
                            join u in db.usrs on x.User_id equals u.Id
                            where x.Doctor_contact == context.UserName
                            select u).FirstOrDefault();
                }
                else
                {
                    user = null;
                }

                if (user != null && user.passwd == context.Password)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                    identity.AddClaim(new Claim("username", user.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Firstname + " " + user.Lastname));
                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }
            }
        }
    }
}