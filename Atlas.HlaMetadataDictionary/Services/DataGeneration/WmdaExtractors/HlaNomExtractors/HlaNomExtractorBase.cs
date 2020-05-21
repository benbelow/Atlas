﻿using Atlas.Common.GeneticData.Hla.Models;
using Atlas.HlaMetadataDictionary.Models.Wmda;
using System.Text.RegularExpressions;

namespace Atlas.HlaMetadataDictionary.Services.DataGeneration.WmdaExtractors.HlaNomExtractors
{
    internal abstract class HlaNomExtractorBase : WmdaDataExtractor<HlaNom>
    {
        private const string FileName = WmdaFilePathPrefix + "hla_nom.txt";
        private readonly Regex regex = new Regex(@"^(\w+\*{0,1})\;([\w:]+)\;\d+\;(\d*)\;([\w:]*)\;");
        private readonly TypingMethod typingMethod;

        protected HlaNomExtractorBase(TypingMethod typingMethod) : base(FileName)
        {
            this.typingMethod = typingMethod;
        }

        protected override HlaNom MapLineOfFileContentsToWmdaHlaTyping(string line)
        {
            if (!regex.IsMatch(line))
                return null;

            var extractedData = regex.Match(line).Groups;

            return new HlaNom(
                typingMethod,
                extractedData[1].Value,
                extractedData[2].Value,
                !extractedData[3].Value.Equals(""),
                extractedData[4].Value);
        }
    }
}
