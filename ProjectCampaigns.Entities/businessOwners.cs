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
    public class businessOwners
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // Dictionary to store business owner objects, using the userId as the key
        Dictionary<int, businessOwner> dictionsryUserBusinessOwners = new Dictionary<int, businessOwner>();

        // Method to retrieve business owner details from the database
        public Dictionary<int, businessOwner> businessOwnerDetailsfromSQL()
        {
            logger.Info("businessOwnerDetailsfromSQL function called ");
            try
            {
                // Call the LoadingCampingsDetails method to execute the SQL query
                // and pass the ReadbusinessOwnerDetailsFromDb method as a callback
                campaingSql.LoadingCampingsDetails("SELECT DISTINCT[CampaignName],[userName],[cellphoneNumber],[dbo].[Users].email,[UserType]\r\nFROM Usertable\r\nJOIN [dbo].[Donation] ON  [dbo].[Users].email = [dbo].[Donation].Email\r\nWHERE[dbo].[Users].[userId] = '@[userId]'", ReadbusinessOwnerDetailsFromDb);

                // Return the dictionary of business owners
                return dictionsryUserBusinessOwners;
            }
            catch (Exception ex)
            {
                // Print the exception message if an error occurs
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // Method to process the retrieved data and fill the dictionary with business owner objects
        public void ReadbusinessOwnerDetailsFromDb(SqlDataReader reader)
        {
            logger.Info("ReadbusinessOwnerDetailsFromDb function called reader: {0}", reader);
            try
            {
                // Clear the dictionary before inserting data from the database
                dictionsryUserBusinessOwners.Clear();

                // Read each row in the data reader
                while (reader.Read())
                {
                    // Create a new businessOwner object
                    businessOwner readbusinessOwnerDetails = new businessOwner();

                    // Initialize the fields of the businessOwner object with data from the database
                    readbusinessOwnerDetails.userId = reader.GetInt32(0);
                    readbusinessOwnerDetails.email = reader.GetString(1);
                    readbusinessOwnerDetails.cellphoneNumber = reader.GetString(2);
                    readbusinessOwnerDetails.companyName = reader.GetString(3);
                    readbusinessOwnerDetails.numRowe = reader.GetString(4);

                    // Check if the dictionary already contains a business owner with the same userId
                    if (dictionsryUserBusinessOwners.ContainsKey(readbusinessOwnerDetails.userId))
                    {
                        // Key already exists, log a warning message
                        Console.WriteLine("Warning: Dictionary already contains business owner with userId: " + readbusinessOwnerDetails.userId);
                    }
                    else
                    {
                        // Add the business owner object to the dictionary
                        dictionsryUserBusinessOwners.Add(readbusinessOwnerDetails.userId, readbusinessOwnerDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message); //




            }
        }
    }
}
