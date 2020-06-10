﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Atlas.MatchPrediction.Client.Models.GenotypeImputation;
using Atlas.MatchPrediction.Services.GenotypeImputation;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Atlas.HlaMetadataDictionary.ExternalInterface;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace Atlas.MatchPrediction.Functions.Functions
{
    public class GenotypeImputationFunctions
    {
        private readonly IGenotypeImputationService genotypeImputationService;

        public GenotypeImputationFunctions(IHlaMetadataDictionaryFactory metadataDictionaryFactory, IGenotypeImputationService genotypeImputationService)
        {
            this.genotypeImputationService = genotypeImputationService;
        }

        [FunctionName(nameof(GenotypeImputation))]
        public async Task<IActionResult> GenotypeImputation(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            [RequestBodyType(typeof(GenotypeImputationInput), "phenotype input")]
            HttpRequest request)
        {
            var genotypeImputationInput = JsonConvert.DeserializeObject<GenotypeImputationInput>(await new StreamReader(request.Body).ReadToEndAsync());

            try
            {
                var genotypes = genotypeImputationService.ImputeGenotype(genotypeImputationInput);
                return new JsonResult(genotypes);
            }
            catch (Exception)
            {
                return new InternalServerErrorResult();
            }
        }
    }
}
