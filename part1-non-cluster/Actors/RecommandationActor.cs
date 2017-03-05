using System;
using System.Linq;
using System.Threading.Tasks;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;
using Akka.Routing;

namespace Actors
{
    public class RecommandationActor : ReceiveActor
    {
        private readonly IActorRef _watchedVideoActor;
        private readonly IActorRef _videoRepoActor;

        private ICancelable _startAttempts;

        public RecommandationActor(IActorRef watchedVideoActor, IActorRef videoRepoActor)
        {
            _watchedVideoActor = watchedVideoActor;
            _videoRepoActor = videoRepoActor;    
            
            Attempt();      
        }

        public void Attempt()
        {
            Receive<StartRecommendation>(recommendation =>
            {
                Console.WriteLine("StartRecommendation mesajı geldi");

                _startAttempts = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.Zero, TimeSpan.FromMilliseconds(200), Self, new BeginAttempt(recommendation), ActorRefs.NoSender);

                Become(StartRecommendation);
            });
        }

        public void StartRecommendation()
        {
            Receive<BeginAttempt>(attempt =>
            {
                Task<Routees> watchedVideoRoutees = _watchedVideoActor.Ask<Routees>(new GetRoutees());
                Task<Routees> videoRoutees = _videoRepoActor.Ask<Routees>(new GetRoutees());

                Task.WhenAll(watchedVideoRoutees, videoRoutees)
                    .ContinueWith(allRoutess => new JobAttemptMessage(attempt.Recommendation, allRoutess.Result.All(routees => routees.Members.Any())))
                    .PipeTo(Self);
            });

            Receive<JobAttemptMessage>(message =>
            {
                if (!message.CanStart)
                {
                    Console.WriteLine("Film tavsiyesinde bulunulamıyor");
                    return;
                }

                _startAttempts.Cancel();

                _watchedVideoActor.Tell(new UserWatchedVideoRequest(message.Job));
            });

            Receive<UserWatchedVideoResponse>(response =>
            {
                Console.WriteLine($"{response.StartRecommendation.UserId} için önceden izlemiş olduğu video'lar getiriliyor");

                int[] watchedVideos = response.VideoIds;

                _videoRepoActor.Tell(new UserUnwatchedVideoRequest(response.StartRecommendation, watchedVideos));
            });

            Receive<UserUnwatchedVideoResponse>(response =>
            {
                Console.WriteLine($"{response.Recommendation.UserId} için video'lar tavsiye ediliyor");

                Video[] responseVideos = response.Videos;

                response.Recommendation.Client.Tell(new RecommendationResponse(responseVideos));

                Self.Tell(PoisonPill.Instance); // Özel tipte bir mesaj, actor'ün kendini yok etmesini sağlıyor. Bu adımdan itibaren actor'le işimiz kalmıyor.
            });
        }
    }
}