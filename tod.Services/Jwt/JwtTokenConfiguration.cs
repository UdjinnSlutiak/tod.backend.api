namespace Tod.Services.Jwt
{
	public class JwtTokenConfiguration
	{
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiresInMinutes { get; set; }
        public int RefreshTokenExpiresInMinutes { get; set; }
    }
}
