using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Actors;
using Actors.Messages;
using Akka.Actor;
using Akka.Routing;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSystem actorSystem = ActorSystem.Create("moviedb");

            IActorRef watchedVideo = actorSystem.ActorOf(Props.Create<WatchedVideoRepoActor>().WithRouter(FromConfig.Instance), "watchedVideo");
            IActorRef videoRepo = actorSystem.ActorOf(Props.Create<VideoRepoActor>().WithRouter(FromConfig.Instance), "videoRepo");
            IActorRef consoleLogger = actorSystem.ActorOf(Props.Create<ConsoleLoggerActor>(), "logger");

            IActorRef apiActor = actorSystem.ActorOf(Props.Create<ApiActor>(watchedVideo, videoRepo), "api");

            apiActor.Tell(new LoginMessage(20), consoleLogger);

            apiActor.Tell(new WatchedVideoEvent(20, 0));
            apiActor.Tell(new WatchedVideoEvent(20, 4));
            apiActor.Tell(new WatchedVideoEvent(20, 7));
            apiActor.Tell(new WatchedVideoEvent(20, 10));
            apiActor.Tell(new WatchedVideoEvent(20, 2));

            Thread.Sleep(2000);

            apiActor.Tell(new LoginMessage(20), consoleLogger);

            Console.Read();

            actorSystem.Terminate();
        }
    }
}
