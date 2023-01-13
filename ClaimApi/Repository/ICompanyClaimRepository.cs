using System;
using ClaimApi.Models;

namespace ClaimApi.Repository
{
	public interface ICompanyClaimRepository
	{
        Company GetCompanyById(int company_id);
		IEnumerable<Claim> GetCompanyClaims(int company_id);
		Claim GetClaimDetailsByClaimNumber(int company_id, int claim_id);
		bool UpdateClaim(Claim claim);
	}
}

