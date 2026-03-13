using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Models
{
    public class AppData
    {
        /// <summary>
        /// Application ASK ID *(Mandatory Field)
        /// </summary>
        public string askId { get; set; } // : "Mandatory Field: Type: string, Des: Application Ask ID >", 
        /// <summary>
        /// Application Formal Name *(Mandatory Field)
        /// </summary>
        public string name { get; set; }  // : "< Mandatory Field: Type: string, Des: Application Formal Name >", 
        /// <summary>
        /// Application environment
        /// </summary>
        public Environment environment { get; set; }
        /// <summary>
        /// CMDB Configuration Item Identifier for the application
        /// </summary>
        public string CI { get; set; }
    }

    /// <summary>
    /// enum [symbols: "DEV", "TEST", "STAGE", "UAT", "PROD"]
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Environment { DEV, TEST, STAGE, UAT, PROD }

    public class DeviceData
    {
        /// <summary>
        /// The company or vendor that created the software generating the event
        /// </summary>
        public string vendor { get; set; } // : "Mandatory Field: Type: string, Des: The company or vendor that created the software generating the event >", 
        /// <summary>
        /// The software product generating the event. For in-house developed applications, use the component name
        /// </summary>
        public string product { get; set; } // : "< Mandatory Field: Type: sting, Des: The software product generating the event. For in-house developed 
        /// <summary>
        /// Version of the software product generating the event
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// Hostname of the device generating the event
        /// </summary>
        public string hostname { get; set; } // : "< Mandatory Field: Type: string, Des: Host name (FQDN) of the device or server generating the event. >", 
        /// <summary>
        /// IPv4 address of the device generating the event in decimal format
        /// </summary>
        public long? ip4 { get; set; } // : "< Mandatory Field: Type: long, Des: IPv4 address of the device generating the event. You can use the standard IP decimal coverters >", 
        /// <summary>
        /// CMDB configuration item identifier for the device
        /// </summary>
        public string CI { get; set; }
        /// <summary>
        /// Process ID of the process generating the event
        /// </summary>
        public int? pid { get; set; }
        /// <summary>
        /// Process name associated with event. For example, Siteminder agent logs recording into web server logs or AppSense logs recording into Windows events
        /// </summary>
        public string proc { get; set; }
    }

    public class DHostData
    {
        /// <summary>
        ///Hostname, preferably FQDN
        /// </summary>
        public string hostname { get; set; }
        /// <summary>
        /// The domain portion of FQDN
        /// </summary>
        public string dnsDomain { get; set; }
        /// <summary>
        /// Windows domain
        /// </summary>
        public string ntDomain { get; set; }
        /// <summary>
        /// IPv4 address of the host in decimal format
        /// </summary>
        public long? ip4 { get; set; }
        /// <summary>
        /// IPv6 address of the host
        /// </summary>
        public double? ip6 { get; set; }
        /// <summary>
        /// URI path invoked on the destination host
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// Name of the process
        /// </summary>
        public string proc { get; set; }
        /// <summary>
        /// Id of the running process
        /// </summary>
        public int? pid { get; set; }
        /// <summary>
        /// Port of the host
        /// </summary>
        public int? port { get; set; }
        /// <summary>
        /// MAC address of the host
        /// </summary>
        public string mac { get; set; }
        /// <summary>
        /// IP address after translation by a load balancer, firewall, or proxy. X-Forwarded-For or Forwarded headers contain this information
        /// </summary>
        public long[] fwdAddr { get; set; }
        /// <summary>
        /// Port after translation by a load balancer, firewall, or proxy. X-Forwarded-Proto or Forwarded headers contain this information
        /// </summary>
        public int[] fwdPort { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Priv { guest, user, privileged_user, administrator, system, root }

    public class SUserData
    {
        /// <summary>
        /// User id. May also be system or service accounts
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// Backend ID or UUID of which represents the uid
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// Full name, username, email, or principal of user
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// First name of user
        /// </summary>
        public string firstName { get; set; }
        /// <summary>
        /// Last name of user
        /// </summary>
        public string lastName { get; set; }
        /// <summary>
        /// Commonly Guest, User, or Administrator
        /// </summary>
        public Priv? priv { get; set; }
        /// <summary>
        /// User role
        /// </summary>
        public string role { get; set; }
        /// <summary>
        /// The issuer of the token
        /// </summary>
        public string tokenIssuer { get; set; }
        /// <summary>
        /// Time the token was created in the Epoch time format
        /// </summary>
        public long tokenCreated { get; set; }
        /// <summary>
        /// Time the token expires in the Epoch time format
        /// </summary>
        public long tokenExpires { get; set; }
        /// <summary>
        /// SHA256 hash of authorization token
        /// </summary>
        public string tokenHash { get; set; }
    }
    public class SHostData
    {
        /// <summary>
        /// Hostname, preferably FQDN
        /// </summary>
        public string hostname { get; set; }
        /// <summary>
        /// The domain portion of FQDN
        /// </summary>
        public string dnsDomain { get; set; }
        /// <summary>
        /// Windows domain
        /// </summary>
        public string ntDomain { get; set; }
        /// <summary>
        /// IPv4 address of the host in decimal format.
        /// </summary>
        public long? ip4 { get; set; }
        /// <summary>
        /// IPv6 address of the host
        /// </summary>
        public double? ip6 { get; set; }
        /// <summary>
        /// URI path invoked on the source host
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// Name of the process
        /// </summary>
        public string proc { get; set; }
        /// <summary>
        /// Id of the running process
        /// </summary>
        public int? pid { get; set; }
        /// <summary>
        /// Port of the host
        /// </summary>
        public int? port { get; set; }
        /// <summary>
        /// MAC address of the host
        /// </summary>
        public string mac { get; set; }
        /// <summary>
        /// IP address after translation by a load balancer, firewall, or proxy. X-Forwarded-For or Forwarded headers contain this information
        /// </summary>
        public long[] fwdAddr { get; set; }
        /// <summary>
        /// Port after translation by a load balancer, firewall, or proxy. X-Forwarded-Proto or Forwarded headers contain this information
        /// </summary>
        public int[] fwdPort { get; set; }
    }
    public class DUserData
    {
        /// <summary>
        /// User id. May also be system or service accounts
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// Backend ID or UUID of which represents the uid
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// Full name, username, email, or principal of user
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// First name of user
        /// </summary>
        public string firstName { get; set; }
        /// <summary>
        /// Last name of user
        /// </summary>
        public string lastName { get; set; }
        /// <summary>
        /// Commonly Guest, User, or Administrator
        /// </summary>
        public Priv? priv { get; set; }
        /// <summary>
        /// User role
        /// </summary>
        public string role { get; set; }
        /// <summary>
        /// The issuer of the token
        /// </summary>
        public string tokenIssuer { get; set; }
        /// <summary>
        /// Time the token was created in the Epoch time format
        /// </summary>
        public long tokenCreated { get; set; }
        /// <summary>
        /// Time the token expires in the Epoch time format
        /// </summary>
        public long tokenExpires { get; set; }
        /// <summary>
        /// SHA256 hash of authorization token
        /// </summary>
        public string tokenHash { get; set; }
    }
    public class RequestData
    {
        /// <summary>
        /// URL requested
        /// </summary>
        public string request { get; set; }
        /// <summary>
        /// The query string from the original URL 
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// he user-agent making the request
        /// </summary>
        public string userAgent { get; set; }
        /// <summary>
        /// Method used for request (GET, POST, etc)
        /// </summary>
        public string method { get; set; }
        /// <summary>
        /// Identifier to associate events in a single near user session. Not an actual session cookies that could be replayed
        /// </summary>
        public string cookies { get; set; }
        /// <summary>
        /// Contents of Optum_CID_Ext header
        /// </summary>
        public string Optum_CID_Ext { get; set; }
        /// <summary>
        /// Contents of Referer header
        /// </summary>
        public string referer { get; set; }
        /// <summary>
        /// Bytes transferred from subject(source) to object(destination) or device
        /// </summary>
        [JsonProperty(PropertyName = "in")]
        public long? in_field { get; set; }
        /// <summary>
        /// Bytes transferred from device or object(destination) to subject(source)
        /// </summary>
        [JsonProperty(PropertyName = "out")]
        public long? out_field { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataType { UnitedHealth_Group_Protected, confidential, Public }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Operation { Read, Modify, Create, Delete, Copy }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OldDataType { UnitedHealth_Group_Protected, confidential, Public }

    public class RecordData
    {
        /// <summary>
        /// Name of the file without path
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// ID associated with the file or record. May be inode for files
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Classification of data in file or record
        /// </summary>
        public DataType? dataType { get; set; }
        /// <summary>
        /// Description of file or record. Does it contain PHI or PII
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// Size of the file in bytes, integer value
        /// </summary>
        public long? size { get; set; }
        /// <summary>
        /// When was the file created
        /// </summary>
        public long? createTime { get; set; }
        /// <summary>
        /// When was the file last modified
        /// </summary>
        public long? modTime { get; set; }
        /// <summary>
        /// What is the path of the file being accessed, created or modified
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// What are the permissions associated with the file?
        /// </summary>
        public string permission { get; set; }
        /// <summary>
        /// Type of file or record
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Operation? operation { get; set; }
        /// <summary>
        /// ID used to locate the change in data storage or logs
        /// </summary>
        public string transactionId { get; set; }
        /// <summary>
        /// Name of old file if name changes happened
        /// </summary>
        public string oldName { get; set; }
        /// <summary>
        /// ID of old file if changed
        /// </summary>
        public string oldid { get; set; }
        /// <summary>
        /// Previous classification of data in file or record
        /// </summary>
        public OldDataType? oldDataType { get; set; }
        /// <summary>
        /// Description of old file if changed
        /// </summary>
        public string oldDesc { get; set; }
        /// <summary>
        /// The previous size of the file in bytes, integer value
        /// </summary>
        public long oldSize { get; set; }
        /// <summary>
        /// When was the old file created
        /// </summary>
        public long? oldCreateTime { get; set; }
        /// <summary>
        /// When was the old file last modified
        /// </summary>
        public long? oldModTime { get; set; }
        /// <summary>
        /// What was the path of the file being accessed, created or modified
        /// </summary>
        public string oldPath { get; set; }
        /// <summary>
        /// What are the permissions associated with the old file?
        /// </summary>
        public string oldPermission { get; set; }
        /// <summary>
        /// Type of file or record if changed
        /// </summary>
        public string oldType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogClass { SECURITY_SUCCESS, SECURITY_FAILURE, SECURITY_AUDIT, NONSECURITY, UNCATEGORIZED, E1, E2, E3, E4, E5, E6, E7, E8, E9, E10, E11 }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Severity { EMERG, ALERT, CRIT, ERR, WARNING, NOTICE, INFO, DEBUG, TRACE }

    public class LogEvent
    {
        /// <summary>
        /// Time the event was received by the application in the Epoch time format *(Mandatory Field)
        /// </summary>
        public long? receivedTime { get; set; }
        /// <summary>
        /// Avro record describing the application *(Mandatory Field)
        /// </summary>
        public AppData application { get; set; }
        /// <summary>
        /// Avro record describing the recording device *(Mandatory Field)
        /// </summary>
        public DeviceData device { get; set; }
        /// <summary>
        /// Pre-defined list of symbols to document what type of security log the event belongs to
        /// </summary>
        public LogClass logClass { get; set; }  // : "< Type: enum [symbols:”SECURITY_SUCCESS”, “SECURITY_FAILURE”, “SECURITY_AUDIT”, “NONSECURITY”, “UNCATEGORIZED”, “E1”, “E2”, “E3”, “E4”, “E5” ,”E6”, “E7”, “E8”, “E9”, “E10”, “E11”], Des: Pre-defined list of symbols to document what type of security log the event belongs to >", 
        /// <summary>
        /// 0-7 logging levels
        /// </summary>
        public Severity severity { get; set; }   // : "< Type: enum [symbols:”EMERG”, “ALERT”, “CRIT”, “ERR”, “WARNING”, “NOTICE”, “INFO”, “DEBUG”, “TRACE”], Des: 0-7 logging levels >",
        /// <summary>
        /// Event class IDs set by the application team. Separate from security event tags
        /// </summary>
        public string eventClass { get; set; }
        /// <summary>
        /// Unique log record identifier from the source system
        /// </summary>
        public string externalId { get; set; }
        /// <summary>
        /// Type or category of event, such as: Login Success, RESOURCE ACCESS, or Authorization Failure
        /// </summary>
        public string name { get; set; }  // : "< Type: string, Des: Name of the event >", 
        /// <summary>
        /// Log message (Mandatory Field)
        /// </summary>
        public string msg { get; set; }  // "< Mandatory Field: Type: string, Des: Description of the event in English >", 
        /// <summary>
        /// Avro record describing the destination or target host
        /// </summary>        
        public DHostData destHost { get; set; }
        /// <summary>
        /// Avro record describing the destination or target user
        /// </summary>
        public DUserData destUser { get; set; }
        /// <summary>
        /// Avro record describing the source or subject host
        /// </summary>
        public SHostData sourceHost { get; set; }
        /// <summary>
        /// Avro record describing the source or subject user
        /// </summary>
        public SUserData sourceUser { get; set; }
        /// <summary>
        /// Avro record describing a web request
        /// </summary>
        public RequestData request { get; set; }
        /// <summary>
        /// An array of avro records describing records or files affected by the event
        /// </summary>
        public RecordData fileRecord { get; set; }
        /// <summary>
        /// Start time of the event described in the Epoch time format. For example, start of a session
        /// </summary>
        public long? start { get; set; }   // : "< Type: long, Des: Start time of the event described. For example, start of a session >", 
        /// <summary>
        /// End time of event in the Epoch time format. For example, end of a session
        /// </summary>
        public long? end { get; set; }  // : "< Type: long, Des: End time of event. For example, end of a session >", 
        /// <summary>
        /// 	Action taken by device
        /// </summary>
        public string act { get; set; }  // " : "< Type: string, Des: Action taken by device >", 
        /// <summary>
        /// Success or failure, if event is a request
        /// </summary>
        public string outcome { get; set; }  //" : "< Type: string, Des: Success or failure, if event is a request >", 
        /// <summary>
        /// Reason for failure, if known. For example: bad credentials
        /// </summary>
        public string reason { get; set; }  //" : "< Type: string, Des: Reason for failure, if known. For example: bad credentials >", 
        /// <summary>
        /// Application level protocol, example values are: HTTP, HTTPS, SSHv2, Telnet, POP, IMAP, IMAPS, etc
        /// </summary>
        public string appProto { get; set; }  // " : "< Type: string, Des: Application level protocol, example values are: HTTP, HTTPS, SSHv2, Telnet, POP, IMAP, IMAPS, etc >", 
        /// <summary>
        /// Layer-4 protocol used. Most commonly TCP or UDP
        /// </summary>
        public string txProto { get; set; }  // " : "< Type: string, Des: Layer-4 protocol used. Most commonly TCP or UDP >", 
        /// <summary>
        /// Optional tags for the event
        /// </summary>
        public string[] tags { get; set; }  // " : "< Type: array [type:string], Des: Optional tags for the event >", 
        // additionalFields { get; set; }  // " : "< Type: Avro map, Des: Optional space to map fields that do not fit in the schema elsewhere >"
    }

}
