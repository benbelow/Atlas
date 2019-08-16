﻿using System;
using Nova.HLAService.Client;
using Nova.HLAService.Client.Models;
using Nova.HLAService.Client.Services;
using Nova.SearchAlgorithm.MatchingDictionary.Caching;
using Nova.SearchAlgorithm.MatchingDictionary.Repositories;
using Nova.Utils.ApplicationInsights;

namespace Nova.SearchAlgorithm.MatchingDictionary.Services.Lookups
{
    internal static class HlaLookupFactory
    {
        public static HlaLookupBase GetLookupByHlaTypingCategory(
            HlaTypingCategory category,
            IHlaLookupRepository hlaLookupRepository,
            IAlleleNamesLookupService alleleNamesLookupService,
            IHlaServiceClient hlaServiceClient,
            IAlleleStringSplitterService alleleSplitter,
            IAntigenCache cache,
            ILogger logger)
        {
            switch (category)
            {
                case HlaTypingCategory.Allele:
                    return new SingleAlleleLookup(
                        hlaLookupRepository, 
                        alleleNamesLookupService);
                case HlaTypingCategory.XxCode:
                    return new XxCodeLookup(
                        hlaLookupRepository);
                case HlaTypingCategory.Serology:
                    return new SerologyLookup(
                        hlaLookupRepository);                   
                case HlaTypingCategory.NmdpCode:
                    return new NmdpCodeLookup(
                        hlaLookupRepository,
                        alleleNamesLookupService,
                        cache,
                        hlaServiceClient,
                        alleleSplitter,
                        logger);                    
                case HlaTypingCategory.AlleleStringOfNames:
                case HlaTypingCategory.AlleleStringOfSubtypes:
                    return new AlleleStringLookup(
                        hlaLookupRepository, 
                        alleleNamesLookupService, 
                        alleleSplitter);                    
                default:
                    throw new ArgumentException(
                        $"Dictionary lookup cannot be performed for HLA typing category: {category}.");
            }
        }
    }
}