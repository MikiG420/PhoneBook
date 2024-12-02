using BookPhoneCall;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class MariaDBDatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public MariaDBDatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void AddContact(Contact contact)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Contacts (FirstName, LastName, PhoneNumber, Email) VALUES (@FirstName, @LastName, @PhoneNumber, @Email)";
            using (var command = new MySqlCommand(query, connection))
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
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Contacts";
            using (var command = new MySqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    contacts.Add(new Contact
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        PhoneNumber = reader.GetString("PhoneNumber"),
                        Email = reader.GetString("Email")
                    });
                }
            }
        }
        return contacts;
    }

    public void UpdateContact(Contact contact)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE Contacts SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Email = @Email WHERE Id = @Id";
            using (var command = new MySqlCommand(query, connection))
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
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Contacts WHERE Id = @Id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Contact> SearchContacts(string searchPhrase)
    {
        var contacts = new List<Contact>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = @"SELECT * FROM Contacts 
                             WHERE FirstName LIKE @SearchPhrase 
                             OR LastName LIKE @SearchPhrase 
                             OR PhoneNumber LIKE @SearchPhrase 
                             OR Email LIKE @SearchPhrase";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SearchPhrase", $"%{searchPhrase}%");
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contacts.Add(new Contact
                        {
                            Id = reader.GetInt32("Id"),
                            FirstName = reader.GetString("FirstName"),
                            LastName = reader.GetString("LastName"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            Email = reader.GetString("Email")
                        });
                    }
                }
            }
        }
        return contacts;
    }
}

