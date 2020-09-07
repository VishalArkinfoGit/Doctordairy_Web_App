using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace DoctorDiaryAPI
{
    public class SmsSend
    {
        public static bool Send(string MobileNo, string Message)
        {
            try
            {

                string smsstr = "http://msg.msgclub.net/rest/services/sendSMS/sendGroupSms?AUTH_KEY=bf61de676eda2e011125d528133625f&message=" + Message + "&senderId=DOCDIR&routeId=1&mobileNos=" + MobileNo + "&smsContentType=english";
                //string smsstr = "http://sms.myepicsoft.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=d2ba29c1a3d8e7ca4e4dfffd5e24070&message=" + Message + "&senderId=ECOUNT&routeId=1&mobileNos=" + MobileNo + "&smsContentType=english";
                HttpWebRequest _createRequest = (HttpWebRequest)WebRequest.Create(smsstr);
                //getting response of sms
                HttpWebResponse myResp = (HttpWebResponse)_createRequest.GetResponse();
                System.IO.StreamReader _responseStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());

                string responseString = _responseStreamReader.ReadToEnd();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                System.Web.Script.Serialization.JavaScriptSerializer js = new JavaScriptSerializer();
                SMSResponse response = (SMSResponse)js.Deserialize(responseString, typeof(SMSResponse));

                _responseStreamReader.Close();
                myResp.Close();
                if (response.responseCode == "3001")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                return false;
            }
        }
    }

    public class SMSResponse
    {
        public string responseCode { get; set; }
        public string response { get; set; }
    }
}