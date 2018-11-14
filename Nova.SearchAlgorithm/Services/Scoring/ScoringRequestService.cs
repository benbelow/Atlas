﻿using AutoMapper;
using Nova.SearchAlgorithm.Client.Models;
using Nova.SearchAlgorithm.Extensions;
using System.Threading.Tasks;
using Nova.SearchAlgorithm.Client.Models.Scoring;

namespace Nova.SearchAlgorithm.Services.Scoring
{
    public interface IScoringRequestService
    {
        Task<ScoringResult> Score(ScoringRequest scoringRequest);
    }
    
    public class ScoringRequestService: IScoringRequestService
    {
        private readonly IDonorScoringService donorScoringService;
        private readonly IMapper mapper;

        public ScoringRequestService(IDonorScoringService donorScoringService, IMapper mapper)
        {
            this.donorScoringService = donorScoringService;
            this.mapper = mapper;
        }   
        
        public async Task<ScoringResult> Score(ScoringRequest scoringRequest)
        {
            var donorHla = scoringRequest.DonorHla.ToPhenotypeInfo();
            var patientHla = scoringRequest.PatientHla.ToPhenotypeInfo();

            var scoringResult = await donorScoringService.ScoreDonorHlaAgainstPatientHla(donorHla, patientHla);

            return mapper.Map<ScoringResult>(scoringResult);
        }
    }
}