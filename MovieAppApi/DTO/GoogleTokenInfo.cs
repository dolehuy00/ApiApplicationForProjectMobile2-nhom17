namespace MovieAppApi.DTO
{
    public class GoogleTokenInfo
    {
        public string? Sub { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Picture { get; set; }
        public string Iss { get; set; } = null!;
        public string Azp { get; set; } = null!;
        public string Aud { get; set; } = null!;
        public long? Iat { get; set; }
        public long? Exp { get; set; }
    }
}
