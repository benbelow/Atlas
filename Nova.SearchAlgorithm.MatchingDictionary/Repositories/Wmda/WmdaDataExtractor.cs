﻿using Nova.SearchAlgorithm.MatchingDictionary.Data;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Wmda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nova.SearchAlgorithm.MatchingDictionary.Repositories.Wmda
{
    internal abstract class WmdaDataExtractor<TWmdaHlaTyping> where TWmdaHlaTyping : IWmdaHlaTyping
    {
        protected const string WmdaFilePathPrefix = "wmda/";

        private static readonly Func<TWmdaHlaTyping, bool> MolecularDataFilter =
            typing => LocusNames.MolecularLoci.Contains(typing.WmdaLocus);

        private static readonly Func<TWmdaHlaTyping, bool> SerologyDataFilter =
            typing => LocusNames.SerologyLoci.Contains(typing.WmdaLocus) && 
                !Drb345Typings.IsDrb345SerologyTyping(typing);

        private readonly string fileName;
        private readonly string regexPattern;
        private readonly TypingMethod typingMethod;

        protected WmdaDataExtractor(string fileName, string regexPattern, TypingMethod typingMethod)
        {
            this.fileName = fileName;
            this.regexPattern = regexPattern;
            this.typingMethod = typingMethod;
        }

        public IEnumerable<TWmdaHlaTyping> GetWmdaData(IWmdaFileReader fileReader)
        {
            var fileContents = fileReader.GetFileContentsWithoutHeader(fileName);
            var data = ExtractWmdaDataFromFileContents(fileContents);

            return data;
        }

        private TWmdaHlaTyping[] ExtractWmdaDataFromFileContents(IEnumerable<string> wmdaFileContents)
        {
            var regex = new Regex(regexPattern);
            var dataFilter = typingMethod == TypingMethod.Molecular ? MolecularDataFilter : SerologyDataFilter;

            var extractionQuery =
                from line in wmdaFileContents
                select regex.Match(line).Groups into regexResults
                where regexResults.Count > 0
                select MapDataExtractedFromWmdaFile(regexResults) into mapped
                where dataFilter(mapped)
                select mapped;

            var enumeratedData = extractionQuery.ToArray();

            return enumeratedData;
        }

        protected abstract TWmdaHlaTyping MapDataExtractedFromWmdaFile(GroupCollection extractedData);
    }
}
