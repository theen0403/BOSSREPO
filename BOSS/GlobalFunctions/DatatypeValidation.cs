using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BOSS.GlobalFunctions
{
    public class DatatypeValidation
    {
        public string ReturnConnectionString()
        {
            string con = (ConfigurationManager.ConnectionStrings["DatasetConnectionString"].ConnectionString);
            return con.ToString();
        }
        public string ReturnEmptyString(object data)
        {
            if (data != null)
            {
                return data.ToString();
            }
            return "N/A";
        }
        public string ReturnEmptyDate(object data)
        {
            if (data != DBNull.Value)
            {
                return Convert.ToDateTime(data).ToString("MM-dd-yyyy");
            }
            return "N/A";
        }

        public string ReturnEmptyDateTime(object data)
        {
            if (data != DBNull.Value)
            {
                return Convert.ToDateTime(data).ToString("MM-dd-yyyy hh:mm tt");
            }
            return "N/A";
        }
        public int ReturnEmptyInt(object data)
        {
            if (data != DBNull.Value)
            {
                return Convert.ToInt32(data);
            }
            return 0;
        }
        public byte ReturnEmptyByte(object data)
        {
            if (data != DBNull.Value)
            {
                return Convert.ToByte(data);
            }
            return 0;
        }
        public decimal ReturnEmptyDecimal(object data)
        {
            if (data != DBNull.Value)
            {
                return Convert.ToDecimal(data);
            }
            return 0;
        }
        public bool ReturnEmptyBool(object data)
        {
            if (data != DBNull.Value)
            {
                return Convert.ToBoolean(data);
            }
            return true;
        }
        public string AutoCaps_RemoveSpaces(string data)
        {
            if (data != null)
            {

                data = Regex.Replace(data, @"\s\s+", " ");
                data = Regex.Replace(data, @"^\s+", "");
                data = Regex.Replace(data, @"\s+$", "");
                data = new CultureInfo("en-US").TextInfo.ToTitleCase(data);
                return data.ToString();
            }
            return "N/A";
        }
        public string RemoveSpaces(object data)
        {
            if (data != null)
            {
                var dataString = data.ToString();
                data = Regex.Replace(dataString, @"\s\s+", " ");
                data = Regex.Replace(dataString, @"^\s+", "");
                data = Regex.Replace(dataString, @"\s+$", "");
                return dataString;
            }
            return "N/A";
        }
    }
}