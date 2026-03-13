using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BO.Dtos
{
    /// <summary>
    /// Returns DTQ Information; this class is obselete and divided into three
    /// </summary>
    public class DPOC_INV_DTQS_V_Dto
    {
        public int RowNumber { get; set; }
        public string DPOC_HIERARCHY_KEY { get; set; }
        public string DPOC_PACKAGE { get; set; }
        public string DPOC_RELEASE { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string DTQ_ATTACH_RQST_IND { get; set; }
        public string DTQ_NM { get; set; }
        public string DTQ_TYPE { get; set; }
        public string DTQ_TYPE_DESC { get; set; }
        public string DTQ_RSN { get; set; }
        public string DTQ_RSN_DESC { get; set; }
        public string REF_CD { get; set; }
        public int GDLN_DTQ_SYS_SEQ { get; set; }
        public string HOLDING_DTQ { get; set; }
        public string HOLDING_DTQ_VERSION { get; set; }
        public string TGT_DTQ { get; set; }
        public string TGT_DTQ_VERSION { get; set; }
        public string DTQ_IQ_GDLN_ID { get; set; }// USER STORY 97790 MFQ 5/15/2024
        public string DTQ_POS_APPL { get; set; }// USER STORY 97790 MFQ 5/15/2024
        public string DTQ_INCL_EXCL_CD { get; set; } // USER STORY 97790 MFQ 5/15/2024
        public string STATES_APPL { get; set; }
        public string STATES_INCL_EXCL_CD { get; set; }
        public string STATES_INCL_EXCL_DESC { get; set; }
        public string POS_APPL { get; set; }
        public string POS_INCL_EXCL_CD { get; set; }
        public string POS_INCL_EXCL_DESC { get; set; }
        public string COMMENTS { get; set; }
        public string RULE_COMMENTS { get; set; }
        //USER STORY 128895 MFQ 5/20/2025
        public string DPOC_VER_NUM { get; set; }
        public string DPOC_SOS_PROVIDER_TIN_EXCL { get; set; }
        public string DPOC_ADDTNL_RQRMNTS { get; set; }
        public string PKG_CONFIG_COMMENTS { get; set; }
    }

    public class DPOC_INV_DTQS_NM_V_Dto
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string DPOC_PACKAGE { get; set; }
        public string DPOC_RELEASE { get; set; }
        [StringLength(100)]
        public string DTQ_NM { get; set; }
        public string DTQ_TYPE { get; set; }
        public string DTQ_TYPE_DESC { get; set; }
        public string DTQ_RSN { get; set; }
        public string DTQ_RSN_DESC { get; set; }
        public string DTQ_ATTACH_RQST_IND { get; set; }
        public string MED_PLCY_REF_CODE { get; set; }
    }

    public class DPOC_INV_DTQS_TGT_V
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string DPOC_PACKAGE { get; set; }
        public string DPOC_RELEASE { get; set; }
        public string TGT_DTQ { get; set; }
        public string TGT_DTQ_VERSION { get; set; }
    }
    public class DPOC_INV_DTQS_HOLDING_V
    {
        public string DPOC_HIERARCHY_KEY { get; set; }
        public DateTime? DPOC_VER_EFF_DT { get; set; }
        public string DPOC_PACKAGE { get; set; }
        public string DPOC_RELEASE { get; set; }
        public string HOLDING_DTQ { get; set; }
        public string HOLDING_DTQ_VERSION { get; set; }
    }

    public class DTQ_TYPE_Dto
    {
        public string DTQ_TYPE { get; set; }
        public string DTQ_TYPE_DESC { get; set; }
    }    
    public class DTQ_RSN_Dto
    {
        public string DTQ_RSN { get; set; }
        public string DTQ_RSN_DESC { get; set; }
    }

    public class IQ_GDLN_Dto
    {
        public string IQ_GDLN_ID { get; set; }
        public string IQ_GDLN_DESC { get; set; }
    }
}
