using System.Text;
using System.Threading.Tasks;
using Atlas.MatchingAlgorithm.Client.Models.SearchResults;
using Atlas.MatchingAlgorithm.Common.Models;
using Atlas.MatchingAlgorithm.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Atlas.MatchingAlgorithm.Clients.ServiceBus
{
    public interface ISearchServiceBusClient
    {
        Task PublishToSearchQueue(IdentifiedSearchRequest searchRequest);
        Task PublishToResultsNotificationTopic(SearchResultsNotification searchResultsNotification);
    }
    
    public class SearchServiceBusClient : ISearchServiceBusClient
    {
        private readonly string connectionString;
        private readonly string searchQueueName;
        private readonly string resultsNotificationTopicName;

        public SearchServiceBusClient(string connectionString, string searchQueueName, string resultsNotificationTopicName)
        {
            this.connectionString = connectionString;
            this.searchQueueName = searchQueueName;
            this.resultsNotificationTopicName = resultsNotificationTopicName;
        }

        public async Task PublishToSearchQueue(IdentifiedSearchRequest searchRequest)
        {
            var json = JsonConvert.SerializeObject(searchRequest);
            var message = new Message(Encoding.UTF8.GetBytes(json));
            
            var client = new QueueClient(connectionString, searchQueueName);
            await client.SendAsync(message);
        }

        public async Task PublishToResultsNotificationTopic(SearchResultsNotification searchResultsNotification)
        {
            var json = JsonConvert.SerializeObject(searchResultsNotification);
            var message = new Message(Encoding.UTF8.GetBytes(json));
            
            var client = new TopicClient(connectionString, resultsNotificationTopicName);
            await client.SendAsync(message);
        }
    }
}