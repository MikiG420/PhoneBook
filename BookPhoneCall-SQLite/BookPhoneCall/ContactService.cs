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

        public ContactService(string databaseConfig)
        {
            connectionString = databaseConfig;
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
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"]?.ToString(),
                            Email = reader["Email"]?.ToString()  
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

        public List<Contact> SearchContacts(string searchPhrase)
        {
            var contacts = new List<Contact>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Zapytanie SQL do wyszukiwania we wszystkich kolumnach
                string query = @"SELECT Id, FirstName, LastName, PhoneNumber, Email 
                         FROM Contacts 
                         WHERE FirstName LIKE @SearchPhrase 
                         OR LastName LIKE @SearchPhrase 
                         OR PhoneNumber LIKE @SearchPhrase 
                         OR Email LIKE @SearchPhrase";

                using (var command = new SQLiteCommand(query, connection))
                {
                    // Dodanie parametru z frazą do wyszukania (LIKE '%fraza%')
                    command.Parameters.AddWithValue("@SearchPhrase", "%" + searchPhrase + "%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contact = new Contact
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Email = reader.GetString(reader.GetOrdinal("Email"))
                            };
                            contacts.Add(contact);
                        }
                    }
                }
            }

            return contacts;
        }

    }
}
