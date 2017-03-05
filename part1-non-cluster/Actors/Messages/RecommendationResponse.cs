using Actors.Models;

namespace Actors.Messages
{
    public class RecommendationResponse
    {
        private readonly Video[] _responseVideos;

        public RecommendationResponse(Video[] responseVideos)
        {
            _responseVideos = responseVideos;
        }

        public Video[] ResponseVideos
        {
            get { return _responseVideos; }
        }
    }
}