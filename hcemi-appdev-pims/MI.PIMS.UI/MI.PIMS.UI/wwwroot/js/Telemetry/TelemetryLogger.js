class TelemetryLogger {
    constructor() {
        this.appInsights = window.appInsights;
        this.isEnabled = false;
        if (window.insightsEnabled !== undefined && window.insightsEnabled !== "") {
            this.isEnabled = window.insightsEnabled;
        }
    };

    trackEvent(eventName, logProperties) {
        if (this.appInsights !== undefined) {
            const properties = logProperties.toTelemetryProperties();
            this.appInsights.trackEvent({
                name: eventName,
                properties: properties
            });
        }
    };

    trackTrace(message, severity, logProperties) {
        if (this.appInsights !== undefined) {
            const properties = logProperties.toTelemetryProperties();
            this.appInsights.trackTrace({
                message: message,
                severity: severity,
                properties: properties
            });
        }
    };

    trackException(exception, logProperties) {
        if (this.appInsights !== undefined) {
            const properties = logProperties.toTelemetryProperties();
            this.appInsights.trackException({
                exception: exception,
                properties: properties
            });
        }
    };

    trackMetric(name, value) {
        if (this.appInsights !== undefined) {
            this.appInsights.trackMetric({
                name: name,
                average: value
            });
        }
    };

    isAppInsightsEnabled(key) {
        if (this.isEnabled && key) {
            return true;
        }
        return false;
    }
};