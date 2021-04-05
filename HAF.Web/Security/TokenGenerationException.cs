using System;

namespace HAF.Web.Security
{
    public class TokenGenerationException : Exception
    {
        public TokenGenerationException(string message)
            : base(message)
        {
        }

        public TokenGenerationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}