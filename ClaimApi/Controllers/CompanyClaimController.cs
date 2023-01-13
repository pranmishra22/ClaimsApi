using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClaimApi.Models;
using ClaimApi.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClaimApi.Controllers
{
    [ApiController]
    [Route("Companies")]
    public class CompanyClaimController : ControllerBase
    {
        private readonly ICompanyClaimRepository _companyClaimRepository;

        public CompanyClaimController()
        {
            _companyClaimRepository = new CompanyClaimRepository();
        }

        [HttpGet("{company_id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetCompanyDetails(int company_id)
        {
            var companyDetails = _companyClaimRepository.GetCompanyById(company_id);
            var company = new
            {
                CompanyId = companyDetails.Id,
                CompanyName = companyDetails.Name,
                Address1 = companyDetails.Address1,
                Address2 = companyDetails.Address2,
                Address3 = companyDetails.Address3,
                Postcode = companyDetails.Postcode,
                Country = companyDetails.Country,
                ActiveInnsurancePolicy = companyDetails.InsuranceEndDate > DateTime.Now ? true : false,
                InsuranceEndDate = companyDetails.InsuranceEndDate
            };

            return company == null ? NotFound() : Ok(company);
        }

        [HttpGet("{company_id}/claims")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetClaims(int company_id)
        {
            var claims = _companyClaimRepository.GetCompanyClaims(company_id);

            return claims.Count() == 0 ? NotFound() : Ok(claims);
        }

        [HttpGet("{company_id}/claims/{claim_id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetClaimDetails(int company_id, int claim_id)
        {
            var claimDetails = _companyClaimRepository.GetClaimDetailsByClaimNumber(company_id, claim_id);

            if (claimDetails == null)
                return NotFound();
            var claim = new
            {
                ClaimId = claimDetails.ClaimNumber,
                CompanyId = claimDetails.CompanyId,
                UCR = claimDetails.UCR,
                ClaimDays = (claimDetails.LossDate - claimDetails.ClaimDate).TotalDays,
                ClaimDate = claimDetails.ClaimDate,
                LossDate = claimDetails.LossDate,
                AssuredName = claimDetails.AssuredName,
                IncurredLoss = claimDetails.IncurredLoss,
                Closed = claimDetails.Closed
            };
            return claim == null ? NotFound() : Ok(claim);
        }
        
        [HttpPut("{company_id}/claims/{claim_id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClaim(int company_id, int claim_id, Claim claim)
        {
            if (claim_id != claim.ClaimNumber && company_id != claim.CompanyId)
                return BadRequest();

            if(_companyClaimRepository.UpdateClaim(claim))
                return NoContent();
            return BadRequest();
        }
        
    }
}

