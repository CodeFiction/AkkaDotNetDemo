using Actors.ExternalApi;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;

namespace Actors
{
    public class VideoRepoActor : ReceiveActor
    {
        private readonly MovieRepository _movieRepository;

        public VideoRepoActor()
        {
            _movieRepository = new MovieRepository();

            Receive<UserUnwatchedVideoRequest>(request =>
            {
                IActorRef actorRef = Sender;
                StartRecommendation startRecommendation = request.ResponseStartRecommendation;

                _movieRepository.GetUnseenVideosAsync(request.Videos).ContinueWith(task =>
                {
                    Video[] taskResult = task.Result;

                    actorRef.Tell(new UserUnwatchedVideoResponse(startRecommendation, taskResult));
                });
            });
        }
    }
}