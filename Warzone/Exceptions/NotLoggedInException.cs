using System;

namespace Warzone.Exceptions
{
    public class NotLoggedInException : Exception
    {
        private const string CustomMessage = "You can only log out if you are currently logged in.";

        public NotLoggedInException()
            : base(CustomMessage)
        {
        }
    }
}