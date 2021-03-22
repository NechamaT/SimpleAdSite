using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimpleAdAuth.Data
{
    public class SimpleAdAuthDb
    {
        private readonly string _connectionString;

        public SimpleAdAuthDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Users(Name, Email, HashedPassword)" +
                              " VALUES (@name, @email, @hashedPassword)";
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hashedPassword", user.HashedPassword);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public User GetUserByEmail(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users" +
                              " WHERE Email = @email";
            conn.Open();
            cmd.Parameters.AddWithValue("@email", email);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int) reader["Id"],
                Name = (string) reader["Name"],
                Email = (string) reader["Email"],
                HashedPassword = (string) reader["HashedPassword"]
            };

        }

        public User Login(string password, string email)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
            return isValid ? user : null;
        }

        public void NewAd(Ad ad)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Posts" +
                              " VALUES(GETDATE(), @text, @UserId)";
            cmd.Parameters.AddWithValue("@text", ad.Text);
            cmd.Parameters.AddWithValue("@UserId", ad.UserId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Ad> GetAllAds()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT p.*, u.Name FROM Posts p
                                JOIN Users u
                                ON p.UserId = u.Id";

            conn.Open();
            List<Ad> results = new List<Ad>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new Ad
                {
                    Id = (int) reader["Id"],
                    DateCreated = (DateTime) reader["DateCreated"],
                    PhoneNumber = (string) reader["PhoneNumber"],
                    Text = (string) reader["Text"],
                    UserId = (int) reader["UserId"],
                    Name = (string) reader["Name"]
                });
                
            }

            return results;
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Posts" +
                              " WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }



}





