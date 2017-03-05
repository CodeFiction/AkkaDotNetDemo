using System.Collections.Generic;
using System.Linq;
using Actors.Messages;
using Akka.Actor;

namespace Actors
{
    public class WatchedVideoRepoActor : ReceiveActor
    {
        private static readonly IList<WatchedVideoEvent> Watched = new List<WatchedVideoEvent>();

        public WatchedVideoRepoActor()
        {
             

            Receive<UserWatchedVideoRequest>(request =>
            {
                int[] videoIds = Watched.Where(videos => videos.UserId == request.AttemptRecommendation.UserId)
                    .Select(videos => videos.VideoId)
                    .Distinct()
                    .ToArray();

                Sender.Tell(new UserWatchedVideoResponse(request.AttemptRecommendation, videoIds));
            });

            Receive<WatchedVideoEvent>(message =>
            {
                Watched.Add(message);
            });
        }
    }
}