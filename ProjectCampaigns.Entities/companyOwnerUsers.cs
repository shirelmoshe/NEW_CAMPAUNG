using NLog;
using ProjectCampaigns.Dal;
using ProjectCampaigns.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCampaigns.Entities
{
    public  class companyOwnerUsers
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public List<companyOwnerUser> CompanyOwnerUser { get; set; }

        string insert2 = "select ompanyName,userName,amountOfDonations from companyOwnerUsers";


        public List<companyOwnerUser> getCompanyOwnerUser()
        {
            logger.Info("getCompanyOwnerUser function called");
            SqlQuery query = new SqlQuery();
            try
            {
                bool isConnected = query.Connect();
                if (isConnected)
                {
                    query.runCommand1(insert2, addOwnerUsers);
                }
                return CompanyOwnerUser;
            }
            catch (SqlException ex)
            {
                // Log the exception and its details
                logger.Error(ex, "An error occurred while connecting to the database");
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception and its details
                logger.Error(ex, "An error occurred while retrieving data from the database");
                return null;
            }
          
        }


        public void addOwnerUsers(System.Data.SqlClient.SqlDataReader reader)
        {
            logger.Info("addOwnerUsers function called reader: {0}", reader);
            CompanyOwnerUser = new List<companyOwnerUser>();
            while (reader.Read())
            {
                companyOwnerUser company_Owner_User = new companyOwnerUser() { companyName = reader["companyName"].ToString(), userName = reader["userName"].ToString(), amountOfDonations = int.Parse(reader["amountOfDonations"].ToString()) };
                CompanyOwnerUser.Add(company_Owner_User);
            }


        }
       


    }
}
