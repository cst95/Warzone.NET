using System;

namespace Warzone.Exceptions
{
    public class NotLoggedInException : Exception
    {
        private const string CustomMessage = "Please log in before attempting this action";

        public NotLoggedInException()
            : base(CustomMessage)
        {
        }
    }
}