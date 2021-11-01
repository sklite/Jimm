namespace JimmyCms.Domain.Settings
{
    public class SecuritySettings
    {
        public string Key { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
    }
}