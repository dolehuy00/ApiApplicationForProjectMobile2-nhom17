namespace MovieAppApi.DTO
{
    public class FirebaseUserTokenInfo
    {
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string Iss { get; set; } = null!;
        public string Aud { get; set; } = null!;
        public int? Iat { get; set; }
        public int? Exp { get; set; }
        public string? FacebookId { get; set; }
        public string? SignInProvider { get; set; }
        public string FirebaseUserId { get; set; } = null!;
    }
}
