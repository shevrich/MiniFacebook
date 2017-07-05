using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Data
{
    public class UserAuthenDb
    {
        private string _connectionString { get; set; }

        public UserAuthenDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string Passward)
        {

            string salt = PasswordHelper.GenerateSalt();
            string passwordHash = PasswordHelper.HashPassword(Passward, salt);
            user.Salt = salt;
            user.PasswordHash = passwordHash;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "INSERT INTO Users (FirstName, LastName, UserName, PasswordHash, Salt) " +
                                       "Values (@firstname, @lastName, @userName, @pwHash, @salt)";
                command.Parameters.AddWithValue("@firstName", user.FirstName);
                command.Parameters.AddWithValue("@lastName", user.LastName);
                command.Parameters.AddWithValue("@userName", user.UserName);
                command.Parameters.AddWithValue("@pwHash", user.PasswordHash);
                command.Parameters.AddWithValue("@salt", user.Salt);
                command.ExecuteNonQuery();
            }
        }
        public User Login(string userName, string password)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                User user = GetByEmail(userName);
                if (user == null)
                {
                    return null;
                }
                bool isCorrectPassword = PasswordHelper.PasswordMatch(password, user.Salt, user.PasswordHash);
                if (!isCorrectPassword)
                {
                    return null;
                }
                return user;
            }
        }

        public User GetByEmail(string userName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users WHERE UserName = @userName";
                command.Parameters.AddWithValue("@userName", userName);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }

                return GetUserFromReader(reader);
            }
        }

        private User GetUserFromReader(SqlDataReader reader)
        {
            User user = new User
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                UserName = (string)reader["UserName"],
                PasswordHash = (string)reader["PasswordHash"],
                Salt = (string)reader["Salt"]
            };
            return user;
        }
    }
}
