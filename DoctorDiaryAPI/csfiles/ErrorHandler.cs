using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DoctorDiaryAPI
{
    public class ErrHandler
    {

        //public static void WriteError(string errorMessage, Exception ex)
        //{
        //    try
        //    {
        //        String path = "~/Error/" + DateTime.Today.ToString("dd-MMM-yy") + ".txt";

        //        if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
        //            File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();

        //        StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path));

        //        try
        //        {


        //            StackTrace trace = new StackTrace(ex, true);

        //            StringBuilder err = new StringBuilder();
        //            err.Append("Log Entry : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + Environment.NewLine);
        //            err.Append("Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() + Environment.NewLine);
        //            err.Append("Inner Exception : " + (ex.InnerException != null ? ex.InnerException.Message : "") + Environment.NewLine);
        //            err.Append("Inner Exception Detail: " + (ex.InnerException != null ? ex.InnerException.InnerException.ToString() : "") + Environment.NewLine);
        //            err.Append("File Name : " + trace.GetFrame(0).GetFileName() + Environment.NewLine);
        //            err.Append("Line : " + trace.GetFrame(0).GetFileLineNumber() + Environment.NewLine);
        //            err.Append("Column: " + trace.GetFrame(0).GetFileColumnNumber() + Environment.NewLine);
        //            err.Append("Error Message: " + ex.Message + Environment.NewLine);
        //            err.Append("Source: " + ex.Source + Environment.NewLine);
        //            err.Append("StackTrace: " + ex.StackTrace + Environment.NewLine);
        //            err.Append("TargetSite:" + Convert.ToString(ex.TargetSite));

        //            w.WriteLine("\r\n" + err);

        //            w.WriteLine("__________________________");
        //            w.Flush();
        //            w.Close();

        //            //DataSet dsEmailTemplate;



        //        }
        //        catch (Exception EX)
        //        {
        //            WriteError("", EX);
        //        }
        //        finally
        //        {
        //            w.Close();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        //public static void Writelog(string log)
        //{
        //    try
        //    {
        //        String path = "~/log/" + DateTime.Today.ToString("dd-MMM-yy") + ".txt";

        //        if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
        //            File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();

        //        StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path));

        //        try
        //        {   //StackTrace trace = new StackTrace(ex, true);

        //            StringBuilder err = new StringBuilder();
        //            err.Append("Log Entry : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + Environment.NewLine);
        //            err.Append("Log Details : " + log.ToString() + Environment.NewLine);
        //            w.WriteLine("\r\n" + err);
        //            w.WriteLine("__________________________");
        //            w.Flush();
        //            w.Close();
        //        }
        //        catch (Exception EX)
        //        {
        //            WriteError("", EX);
        //        }
        //        finally
        //        {
        //            w.Close();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        public static void WriteError(string errorMessage, Exception ex)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/Error/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += @"err_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

                // Create the file if it does not exist.
                if (!File.Exists(path))
                {
                    // Create the file.
                    FileStream fs = File.Create(path);
                    fs.Close();
                }

                StreamWriter w = File.AppendText(path);

                try
                {


                    StackTrace trace = new StackTrace(ex, true);

                    StringBuilder err = new StringBuilder();
                    err.Append("Log Entry : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + Environment.NewLine);
                    err.Append("Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() + Environment.NewLine);
                    err.Append("Inner Exception : " + (ex.InnerException != null ? ex.InnerException.Message : "") + Environment.NewLine);
                    err.Append("Inner Exception Detail: " + (ex.InnerException != null ? ex.InnerException.InnerException.ToString() : "") + Environment.NewLine);
                    err.Append("File Name : " + trace.GetFrame(0).GetFileName() + Environment.NewLine);
                    err.Append("Line : " + trace.GetFrame(0).GetFileLineNumber() + Environment.NewLine);
                    err.Append("Column: " + trace.GetFrame(0).GetFileColumnNumber() + Environment.NewLine);
                    err.Append("Error Message: " + ex.Message + Environment.NewLine);
                    err.Append("Source: " + ex.Source + Environment.NewLine);
                    err.Append("StackTrace: " + ex.StackTrace + Environment.NewLine);
                    err.Append("TargetSite:" + Convert.ToString(ex.TargetSite));

                    w.WriteLine("\r\n" + err);

                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();

                    //DataSet dsEmailTemplate;



                }
                catch (Exception EX)
                {
                    WriteError("", EX);
                }
                finally
                {
                    w.Close();
                }
            }
            catch
            {
            }
        }
        public static void Writelog(string log)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/Error/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += @"log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

                // Create the file if it does not exist.
                if (!File.Exists(path))
                {
                    // Create the file.
                    FileStream fs = File.Create(path);
                    fs.Close();
                }

                StreamWriter w = File.AppendText(path);

                //StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path));

                try
                {   //StackTrace trace = new StackTrace(ex, true);

                    StringBuilder err = new StringBuilder();
                    err.Append("Log Entry : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + Environment.NewLine);
                    err.Append("Log Details : " + log.ToString() + Environment.NewLine);
                    w.WriteLine("\r\n" + err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
                catch (Exception EX)
                {
                    WriteError("", EX);
                }
                finally
                {
                    w.Close();
                }
            }
            catch
            {
            }
        }


        public static string DefaultImagePath(string Path)
        {
            if (File.Exists(Path))
            {
                return Path;
            }
            else
            {
                return "~/userimage/defaultimge.jpg";
            }
        }
    }
}