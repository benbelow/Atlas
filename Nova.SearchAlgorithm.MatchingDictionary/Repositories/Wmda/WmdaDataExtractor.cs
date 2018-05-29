﻿using Nova.SearchAlgorithm.MatchingDictionary.Data;
using Nova.SearchAlgorithm.MatchingDictionary.Models.HLATypings;
using Nova.SearchAlgorithm.MatchingDictionary.Models.Wmda;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nova.SearchAlgorithm.MatchingDictionary.Repositories.Wmda
{
    internal abstract class WmdaDataExtractor<TWmdaHlaTyping> where TWmdaHlaTyping : IWmdaHlaTyping
    {
        protected const string WmdaFilePathPrefix = "wmda/";

        private static readonly Func<TWmdaHlaTyping, bool> FilterTypingsByMolecularLociNames =
            typing => LocusNames.MolecularLoci.Contains(typing.WmdaLocus);

        private static readonly Func<TWmdaHlaTyping, bool> FilterTypingsBySerologyLociNames =
            typing => LocusNames.SerologyLoci.Contains(typing.WmdaLocus) && !typing.IsDrb345SerologyTyping();

        private readonly string fileName;
        private readonly TypingMethod typingMethod;

        protected WmdaDataExtractor(string fileName, TypingMethod typingMethod)
        {
            this.fileName = fileName;
            this.typingMethod = typingMethod;
        }

        public IEnumerable<TWmdaHlaTyping> GetWmdaData(IWmdaFileReader fileReader)
        {
            var fileContents = fileReader.GetFileContentsWithoutHeader(fileName);
            var data = ExtractWmdaDataFromFileContents(fileContents);

            return data;
        }

        private IEnumerable<TWmdaHlaTyping> ExtractWmdaDataFromFileContents(IEnumerable<string> wmdaFileContents)
        {
            var selectTypingsForLociOfInterestOnly =
                typingMethod == TypingMethod.Molecular ? FilterTypingsByMolecularLociNames : FilterTypingsBySerologyLociNames;

            var extractionQuery =
                from line in wmdaFileContents
                select TryToMapLineOfFileToWmdaHlaTyping(line) into typing
                where typing != null && selectTypingsForLociOfInterestOnly(typing)
                select typing;

            var extractedData = extractionQuery.ToArray();
            return extractedData;
        }

        protected abstract TWmdaHlaTyping TryToMapLineOfFileToWmdaHlaTyping(string line);
    }
}
