namespace WpfCalculator
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class ArgumentsException : Exception
    {
        public ArgumentsException()
        {
        }

        public ArgumentsException(string message) : base(message)
        {
        }

        public ArgumentsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ArgumentsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}