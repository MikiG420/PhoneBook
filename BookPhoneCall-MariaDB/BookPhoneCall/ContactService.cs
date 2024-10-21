using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BookPhoneCall
{
    internal class ContactService
    {
        private readonly string connectionString;

        public ContactService(string dbConnectionString)
        {
            connectionString = dbConnectionString;
            CreateDatabase();
        }

        public void CreateDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Contacts (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                FirstName TEXT NOT NULL,
                                LastName TEXT NOT NULL,
                                PhoneNumber TEXT,
                                Email TEXT)";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        // Dodawanie kontaktu
        public void AddContact(Contact contact)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Contacts (FirstName, LastName, PhoneNumber, Email) " +
                               "VALUES (@FirstName, @LastName, @PhoneNumber, @Email)";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                command.Parameters.AddWithValue("@LastName", contact.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                command.Parameters.AddWithValue("@Email", contact.Email);
                command.ExecuteNonQuery();
            }
        }

        // Pobieranie wszystkich kontaktów
        public List<Contact> GetAllContacts()
        {
            List<Contact> contacts = new List<Contact>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, FirstName, LastName, PhoneNumber, Email FROM Contacts";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contact contact = new Contact
                        {
                            Id = Convert.ToInt32(reader["Id"]),  // Bezpieczna konwersja na int
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"]?.ToString(),  // Może być null, więc używamy null-conditional operator
                            Email = reader["Email"]?.ToString()  // Może być null
                        };
                        contacts.Add(contact);
                    }
                }
            }

            return contacts;
        }

        // Usuwanie kontaktu po ID
        public void DeleteContact(int contactId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Contacts WHERE Id = @Id";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", contactId);
                command.ExecuteNonQuery();
            }
        }

        // Aktualizacja kontaktu
        public void UpdateContact(Contact contact)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, " +
                               "PhoneNumber = @PhoneNumber, Email = @Email WHERE Id = @Id";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                command.Parameters.AddWithValue("@LastName", contact.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                command.Parameters.AddWithValue("@Email", contact.Email);
                command.Parameters.AddWithValue("@Id", contact.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}
