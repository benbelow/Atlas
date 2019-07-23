using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Nova.SearchAlgorithm.Services.DataRefresh;

namespace Nova.SearchAlgorithm.Functions.Functions
{
    public class DataRefresh
    {
        private readonly IDonorImporter donorImporter;
        private readonly IHlaProcessor hlaProcessor;
        private readonly IDataRefreshOrchestrator dataRefreshOrchestrator;

        public DataRefresh(IDonorImporter donorImporter, IHlaProcessor hlaProcessor, IDataRefreshOrchestrator dataRefreshOrchestrator)
        {
            this.donorImporter = donorImporter;
            this.hlaProcessor = hlaProcessor;
            this.dataRefreshOrchestrator = dataRefreshOrchestrator;
        }

        [FunctionName("RunDataRefresh")]
        public async Task RunDataRefresh([HttpTrigger] HttpRequest httpRequest)
        {
            await dataRefreshOrchestrator.RefreshDataIfNecessary();
        }

        [FunctionName("RunDonorImport")]
        public async Task RunDonorImport([HttpTrigger] HttpRequest httpRequest)
        {
            await donorImporter.StartDonorImport();
        }

        [FunctionName("ProcessDonorHla")]
        public async Task RunHlaRefresh([HttpTrigger] HttpRequest httpRequest)
        {
            await hlaProcessor.UpdateDonorHla();
        }
    }
}