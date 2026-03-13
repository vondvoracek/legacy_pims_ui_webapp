namespace MI.PIMS.UI.Models.Config
{
    public class AppSettings
    {
        public string ApplicationName { get; set; }
        public string ServiceUrl { get; set; }
        public string AppBaseUrl { get; set; }
        public string ServiceAPIBaseUrl { get; set; }
        public string MIServicesAPIUrl { get; set; }
        public string VirtualDirectory { get; set; }
        public string ErrorReceivers { get; set; }
        public string SMTPHost { get; set; }
        public string AccessGlobalGroup { get; set; }
        public string AttachmentAllowed { get; set; }
        public string AttachmentMaxLength { get; set; }
        public string AttachmentPath { get; set; }
        public string Environment { get; set; }
        public string ClearCacheUsers { get; set; }
        public string ClearCacheCode { get; set; }
        public string OracleConnectionStrings { get; set; }
        public string CacheType { get; set; }
        public string RecordEntryMethod { get; set; }
    }
}
