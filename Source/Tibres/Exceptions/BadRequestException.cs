using System;

namespace Tibres
{
    internal class BadRequestException()
        : Exception("HTTP request does not contain a body or required headers.")
    {
    }
}
