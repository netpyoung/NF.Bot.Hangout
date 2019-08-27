using System.Threading.Tasks;
using Google.Cloud.PubSub.V1;
using Google.Apis.HangoutsChat.v1.Data;

namespace NF.Bot.Hangout
{
    public interface IReceiveMessageHandler
    {
        // ref: https://developers.google.com/hangouts/chat/reference/message-formats/events
        // ref: https://developers.google.com/hangouts/chat/reference/rest/v1/Event
        // ref: https://developers.google.com/resources/api-libraries/documentation/chat/v1/csharp/latest/classGoogle_1_1Apis_1_1HangoutsChat_1_1v1_1_1Data_1_1DeprecatedEvent.html
        Task<SubscriberClient.Reply> Handle(ISendMessageFactory factory, DeprecatedEvent evt);
    }
}
