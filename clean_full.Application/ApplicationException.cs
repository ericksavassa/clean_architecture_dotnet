namespace clean_full.Application
{
    using System;
    public class ApplicationException : Exception
    {
        internal ApplicationException(string businessMessage)
               : base(businessMessage)
        {
        }
    }
}
