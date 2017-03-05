using System;
using Actors.Messages;
using Akka.Actor;

namespace Client
{
    public class ConsoleLoggerActor : ReceiveActor
    {
        public ConsoleLoggerActor()
        {
            Receive<RecommendationResponse>(response =>
            {
                foreach (var responseResponseVideo in response.ResponseVideos)
                {
                    Console.WriteLine(responseResponseVideo);
                }
            });
        }
    }
}
