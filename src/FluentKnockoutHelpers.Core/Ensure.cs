using System;

namespace FluentKnockoutHelpers.Core
{
    internal class Ensure
    {
        public static void NotNullEmptyOrWhiteSpace(string arg, string argName)
        {
            if (string.IsNullOrWhiteSpace(arg))
                throw new ArgumentException(string.Format("The argument '{0}' cannot be null, empty or whitespace", argName));
        }

        public static void NotNull<T>(T arg, string argName)
            where T : class
        {
            if (arg == null)
                throw new ArgumentException(string.Format("The argument '{0}' cannot be null", argName));
        }

        public static void NotDefault<T>(T arg, string argName)
            where T : struct
        {
            if (arg.Equals(default(T)))
                throw new ArgumentException(string.Format("The argument '{0}' cannot be it's default value of '{1}'", argName, arg));
        }
    }
}