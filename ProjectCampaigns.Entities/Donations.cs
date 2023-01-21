using NLog;
using ProjectCampaigns.Dal;
using ProjectCampaigns.data.sql;
using ProjectCampaigns.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCampaigns.Entities
{
    public class Donations
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // Connection string for the database
        string connectionString = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = campaign ; Data Source = localhost\\sqlEXPRESS";

        // Method to create a new donation in the database
        public void CreateDonation(string CompanyName, string CampaignName, string Product, string Email, string Price)
        {

            logger.Info("CreateDonation function called reader: {0} {1} {2} {3} {4} ", CompanyName, CampaignName, Product, Email, Price);
            // SQL insert statement to insert a new row into the Donation table
            string insert = "insert into Donation values (@CompanyName,@Product,@Email,@Price,@CampaignName)";

            try
            {
                // Use a SqlConnection and SqlCommand to execute the insert statement
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {
                        connection.Open();

                        // Add the donation data as parameters to the command
                        command.Parameters.AddWithValue("@CompanyName", CompanyName);
                        command.Parameters.AddWithValue("@Product", Product);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.Parameters.AddWithValue("@CampaignName", CampaignName);

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "SQL exception occurred: {0}", ex.Message);
                // Handle SQL exceptions
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception occurred: {0}", ex.Message);
                // Handle other exceptions
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Dictionary to store the donations retrieved from the database
        Dictionary<int, Donation> dictionsryproducts = new Dictionary<int, Donation>();

        // Method to retrieve all donations from the database and store them in the dictionary
        public Dictionary<int, Donation> ProductsDetailsfromSQL()
        {
            logger.Info("ProductsDetailsfromSQL function called ");
            try
            {
                // Call the LoadingCampingsDetails method of the campaingSql class to retrieve the donations
                campaingSql.LoadingCampingsDetails("select * from  Donation", ReadCampingFromDb);

                // Return the dictionary of donations
                return dictionsryproducts;
            }
            catch (Exception ex)
            {
                // Print the error message if an exception occurs
                Console.WriteLine(ex.Message);
                return null;
                
            }
        }

        // Method to process each row returned by the query and create a Donation object to represent the donation
        public void ReadCampingFromDb(SqlDataReader reader)
        {
            logger.Info("ReadCampingFromDb function called reader: {0}  ", reader);
            // Clear the dictionary before inserting new information from the database
            dictionsryproducts.Clear();

            // Read each row and create a Donation object to represent the donation
           



                while (reader.Read())
            {
                Donation readProducts = new Donation();

                readProducts.productsId = reader.GetInt32(0);
                readProducts.CompanyName = reader.GetString(1);
                readProducts.Product = reader.GetString(2);
                readProducts.Email = reader.GetString(3);
                readProducts.Price = reader.GetString(4);
                readProducts.CampaignName = reader.GetString(5);
              
              
            
             



                //Cheking If Hashtable contains the key
                if (dictionsryproducts.ContainsKey(readProducts.productsId))
                {
                    //key already exists
                }
                else
                {
                    //Filling a hashtable
                    dictionsryproducts.Add(readProducts.productsId, readProducts);
                }

            }


        }

        public Donation GetProductFromDbById(int id)
        {
            logger.Info("GetProductFromDbById function called reader: {0}  ", id );
            data.sql.campaingSql ProductFromSql  = new data.sql.campaingSql();
            Donation productNew = (Donation)ProductFromSql.LoadOneProduct(id);
            return productNew;
        }

        
    }






}



    

