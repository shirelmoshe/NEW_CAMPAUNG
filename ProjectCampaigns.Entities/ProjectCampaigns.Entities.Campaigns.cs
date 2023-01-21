using ProjectCampaigns.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectCampaigns.Model;
using System.Data.SqlClient;
using System.Security.Policy;
using ProjectCampaigns.data.sql;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using NLog;

namespace ProjectCampaigns.Entities
{
	public class Campaigns

	{
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Dictionary<int, Campaign> dictionsryCampaing = new Dictionary<int, Campaign>();
        string connectionString = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = campaign ; Data Source = localhost\\sqlEXPRESS";
        public void InsertUserMessageToDb(string associationName, string email, string uri, string hashtag ,string CampaignName)
		{
            logger.Info("InsertUserMessageToDb function called reader: {0} {1} {2} {3} {4} ", associationName, email, uri, hashtag, CampaignName );

            string insert = "insert into  Campaigns values (@associationName,@email,@uri,@hashtag,@CampaignName)";

            try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					using (SqlCommand command = new SqlCommand(insert, connection))
					{
						connection.Open();

                        //Add the user message data as parameters to the command
                     
                        command.Parameters.AddWithValue("@associationName", associationName);
						command.Parameters.AddWithValue("@email", email);
						command.Parameters.AddWithValue("@uri", uri);
						command.Parameters.AddWithValue("@hashtag", hashtag);
                        command.Parameters.AddWithValue("@CampaignName", CampaignName);

                        //Execute the command
                        command.ExecuteNonQuery();

                      
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
		}

        public void ReadCampingFromDb(SqlDataReader reader)
        {

            //Clear Dictionary Before Inserting Information From Sql Server
            dictionsryCampaing.Clear();


            while (reader.Read())
            {
                Campaign readCampaing = new Campaign();

                readCampaing.usreId = reader.GetInt32(0);
                readCampaing.associationName = reader.GetString(1);
                readCampaing.email = reader.GetString(2);
                readCampaing.uri = reader.GetString(3);
                readCampaing.hashtag = reader.GetString(4);


                //Cheking If Hashtable contains the key
                if (dictionsryCampaing.ContainsKey(readCampaing.usreId))
                {
                    //key already exists
                }
                else
                {
                    //Filling a hashtable
                    dictionsryCampaing.Add(readCampaing.usreId, readCampaing);
                }

            }


        }
        public Dictionary<int, Campaign> CampaignDetailsfromSQL()
        {
            try
            {
                Dictionary<int, Campaign> ret;
                campaingSql.LoadingCampingsDetails("select * from Campaigns", ReadCampingFromDb);

                return dictionsryCampaing;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message);

                Console.WriteLine(ex.Message);
                return null;
                throw;

            }



        }
        Dictionary<int, Campaign> dictionsryCampaingTable = new Dictionary<int, Campaign>();
        public Dictionary<int, Campaign> campaingsTableDetailsfromSQL()
        {
            logger.Info("campaingsTableDetailsfromSQL function called  ");
            try
            {
                // Call the LoadingCampingsDetails method to execute the SQL query
                // and pass the ReadbusinessOwnerDetailsFromDb method as a callback
                campaingSql.LoadingCampingsDetails("select * from Campaigns", ReadCampaingsTableDetailsFromDb);

                // Return the dictionary of business owners
                return dictionsryCampaingTable;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message);
                // Print the exception message if an error occurs
                Console.WriteLine(ex.Message);
                return null;
                throw;
            }
        }


        public void ReadCampaingsTableDetailsFromDb(SqlDataReader reader)
        {
            logger.Info("ReadCampaingsTableDetailsFromDb function called  ");
            try
            {
                // Clear the dictionary before inserting data from the database
                dictionsryCampaingTable.Clear();

                // Read each row in the data reader
                while (reader.Read())
                {
                    // Create a new businessOwner object
                    Campaign readDictionsryCampingDetails = new Campaign();

                    // Initialize the fields of the businessOwner object with data from the database
                    readDictionsryCampingDetails.usreId = reader.GetInt32(0);
                    readDictionsryCampingDetails.associationName = reader.GetString(1);
                    readDictionsryCampingDetails.email = reader.GetString(2);
                    readDictionsryCampingDetails.uri = reader.GetString(3);
                    readDictionsryCampingDetails.hashtag = reader.GetString(4);
                    readDictionsryCampingDetails.CampaignName = reader.GetString(5);
                   ;
                    // Check if the dictionary already contains a business owner with the same userId
                    if (dictionsryCampaingTable.ContainsKey(readDictionsryCampingDetails.usreId))
                    {
                        // Key already exists, log a warning message
                        Console.WriteLine("Warning: Dictionary already contains business owner with userId: " + readDictionsryCampingDetails.usreId);
                    }
                    else
                    {
                        // Add the business owner object to the dictionary
                        dictionsryCampaingTable.Add(readDictionsryCampingDetails.usreId, readDictionsryCampingDetails);
                    }
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

		