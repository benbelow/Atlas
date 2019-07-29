using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Nova.SearchAlgorithm.Models;
using Nova.SearchAlgorithm.Services;

namespace Nova.SearchAlgorithm.Functions.DonorManagement.Functions
{
    public class DonorManagement
    {
        private readonly IDonorManagementService donorManagementService;

        public DonorManagement(IDonorManagementService donorManagementService)
        {
            this.donorManagementService = donorManagementService;
        }

        [FunctionName("ManageDonorByAvailability")]
        public async Task Run(
            [ServiceBusTrigger(
                "%MessagingServiceBus.DonorManagement.Topic%",
                "%MessagingServiceBus.DonorManagement.Subscription%",
                Connection = "MessagingServiceBus.ConnectionString")]
            string message)
        {
            var update = JsonConvert.DeserializeObject<DonorAvailabilityUpdate>(message);
            await donorManagementService.ManageDonorByAvailability(update);
        }
    }
}