namespace MI.PIMS.UI.Models.Config
{
    public class SmtpSettings
    {
        public string SMTPHost { get; set; }
        public string SMTPHostMailKit { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPSenderName { get; set; }
        public string FromEmailAddress { get; set; }
    }
}
