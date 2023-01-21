using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ProjectCampaigns.Dal;
using ProjectCampaigns.data.sql;
using ProjectCampaigns.Model;

namespace ProjectCampaigns.Entities
{
    public class users
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Dictionary<int, user> dictionsryUser= new Dictionary<int, user>();
        string connectionString = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = campaign ; Data Source = localhost\\sqlEXPRESS";
        public void CreateUsers(string userName, string cellphoneNumber, string email, string UserType, string twitterUsername,string user_id)
        {
            logger.Info("CreateUsers function called : {0} {1} {2} {3} {4} ",  userName, cellphoneNumber, email, UserType, twitterUsername, user_id);

            // Define the insert statement
            string insert = "insert into Usertable (userName, cellphoneNumber, twitterUsername, email, UserType,user_id) OUTPUT INSERTED.userId values (@userName, @cellphoneNumber, @twitterUsername, @email, @UserType,@user_id)";

            try
            {
                // Open a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create a command with the insert statement and the connection
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        if (string.IsNullOrWhiteSpace(userName))
                            throw new ArgumentException("userName cannot be null or empty", nameof(userName));
                        if (string.IsNullOrWhiteSpace(twitterUsername))
                            throw new ArgumentException("twitterUsername cannot be null or empty", nameof(twitterUsername));
                        // Open the connection
                        connection.Open();

                        // Add the user message data as parameters to the command
                        command.Parameters.AddWithValue("@userName", userName);
                        command.Parameters.AddWithValue("@cellphoneNumber", cellphoneNumber);
                        command.Parameters.AddWithValue("@twitterUsername", twitterUsername);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@UserType", UserType);
                        command.Parameters.AddWithValue("@user_id", user_id);

                        int userId = (int)command.ExecuteScalar();
                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "SQL exception occurred: {0}", ex.Message);
                // Handle SQL exceptions
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message);
                // Handle other exceptions
                throw;
            }
        }



        public Dictionary<int, user> UserDetailsfromSQL()
        {
            logger.Info(" UserDetailsfromSQL function called");
            try
            {
                Dictionary<int, user> ret;
                campaingSql.LoadingCampingsDetails("select * from Usertable", ReadUsersFromDb);

                return dictionsryUser;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }



        }


        public void ReadUsersFromDb(SqlDataReader reader)
        {
            logger.Info("ReadUsersFromDb function called");
            //Clear Dictionary Before Inserting Information From Sql Server
            dictionsryUser.Clear();


            while (reader.Read())
            {
                user readUser = new user();

                readUser.userId = reader.GetInt32(0);
                readUser.userName = reader.GetString(1);
                readUser.cellphoneNumber = reader.GetString(2);
                readUser.email = reader.GetString(3);
                readUser.UserType = reader.GetString(4);


                //Cheking If Hashtable contains the key
                if (dictionsryUser.ContainsKey(readUser.userId))
                {
                    //key already exists
                }
                else
                {
                    //Filling a hashtable
                    dictionsryUser.Add(readUser.userId, readUser);
                }


               

            }


        }
        public user GetUserSocialActivistFromDbById(string id)
        {
            logger.Info(" GetUserSocialActivistFromDbById function called");
            data.sql.campaingSql userFromSql = new data.sql.campaingSql();
            user socialActivistNew = (user)userFromSql.LoadOneSocialActivist(id);
            return socialActivistNew;
        }

    }

}

