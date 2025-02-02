namespace Atlas.DonorImport.ExternalInterface.Settings.ServiceBus
{
    public class MessagingServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string MatchingDonorUpdateTopic { get; set; }

        // ReSharper disable once UnusedMember.Global - Used in function binding, and included here for completeness
        public string ImportFileSubscription { get; set; }

        // ReSharper disable once UnusedMember.Global - Used in function binding, and included here for completeness
        public string ImportFileTopic { get; set; }
    }
}