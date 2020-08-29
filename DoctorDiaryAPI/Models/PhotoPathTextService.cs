using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class PhotoPathTextService
    {
        public string IsAvailable(string filePath, string name)
        {
            string file = filePath != null ? (@"" + filePath) : @"~\temp\test.txt";

            if (!System.IO.File.Exists(file))
            {
                if (name.Contains(' '))
                {
                    var temp = name.Split(' ');
                    if (temp.Length > 1)
                    {
                        return temp[0].Substring(0, 1) + temp[1].Substring(0, 1);
                    }
                    else
                    {
                        return temp[0].Substring(0, 1);
                    }
                }
                else
                {
                    return name.Substring(0, 1);
                }
            }
            else
            {
                return filePath;
            }
        }
    }
}