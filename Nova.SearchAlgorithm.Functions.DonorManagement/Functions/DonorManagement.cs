using Microsoft.Azure.WebJobs;
using Nova.DonorService.Client.Models.DonorUpdate;
using Nova.SearchAlgorithm.Exceptions;
using Nova.SearchAlgorithm.Services.DonorManagement;
using Nova.Utils.ApplicationInsights;
using Nova.Utils.ServiceBus.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nova.SearchAlgorithm.Functions.DonorManagement.Functions
{
    public class DonorManagement
    {
        const string ErrorMessagePrefix = "Error when running the donor management function. ";

        private readonly IDonorUpdateProcessor donorUpdateProcessor;
        private readonly ILogger logger;

        public DonorManagement(IDonorUpdateProcessor donorUpdateProcessor, ILogger logger)
        {
            this.donorUpdateProcessor = donorUpdateProcessor;
            this.logger = logger;
        }

        [FunctionName("ManageDonorByAvailability")]
        public async Task Run([TimerTrigger("%MessagingServiceBus.DonorManagement.CronSchedule%")] TimerInfo myTimer)
        {
            try
            {
                await donorUpdateProcessor.ProcessDonorUpdates();
            }
            catch (MessageBatchException<SearchableDonorUpdate> ex)
            {
                SendMessageBatchExceptionTrace(ex);
                throw new DonorManagementException(ex);
            }
            catch (Exception ex)
            {
                SendExceptionTrace(ex);
                throw new DonorManagementException(ex);
            }
        }

        private void SendMessageBatchExceptionTrace(MessageBatchException<SearchableDonorUpdate> ex)
        {
            logger.SendTrace(
                ErrorMessagePrefix + ex.Message,
                LogLevel.Error,
                new Dictionary<string, string>
                {
                    {"SequenceNumbers", string.Join(",", ex.SequenceNumbers.Select(seqNo => seqNo))}
                });
        }

        private void SendExceptionTrace(Exception ex)
        {
            logger.SendTrace(ErrorMessagePrefix + ex.Message, LogLevel.Error);
        }
    }
}