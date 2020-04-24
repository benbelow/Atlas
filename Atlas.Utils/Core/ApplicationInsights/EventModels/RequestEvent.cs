﻿using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Nova.Utils.Middleware.RequestId;

namespace Nova.Utils.ApplicationInsights.EventModels
{
    public class RequestEvent : EventModel
    {
        private const string IncomingRequestEventName = "Incoming Request";
        private const string OutgoingNovaRequestEventName = "Outgoing Nova Request";
        private readonly Stopwatch responseTimer;

        public RequestEvent(string eventName) : base(eventName)
        {
            responseTimer = new Stopwatch();
            responseTimer.Start();
            Level = LogLevel.Verbose;
        }

        public void AddOutgoingRequestDetails(HttpRequestMessage request)
        {
            Properties.Add("Api Call", request.Method + " " + request.RequestUri.OriginalString);
            Properties.Add("Proposed Id", request.GetRequestId());
        }

        public static RequestEvent IncomingRequest()
        {
            return new RequestEvent(IncomingRequestEventName);
        }

        public static RequestEvent OutgoingNovaRequest(HttpRequestMessage request)
        {
            var requestEvent = new RequestEvent(OutgoingNovaRequestEventName);
            requestEvent.AddOutgoingRequestDetails(request);
            return requestEvent;
        }

        public void RequestCompleted(int statusCode)
        {
            responseTimer.Stop();
            Metrics.Add("Request Duration Millis", responseTimer.ElapsedMilliseconds);
            Properties.Add("Status Code", statusCode.ToString());
        }

        public void NoResponseReceived()
        {
            Properties.Add("No Response", string.Empty);
        }
    }
}