using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BookPhoneCall
{
    internal class Program
    {
        static void Main()
        {

            IDatabaseService databaseService = new SQLiteDatabaseService("Data Source=ContactsDB.db;Version=3;");

            //IDatabaseService databaseService = new MariaDBDatabaseService("Server=localhost;Database=phonebook;User=root;Password=;");

            Menu menu = new Menu(databaseService);
            menu.DisplayMainMenu();
        }
    }
}
