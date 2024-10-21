using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookPhoneCall
{
    internal class ContactActions
    {
        public static void AddContact(ContactService contactService)
        {
            Console.Clear();
            Console.WriteLine("Dodaj nowy kontakt:");

            string firstName = GetValidInput("Imię: ");
            string lastName = GetValidInput("Nazwisko: ");
            string phoneNumber = GetValidPhoneNumber();
            string email = GetValidEmail();

            Contact newContact = new Contact
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email
            };

            contactService.AddContact(newContact);
            Console.WriteLine("Kontakt został dodany. Naciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        // Wyświetlanie kontaktów bez zmian
        public static void DisplayContacts(ContactService contactService)
        {
            Console.Clear();
            List<Contact> contacts = contactService.GetAllContacts();

            Console.WriteLine("Lista kontaktów:");
            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.Id}. {contact.FirstName} {contact.LastName} - {contact.PhoneNumber}, {contact.Email}");
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        // Wybieranie kontaktu do usunięcia
        public static void DeleteContact(ContactService contactService)
        {
            Console.Clear();
            List<Contact> contacts = contactService.GetAllContacts();

            if (contacts.Count == 0)
            {
                Console.WriteLine("Brak kontaktów do usunięcia.");
                Console.ReadKey();
                return;
            }

            int selectedIndex = 0;
            Contact selectedContact = NavigateContacts(contacts, ref selectedIndex);

            Console.WriteLine($"\nCzy na pewno chcesz usunąć kontakt: {selectedContact.FirstName} {selectedContact.LastName}? (y/n)");
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                contactService.DeleteContact(selectedContact.Id);
                Console.WriteLine("Kontakt został usunięty.");
            }
            else
            {
                Console.WriteLine("Anulowano.");
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        // Wybieranie kontaktu do aktualizacji
        public static void UpdateContact(ContactService contactService)
        {
            Console.Clear();
            List<Contact> contacts = contactService.GetAllContacts();

            if (contacts.Count == 0)
            {
                Console.WriteLine("Brak kontaktów do aktualizacji.");
                Console.ReadKey();
                return;
            }

            int selectedIndex = 0;
            Contact selectedContact = NavigateContacts(contacts, ref selectedIndex);

            Console.WriteLine($"\nAktualizuj dane kontaktu: {selectedContact.FirstName} {selectedContact.LastName}:");

            string firstName = GetValidInput("Nowe imię (pozostaw puste, aby nie zmieniać): ");
            string lastName = GetValidInput("Nowe nazwisko (pozostaw puste, aby nie zmieniać): ");
            string phoneNumber = GetValidPhoneNumber(true);
            string email = GetValidEmail(true);

            // Aktualizacja tylko tych pól, które zostały podane
            selectedContact.FirstName = string.IsNullOrWhiteSpace(firstName) ? selectedContact.FirstName : firstName;
            selectedContact.LastName = string.IsNullOrWhiteSpace(lastName) ? selectedContact.LastName : lastName;
            selectedContact.PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? selectedContact.PhoneNumber : phoneNumber;
            selectedContact.Email = string.IsNullOrWhiteSpace(email) ? selectedContact.Email : email;

            contactService.UpdateContact(selectedContact);
            Console.WriteLine("Kontakt został zaktualizowany.");
            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }
        // Metoda do poruszania się po liście kontaktów i wybierania kontaktu
        private static Contact NavigateContacts(List<Contact> contacts, ref int selectedIndex)
        {
            bool optionSelected = false;

            while (!optionSelected)
            {
                Console.Clear();
                Console.WriteLine("Wybierz kontakt:");

                for (int i = 0; i < contacts.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"> {contacts[i].FirstName} {contacts[i].LastName} - {contacts[i].PhoneNumber}, {contacts[i].Email}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {contacts[i].FirstName} {contacts[i].LastName} - {contacts[i].PhoneNumber}, {contacts[i].Email}");
                    }
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? contacts.Count - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == contacts.Count - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        optionSelected = true;
                        break;
                }
            }

            return contacts[selectedIndex];
        }

        private static string GetValidPhoneNumber(bool optional = false)
        {
            Regex phoneRegex = new Regex(@"^[0-9\s\-]+$");  // Numer może składać się z cyfr, spacji, myślników

            while (true)
            {
                Console.Write("Numer telefonu" + (optional ? " (pozostaw puste, aby nie zmieniać): " : ": "));
                string phoneNumber = Console.ReadLine();

                if (optional && string.IsNullOrWhiteSpace(phoneNumber))
                    return null;

                if (phoneRegex.IsMatch(phoneNumber))
                    return phoneNumber;
                else
                    Console.WriteLine("Błędny format numeru telefonu. Wprowadź ponownie.");
            }
        }

        private static string GetValidEmail(bool optional = false)
        {
            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");  // Prosta walidacja emaila

            while (true)
            {
                Console.Write("Email" + (optional ? " (pozostaw puste, aby nie zmieniać): " : ": "));
                string email = Console.ReadLine();

                if (optional && string.IsNullOrWhiteSpace(email))
                    return null;

                if (emailRegex.IsMatch(email))
                    return email;
                else
                    Console.WriteLine("Błędny format emaila. Wprowadź ponownie.");
            }
        }

        private static string GetValidInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
