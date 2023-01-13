using System;
namespace ClaimApi.Models
{
	public class Claim
	{
		public int ClaimNumber { get; set; }
		public string UCR { get; set; }
		public int ClaimTypeId { get; set; }
        public int CompanyId { get; set; }
        public DateTime ClaimDate { get; set; }
		public DateTime LossDate { get; set; }
		public string AssuredName { get; set; }
		public decimal IncurredLoss { get; set; }
		public bool Closed { get; set; }
	}
}

