﻿using System.Text.Json.Serialization;

namespace FinApi.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
