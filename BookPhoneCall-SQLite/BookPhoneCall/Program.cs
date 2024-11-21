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
            IDatabaseConfig databaseConfig = new DatabaseConfig();
            ContactService contactService = new ContactService(databaseConfig.GetConnectionString());
            Menu menu = new Menu(contactService);
            menu.DisplayMainMenu();
        }
    }
}
