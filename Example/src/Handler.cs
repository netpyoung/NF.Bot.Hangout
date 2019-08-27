using Google.Apis.HangoutsChat.v1.Data;
using Google.Cloud.PubSub.V1;
using NF.Bot.Hangout;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Example
{
    public class Handler : IReceiveMessageHandler
    {
        async Task<SubscriberClient.Reply> IReceiveMessageHandler.Handle(ISendMessageFactory factory,
            DeprecatedEvent evt)
        {
            string spaceId = evt.Space.Name;
            string threadId = evt.Message.Thread.Name;
            Message hangoutMessage = new Message
            {
                Thread = new Thread {Name = threadId},
                Text = $"echo> {evt.Message.ArgumentText}"
            };
            HttpResponseMessage res = await factory.SendMessage(hangoutMessage, spaceId);
            string contents = await res.Content.ReadAsStringAsync();
            HttpStatusCode statusCode = res.StatusCode;
            Console.WriteLine($"response: {statusCode}, {contents}");
            return SubscriberClient.Reply.Ack;
        }

        private readonly Regex _reg = new Regex("^!([a-zA-Z0-9]+)",RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public async Task<int> Hello()
        {
            string text = "!hello world nice";

            MatchCollection matches = _reg.Matches(text);
            Match match = matches[0];

            string cmd = match.Groups[1].Value;
            switch (cmd)
            {
                case "hello":
                    return RunInvokeTask("bamboo-update");
            }

            return 0;
        }
        
        
        int RunInvokeTask(string taskName)
        {
            using (new Dir("/Users/pyoung/company/integrated/CITools/pytasks"))
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/zsh",
                        Arguments = $@"-c ""PYENV_VERSION=3.6.4 ~/.pyenv/shims/python -m invoke {taskName}""",
                        WorkingDirectory = Directory.GetCurrentDirectory(),
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };
                    process.Start();
                    process.WaitForExit();
                    return process.ExitCode;
                }
            }
        }
    }
    

    public class Dir : IDisposable
    {
        public Dir(string new_dir)
        {
            OriginDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(new_dir);
            Environment.CurrentDirectory = new_dir;
            CurrentDir = new_dir;
        }

        public string CurrentDir { get; }
        public string OriginDir { get; }

        public void Dispose()
        {
            Directory.SetCurrentDirectory(OriginDir);
            Environment.CurrentDirectory = OriginDir;
        }
    }
}