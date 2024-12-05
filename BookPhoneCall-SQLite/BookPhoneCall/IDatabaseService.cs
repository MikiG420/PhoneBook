using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPhoneCall
{
    internal interface IDatabaseService
    {
        void AddContact(Contact contact);
        void InitializeDatabase();
        List<Contact> GetAllContacts();
        void UpdateContact(Contact contact);
        void DeleteContact(int id);
        List<Contact> SearchContacts(string searchPhrase);
    }
}
