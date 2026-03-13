namespace MI.PIMS.BL.Common.Telemetry
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics;

    public class TelemetryLogger<T>
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<T> _logger;
        private readonly Helper _helper;
        private string _MSID = string.Empty;

        public TelemetryLogger(TelemetryClient telemetryClient, ILogger<T> logger, Helper helper)
        {
            _telemetryClient = telemetryClient;
            _logger = logger;
            _helper = helper;

            try
            {
                _MSID = _helper.GetMSID();
            }
            catch (Exception) {
                _MSID = "";
            }

            _telemetryClient.Context.User.AuthenticatedUserId = _MSID;
        }

        public string GetAppInsightsOperationId
        {
            get
            {
                try
                {
                    var activity = Activity.Current;
                    return activity?.RootId ?? activity?.Id;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public void TrackEvent(string eventName, LogInfo log = null)
        {
            _logger.LogInformation("Telemetry Event: {EventName}", eventName);
            _telemetryClient.TrackEvent(eventName, log.ToDictionary());
        }

        public void TrackTrace(string message, SeverityLevel severity = SeverityLevel.Information, LogInfo log = null)
        {
            _logger.LogInformation("Telemetry Trace: {Message}", message);
            _telemetryClient.TrackTrace(message, severity, log.ToDictionary());
        }

        public void TrackException(Exception ex, LogInfo log = null)
        {
            _logger.LogError(ex, "Telemetry Exception: {Message}", ex.Message);
            _telemetryClient.TrackException(ex, log.ToDictionary());
        }

        public void TrackMetric(string name, double value)
        {
            _logger.LogInformation("Telemetry Metric: {Metric} = {Value}", name, value);
            _telemetryClient.GetMetric(name).TrackValue(value);
        }
    }
}