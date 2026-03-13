#nullable enable

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using System.Text.Json;

namespace MI.PIMS.BL.Common.Telemetry
{
    public class LogInfo
    {
        public string Operation { get; set; } = string.Empty;
        public string Status { get; set; } = "Success";
        public string Source { get; set; } = "PIMS UI";
        public string? UserId { get; set; }
        public string? CorrelationId { get; set; }
        public object? AdditionalInfo { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = string.Empty;

        public void AutoFillCorrelationId(HttpContext? context)
        {
            if (string.IsNullOrEmpty(CorrelationId))
            {
                CorrelationId = context?.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                                ?? Activity.Current?.TraceId.ToString()
                                ?? context?.TraceIdentifier;
            }
        }

        public string GetAdditionalInfo()
        {
            if (AdditionalInfo == null)
            {
                return string.Empty;
            }

            if (AdditionalInfo is string)
            {
                return AdditionalInfo.ToStringNullSafe();
            }
            else
            {
                try
                {
                    return JsonSerializer.Serialize(AdditionalInfo, new JsonSerializerOptions { WriteIndented = true });
                }
                catch(Exception)
                {
                    return AdditionalInfo.ToStringOrNull();
                }
            }
        }

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                { "Operation", Operation },
                { "Status", Status },
                { "Source", Source },
                { "Timestamp", Timestamp.ToString("o") }
            };

            if (!string.IsNullOrEmpty(UserId)) dict["UserId"] = UserId;
            if (!string.IsNullOrEmpty(CorrelationId)) dict["CorrelationId"] = CorrelationId;
            if (AdditionalInfo != null) dict["AdditionalInfo"] = GetAdditionalInfo();
            if (!string.IsNullOrEmpty(Type)) dict["Type"] = Type;

            return dict;
        }
    }
}
