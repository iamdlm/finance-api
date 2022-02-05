namespace Fin.Application.DTOs
{
    public class TokenSettingsDto
    {
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public string TokenSecret { get; set; }
    }
}
