namespace NF.Bot.Hangout
{
    public class RunnerArgs
    {
        // ref: Service Account: https://developers.google.com/hangouts/chat/how-tos/service-accounts
        // ref: Hgnouts/chat - pubsub: https://developers.google.com/hangouts/chat/how-tos/pub-sub
        public RunnerArgs(string serviceAccountJsonFpath, string pubSubProjectId, string pubSubSubscriptionId)
        {
            this.ServiceAccountJsonFpath = serviceAccountJsonFpath;
            this.PubSubProjectId = pubSubProjectId;
            this.PubSubSubscriptionId = pubSubSubscriptionId;
        }
        public string PubSubProjectId { get; }
        public string PubSubSubscriptionId { get; }
        public string ServiceAccountJsonFpath { get; }
    }
}
