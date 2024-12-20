﻿using BookPhoneCall;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class MariaDBDatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    public MariaDBDatabaseService(string connectionString, string databaseName)
    {
        _databaseName = databaseName;
        _connectionString = connectionString;

        InitializeDatabase();
    }

    public void InitializeDatabase()
    {
        var serverConnectionString = _connectionString.Replace($"database={_databaseName};", string.Empty);

        using (var connection = new MySqlConnection(serverConnectionString))
        {
            connection.Open();

            string createDatabaseQuery = $"CREATE DATABASE IF NOT EXISTS `{_databaseName}`;";
            using (var command = new MySqlCommand(createDatabaseQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        InitializeTables();
    }

    private void InitializeTables()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Contacts (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    FirstName VARCHAR(255) NOT NULL,
                    LastName VARCHAR(255) NOT NULL,
                    PhoneNumber VARCHAR(50) NOT NULL,
                    Email VARCHAR(255) NOT NULL
                );";

            using (var command = new MySqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Baza danych i tabela zostały pomyślnie zainicjalizowane (jeśli nie istniały).");
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
            string query = "SELECT * FROM Contacts ORDER BY FirstName ASC";
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

