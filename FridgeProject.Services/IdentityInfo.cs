namespace FridgeProject.Services
{
    public class IdentityInfo
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifeTimeInMSeconds { get; set; }
        public string Key { get; set; }
    }
}
