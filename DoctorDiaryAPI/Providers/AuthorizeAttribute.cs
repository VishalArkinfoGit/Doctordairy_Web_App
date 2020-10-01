using DoctorDiaryAPI.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace DoctorDiaryAPI.Providers
{
    public class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //if (!HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    base.HandleUnauthorizedRequest(actionContext);
            //}
            //else
            //{
            //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);

            var content = new ReturnObject()
            {
                message = "Oops something went wrong! ",
                status_code = Convert.ToInt32(Status.Failed)
            };

            //actionContext.Response = new HttpResponseMessage
            //{

            //    StatusCode = HttpStatusCode.Unauthorized,
            //    Content = new StringContent(JsonConvert.SerializeObject(content))

            //};




            if (HttpContext.Current.User == null || HttpContext.Current.User.Identity == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(JsonConvert.SerializeObject(content))
                };
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(JsonConvert.SerializeObject(content))
                };
            }
        }
    }
}