class LogInfo {
    constructor(operationName, userId, additionalInfo, type = "", correlationId = "", status = "Success", source = "PIMS UI", timestamp = new Date()) {
        this.operationName = operationName;
        this.userId = userId;
        this.status = status;
        this.source = source;
        this.correlationId = correlationId;
        this.type = type;
        this.additionalInfo = additionalInfo;
        this.timestamp = timestamp.toISOString();
    };

    getAdditionalInfo() {
        try {
            const parsed = JSON.parse(this.additionalInfo);

            if (typeof parsed === 'object' && parsed !== null) {
                return JSON.stringify(this.additionalInfo);
            }
            else {
                return this.additionalInfo.toString();
            }
        }
        catch {
            return this.additionalInfo.toString();
        }
    };

    // Method to serialize the object properties to a format suitable for Application Insights
    toTelemetryProperties() {
        return {
            "OperationName": this.operationName,
            "UserId": this.userId,
            "Status": this.status,
            "Source": this.source,
            "CorrelationId": this.correlationId,
            "Type": this.type,
            "Timestamp": this.timestamp,
            "AdditionalInfo": this.getAdditionalInfo()
        };
    };
};

