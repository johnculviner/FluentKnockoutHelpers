using System;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// this class performs null checks
    /// </summary>
    internal class Ensure
    {
        /// <summary>
        /// Checks to make sure the the argument is not null, empty, or white space
        /// </summary>
        /// <param name="arg">the argument to check</param>
        /// <param name="argName">the name of the argument to be added to error message</param>
        public static void NotNullEmptyOrWhiteSpace(string arg, string argName)
        {
            if (string.IsNullOrWhiteSpace(arg))
                throw new ArgumentException(string.Format("The argument '{0}' cannot be null, empty or whitespace", argName));
        }

        /// <summary>
        /// Checks to make sure the the generic type argument is not null
        /// </summary>
        /// <typeparam name="T">the generic type of class</typeparam>
        /// <param name="arg">the argument</param>
        /// <param name="argName">the name of the argument to be added to error message</param>
        public static void NotNull<T>(T arg, string argName)
            where T : class
        {
            if (arg == null)
                throw new ArgumentException(string.Format("The argument '{0}' cannot be null", argName));
        }

        /// <summary>
        /// Checks to make sure the the generic type argument is not the default value
        /// </summary>
        /// <typeparam name="T">the generic type of struct</typeparam>
        /// <param name="arg">the argument</param>
        /// <param name="argName">the name of the argument to be added to error message</param>
        public static void NotDefault<T>(T arg, string argName)
            where T : struct
        {
            if (arg.Equals(default(T)))
                throw new ArgumentException(string.Format("The argument '{0}' cannot be it's default value of '{1}'", argName, arg));
        }
    }
}