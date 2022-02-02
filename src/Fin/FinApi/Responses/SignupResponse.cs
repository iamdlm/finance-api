namespace FinApi.Responses
{
    public class SignupResponse : BaseResponse
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
