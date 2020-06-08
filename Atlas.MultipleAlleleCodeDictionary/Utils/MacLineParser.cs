﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atlas.Common.ApplicationInsights;
using Atlas.MultipleAlleleCodeDictionary.AzureStorage.Models;
using Atlas.MultipleAlleleCodeDictionary.MacImportServices.SourceData;

namespace Atlas.MultipleAlleleCodeDictionary.utils
{
    internal interface IMacParser
    {
        public Task<List<MultipleAlleleCodeEntity>> GetMacsSinceLastEntry(Stream file, string lastMacEntry);
    }

    internal class MacLineParser : IMacParser
    {
        private readonly IMacCodeDownloader macCodeDownloader;
        private readonly ILogger logger;

        public MacLineParser(IMacCodeDownloader macCodeDownloader, ILogger logger)
        {
            this.macCodeDownloader = macCodeDownloader;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<MultipleAlleleCodeEntity>> GetMacsSinceLastEntry(Stream file, string lastMacEntry)
        {
            logger.SendTrace($"Parsing MACs since: {lastMacEntry}", LogLevel.Info);
            var macCodes = new List<MultipleAlleleCodeEntity>();

            using (var reader = new StreamReader(file))
            {
                ReadToEntry(reader, lastMacEntry);
                while (!reader.EndOfStream)
                {
                    var macLine = (await reader.ReadLineAsync())?.TrimEnd();

                    if (string.IsNullOrWhiteSpace(macLine))
                    {
                        continue;
                    }

                    macCodes.Add(ParseMac(macLine));
                }
            }

            return macCodes;
        }

        private static MultipleAlleleCode ParseMac(string macString)
        {
            var substrings = macString.Split('\t');
            var isGeneric = substrings[0] != "*";
            return new MultipleAlleleCode(substrings[1], substrings[2], isGeneric);
        }

        private static void ReadToEntry(StreamReader reader, string entryToReadTo)
        {
            // The first two lines of the NMDP source file contain descriptions, so are discarded
            reader.ReadLine();
            reader.ReadLine();

            if (entryToReadTo == null)
            {
                return;
            }
            
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().TrimEnd();
                // Regex checking can slow down performance, it might be worth using a different comparison method if this slows us down significantly
                var match = Regex.IsMatch(line, $@"\b{entryToReadTo}\b");

                if (match)
                {
                    return;
                }
            }

            throw new Exception($"Reached end of file without finding entry {entryToReadTo}");
        }
    }
}