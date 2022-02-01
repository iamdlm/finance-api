using System;

namespace FinApi.Responses
{
    public class ValidateRefreshTokenResponse : BaseResponse
    {
        public Guid UserId { get; set; }

    }
}
