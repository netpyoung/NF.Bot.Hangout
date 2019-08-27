using System.Threading.Tasks;
using System.Net.Http;
using Google.Apis.HangoutsChat.v1.Data;

namespace NF.Bot.Hangout
{
    public interface ISendMessageFactory
    {
        // ref: https://developers.google.com/resources/api-libraries/documentation/chat/v1/csharp/latest/classGoogle_1_1Apis_1_1HangoutsChat_1_1v1_1_1Data_1_1Message.html
        // ref: https://developers.google.com/hangouts/chat/reference/rest/v1/spaces.messages#Message
        Task<HttpResponseMessage> SendMessage(Message message, string space);
    }
}
