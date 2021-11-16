﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EncountifyAPI.Data;
using EncountifyAPI.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EncountifyAPI.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string ConnectionString;

        public UsersController(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("EncountifyAPIContext");
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                using SqlCommand command = new SqlCommand(query, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(ParseUser(reader));
                }
            }
            return users;
        }

        /// <summary>
        /// Get user with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public IEnumerable<User> GetUser(int id)
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(ParseUser(reader));
                }
            }
            return users;
        }

        /// <summary>
        /// Get user with specified username
        /// </summary>
        [HttpGet("Username/{username}")]
        public IEnumerable<User> GetUser(string username)
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE CONVERT(VARCHAR, Username) = @username";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(ParseUser(reader));
                }
            }
            return users;
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        [HttpPost("")]
        public IEnumerable<User> AddUser(string username, string password, string email, bool isAdmin = false, string image = "")
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users VALUES (@username, @password, @email, @isAdmin, @image)";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@isAdmin", isAdmin ? 1 : 0);
                command.Parameters.AddWithValue("@image", image);
                command.ExecuteNonQuery();
            }
            return GetUser(username);
        }

        /// <summary>
        /// Edit an existing user
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<User> EditUser(int id, string username = "", string password = "", string email = "", bool? isAdmin = null, string image = "")
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (username != "")
                {
                    IEnumerable<User> user = EditUserName(id, username);
                }
                if (password != "")
                {
                    IEnumerable<User> user = EditUserPassword(id, password);
                }
                if (email != "")
                {
                    IEnumerable<User> user = EditUserEmail(id, email);
                }
                if (isAdmin != null)
                {
                    IEnumerable<User> user = EditUserIsAdmin(id, (bool)isAdmin);
                }
                if (image != "")
                {
                    IEnumerable<User> user = EditUserImage(id, image);
                }
            }
            return GetUser(id);
        }


        /// <summary>
        /// Edit an existing user's username
        /// </summary>
        [HttpPut("{id}/Username")]
        public IEnumerable<User> EditUserName(int id, string username)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Username = @username WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's password
        /// </summary>
        [HttpPut("{id}/Password")]
        public IEnumerable<User> EditUserPassword(int id, string password)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Password = @password WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@password", password);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's email
        /// </summary>
        [HttpPut("{id}/Email")]
        public IEnumerable<User> EditUserEmail(int id, string email)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Email = @email WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@email", email);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's permissions
        /// </summary>
        [HttpPut("{id}/IsAdmin")]
        public IEnumerable<User> EditUserIsAdmin(int id, bool isAdmin)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET IsAdmin = @isAdmin WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@isAdmin", isAdmin);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's image
        /// </summary>
        [HttpPut("{id}/Image")]
        public IEnumerable<User> EditUserImage(int id, string image)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();                
                string query = "UPDATE Users SET Image = @image WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@image", image);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Delete all users
        /// </summary>
        [HttpDelete]
        public IEnumerable<User> DeleteAllUsers()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users";
                using SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            return GetAllUsers();
        }

        /// <summary>
        /// Delete user with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<User> DeleteUser(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        private User ParseUser(SqlDataReader reader)
        {
            User user = new User()
            {
                Id = (int)reader["Id"],
                Username = reader["Username"].ToString(),
                Email = reader["Email"].ToString(),
                Password = reader["Password"].ToString(),
                IsAdmin = Convert.ToBoolean(reader["IsAdmin"]),
                Image = reader["Image"].ToString(),
            };
            return user;
        }
    }
}