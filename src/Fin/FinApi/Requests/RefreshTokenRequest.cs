﻿using System;

namespace FinApi.Requests
{
    public class RefreshTokenRequest
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }

    }
}
