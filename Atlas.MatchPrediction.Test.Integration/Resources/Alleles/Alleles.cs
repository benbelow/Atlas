using Atlas.Common.GeneticData;
using Atlas.Common.GeneticData.PhenotypeInfo;

namespace Atlas.MatchPrediction.Test.Integration.Resources.Alleles
{
    internal static class Alleles
    {
        /// <summary>
        /// Alleles selected, such that each position has a single allele that corresponds to a single G-Group.
        /// All chosen alleles are heterozygous with respect to the chosen GGroup.
        /// Creates new property each time to avoid mutation.
        /// </summary>
        public static PhenotypeInfo<AlleleWithGGroups> UnambiguousAlleleDetails => new PhenotypeInfo<AlleleWithGGroups>
        (
            valueA: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "02:09", GGroup = "02:01:01G", SmallGGroup = "02:01g"},
                new AlleleWithGGroups {Allele = "11:03", GGroup = "11:03:01G", SmallGGroup = "11:03g"}
            ),
            valueB: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "15:12:01", GGroup = "15:12:01G", SmallGGroup = "15:12g"},
                new AlleleWithGGroups {Allele = "08:182", GGroup = "08:01:01G", SmallGGroup = "08:01g"}
            ),
            valueC: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "01:03", GGroup = "01:03:01G", SmallGGroup = "01:03g"},
                new AlleleWithGGroups {Allele = "03:05", GGroup = "03:05:01G", SmallGGroup = "03:05g"}
            ),
            valueDqb1: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "03:09", GGroup = "03:01:01G", SmallGGroup = "03:01g"},
                new AlleleWithGGroups {Allele = "02:04", GGroup = "02:01:01G", SmallGGroup = "02:01g"}
            ),
            valueDrb1: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "03:124", GGroup = "03:01:01G", SmallGGroup = "03:01g"},
                new AlleleWithGGroups {Allele = "11:129", GGroup = "11:06:01G", SmallGGroup = "11:06g"}
            )
        );

        public static PhenotypeInfo<AlleleWithGGroups> AmbiguousAlleleDetails => new PhenotypeInfo<AlleleWithGGroups>
        (
            valueA: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "01:01", GGroup = "01:01:01G", SmallGGroup = "01:01g"},
                new AlleleWithGGroups {Allele = "02:01", GGroup = "02:01:01G", SmallGGroup = "02:01g"}
            ),
            valueB: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "15:146", GGroup = "15:01:01G", SmallGGroup = "15:01g"},
                new AlleleWithGGroups {Allele = "08:182", GGroup = "08:01:01G", SmallGGroup = "08:01g"}
            ),
            valueC: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "04:82", GGroup = "04:01:01G", SmallGGroup = "04:01g"},
                new AlleleWithGGroups {Allele = "03:04", GGroup = "03:04:01G", SmallGGroup = "03:04g"}
            ),
            valueDqb1: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "03:19", GGroup = "03:01:01G", SmallGGroup = "03:01g"},
                new AlleleWithGGroups {Allele = "03:03", GGroup = "03:03:01G", SmallGGroup = "03:03g"}
            ),
            valueDrb1: new LocusInfo<AlleleWithGGroups>
            (
                new AlleleWithGGroups {Allele = "*15:03", GGroup = "15:03:01G", SmallGGroup = "15:03g"},
                new AlleleWithGGroups {Allele = "*13:01", GGroup = "13:01:01G", SmallGGroup = "13:01g"}
            )
        );
    }

    internal class AlleleWithGGroups
    {
        public string Allele { get; set; }
        public string GGroup { get; set; }
        public string SmallGGroup { get; set; }
    }

    internal static class AlleleWithGGroupPhenotypeExtensions
    {
        public static PhenotypeInfo<string> Alleles(this PhenotypeInfo<AlleleWithGGroups> phenotypeInfo)
        {
            return phenotypeInfo.Map((l, d) => l == Locus.Dpb1 ? null : d.Allele);
        }

        public static PhenotypeInfo<string> GGroups(this PhenotypeInfo<AlleleWithGGroups> phenotypeInfo)
        {
            return phenotypeInfo.Map((l, d) => l == Locus.Dpb1 ? null : d.GGroup);
        }

        public static PhenotypeInfo<string> SmallGGroups(this PhenotypeInfo<AlleleWithGGroups> phenotypeInfo)
        {
            return phenotypeInfo.Map((l, d) => l == Locus.Dpb1 ? null : d.SmallGGroup);
        }
    }
}