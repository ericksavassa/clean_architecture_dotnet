﻿namespace clean_full.Infrastructure
{
    using System;
    public class InfrastructureException : Exception
    {
        internal InfrastructureException(string businessMessage)
               : base(businessMessage)
        {
        }
    }
}
