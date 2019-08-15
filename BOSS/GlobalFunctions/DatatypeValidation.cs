using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    }
}