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
            ContactService contactService = new ContactService(DatabaseConfig.GetConnectionString());
            Menu menu = new Menu(contactService);
            menu.DisplayMainMenu();
        }
    }
}
