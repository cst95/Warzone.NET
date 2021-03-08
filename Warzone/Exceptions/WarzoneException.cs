using System;

namespace Warzone.Exceptions
{
    public class WarzoneException : Exception
    {
        private const string CustomMessage = "Something went wrong whilst fetching warzone data";

        public WarzoneException()
            : base(CustomMessage)
        {
        }
    }
}