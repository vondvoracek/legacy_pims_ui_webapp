using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MI.PIMS.UI
{
    public enum Duration
    {
        Second = 1,
        Minute = 60,
        Hour = 3600,
        Day = 86400,
        TwoDays = 172800,
        ThreeDays = 259200

    }
    public enum Roles
    {

        [Description("Super Admin")]
        SuperAdmin = 1,
        
        [Description("EPAL Read Only")]
        EPALReadOnly = 2,
        
        [Description("EPAL Read Write")]
        EPALReadWrite = 3,
        
        [Description("EPAL Admin")]
        EPALAdmin = 4,
        
        [Description("PayCode Read Only")]
        PayCodeReadOnly = 5,
        
        [Description("PayCode Read Write")]
        PayCodeReadWrite = 6,
        
        [Description("PayCode Admin")]
        PayCodeAdmin = 7,
        
        [Description("DPOC Read Only")]
        DPOCReadOnly = 8,
        
        [Description("DPOC Read Write")]
        DPOCReadWrite = 9,
        
        [Description("DPOC Admin")]
        DPOCAdmin = 10,
        
        [Description("UMR ReadOnly")]
        UMRReadOnly = 11,
        
        [Description("UMR Read Write")]
        UMRReadWrite = 12,
        
        [Description("UMR Admin")]
        UMRAdmin = 13,

        [Description("CMP Read Only")]
        CMPReadOnly = 18,

        [Description("CMP Read Write")]
        CMPReadWrite = 19,

        [Description("CMP Admin")]
        CMPAdmin = 20,

        [Description("CMP Reports Only")]
        CMPReportsOnly = 21,

        [Description("Donor Records Read Only")]
        DonorRecordsReadOnly = 22,

        [Description("Donor Records Read Write")]
        DonorRecordsReadWrite = 23,

        [Description("Donor Records Admin")]
        DonorRecordsAdmin = 24,

        [Description("Donor Records Reports Only")]
        DonorRecordsReportsOnly = 25,

        [Description("ICHRA Read Only")]
        ICHRAReadOnly = 26,

        [Description("ICHRA Read Write")]
        ICHRAReadWrite = 27,

        [Description("ICHRA Admin")]
        ICHRAAdmin = 28,

        [Description("ICHRA Reports Only")]
        ICHRAReportsOnly = 29,
    }

    public enum EPALPageView
    {
        [Description("AddDetail")]
        AddDetail = 1,
        [Description("EditDetail")]
        EditDetail = 2,
        [Description("ViewDetail")]
        ViewDetail = 3,        
        [Description("DuplicateRecordDetail")]
        DuplicateRecordDetail = 4,
        [Description("AddNew")]
        AddNew = 5
    }

    public enum DPOCPageView
    {
        [Description("AddDetail")]
        AddDetail = 6,
        [Description("EditDetail")]
        EditDetail = 7,
        [Description("ViewDetail")]
        ViewDetail = 8,
        [Description("DuplicateRecordDetail")]
        DuplicateRecordDetail = 9,
        [Description("AddNew")]
        AddNew = 10
    }

    public enum PayCodePageView
    {
        [Description("AddDetail")]
        AddDetail = 11,
        [Description("EditDetail")]
        EditDetail = 12,
        [Description("ViewDetail")]
        ViewDetail = 13,
        [Description("DuplicateRecordDetail")]
        DuplicateRecordDetail = 14,
        [Description("AddNew")]
        AddNew = 15
    }

    public enum EPALDrivingStatusEnum
    {
        [Description("Prior Auth with SOS")]
        PASOS = 0,
        [Description("Prior Auth")]
        PA = 1,
        [Description("Pre-Determination")]
        Pred = 2,
        [Description("Advanced Notification")]
        Adv = 3,
        [Description("Advanced Notification with SOS")]
        AdvSOS = 9,
        [Description("Drug Review At Launch")]
        DRAL = 4,
        [Description("Auto Approval")]
        AA = 5,
        [Description("Medicare Special Processing")]
        MSP = 6,
        [Description("ALL")]
        ALL = 7,
        [Description("NONE")]
        NONE = 8,
        [Description("Prior Auth Suspension")]
        PAS = 9,
        [Description("Pre-Determination Suspension")]
        PredS = 10,
        [Description("Drug Review At Launch Suspension")]
        DRALS = 11,
        [Description("Advanced Notification Suspension")]
        AdvS = 12,
        [Description("Auto Approval Suspension")]
        AAS = 13,
        [Description("Site of Service Suspension")]
        SOSS = 14,
        [Description("Medical Special Processing Suspension")]
        MSPS = 15
    }

    public static class RolesExtensions
    {
        public static string ToDescriptionString(this Roles val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }

    public class AuthorizeRolesAttribute: Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Roles[] allowedRoles)
        {
            //var allowedRolesAsStrings = allowedRoles.Select(x => x.ToDescriptionString()); // Enum.GetName(typeof(Roles), x)            
            List<int> allowedRolesAsStrings = (((Roles[])allowedRoles).Select(role => (int)role)).ToList();            
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
