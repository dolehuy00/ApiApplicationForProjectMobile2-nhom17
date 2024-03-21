namespace MovieAppApi.DTO
{
    public class ForgotDTO
    {
        public string Email { get; set; } = null!;
        public int Code { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
    }
}
