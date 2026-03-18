namespace Share.Extensions.Exceptions
{
    using System;

    public class OracleSqlException : Exception
    {
        public OracleSqlException(string message) : base(message)
        {
        }
    }
}