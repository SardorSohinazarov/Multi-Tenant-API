﻿namespace Admin.Domain.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not found")
        {
        }

        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}