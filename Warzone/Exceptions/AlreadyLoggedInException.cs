using System;

namespace Warzone.Exceptions
{
    public class AlreadyLoggedInException : Exception
    {
        private const string CustomMessage = "Please log out from your current session before attempting to log in again";

        public AlreadyLoggedInException()
            : base(CustomMessage)
        {
        }
    }
}