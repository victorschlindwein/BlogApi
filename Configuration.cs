namespace Blog
{
    public static class Configuration
    {
        public static string JwtKey = "98SHDKJLFNfdrs98dfdsajf9sdfs09fd7A9YFDS9GF23LRH3E9F8DFSHD9FDFN99ddfnsdf";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "curso_api_rs98dfdsa==";
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
