using System;
using System.Linq;
using System.Threading.Tasks;
using Atlas.Common.ApplicationInsights;
using Atlas.Common.Notifications;
using Atlas.DonorImport.ExternalInterface.Models;
using Atlas.DonorImport.Helpers;
using MoreLinq.Extensions;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

namespace Atlas.DonorImport.Services
{
    public interface IDonorFileImporter
    {
        Task ImportDonorFile(DonorImportFile file);
    }

    internal class DonorFileImporter : IDonorFileImporter
    {
        private const int BatchSize = 10000;
        private readonly IDonorImportFileParser fileParser;
        private readonly IDonorRecordChangeApplier donorRecordChangeApplier;
        private readonly INotificationSender notificationSender;
        private readonly ILogger logger;

        public DonorFileImporter(
            IDonorImportFileParser fileParser,
            IDonorRecordChangeApplier donorRecordChangeApplier,
            INotificationSender notificationSender,
            ILogger logger)
        {
            this.fileParser = fileParser;
            this.donorRecordChangeApplier = donorRecordChangeApplier;
            this.notificationSender = notificationSender;
            this.logger = logger;
        }

        public async Task ImportDonorFile(DonorImportFile file)
        {
            logger.SendTrace($"Beginning Donor Import for file '{file.FileLocation}'.");

            var importedDonorCount = 0;
            var lazyFile = fileParser.PrepareToLazilyParseDonorUpdates(file.Contents);
            try
            {
                var donorUpdates = lazyFile.ReadLazyDonorUpdates();
                foreach (var donorUpdateBatch in donorUpdates.Batch(BatchSize))
                {
                    var reifiedDonorBatch = donorUpdateBatch.ToList();
                    await donorRecordChangeApplier.ApplyDonorRecordChangeBatch(reifiedDonorBatch, file.FileLocation);
                    importedDonorCount += reifiedDonorBatch.Count;
                }

                logger.SendTrace($"Donor Import for file '{file.FileLocation}' complete. Imported {importedDonorCount} donor(s).");
            }
            catch (EmptyDonorFileException e)
            {
                const string summary = "Donor file was present but it was empty.";
                var description = e.StackTrace;
                
                logger.SendTrace(summary, LogLevel.Warn);
                await notificationSender.SendAlert(summary, description, Priority.Medium, nameof(ImportDonorFile));
            }
            catch (MalformedDonorFileException e)
            {
                logger.SendTrace(e.Message, LogLevel.Warn);
                await notificationSender.SendAlert(e.Message, e.StackTrace, Priority.Medium, nameof(ImportDonorFile));
            }
            catch (Exception e)
            {
                var summary = $"Donor Import Failed: {file.FileLocation}";
                var description = @$"Importing donors for file: {file.FileLocation} has failed. With exception {e.Message}.
{importedDonorCount} Donors were successfully imported prior to this error and have already been stored in the Database. Any remaining donors in the file have not been stored.
The first {lazyFile?.ParsedDonorCount} Donors were able to be parsed from the file. The last Donor to be *successfully* parsed had DonorCode '{lazyFile?.LastSuccessfullyParsedDonorCode}'.
Manual investigation is recommended; see Application Insights for more information.";

                await notificationSender.SendAlert(summary, description, Priority.Medium, nameof(ImportDonorFile));

                throw;
            }
        }
    }
}