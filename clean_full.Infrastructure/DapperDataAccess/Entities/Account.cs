﻿namespace clean_full.Infrastructure.DapperDataAccess.Entities
{
    using System;

    public class Account
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
    }
}
