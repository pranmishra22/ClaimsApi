using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClaimApi.Models;
using Microsoft.AspNetCore.Hosting.Server;

namespace ClaimApi.Repository
{
	public class CompanyClaimRepository : ICompanyClaimRepository
    {
        private static List<Company> CompanyClaimList;

        public Company GetCompanyById(int company_id)
        {
            if (company_id == 0)
                return null;

            if (CompanyClaimList == null || CompanyClaimList.Count == 0)
                GetOrUpdateDetails(CompanyClaimList);

            Company company = CompanyClaimList.FirstOrDefault(m => m.Id == company_id);
            return company;
        }

        public IEnumerable<Claim> GetCompanyClaims(int company_id)
        {
            if (company_id == 0)
                return null;

            if (CompanyClaimList == null || CompanyClaimList.Count == 0)
                GetOrUpdateDetails(CompanyClaimList);

            List<Claim> claims = CompanyClaimList.FirstOrDefault(m => m.Id == company_id) != null ? CompanyClaimList.FirstOrDefault(m => m.Id == company_id).claims.ToList() : new List<Claim>();
            return claims;
        }

        public Claim GetClaimDetailsByClaimNumber(int company_id, int claim_id)
        {
            if (company_id == 0 || claim_id == 0)
                return null;

            if (CompanyClaimList == null || CompanyClaimList.Count == 0)
                GetOrUpdateDetails(CompanyClaimList);

            Company company = CompanyClaimList.FirstOrDefault(m => m.Id == company_id);
            Claim claim = null;
            if (company != null)
            {
                claim = company.claims.FirstOrDefault(m => m.ClaimNumber == claim_id);
            }
            return claim;
        }

        public bool UpdateClaim(Claim claim)
        {
            bool isUpdated = false;
            if (CompanyClaimList == null || CompanyClaimList.Count == 0)
                GetOrUpdateDetails(CompanyClaimList);

            Company company = CompanyClaimList.FirstOrDefault(m => m.Id == claim.CompanyId);
            if(company != null)
            {
                Claim claimObj = company.claims.FirstOrDefault(m => m.ClaimNumber == claim.ClaimNumber);
                if (claimObj != null)
                {
                    claimObj.UCR = claim.UCR ?? claimObj.UCR;
                    claimObj.ClaimDate = claim.ClaimDate;
                    claimObj.LossDate = claim.LossDate;
                    claimObj.AssuredName = claim.AssuredName ?? claimObj.AssuredName;
                    claimObj.IncurredLoss = claim.IncurredLoss;
                    claimObj.Closed = claim.Closed;
                }
            }
            try
            {
                CompanyClaimRepository.GetOrUpdateDetails(CompanyClaimList);
                isUpdated = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return isUpdated;
        }

        private static void GetOrUpdateDetails(List<Company> companies)
        {
            string fileName = "./TestJSON.json";
            if (companies != null && companies.Count != 0)
            {
                JsonSerializerOptions _options = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                var jsonString = JsonSerializer.Serialize(companies, _options);
                File.WriteAllText(fileName, jsonString);
                CompanyClaimList = companies;
            }
            else
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    CompanyClaimList = JsonSerializer.Deserialize<List<Company>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
            }
        }
    }
}

