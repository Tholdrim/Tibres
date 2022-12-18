using System;

namespace Tibres
{
    internal class BadRequestException : Exception
    {
        public BadRequestException()
            : base("HTTP request does not contain a body or required headers.")
        {
        }
    }
}
