﻿using Atlas.Common.GeneticData.PhenotypeInfo;

namespace Atlas.Functions.Models.Search.Requests
{
    public class SearchHlaData
    {
        /// <summary>
        /// Search HLA for locus A.
        /// Required.
        /// </summary>
        public LocusSearchHla LocusSearchHlaA { get; set; }

        /// <summary>
        /// Search HLA for locus B.
        /// Required.
        /// </summary>
        public LocusSearchHla LocusSearchHlaB { get; set; }

        /// <summary>
        /// Search HLA for locus C.
        /// Optional.
        /// </summary>
        public LocusSearchHla LocusSearchHlaC { get; set; }

        /// <summary>
        /// Search HLA for locus DPB1.
        /// Optional.
        /// </summary>
        public LocusSearchHla LocusSearchHlaDpb1 { get; set; }

        /// <summary>
        /// Search HLA for locus DQB1.
        /// Optional.
        /// </summary>
        public LocusSearchHla LocusSearchHlaDqb1 { get; set; }

        /// <summary>
        /// Search HLA for locus DRB1.
        /// Required.
        /// </summary>
        public LocusSearchHla LocusSearchHlaDrb1 { get; set; }
    }

    public class LocusSearchHla
    {
        /// <summary>
        /// String representation of the 1st search HLA type position at this locus.
        /// </summary>
        public string SearchHla1 { get; set; }

        /// <summary>
        /// String representation of the 2nd search HLA type position at this locus.
        /// </summary>
        public string SearchHla2 { get; set; }
    }

    public static class SearchHlaMappings
    {
        public static PhenotypeInfo<string> ToPhenotypeInfo(this SearchHlaData hlaData)
        {
            return new PhenotypeInfo<string>
            {
                A = hlaData?.LocusSearchHlaA?.ToLocusInfo() ?? new LocusInfo<string>(null),
                B = hlaData?.LocusSearchHlaB?.ToLocusInfo() ?? new LocusInfo<string>(null),
                C = hlaData?.LocusSearchHlaC?.ToLocusInfo() ?? new LocusInfo<string>(null),
                Dqb1 = hlaData?.LocusSearchHlaDqb1?.ToLocusInfo() ?? new LocusInfo<string>(null),
                Drb1 = hlaData?.LocusSearchHlaDrb1?.ToLocusInfo() ?? new LocusInfo<string>(null),
            };
        }

        private static LocusInfo<string> ToLocusInfo(this LocusSearchHla locusSearchHla)
        {
            return new LocusInfo<string>{
                Position1 = locusSearchHla?.SearchHla1,
                Position2 = locusSearchHla?.SearchHla2
            };
        }
    }
}