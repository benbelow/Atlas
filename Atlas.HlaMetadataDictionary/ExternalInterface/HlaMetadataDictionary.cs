using Atlas.Common.ApplicationInsights;
using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.PhenotypeInfo;
using Atlas.HlaMetadataDictionary.ExternalInterface.Models;
using Atlas.HlaMetadataDictionary.ExternalInterface.Models.Metadata;
using Atlas.HlaMetadataDictionary.ExternalInterface.Models.Metadata.ScoringMetadata;
using Atlas.HlaMetadataDictionary.Services.DataGeneration;
using Atlas.HlaMetadataDictionary.Services.DataRetrieval;
using Atlas.HlaMetadataDictionary.Services.HlaConversion;
using Atlas.HlaMetadataDictionary.WmdaDataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atlas.HlaMetadataDictionary.ExternalInterface
{
    public interface IHlaMetadataDictionary
    {
        Task<string> RecreateHlaMetadataDictionary(CreationBehaviour recreationBehaviour);
        Task<IReadOnlyCollection<string>> ConvertHla(Locus locus, string hlaName, TargetHlaOptions targetHlaOption);
        Task<LocusInfo<IHlaMatchingMetadata>> GetLocusHlaMatchingMetadata(Locus locus, LocusInfo<string> locusTyping);
        Task<IHlaScoringMetadata> GetHlaScoringMetadata(Locus locus, string hlaName);
        Task<string> GetDpb1TceGroup(string dpb1HlaName);
        IEnumerable<string> GetAllPGroups();

        /// <summary>
        /// Indicates whether there's a discrepancy between the version of the HLA Nomenclature that we would use from WMDA,
        /// and the version of the HLA Nomenclature that was used to pre-process the current Donor data.
        /// </summary>
        /// <returns>True if the versions are different, otherwise false.</returns>
        bool IsActiveVersionDifferentFromLatestVersion();
    }

    internal class HlaMetadataDictionary : IHlaMetadataDictionary
    {
        private readonly string activeHlaNomenclatureVersion;
        private readonly IRecreateHlaMetadataService recreateMetadataService;
        private readonly IHlaConverter hlaConverter;
        private readonly IHlaMatchingMetadataService hlaMatchingMetadataService;
        private readonly ILocusHlaMatchingMetadataService locusHlaMatchingMetadataService;
        private readonly IHlaScoringMetadataService hlaScoringMetadataService;
        private readonly IDpb1TceGroupMetadataService dpb1TceGroupMetadataService;
        private readonly IWmdaHlaNomenclatureVersionAccessor wmdaHlaNomenclatureVersionAccessor;
        private readonly ILogger logger;

        public HlaMetadataDictionary(
            string activeHlaNomenclatureVersion,
            IRecreateHlaMetadataService recreateMetadataService,
            IHlaConverter hlaConverter,
            IHlaMatchingMetadataService hlaMatchingMetadataService,
            ILocusHlaMatchingMetadataService locusHlaMatchingMetadataService,
            IHlaScoringMetadataService hlaScoringMetadataService,
            IDpb1TceGroupMetadataService dpb1TceGroupMetadataService,
            IWmdaHlaNomenclatureVersionAccessor wmdaHlaNomenclatureVersionAccessor,
            ILogger logger)
        {
            this.activeHlaNomenclatureVersion = activeHlaNomenclatureVersion;
            this.recreateMetadataService = recreateMetadataService;
            this.hlaConverter = hlaConverter;
            this.hlaMatchingMetadataService = hlaMatchingMetadataService;
            this.locusHlaMatchingMetadataService = locusHlaMatchingMetadataService;
            this.hlaScoringMetadataService = hlaScoringMetadataService;
            this.dpb1TceGroupMetadataService = dpb1TceGroupMetadataService;
            this.wmdaHlaNomenclatureVersionAccessor = wmdaHlaNomenclatureVersionAccessor;
            this.logger = logger;
        }

        public bool IsActiveVersionDifferentFromLatestVersion()
        {
            var active = activeHlaNomenclatureVersion;
            var latest = wmdaHlaNomenclatureVersionAccessor.GetLatestStableHlaNomenclatureVersion();
            return active != latest;
        }

        public async Task<string> RecreateHlaMetadataDictionary(CreationBehaviour recreationBehaviour)
        {
            var version = IdentifyVersionToRecreate(recreationBehaviour);

            if (ShouldRecreate(recreationBehaviour))
            {
                logger.SendTrace($"HLA-METADATA-DICTIONARY REFRESH: Recreating HLA Metadata dictionary for desired HLA Nomenclature version.", LogLevel.Info);
                await recreateMetadataService.RefreshAllHlaMetadata(version);
                logger.SendTrace($"HLA-METADATA-DICTIONARY REFRESH: HLA Metadata dictionary recreated at HLA Nomenclature version: {version}", LogLevel.Info);
            }
            else
            {
                logger.SendTrace($"HLA-METADATA-DICTIONARY REFRESH: HLA Metadata dictionary was already using the desired HLA Nomenclature version, so did not update.", LogLevel.Info);
            }

            return version;
        }

        private bool ShouldRecreate(CreationBehaviour creationConfig)
        {
            return creationConfig.CreationMode switch
            {
                CreationBehaviour.Mode.Latest => IsActiveVersionDifferentFromLatestVersion() || creationConfig.ShouldForce,
                CreationBehaviour.Mode.Active => creationConfig.ShouldForce ? true : throw new NotImplementedException(),
                CreationBehaviour.Mode.Specific => creationConfig.ShouldForce ? true : throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(creationConfig.CreationMode), creationConfig.CreationMode, "Unexpected enum value")
            };
        }

        private string IdentifyVersionToRecreate(CreationBehaviour creationConfig)
        {
            return creationConfig.CreationMode switch
            {
                CreationBehaviour.Mode.Specific => creationConfig.SpecificVersion,
                CreationBehaviour.Mode.Active => activeHlaNomenclatureVersion,
                CreationBehaviour.Mode.Latest => wmdaHlaNomenclatureVersionAccessor.GetLatestStableHlaNomenclatureVersion(),
                _ => throw new ArgumentOutOfRangeException(nameof(creationConfig.CreationMode), creationConfig.CreationMode, "Unexpected enum value")
            };
        }

        public async Task<IReadOnlyCollection<string>> ConvertHla(Locus locus, string hlaName, TargetHlaOptions targetHlaOption)
        {
            return await hlaConverter.ConvertHla(locus, hlaName, new HlaConversionBehaviour
            {
                HlaNomenclatureVersion = activeHlaNomenclatureVersion,
                TargetHlaOptions = targetHlaOption
            });
        }

        public async Task<LocusInfo<IHlaMatchingMetadata>> GetLocusHlaMatchingMetadata(Locus locus, LocusInfo<string> locusTyping)
        {
            return await locusHlaMatchingMetadataService.GetHlaMatchingMetadata(locus, locusTyping, activeHlaNomenclatureVersion);
        }

        public async Task<IHlaScoringMetadata> GetHlaScoringMetadata(Locus locus, string hlaName)
        {
            return await hlaScoringMetadataService.GetHlaMetadata(locus, hlaName, activeHlaNomenclatureVersion);
        }

        public async Task<string> GetDpb1TceGroup(string dpb1HlaName)
        {
            return await dpb1TceGroupMetadataService.GetDpb1TceGroup(dpb1HlaName, activeHlaNomenclatureVersion);
        }

        public IEnumerable<string> GetAllPGroups()
        {
            return hlaMatchingMetadataService.GetAllPGroups(activeHlaNomenclatureVersion);
        }
    }
}