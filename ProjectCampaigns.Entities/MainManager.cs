using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectCampaigns.Dal;
using ProjectCampaigns.Model;
using System.Data.SqlClient;



namespace ProjectCampaigns.Entities
{
    public class MainManager
    {
        private static readonly MainManager instance = new MainManager();
        public static MainManager Instance { get { return instance; } }




        public users userNew = new users();
        public businessOwners businessOwner = new businessOwners();
        public sales sales = new sales();
        public sales Shipments = new sales();
        public Campaigns newcampaing = new Campaigns();
        public Twitters Twitter = new Twitters();
        public sales newsales = new sales();
        public Donation buyNewProduct = new Donation();
        public Donations newDonorDetail = new Donations();
        public associations associations = new associations();
        public moneyTrackings moneyTracking = new moneyTrackings();





        public List<companyOwnerUser> companyOwnerUsersInit()
        {

            companyOwnerUsers usreCompanyOwner = new companyOwnerUsers();
            return usreCompanyOwner.getCompanyOwnerUser();

        }


    }
}







                      










