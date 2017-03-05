using System;
using Actors.Messages;
using Akka.Actor;

namespace Actors
{
    public class ApiActor : ReceiveActor
    {
        private readonly IActorRef _watchedVideoActor;
        private readonly IActorRef _videoActor;

        public ApiActor(IActorRef watchedVideoActor, IActorRef videoActor)
        {
            _watchedVideoActor = watchedVideoActor;
            _videoActor = videoActor;

            Receive<LoginMessage>(message =>
            {
                Console.WriteLine($"{message.UserId} için login talebi geldi");

                IActorRef recActor = Context.ActorOf(Props.Create<RecommandationActor>(watchedVideoActor, videoActor));

                recActor.Tell(new StartRecommendation(message.UserId, Sender));
            });

            Receive<WatchedVideoEvent>(videos =>
            {
                _watchedVideoActor.Tell(videos);
            });
        }
    }
}
