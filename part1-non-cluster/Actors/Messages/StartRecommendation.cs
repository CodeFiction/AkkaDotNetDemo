using System;
using Akka.Actor;

namespace Actors.Messages
{
    public class StartRecommendation
    {
        private readonly int _userId;
        private readonly IActorRef _client;

        public StartRecommendation(int userId, IActorRef client)
        {
            _userId = userId;
            _client = client;
        }

        public int UserId
        {
            get { return _userId; }
        }

        public IActorRef Client
        {
            get { return _client; }
        }
    }
}