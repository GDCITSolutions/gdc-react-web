namespace BE.LocalAccountabilitySystem.Common.Util
{
    public partial class Util
    {
        /// <summary>
        /// Utility class containing methods for guarding against malformed or missing dependencies.
        /// </summary>
        public static class Guard
        {
            /// <summary>
            /// Verifies argument provided is not null. If so, throw exception including the provided argument name.
            /// </summary>
            /// <param name="argument"></param>
            /// <param name="argumentName"></param>
            /// <exception cref="ArgumentNullException"></exception>     
            public static void ArgumentIsNotNull(object argument, string argumentName)
            {
                if (argument == null)
                    throw new ArgumentNullException(argumentName);
            }

            /// <summary>
            /// Verify arguments provided are not null. If any are null, exception will be thrown.
            /// </summary>
            /// <param name="arguments">Arguments to verify.</param>
            /// <exception cref="ArgumentNullException"></exception>    
            public static void ArgumentsAreNotNull(params object[] arguments)
            {
                foreach (var arg in arguments)
                    ArgumentIsNotNull(arg, nameof(arg));
            }
        }
    }
}
