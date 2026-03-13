using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services.Interfaces
{
    public interface IPayCodeProceduresService
    {
        Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodeProceduresSearch(PayCode_Procedures_Param_Dto obj);
        Task<PayCode_Procedures_T_Dto> GetPayCodeProcedureByPIMS_ID(PayCode_Procedures_Param_Dto obj);
        Task<IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>> GetAllPayCodeHierarchyCodesXwalk();
        Task<IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>> GetAllPayCodeHierarchyCodesXwalk2();
        Task<IEnumerable<PayCode_EPAL_Summary_Dto>> GetPayCodeEPALSummary(PayCode_Procedures_Param_Dto obj);
        Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodeHistorical(PayCode_Procedures_Param_Dto obj);
        Task<UpdateDto> UpdatePayCodeProcedure(PayCode_Procedures_T_Dto obj);
        Task<UpdateDto> PAYC_HISTORIC_INS_UPD_DRIVER(PayCode_Procedures_T_Dto obj);
        Task<UpdateDto> PAYC_DELETE_DRIVER_PRC(PayCode_Procedures_T_Dto obj);
        Task<IEnumerable<PayCode_ChangeHistory_Dto>> GetPayCodeChangeHistory(PayCode_Procedures_Param_Dto obj);
        Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodesNotesFurtherConsiderationByPIMS_ID(string PAYC_HIERARCHY_KEY);
        Task<IEnumerable<PayCodeFiltersDto>> GetPayCodeSearchFilters(EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto obj);        
    }
}
