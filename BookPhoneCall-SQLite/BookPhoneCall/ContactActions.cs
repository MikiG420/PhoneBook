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
        public static void AddContact(IDatabaseService contactService)
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

        public static void DisplayContacts(IDatabaseService contactService)
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

        public static void DeleteContact(IDatabaseService contactService)
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
            if (selectedContact == null)
            {
                Console.WriteLine("Wrócono do menu.");
                return;
            }

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

        public static void UpdateContact(IDatabaseService contactService)
        {
            Console.Clear();
            Console.WriteLine("Aktualizacja kontaktu:");

            List<Contact> contacts = contactService.GetAllContacts();
            if (contacts.Count == 0)
            {
                Console.WriteLine("Brak kontaktów do aktualizacji.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
                Console.ReadKey();
                return;
            }

            int selectedIndex = 0;
            Contact selectedContact = NavigateContacts(contacts, ref selectedIndex);

            if (selectedContact == null)
            {
                Console.WriteLine("Wrócono do menu.");
                return;
            }

            Console.WriteLine($"Aktualizujesz kontakt: {selectedContact.FirstName} {selectedContact.LastName}");
            Console.Write("Nowe imię (pozostaw puste, aby nie zmieniać): ");
            string firstName = GetValidInput("Imię: ");
            Console.Write("Nowe nazwisko (pozostaw puste, aby nie zmieniać): ");
            string lastName = GetValidInput("nazwisko: ");
            Console.Write("Nowy numer telefonu (pozostaw puste, aby nie zmieniać): ");
            string phoneNumber = GetValidPhoneNumber();
            Console.Write("Nowy email (pozostaw puste, aby nie zmieniać): ");
            string email = GetValidEmail();

            if (!string.IsNullOrWhiteSpace(firstName)) selectedContact.FirstName = firstName;
            if (!string.IsNullOrWhiteSpace(lastName)) selectedContact.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(phoneNumber)) selectedContact.PhoneNumber = phoneNumber;
            if (!string.IsNullOrWhiteSpace(email)) selectedContact.Email = email;

            contactService.UpdateContact(selectedContact);
            Console.WriteLine("Kontakt został zaktualizowany. Naciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }
        private static Contact NavigateContacts(List<Contact> contacts, ref int selectedIndex)
        {
            bool optionSelected = false;
            bool exitToMenu = false;

            while (!optionSelected && !exitToMenu)
            {
                Console.Clear();
                Console.WriteLine("Wybierz kontakt (naciśnij ESC, aby wrócić do menu):");

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

                var key = Console.ReadKey(true).Key;

                switch (key)
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
                    case ConsoleKey.Escape:
                        exitToMenu = true;
                        break;
                }
            }

            return exitToMenu ? null : contacts[selectedIndex];
        }

        public static void SearchContacts(IDatabaseService contactService)
        {
            Console.Clear();
            Console.WriteLine("Wprowadź frazę do wyszukania:");

            string searchPhrase = Console.ReadLine();
            var results = contactService.SearchContacts(searchPhrase);

            if (results.Count == 0)
            {
                Console.WriteLine("Brak wyników dla podanej frazy.");
            }
            else
            {
                Console.WriteLine("Znalezione kontakty:");
                foreach (var contact in results)
                {
                    Console.WriteLine($"{contact.Id}. {contact.FirstName} {contact.LastName} - {contact.PhoneNumber}, {contact.Email}");
                }
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        private static string GetValidPhoneNumber(bool optional = false)
        {
            Regex phoneRegex = new Regex(@"^[0-9\s\-]+$");

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
            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

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
