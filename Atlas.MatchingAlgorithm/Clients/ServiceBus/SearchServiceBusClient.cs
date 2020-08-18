using System.Text;
using System.Threading.Tasks;
using Atlas.Client.Models.Search.Results.Matching;
using Atlas.MatchingAlgorithm.Common.Models;
using Atlas.MatchingAlgorithm.Settings.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Atlas.MatchingAlgorithm.Clients.ServiceBus
{
    public interface ISearchServiceBusClient
    {
        Task PublishToSearchQueue(IdentifiedSearchRequest searchRequest);
        Task PublishToResultsNotificationTopic(MatchingResultsNotification matchingResultsNotification);
    }
    
    public class SearchServiceBusClient : ISearchServiceBusClient
    {
        private readonly string connectionString;
        private readonly string searchQueueName;
        private readonly string resultsNotificationTopicName;

        public SearchServiceBusClient(MessagingServiceBusSettings messagingServiceBusSettings)
        {
            connectionString = messagingServiceBusSettings.ConnectionString;
            searchQueueName = messagingServiceBusSettings.SearchRequestsQueue;
            resultsNotificationTopicName = messagingServiceBusSettings.SearchResultsTopic;
        }

        public async Task PublishToSearchQueue(IdentifiedSearchRequest searchRequest)
        {
            var json = JsonConvert.SerializeObject(searchRequest);
            var message = new Message(Encoding.UTF8.GetBytes(json));
            
            var client = new QueueClient(connectionString, searchQueueName);
            await client.SendAsync(message);
        }

        public async Task PublishToResultsNotificationTopic(MatchingResultsNotification matchingResultsNotification)
        {
            var json = JsonConvert.SerializeObject(matchingResultsNotification);
            var message = new Message(Encoding.UTF8.GetBytes(json));
            
            var client = new TopicClient(connectionString, resultsNotificationTopicName);
            await client.SendAsync(message);
        }
    }
}