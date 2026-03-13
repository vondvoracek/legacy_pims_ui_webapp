namespace MI.PIMS.UI.Models.Config
{
    public class AzureMailServiceSettings
    {
        public string AuthTokenUrl { get; set; }
        public string SendMailUrl { get; set; }
        public string GrantType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string FromEmailSender { get; set; }
    }
}
