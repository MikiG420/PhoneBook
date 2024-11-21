using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPhoneCall
{
    internal class Menu
    {
        private ContactService contactService;

        public Menu(ContactService contactService)
        {
            this.contactService = contactService;
        }

        public void DisplayMainMenu()
        {
            string[] options = { "Dodaj kontakt", "Wyświetl kontakty", "Usuń kontakt", "Zaktualizuj kontakt", "Szukaj kontaktu", "Wyjdź" };
            int selectedIndex = 0;
            bool running = true;

            while (running)
            {
                selectedIndex = NavigateMenu(options, selectedIndex);

                switch (selectedIndex)
                {
                    case 0:
                        ContactActions.AddContact(contactService);
                        break;
                    case 1:
                        ContactActions.DisplayContacts(contactService);
                        break;
                    case 2:
                        ContactActions.DeleteContact(contactService);
                        break;
                    case 3:
                        ContactActions.UpdateContact(contactService);
                        break;
                    case 4:
                        ContactActions.SearchContacts(contactService);
                        break;
                    case 5:
                        running = false;
                        break;
                }
            }
        }

        private int NavigateMenu(string[] options, int selectedIndex)
        {
            bool optionSelected = false;

            while (!optionSelected)
            {
                Console.Clear();
                Console.WriteLine("Menu kontaktów:");
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;  // Wybrana opcja
                        Console.WriteLine($"> {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {options[i]}");
                    }
                }

                // Obsługa strzałek i enteru
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        optionSelected = true;
                        break;
                }
            }

            return selectedIndex;
        }
    }
}
