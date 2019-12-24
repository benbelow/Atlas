﻿using System;

namespace Nova.SearchAlgorithm.ApplicationInsights.SearchRequests
{
    public interface ISearchRequestContext
    {
        string SearchRequestId { get; set; }
    }

    public class SearchRequestContext : ISearchRequestContext
    {
        private string searchRequestId;

        public string SearchRequestId
        {
            get => searchRequestId;
            set
            {
                if (!string.IsNullOrEmpty(searchRequestId))
                {
                    throw new InvalidOperationException(
                        $"Cannot set {nameof(SearchRequestId)} to '{value}' as it is already set to '{searchRequestId}'.");
                }

                searchRequestId = value;
            }
        }
    }
}
