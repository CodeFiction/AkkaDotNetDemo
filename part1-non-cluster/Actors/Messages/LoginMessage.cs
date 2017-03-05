namespace Actors.Messages
{
    public class LoginMessage
    {
        private readonly int _userId;

        public LoginMessage(int userId)
        {
            _userId = userId;
        }

        public int UserId
        {
            get { return _userId; }
        }
    }
}