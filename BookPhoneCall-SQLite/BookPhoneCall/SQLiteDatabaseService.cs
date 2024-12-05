using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace BookPhoneCall
{
    public class SQLiteDatabaseService : IDatabaseService
    {
        private readonly string _connectionString;
        private readonly string _databaseFileName;

        public SQLiteDatabaseService(string databaseFileName)
        {
            _databaseFileName = databaseFileName;
            _connectionString = $"Data Source={_databaseFileName};Version=3;";
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            if (!File.Exists(_databaseFileName))
            {
                SQLiteConnection.CreateFile(_databaseFileName);
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string createTableQuery = @"
                    CREATE TABLE Contacts (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        PhoneNumber TEXT NOT NULL,
                        Email TEXT NOT NULL
                    );";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Baza danych została utworzona wraz z tabelą Contacts.");
            }
            else
            {
                Console.WriteLine("Baza danych już istnieje.");
            }
        }

        public void AddContact(Contact contact)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Contacts (FirstName, LastName, PhoneNumber, Email) VALUES (@FirstName, @LastName, @PhoneNumber, @Email)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Contact> GetAllContacts()
        {
            var contacts = new List<Contact>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Contacts";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        });
                    }
                }
            }
            return contacts;
        }

        public void UpdateContact(Contact contact)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Email = @Email WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@Id", contact.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteContact(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Contacts WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Contact> SearchContacts(string searchPhrase)
        {
            var contacts = new List<Contact>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM Contacts 
                             WHERE FirstName LIKE @SearchPhrase 
                             OR LastName LIKE @SearchPhrase 
                             OR PhoneNumber LIKE @SearchPhrase 
                             OR Email LIKE @SearchPhrase";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchPhrase", $"%{searchPhrase}%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(new Contact
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Email = reader.GetString(reader.GetOrdinal("Email"))
                            });
                        }
                    }
                }
            }
            return contacts;
        }
    }

}
