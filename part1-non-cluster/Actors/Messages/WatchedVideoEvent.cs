namespace Actors.Messages
{
    public class WatchedVideoEvent
    {
        private readonly int _userId;
        private readonly int _videoId;

        public WatchedVideoEvent(int userId, int videoId)
        {
            _userId = userId;
            _videoId = videoId;
        }

        public int UserId
        {
            get { return _userId; }
        }

        public int VideoId
        {
            get { return _videoId; }
        }
    }
}