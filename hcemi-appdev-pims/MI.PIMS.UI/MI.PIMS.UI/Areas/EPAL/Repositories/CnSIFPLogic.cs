using MI.PIMS.UI.Common;

namespace MI.PIMS.UI.Areas.EPAL.Repositories
{
    public class CnSIFPLogic: ICnSIFPLogic
    {
        public CnSIFPLogic(){}

        public bool IsLOBCnSIFP(string lob)
        {
            if(lob.ToStringNullSafe().ToLower() == "cns" || lob.ToStringNullSafe().ToLower() == "ifp")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
