using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using ProjectCampaigns.Dal;
using ProjectCampaigns.data.sql;
using ProjectCampaigns.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCampaigns.Entities
{

    public class Twitters

    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public int userMoney = 0;
        string connectionString = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = campaign ; Data Source = localhost\\sqlEXPRESS";

        private static readonly DateTime today = DateTime.Today.AddDays(-1);
        private static readonly DateTime dateOfnextDay = DateTime.Today;
        private static readonly string nextDay = today.ToString("yyyy-MM-dd");
        private static readonly string tomorrow = dateOfnextDay.ToString("yyyy-MM-dd");
        private static readonly string startTime = DateTime.UtcNow.AddHours(-30).ToString("yyyy-MM-ddTHH:mm:ssZ");
        private static readonly string endTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        //  private static readonly string start_time = currentDay + "T00:00:00Z";
        //private static readonly string end_time = tomorrow + "T13:50:00Z";


        public void amountOftweets()
        {
            logger.Info("amountOftweets function called" );
            List <Twitter> userData = MainManager.Instance.Twitter.ReadTwitterFromDb();

            foreach (Twitter user in userData)
            {
                string url = $"https://api.twitter.com/2/tweets/search/recent?start_time={startTime}&end_time={endTime}&query=from:{user.twitterUsername}";

                var clientTwitter = new HttpClient();
                var requestTwitter = new HttpRequestMessage(HttpMethod.Get, url);
                requestTwitter.Headers.Add("authorization", "Bearer AAAAAAAAAAAAAAAAAAAAAIKmlAEAAAAAi4dUwLeoayKuJvRAVvp99%2BNVzXQ%3DSoycLC7ReE1jcWXN4roMBNMjGVuko4HGREzxjtuX3X9kZwzylW");

                try
                {
                    var responseTwitter = clientTwitter.SendAsync(requestTwitter).Result;
                    if (responseTwitter.IsSuccessStatusCode)
                    {
                        var json = JObject.Parse(responseTwitter.Content.ReadAsStringAsync().Result);
                        int tweetCount = 0;
                        int resultCount = (int)json["meta"]["result_count"];
                        if (resultCount != 0)
                        {
                            foreach (var tweet in json["data"])
                            {
                                if (tweet["text"].ToString().Contains(user.hashtag))
                                {
                                    tweetCount++;
                                }
                            }
                        }
                        

                        // Insert user data into the Twitter table
                        using (SqlConnection connection = new SqlConnection("Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = campaign ; Data Source = localhost\\sqlEXPRESS"))
                        {
                            connection.Open();
                            user.userMoney += tweetCount;
                            using (SqlCommand command = new SqlCommand("Update Twitter SET userMoney = @userMoney WHERE twitterUsername = @twitterUsername AND hashtag=@hashtag ", connection))
                            {
                                command.Parameters.AddWithValue("@userMoney", user.userMoney);
                                command.Parameters.AddWithValue("@twitterUsername", user.twitterUsername);
                                command.Parameters.AddWithValue("@hashtag", user.hashtag);

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error connecting to Twitter API: " + ex.Message);
                    throw;
                    // Log the error message
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine("Error parsing JSON response: " + ex.Message);
                    // Log the error message
                    throw;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception occurred: {0}", ex.Message);
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                    // Log the error message
                    throw;
                }
            }
        }

        public void InsertUserSupportToDb(string associationName, string email, string userName, string hashtag, string CampaignName, string twitterUsername)
        {
            logger.Info("InsertnewSaleseToDb function called : {0} {1} {2} {3} {4} ", associationName, email, userName, hashtag, CampaignName, twitterUsername);

            string insert = "INSERT INTO Twitter (associationName, hashtag, email, userName, twitterUsername, CampaignName, userMoney) VALUES (@associationName, @hashtag, @email, @userName, @twitterUsername, @CampaignName, @userMoney)";


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        connection.Open();

                        //Add the user message data as parameters to the command

                        command.Parameters.AddWithValue("@associationName", associationName);
                        command.Parameters.AddWithValue("@hashtag", hashtag);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@userName", userName);
                        command.Parameters.AddWithValue("@twitterUsername", twitterUsername);
                        command.Parameters.AddWithValue("@CampaignName", CampaignName);
                        command.Parameters.AddWithValue("@userMoney", 0);

                        //Execute the command
                        command.ExecuteNonQuery();


                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex, "SQL exception occurred: {0}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Twitter> ReadTwitterFromDb()
        {
            logger.Info("ReadTwitterFromDb function called ");
            string select = "SELECT associationName, hashtag, email, userName, twitterUsername, CampaignName FROM Twitter";
            List<Twitter> TwitterList = new List<Twitter>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(select, connection))
                    {
                        connection.Open();

                        //Execute the command and store the results in a SqlDataReader
                        SqlDataReader reader = command.ExecuteReader();

                        //Read the data from the reader and store it in the campaignList
                        while (reader.Read())
                        {
                            Twitter Twitter = new Twitter();
                            Twitter.associationName = reader["associationName"].ToString();
                            Twitter.hashtag = reader["hashtag"].ToString();
                            Twitter.email = reader["email"].ToString();
                            Twitter.userName = reader["userName"].ToString();
                            Twitter.twitterUsername = reader["twitterUsername"].ToString();
                            Twitter.CampaignName = reader["CampaignName"].ToString();
                            TwitterList.Add(Twitter);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "SQL exception occurred: {0}", ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message);
                Console.WriteLine(ex.Message);
                throw;
            }

            return TwitterList;
        }



        List<Twitter> listTwitterTable = new List<Twitter>();



        public List<Twitter> TwitterTableDetailsfromSQL()
        {
            logger.Info("TwitterTableDetailsfromSQL function called ");
            try
            {
                // Call the LoadingCampingsDetails method to execute the SQL query
                // and pass the ReadbusinessOwnerDetailsFromDb method as a callback
                campaingSql.LoadingCampingsDetails("select * from Twitter ", ReadCampaingsTableDetailsFromDb);

                // Return the dictionary of business owners
                return listTwitterTable;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message);
                // Print the exception message if an error occurs
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public void ReadCampaingsTableDetailsFromDb(SqlDataReader reader)
        {
            logger.Info("ReadCampaingsTableDetailsFromDb function called ");
            try
            {
                // Clear the dictionary before inserting data from the database
                listTwitterTable.Clear();

                // Read each row in the data reader
                while (reader.Read())
                {
                    // Create a new businessOwner object
                    Twitter Twitter = new Twitter();


                    // Initialize the fields of the businessOwner object with data from the database
                    Twitter.userId = reader.GetInt32(0);


                    Twitter.associationName = reader.GetString(1);

                    Twitter.hashtag = reader.GetString(2);
                    Twitter.email = reader.GetString(3);
                    Twitter.userName = reader.GetString(4);
                    Twitter.twitterUsername = reader.GetString(5);
                    Twitter.CampaignName = reader.GetString(6);
                    Twitter.userMoney = reader.GetInt32(7);

                    ;
                    // Check if the dictionary already contains a business owner with the same userId

                    // Add the business owner object to the dictionary
                    listTwitterTable.Add(Twitter);

                }
            }
            catch (Exception ex)
            {
                //

                logger.Error(ex, "Exception occurred: {0}", ex.Message);
                throw;

            }


        }
    }
}










