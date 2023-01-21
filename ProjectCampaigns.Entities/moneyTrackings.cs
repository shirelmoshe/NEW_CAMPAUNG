using NLog;
using ProjectCampaigns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCampaigns.Entities
{
    public class moneyTrackings
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public List<moneyTracking> GetmoneyTrackingsFromDbById(string id)
        {
            logger.Info("GetmoneyTrackingsFromDbById function called reader: {0}  ", id);
            data.sql.campaingSql userFromSql = new data.sql.campaingSql();
            List<moneyTracking> moneyTrackingsList = userFromSql.LoadAllmoneyTrackings(id);
            return moneyTrackingsList;
        }

    }
}
