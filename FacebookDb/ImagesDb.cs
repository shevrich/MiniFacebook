using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Data
{
    public class ImagesDb
    {
        private string _connectionString { get; set; }

        public ImagesDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Image> GetPopularImages()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT TOP 5 * from Images Order By Views Desc";
                List<Image> images = new List<Image>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Image i = new Image();
                    i.Id = (int)reader["Id"];
                    i.FileName = (string)reader["FileName"];
                    i.FirstName = (string)reader["FirstName"];
                    i.LastName = (string)reader["LastName"];
                    i.DateUploaded = (DateTime)reader["DateUploaded"];
                    if (!DBNull.Value.Equals(reader["Views"]))
                    {
                        i.Views = (int)reader["Views"];
                    }
                    else
                    {
                        i.Views = 0;
                    }
                    images.Add(i);
                }
                return images;
            }
        }

        public IEnumerable<Image> GetRecentImages()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT TOP 5 * from Images ORDER BY DateUploaded Desc";
                List<Image> images = new List<Image>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Image i = new Image();
                    i.Id = (int)reader["Id"];
                    i.FileName = (string)reader["FileName"];
                    i.FirstName = (string)reader["FirstName"];
                    i.LastName = (string)reader["LastName"];
                    i.DateUploaded = (DateTime)reader["DateUploaded"];
                    if (!DBNull.Value.Equals(reader["Views"]))
                    {
                        i.Views = (int)reader["Views"];
                    }
                    else
                    {
                        i.Views = 0;
                    }
                    images.Add(i);
                }
                return images;
            }

        }

        public IEnumerable<Image> GetLikedImages()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "select TOP 5 *, COUNT(ImageId) as Count from Images i Join Likes l " +
                "On l.ImageId = i.Id Group By i.Id, i.FileName, i.FirstName, i.LastName, i.DateUploaded, i.Views, " + 
                "l.ImageId, l.UserId ORDER BY Count DESC";
                List<Image> images = new List<Image>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Image i = new Image();
                    i.Id = (int)reader["Id"];
                    i.FileName = (string)reader["FileName"];
                    i.FirstName = (string)reader["FirstName"];
                    i.LastName = (string)reader["LastName"];
                    i.DateUploaded = (DateTime)reader["DateUploaded"];
                    if (!DBNull.Value.Equals(reader["Views"]))
                    {
                        i.Views = (int)reader["Views"];
                    }
                    else
                    {
                        i.Views = 0;
                    }
                    i.LikesCount = GetImageLikedCount(i.Id);
                    images.Add(i);
                }
                return images;
            }
        }
   
        public void AddUpload(Image i, string fileName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "Insert into IMAGES (FileName, FirstName, LastName, DateUploaded) " +
                    "Values (@fileName, @firstName, @lastName, @dateUploaded)";
                command.Parameters.AddWithValue("@fileName", fileName);
                command.Parameters.AddWithValue("@firstName", i.FirstName);
                command.Parameters.AddWithValue("@lastName", i.LastName);
                command.Parameters.AddWithValue("@dateUploaded", i.DateUploaded);
                command.ExecuteNonQuery();
            }
        }

        public Image GetById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT * from Images WHERE Id = @id";
                command.Parameters.AddWithValue("@id", Id);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                Image i = new Image();
                i.Id = (int)reader["Id"];
                i.FileName = (string)reader["FileName"];
                i.FirstName = (string)reader["FirstName"];
                i.LastName = (string)reader["LastName"];
                i.DateUploaded = (DateTime)reader["DateUploaded"];
                if (!DBNull.Value.Equals(reader["Views"]))
                {
                    i.Views = (int)reader["Views"];
                }
                else
                {
                    i.Views = 0;
                }
                i.LikesCount = GetImageLikedCount(i.Id);
                return i;
            }
        }

        public Image GetByFileName(string fn)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT * from Images WHERE FileName = @fn";
                command.Parameters.AddWithValue("@fn", fn);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                Image i = new Image();
                i.Id = (int)reader["Id"];
                i.FileName = (string)reader["FileName"];
                i.FirstName = (string)reader["FirstName"];
                i.LastName = (string)reader["LastName"];
                i.DateUploaded = (DateTime)reader["DateUploaded"];
                if (!DBNull.Value.Equals(reader["Views"]))
                {
                    i.Views = (int)reader["Views"];
                }
                else
                {
                    i.Views = 0;
                }
                return i;
            }
        }

        public int GetViews(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT Views from Images WHERE Id = @id";
                command.Parameters.AddWithValue("@id", Id);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int count = 0;
                if (!DBNull.Value.Equals(reader["Views"]))
                {
                    count = (int)reader["Views"];
                }
                return count;
            }

        }

        public void UpdateImageViews(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                int count = GetViews(Id);
                count += 1;
                command.CommandText = "Update Images Set Views = @count Where Id = @id";
                command.Parameters.AddWithValue("@count", count);
                command.Parameters.AddWithValue("@id", Id);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Image> GetLikesPerUser(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT i.Id, FileName, Views, i.FirstName, i.LastName, DateUploaded, COUNT(ImageId) FROM Images i " +
                "join Likes l on l.ImageId = i.Id join Users u on u.Id = l.UserId WHERE u.Id = @id Group By i.Id, i.FileName, i.FirstName, i.LastName, i.DateUploaded, i.Views, " +
                "l.ImageId, l.UserId";
                command.Parameters.AddWithValue("@id", Id);
                List<Image> images = new List<Image>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Image i = new Image();
                    i.Id = (int)reader["Id"];
                    i.FileName = (string)reader["FileName"];
                    i.FirstName = (string)reader["FirstName"];
                    i.LastName = (string)reader["LastName"];
                    i.DateUploaded = (DateTime)reader["DateUploaded"];
                    if (!DBNull.Value.Equals(reader["Views"]))
                    {
                        i.Views = (int)reader["Views"];
                    }
                    else
                    {
                        i.Views = 0;
                    }
                    i.LikesCount = GetImageLikedCount(i.Id);
                    images.Add(i);
                }
                return images;
            }

        }

        public void LikeImage(int UserId, int ImageId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "Insert into Likes (UserId, ImageId) " +
                    "Values (@userId, @imageId)";
                command.Parameters.AddWithValue("@userId", UserId);
                command.Parameters.AddWithValue("@imageId", ImageId);
                command.ExecuteNonQuery();
            }
        }

        public int GetImageLikedCount(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT COUNT(ImageId) As Count from Likes where ImageId = @imageId";
                command.Parameters.AddWithValue("@imageId", Id);
                int count = 0;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count =(int)reader["Count"];
                }
                return count;
            }
            }

        public bool CheckIfUserAlreadyLikedThis(int ImageId, int UserId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT * FROM Likes Where ImageId = @imageId AND UserId = @userId";
                command.Parameters.AddWithValue("@imageId", ImageId);
                command.Parameters.AddWithValue("@userId", UserId);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
        }
        
    }
}
