using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace DoctorDiaryAPI
{
    public class Notification
    {
        const string serverKey = "AAAAsbxu4e8:APA91bF32lRjXdF7plRalcbFzMeWbicn4j9oevJmSo-1nscYgXZ4aBlATRwPNrfb9Ow_hmUjLtXsabHk0IgglW3WKzQmX5s70Jku_UjTDuEAx4L1Nkl9NUK5XAdZyxgjCL6e1h6v0aNe";
        const string senderId = "763370594799 ";
        public static string SendPushNotification(string deviceId, string msgBody, string msgTitle)
        {
            string response;
            try
            {                
                //string photo = Reference.AdminLink + "/format/HeaderImages/logo.png";
                // topic notification
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //       tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = msgBody,
                        title = msgTitle,
                        sound = "Enabled"
                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                FCMResponse hresponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(sResponseFromServer);
                                if (hresponse.success == 1)
                                {
                                    response = "" + 200;
                                }
                                else
                                {
                                    response = sResponseFromServer;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        public class FCMResponse
        {
            public long multicast_id { get; set; }
            public int success { get; set; }
            public int failure { get; set; }
            public int canonical_ids { get; set; }
        }
    }
}