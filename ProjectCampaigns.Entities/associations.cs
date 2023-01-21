using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using NLog;
using ProjectCampaigns.Dal;
using ProjectCampaigns.data.sql;
using ProjectCampaigns.Model;
using Logger = NLog.Logger;

namespace ProjectCampaigns.Entities
{
    public  class associations
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        // Dictionary to store business owner objects, using the userId as the key


        // Method to process the retrieved data and fill the dictionary with business owner objects


        public association GetUserFromDbById(int id)
        {
            logger.Info("GetUserFromDbById function called with id: {0}", id);
            data.sql.campaingSql userFromSql = new data.sql.campaingSql();
            association associationtNew = (association)userFromSql.LoadOneAssociation(id);
            return associationtNew;
        }


      

    }
}
