using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoctorDiaryAPI.Models;
using DoctorDiaryAPI.Providers;
using DoctorDiaryAPI.Results;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using DoctorDiaryAPI;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using AutoMapper;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Web.Util;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace DoctorDiaryAPI.Controllers
{
    [System.Web.Http.Authorize]
    [RoutePrefix("api/Doctor")]
    public class DoctorController : ApiController
    {
        ddiarydbEntities db = new ddiarydbEntities();
        Query query = new Query();
        MapperConfiguration Config;
        IMapper mapper;
        //Email mail = new Email();

        [HttpGet]
        //[Route("api/Doctor/Get_ALL_Patient")]
        [ActionName("Get_ALL_Patient")]
        public ReturnObject Get_ALL_Patient(string User_Id)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                int docid = int.Parse(User_Id);

                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                returnData.data1 = db.Patient_Master.Where(x => x.User_Id == docid).Select(x => new { x.Patient_Id, x.Patient_name }).ToList();
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Check_ExistingUserInDoctor")]
        public ReturnObject Check_ExistingUserInDoctor(string User_Id)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                int docid = int.Parse(User_Id);

                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                //var docmaster = db.Doctor_Master.Select(x => new { x.User_id, x.Clinic_name, x.Doctor_address, x.Doctor_city, x.Doctor_contact, x.Doctor_email, x.Doctor_country, x.Doctor_exp, x.Doctor_id, x.Doctor_name, x.Doctor_photo, x.Doctor_state, x.Gender, x.Reg_date, x.IsActive }).FirstOrDefault(x => x.Doctor_email == Email);

                returnData.data1 = db.Doctor_Master.Where(x => x.User_id == docid).Select(x => new { x.Doctor_id }).ToList();
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Delete_Patient")]
        public ReturnObject Delete_Patient(string Patient_ID)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                string[] datas = Patient_ID.Split(',');
                foreach (var item in datas)
                {
                    int rowsAffected2 = query.delete("Treatment_Master", "WHERE Patient_Id =" + item);

                    if (rowsAffected2 > 0)
                    {
                        int rows1 = query.delete("account_table", "where patient_id =" + item);
                        if (rows1 > 0)
                        {
                            int row2 = query.delete("prescription_table", "where Patient_Id =" + item);
                        }
                    }
                    query.delete("Patient_Master", "WHERE Patient_id =" + item);
                }
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Delete_Treat")]
        public ReturnObject Delete_Treat(string Treat_crno)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                string[] datas = Treat_crno.Split(',');
                foreach (var item in datas)
                {
                    int rowsAffected = query.delete("Treatment_Master", " WHERE Treat_crno =" + item);
                    if (rowsAffected > 0)
                    {
                        int row2 = query.delete("prescription_table", "where Treat_crno =" + item);
                        query.delete("account_table", " where Treat_crno =" + item);
                    }
                    else
                    {
                        returnData.message = "No Record Found..";
                        returnData.status_code = Convert.ToInt32(Status.NotFound);
                        //return returnMethodString("No Record Found..", "Failure", 0, "Delete_Treat");
                    }

                }
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Delete_prescription")]
        public ReturnObject Delete_prescription(string prescription_id)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {

                string[] datas = prescription_id.Split(',');
                foreach (var item in datas)
                {
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    int rowsAffected = query.delete("prescription_table", "WHERE prescription_id =" + item);
                }
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Forgot_Password")]
        public ReturnObject Forgot_Password(string email)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                usr user = db.usrs.Where(x => x.Email == email.Trim()).FirstOrDefault();
                if (user != null)
                {
                    string body = "Dear user," + Environment.NewLine + " Thank you for using our app." + Environment.NewLine + "Your password is:" + user.passwd;
                    Email.MailSend(user.Email, "Doctor Diary App Forgot Password", body, "", "");

                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                }
                else
                {
                    returnData.message = "User Record Not Found";
                    returnData.status_code = Convert.ToInt32(Status.Failed);
                }

                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }


        [HttpGet]
        [ActionName("GetPatientDataCount")]
        public ReturnObject GetPatientDataCount(string User_Id, string DateFrom, string DateTo)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                List<PatientCounts> patCnts = new List<PatientCounts>();
                string[] date = DateTo.Split('/');
                int docid = int.Parse(User_Id);
                string follow = "" + date[2] + "-" + date[0] + "-" + date[1];

                DateTime dt = Convert.ToDateTime(follow);
                //BeginCollectionWise
                // var accnt = db.account_table.GroupBy(a => a.ArticleName).Select(x=>new { x.sum}).Where(x => x.Doctor_id == int.Parse(User_Id) && x.date.Value.Month == dt.Month && x.date.Value.Year == dt.Year).ToList();
                var accnt = db.account_table
                                .Where(x => x.Doctor_id == docid && x.date.Value.Month == dt.Month && x.date.Value.Year == dt.Year && x.transaction_type == "Credit")
                                .GroupBy(a => a.date)
                                .Select(a => new { Amount = a.Sum(b => b.amount), treatdate = a.Key.ToString() })
                                .OrderBy(a => a.treatdate)
                                .ToList();
                returnData.data1 = accnt;
                //Begin FollowPatients
                var FollowUpPatientCnt = db.Treatment_Master.Count(x => x.Treat_date == dt && x.User_Id == docid);
                patCnts.Add(new Controllers.PatientCounts { type = "followupPatients", counts = FollowUpPatientCnt.ToString() });

                //Normal Patient Count
                var NormalPatientCnt = db.Treatment_Master.Count(x => x.Treat_date.Value.Month == dt.Month && x.Treat_date.Value.Year == dt.Year && x.User_Id == docid && x.Status == "Normal");
                patCnts.Add(new Controllers.PatientCounts { type = "NormalMontlyPatients", counts = NormalPatientCnt.ToString() });

                //Critical Patient Count
                var CriticalPatientCnt = db.Treatment_Master.Count(x => x.Treat_date.Value.Month == dt.Month && x.Treat_date.Value.Year == dt.Year && x.User_Id == docid && x.Status == "Critical");
                patCnts.Add(new Controllers.PatientCounts { type = "CriticalMontlyPatients", counts = CriticalPatientCnt.ToString() });

                returnData.data2 = patCnts;


                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }


        }

        [HttpGet]
        [ActionName("Get_Account")]
        public ReturnObject Get_Account(int patient_id)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                returnData.data1 = db.account_table.Where(x => x.patient_id == patient_id).ToList();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_Daily_Report")]
        public ReturnObject Get_Daily_Report(string User_Id, string Type, string FrmDate, string ToDate)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                string[] date1 = FrmDate.Split('/');
                string[] date2 = null;
                if (ToDate == "" || ToDate == null)
                {
                    date2 = FrmDate.Split('/');
                }
                else
                {
                    date2 = ToDate.Split('/');
                }
                int DocId = int.Parse(User_Id);
                string ReportDate = "" + date1[2] + "-" + date1[0] + "-" + date1[1];
                string RtToDate = "" + date2[2] + "-" + date2[0] + "-" + date2[1];
                DateTime dtFromReport = Convert.ToDateTime(ReportDate);
                DateTime dtToReport = Convert.ToDateTime(RtToDate);
                if (Type == "Patient Wise" || Type == "Collection Wise")
                {
                    List<Patient_Master> ptList = db.Patient_Master.ToList();
                    List<Treatment_Master> treatList = db.Treatment_Master.ToList();
                    var record = from p in ptList
                                 join t in treatList on p.Patient_Id equals t.Patient_Id
                                 where (t.User_Id == DocId && t.Treat_date >= dtFromReport && t.Treat_date <= dtToReport)
                                 select new
                                 {
                                     t.Patient_Master.Patient_name,
                                     t.Patient_BP,
                                     t.Patient_complain,
                                     t.Treat_date,
                                     t.Treat_crno,
                                     t.SPO_two,
                                     t.Remarks,
                                     t.Patient_temp,
                                     t.Patient_pulse,
                                     t.Patient_image,
                                     t.Patient_Id,
                                     t.Medicine,
                                     t.Counsultancyfee,
                                     t.Advice,
                                     t.Status,
                                     t.Referred_Doctor_Name,
                                     t.Referred_Hospital_Detail,
                                     t.Patient_Report_Detail,
                                     t.User_Id,
                                     t.Is_Paid,
                                     t.Follow_Date,
                                     t.paid_Amount,
                                     t.bill_Amount,
                                     t.symptoms_id
                                 };
                    returnData.data1 = record;
                }
                else if (Type == "Referred Patients")
                {
                    List<Patient_Master> ptList = db.Patient_Master.ToList();
                    List<Treatment_Master> treatList = db.Treatment_Master.ToList();
                    var record = from p in ptList
                                 join t in treatList on p.Patient_Id equals t.Patient_Id
                                 where (t.User_Id == DocId && t.Treat_date >= dtFromReport && t.Treat_date <= dtToReport && t.Referred_Doctor_Name != "null" && t.Referred_Doctor_Name != "")
                                 select new
                                 {
                                     t.Patient_Master.Patient_name,
                                     t.Patient_BP,
                                     t.Patient_complain,
                                     t.Treat_date,
                                     t.Treat_crno,
                                     t.SPO_two,
                                     t.Remarks,
                                     t.Patient_temp,
                                     t.Patient_pulse,
                                     t.Patient_image,
                                     t.Patient_Id,
                                     t.Medicine,
                                     t.Counsultancyfee,
                                     t.Advice,
                                     t.Status,
                                     t.Referred_Doctor_Name,
                                     t.Referred_Hospital_Detail,
                                     t.Patient_Report_Detail,
                                     t.User_Id,
                                     t.Is_Paid,
                                     t.Follow_Date,
                                     t.paid_Amount,
                                     t.bill_Amount,
                                     t.symptoms_id
                                 };
                    returnData.data1 = record;
                }
                else if (Type == "Patient With Report")
                {
                    List<Patient_Master> ptList = db.Patient_Master.ToList();
                    List<Treatment_Master> treatList = db.Treatment_Master.ToList();
                    var record = from p in ptList
                                 join t in treatList on p.Patient_Id equals t.Patient_Id
                                 where (t.User_Id == DocId && t.Treat_date >= dtFromReport && t.Treat_date <= dtToReport && t.Patient_Report_Detail != "null" && t.Patient_Report_Detail != "")
                                 select new
                                 {
                                     t.Patient_Master.Patient_name,
                                     t.Patient_BP,
                                     t.Patient_complain,
                                     t.Treat_date,
                                     t.Treat_crno,
                                     t.SPO_two,
                                     t.Remarks,
                                     t.Patient_temp,
                                     t.Patient_pulse,
                                     t.Patient_image,
                                     t.Patient_Id,
                                     t.Medicine,
                                     t.Counsultancyfee,
                                     t.Advice,
                                     t.Status,
                                     t.Referred_Doctor_Name,
                                     t.Referred_Hospital_Detail,
                                     t.Patient_Report_Detail,
                                     t.User_Id,
                                     t.Is_Paid,
                                     t.Follow_Date,
                                     t.paid_Amount,
                                     t.bill_Amount,
                                     t.symptoms_id
                                 };
                    returnData.data1 = record;
                }

                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_DoctorMaster")]
        public ReturnObject Get_DoctorMaster(string User_Id)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                int docid = int.Parse(User_Id);
                var doctor = db.Doctor_Master.Select(x => new { x.User_id, x.Clinic_name, x.Doctor_address, x.Doctor_city, x.Doctor_contact, x.Doctor_email, x.Doctor_country, x.Doctor_exp, x.Doctor_id, x.Doctor_name, x.Doctor_photo, x.Doctor_state, x.Gender, x.Reg_date, x.IsActive }).FirstOrDefault(x => x.User_id == docid);

                //Doctor_Master doc = db.Doctor_Master.Where(x => x.User_id == docid).FirstOrDefault();
                if (doctor.Doctor_photo == "" || doctor.Doctor_photo == null)
                {
                    // doctor.Doctor_photo = "";
                }
                else
                {
                    returnData.data2 = Getbase64str(System.Web.Hosting.HostingEnvironment.MapPath(doctor.Doctor_photo));
                }
                returnData.data1 = doctor;
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_Patient_FromDashboard")]
        public ReturnObject Get_Patient_FromDashboard(string User_Id, string Name)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                string json = string.Empty;
                DataTable dt = new DataTable();
                List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
                Dictionary<string, object> row1;
                List<List<Dictionary<string, object>>> rowslist = new List<List<Dictionary<string, object>>>();
                DataTable dtresult;
                csSymptomsname data = new csSymptomsname();
                string constr = ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {

                    // using (SqlCommand cmd = new SqlCommand("SELECT patient_name,Patient_email,patient_contact,Reg_Date FROM Treatment_Master where Patient_name='" + Name + "' and User_Id='" + User_Id + "'", con))
                    using (SqlCommand cmd = new SqlCommand("SELECT account_table.account_id, account_table.transaction_type, Treatment_Master.*, Patient_Master.Patient_name FROM account_table,Patient_Master,Treatment_Master where Treatment_Master.Treat_crno=account_table.Treat_crno and Treatment_Master.Patient_Id=Patient_Master.Patient_Id and Treatment_Master.User_Id='" + User_Id + "'and Patient_Master.Patient_name LIKE '%" + Name + "%'", con))
                    {

                        con.Open();
                        SqlDataAdapter datemp = new SqlDataAdapter(cmd);
                        dt.TableName = "Patient_Master";
                        datemp.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            returnData.data1 = "No Record Found";
                            returnData.message = "Successfull";
                            returnData.status_code = Convert.ToInt32(Status.Sucess);
                            return returnData;
                            //return returnMethodString("No Record Found..", "Success", 1, "Get_Patient_FromDashboard");
                        }

                        dtresult = dt.Clone();
                        dtresult.Clear();
                        dtresult.Columns.Add("Acct_details");


                        int count = 0;
                        int crno = 0;
                        string stracct = "";
                        string name = "";
                        foreach (DataRow dr in dt.Rows)
                        {

                            if (crno == Convert.ToInt32(dr["treat_crno"]))
                            {
                                continue;
                            }
                            crno = int.Parse(dr["treat_crno"].ToString());
                            DataRow[] result = dt.Select("treat_crno =" + crno);

                            foreach (DataRow drtemp in result)
                            {

                                if (count == 0)
                                {
                                    if (drtemp["transaction_type"].Equals("Debit"))
                                    {
                                        name = "billAmountId";
                                    }
                                    else
                                    {
                                        name = "paidAmountId";
                                    }

                                    stracct = stracct + name + ":" + drtemp["account_id"].ToString();
                                    count = 1;
                                    continue;
                                }
                                if (drtemp["transaction_type"].Equals("Debit"))
                                {
                                    name = "billAmountId";
                                }
                                else
                                {
                                    name = "paidAmountId";
                                }

                                stracct = stracct + "," + name + ":" + drtemp["account_id"].ToString();

                                dtresult.Rows.Add(drtemp["account_id"], drtemp["transaction_type"], drtemp["Treat_date"], drtemp["Treat_crno"], drtemp["SPO_two"], drtemp["Remarks"], drtemp["Patient_temp"], drtemp["Patient_pulse"], drtemp["Patient_image"], drtemp["Patient_complain"], drtemp["Patient_Id"], drtemp["Patient_BP"], drtemp["Medicine"], drtemp["Counsultancyfee"], drtemp["Advice"], drtemp["Status"], drtemp["Referred_Doctor_Name"], drtemp["Referred_Hospital_Detail"], drtemp["Referred_Hospital_Detail"], drtemp["User_Id"], drtemp["Is_Paid"], drtemp["Follow_Date"], drtemp["paid_Amount"], drtemp["bill_Amount"], drtemp["symptoms_id"], drtemp["Patient_name"], stracct);
                                count = 0;
                                stracct = "";

                                //symptoms

                                int[] zero = { 0 };
                                string[] empty = { "" };

                                string symp_id = drtemp["symptoms_id"].ToString();
                                if (symp_id.Equals(""))
                                {
                                    data.id = zero;
                                    data.treat_crno = Convert.ToInt32(drtemp["Treat_crno"].ToString());
                                    data.name = empty;

                                }
                                else
                                {
                                    string[] sid = symp_id.Split(',');
                                    data.id = Array.ConvertAll(sid, int.Parse);
                                    data.treat_crno = Convert.ToInt32(drtemp["Treat_crno"].ToString());
                                    string strsymp = "select name from symptoms where ";
                                    for (int i = 0; i < sid.Length; i++)
                                    {
                                        if (i == (sid.Length - 1))
                                        {
                                            strsymp += "id='" + sid[i] + "'";
                                        }
                                        else
                                        {
                                            strsymp += "id='" + sid[i] + "' OR ";
                                        }

                                    }
                                    SqlCommand cmdsymptoms = new SqlCommand(strsymp, con);
                                    SqlDataAdapter dapsymptoms = new SqlDataAdapter(cmdsymptoms);

                                    DataTable dtsymptoms = new DataTable();
                                    dtsymptoms.TableName = "symptoms";
                                    dapsymptoms.Fill(dtsymptoms);
                                    List<string> names = new List<string>();
                                    foreach (DataRow drow in dtsymptoms.Rows)
                                    {
                                        string str = drow[0].ToString();
                                        names.Add(str);

                                    }

                                    data.name = names.ToArray();

                                    dapsymptoms.Dispose();
                                    dtsymptoms.Dispose();
                                }
                                row1 = new Dictionary<string, object>();
                                row1.Add("id", data.id);
                                row1.Add("name", data.name);
                                row1.Add("treat_crno", data.treat_crno);
                                rows1.Add(row1);


                            }

                        }



                    }
                }

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;

                foreach (DataRow dr in dtresult.Rows)
                {
                    row = new Dictionary<string, object>();

                    string Title = "responseData";
                    foreach (DataColumn col in dtresult.Columns)
                    {
                        //row.Add(Title,dr[col]);
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                rowslist.Add(rows);
                rowslist.Add(rows1);
                if (dtresult.Rows.Count > 0)
                {
                    returnData.data1 = rowslist;
                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    return returnData;

                    // return returnTreatMaster(rowslist, "Success", 1, "Get_Patient_FromDashboard");
                }
                else
                {
                    returnData.data1 = "No Record Found";
                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    return returnData;

                    //return returnMethodTreatMaster("No Record Found..", "Success", 1, "Get_Patient_FromDashboard");
                }


            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_MonthPatientDataFromDate")]
        public ReturnObject Get_MonthPatientDataFromDate(string User_Id, string DateFrom, string DateTo, string patientCount)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                string json = string.Empty;
                int counter = 1;
                DataTable dt = new DataTable();
                DataTable dtresult = new DataTable();
                DataTable dtpatient = new DataTable();
                csSymptomsname data = new csSymptomsname();
                Dictionary<string, object> row1;
                List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
                List<List<Dictionary<string, object>>> rowslist = new List<List<Dictionary<string, object>>>();
                string constr = ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString;
                //DateTime dateto = Convert.ToDateTime(DateTo);
                //DateTime datefrom = Convert.ToDateTime(DateFrom);
                string[] datett = DateTo.Split('/');
                string[] datefr = DateFrom.Split('/');

                string datet = "" + datett[2] + "-" + datett[0] + "-" + datett[1];
                string datef = "" + datefr[2] + "-" + datefr[0] + "-" + datefr[1];
                using (SqlConnection con = new SqlConnection(constr))
                {

                    if (patientCount == "" || patientCount == null)
                    {
                        //string treat = "SELECT account_table.account_id,account_table.transaction_type,Patient_Master.Patient_name,Treatment_Master.* FROM account_table, Patient_Master,Treatment_Master where account_table.Treat_crno = Treatment_Master.Treat_crno AND Patient_Master.Patient_Id = Treatment_Master.Patient_Id AND  Treatment_Master.User_Id=176 AND CAST(Treatment_Master.Treat_date as date) >='1/8/2017' and CAST(Treatment_Master.Treat_date as date) <='11/8/2017'";
                        string treat = "SELECT account_table.account_id,account_table.transaction_type,Patient_Master.Patient_name,Treatment_Master.* FROM account_table, Patient_Master,Treatment_Master where account_table.Treat_crno = Treatment_Master.Treat_crno AND Patient_Master.Patient_Id = Treatment_Master.Patient_Id AND  Treatment_Master.User_Id=" + User_Id + " AND CAST(Treatment_Master.Treat_date as date) >='" + datef + "' and CAST(Treatment_Master.Treat_date as date) <='" + datet + "'";
                        SqlCommand cmd = new SqlCommand(treat, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        dt.Clear();
                        dt.TableName = "Treatment_Master";
                        da.Fill(dt);
                        if (counter == 1)
                        {
                            counter++;
                            dtresult = dt.Clone();
                            dtresult.Columns.Add("Acct_details");
                        }
                        if (dt.Rows.Count != 0)
                        {

                            int count = 0;

                            int crno = 0;
                            string stracct = "";
                            string name = "";
                            foreach (DataRow dr in dt.Rows)
                            {

                                if (crno == Convert.ToInt32(dr["treat_crno"]))
                                {
                                    continue;
                                }

                                crno = int.Parse(dr["treat_crno"].ToString());
                                DataRow[] result = dt.Select("treat_crno =" + crno);

                                foreach (DataRow drtemp in result)
                                {


                                    if (count == 0)
                                    {
                                        if (drtemp["transaction_type"].Equals("Debit"))
                                        {
                                            name = "billAmountId";
                                        }
                                        else
                                        {
                                            name = "paidAmountId";
                                        }

                                        stracct = stracct + name + ":" + drtemp["account_id"].ToString();
                                        count = 1;
                                        continue;
                                    }
                                    if (drtemp["transaction_type"].Equals("Debit"))
                                    {
                                        name = "billAmountId";
                                    }
                                    else
                                    {
                                        name = "paidAmountId";
                                    }

                                    stracct = stracct + "," + name + ":" + drtemp["account_id"].ToString();

                                    dtresult.Rows.Add(drtemp["account_id"], drtemp["transaction_type"], drtemp["Patient_name"], drtemp["Treat_date"], drtemp["Treat_crno"], drtemp["SPO_two"], drtemp["Remarks"], drtemp["Patient_temp"], drtemp["Patient_pulse"], drtemp["Patient_image"], drtemp["Patient_complain"], drtemp["Patient_Id"], drtemp["Patient_BP"], drtemp["Medicine"], drtemp["Counsultancyfee"], drtemp["Advice"], drtemp["Status"], drtemp["Referred_Doctor_Name"], drtemp["Referred_Hospital_Detail"], drtemp["Patient_Report_Detail"], drtemp["User_Id"], drtemp["Is_Paid"], drtemp["Follow_Date"], drtemp["paid_Amount"], drtemp["bill_Amount"], drtemp["symptoms_id"], stracct);
                                    count = 0;
                                    stracct = "";

                                    //symptoms

                                    int[] zero = { 0 };
                                    string[] empty = { "" };

                                    string symp_id = drtemp["symptoms_id"].ToString();
                                    if (symp_id.Equals(""))
                                    {
                                        data.id = zero;
                                        data.treat_crno = Convert.ToInt32(drtemp["Treat_crno"].ToString());
                                        data.name = empty;

                                    }
                                    else
                                    {
                                        string[] sid = symp_id.Split(',');
                                        data.id = Array.ConvertAll(sid, int.Parse);
                                        data.treat_crno = Convert.ToInt32(drtemp["Treat_crno"].ToString());
                                        string strsymp = "select name from symptoms where ";
                                        for (int i = 0; i < sid.Length; i++)
                                        {
                                            if (i == (sid.Length - 1))
                                            {
                                                strsymp += "id='" + sid[i] + "'";
                                            }
                                            else
                                            {
                                                strsymp += "id='" + sid[i] + "' OR ";
                                            }

                                        }
                                        SqlCommand cmdsymptoms = new SqlCommand(strsymp, con);
                                        SqlDataAdapter dapsymptoms = new SqlDataAdapter(cmdsymptoms);

                                        DataTable dtsymptoms = new DataTable();
                                        dtsymptoms.TableName = "symptoms";
                                        dapsymptoms.Fill(dtsymptoms);
                                        List<string> names = new List<string>();
                                        foreach (DataRow drow in dtsymptoms.Rows)
                                        {
                                            string str = drow[0].ToString();
                                            names.Add(str);

                                        }

                                        data.name = names.ToArray();

                                        dapsymptoms.Dispose();
                                        dtsymptoms.Dispose();
                                    }

                                    row1 = new Dictionary<string, object>();
                                    row1.Add("id", data.id);
                                    row1.Add("name", data.name);
                                    row1.Add("treat_crno", data.treat_crno);
                                    rows1.Add(row1);
                                }

                            }
                        }

                        //Account without treatment
                        string account = "SELECT account_table.*,Patient_Master.Patient_name FROM account_table, Patient_Master where account_table.Doctor_id=" + User_Id + " and CAST(account_table.date as date) >='" + datef + "' and CAST(account_table.date as date) <='" + datet + "' and account_table.patient_id= Patient_Master.Patient_Id and account_table.Treat_crno is null;";
                        SqlCommand cmd_account = new SqlCommand(account, con);
                        SqlDataAdapter da_account = new SqlDataAdapter(cmd_account);
                        DataTable dt_account = new DataTable();
                        dt_account.TableName = "Account_Master";
                        da_account.Fill(dt_account);
                        DataTable dt_acct_clone = new DataTable();
                        dt_acct_clone = dtresult.Clone();
                        int[] id = { 0 };
                        string[] namearr = { " " };

                        foreach (DataRow dr in dt_account.Rows)
                        {
                            dt_acct_clone.Rows.Add(dr["account_id"], dr["transaction_type"], dr["Patient_name"], dr["date"], 0, 0, dr["remarks"], 0, 0, 0, 0, dr["patient_id"], 0, 0, dr["amount"], 0, 0, "", "", "", dr["Doctor_id"], true, DBNull.Value, 0, 0, 0, "billAmountId:0,paidAmountId:0");
                            row1 = new Dictionary<string, object>();
                            row1.Add("id", id);
                            row1.Add("name", namearr);
                            row1.Add("treat_crno", "0");
                            rows1.Add(row1);
                        }

                        dtresult.Merge(dt_acct_clone);




                    }
                    else if (patientCount.Equals("patientCount"))
                    {
                        string[] date = DateTo.Split('/');
                        string patient = "SELECT * From Patient_Master where FORMAT(Reg_Date,'MM')=" + date[0] + " and year(Reg_Date)=" + date[2] + " and User_Id=" + User_Id;
                        SqlCommand cmdpatient = new SqlCommand(patient, con);
                        SqlDataAdapter daptient = new SqlDataAdapter(cmdpatient);
                        daptient.Fill(dtpatient);
                        //List<string> x = new List<string>();

                        foreach (DataRow drpatient in dtpatient.Rows)
                        {
                            row1 = new Dictionary<string, object>();
                            //x.Add(drpatient[0].ToString());
                            foreach (DataColumn dcpatient in dtpatient.Columns)
                            {
                                row1.Add(dcpatient.ColumnName, drpatient[dcpatient]);
                            }
                            rows1.Add(row1);

                        }
                        con.Open();


                    }
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dtresult.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dtresult.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    rowslist.Add(rows);
                    rowslist.Add(rows1);
                    if (dtresult.Rows.Count > 0)
                    {
                        returnData.data1 = rowslist;
                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        return returnData;
                        //return returnTreatMaster(rowslist, "Success", 1, "Get_MonthPatientDataFromDate");
                    }
                    else if (dtpatient.Rows.Count > 0)
                    {
                        returnData.data1 = rows1;
                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        return returnData;
                        //return returnMethod(rows1, "Success", 1, "Get_MonthPatientDataFromDate");
                    }
                    else
                    {
                        returnData.data1 = "No Record found";
                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        return returnData;
                        // return returnMethodTreatMaster("No Record Found..", "Success", 1, "Get_MonthPatientDataFromDate");
                    }

                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Patient_History")]
        public ReturnObject Patient_History(int Patient_Id)
        {
            ReturnObject returnData = new ReturnObject();

            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {

                string json = string.Empty;
                DataTable dt = new DataTable();
                List<List<Dictionary<string, object>>> rowslist = new List<List<Dictionary<string, object>>>();
                string constr = ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    //var Query = "SELECT * FROM Treatment_Master";
                    using (SqlCommand cmd = new SqlCommand("SELECT account_table.account_id, account_table.transaction_type, Treatment_Master.*, Patient_Master.Patient_name FROM account_table,Patient_Master,Treatment_Master where Patient_Master.Patient_Id = Treatment_Master.Patient_Id AND account_table.Treat_crno = Treatment_Master.Treat_crno and Treatment_Master.Patient_Id='" + Patient_Id + "'", con))
                    {
                        con.Open();
                        SqlDataAdapter datemp = new SqlDataAdapter(cmd);
                        dt.TableName = "Patient_Master";
                        datemp.Fill(dt);


                        DataTable dtresult = dt.Clone();
                        dtresult.Clear();
                        dtresult.Columns.Add("Acct_details");

                        int count = 0;
                        int crno = 0;
                        string stracct = "";
                        string name = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (crno == Convert.ToInt32(dr["treat_crno"]))
                            {
                                continue;
                            }
                            crno = int.Parse(dr["treat_crno"].ToString());
                            DataRow[] result = dt.Select("treat_crno =" + crno);
                            foreach (DataRow drtemp in result)
                            {
                                if (count == 0)
                                {
                                    if (drtemp["transaction_type"].Equals("Debit"))
                                    {
                                        name = "billAmountId";
                                    }
                                    else
                                    {
                                        name = "paidAmountId";
                                    }

                                    stracct = stracct + name + ":" + drtemp["account_id"].ToString();
                                    count = 1;
                                    continue;
                                }
                                if (drtemp["transaction_type"].Equals("Debit"))
                                {
                                    name = "billAmountId";
                                }
                                else
                                {
                                    name = "paidAmountId";
                                }

                                stracct = stracct + "," + name + ":" + drtemp["account_id"].ToString();

                                dtresult.Rows.Add(drtemp["account_id"], drtemp["transaction_type"], drtemp["Treat_date"], drtemp["Treat_crno"], drtemp["SPO_two"], drtemp["Remarks"], drtemp["Patient_temp"], drtemp["Patient_pulse"], drtemp["Patient_image"], drtemp["Patient_complain"], drtemp["Patient_Id"], drtemp["Patient_BP"], drtemp["Medicine"], drtemp["Counsultancyfee"], drtemp["Advice"], drtemp["Status"], drtemp["Referred_Doctor_Name"], drtemp["Referred_Hospital_Detail"], drtemp["Referred_Hospital_Detail"], drtemp["User_Id"], drtemp["Is_Paid"], drtemp["Follow_Date"], drtemp["paid_Amount"], drtemp["bill_Amount"], drtemp["symptoms_id"], drtemp["Patient_name"], stracct);
                                count = 0;
                                stracct = "";

                            }

                        }

                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;

                        foreach (DataRow dr in dtresult.Rows)
                        {
                            row = new Dictionary<string, object>();

                            foreach (DataColumn col in dtresult.Columns)
                            {
                                //row.Add(Title,dr[col]);
                                row.Add(col.ColumnName, dr[col]);
                            }
                            rows.Add(row);
                        }

                        rowslist.Add(rows);
                        SqlCommand cmd2 = new SqlCommand("select symptoms_id,Treat_crno from Treatment_Master where Patient_Id='" + Patient_Id + "'", con);


                        SqlDataReader read = cmd2.ExecuteReader();
                        List<Dictionary<string, object>> ids = new List<Dictionary<string, object>>();
                        List<Dictionary<string, object>> rows1 = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row1;

                        Dictionary<string, object> symp;
                        while (read.Read())
                        {
                            symp = new Dictionary<string, object>();
                            if ((read.GetValue(0).ToString().Equals("")))
                            {
                                symp.Add("", read.GetValue(1).ToString());
                                ids.Add(symp);

                            }
                            else
                            {
                                symp.Add(read.GetValue(0).ToString(), read.GetValue(1).ToString());
                                ids.Add(symp);
                            }

                        }
                        read.Close();
                        int index = 0;
                        foreach (Dictionary<string, object> r in ids)
                        {
                            int[] sympid = { 0 };
                            string[] sympname = { "" };
                            csSymptomsname data = new csSymptomsname();
                            foreach (string key in r.Keys)
                            {
                                if (key.Equals(""))
                                {
                                    data.id = sympid;
                                    foreach (string value in r.Values)
                                    {
                                        data.treat_crno = Convert.ToInt32(value);
                                    }
                                    data.name = sympname;
                                    continue;
                                }
                                string[] sid = key.Split(',');
                                data.id = Array.ConvertAll(sid, int.Parse);
                                foreach (string value in r.Values)
                                {
                                    data.treat_crno = Convert.ToInt32(value);
                                }

                                string strsymp = "select name from symptoms where ";
                                for (int i = 0; i < sid.Length; i++)
                                {
                                    if (i == (sid.Length - 1))
                                    {
                                        strsymp += "id='" + sid[i] + "'";
                                    }
                                    else
                                    {
                                        strsymp += "id='" + sid[i] + "' OR ";
                                    }

                                }
                                SqlCommand cmdsymptoms = new SqlCommand(strsymp, con);
                                SqlDataAdapter dapsymptoms = new SqlDataAdapter(cmdsymptoms);


                                DataTable dtsymptoms = new DataTable();
                                dtsymptoms.TableName = "symptoms";
                                dapsymptoms.Fill(dtsymptoms);


                                List<string> names = new List<string>();


                                foreach (DataRow drow in dtsymptoms.Rows)
                                {
                                    string str = drow[0].ToString();
                                    names.Add(str);

                                }
                                index++;
                                data.name = names.ToArray();


                                dapsymptoms.Dispose();
                                dtsymptoms.Dispose();

                            }
                            row1 = new Dictionary<string, object>();
                            row1.Add("id", data.id);
                            row1.Add("name", data.name);
                            row1.Add("treat_crno", data.treat_crno);
                            rows1.Add(row1);

                        }

                        rowslist.Add(rows1);

                        if (dtresult.Rows.Count > 0)
                        {
                            returnData.data1 = rowslist;
                            returnData.message = "Successfull";
                            returnData.status_code = Convert.ToInt32(Status.Sucess);
                            return returnData;
                            //return returnTreatMaster(rowslist, "Success", 1, "Patience_History");
                        }
                        else
                        {
                            returnData.data1 = "no data found";
                            returnData.message = "Successfull";
                            returnData.status_code = Convert.ToInt32(Status.NotFound);
                            return returnData;
                            //return returnMethodTreatMaster("No Record Found..", "Success", 1, "Patient_History");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
                //   ErrHandler.WriteError(ex.Message, ex);
                //return returnMethodString(ex.Message.ToString(), "Error in code", 2, "Patient_History");
            }


        }

        [HttpGet]
        [ActionName("Send_ReportMail")]
        public ReturnObject Send_ReportMail(string subject, string body)
        {
            ReturnObject returnData = new ReturnObject();
            //const string to1 = "mayur@arkinfosoft.com";
            // const string to2 = "ruchitaajmera27@gmail.com";
            //const string to3 = "info@arkinfosoft.com";Send

            //string cc = "info@arkinfosoft.com" + "|" + "mayur@arkinfosoft.com";
            string cc = "";

            string data;
            try
            {
                if (Email.MailSend("doctordairy@arkinfosoft.com", subject, body, "", cc) == true)
                {
                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    return returnData;
                }
                else
                {
                    returnData.message = "Email send failed";
                    returnData.status_code = Convert.ToInt32(Status.Failed);
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;

            }

        }

        public string Getbase64str(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        [HttpGet]
        [ActionName("Get_Medicine")]
        public ReturnObject Get_Medicine(int Doctor_id)
        {
            ReturnObject returnData = new ReturnObject();

            try
            {
                returnData.data1 = db.medicine_table.Where(x => x.Doctor_id == Doctor_id || x.Doctor_id == null).ToList();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_Package")]
        public ReturnObject Get_Package()
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                //var dbdd = db.Package_Master.ToList();
                var Package = db.Package_Master.Where(x => x.isActive == true).Select(x => new { x.Days, x.Id, x.isActive, x.No_Of_Sms, x.Package_Name, x.Price }).FirstOrDefault();

                returnData.data1 = Package;//db.Package_Master.Where(x => x.isActive == true).ToList();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_Patient")]
        public ReturnObject Get_Patient(string User_Id)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                int userid = int.Parse(User_Id);
                var patientlist = db.Patient_Master.Where(x => x.User_Id == userid).Select(x => new { x.age, x.note, x.Patient_address, x.Patient_city, x.Patient_contact, x.Patient_Country, x.Patient_email, x.Patient_Id, x.Patient_name, x.Patient_photo, x.Patient_state, x.Reg_Date, x.User_Id }).ToList();

                returnData.data1 = patientlist;//db.Patient_Master.Where(x => x.User_Id == userid).ToList();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_Dashboard")]
        public ReturnObject Get_Dashboard(string User_Id, string DateFrom, string DateTo)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                int docid = int.Parse(User_Id);
                List<PatientCounts> patCnts = new List<PatientCounts>();
                string[] date = DateTo.Split('/');
                string followDtto = "" + date[2] + "-" + date[0] + "-" + date[1];
                DateTime dtTo = Convert.ToDateTime(followDtto);
                List<Treatment_Master> MonthlyTreatmentlistForDoc = new List<Treatment_Master>();
                //GetPatientDataCountDash//
                //BeginCollectionWise
                var accnt = db.account_table
                                .Where(x => x.Doctor_id == docid && x.date.Value.Month == dtTo.Month && x.date.Value.Year == dtTo.Year && x.transaction_type == "Debit")
                                .GroupBy(a => a.date)
                                .Select(a => new { Amount = a.Sum(b => b.amount), treatdate = a.Key.ToString() })
                                .OrderBy(a => a.treatdate)
                                .ToList();
                returnData.data1 = accnt;

                MonthlyTreatmentlistForDoc = db.Treatment_Master.Where(x => x.Treat_date.Value.Month == dtTo.Month && x.Treat_date.Value.Year == dtTo.Year && x.User_Id == docid).ToList();


                //Follow up patients 

                var FollowUpPatientCnt = MonthlyTreatmentlistForDoc.Count(x => x.Treat_date == dtTo && x.User_Id == docid);
                patCnts.Add(new Controllers.PatientCounts { type = "followupPatients", counts = FollowUpPatientCnt.ToString() });

                //Normal Patient Count
                var NormalPatientCnt = MonthlyTreatmentlistForDoc.Count(x => x.Status == "Normal");
                patCnts.Add(new Controllers.PatientCounts { type = "NormalMontlyPatients", counts = NormalPatientCnt.ToString() });

                //Critical Patient Count
                var CriticalPatientCnt = MonthlyTreatmentlistForDoc.Count(x => x.Status == "Critical");
                patCnts.Add(new Controllers.PatientCounts { type = "CriticalMontlyPatients", counts = CriticalPatientCnt.ToString() });

                //Unpaid Patient list
                var Unpaidlist = db.account_table
                               .Where(x => x.Doctor_id == docid && x.date.Value.Month == dtTo.Month && x.date.Value.Year == dtTo.Year && x.transaction_type == "Credit")
                               .GroupBy(a => a.date)
                               .Select(a => new { Amount = a.Sum(b => b.amount), treatdate = a.Key.ToString() })
                               .OrderBy(a => a.treatdate)
                               .ToList();

                returnData.data2 = Unpaidlist;
                //Begin TreatedPatients
                var TreatedPtCnt = MonthlyTreatmentlistForDoc.Count();
                patCnts.Add(new Controllers.PatientCounts { type = "MontlyTreatedPatients", counts = TreatedPtCnt.ToString() });

                //Begin ReferredPatients
                var ReferedPtCnt = MonthlyTreatmentlistForDoc.Count(x => x.Referred_Doctor_Name != null && x.Referred_Doctor_Name != "");
                patCnts.Add(new Controllers.PatientCounts { type = "MontlyReferedPatients", counts = ReferedPtCnt.ToString() });

                //Begin ReportPatients
                var ReportedPtCnt = MonthlyTreatmentlistForDoc.Count(x => x.Patient_Report_Detail != null && x.Patient_Report_Detail != "");
                patCnts.Add(new Controllers.PatientCounts { type = "MontlyReportsPatients", counts = ReportedPtCnt.ToString() });

                //Month Patients
                var MonthlyPatients = db.Patient_Master.Count(x => x.Reg_Date.Value.Month == dtTo.Month && x.Reg_Date.Value.Year == dtTo.Year && x.User_Id == docid);
                patCnts.Add(new Controllers.PatientCounts { type = "MonthlyPatients", counts = MonthlyPatients.ToString() });

                //SMS counts
                var Smscount = db.monthly_sms.Where(x => x.date.Value.Month == DateTime.Now.Month && x.date.Value.Year == DateTime.Now.Year && x.user_id == docid).FirstOrDefault();
                if (Smscount != null)
                {
                    patCnts.Add(new Controllers.PatientCounts { type = "Sms_Count", counts = Smscount.sms_count.ToString() });
                    patCnts.Add(new Controllers.PatientCounts { type = "Sms_Remaining_Count", counts = Smscount.sms_remaining_count.ToString() });

                }

                //Package Count
                var Package = db.Package_Assigned.Where(x => x.Last_Date.Value >= DateTime.Now && x.IsActive == true && x.User_id == docid).FirstOrDefault();
                if (Package != null)
                {
                    patCnts.Add(new Controllers.PatientCounts { type = "Package_Count", counts = Package.Package_Count.HasValue.ToString() });
                    patCnts.Add(new Controllers.PatientCounts { type = "Package_Remaining_Count", counts = Package.Package_Remaining_Count.ToString() });
                }

                //doctor country
                var doctor = db.Doctor_Master.Select(x => new { x.User_id, x.Clinic_name, x.Doctor_address, x.Doctor_city, x.Doctor_contact, x.Doctor_email, x.Doctor_country, x.Doctor_exp, x.Doctor_id, x.Doctor_name, x.Doctor_photo, x.Doctor_state, x.Gender, x.Reg_date, x.IsActive }).FirstOrDefault(x => x.User_id == docid);

                patCnts.Add(new Controllers.PatientCounts { type = "DoctorCountry", counts = doctor.Doctor_country });

                //get patient list
                var patientlist = db.Patient_Master.Where(x => x.User_Id == docid).Select(x => new { x.age, x.note, x.Patient_address, x.Patient_city, x.Patient_contact, x.Patient_Country, x.Patient_email, x.Patient_Id, x.Patient_name, x.Patient_photo, x.Patient_state, x.Reg_Date, x.User_Id }).ToList();
                returnData.data3 = patientlist;
                returnData.data4 = patCnts;
                returnData.data5 = doctor;

                //treatment data
                var treatdata = Get_TreatMaster_Dash(docid.ToString());
                returnData.data6 = treatdata;
                //treatment symptops name
                var symptompslist = db.symptoms.Where(x => (x.user_id == null || x.user_id == docid)).ToList();
                returnData.data7 = symptompslist;
                var Disease = db.Diseases.ToList();
                returnData.data8 = Disease;


                //List<Treatment_Master> treatList = db.Treatment_Master.ToList();
                //List<account_table> accntList = db.account_table.ToList();
                //var record = from a in accntList
                //             join t in treatList on a.Treat_crno equals t.Treat_crno
                //             where (a.account_id == docid)
                //             select new
                //             {
                //                 a.account_id,
                //                 t.Patient_Master.Patient_name,
                //                 t.Patient_BP,
                //                 t.Patient_complain,
                //                 t.Treat_date,
                //                 t.Treat_crno,
                //                 t.SPO_two,
                //                 t.Remarks,
                //                 t.Patient_temp,
                //                 t.Patient_pulse,
                //                 t.Patient_image,
                //                 t.Patient_Id,
                //                 t.Medicine,
                //                 t.Counsultancyfee,
                //                 t.Advice,
                //                 t.Status,
                //                 t.Referred_Doctor_Name,
                //                 t.Referred_Hospital_Detail,
                //                 t.Patient_Report_Detail,
                //                 t.User_Id,
                //                 t.Is_Paid,
                //                 t.Follow_Date,
                //                 t.paid_Amount,
                //                 t.bill_Amount,
                //                 t.symptoms_id,

                //             };


                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        //[HttpGet]
        //[ActionName("Get_TreatMaster")]
        public List<Dictionary<string, object>> Get_TreatMaster_Dash(string User_Id)
        {
            // System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                string json = string.Empty;
                DataTable dt = new DataTable();
                string constr = ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

                    using (SqlCommand cmd = new SqlCommand("SELECT account_table.account_id,account_table.transaction_type, Patient_Master.Patient_Id, Patient_Master.Patient_name, Treatment_Master.* FROM account_table INNER JOIN Patient_Master ON  Patient_Master.Patient_Id = account_table.patient_id  INNER JOIN Treatment_Master ON Treatment_Master.Treat_crno = account_table.Treat_crno and account_table.Doctor_id=" + User_Id + "", con))
                    {

                        Dictionary<string, object> row;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        dt.TableName = "Treatment_Master";
                        da.Fill(dt);

                        DataTable dtresult = dt.Clone();
                        dtresult.Clear();
                        dtresult.Columns.Add("Acct_details");
                        dtresult.Columns.Add("Symptoms_Names");

                        int count = 0;
                        int crno = 0;
                        string stracct = "";
                        string name = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (crno == Convert.ToInt32(dr["treat_crno"]))
                            {
                                continue;
                            }
                            crno = int.Parse(dr["treat_crno"].ToString());
                            DataRow[] result = dt.Select("treat_crno =" + crno);
                            foreach (DataRow drtemp in result)
                            {
                                if (count == 0)
                                {
                                    if (drtemp["transaction_type"].Equals("Debit"))
                                    {
                                        name = "billAmountId";
                                    }
                                    else
                                    {
                                        name = "paidAmountId";
                                    }

                                    stracct = stracct + name + ":" + drtemp["account_id"].ToString();
                                    count = 1;
                                    continue;
                                }

                                if (drtemp["transaction_type"].Equals("Debit"))
                                {
                                    name = "billAmountId";
                                }
                                else
                                {
                                    name = "paidAmountId";
                                }


                                stracct = stracct + "," + name + ":" + drtemp["account_id"].ToString();


                                string sym = dr["symptoms_id"].ToString();
                                string sym_nm = "";
                                if (sym != "" && sym != null)
                                {
                                    foreach (var itm in sym.Split(','))
                                    {
                                        int sid = int.Parse(itm);
                                        var nam = db.symptoms.Where(x => x.id == sid).Select(x => x.name).FirstOrDefault();
                                        if (sym_nm == "")
                                        {
                                            sym_nm = nam.ToString();
                                        }
                                        else
                                        {
                                            sym_nm = sym_nm + "," + nam.ToString();
                                        }
                                    }

                                }

                                dtresult.Rows.Add(drtemp["account_id"], drtemp["transaction_type"], drtemp["Patient_Id"], drtemp["Patient_name"], drtemp["Treat_date"], drtemp["Treat_crno"], drtemp["SPO_two"], drtemp["Remarks"], drtemp["Patient_temp"], drtemp["Patient_pulse"], drtemp["Patient_image"], drtemp["Patient_complain"], drtemp["Patient_Id"], drtemp["Patient_BP"], drtemp["Medicine"], drtemp["Counsultancyfee"], drtemp["Advice"], drtemp["Status"], drtemp["Referred_Doctor_Name"], drtemp["Referred_Hospital_Detail"], drtemp["Patient_Report_Detail"], drtemp["User_Id"], drtemp["Is_Paid"], drtemp["Follow_Date"], drtemp["paid_Amount"], drtemp["bill_Amount"], drtemp["symptoms_id"], stracct, sym_nm);
                                count = 0;
                                stracct = "";

                            }


                        }

                        foreach (DataRow dr in dtresult.Rows)
                        {
                            row = new Dictionary<string, object>();

                            foreach (DataColumn col in dtresult.Columns)
                            {

                                row.Add(col.ColumnName, dr[col]);

                            }

                            rows.Add(row);
                        }

                        cmd.Dispose();
                        da.Dispose();
                        dt.Dispose();
                    }


                    return rows;
                    //if (dt.Rows.Count > 0)
                    //{
                    //    return returnTreatMaster(rowslist, "Success", 1, "Get_TreatMaster");
                    //}
                    //else
                    //{
                    //    return returnMethodTreatMaster("No Record Found..", "Success", 1, "Get_TreatMaster");
                    //}

                }
            }
            catch (Exception ex)
            {
                List<Dictionary<string, object>> errorrow = new List<Dictionary<string, object>>();
                return errorrow;
                //   ErrHandler.WriteError(ex.Message, ex);
                //return returnMethodString(ex.Message.ToString(), "Error in code", 2, "Get_TreatMaster");
            }
        }


        [HttpGet]
        [ActionName("Get_Prescription")]
        public ReturnObject Get_Prescription(int Treat_crno)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                returnData.data1 = db.prescription_table.Where(x => x.Treat_crno == Treat_crno).ToList();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_SmsDetail")]
        public ReturnObject Get_SmsDetail(string User_Id)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                int docid = int.Parse(User_Id);
                //SMS counts
                var Smscount = db.monthly_sms.Where(x => x.date.Value.Month == DateTime.Now.Month && x.date.Value.Year == DateTime.Now.Year && x.user_id == docid).Select(x => new { x.date, x.id, x.sms_count, x.sms_remaining_count, x.user_id }).FirstOrDefault();
                //patCnts.Add(new Controllers.PatientCounts { type = "Sms_Count", counts = Smscount.sms_count.ToString() });
                //patCnts.Add(new Controllers.PatientCounts { type = "Sms_Remaining_Count", counts = Smscount.sms_remaining_count.ToString() });

                //Package Count
                var Package = db.Package_Assigned.Where(x => x.Last_Date.Value >= DateTime.Now && x.IsActive == true && x.User_id == docid).Select(x => new { x.Date, x.Id, x.IsActive, x.Last_Date, x.User_id, x.Package_Count, x.Package_Id, x.Package_Remaining_Count }).FirstOrDefault();
                // patCnts.Add(new Controllers.PatientCounts { type = "Package_Count", counts = Package.Package_Count.HasValue.ToString() });
                //patCnts.Add(new Controllers.PatientCounts { type = "Package_Remaining_Count", counts = Package.Package_Remaining_Count.ToString() });

                returnData.data1 = Smscount;
                returnData.data2 = Package;
                if (Smscount == null || Package == null)
                {
                    returnData.message = "Some of the data is not found";
                    returnData.status_code = Convert.ToInt32(Status.NotFound);
                }
                else
                {
                    returnData.message = "Success";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                }
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_SymptomsDisease")]
        public ReturnObject Get_SymptomsDisease(string User_Id)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                int docid = int.Parse(User_Id);

                returnData.data1 = db.symptoms.Where(x => x.user_id == docid || x.user_id == null).ToList();
                returnData.data2 = db.Diseases.ToList();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_User")]
        [AllowAnonymous]
        public async Task<ReturnObject> Get_User(string email, string password, string app_version, string OS)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {

                //int docid = int.Parse(User_Id);
                var user = db.usrs.Select(x => new { x.AccountId, x.Email, x.Firstname, x.Lastname, x.Gender, x.IsActive, x.passwd, x.token_id, x.Id }).FirstOrDefault(x => x.Email == email);
                //usr user = db.usrs.Where(x => x.Email == Email && x.passwd == password).FirstOrDefault();
                // Doctor_Master docmaster = db.Doctor_Master.Where(x => x.Doctor_email == Email).FirstOrDefault();
                if (user != null)
                {
                    user = db.usrs.Select(x => new { x.AccountId, x.Email, x.Firstname, x.Lastname, x.Gender, x.IsActive, x.passwd, x.token_id, x.Id }).FirstOrDefault(x => x.Email == email && x.passwd == password);

                    if (user != null)
                    {
                        var docmaster = db.Doctor_Master.Select(x => new { x.User_id, x.Clinic_name, x.Doctor_address, x.Doctor_city, x.Doctor_contact, x.Doctor_email, x.Doctor_country, x.Doctor_exp, x.Doctor_id, x.Doctor_name, x.Doctor_photo, x.Doctor_state, x.Gender, x.Reg_date, x.IsActive }).FirstOrDefault(x => x.Doctor_email == email);

                        Login_Track lt = new Login_Track();
                        lt.Email = email;
                        lt.App_Version = app_version;
                        lt.Login_Date = DateTime.Now;
                        lt.User_Id = user.Id;
                        lt.OS = OS;
                        db.Login_Track.Add(lt);
                        db.SaveChanges();

                        if (docmaster != null)
                        {
                            //Access Authorization Token
                            string baseAddress = string.Format("{0}://{1}{2}{3}",
                                          System.Web.HttpContext.Current.Request.Url.Scheme,
                                          System.Web.HttpContext.Current.Request.Url.Host,
                                          System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port,
                                          System.Web.HttpContext.Current.Request.ApplicationPath);

                            using (var client = new HttpClient())
                            {
                                var form = new Dictionary<string, string>{
                                   {"grant_type", "password"},
                                   {"username", email.Trim()},
                                   {"password", password.Trim()},
                                };

                                var tokenResponse = client.PostAsync(baseAddress + "/token", new FormUrlEncodedContent(form)).Result;
                                //var token = tokenResponse.Content.ReadAsStringAsync().Result;  
                                var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;

                                returnData.data3 = token;

                            }
                        }

                        if (docmaster != null && docmaster.Doctor_country == "India")
                        {
                            var Smscount = db.monthly_sms.Where(x => x.date.Value.Month == DateTime.Now.Month && x.date.Value.Year == DateTime.Now.Year && x.user_id == user.Id).FirstOrDefault();
                            if (Smscount == null)
                            {
                                monthly_sms ms = new monthly_sms();
                                ms.user_id = user.Id;
                                ms.sms_count = 50;
                                ms.sms_remaining_count = 50;
                                ms.date = DateTime.Now;
                                db.monthly_sms.Add(ms);
                                db.SaveChanges();
                            }
                            returnData.data1 = user;
                            returnData.data2 = docmaster;
                            returnData.message = "Successfull";
                            returnData.status_code = Convert.ToInt32(Status.Sucess);
                            return returnData;
                        }
                        else
                        {
                            returnData.data1 = user;
                            returnData.message = "Doctor data not found";
                            returnData.status_code = Convert.ToInt32(Status.Failed);
                            return returnData;
                        }
                    }
                    else
                    {
                        returnData.message = "Enter valid password.";
                        returnData.status_code = Convert.ToInt32(Status.Failed);
                        return returnData;
                    }
                }
                else
                {
                    returnData.message = "Invalid username and password.";
                    returnData.status_code = Convert.ToInt32(Status.Failed);
                    return returnData;
                }

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Get_Version")]
        public ReturnObject Get_Version()
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                returnData.data1 = db.application_version.OrderByDescending(x => x.version).FirstOrDefault();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Update_UserPassword")]
        public ReturnObject Update_UserPassword(string uid, string newpasswd)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                int usid = int.Parse(uid);
                usr user = db.usrs.FirstOrDefault(x => x.Id == usid);
                user.passwd = newpasswd;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpGet]
        [ActionName("Notification")]
        public ReturnObject Notification(string id, string strtitle, string strbody)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var objNotification = new
                {
                    to = id,
                    notification = new
                    {
                        title = strtitle,
                        body = strbody
                    }
                };
                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AIzaSyDoCijiN5bDUh4pW6bog9RwHcKovUZWTdc"));
                tRequest.Headers.Add(string.Format("Sender: id={0}", "320345011636"));
                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();

                                FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                                if (response.success == 1)
                                {
                                    returnData.message = "Successfull";
                                }
                                else if (response.failure == 1)
                                {
                                    returnData.message = "failed";
                                }

                            }
                        }

                    }
                }

                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpPost]
        [ActionName("Insert_Account")]
        public ReturnObject Insert_Account(csAccount Acc)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                account_table ac = new DoctorDiaryAPI.account_table();
                ac.patient_id = Acc.patient_id;
                ac.remarks = Acc.remarks;
                ac.date = DateTime.Now;
                ac.amount = Convert.ToDecimal(Acc.amount);
                ac.Doctor_id = Convert.ToInt32(Acc.Doctor_id);
                ac.transaction_type = Acc.transaction_type;
                db.account_table.Add(ac);
                db.SaveChanges();

                returnData.data1 = ac.account_id;//db.application_version.OrderByDescending(x => x.version).FirstOrDefault();
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpPost]
        [ActionName("Insert_Medicine")]
        public ReturnObject Insert_Medicine(int User_id, string medicine_name)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                List<medicine_table> mdlist = db.medicine_table.Where(x => x.Medicine == medicine_name && (x.Doctor_id == User_id || x.Doctor_id == null)).ToList();
                if (mdlist.Count == 0)
                {
                    medicine_table md = new medicine_table();
                    md.Doctor_id = User_id;
                    md.Medicine = medicine_name;
                    db.medicine_table.Add(md);
                    db.SaveChanges();
                    db.Entry(md).Reload();
                    returnData.data1 = md.medicine_id;
                }
                else
                {
                    returnData.data1 = "Medicine Already Exists";
                }
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpPost]
        [ActionName("Insert_Doctors")]
        public ReturnObject Insert_Doctors(csDoctor doc)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                string filepath = "";
                if (doc.Doctor_photo != "" && doc.Doctor_photo != null)
                {
                    byte[] f = Convert.FromBase64String(doc.Doctor_photo);
                    MemoryStream ms = new MemoryStream(f);
                    Image returnImage = byteArrayToImage(f);
                    FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/ProfilePicture/") + doc.Doctor_name + "_" + doc.User_id + ".png", FileMode.Create);
                    filepath = "~/ProfilePicture/" + doc.Doctor_name + "_" + doc.User_id + ".png";
                    ms.WriteTo(fs);
                    ms.Close();
                    fs.Close();
                    fs.Dispose();
                }
                //string[] drcol = { "Reg_date", "User_id", "Doctor_state", "Doctor_photo", "Doctor_name", "Doctor_exp", "Doctor_email", "Doctor_degree", "Doctor_country", "Doctor_city", "Doctor_address", "Clinic_name", "Gender", "IsActive" };
                //string[] drval = { DateTime.Now.ToString("yyyy-MM-dd"), User_id.ToString(), Doctor_state, filepath, Doctor_name, Doctor_exp, Doctor_email, Doctor_degree, Doctor_country, Doctor_contact, Doctor_city, Doctor_address, Clinic_name, Gender, IsActive };
                Doctor_Master dm = new Doctor_Master();
                dm.Reg_date = DateTime.Now;
                dm.User_id = doc.User_id;
                dm.Doctor_state = doc.Doctor_state;
                dm.Doctor_photo = filepath;
                dm.Doctor_name = doc.Doctor_name;
                dm.Doctor_exp = doc.Doctor_exp;
                dm.Doctor_email = doc.Doctor_email;
                dm.Doctor_degree = doc.Doctor_degree;
                dm.Doctor_country = doc.Doctor_country;
                dm.Doctor_city = doc.Doctor_city;
                dm.Doctor_address = doc.Doctor_address;
                dm.Gender = doc.Gender;
                dm.IsActive = doc.IsActive == "true" ? true : false;
                db.Doctor_Master.Add(dm);
                db.SaveChanges();
                db.Entry(dm).Reload();

                if (dm.Doctor_id > 0)
                {
                    var unique = new EncryptDecrypt().Encrypt(dm.Doctor_id.ToString());
                    dm.Url = "/Home/Booking?doctorId=" + unique;

                    db.Entry(dm).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Entry(dm).Reload();
                }

                returnData.data1 = dm.Doctor_id;
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpPost]
        [ActionName("Update_Doctor")]
        public ReturnObject Update_Doctor(csDoctor doc)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                string filepath = "";
                if (doc.Doctor_photo != "" && doc.Doctor_photo != null)
                {
                    byte[] f = Convert.FromBase64String(doc.Doctor_photo);
                    MemoryStream ms = new MemoryStream(f);
                    Image returnImage = byteArrayToImage(f);
                    FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/ProfilePicture/") + doc.Doctor_name + "_" + doc.User_id + ".png", FileMode.Create);
                    filepath = "~/ProfilePicture/" + doc.Doctor_name + "_" + doc.User_id + ".png";
                    ms.WriteTo(fs);
                    ms.Close();
                    fs.Close();
                    fs.Dispose();
                }
                //string[] drcol = { "Reg_date", "User_id", "Doctor_state", "Doctor_photo", "Doctor_name", "Doctor_exp", "Doctor_email", "Doctor_degree", "Doctor_country", "Doctor_city", "Doctor_address", "Clinic_name", "Gender", "IsActive" };
                //string[] drval = { DateTime.Now.ToString("yyyy-MM-dd"), User_id.ToString(), Doctor_state, filepath, Doctor_name, Doctor_exp, Doctor_email, Doctor_degree, Doctor_country, Doctor_contact, Doctor_city, Doctor_address, Clinic_name, Gender, IsActive };
                Doctor_Master dm = db.Doctor_Master.FirstOrDefault(x => x.Doctor_id == doc.Doctorid);
                dm.Reg_date = DateTime.Now;
                dm.User_id = doc.User_id;
                dm.Doctor_state = doc.Doctor_state;
                dm.Doctor_photo = filepath;
                dm.Doctor_name = doc.Doctor_name;
                dm.Doctor_exp = doc.Doctor_exp;
                dm.Doctor_email = doc.Doctor_email;
                dm.Doctor_degree = doc.Doctor_degree;
                dm.Doctor_country = doc.Doctor_country;
                dm.Doctor_city = doc.Doctor_city;
                dm.Doctor_address = doc.Doctor_address;
                dm.Gender = doc.Gender;
                dm.IsActive = bool.Parse(doc.IsActive);
                db.Entry(dm).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //Insert Doctor Shift data
                if (doc.doctorShift != null)
                {
                    //returnData = Insert_DoctorShift(doc.doctorShift, db.Database.BeginTransaction());

                    DoctorShift doctorShift = new DoctorShift();

                    doctorShift = db.DoctorShifts.Where(x => x.DoctorId == doc.doctorShift.DoctorId).FirstOrDefault();

                    if (doctorShift != null)
                    {
                        doctorShift.MorningStart = doc.doctorShift.MorningStart;
                        doctorShift.MorningEnd = doc.doctorShift.MorningEnd;
                        doctorShift.AfternoonStart = doc.doctorShift.AfternoonStart;
                        doctorShift.AfternoonEnd = doc.doctorShift.AfternoonEnd;
                        doctorShift.Slot = doc.doctorShift.Slot;

                        doctorShift.UpdatedDate = DateTime.Now;

                        db.Entry(doctorShift).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        doctorShift = new DoctorShift();

                        doctorShift.DoctorId = dm.Doctor_id;
                        doctorShift.MorningStart = doc.doctorShift.MorningStart;
                        doctorShift.MorningEnd = doc.doctorShift.MorningEnd;
                        doctorShift.AfternoonStart = doc.doctorShift.AfternoonStart;
                        doctorShift.AfternoonEnd = doc.doctorShift.AfternoonEnd;
                        doctorShift.Slot = doc.doctorShift.Slot;
                        doctorShift.CreatedDate = DateTime.Now;
                        doctorShift.UpdatedDate = DateTime.Now;

                        db.DoctorShifts.Add(doctorShift);
                        db.SaveChanges();
                    }
                }

                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpPost]
        [ActionName("Insert_Package")]
        public ReturnObject Insert_Package(string user_id, string package_id, string date)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                Package_Requested pack = new Package_Requested();
                pack.Date = DateTime.Now;
                pack.Package_Id = long.Parse(package_id);
                pack.User_Id = int.Parse(user_id);
                pack.isActive = "R";
                db.Package_Requested.Add(pack);
                db.SaveChanges();

                var doc = db.Doctor_Master.FirstOrDefault(x => x.Doctor_id == pack.User_Id);
                var package = db.Package_Master.FirstOrDefault(x => x.Id == pack.Package_Id);

                string title = "Package Request from Doctor Diary";
                string body = "Package Request from:<br/> <br/> Name: " + doc.Doctor_name + "<br/> Contact No: " + doc.Doctor_contact;
                body = body + "<br/><br/>Requested Package:<br/> Package Name: " + package.Package_Name + "<br/> Price: " + package.Price;
                body = body + "<br/> Validity: " + package.Days + "<br/> SMS: " + package.No_Of_Sms;

                Email.MailSend("info@arkinfosoft.com", title, body, "", "");

                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        #region Patient Module
        [HttpPost]
        [ActionName("Insert_Patient")]
        public ReturnObject Insert_Patient(csPatient pat)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                string filepath = "";
                Patient_Master dm = new Patient_Master();
                dm.Reg_Date = Convert.ToDateTime(pat.date);// DateTime.Now;
                dm.User_Id = pat.User_Id;
                dm.Patient_state = pat.Patient_state;
                dm.Patient_photo = pat.Patient_photo;
                dm.Patient_name = pat.Patient_name;
                dm.Patient_email = pat.Patient_email;
                dm.Patient_contact = pat.Patient_contact;
                dm.Patient_Country = pat.Patient_country;
                dm.Patient_city = pat.Patient_city;
                dm.Patient_address = pat.Patient_address;
                dm.note = pat.note;
                dm.IsActive = true;
                dm.age = Convert.ToDecimal(pat.age);

                dm.relation = pat.relation;
                dm.gender = pat.gender;

                db.Patient_Master.Add(dm);
                db.SaveChanges();
                db.Entry(dm).Reload();

                DoctorPatient_Master temp = new DoctorPatient_Master();

                //temp.DoctorId = dm.DoctorId;
                temp.PatientId = dm.Patient_Id;
                temp.IsActive = true;
                temp.CreatedDate = DateTime.Now;
                temp.UpdatedDate = DateTime.Now;

                db.DoctorPatient_Master.Add(temp);
                db.SaveChanges();

                returnData.data1 = dm;
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpPost]
        [ActionName("Update_Patient")]
        public ReturnObject Update_Patient(csPatient pat)
        {
            ReturnObject returnData = new ReturnObject();
            try
            {
                string filepath = "";
                Patient_Master dm = db.Patient_Master.Include(a => a.usr).FirstOrDefault(x => x.Patient_Id == pat.Patient_Id);
                dm.Reg_Date = DateTime.Now;
                dm.User_Id = pat.User_Id;
                dm.Patient_state = pat.Patient_state;
                dm.Patient_photo = pat.Patient_photo;
                dm.Patient_name = pat.Patient_name;
                dm.Patient_email = pat.Patient_email;
                dm.Patient_contact = pat.Patient_contact;
                dm.Patient_Country = pat.Patient_country;
                dm.Patient_city = pat.Patient_city;
                dm.Patient_address = pat.Patient_address;
                dm.note = pat.note;
                dm.age = pat.age != "" ? Convert.ToDecimal(pat.age) : 0;

                dm.relation = pat.relation;
                dm.gender = pat.gender;

                db.Entry(dm).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //db.Patient_Master.Add(dm);
                //db.SaveChanges();
                //db.Entry(dm).Reload();
                //returnData.data1 = dm;
                returnData.message = "Successfull";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }
        #endregion

        /// <summary>
        /// Change by : Harshal koshti 
        /// Purpose :  in this api add new functionality for add treatment images in database for perticular treatment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Insert_Treat")]
        public ReturnObject Insert_Treat(System.Net.Http.HttpRequestMessage request)
        {
            csTreat tr = new csTreat();
            ReturnObject returnData = new ReturnObject();
            List<PatientCounts> pts = new List<PatientCounts>();
            List<csTritmentImage> modelTreatmentImage = new List<csTritmentImage>();
            try
            {
                var modelTreatment = HttpContext.Current.Request.Form["model"];
                if (!string.IsNullOrEmpty(modelTreatment))
                {
                    tr = JsonConvert.DeserializeObject<csTreat>(modelTreatment);
                    string symptoms_id = "";
                    int chksms = 0;
                    long dsid = 0;
                    List<csSymptoms> symp_name = new List<csSymptoms>();// =  ? SendList(tr.symptoms) : null;
                    csSymptoms modelsym;
                    foreach (var sym in tr.symptoms)
                    {
                        modelsym = new csSymptoms();
                        modelsym.name = sym;
                        symp_name.Add(modelsym);

                    }
                    //if (tr.symptoms != null && tr.symptoms != "")
                    //{
                    //    symp_name = SendList(tr.symptoms);
                    //}

                    //NOTE : Here Patient_complain is a Disease
                    if (tr.Patient_complain != null && tr.Patient_complain != "")
                    {
                        string[] disease_name = tr.Patient_complain.Split(',');
                        Disease ds = new Disease();
                        foreach (string dis_name in disease_name)
                        {
                            var disease = db.Diseases.Where(x => x.disease_name == dis_name).Select(x => x.id).FirstOrDefault();
                            if (disease == 0)
                            {

                                ds.disease_name = dis_name;
                                db.Diseases.Add(ds);
                                db.SaveChanges();
                                db.Entry(ds).Reload();

                                if (symp_name.Count != 0 && symp_name != null)
                                {
                                    foreach (csSymptoms sym in symp_name)
                                    {
                                        symptom sm = new symptom();
                                        sm.d_id = ds.id;
                                        sm.name = sym.name;
                                        sm.user_id = long.Parse(tr.User_Id);
                                        db.symptoms.Add(sm);
                                        db.SaveChanges();
                                        //db.Entry(sm).Reload();
                                    }

                                }

                            }
                            else
                            {
                                dsid = disease;
                            }

                        }
                    }

                    //Symptopms Table 
                    int j = 0;
                    long userid = long.Parse(tr.User_Id);
                    if (tr.symptoms != null)
                    {
                        if (symp_name.Count != 0)
                        {
                            foreach (csSymptoms sym in symp_name)
                            {
                                var symt = db.symptoms.Where(x => x.name == sym.name && (x.user_id.Value == userid || x.user_id == null)).Select(x => x.id).FirstOrDefault();
                                if (symt == 0)
                                {
                                    symptom sm = new symptom();
                                    sm.d_id = dsid;//null;//ds.id;
                                    sm.name = sym.name;
                                    sm.user_id = long.Parse(tr.User_Id);
                                    db.symptoms.Add(sm);
                                    db.SaveChanges();
                                    db.Entry(sm).Reload();

                                    if (j == 0)
                                    {
                                        symptoms_id = sm.id.ToString();
                                        j++;
                                    }
                                    else
                                    {
                                        symptoms_id = symptoms_id + "," + sm.id.ToString();
                                    }

                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        symptoms_id = symt.ToString();
                                        j++;
                                    }
                                    else
                                    {
                                        symptoms_id = symptoms_id + "," + symt.ToString();
                                    }
                                }
                            }
                        }
                    }

                    // Insert into Treatment Master
                    Treatment_Master treat = new Treatment_Master();
                    treat.Treat_date = Convert.ToDateTime(tr.Treat_date);
                    treat.SPO_two = tr.SPO_two;
                    treat.Remarks = tr.Remarks;
                    treat.Patient_temp = tr.Patient_temp;
                    treat.Patient_pulse = tr.Patient_pulse;
                    treat.Patient_image = tr.Patient_image;
                    treat.Patient_complain = tr.Patient_complain;
                    treat.Patient_Id = int.Parse(tr.Patient_Id);
                    treat.Patient_BP = tr.Patient_BP;
                    treat.Medicine = tr.Medicine;
                    treat.Counsultancyfee = tr.Counsultancyfee != "" ? Convert.ToDecimal(tr.Counsultancyfee) : 0;
                    treat.Advice = tr.Advice;
                    treat.Status = tr.Status;
                    treat.Referred_Doctor_Name = tr.Referred_Doctor_Name;
                    treat.Referred_Hospital_Detail = tr.Referred_Hospital_Detail;
                    treat.Patient_Report_Detail = tr.Patient_Report_Detail;
                    treat.User_Id = int.Parse(tr.User_Id);
                    treat.Is_Paid = tr.Is_Paid == "true" ? true : false; //bool.Parse(tr.Is_Paid);
                    if (tr.Follow_Date != "" && tr.Follow_Date != null)
                    {
                        treat.Follow_Date = Convert.ToDateTime(treat.Follow_Date);
                    }
                    treat.bill_Amount = Convert.ToDecimal(tr.bill_Amount);
                    treat.paid_Amount = Convert.ToDecimal(tr.paid_Amount);
                    treat.symptoms_id = symptoms_id;
                    db.Treatment_Master.Add(treat);
                    db.SaveChanges();
                    db.Entry(treat).Reload();

                    var fileCount = HttpContext.Current.Request.Files;
                    //This portion is check treatment image folder is exist or not 
                    var defaultPath = System.Web.HttpContext.Current.Server;
                    string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/TreatmentImage/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //this portion is use get file from requeest and save into treatmentImage folder
                    Directory.CreateDirectory(folderPath + treat.Treat_crno);
                    for (int i = 1; i <= fileCount.Count; i++)
                    {
                        csTritmentImage modelImage = new csTritmentImage();
                        var file = HttpContext.Current.Request.Files["img" + i];
                        if (file != null)
                        {
                            var filename = Path.GetFileName(file.FileName);
                            var path = Path.Combine(folderPath + treat.Treat_crno, treat.Treat_crno + "_" + i + ".png");
                            file.SaveAs(path);

                            string DbImagePath = ("~/TreatmentImage/" + treat.Treat_crno + "/" + treat.Treat_crno + "_" + i + ".png");
                            modelImage.Treat_crno = treat.Treat_crno;
                            modelImage.Image_Path = DbImagePath;
                            modelImage.CreatedDate = DateTime.Now;
                            modelTreatmentImage.Add(modelImage);
                        }

                    }

                    if (treat.Treat_crno > 0 && modelTreatmentImage.Count > 0)
                    {
                        List<Treatment_Image_Master> modelTreatImage = new List<Treatment_Image_Master>();
                        modelTreatImage = ConvertTreatmentImageModel(modelTreatmentImage);
                        db.Treatment_Image_Master.AddRange(modelTreatImage);
                        db.SaveChanges();
                    }

                    if (treat.Treat_crno > 0)
                    {
                        account_table accDebit = new account_table();
                        accDebit.patient_id = treat.Patient_Id.Value;
                        accDebit.amount = treat.bill_Amount.Value;
                        accDebit.remarks = null;
                        accDebit.transaction_type = "Debit";
                        accDebit.Treat_crno = treat.Treat_crno;
                        accDebit.Doctor_id = treat.User_Id.Value;
                        accDebit.date = treat.Treat_date;
                        db.account_table.Add(accDebit);
                        db.SaveChanges();
                        db.Entry(accDebit).Reload();

                        account_table accCredit = new account_table();
                        accCredit.patient_id = treat.Patient_Id.Value;
                        accCredit.amount = treat.paid_Amount.Value;
                        accCredit.remarks = null;
                        accCredit.transaction_type = "Credit";
                        accCredit.Treat_crno = treat.Treat_crno;
                        accCredit.Doctor_id = treat.User_Id.Value;
                        accCredit.date = treat.Treat_date;
                        db.account_table.Add(accCredit);
                        db.SaveChanges();
                        db.Entry(accCredit).Reload();

                        //insert prescription code
                        string sms = "For:" + tr.patientName;
                        int med_count = 0;
                        List<csPrescription> items = null;// SendTransaction(tr.prescription);
                        if (tr.prescription != null && tr.prescription != "")
                        {
                            items = SendTransaction(tr.prescription);
                        }

                        if (items != null)
                        {
                            foreach (csPrescription obj in items)
                            {
                                med_count++;
                                if (Convert.ToBoolean(obj.send))
                                {
                                    chksms = 1;
                                    sms = sms + Environment.NewLine + "" + med_count + "." + obj.medicine_name + "(";

                                }
                                if (obj.isMorning == null)
                                {
                                    obj.isMorning = "0";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(obj.isMorning))
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {
                                            sms = sms + "1-";
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {
                                            sms = sms + "0-";
                                        }
                                    }
                                }
                                if (obj.isNoon == null)
                                {
                                    obj.isNoon = "0";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(obj.isNoon))
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {
                                            sms = sms + "1-";
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {
                                            sms = sms + "0-";
                                        }
                                    }
                                }

                                if (obj.isEvening == null)
                                {
                                    obj.isEvening = "0";


                                }
                                else
                                {

                                    if (Convert.ToBoolean(obj.isEvening))
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {
                                            sms = sms + "1-";

                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {
                                            sms = sms + "0-";

                                        }

                                    }

                                }


                                if (obj.isNight == null)
                                {
                                    obj.isNight = "0";


                                }
                                else
                                {

                                    if (Convert.ToBoolean(obj.isNight))
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {
                                            sms = sms + "1";
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {
                                            sms = sms + "0";
                                        }
                                    }


                                }

                                if (obj.no_of_tablet.Equals(""))
                                {
                                    obj.no_of_tablet = "0";


                                }
                                else
                                {


                                    if (Convert.ToBoolean(obj.send))
                                    {

                                        sms = sms + ")(" + obj.no_of_tablet + ")";

                                    }

                                }
                                if (obj.note == null)
                                {
                                    obj.note = "";

                                }
                                else
                                {

                                    if (obj.note.Equals(""))
                                    {

                                    }
                                    else
                                    {
                                        if (obj.send.Equals("true"))
                                        {
                                            sms = sms + "(" + obj.note + ")";

                                        }
                                    }

                                }
                                prescription_table pt = new prescription_table();
                                pt.Doctor_id = treat.User_Id.Value;
                                pt.Patient_Id = treat.Patient_Id.Value;
                                pt.Treat_crno = treat.Treat_crno;
                                pt.medicine_name = obj.medicine_name;
                                pt.note = obj.note;
                                pt.isMorning = obj.isMorning == "true" ? true : false;
                                pt.isNight = obj.isNight == "true" ? true : false; //bool.Parse(obj.isNight);
                                pt.isNoon = obj.isNoon == "true" ? true : false; //bool.Parse(obj.isNoon);
                                pt.isEvening = obj.isEvening == "true" ? true : false;// bool.Parse(obj.isEvening);
                                pt.no_of_tablet = obj.no_of_tablet;
                                db.prescription_table.Add(pt);
                                db.SaveChanges();
                                db.Entry(pt).Reload();

                                //Insert Medicine Table
                                var medTablist = db.medicine_table.Where(x => x.Medicine == obj.medicine_name && x.Doctor_id == treat.User_Id).ToList();
                                if (medTablist.Count == 0)
                                {
                                    medicine_table mdt = new medicine_table();
                                    mdt.Doctor_id = treat.User_Id;
                                    mdt.Medicine = obj.medicine_name;
                                    db.medicine_table.Add(mdt);
                                    db.SaveChanges();
                                }
                            }

                        }
                        if (chksms > 0)
                        {
                            sms = sms + Environment.NewLine + "From:Dr." + tr.DocNm + " via DoctorDiary";

                            bool ans = false;

                            int smslength = sms.Length;
                            if (smslength > 140)
                            {
                                smslength = smslength / 140;
                                if (smslength % 140 != 0)
                                {
                                    smslength = smslength + 1;
                                }
                            }
                            else
                            {
                                smslength = 1;
                            }

                            int remcount, rem;

                            ans = SmsSend.Send(tr.Phone, sms);
                            bool isMOnthly;
                            bool isdeliverd;
                            if (ans)
                            {
                                returnData.data1 = "SMS send successfully";
                                isdeliverd = true;
                                if (tr.priority.Equals("Sms"))
                                {
                                    isMOnthly = true;
                                    //int userid = int.Parse(tr.User_Id);
                                    var monthsms = db.monthly_sms.Where(x => x.date.Value.Month == DateTime.Now.Month && x.date.Value.Year == DateTime.Now.Year && x.user_id == userid).Select(x => new { x.id, x.sms_remaining_count }).FirstOrDefault();
                                    if (monthsms.sms_remaining_count > 0)
                                    {
                                        rem = monthsms.sms_remaining_count - smslength;
                                        monthly_sms ms = db.monthly_sms.Where(x => x.id == monthsms.id && x.user_id == userid).FirstOrDefault();
                                        ms.sms_remaining_count = rem;
                                        db.Entry(ms).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    isMOnthly = false;
                                    var packageassigned = db.Package_Assigned.Where(x => x.Last_Date >= DateTime.Now && x.IsActive == true && x.User_id == userid).FirstOrDefault();
                                    if (packageassigned.Package_Remaining_Count > 0)
                                    {
                                        rem = packageassigned.Package_Remaining_Count - smslength;
                                        if (rem <= 0)
                                        {
                                            packageassigned.Package_Remaining_Count = 0;
                                            packageassigned.IsActive = false;
                                            db.Entry(packageassigned).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            packageassigned.Package_Remaining_Count = rem;
                                            db.Entry(packageassigned).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                    }


                                }
                            }
                            else
                            {
                                isdeliverd = false;
                                if (tr.priority.Equals("Sms"))
                                {
                                    isMOnthly = true;
                                }
                                else
                                {
                                    isMOnthly = false;
                                }

                                returnData.data1 = "SMS sending failed";

                            }
                            SMS_Master smsmas = new SMS_Master();
                            smsmas.Date = DateTime.Now;
                            smsmas.isDelivered = isdeliverd;
                            smsmas.isMonthly = isMOnthly;
                            smsmas.Message = sms;
                            smsmas.To_Address = tr.Phone;
                            smsmas.User_id = int.Parse(userid.ToString());
                            db.SMS_Master.Add(smsmas);
                            db.SaveChanges();

                        }
                        else
                        {
                            returnData.data1 = "No SMS selected";
                        }

                        if (Convert.ToBoolean(tr.doctor_country))
                        {
                            var smscount = db.monthly_sms.Where(x => x.date.Value.Month == DateTime.Now.Month && x.date.Value.Year == DateTime.Now.Year && x.user_id == userid).Select(x => new { x.sms_count, x.sms_remaining_count }).FirstOrDefault();
                            if (smscount != null)
                            {
                                pts.Add(new Controllers.PatientCounts { type = "Sms_Count", counts = smscount.sms_count.ToString() });
                                pts.Add(new Controllers.PatientCounts { type = "Sms_Remaining_Count", counts = smscount.sms_remaining_count.ToString() });
                            }
                            else
                            {
                                pts.Add(new Controllers.PatientCounts { type = "Sms_Count", counts = "0" });
                                pts.Add(new Controllers.PatientCounts { type = "Sms_Remaining_Count", counts = "0" });
                            }

                            var packcnt = db.Package_Assigned.Where(x => x.Last_Date >= DateTime.Now && x.IsActive == true && x.User_id == userid).FirstOrDefault();
                            if (packcnt != null)
                            {
                                pts.Add(new Controllers.PatientCounts { type = "Package_Count", counts = packcnt.Package_Count.ToString() });
                                pts.Add(new Controllers.PatientCounts { type = "Package_Remaining_Count", counts = packcnt.Package_Remaining_Count.ToString() });
                            }
                            else
                            {
                                pts.Add(new Controllers.PatientCounts { type = "Package_Count", counts = "0" });
                                pts.Add(new Controllers.PatientCounts { type = "Package_Remaining_Count", counts = "0" });
                            }

                        }
                        else
                        {
                            pts.Add(new Controllers.PatientCounts { type = "Sms_Count", counts = "0" });
                            pts.Add(new Controllers.PatientCounts { type = "Sms_Remaining_Count", counts = "0" });
                            pts.Add(new Controllers.PatientCounts { type = "Package_Count", counts = "0" });
                            pts.Add(new Controllers.PatientCounts { type = "Package_Remaining_Count", counts = "0" });
                        }
                        returnData.data2 = pts;
                        returnData.data3 = treat;
                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        return returnData;

                    }
                    else
                    {
                        returnData.message = "Falied";
                        returnData.status_code = Convert.ToInt32(Status.Failed);
                        return returnData;
                    }
                }
                else
                {
                    returnData.message = "Treatment object not found! ";
                    returnData.status_code = Convert.ToInt32(Status.Failed);
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        /// <summary>
        /// Change by : Harshal koshti 
        /// purpose : in this api add new functionality for add treatment image for perticular treatment.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Update_Treat")]
        public ReturnObject Update_Treat(System.Net.Http.HttpRequestMessage request)
        {
            csTreat tr = new csTreat();
            ReturnObject returnData = new ReturnObject();
            List<PatientCounts> pts = new List<PatientCounts>();
            List<csTritmentImage> modelTreatmentImage = new List<csTritmentImage>();
            try
            {
                long dsid = 0;
                string symptoms_id = "";
                int chksms = 0;
                var modelTreatment = HttpContext.Current.Request.Form["model"];
                if (!string.IsNullOrEmpty(modelTreatment))
                {
                    tr = JsonConvert.DeserializeObject<csTreat>(modelTreatment);

                    List<csSymptoms> symp_name = new List<csSymptoms>();// SendList(tr.symptoms);
                    csSymptoms modelsym;
                    foreach (var sym in tr.symptoms)
                    {
                        modelsym = new csSymptoms();
                        modelsym.name = sym;
                        symp_name.Add(modelsym);
                    }
                    //if (tr.symptoms != null && tr.symptoms != "")
                    //{
                    //    symp_name = SendList(tr.symptoms);
                    //}
                    if (tr.Patient_complain != null && tr.Patient_complain != "")
                    {
                        string[] disease_name = tr.Patient_complain.Split(',');
                        Disease ds = new Disease();
                        foreach (string dis_name in disease_name)
                        {
                            var disease = db.Diseases.Where(x => x.disease_name == dis_name).Select(x => x.id).FirstOrDefault();
                            if (disease == 0)
                            {

                                ds.disease_name = dis_name;
                                db.Diseases.Add(ds);
                                db.SaveChanges();
                                db.Entry(ds).Reload();

                                if (symp_name.Count != 0)
                                {
                                    foreach (csSymptoms sym in symp_name)
                                    {
                                        symptom sm = new symptom();
                                        sm.d_id = ds.id;
                                        sm.name = sym.name;
                                        sm.user_id = long.Parse(tr.User_Id);
                                        db.symptoms.Add(sm);
                                        db.SaveChanges();
                                        db.Entry(sm).Reload();
                                    }

                                }

                            }
                            else
                            {
                                dsid = disease;
                            }

                        }
                    }

                    //Symptopms Table 
                    int j = 0;
                    long userid = long.Parse(tr.User_Id);
                    if (symp_name != null)
                    {
                        if (symp_name.Count > 0)
                        {
                            foreach (csSymptoms sym in symp_name)
                            {
                                var symt = db.symptoms.Where(x => x.name == sym.name && (x.user_id.Value == userid || x.user_id == null)).Select(x => x.id).FirstOrDefault();
                                if (symt == 0)
                                {
                                    symptom sm = new symptom();
                                    sm.d_id = dsid;
                                    sm.name = sym.name;
                                    sm.user_id = long.Parse(tr.User_Id);
                                    db.symptoms.Add(sm);
                                    db.SaveChanges();
                                    db.Entry(sm).Reload();

                                    if (j == 0)
                                    {
                                        symptoms_id = sm.id.ToString();
                                        j++;
                                    }
                                    else
                                    {
                                        symptoms_id = symptoms_id + "," + sm.id.ToString();
                                    }

                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        symptoms_id = symt.ToString();
                                        j++;
                                    }
                                    else
                                    {
                                        symptoms_id = symptoms_id + "," + symt.ToString();
                                    }
                                }
                            }
                        }
                    }

                    // Insert into Treatment Master
                    Treatment_Master treat = db.Treatment_Master.FirstOrDefault(x => x.Treat_crno == tr.Treat_crno);
                    treat.Treat_date = Convert.ToDateTime(tr.Treat_date);
                    treat.SPO_two = tr.SPO_two;
                    treat.Remarks = tr.Remarks;
                    treat.Patient_temp = tr.Patient_temp;
                    treat.Patient_pulse = tr.Patient_pulse;
                    treat.Patient_image = tr.Patient_image;
                    treat.Patient_complain = tr.Patient_complain;
                    treat.Patient_Id = int.Parse(tr.Patient_Id);
                    treat.Patient_BP = tr.Patient_BP;
                    treat.Medicine = tr.Medicine;
                    treat.Counsultancyfee = tr.Counsultancyfee != "" ? Convert.ToDecimal(tr.Counsultancyfee) : 0;
                    treat.Advice = tr.Advice;
                    treat.Status = tr.Status;
                    treat.Referred_Doctor_Name = tr.Referred_Doctor_Name;
                    treat.Referred_Hospital_Detail = tr.Referred_Hospital_Detail;
                    treat.Patient_Report_Detail = tr.Patient_Report_Detail;
                    treat.User_Id = int.Parse(tr.User_Id);
                    treat.Is_Paid = tr.Is_Paid == "true" ? true : false;// bool.Parse();
                    if (tr.Follow_Date != "" && tr.Follow_Date != null)
                    {
                        treat.Follow_Date = Convert.ToDateTime(treat.Follow_Date);
                    }
                    treat.bill_Amount = Convert.ToDecimal(tr.bill_Amount);
                    treat.paid_Amount = Convert.ToDecimal(tr.paid_Amount);
                    treat.symptoms_id = symptoms_id;
                    db.Entry(treat).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    //db.Treatment_Master.Add(treat);
                    //db.SaveChanges();
                    //db.Entry(treat).Reload();

                    var fileCount = HttpContext.Current.Request.Files;
                    //Check in folder current treatmemntNo folder is exist or not.
                    //if exist then remove current folder with all file and create new folder with same id. and add new image
                    string treatmentFile = System.Web.HttpContext.Current.Server.MapPath("~/TreatmentImage/" + treat.Treat_crno);
                    if (Directory.Exists(treatmentFile))
                    {
                        foreach (string file in Directory.GetFiles(treatmentFile))
                        {
                            File.Delete(file);
                        }
                        Directory.Delete(treatmentFile);
                    }
                    Directory.CreateDirectory(treatmentFile);

                    //this portion is use for save image in folder and add file name to model
                    for (int i = 1; i <= fileCount.Count; i++)
                    {
                        csTritmentImage modelImage = new csTritmentImage();
                        var file = HttpContext.Current.Request.Files["img" + i];

                        var filename = Path.GetFileName(file.FileName);
                        //var path = Path.Combine(treatmentFile, filename);
                        var path = Path.Combine(treatmentFile, treat.Treat_crno + "_" + i + ".png");
                        file.SaveAs(path);

                        string DbImagePath = ("~/TreatmentImage/" + treat.Treat_crno + "/" + treat.Treat_crno + "_" + i + ".png");
                        modelImage.Treat_crno = treat.Treat_crno;
                        modelImage.Image_Path = DbImagePath;
                        modelImage.CreatedDate = DateTime.Now;
                        modelTreatmentImage.Add(modelImage);
                    }

                    //this portion is use for remove current treatment image from db record and add new record
                    var oldTreatmentImage = db.Treatment_Image_Master.Where(a => a.Treat_crno == treat.Treat_crno).ToList();
                    if (oldTreatmentImage != null && oldTreatmentImage.Count > 0)
                    {
                        db.Treatment_Image_Master.RemoveRange(oldTreatmentImage);
                        db.SaveChanges();
                    }

                    //This portion is use to add treatment image in database
                    if (treat.Treat_crno > 0 && modelTreatmentImage.Count > 0)
                    {
                        List<Treatment_Image_Master> modelTreatImage = new List<Treatment_Image_Master>();
                        modelTreatImage = ConvertTreatmentImageModel(modelTreatmentImage);
                        db.Treatment_Image_Master.AddRange(modelTreatImage);
                        db.SaveChanges();
                    }

                    if (treat.Treat_crno > 0)
                    {
                        if (tr.Acc_BillId != 0)
                        {
                            account_table accDebit = db.account_table.FirstOrDefault(x => x.account_id == tr.Acc_BillId);//new account_table();
                            accDebit.patient_id = int.Parse(tr.Patient_Id);//treat.Patient_Id.Value;
                            accDebit.amount = treat.bill_Amount.Value;
                            accDebit.remarks = null;
                            accDebit.transaction_type = "Debit";
                            accDebit.Treat_crno = treat.Treat_crno;
                            accDebit.Doctor_id = treat.User_Id.Value;
                            accDebit.date = treat.Treat_date;
                            //db.account_table.Add(accDebit);
                            //db.SaveChanges();
                            //db.Entry(accDebit).Reload();
                            db.Entry(accDebit).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }



                        //account_table accCredit = new account_table();
                        if (tr.Acc_PaidId != 0)
                        {
                            account_table accCredit = db.account_table.FirstOrDefault(x => x.account_id == tr.Acc_PaidId);//new account_table();
                            accCredit.patient_id = int.Parse(tr.Patient_Id);//treat.Patient_Id.Value;
                            accCredit.amount = treat.paid_Amount.Value;
                            accCredit.remarks = null;
                            accCredit.transaction_type = "Credit";
                            accCredit.Treat_crno = treat.Treat_crno;
                            accCredit.Doctor_id = treat.User_Id.Value;
                            accCredit.date = treat.Treat_date;
                            //db.account_table.Add(accCredit);
                            //db.SaveChanges();
                            //db.Entry(accCredit).Reload();
                            db.Entry(accCredit).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                        //insert prescription code
                        string sms = "For:" + tr.patientName;
                        int med_count = 0;
                        List<csPrescription> items = null;

                        if (tr.prescription != null)
                        {
                            SendTransaction(tr.prescription);
                        }
                        if (items != null)
                        {
                            foreach (csPrescription obj in items)
                            {
                                med_count++;
                                if (obj.prescription_id == "0")
                                {
                                    if (Convert.ToBoolean(obj.send))
                                    {
                                        chksms = 1;
                                        sms = sms + Environment.NewLine + "" + med_count + "." + obj.medicine_name + "(";

                                    }
                                    if (obj.isMorning == null)
                                    {
                                        obj.isMorning = "0";
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.isMorning))
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "1-";
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "0-";
                                            }
                                        }
                                    }
                                    if (obj.isNoon == null)
                                    {
                                        obj.isNoon = "0";
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.isNoon))
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "1-";
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "0-";
                                            }
                                        }
                                    }

                                    if (obj.isEvening == null)
                                    {
                                        obj.isEvening = "0";


                                    }
                                    else
                                    {

                                        if (Convert.ToBoolean(obj.isEvening))
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "1-";

                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "0-";

                                            }

                                        }

                                    }


                                    if (obj.isNight == null)
                                    {
                                        obj.isNight = "0";


                                    }
                                    else
                                    {

                                        if (Convert.ToBoolean(obj.isNight))
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "1";
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "0";
                                            }
                                        }


                                    }

                                    if (obj.no_of_tablet.Equals(""))
                                    {
                                        obj.no_of_tablet = "0";


                                    }
                                    else
                                    {


                                        if (Convert.ToBoolean(obj.send))
                                        {

                                            sms = sms + ")(" + obj.no_of_tablet + ")";

                                        }

                                    }
                                    if (obj.note == null)
                                    {
                                        obj.note = "";

                                    }
                                    else
                                    {

                                        if (obj.note.Equals(""))
                                        {

                                        }
                                        else
                                        {
                                            if (obj.send.Equals("true"))
                                            {
                                                sms = sms + "(" + obj.note + ")";

                                            }
                                        }

                                    }
                                    prescription_table pt = new prescription_table();
                                    pt.Doctor_id = treat.User_Id.Value;
                                    pt.Patient_Id = treat.Patient_Id.Value;
                                    pt.Treat_crno = treat.Treat_crno;
                                    pt.medicine_name = obj.medicine_name;
                                    pt.note = obj.note;
                                    pt.isMorning = obj.isMorning == "true" ? true : false;
                                    pt.isNight = obj.isNight == "true" ? true : false;// bool.Parse(obj.isNight);
                                    pt.isNoon = obj.isNoon == "true" ? true : false;// bool.Parse(obj.isNoon);
                                    pt.isEvening = obj.isEvening == "true" ? true : false;// bool.Parse(obj.isEvening);
                                    pt.no_of_tablet = obj.no_of_tablet;
                                    db.prescription_table.Add(pt);
                                    db.SaveChanges();
                                    db.Entry(pt).Reload();


                                }
                                else
                                {
                                    if (Convert.ToBoolean(obj.send))
                                    {
                                        chksms = 1;
                                        sms = sms + Environment.NewLine + "" + med_count + "." + obj.medicine_name + "(";

                                    }

                                    if (obj.isMorning == null)
                                    {
                                        obj.isMorning = "0";
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.isMorning))
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "1-";
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "0-";
                                            }
                                        }


                                    }
                                    if (obj.isNoon == null)
                                    {
                                        obj.isNoon = "0";
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.isNoon))
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "1-";
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "0-";
                                            }

                                        }

                                    }

                                    if (obj.isEvening == null)
                                    {
                                        obj.isEvening = "0";
                                    }
                                    else
                                    {

                                        if (Convert.ToBoolean(obj.isEvening))
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "1-";
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "0-";
                                            }
                                        }

                                    }


                                    if (obj.isNight == null)
                                    {
                                        obj.isNight = "0";
                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.isNight))
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "1";

                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "0";

                                            }
                                        }


                                    }


                                    if (obj.no_of_tablet.Equals(""))
                                    {
                                        obj.no_of_tablet = "0";

                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(obj.send))
                                        {

                                            sms = sms + ")(" + obj.no_of_tablet + ")";


                                        }

                                    }

                                    if (obj.note == null)
                                    {
                                        obj.note = " ";

                                    }
                                    else
                                    {


                                        if (obj.note.Equals(""))
                                        {
                                        }
                                        else
                                        {
                                            if (Convert.ToBoolean(obj.send))
                                            {
                                                sms = sms + "(" + obj.note + ")";

                                            }

                                        }
                                    }
                                    int pr_id = int.Parse(obj.prescription_id);
                                    prescription_table pt = db.prescription_table.FirstOrDefault(z => z.prescription_id == pr_id);//new prescription_table();
                                    pt.Doctor_id = treat.User_Id.Value;
                                    pt.Patient_Id = treat.Patient_Id.Value;
                                    pt.Treat_crno = treat.Treat_crno;
                                    pt.medicine_name = obj.medicine_name;
                                    pt.note = obj.note;
                                    pt.isMorning = obj.isMorning == "true" ? true : false;
                                    pt.isNight = obj.isNight == "true" ? true : false;// bool.Parse(obj.isNight);
                                    pt.isNoon = obj.isNoon == "true" ? true : false;// bool.Parse(obj.isNoon);
                                    pt.isEvening = obj.isEvening == "true" ? true : false;// bool.Parse(obj.isEvening);
                                    pt.no_of_tablet = obj.no_of_tablet;
                                    db.prescription_table.Add(pt);
                                    db.SaveChanges();

                                    int Insertedpc = Convert.ToInt32(obj.prescription_id);
                                }
                                //Insert Medicine Table
                                var medTablist = db.medicine_table.Where(x => x.Medicine == obj.medicine_name && x.Doctor_id == treat.User_Id).ToList();
                                if (medTablist.Count == 0)
                                {
                                    medicine_table mdt = new medicine_table();
                                    mdt.Doctor_id = treat.User_Id;
                                    mdt.Medicine = obj.medicine_name;
                                    db.medicine_table.Add(mdt);
                                    db.SaveChanges();
                                }
                            }

                        }
                        if (chksms > 0)
                        {
                            sms = sms + Environment.NewLine + "From:Dr." + tr.DocNm + " via DoctorDiary";

                            bool ans = false;

                            int smslength = sms.Length;
                            if (smslength > 140)
                            {
                                smslength = smslength / 140;
                                if (smslength % 140 != 0)
                                {
                                    smslength = smslength + 1;
                                }
                            }
                            else
                            {
                                smslength = 1;
                            }

                            int remcount, rem;

                            ans = SmsSend.Send(tr.Phone, sms);
                            bool isMOnthly;
                            bool isdeliverd;
                            if (ans)
                            {
                                returnData.data1 = "SMS send successfully";
                                isdeliverd = true;
                                if (tr.priority.Equals("Sms"))
                                {
                                    isMOnthly = true;
                                    //int userid = int.Parse(tr.User_Id);
                                    var monthsms = db.monthly_sms.Where(x => x.date.Value.Month == DateTime.Now.Month && x.date.Value.Year == DateTime.Now.Year && x.user_id == userid).Select(x => new { x.id, x.sms_remaining_count }).FirstOrDefault();
                                    if (monthsms.sms_remaining_count > 0)
                                    {
                                        rem = monthsms.sms_remaining_count - smslength;
                                        monthly_sms ms = db.monthly_sms.Where(x => x.id == monthsms.id && x.user_id == userid).FirstOrDefault();
                                        ms.sms_remaining_count = rem;
                                        db.Entry(ms).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    isMOnthly = false;
                                    var packageassigned = db.Package_Assigned.Where(x => x.Last_Date >= DateTime.Now && x.IsActive == true && x.User_id == userid).FirstOrDefault();
                                    if (packageassigned.Package_Remaining_Count > 0)
                                    {
                                        rem = packageassigned.Package_Remaining_Count - smslength;
                                        if (rem <= 0)
                                        {
                                            packageassigned.Package_Remaining_Count = 0;
                                            packageassigned.IsActive = false;
                                            db.Entry(packageassigned).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            packageassigned.Package_Remaining_Count = rem;
                                            db.Entry(packageassigned).State = System.Data.Entity.EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                    }


                                }
                            }
                            else
                            {
                                isdeliverd = false;
                                if (tr.priority.Equals("Sms"))
                                {
                                    isMOnthly = true;
                                }
                                else
                                {
                                    isMOnthly = false;
                                }

                                returnData.data1 = "SMS sending failed";

                            }
                            SMS_Master smsmas = new SMS_Master();
                            smsmas.Date = DateTime.Now;
                            smsmas.isDelivered = isdeliverd;
                            smsmas.isMonthly = isMOnthly;
                            smsmas.Message = sms;
                            smsmas.To_Address = tr.Phone;
                            smsmas.User_id = int.Parse(userid.ToString());
                            db.SMS_Master.Add(smsmas);
                            db.SaveChanges();

                        }
                        else
                        {
                            returnData.data1 = "No SMS selected";
                        }

                        if (Convert.ToBoolean(tr.doctor_country))
                        {
                            var smscount = db.monthly_sms.Where(x => x.date.Value.Month == DateTime.Now.Month && x.date.Value.Year == DateTime.Now.Year && x.user_id == userid).Select(x => new { x.sms_count, x.sms_remaining_count }).FirstOrDefault();
                            if (smscount != null)
                            {
                                pts.Add(new Controllers.PatientCounts { type = "Sms_Count", counts = smscount.sms_count.ToString() });
                                pts.Add(new Controllers.PatientCounts { type = "Sms_Remaining_Count", counts = smscount.sms_remaining_count.ToString() });
                            }
                            else
                            {
                                pts.Add(new Controllers.PatientCounts { type = "Sms_Count", counts = "0" });
                                pts.Add(new Controllers.PatientCounts { type = "Sms_Remaining_Count", counts = "0" });
                            }

                            var packcnt = db.Package_Assigned.Where(x => x.Last_Date >= DateTime.Now && x.IsActive == true && x.User_id == userid).FirstOrDefault();
                            if (packcnt != null)
                            {
                                pts.Add(new Controllers.PatientCounts { type = "Package_Count", counts = packcnt.Package_Count.ToString() });
                                pts.Add(new Controllers.PatientCounts { type = "Package_Remaining_Count", counts = packcnt.Package_Remaining_Count.ToString() });
                            }
                            else
                            {
                                pts.Add(new Controllers.PatientCounts { type = "Package_Count", counts = "0" });
                                pts.Add(new Controllers.PatientCounts { type = "Package_Remaining_Count", counts = "0" });
                            }

                        }
                        else
                        {
                            pts.Add(new Controllers.PatientCounts { type = "Sms_Count", counts = "0" });
                            pts.Add(new Controllers.PatientCounts { type = "Sms_Remaining_Count", counts = "0" });
                            pts.Add(new Controllers.PatientCounts { type = "Package_Count", counts = "0" });
                            pts.Add(new Controllers.PatientCounts { type = "Package_Remaining_Count", counts = "0" });
                        }
                        returnData.data2 = pts;
                        returnData.data3 = treat;
                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        return returnData;

                    }
                    else
                    {
                        returnData.message = "Falied";
                        returnData.status_code = Convert.ToInt32(Status.Failed);
                        return returnData;
                    }
                }
                else
                {
                    returnData.message = "Treatment object not found! ";
                    returnData.status_code = Convert.ToInt32(Status.Failed);
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }

        [HttpPost]
        [ActionName("Insert_User")]
        public ReturnObject Insert_User(csUser cUser)
        {
            ReturnObject returnData = new ReturnObject();
            List<PatientCounts> pts = new List<PatientCounts>();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var usrchk = db.usrs.Where(x => x.Email == cUser.Email).FirstOrDefault();
                    if (usrchk != null)
                    {
                        returnData.data1 = "Email Already Exists";
                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.AlreadyExists);
                        return returnData;
                    }
                    //insert user
                    usr us = new usr();
                    us.Email = cUser.Email;
                    us.Firstname = cUser.Firstname;
                    us.Gender = cUser.Gender;
                    us.Lastname = cUser.Lastname;
                    us.passwd = cUser.passwd;
                    us.IsActive = cUser.IsActive == "true" ? true : false;
                    us.token_id = cUser.token_id;
                    us.Provider = cUser.Provider;
                    us.ProviderId = cUser.ProviderId;
                    db.usrs.Add(us);
                    db.SaveChanges();
                    us.Doctor_Master = null;
                    //db.Entry(us).Reload();

                    Doctor_Master dm = new Doctor_Master();

                    if (us.Id > 0)
                    {
                        dm.Reg_date = DateTime.Now;
                        dm.User_id = us.Id;
                        dm.Gender = cUser.Gender;
                        dm.Doctor_state = cUser.Doctor_state;
                        dm.Doctor_name = cUser.Firstname + " " + cUser.Lastname;
                        dm.Doctor_email = cUser.Email;
                        dm.Doctor_country = cUser.Doctor_country;
                        dm.Doctor_contact = cUser.Doctor_contact;
                        dm.Doctor_city = cUser.Doctor_city;
                        dm.Doctor_address = cUser.Doctor_address;
                        dm.Gender = cUser.Gender;
                        dm.IsActive = cUser.IsActive == "true" ? true : false;//bool.Parse(cUser.IsActive);
                        db.Doctor_Master.Add(dm);
                        db.SaveChanges();
                        db.Entry(dm).Reload();

                        if (dm.Doctor_id > 0)
                        {
                            var unique = new EncryptDecrypt().Encrypt(dm.Doctor_id.ToString());
                            dm.Url = "/Home/Booking?doctorId=" + unique;

                            db.Entry(dm).State = EntityState.Modified;
                            db.SaveChanges();
                            db.Entry(dm).Reload();
                        }

                        //dm.usr = null;

                        //Insert Doctor Shift data
                        if (cUser.doctorShift != null)
                        {
                            cUser.doctorShift.DoctorId = dm.Doctor_id;
                            cUser.doctorShift.CreatedDate = DateTime.Now;
                            cUser.doctorShift.UpdatedDate = DateTime.Now;

                            //returnData = Insert_DoctorShift(cUser.doctorShift, transaction);

                            DoctorShift doctorShift = new MappingService().Map<csDoctorShift, DoctorShift>(cUser.doctorShift);

                            db.DoctorShifts.Add(doctorShift);
                            db.SaveChanges();
                        }

                        Login_Track lt = new Login_Track();
                        lt.User_Id = us.Id;
                        lt.Email = us.Email;
                        lt.Login_Date = DateTime.Now;
                        lt.IsSuccess = true;
                        lt.App_Version = cUser.app_version;
                        db.Login_Track.Add(lt);
                        db.SaveChanges();

                        if (cUser.Doctor_country.Equals("India"))
                        {
                            monthly_sms ms = new monthly_sms();
                            ms.user_id = us.Id;
                            ms.date = DateTime.Now;
                            ms.sms_count = 50;
                            ms.sms_remaining_count = 50;
                            db.monthly_sms.Add(ms);
                            db.SaveChanges();
                        }


                    }

                    transaction.Commit();

                    returnData.data1 = db.usrs.Select(x => new { x.AccountId, x.Email, x.Firstname, x.Lastname, x.Gender, x.IsActive, x.passwd, x.token_id, x.Id }).FirstOrDefault(x => x.Id == us.Id);
                    returnData.data2 = db.Doctor_Master.Select(x => new { x.User_id, x.Clinic_name, x.Doctor_address, x.Doctor_city, x.Doctor_contact, x.Doctor_email, x.Doctor_country, x.Doctor_exp, x.Doctor_id, x.Doctor_name, x.Doctor_photo, x.Doctor_state, x.Gender, x.Reg_date, x.IsActive }).FirstOrDefault(x => x.User_id == dm.Doctor_id);

                    returnData.message = "Successfull";
                    returnData.status_code = Convert.ToInt32(Status.Sucess);
                    return returnData;
                }
                catch (Exception ex)
                {
                    // roll back all database operations, if any thing goes wrong
                    transaction.Rollback();

                    ErrHandler.WriteError(ex.Message, ex);
                    returnData.data1 = ex;
                    returnData.message = "Oops something went wrong! ";
                    returnData.status_code = Convert.ToInt32(Status.Failed);
                    return returnData;
                }
            }
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }

        }
        public List<csSymptoms> SendList(string name)
        {
            List<csSymptoms> symptoms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<csSymptoms>>(name);
            return symptoms;
        }
        public List<csPrescription> SendTransaction(string trans)
        {
            List<csPrescription> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<csPrescription>>(trans);
            return items;
        }

        #region FOR OTP
        /// <summary>
        /// Created by Harshal koshti on 18 July 202
        /// Purpose : Generate OTP for perticular new user.
        /// </summary>
        /// <param name="Email">User EmailId</param>
        /// <param name="MobileNo">User MobileNo</param>
        /// <returns>Return OTP for mobile user</returns>
        [HttpPost]
        [ActionName("SendOtp")]
        public ReturnObject SendOTP(string EmailId = "", string MobileNo = "", string OTP = "")
        {
            ReturnObject returnData = new ReturnObject();
            int currentId = 0;
            OTPVerification model = new OTPVerification();
            OTPVerification result = new OTPVerification();
            try
            {
                if (!string.IsNullOrEmpty(OTP) && OTP.Length > 0)
                {
                    using (ddiarydbEntities db = new ddiarydbEntities())
                    {
                        //This portion check user otp is already available or not for perticular user. it is get based on email and mobile no
                        //var result = db.OTPVerifications.FirstOrDefault(x => x.EmailId.ToLower() == EmailId.ToLower() || x.MobileNo == MobileNo);

                        if (!string.IsNullOrEmpty(EmailId) && EmailId.Length > 0)
                        {
                            result = db.OTPVerifications.FirstOrDefault(x => x.EmailId.ToLower() == EmailId.ToLower());
                        }
                        else if (!string.IsNullOrEmpty(MobileNo) && MobileNo.Length > 0)
                        {
                            result = db.OTPVerifications.FirstOrDefault(x => x.MobileNo == MobileNo);
                        }
                        else
                        {
                            returnData.message = "Enter Email or Mobile no.";
                            returnData.status_code = Convert.ToInt32(Status.Failed);
                            return returnData;
                        }

                        if (result != null)
                        {
                            currentId = Convert.ToInt32(result.Id);
                            result.UpdatedDate = DateTime.Now;
                        }
                        else   //OTP is null no record found for user then create new OTP and send to user.
                        {
                            model.EmailId = EmailId;
                            model.MobileNo = MobileNo;
                            model.OTP = OTP;
                            model.CreatedDate = DateTime.Now;
                            model.UpdatedDate = DateTime.Now;
                        }

                        //Send Email on Email
                        if (!string.IsNullOrEmpty(EmailId))
                        {
                            string body = "Dear user," + Environment.NewLine + " Thank you for using our app." + Environment.NewLine + "Your verification code is: " + OTP;
                            var isSend = Email.MailSend(EmailId, "Doctor Diary App OTP Verification code", body, "", "");
                            if (isSend)
                            {
                                model.IsEmailSend = isSend;
                                if (result != null)
                                    result.IsEmailSend = isSend;
                            }
                        }

                        //Send SMS on mobile number 
                        if (!string.IsNullOrEmpty(MobileNo))
                        {
                            string sms = "Dear user," + Environment.NewLine + " Thank you for using our app." + Environment.NewLine + "Your verification code is: " + OTP;
                            var isSMSSend = SmsSend.Send(MobileNo, sms);
                            if (isSMSSend)
                            {
                                model.IsSMSSend = isSMSSend;
                                if (result != null)
                                    result.IsSMSSend = isSMSSend;
                            }
                        }

                        if (currentId > 0)
                        {
                            result.OTP = OTP;
                            result.UpdatedDate = DateTime.Now;
                            db.Entry<OTPVerification>(result).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            result.OTP = "";
                            returnData.data1 = result;
                        }
                        else
                        {
                            db.OTPVerifications.Add(model);
                            db.SaveChanges();

                            model.OTP = "";
                            returnData.data1 = model;
                        }


                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);

                        if (!string.IsNullOrEmpty(EmailId) && (result.IsEmailSend == false))
                        {
                            returnData.message = "Oops something went wrong! ";
                            returnData.status_code = Convert.ToInt32(Status.Failed);
                        }

                        if (!string.IsNullOrEmpty(MobileNo) && (result.IsSMSSend == false))
                        {
                            returnData.message = "Oops something went wrong! ";
                            returnData.status_code = Convert.ToInt32(Status.Failed);
                        }


                        return returnData;
                    }
                }
                else
                {
                    returnData.message = "Oops something went wrong! ";
                    returnData.status_code = Convert.ToInt32(Status.Failed);
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }
        #endregion


        /// <summary>
        /// Purpose: Check User is available or not
        /// Created By: Vishal Chudasama on 18 Aug 2020
        /// </summary>
        /// <param name="cUser">User</param>
        /// <returns>Message, User, Doctor, Last Login Details</returns>

        [HttpPost]
        [ActionName("SignUpUsingSocialMedia")]
        public ReturnObject SignUpUsingSocialMedia(csUser cUser)
        {
            ReturnObject returnData = new ReturnObject();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var User = db.usrs.Where(x => x.Email == cUser.Email).FirstOrDefault();

                    if (User != null)
                    {
                        var Doctor = db.Doctor_Master.Where(x => x.User_id == User.Id).FirstOrDefault();

                        Login_Track Login = db.Login_Track.Where(x => x.Email == User.Email).OrderByDescending(x => x.Login_Date).FirstOrDefault();

                        if (Login != null)
                        {
                            returnData.data3 = Login;
                        }

                        returnData.message = "You are allready register with this email.";
                        returnData.data1 = User;
                        returnData.data2 = Doctor;
                        returnData.status_code = Convert.ToInt32(Status.AlreadyExists);

                        return returnData;
                    }
                    else
                    {
                        //insert user
                        usr us = new usr();
                        us.Email = cUser.Email;
                        us.Firstname = cUser.Firstname;
                        us.Gender = cUser.Gender;
                        us.Lastname = cUser.Lastname;
                        us.passwd = cUser.passwd;
                        us.IsActive = cUser.IsActive == "true" ? true : false;
                        us.token_id = cUser.token_id;
                        us.Provider = cUser.Provider;
                        us.ProviderId = cUser.ProviderId;
                        db.usrs.Add(us);
                        db.SaveChanges();
                        us.Doctor_Master = null;
                        //db.Entry(us).Reload();

                        Doctor_Master dm = new Doctor_Master();

                        if (us.Id > 0)
                        {
                            dm.Reg_date = DateTime.Now;
                            dm.User_id = us.Id;
                            dm.Gender = cUser.Gender;
                            dm.Doctor_state = cUser.Doctor_state;
                            dm.Doctor_name = cUser.Firstname + " " + cUser.Lastname;
                            dm.Doctor_email = cUser.Email;
                            dm.Doctor_country = cUser.Doctor_country;
                            dm.Doctor_contact = cUser.Doctor_contact;
                            dm.Doctor_city = cUser.Doctor_city;
                            dm.Doctor_address = cUser.Doctor_address;
                            dm.Gender = cUser.Gender;
                            dm.IsActive = cUser.IsActive == "true" ? true : false;//bool.Parse(cUser.IsActive);
                            db.Doctor_Master.Add(dm);
                            db.SaveChanges();
                            db.Entry(dm).Reload();

                            if (dm.Doctor_id > 0)
                            {
                                var unique = new EncryptDecrypt().Encrypt(dm.Doctor_id.ToString());
                                dm.Url = "/Home/Booking?doctorId=" + unique;

                                db.Entry(dm).State = EntityState.Modified;
                                db.SaveChanges();
                                db.Entry(dm).Reload();
                            }

                            //dm.usr = null;

                            //Insert Doctor Shift data
                            if (cUser.doctorShift != null)
                            {
                                cUser.doctorShift.DoctorId = dm.Doctor_id;
                                cUser.doctorShift.CreatedDate = DateTime.Now;
                                cUser.doctorShift.UpdatedDate = DateTime.Now;

                                //returnData = Insert_DoctorShift(cUser.doctorShift, transaction);

                                DoctorShift doctorShift = new MappingService().Map<csDoctorShift, DoctorShift>(cUser.doctorShift);

                                db.DoctorShifts.Add(doctorShift);
                                db.SaveChanges();
                            }


                            Login_Track lt = new Login_Track();
                            lt.User_Id = us.Id;
                            lt.Email = us.Email;
                            lt.Login_Date = DateTime.Now;
                            lt.IsSuccess = true;
                            lt.App_Version = cUser.app_version;
                            db.Login_Track.Add(lt);
                            db.SaveChanges();

                            if (cUser.Doctor_country != null)
                            {
                                if (cUser.Doctor_country.Equals("India"))
                                {
                                    monthly_sms ms = new monthly_sms();
                                    ms.user_id = us.Id;
                                    ms.date = DateTime.Now;
                                    ms.sms_count = 50;
                                    ms.sms_remaining_count = 50;
                                    db.monthly_sms.Add(ms);
                                    db.SaveChanges();
                                }
                            }

                            returnData.data3 = lt;
                            returnData.data2 = dm;
                        }

                        returnData.data1 = us;
                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);

                        transaction.Commit();

                        return returnData;
                    }
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex.Message, ex);
                    returnData.data1 = ex;
                    returnData.message = "Oops something went wrong! ";
                    returnData.status_code = Convert.ToInt32(Status.Failed);

                    // roll back all database operations, if any thing goes wrong
                    transaction.Rollback();

                    return returnData;
                }
            }
        }

        /// <summary>
        /// Created by Vishal Chudasama on 20 July 2020
        /// Purpose : Create record in DoctorShift table
        /// </summary>
        /// <param name="obj">Doctor Shift object</param>
        /// <returns>Return Success or Failed with Message</returns>

        [HttpPost]
        [ActionName("Insert_DoctorShift")]
        public ReturnObject Insert_DoctorShift(csDoctorShift obj, DbContextTransaction transaction)
        {
            ReturnObject returnData = new ReturnObject();

            using (transaction)
            {
                //db.Database.UseTransaction(transaction);
                try
                {
                    csDoctorShift ds = new csDoctorShift();

                    if (obj.DoctorId > 0)
                    {
                        DoctorShift doctorShift = db.DoctorShifts.Where(x => x.DoctorId == obj.DoctorId).FirstOrDefault();

                        if (doctorShift != null)
                        {
                            ds = new MappingService().Map<DoctorShift, csDoctorShift>(doctorShift);

                            returnData.data1 = ds;
                            returnData.message = "Allready Available!";
                            returnData.status_code = Convert.ToInt32(Status.AlreadyExists);
                        }
                        else
                        {
                            doctorShift = new DoctorShift();

                            doctorShift = new MappingService().Map<csDoctorShift, DoctorShift>(obj);
                            doctorShift.CreatedDate = DateTime.Now;
                            doctorShift.UpdatedDate = DateTime.Now;

                            db.DoctorShifts.Add(doctorShift);
                            db.SaveChanges();


                            returnData.data1 = doctorShift;
                            returnData.data2 = transaction;
                            returnData.message = "Successfull";
                            returnData.status_code = Convert.ToInt32(Status.Sucess);
                        }
                    }
                    else
                    {
                        returnData.message = "Enter Doctor id.";
                        returnData.status_code = Convert.ToInt32(Status.Failed);
                    }
                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex.Message, ex);
                    returnData.data1 = ex;
                    returnData.data2 = transaction;
                    returnData.message = "Oops something went wrong! ";
                    returnData.status_code = Convert.ToInt32(Status.Failed);

                    transaction.Rollback();
                }
            }

            return returnData;
        }

        #region Get Current user data
        /// <summary>
        /// This Method is use to get all data using UserID
        /// </summary>
        /// <param name="userId">All login userId</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAllDataByUserId")]
        public ReturnDataList GetCurrentDataByUserId(int userId)
        {
            ReturnDataList returnData = new ReturnDataList();
            List<Patient_Master> modelPatientList = new List<Patient_Master>();
            List<Treatment_Master> modelTreatmentList = new List<Treatment_Master>();
            List<symptom> modelSymptopms = new List<symptom>();
            List<Disease> modelDiseaseList = new List<Disease>();
            List<prescription_table> modelPrescriptionList = new List<prescription_table>();
            List<csAccount> modelAccountList = new List<csAccount>();
            csPatient model = new csPatient();
            try
            {
                var user = db.usrs.AsNoTracking().FirstOrDefault(a => a.Id == userId);
                if (user != null)
                {
                    modelPatientList = db.Patient_Master.AsNoTracking().Where(a => a.User_Id == user.Id).ToList();//.Include("Treatment_Master").Where(a => a.User_Id == user.Id).ToList();
                    foreach (var item in modelPatientList)
                    {
                        var treatList = db.Treatment_Master.AsNoTracking().Where(a => a.Patient_Id == item.Patient_Id).ToList();
                        foreach (var item1 in treatList)
                        {

                            //var listTreatmentImageMaster = db.Treatment_Image_Master.Where(x => x.Treat_crno == item1.Treat_crno).ToList();

                            //foreach (var itemTIM in listTreatmentImageMaster)
                            //{
                            //    item1.Treatment_Image_Master.Add(itemTIM);
                            //}

                            foreach (var itemTIM in item1.Treatment_Image_Master)
                            {
                                itemTIM.Image_Path = (itemTIM.Image_Path != null) ? itemTIM.Image_Path : "";
                                string file = itemTIM.Image_Path != null ? itemTIM.Image_Path : "~\temp\test.txt";

                                if (!System.IO.File.Exists(file))
                                {
                                    itemTIM.Image_Path = string.Format("{0}://{1}{2}{3}{4}",
                                          System.Web.HttpContext.Current.Request.Url.Scheme,
                                          System.Web.HttpContext.Current.Request.Url.Host,
                                          System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port,
                                          System.Web.HttpContext.Current.Request.ApplicationPath,
                                          itemTIM.Image_Path.Replace("~/", string.Empty));
                                }
                                else
                                {
                                    itemTIM.Image_Path = "";
                                }
                            }

                            modelTreatmentList.Add(item1);

                            if (!string.IsNullOrEmpty(item1.symptoms_id))
                            {
                                string[] symptoms = item1.symptoms_id.Split(',');
                                foreach (var item2 in symptoms)
                                {
                                    long sympId = Convert.ToInt64(item2);
                                    var modelSymptopmsList = db.symptoms.AsNoTracking().Where(a => a.id == sympId).ToList();
                                    foreach (var item3 in modelSymptopmsList)
                                    {
                                        modelSymptopms.Add(item3);
                                        modelDiseaseList.AddRange(db.Diseases.AsNoTracking().Where(a => a.id == item3.d_id).ToList());
                                    }
                                }
                            }
                        }

                        // modelPrescriptionList = db.prescription_table.AsNoTracking().Where(a => a.Patient_Id == item.Patient_Id).ToList();
                        var prescriptionList = db.prescription_table.AsNoTracking().Where(a => a.Patient_Id == item.Patient_Id).ToList();
                        foreach (var item4 in prescriptionList)
                        {
                            modelPrescriptionList.Add(item4);
                        }

                        var accountTable = db.account_table.Where(a => a.patient_id == item.Patient_Id).ToList();
                        modelAccountList.AddRange(ConvertAccountdbtolocal(accountTable));
                    }
                }
                returnData.message = "Successfully";
                returnData.status_code = Convert.ToInt32(Status.Sucess);
                returnData.PatientMasterList = Convertpatientdbtolocal(modelPatientList);
                returnData.TreatmentMasterList = ConvertTreatmentdbtolocal(modelTreatmentList);

                foreach (var item in returnData.TreatmentMasterList)
                {
                    var TIM = modelTreatmentList.Where(x => x.Treat_crno == item.Treat_crno).FirstOrDefault();
                    if (TIM.Treatment_Image_Master.Count > 0)
                    {
                        item.treatment_Images = new List<csTritmentImage>();
                        foreach (var itemTIM in TIM.Treatment_Image_Master)
                        {
                            item.treatment_Images.Add(new MappingService().Map<Treatment_Image_Master, csTritmentImage>(itemTIM));
                        }
                        //item.treatment_Images.AddRange(new MappingService().Map<ICollection<Treatment_Image_Master>, List<csTritmentImage>>(TIM.Treatment_Image_Master));
                    }
                }

                returnData.SymptomMasterList = ConvertSymptomsdbtolocal(modelSymptopms);
                returnData.DiseaseMasterList = modelDiseaseList;
                returnData.PrescriptionMasterList = ConvertPrescriptiondbtolocal(modelPrescriptionList);
                returnData.AccountList = modelAccountList;
                return returnData;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
        }
        #endregion

        #region For Synchronize Data
        [HttpPost]
        [ActionName("SynchronizeData")]
        public ReturnObject SynchronizeData(List<csPatient> modelList)
        {
            ReturnObject ObjReturn = new Controllers.ReturnObject();
            List<Disease> modelDiseaseList = new List<Disease>();
            List<symptom> modelSymptomsList = new List<symptom>();
            List<medicine_table> modelMedicineList = new List<medicine_table>();
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<object> modelNewObject = new List<object>();
                    for (int i = 0; i < modelList.Count(); i++)
                    {
                        if (modelList[0].isPatientDelete == true)
                        {
                            //Delete Patient related data
                            ObjReturn = DeletePatientWithAllRecords(modelList[0], transaction);
                            if (ObjReturn.status_code == 1)
                            {
                                continue;
                            }
                            else
                            {
                                return ObjReturn;
                            }
                        }
                        else
                        {
                            var modelPatient = Insert_Patient(modelList[i]);
                            modelList[i].Patient_Id = ((Patient_Master)modelPatient.data1).Patient_Id;

                            //Add Account by Patient id - ModelPatientPayment 

                            if (modelList[i].ModelPatientPayment != null)
                            {
                                modelList[i].ModelPatientPayment.patient_id = modelList[i].Patient_Id;
                                var patientAccount = Insert_Account(modelList[i].ModelPatientPayment);
                            }

                            //End

                            for (int j = 0; j < modelList[i].ModeltreatmentList.Count(); j++)
                            {
                                modelList[i].ModeltreatmentList[j].Patient_Id = Convert.ToString(((Patient_Master)modelPatient.data1).Patient_Id);
                                var treatmentModel = InsertSyncTreatment(modelList[i].ModeltreatmentList[j]);
                                modelList[i].ModeltreatmentList[j].Treat_crno = ((Treatment_Master)treatmentModel.data1).Treat_crno;
                                modelList[i].ModeltreatmentList[j].modelPrescriptions = new List<csPrescription>();
                                modelList[i].ModeltreatmentList[j].modelPrescriptions.AddRange(ConvertPrescriptiondbtolocal((List<prescription_table>)treatmentModel.data2));

                                if ((List<symptom>)treatmentModel.data4 != null)
                                {
                                    modelSymptomsList = (List<symptom>)treatmentModel.data4;
                                }

                                if ((List<Disease>)treatmentModel.data5 != null)
                                {
                                    modelDiseaseList = (List<Disease>)treatmentModel.data5;
                                }

                                if ((List<medicine_table>)treatmentModel.data3 != null)
                                {
                                    modelMedicineList = (List<medicine_table>)treatmentModel.data3;
                                }
                                //foreach (var item in (List<symptom>)treatmentModel.data4)
                                //{
                                //    modelSymptomsList.Add((symptom)item);
                                //}

                                //modelDiseaseList = null;

                                //foreach (var item in (List<Disease>)treatmentModel.data5)
                                //{
                                //    modelDiseaseList.Add((Disease)item);
                                //}

                                //modelMedicineList = null;

                                //foreach (var item in (List<medicine_table>)treatmentModel.data3)
                                //{
                                //    modelMedicineList.Add((medicine_table)item);
                                //}


                            }
                            modelNewObject.Add(modelList[i]);
                        }
                    }

                    ObjReturn.message = "Successfully";
                    ObjReturn.status_code = Convert.ToInt32(Status.Sucess);
                    ObjReturn.data1 = modelNewObject;
                    ObjReturn.data2 = modelSymptomsList;
                    ObjReturn.data3 = modelDiseaseList;
                    ObjReturn.data4 = modelMedicineList;

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrHandler.WriteError(ex.Message, ex);
                    ObjReturn.data1 = ex;
                    ObjReturn.message = "Oops something went wrong! ";
                    ObjReturn.status_code = Convert.ToInt32(Status.Failed);
                    return ObjReturn;
                }
            }
            return ObjReturn;
        }

        private ReturnObject DeletePatientWithAllRecords(csPatient patient, DbContextTransaction transaction)
        {
            ReturnObject returnObject = new ReturnObject();

            using (transaction)
            {

                try
                {
                    if (patient.isPatientDelete == true)
                    {
                        var listPayment = (from x in db.account_table
                                           where x.patient_id == patient.Patient_Id
                                           select x).ToList<account_table>();

                        //db.account_table.RemoveRange(listPayment);
                        //db.SaveChanges();

                        var listTreatment = (from x in db.Treatment_Master
                                             where x.Patient_Id == patient.Patient_Id
                                             select x).ToList<Treatment_Master>();

                        //db.Treatment_Master.RemoveRange(listTreatment);
                        //db.SaveChanges();

                        var listPrescription = (from x in db.prescription_table
                                                where x.Patient_Id == patient.Patient_Id
                                                select x).ToList<prescription_table>();

                        //db.prescription_table.RemoveRange(listPrescription);
                        //db.SaveChanges();

                        var patient_master = db.Patient_Master.Where(x => x.Patient_Id == patient.Patient_Id).FirstOrDefault();
                        //db.Patient_Master.Remove(patient_master);
                    }

                    //transaction.Commit();
                    returnObject.message = "Successfully";
                    returnObject.status_code = Convert.ToInt32(Status.Sucess);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrHandler.WriteError(ex.Message, ex);
                    returnObject.data1 = ex;
                    returnObject.message = "Oops something went wrong! ";
                    returnObject.status_code = Convert.ToInt32(Status.Failed);
                }
            }
            return returnObject;
        }

        /// <summary>
        /// Created by : Harshal Koshti on 8 Aug 2020
        /// purpose : this method is use for save treatment,medicine,presciption,symptoms and diseas when Syncronize method call
        /// Changes By : Vishal Chudasama on 18 Aug 2020
        /// Purpose : 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnObject InsertSyncTreatment(csTreat model)
        {
            ReturnObject returnData = new ReturnObject();
            List<Disease> modelDiseaseList = new List<Disease>();
            List<symptom> modelSymptomsList = new List<symptom>();
            List<prescription_table> modelPrescriptionList = new List<prescription_table>();
            List<medicine_table> modelMedicineList = new List<medicine_table>();

            try
            {
                //var modelTreatment = HttpContext.Current.Request.Form["model"];
                if (model != null)
                {
                    string symptoms_id = "";
                    int chksms = 0;
                    long dsid = 0;
                    List<csSymptoms> symp_name = new List<csSymptoms>();// =  ? SendList(tr.symptoms) : null;
                    csSymptoms modelsym;
                    foreach (var sym in model.symptoms)
                    {
                        modelsym = new csSymptoms();
                        modelsym.name = sym;
                        symp_name.Add(modelsym);

                    }
                    //if (tr.symptoms != null && tr.symptoms != "")
                    //{
                    //    symp_name = SendList(tr.symptoms);
                    //}

                    //NOTE : Here Patient_complain is a Disease
                    if (model.Patient_complain != null && model.Patient_complain != "")
                    {
                        string[] disease_name = model.Patient_complain.Split(',');
                        Disease ds = new Disease();
                        foreach (string dis_name in disease_name)
                        {
                            var disease = db.Diseases.Where(x => x.disease_name == dis_name).Select(x => x.id).FirstOrDefault();
                            if (disease == 0)
                            {

                                ds.disease_name = dis_name;
                                db.Diseases.Add(ds);
                                db.SaveChanges();
                                db.Entry(ds).Reload();
                                modelDiseaseList.Add(ds);

                                if (symp_name.Count != 0 && symp_name != null)
                                {
                                    foreach (csSymptoms sym in symp_name)
                                    {
                                        symptom sm = new symptom();
                                        sm.d_id = ds.id;
                                        sm.name = sym.name;
                                        sm.user_id = long.Parse(model.User_Id);
                                        db.symptoms.Add(sm);
                                        db.SaveChanges();
                                        db.Entry(sm).Reload();

                                        modelSymptomsList.Add(sm);
                                        //db.Entry(sm).Reload();
                                    }

                                }

                            }
                            else
                            {
                                dsid = disease;
                            }

                        }
                    }

                    //Symptopms Table 
                    int j = 0;
                    long userid = long.Parse(model.User_Id);
                    if (model.symptoms != null)
                    {
                        if (symp_name.Count != 0)
                        {
                            foreach (csSymptoms sym in symp_name)
                            {
                                var symt = db.symptoms.Where(x => x.name == sym.name && (x.user_id.Value == userid || x.user_id == null)).Select(x => x.id).FirstOrDefault();
                                if (symt == 0)
                                {
                                    symptom sm = new symptom();
                                    sm.d_id = dsid;//null;//ds.id;
                                    sm.name = sym.name;
                                    sm.user_id = long.Parse(model.User_Id);
                                    db.symptoms.Add(sm);
                                    db.SaveChanges();
                                    db.Entry(sm).Reload();
                                    modelSymptomsList.Add(sm);

                                    if (j == 0)
                                    {
                                        symptoms_id = sm.id.ToString();
                                        j++;
                                    }
                                    else
                                    {
                                        symptoms_id = symptoms_id + "," + sm.id.ToString();
                                    }

                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        symptoms_id = symt.ToString();
                                        j++;
                                    }
                                    else
                                    {
                                        symptoms_id = symptoms_id + "," + symt.ToString();
                                    }
                                }
                            }
                        }
                    }

                    // Insert into Treatment Master
                    Treatment_Master treat = new Treatment_Master();
                    treat.Treat_date = Convert.ToDateTime(model.Treat_date);
                    treat.SPO_two = model.SPO_two;
                    treat.Remarks = model.Remarks;
                    treat.Patient_temp = model.Patient_temp;
                    treat.Patient_pulse = model.Patient_pulse;
                    treat.Patient_image = model.Patient_image;
                    treat.Patient_complain = model.Patient_complain;
                    treat.Patient_Id = int.Parse(model.Patient_Id);
                    treat.Patient_BP = model.Patient_BP;
                    treat.Medicine = model.Medicine;
                    treat.Counsultancyfee = model.Counsultancyfee != "" ? Convert.ToDecimal(model.Counsultancyfee) : 0;
                    treat.Advice = model.Advice;
                    treat.Status = model.Status;
                    treat.Referred_Doctor_Name = model.Referred_Doctor_Name;
                    treat.Referred_Hospital_Detail = model.Referred_Hospital_Detail;
                    treat.Patient_Report_Detail = model.Patient_Report_Detail;
                    treat.User_Id = int.Parse(model.User_Id);
                    treat.Is_Paid = model.Is_Paid == "true" ? true : false; //bool.Parse(tr.Is_Paid);
                    if (model.Follow_Date != "" && model.Follow_Date != null)
                    {
                        treat.Follow_Date = Convert.ToDateTime(treat.Follow_Date);
                    }
                    treat.bill_Amount = Convert.ToDecimal(model.bill_Amount);
                    treat.paid_Amount = Convert.ToDecimal(model.paid_Amount);
                    treat.symptoms_id = symptoms_id;
                    db.Treatment_Master.Add(treat);
                    db.SaveChanges();
                    db.Entry(treat).Reload();

                    //var fileCount = HttpContext.Current.Request.Files;
                    ////This portion is check treatment image folder is exist or not 
                    //var defaultPath = System.Web.HttpContext.Current.Server;
                    //string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/TreatmentImage/");
                    //if (!Directory.Exists(folderPath))
                    //{
                    //    Directory.CreateDirectory(folderPath);
                    //}

                    ////this portion is use get file from requeest and save into treatmentImage folder
                    //Directory.CreateDirectory(folderPath + treat.Treat_crno);
                    //for (int i = 1; i <= fileCount.Count; i++)
                    //{
                    //    csTritmentImage modelImage = new csTritmentImage();
                    //    var file = HttpContext.Current.Request.Files["img" + i];
                    //    if (file != null)
                    //    {
                    //        var filename = Path.GetFileName(file.FileName);
                    //        var path = Path.Combine(folderPath + treat.Treat_crno, treat.Treat_crno + "_" + i + ".png");
                    //        file.SaveAs(path);

                    //        string DbImagePath = ("~/TreatmentImage/" + treat.Treat_crno + "/" + treat.Treat_crno + "_" + i + ".png");
                    //        modelImage.Treat_crno = treat.Treat_crno;
                    //        modelImage.Image_Path = DbImagePath;
                    //        modelImage.CreatedDate = DateTime.Now;
                    //        modelTreatmentImage.Add(modelImage);
                    //    }

                    //}

                    //if (treat.Treat_crno > 0 && modelTreatmentImage.Count > 0)
                    //{
                    //    List<Treatment_Image_Master> modelTreatImage = new List<Treatment_Image_Master>();
                    //    modelTreatImage = ConvertTreatmentImageModel(modelTreatmentImage);
                    //    db.Treatment_Image_Master.AddRange(modelTreatImage);
                    //    db.SaveChanges();
                    //}

                    if (treat.Treat_crno > 0)
                    {
                        List<csPrescription> items = null;// SendTransaction(tr.prescription);modelPrescriptions
                                                          //if (model.prescription != null && model.prescription != "")
                                                          //{
                                                          //    items = SendTransaction(model.prescription);
                                                          //}
                        if (model.modelPrescriptions != null)
                        {
                            foreach (csPrescription obj in model.modelPrescriptions)
                            {

                                prescription_table pt = new prescription_table();
                                pt.Doctor_id = treat.User_Id.Value;
                                pt.Patient_Id = treat.Patient_Id.Value;
                                pt.Treat_crno = treat.Treat_crno;
                                pt.medicine_name = obj.medicine_name;
                                pt.note = obj.note;
                                pt.isMorning = obj.isMorning == "true" ? true : false;
                                pt.isNight = obj.isNight == "true" ? true : false; //bool.Parse(obj.isNight);
                                pt.isNoon = obj.isNoon == "true" ? true : false; //bool.Parse(obj.isNoon);
                                pt.isEvening = obj.isEvening == "true" ? true : false;// bool.Parse(obj.isEvening);
                                pt.no_of_tablet = obj.no_of_tablet;
                                db.prescription_table.Add(pt);
                                db.SaveChanges();
                                db.Entry(pt).Reload();
                                modelPrescriptionList.Add(pt);

                                //Insert Medicine Table
                                var medTablist = db.medicine_table.Where(x => x.Medicine == obj.medicine_name && x.Doctor_id == treat.User_Id).ToList();
                                if (medTablist.Count == 0)
                                {
                                    medicine_table mdt = new medicine_table();
                                    mdt.Doctor_id = treat.User_Id;
                                    mdt.Medicine = obj.medicine_name;
                                    db.medicine_table.Add(mdt);
                                    //mdt.medicine_id = db.SaveChanges();
                                    db.SaveChanges();
                                    db.Entry(mdt).Reload();
                                    modelMedicineList.Add(mdt);
                                }
                            }

                        }

                        returnData.data1 = treat;
                        returnData.data2 = modelPrescriptionList;
                        returnData.data3 = modelMedicineList;
                        returnData.data4 = modelSymptomsList;
                        returnData.data5 = modelDiseaseList;
                        returnData.message = "Successfull";
                        returnData.status_code = Convert.ToInt32(Status.Sucess);
                        return returnData;

                    }
                    else
                    {
                        returnData.message = "Falied";
                        returnData.status_code = Convert.ToInt32(Status.Failed);
                        return returnData;
                    }
                }
                else
                {
                    returnData.message = "Treatment object not found! ";
                    returnData.status_code = Convert.ToInt32(Status.Failed);
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                returnData.data1 = ex;
                returnData.message = "Oops something went wrong! ";
                returnData.status_code = Convert.ToInt32(Status.Failed);
                return returnData;
            }
            return returnData;
        }
        #endregion

        #region Send Prescription SMS

        /// <summary>
        /// Purpose: Send SMS with Priscription details to Patient
        /// Created By: Vishal Chudasama on 22 Aug 2020
        /// </summary>
        /// <param>PrescriptionList</param>
        /// <param>MobileNo</param>
        /// <returns>Send or Not</returns>

        public class paramSendPrescriptoin
        {
            public string patient_number { get; set; }
            public List<csPrescription> PrescriptionMasterList { get; set; }
        }


        [HttpPost]
        [ActionName("SendPrescriptoinSMS")]
        public ReturnObject SendPrescriptoinSMS(paramSendPrescriptoin data)
        {
            ReturnObject result = new ReturnObject();

            if (data.PrescriptionMasterList != null && !string.IsNullOrEmpty(data.patient_number))
            {

                try
                {
                    var patient = (from x in db.Patient_Master.AsEnumerable()
                                   where x.Patient_Id == data.PrescriptionMasterList[0].Patient_Id
                                   select x).FirstOrDefault();


                    string sms = "Hello, " + patient.Patient_name + "\n";

                    var doctorId = data.PrescriptionMasterList[0].Doctor_id;

                    var doctor = (from x in db.Doctor_Master.AsEnumerable()
                                  where x.Doctor_id == doctorId
                                  select x).FirstOrDefault();

                    sms += "Your prescription from Dr." + doctor.Doctor_name + " is mentioned below." + "\n";
                    int med_count = 0;

                    foreach (csPrescription obj in data.PrescriptionMasterList)
                    {
                        med_count++;
                        sms += med_count + ". " + obj.medicine_name;

                        sms += " (";
                        sms += (obj.isMorning.ToLower() == "true") ? "1-" : "0-";
                        sms += (obj.isNight.ToLower() == "true") ? "1-" : "0-";
                        sms += (obj.isNoon.ToLower() == "true") ? "1-" : "0-";
                        sms += (obj.isEvening.ToLower() == "true") ? "1" : "0";
                        sms += ") ";

                        sms += (obj.no_of_tablet.Equals("")) ? "" : "(" + obj.no_of_tablet + ")" + "\n";

                        sms += obj.note.Equals("") ? "" : " (" + obj.note + ") " + "\n";

                    }

                    sms += "From DoctorDiary.";

                    var isSMSSend = SmsSend.Send(data.patient_number, sms);
                    if (isSMSSend)
                    {
                        result.message = "SMS sent successfully to your registered number";
                        result.status_code = Convert.ToInt32(Status.Sucess);
                    }
                    else
                    {
                        result.message = "Oops something went wrong! ";
                        result.status_code = Convert.ToInt32(Status.Failed);
                    }

                }
                catch (Exception ex)
                {
                    ErrHandler.WriteError(ex.Message, ex);
                    result.data1 = ex;
                    result.message = "Oops something went wrong! ";
                    result.status_code = Convert.ToInt32(Status.Failed);
                }
            }
            else
            {
                result.message = "Oops something went wrong! ";
                result.status_code = Convert.ToInt32(Status.Failed);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Purpose: Get list of symptoms, list of Disease and list of Medicines
        /// Created By: Vishal Chudasama on 25 Aug 2020
        /// </summary>
        /// <returns> list of symptoms, list of Disease and list of Medicines </returns>
        /// <param name="userId"> User Id </param>

        [HttpGet]
        [ActionName("GetAllSymptomsDisease")]
        public ReturnObject GetAllSymptomsDisease(int userId = 0)
        {
            ReturnObject result = new ReturnObject();

            try
            {
                //if (userId == 0)
                //{
                //    var listSymptoms = (from x in db.symptoms.AsEnumerable()
                //                        select x).ToList<symptom>();


                //    result.data1 = listSymptoms;
                //}
                //else
                //{
                var listSymptoms = (from x in db.symptoms.AsEnumerable()
                                    where x.user_id == userId || x.user_id == null
                                    select x).ToList<symptom>();


                result.data1 = listSymptoms;
                // }

                var listDisease = db.Diseases.ToList();

                result.data2 = listDisease;

                //if (userId == 0)
                //{
                //    var listMedicines = (from x in db.medicine_table.AsEnumerable()
                //                         where x.Doctor_id > 0 || x.Doctor_id == null
                //                         select x).ToList<medicine_table>();


                //    result.data3 = listMedicines;
                //}
                //else
                //{
                var listMedicines = db.medicine_table.AsNoTracking().Where(a => a.Doctor_id == userId || a.Doctor_id == null).ToList();

                result.data3 = listMedicines;
                //  }

                result.message = "Successfull";
                result.status_code = Convert.ToInt32(Status.Sucess);
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex.Message, ex);
                result.data1 = ex;
                result.message = "Oops something went wrong! ";
                result.status_code = Convert.ToInt32(Status.Failed);
            }
            return result;
        }

        #region Mapping Class
        private List<Treatment_Image_Master> ConvertTreatmentImageModel(List<csTritmentImage> images)
        {
            Config = new AutoMapper.MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<csTritmentImage, Treatment_Image_Master>();
                });
            mapper = Config.CreateMapper();

            var model = mapper.Map<List<Treatment_Image_Master>>(images);
            return model;
        }

        private List<csPatient> Convertpatientdbtolocal(List<Patient_Master> modelList)
        {
            Config = new AutoMapper.MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Patient_Master, csPatient>();
                });
            mapper = Config.CreateMapper();

            var model = mapper.Map<List<csPatient>>(modelList);
            return model;
        }

        private List<csTreat> ConvertTreatmentdbtolocal(List<Treatment_Master> modelList)
        {
            Config = new AutoMapper.MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Treatment_Master, csTreat>();
                    cfg.ValidateInlineMaps = false;
                });
            mapper = Config.CreateMapper();

            var model = mapper.Map<List<csTreat>>(modelList);
            return model;
        }

        private List<csSymptoms> ConvertSymptomsdbtolocal(List<symptom> modelList)
        {
            Config = new AutoMapper.MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<symptom, csSymptoms>();
                });
            mapper = Config.CreateMapper();

            var model = mapper.Map<List<csSymptoms>>(modelList);
            return model;
        }

        private List<csPrescription> ConvertPrescriptiondbtolocal(List<prescription_table> modelList)
        {
            Config = new AutoMapper.MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<prescription_table, csPrescription>();
                    cfg.ValidateInlineMaps = false;
                });
            mapper = Config.CreateMapper();

            var model = mapper.Map<List<csPrescription>>(modelList);
            return model;
        }

        private List<csAccount> ConvertAccountdbtolocal(List<account_table> modelList)
        {
            Config = new AutoMapper.MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<account_table, csAccount>();
                    cfg.ValidateInlineMaps = false;
                });
            mapper = Config.CreateMapper();

            var model = mapper.Map<List<csAccount>>(modelList);
            return model;
        }

        #endregion

    }

    #region All Classes

    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
    enum Status { Sucess = 1, Failed = 0, NotFound = 2, AlreadyExists = 3 };
    public class csSymptomsname
    {
        public int[] id { get; set; }
        public int treat_crno { get; set; }
        public string[] name { get; set; }
    }
    public class ReturnObject
    {
        public string message { get; set; }
        public int status_code { get; set; }
        public object data1 { get; set; }
        public object data2 { get; set; }
        public object data3 { get; set; }
        public object data4 { get; set; }
        public object data5 { get; set; }
        public object data6 { get; set; }
        public object data7 { get; set; }
        public object data8 { get; set; }
        public object data9 { get; set; }
    }

    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FCMResult> results { get; set; }
    }
    public class FCMResult
    {
        public string message_id { get; set; }
    }
    public class PatientCounts
    {
        public string type { get; set; }
        public string counts { get; set; }
    }

    public class csAccount
    {
        //public int patientid { get; set; }
        public int account_id { get; set; }
        public string amount { get; set; }
        public string date { get; set; }
        public string remarks { get; set; }
        public string transaction_type { get; set; }
        public string Doctor_id { get; set; }
        public int patient_id { get; set; }
        public string Treat_crno { get; set; }

        public bool isPaymentDelete { get; set; }
    }
    public class csDoctor
    {
        public int Doctorid { get; set; }
        public int User_id { get; set; }
        public string date { get; set; }
        public string Doctor_state { get; set; }
        public string Doctor_photo { get; set; }
        public string Doctor_name { get; set; }
        public string Doctor_exp { get; set; }
        public string Doctor_email { get; set; }
        public string Doctor_degree { get; set; }
        public string Doctor_country { get; set; }
        public string Doctor_contact { get; set; }
        public string Doctor_city { get; set; }
        public string Doctor_address { get; set; }
        public string Clinic_name { get; set; }
        public string Gender { get; set; }
        public string IsActive { get; set; }
        public string Url { get; set; }

        public csDoctorShift doctorShift { get; set; }
    }
    public class csTreat
    {
        public int Treat_crno { get; set; }
        public int Acc_BillId { get; set; }
        public int Acc_PaidId { get; set; }
        public string Treat_date { get; set; }
        public string SPO_two { get; set; }
        public string Remarks { get; set; }
        public string Patient_temp { get; set; }
        public string Patient_pulse { get; set; }
        public string Patient_image { get; set; }
        public string Patient_complain { get; set; }
        public string Patient_Id { get; set; }
        public string Patient_BP { get; set; }
        public string Medicine { get; set; }
        public string Counsultancyfee { get; set; }
        public string Advice { get; set; }
        public string Status { get; set; }
        public string Referred_Doctor_Name { get; set; }
        public string Referred_Hospital_Detail { get; set; }
        public string Patient_Report_Detail { get; set; }
        public string User_Id { get; set; }
        public string Is_Paid { get; set; }
        public string Follow_Date { get; set; }
        public string bill_Amount { get; set; }
        public string paid_Amount { get; set; }
        public string prescription { get; set; }
        public string[] symptoms { get; set; }
        public string priority { get; set; }
        public string DocNm { get; set; }
        public string Phone { get; set; }
        public string doctor_country { get; set; }
        public string patientName { get; set; }

        public List<csTritmentImage> treatment_Images { get; set; }

        public virtual List<csPrescription> modelPrescriptions { get; set; }

        public bool isTreatmentDelete { get; set; }
    }
    public class csPatient
    {
        public int Patient_Id { get; set; }
        public int User_Id { get; set; }
        public string date { get; set; }
        public string Patient_state { get; set; }
        public string Patient_photo { get; set; }
        public string Patient_name { get; set; }
        public string Patient_email { get; set; }
        public string Patient_country { get; set; }
        public string Patient_contact { get; set; }
        public string Patient_city { get; set; }
        public string Patient_address { get; set; }
        public string note { get; set; }
        public string age { get; set; }
        public DateTime Reg_Date { get; set; }

        public string relation { get; set; }


        public string gender { get; set; }



        public virtual List<csTreat> ModeltreatmentList { get; set; }
        public virtual csAccount ModelPatientPayment { get; set; }


        public bool isPatientDelete { get; set; }

    }
    public class csSymptoms
    {
        public int id { get; set; }
        public int d_id { get; set; }
        public string name { get; set; }
        public string userId { get; set; }

    }
    public class csPrescription
    {
        public string prescription_id { get; set; }
        public string isNoon { get; set; }
        public string isEvening { get; set; }
        public string isMorning { get; set; }
        public string isNight { get; set; }
        public string medicine_name { get; set; }
        public string note { get; set; }
        public string no_of_tablet { get; set; }
        public string send { get; set; }

        public int Patient_Id { get; set; }
        public int Doctor_id { get; set; }
        public int Treat_crno { get; set; }

        // public string tablet_description { get; set; }
        public bool isPrescriptionDelete { get; set; }
    }
    public class csUser
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Gender { get; set; }
        public string Lastname { get; set; }
        public string passwd { get; set; }
        public string IsActive { get; set; }
        public string date { get; set; }
        public string Doctor_state { get; set; }
        public string Doctor_country { get; set; }
        public string Doctor_contact { get; set; }
        public string Doctor_city { get; set; }
        public string Doctor_address { get; set; }
        public string token_id { get; set; }
        public string app_version { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }

        public csDoctorShift doctorShift { get; set; }
    }

    public class csTritmentImage
    {
        public int Id { get; set; }
        public int Treat_crno { get; set; }
        public string Image_Path { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ReturnDataList
    {
        public string message { get; set; }
        public int status_code { get; set; }

        public List<csPatient> PatientMasterList { get; set; }
        public List<csTreat> TreatmentMasterList { get; set; }
        public List<csPrescription> PrescriptionMasterList { get; set; }
        public List<csSymptoms> SymptomMasterList { get; set; }
        public List<Disease> DiseaseMasterList { get; set; }
        public List<csAccount> AccountList { get; set; }
        public object data1 { get; set; }
    }

    public partial class csDoctorShift
    {

        public int Id { get; set; }

        public int DoctorId { get; set; }

        public string MorningStart { get; set; }

        public string MorningEnd { get; set; }

        public string AfternoonStart { get; set; }

        public string AfternoonEnd { get; set; }

        public int Slot { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
    // GET api/Account/UserInfo
    #endregion

}