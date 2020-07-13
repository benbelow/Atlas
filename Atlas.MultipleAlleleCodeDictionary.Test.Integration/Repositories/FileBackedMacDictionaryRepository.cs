﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Atlas.Common.Caching;
using Atlas.MultipleAlleleCodeDictionary.AzureStorage.Models;
using Atlas.MultipleAlleleCodeDictionary.AzureStorage.Repositories;
using Atlas.MultipleAlleleCodeDictionary.ExternalInterface.Models;
using CsvHelper;
using LazyCache;

namespace Atlas.MultipleAlleleCodeDictionary.Test.Integration.Repositories
{
    public class FileBackedMacDictionaryRepository : IMacRepository
    {
        private readonly IAppCache cache;
        
        // This class uses the same cache as the MacCacheServiceRepository,
        // which means that we'd be loading macs into the cache twice with the same key ... in the same call.
        // LazyCache isn't too offended by that, when it works. But if a key is missing it causes deadlocks. :(
        private const string KeysPrefix = nameof(FileBackedMacDictionaryRepository) + "_";
        private const string AllMacsKey = KeysPrefix + nameof(AllMacsKey);

        public FileBackedMacDictionaryRepository(IPersistentCacheProvider cacheProvider)
        {
            cache = cacheProvider.Cache;
            CacheAllMacs();
        }

        public Task<string> GetLastMacEntry()
        {
            throw new System.NotImplementedException();
        }

        public Task InsertMacs(IEnumerable<Mac> macCodes)
        {
            throw new System.NotImplementedException();
        }

        public Task TruncateMacTable()
        {
            throw new System.NotImplementedException();
        }

        public Task<Mac> GetMac(string macCode)
        {
            var mac = cache.Get<Mac>(KeysPrefix + macCode);
            return Task.FromResult(mac);
        }

        public Task<IReadOnlyCollection<Mac>> GetAllMacs()
        {
            var macs = cache.Get<IReadOnlyCollection<Mac>>(AllMacsKey);
            return Task.FromResult(macs);
        }

        private void CacheAllMacs()
        {
            var macs = ReadMacsFromFile().Select(macEnt => new Mac(macEnt)).ToList().AsReadOnly();
            cache.Add(AllMacsKey, macs);
            foreach (var mac in macs)
            {
                cache.Add(KeysPrefix + mac.Code, mac);
            }
        }

        private IEnumerable<MacEntity> ReadMacsFromFile()
        {
            // The csv file is generated by exporting the table from AzureStorageExplorer into a csv.
            // That generates a file with the following columns, which came originally from the format of the MacEntity class:
            //
            // Code,Code@type,HLA,HLA@type,IsGeneric,IsGeneric@type,PartitionKey,RowKey,Timestamp
            // eg: AA,Edm.String,01/02/03/05,Edm.String,true,Edm.Boolean,2,AA,2020-07-11T06:43:49.033Z
            //
            // If you need to drastically change the contents of the file, so that it contains a known set of MacRecords, then
            // one easy way to achieve this is:
            // * Modify the MacImport code, so that after parsing the Mac file, it filters the macs to only those records you want (use HashSet.Contains for speed!)
            // * Modify your local Top-level function settings so that it points at a new (temporary) table in AzureTableStorage.
            // * Run the Manual Import endpoint, to populate your temporary table with the records you care about.
            // * Using AzureStorageExplorer navigate to your temp table and export it to CSV.

            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream($"{GetType().Namespace}.LargeMacDictionary.csv"))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HeaderValidated = null; // Don't worry about the columns being a perfect match.
                csv.Configuration.MissingFieldFound = null; // Don't worry about the columns being a perfect match.
                csv.Configuration.AllowComments = true; 
                csv.Configuration.Comment = '#';

                return csv.GetRecords<MacEntity>().ToList();
            }
        }
    }
}