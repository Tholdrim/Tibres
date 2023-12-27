using System;

namespace Tibres
{
    internal class BadRequestException()
        : Exception("The HTTP request does not contain a body or the required headers.")
    {
    }
}
