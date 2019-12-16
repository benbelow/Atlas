﻿using FluentValidation;
using Nova.DonorService.Client.Models.SearchableDonors;
using Nova.SearchAlgorithm.Data.Models.DonorInfo;
using Nova.SearchAlgorithm.Extensions;
using Nova.SearchAlgorithm.Models;
using Nova.SearchAlgorithm.Services.Donors;
using Nova.SearchAlgorithm.Validators.DonorInfo;
using Nova.Utils.ApplicationInsights;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nova.SearchAlgorithm.Services.DataRefresh
{
    public interface IDonorInfoConverter
    {
        Task<DonorBatchProcessingResult<DonorInfo>> ConvertDonorInfoAsync(
            IEnumerable<SearchableDonorInformation> donorInfos,
            string failureEventName);
    }

    public class DonorInfoConverter :
        DonorBatchProcessor<SearchableDonorInformation, DonorInfo, ValidationException>,
        IDonorInfoConverter
    {
        public DonorInfoConverter(ILogger logger)
            : base(logger)
        {
        }

        public async Task<DonorBatchProcessingResult<DonorInfo>> ConvertDonorInfoAsync(
            IEnumerable<SearchableDonorInformation> donorInfos,
            string failureEventName)
        {
            return await ProcessBatchAsync(
                donorInfos,
                async info => await ConvertDonorInfo(info),
                info => new FailedDonorInfo(info)
                {
                    DonorId = info.DonorId.ToString(),
                    RegistryCode = info.RegistryCode
                },
                failureEventName);
        }

        private static async Task<DonorInfo> ConvertDonorInfo(SearchableDonorInformation info)
        {
            await new SearchableDonorInformationValidator().ValidateAndThrowAsync(info);

            return info.ToDonorInfo();
        }
    }
}
