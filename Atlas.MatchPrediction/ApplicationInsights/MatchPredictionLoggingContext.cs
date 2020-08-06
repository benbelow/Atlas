using System.Collections.Generic;
using Atlas.Common.ApplicationInsights;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.Common.GeneticData.PhenotypeInfo.TransferModels;
using Atlas.MatchPrediction.ExternalInterface.Models.MatchProbability;

namespace Atlas.MatchPrediction.ApplicationInsights
{
    internal class MatchPredictionLoggingContext : LoggingContext
    {
        public void Initialise(SingleDonorMatchProbabilityInput singleDonorMatchProbabilityInput)
        {
            SearchRequestId = singleDonorMatchProbabilityInput.SearchRequestId;
            HlaNomenclatureVersion = singleDonorMatchProbabilityInput.HlaNomenclatureVersion;
            DonorId = singleDonorMatchProbabilityInput.Donor.DonorId.ToString();
            DonorHla = singleDonorMatchProbabilityInput.Donor.DonorHla?.ToPhenotypeInfo();
            PatientHla = singleDonorMatchProbabilityInput.PatientHla?.ToPhenotypeInfo();
        }

        public string SearchRequestId { get; set; }
        public string HlaNomenclatureVersion { get; set; }
        public string DonorId { get; set; }
        public PhenotypeInfo<string> DonorHla { get; set; }
        public PhenotypeInfo<string> PatientHla { get; set; }

        /// <inheritdoc />
        public override Dictionary<string, string> PropertiesToLog()
        {
            return new Dictionary<string, string>
            {
                {nameof(SearchRequestId), SearchRequestId},
                {nameof(HlaNomenclatureVersion), HlaNomenclatureVersion},
                {nameof(DonorId), DonorId},
                {nameof(DonorHla), DonorHla?.PrettyPrint()},
                {nameof(PatientHla), PatientHla?.PrettyPrint()}
            };
        }
    }
}