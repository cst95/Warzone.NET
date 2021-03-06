using System;
using System.Collections.Generic;
using System.Linq;

namespace Warzone.Constants
{
    public static class Platforms
    {
        public static bool IsValid(string platform) =>
            ValidPlatforms.Any(p => p.Equals(platform, StringComparison.OrdinalIgnoreCase));

        public const string Xbox = "xbl";
        public const string Activision = "acti";
        public const string Psn = "psn";

        private static readonly IEnumerable<string> ValidPlatforms = new List<string> {Xbox, Activision, Psn};
    }
}