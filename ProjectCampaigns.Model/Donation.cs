using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCampaigns.Model
{
   public  class Donation
	{
        public string CompanyName { get; set; }

        public string Product { get; set; }

        public string Email { get; set; }

        public string Price { get; set; }

        public string CampaignName { get; set; }

		public int productsId { get; set; }


	}
}
