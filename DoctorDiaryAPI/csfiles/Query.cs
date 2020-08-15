using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Net;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace DoctorDiaryAPI
{
    public class Query
    {

        SqlConnection conn;
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter adp;
        int i;
        string col, val, cndtion;

        public object Insert(string[] values, string tableNm, string[] columns, int identity)
        {
            using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ddiarydbEntities"].ConnectionString))
            {
                col = "";
                val = "";
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                for (i = 0; i < columns.Length; i++)
                {
                    if (i == columns.Length - 1)
                    {
                        col = col + columns[i];
                        val = val + "@" + columns[i];
                    }
                    else
                    {
                        col = col + columns[i] + ",";
                        val = val + "@" + columns[i] + ",";
                    }
                    cmd.Parameters.AddWithValue("@" + columns[i], values[i]); 
                }
                string s = "insert into " + tableNm + "(" + col + ") values (" + val + ")";                              
                cmd.CommandText = s;
                if (identity == 1)
                {
                    cmd.ExecuteNonQuery();
                    SqlCommand cmdtest = new SqlCommand("Select @@identity", conn);
                    return cmdtest.ExecuteScalar();

                    
                }
                else
                {
                    return cmd.ExecuteNonQuery();

                }
                
            }
            
            //return ExecuteProcedureIUD(s);
        }
        public int delete(string tableNm, string id)
        {
            string s = "delete from " + tableNm + " " + id + "";
            return ExecuteProcedureIUD(s);
        }
        public object Update(string[] columns, string tableNm, string id, string[] values,int identity)
        {
            using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString))
            {
                col = "";
                val = "";
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                for (i = 0; i < columns.Length; i++)
                {
                    if (i == columns.Length - 1)
                    {
                        col = col + columns[i] + "=@" + columns[i];
                        
                    }
                    else
                    {
                        col = col + columns[i] + "=@" + columns[i]+",";
                        
                    }
                    cmd.Parameters.AddWithValue("@" + columns[i], values[i]);
                }
                string s = "update " + tableNm + " set " + col + " "+id;
                cmd.CommandText = s;
                if (identity == 1)
                {
                    cmd.ExecuteNonQuery();
                    SqlCommand cmdtest = new SqlCommand("Select @@identity", conn);
                    return cmdtest.ExecuteScalar();


                }
                else
                {
                    return cmd.ExecuteNonQuery();

                }

            }

        }

        public object Select(string[] columns, string[] condition,string tableNm,string[] values)
        {
            using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString))
            {
                col = "";
                val = "";
                cndtion = "";
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                for (i = 0; i < columns.Length; i++)
                {
                    if (i == columns.Length - 1)
                    {
                        col = col + columns[i];
                        cndtion = condition[i]+"=@"+condition[i];
                    }
                    else
                    {
                        col = col + columns[i] + ",";
                        cndtion = condition[i] + "=@" + condition[i];
                        
                    }
                    cmd.Parameters.AddWithValue("@" + condition[i], values[i]);
                }
                string s = "select " + col + " from " + tableNm + " where ";
                cmd.CommandText = s;                
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                return dt;
                

            }

            
        }

        public DataTable ExecuteProcedureSelect(string strproc)
        {
            using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString))
            {

                conn.Open();
                cmd = new SqlCommand();
                dt = new DataTable();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strproc;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                return dt;
            }
        }
        public int ExecuteProcedureIUD(string strproc)
        {
            using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString))
            {
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strproc;
                //if (param != null)
                //{
                //    cmd.Parameters.AddRange(param);
                //}
                return cmd.ExecuteNonQuery();
            }
        }
        public object ExecuteScalar(string strproc)
        {
            using (conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ark_MediplusConnectionString"].ConnectionString))
            {
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strproc;
                //if (param != null)
                //{
                //    cmd.Parameters.AddRange(param);
                //}
                return cmd.ExecuteScalar();
            }
        }
    }
}