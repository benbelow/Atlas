﻿using System.Threading.Tasks;
using Atlas.MatchingAlgorithm.Data.Models.DonorInfo;
using Atlas.MatchingAlgorithm.Services.Donors;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.MatchingAlgorithm.Api.Controllers
{
    [Route("donor")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService donorService;

        public DonorController(IDonorService donorService)
        {
            this.donorService = donorService;
        }

        [HttpPut]
        [Route("batch")]
        public async Task CreateOrUpdateDonorBatch([FromBody] DonorInfoBatch donorBatch)
        {
            await donorService.CreateOrUpdateDonorBatch(donorBatch.Donors);
        }
    }
}
