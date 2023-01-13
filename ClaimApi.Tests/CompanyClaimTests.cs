using System;
using ClaimApi.Controllers;
using ClaimApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimApi.Tests
{
	public class CompanyClaimTests
	{
        private readonly CompanyClaimController _controller;
        public CompanyClaimTests()
        {
            _controller = new CompanyClaimController();
        }

        [Theory]
        [InlineData(1)]
        public void GetClaims_WhenCalled_ReturnsAllItems(int company_id)
        {
            // Act
            var okResult = _controller.GetClaims(company_id) as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Claim>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

    }
}

