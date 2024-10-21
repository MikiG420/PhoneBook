using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BookPhoneCall
{
    internal class DatabaseConfig
    {
        public static string GetConnectionString()
        {
            return "Data Source=ContactsDB.db;Version=3;";
        }
    }
}
