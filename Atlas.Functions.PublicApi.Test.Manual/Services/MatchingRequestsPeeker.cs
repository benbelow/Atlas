﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlas.Functions.PublicApi.Test.Manual.Models;
using Atlas.Functions.PublicApi.Test.Manual.Services.ServiceBus;
using Atlas.MatchingAlgorithm.Common.Models;

namespace Atlas.Functions.PublicApi.Test.Manual.Services
{
    public interface IMatchingRequestsPeeker
    {
        Task<IEnumerable<string>> GetIdsOfDeadLetteredMatchingRequests(PeekRequest peekRequest);
    }

    internal class MatchingRequestsPeeker : IMatchingRequestsPeeker
    {
        private readonly IDeadLettersPeeker<IdentifiedSearchRequest> deadLetterReceiver;

        public MatchingRequestsPeeker(IDeadLettersPeeker<IdentifiedSearchRequest> deadLetterReceiver)
        {
            this.deadLetterReceiver = deadLetterReceiver;
        }

        public async Task<IEnumerable<string>> GetIdsOfDeadLetteredMatchingRequests(PeekRequest peekRequest)
        {
            var deadLetteredRequests = await deadLetterReceiver.Peek(peekRequest);

            return deadLetteredRequests
                .Select(r => r.DeserializedBody.Id)
                .Distinct();
        }
    }
}