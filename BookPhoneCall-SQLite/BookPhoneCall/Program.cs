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

            //IDatabaseService databaseService = new SQLiteDatabaseService("ContactsDB.db");
            IDatabaseService databaseService = new MariaDBDatabaseService("server=localhost;user=root;password=;database=phonebook;", "phonebook");

            Menu menu = new Menu(databaseService);
            menu.DisplayMainMenu();
        }
    }
}
