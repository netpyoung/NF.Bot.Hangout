using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Cloud.PubSub.V1;
using Google.Apis.HangoutsChat.v1.Data;
using Google.Apis.HangoutsChat.v1;
using static Google.Apis.HangoutsChat.v1.SpacesResource;
using Grpc.Core;
using Grpc.Auth;
using Newtonsoft.Json;

namespace NF.Bot.Hangout
{
    public class Runner
    {
        public Runner(RunnerArgs args, IReceiveMessageHandler handler)
        {
            string jsonpath = args.ServiceAccountJsonFpath;
            string projectId = args.PubSubProjectId;
            string subscriptionId = args.PubSubSubscriptionId;

            GoogleCredential googleCredential = GoogleCredential.FromFile(jsonpath).CreateScoped(SCOPES);
            ChannelCredentials channelCredentials = googleCredential.ToChannelCredentials();
            Channel channel = new Channel(SubscriberServiceApiClient.DefaultEndpoint.Host, SubscriberServiceApiClient.DefaultEndpoint.Port, channelCredentials);
            SubscriberServiceApiClient subscriberService = SubscriberServiceApiClient.Create(channel);
            SubscriptionName subscriptionName = new SubscriptionName(projectId, subscriptionId);
            SubscriberClient.ClientCreationSettings settings = new SubscriberClient.ClientCreationSettings(credentials: channelCredentials);
            SubscriberClient subscriber = SubscriberClient.CreateAsync(subscriptionName, settings).Result;

            this._factory = new MessageFactory(googleCredential);
            this._subscriber = subscriber;
            this._handler = handler;
        }

        public Task Start()
        {
            return _subscriber.StartAsync(async (msg, cancellationToken) =>
            {
                string json = msg.Data.ToStringUtf8();
                DeprecatedEvent evt = JsonConvert.DeserializeObject<DeprecatedEvent>(json);
                return await this._handler.Handle(_factory, evt);
            });
        }

        public async Task Stop()
        {
            await _subscriber.StopAsync(CancellationToken.None);
        }

        #region private
        private static readonly string[] SCOPES = new string[] { "https://www.googleapis.com/auth/chat.bot", "https://www.googleapis.com/auth/pubsub" };
        private SubscriberClient _subscriber;
        private MessageFactory _factory;
        private IReceiveMessageHandler _handler;

        internal class MessageFactory : ISendMessageFactory
        {
            private HangoutsChatService _hangouts;

            public MessageFactory(GoogleCredential googleCredential)
            {
                this._hangouts = new HangoutsChatService(new BaseClientService.Initializer { HttpClientInitializer = googleCredential });
            }

            public Task<HttpResponseMessage> SendMessage(Message message, string space)
            {
                MessagesResource.CreateRequest createRequest = this._hangouts.Spaces.Messages.Create(message, space);
                HttpRequestMessage httpRequestMessage = createRequest.CreateRequest();
                return this._hangouts.HttpClient.SendAsync(httpRequestMessage);
            }
        }
        #endregion // private
    }
}
