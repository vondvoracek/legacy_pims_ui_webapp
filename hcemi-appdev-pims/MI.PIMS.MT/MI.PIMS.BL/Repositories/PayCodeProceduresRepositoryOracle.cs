using Dapper;
using Dapper.Oracle;
using MI.PIMS.BL.Common;
using MI.PIMS.BO.Dtos;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Repositories
{
    public class PayCodeProceduresRepositoryOracle : DapperOracleBaseRepository
    {        
        public PayCodeProceduresRepositoryOracle(Helper helper) : base(helper) { }
        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodeProceduresSearch(PayCode_Procedures_Param_Dto obj)
        {
            var parameter = new DynamicParameters();

            // DYU -- Added loop through multi-select values and append single quotes to each value before sending to sp. 
            if (obj.p_PAYC_PRODUCT_CD != null) // < -- Multi-Select value
            {        
                string s1 = obj.p_PAYC_PRODUCT_CD;
                string[] items1 = s1.Split(',');  
                obj.p_PAYC_PRODUCT_CD = "";

                for (int i = 0; i < items1.Length; i++)
                {
                    obj.p_PAYC_PRODUCT_CD += "'" + items1[i] + "',";
                }

                obj.p_PAYC_PRODUCT_CD = obj.p_PAYC_PRODUCT_CD.TrimEnd(',');
            } 
            
            if (obj.p_PAYC_BUS_SEG_CD != null)  // < -- Multi-Select value
            {
                string s2 = obj.p_PAYC_BUS_SEG_CD;
                string[] items2 = s2.Split(',');
                obj.p_PAYC_BUS_SEG_CD = "";

                for (int i = 0; i < items2.Length; i++)
                {
                    obj.p_PAYC_BUS_SEG_CD += "'" + items2[i] + "',";
                }

                obj.p_PAYC_BUS_SEG_CD = obj.p_PAYC_BUS_SEG_CD.TrimEnd(',');
            } 
            
            if (obj.p_PAYC_ENTITY_CD != null)  // < -- Multi-Select value
            {
                string s3 = obj.p_PAYC_ENTITY_CD;
                string[] items3 = s3.Split(',');
                obj.p_PAYC_ENTITY_CD = "";

                for (int i = 0; i < items3.Length; i++)
                {
                    obj.p_PAYC_ENTITY_CD += "'" + items3[i] + "',";
                }
                obj.p_PAYC_ENTITY_CD = obj.p_PAYC_ENTITY_CD.TrimEnd(',');
            } 
            
            if (obj.p_iCES != null) // < -- Multi-Select value
            {
                string s4 = obj.p_iCES;
                string[] items4 = s4.Split(',');
                obj.p_iCES = "";

                for (int i = 0; i < items4.Length; i++)
                {
                    obj.p_iCES += "'" + items4[i] + "',";
                }
                obj.p_iCES = obj.p_iCES.TrimEnd(',');
            } 
            
            if (obj.p_PAYC_KL_PCS != null) // < -- Multi-Select value
            {
                string s5 = obj.p_PAYC_KL_PCS;
                string[] items5 = s5.Split(',');
                obj.p_PAYC_KL_PCS = "";

                for (int i = 0; i < items5.Length; i++)
                {
                    obj.p_PAYC_KL_PCS += "'" + items5[i] + "',";
                }
                obj.p_PAYC_KL_PCS = obj.p_PAYC_KL_PCS.TrimEnd(',');
            } 
            
            if (obj.p_PAYC_NDB_PCS != null) // < -- Multi-Select value
            {
                string s6 = obj.p_PAYC_NDB_PCS;
                string[] items6 = s6.Split(',');
                obj.p_PAYC_NDB_PCS = "";

                for (int i = 0; i < items6.Length; i++)
                {
                    obj.p_PAYC_NDB_PCS += "'" + items6[i] + "',";
                }
                obj.p_PAYC_NDB_PCS = obj.p_PAYC_NDB_PCS.TrimEnd(',');
            }

            if (obj.p_PAYC_PLAN_CD != null) // < -- Multi-Select value
            {
                string s7 = obj.p_PAYC_PLAN_CD;
                string[] items7 = s7.Split(',');
                obj.p_PAYC_PLAN_CD = "";

                for (int i = 0; i < items7.Length; i++)
                {
                    obj.p_PAYC_PLAN_CD += "'" + items7[i] + "',";
                }
                obj.p_PAYC_PLAN_CD = obj.p_PAYC_PLAN_CD.TrimEnd(',');
            }

            if (obj.p_PROC_CD != null) // < -- Multi-Select value
            {
                string s8 = obj.p_PROC_CD;
                string[] items8 = s8.Split(',');
                obj.p_PROC_CD = "";

                for (int i = 0; i < items8.Length; i++)
                {
                    obj.p_PROC_CD += "'" + items8[i].Trim() + "',";
                }
                obj.p_PROC_CD = obj.p_PROC_CD.TrimEnd(',');
            }

            if (obj.p_PAYC_ICES_EDIT_NAME != null) // < -- Multi-Select value
            {
                string s9 = obj.p_PAYC_ICES_EDIT_NAME;
                string[] items9 = s9.Split(',');
                obj.p_PAYC_ICES_EDIT_NAME = "";

                for (int i = 0; i < items9.Length; i++)
                {
                    obj.p_PAYC_ICES_EDIT_NAME += "'" + items9[i].Trim() + "',";
                }
                obj.p_PAYC_ICES_EDIT_NAME = obj.p_PAYC_ICES_EDIT_NAME.TrimEnd(',');
            }

            if (obj.p_PAYC_ICES_EDIT_ACTION != null) // < -- Multi-Select value
            {
                string s10 = obj.p_PAYC_ICES_EDIT_ACTION;
                string[] items10 = s10.Split(',');
                obj.p_PAYC_ICES_EDIT_ACTION = "";

                for (int i = 0; i < items10.Length; i++)
                {
                    obj.p_PAYC_ICES_EDIT_ACTION += "'" + items10[i].Trim() + "',";
                }
                obj.p_PAYC_ICES_EDIT_ACTION = obj.p_PAYC_ICES_EDIT_ACTION.TrimEnd(',');
            }

            parameter.Add("p_PAYC_HIERARCHY_KEY", obj.p_PAYC_HIERARCHY_KEY);
            parameter.Add("p_PAYC_PRODUCT_CD", obj.p_PAYC_PRODUCT_CD); // < -- Multi-Select
            parameter.Add("p_PAYC_PLAN_CD", obj.p_PAYC_PLAN_CD); // < -- Multi-Select
            parameter.Add("p_PAYC_BUS_SEG_CD", obj.p_PAYC_BUS_SEG_CD); // < -- Multi-Select
            parameter.Add("p_PAYC_ENTITY_CD", obj.p_PAYC_ENTITY_CD); // < -- Multi-Select
            parameter.Add("p_CURRENT_EFF_DT", obj.p_CURRENT_EFF_DT);
            parameter.Add("p_CURRENT_EXP_DT", obj.p_CURRENT_EXP_DT);
            parameter.Add("p_PAYC_VER_EFF_DT", obj.p_PAYC_VER_EFF_DT);
            parameter.Add("p_INCLUDE_HISTORICAL", obj.p_INCLUDE_HISTORICAL);
            parameter.Add("p_PROC_CD", obj.p_PROC_CD); // < -- Multi-Select
            parameter.Add("p_PAYC_STATUS", obj.p_PAYC_STATUS);
            parameter.Add("p_ALTRNT_SVC_CAT", obj.p_ALTRNT_SVC_CAT);
            parameter.Add("p_ALTRNT_SVC_SUBCAT", obj.p_ALTRNT_SVC_SUBCAT);
            parameter.Add("p_iCES", obj.p_iCES); // < -- Multi-Select
            parameter.Add("p_PRIOR_AUTH_STATUS", obj.p_PRIOR_AUTH_STATUS);
            parameter.Add("p_PAYC_KL_PCS", obj.p_PAYC_KL_PCS);  // < -- Multi-Select
            parameter.Add("p_PAYC_NDB_PCS", obj.p_PAYC_NDB_PCS); // < -- Multi-Select
            parameter.Add("p_PAYC_ICES_EDIT_NAME", obj.p_PAYC_ICES_EDIT_NAME); // < -- Multi-Select



            var data = await QueryAsync<PayCode_Procedures_V_Dto>("usp_Get_PIMS_APP_PAY_CODE_PROCEDURES_V_ByParam_prc", parameter, 600);

            return data;
        }


        public async Task<IEnumerable<PayCodeFiltersDto>> GetPayCodeSearchFilters(EPAL_PIMSHierarchyCode_V_XwalkByEPALBusSegCD_Dto obj)
        {
            var parameter = new DynamicParameters();
            parameter.Add("P_EPAL_BUS_SEG_CD", obj.EPAL_BUS_SEG_CD);
            parameter.Add("P_COLUMN_NAME", obj.COLUMN_NAME);
            parameter.Add("P_ACTIVE", obj.ACTIVE);
            parameter.Add("P_EPAL_ENTITY_CD", obj.EPAL_ENTITY_CD);
            var data = await QueryAsync<PayCodeFiltersDto>("USP_GET_PIMS_PAYC_HIERARCHY_CODES_XWALK_V_PRC", parameter, 600);
            return data;
        }



        public async Task<PayCode_Procedures_T_Dto> GetPayCodeProcedureByPIMS_ID(PayCode_Procedures_Param_Dto obj)
        {
            var parameter = new DynamicParameters();
            parameter.Add("p_PAYC_HIERARCHY_KEY", obj.p_PAYC_HIERARCHY_KEY);
            parameter.Add("p_PAYC_VER_EFF_DT", obj.p_PAYC_VER_EFF_DT);
            parameter.Add("p_MS_ID", obj.p_MS_ID);
            var stored_procedure = "";

            if (obj.p_PAYC_VER_EFF_DT != null)
            {
                stored_procedure = "USP_GET_PIMS_APP_PAY_CODE_PROCEDURES_V_BY_PIMS_ID_PRC";
            }
            else if (obj.p_PAYC_VER_EFF_DT == null) 
            {
                stored_procedure = "USP_GET_PIMS_APP_PAY_CODE_PROCEDURES_CURR_VER_V_BY_PIMS_ID_PRC";
            }
            var data = await QueryFirstOrDefaultAsync<PayCode_Procedures_T_Dto>(stored_procedure, parameter, 600);

            // Convert 12/31/2999, 12/31/1999 to null
            data.EPAL_PRIOR_AUTH_EFF_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EFF_DT);
            data.EPAL_PRIOR_AUTH_EXP_DT = Helper.CheckExpYear(data.EPAL_PRIOR_AUTH_EXP_DT);
            data.PAYC_EFF_DT = Helper.CheckExpYear(data.PAYC_EFF_DT);
            data.PAYC_EXP_DT = Helper.CheckExpYear(data.PAYC_EXP_DT);

            //
            // Get MCR_Route
            //
            var parameter2 = new DynamicParameters();
            parameter2.Add("p_PAYC_HIERARCHY_KEY", obj.p_PAYC_HIERARCHY_KEY);
            parameter2.Add("p_PAYC_VER_EFF_DT", data.PAYC_VER_EFF_DT);
            parameter2.Add("p_MS_ID", obj.p_MS_ID);
            var data2 = await QueryAsync<PayCode_MCR_Routed_Dto>("usp_Get_PIMS_APP_PAY_CODE_MCR_ROUTED_BY_PIMS_ID_PRC", parameter2, 600);

            // 
            StringBuilder s = new StringBuilder();
            if (data2 != null && data2.Count() > 0)
            {
                int i = data2.Count();
                foreach(var m in data2) {
                    s.Append(m.MCR_ROUTED);
                    i--;
                    if (i == 0)
                    {                       
                        data.PAYC_MCR_ROUTED = s.ToString();
                        break;
                    }
                    else
                    {
                        s.Append(',');
                    }
                }
            }

            //Check if IsCurrent record if a effective date is passed.
            if (obj.p_PAYC_VER_EFF_DT != null)
            {
                IsCurrentRecordDto obj2 = new IsCurrentRecordDto();
                {
                    obj2.p_PIMS_ID = obj.p_PAYC_HIERARCHY_KEY;
                    obj2.p_PIMS_VER_EFF_DT = obj.p_PAYC_VER_EFF_DT;
                    obj2.p_MODULE_NAME = "PAYCODES";
                };
                var data3 = await GetPIMSIsCurrentRecord(obj2);

                if (data3 != null)
                {
                    data.IS_CURRENT = data3.IS_CURRENT;
                }
            }
            else if (obj.p_PAYC_VER_EFF_DT == null) 
            {
                data.IS_CURRENT ="Y";
            }

            return data;
        }

        public async Task<IsCurrentRecordDto> GetPIMSIsCurrentRecord(IsCurrentRecordDto obj)
        {
            var parameter = new DynamicParameters();
            parameter.Add("p_PIMS_ID", obj.p_PIMS_ID);
            parameter.Add("p_PIMS_VER_EFF_DT", obj.p_PIMS_VER_EFF_DT);
            parameter.Add("p_MODULE_NAME", obj.p_MODULE_NAME);
            var data = await QueryFirstOrDefaultAsync<IsCurrentRecordDto>("USP_GET_PIMS_IS_CURRENT_RECORD_PRC", parameter, 600);
            return data;
        }

        public async Task<IEnumerable<PayCode_EPAL_Summary_Dto>> GetPayCodeEPALSummary(PayCode_Procedures_Param_Dto obj)
        {
            var parameter = new DynamicParameters();
            parameter.Add("p_PAYC_HIERARCHY_KEY", obj.p_PAYC_HIERARCHY_KEY);
            parameter.Add("p_MS_ID", obj.p_MS_ID);
            var data = await QueryAsync<PayCode_EPAL_Summary_Dto>("usp_Get_PIMS_APP_PAY_CODE_EPAL_SUMMARY_BY_PIMS_ID_PRC", parameter, 600);
            return data;
        }

        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodesNotesFurtherConsiderationByPIMS_ID(string PAYC_HIERARCHY_KEY)
        {
            var parameter = new DynamicParameters();
            parameter.Add("p_PAYC_HIERARCHY_KEY", PAYC_HIERARCHY_KEY);
            var data = await QueryAsync<PayCode_Procedures_V_Dto>("USP_GET_PIMS_APP_PAY_CODE_NOTES_FURTHER_CONSIDERATIONS_BY_PIMS_ID_PRC", parameter, 600);
            return data;
        }


        public async Task<IEnumerable<PayCode_Procedures_V_Dto>> GetPayCodeHistorical(PayCode_Procedures_Param_Dto obj)
        {
            var parameter = new DynamicParameters();
            parameter.Add("p_PAYC_HIERARCHY_KEY", obj.p_PAYC_HIERARCHY_KEY);
            parameter.Add("p_MS_ID", obj.p_MS_ID);
            var data = await QueryAsync<PayCode_Procedures_V_Dto>("usp_Get_PIMS_APP_PAY_CODE_HISTORY_BY_PIMS_ID_PRC", parameter, 600);
            return data;
        }

        public async Task<int> PAYC_HISTORIC_INS_UPD_DRIVER(PayCode_Procedures_T_Dto obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("P_PAYC_BUS_SEG_CD", obj.PAYC_BUS_SEG_CD);
            parameters.Add("P_PAYC_ENTITY_CD", obj.PAYC_ENTITY_CD);
            parameters.Add("P_PAYC_PROC_CD", obj.PAYC_PROC_CD);
            parameters.Add("P_PAYC_VER_EFF_DT", obj.PAYC_VER_EFF_DT);
            parameters.Add("P_PAYC_PLAN_CD", obj.PAYC_PLAN_CD);
            parameters.Add("P_PAYC_PRODUCT_CD", obj.PAYC_PRODUCT_CD);
            parameters.Add("P_PAYC_KL_PCS", obj.PAYC_KL_PCS);
            parameters.Add("P_PAYC_NDB_PCS", obj.PAYC_NDB_PCS);
            parameters.Add("P_PAYC_NDB_REMARK_CD", obj.PAYC_NDB_REMARK_CD);
            parameters.Add("P_PAYC_ICES_IND", obj.PAYC_ICES_IND);
            parameters.Add("P_PAYC_ICES_EDIT_ACTION", obj.PAYC_ICES_EDIT_ACTION);
            parameters.Add("P_PAYC_ADVN_NOTIF", obj.PAYC_ADVN_NOTIF);
            parameters.Add("P_PAYC_PRED_EFF_DT", obj.PAYC_PRED_EFF_DT);
            parameters.Add("P_PAYC_PRED_EXP_DT", obj.PAYC_PRED_EXP_DT);
            parameters.Add("P_PAYC_MCR_ROUTED", obj.PAYC_MCR_ROUTED);
            parameters.Add("P_PAYC_BIFURCATED", obj.PAYC_BIFURCATED);
            parameters.Add("P_PAYC_NS88_COMPLIANCE", obj.PAYC_NS88_COMPLIANCE);
            parameters.Add("P_PAYC_ADDITIONAL_EDITS", obj.PAYC_ADDITIONAL_EDITS);
            parameters.Add("P_PAYC_COMMENTS", obj.PAYC_COMMENTS);
            parameters.Add("P_PAYC_ICES_EDIT_NAME", obj.PAYC_ICES_EDIT_NAME);
            parameters.Add("P_PAYC_PRED_IND", obj.PAYC_PRED_IND);
            parameters.Add("P_PAYC_EFF_DT", obj.PAYC_EFF_DT);
            parameters.Add("P_PAYC_EXP_DT", obj.PAYC_EXP_DT);
            parameters.Add("P_PAYC_FUND_ARNGMNT_CD", obj.PAYC_FUND_ARNGMNT_CD);
            parameters.Add("P_USER_ID", obj.LST_UPDT_BY);
            parameters.Add("P_CHANGE_REQ_ID", obj.CHANGE_REQ_ID);
            parameters.Add("P_CHANGE_DESC", obj.CHANGE_DESC);
            parameters.Add("P_PAYC_FURTHER_CONSIDERATIONS", obj.PAYC_FURTHER_CONSIDERATIONS);
            parameters.Add("P_PAYC_NOTES", obj.PAYC_NOTES);            

            var retVal = await ExecuteAsync("USP_PAYC_HISTORIC_INS_UPD_DRIVER_PRC", parameters, 600);

            return retVal;
        }


        public async Task<int> PAYC_DELETE_DRIVER_PRC(PayCode_Procedures_T_Dto obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_PAYC_HIERARCHY_KEY", obj.PAYC_HIERARCHY_KEY);
            parameters.Add("p_PAYC_VER_EFF_DT", obj.PAYC_VER_EFF_DT);
            parameters.Add("p_USER_ID", obj.LST_UPDT_BY);
            parameters.Add("p_CHANGE_REQ_ID", obj.CHANGE_REQ_ID);
            parameters.Add("p_CHANGE_DESC", obj.CHANGE_DESC);
            var retVal = await ExecuteAsync("USP_PAYC_DELETE_DRIVER_PRC", parameters, 60);
            return retVal;
        }


        public async Task<int> UpdatePayCodeProcedure(PayCode_Procedures_T_Dto obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("P_PAYC_HIERARCHY_KEY", obj.PAYC_HIERARCHY_KEY);
            parameters.Add("P_PAYC_BUS_SEG_CD", obj.PAYC_BUS_SEG_CD);
            parameters.Add("P_PAYC_ENTITY_CD", obj.PAYC_ENTITY_CD);
            parameters.Add("P_PAYC_PLAN_CD", obj.PAYC_PLAN_CD);
            parameters.Add("P_PAYC_PRODUCT_CD", obj.PAYC_PRODUCT_CD);
            parameters.Add("P_PAYC_FUND_ARNGMNT_CD", obj.PAYC_FUND_ARNGMNT_CD);
            parameters.Add("P_PAYC_PROC_CD", obj.PAYC_PROC_CD);
            parameters.Add("P_PAYC_EFF_DT", obj.PAYC_EFF_DT);
            parameters.Add("P_PAYC_EXP_DT", obj.PAYC_EXP_DT);
            parameters.Add("P_PAYC_PRED_EFF_DT", obj.PAYC_PRED_EFF_DT);

            parameters.Add("P_PAYC_PRED_EXP_DT", obj.PAYC_PRED_EXP_DT);
            parameters.Add("P_PAYC_KL_PCS", obj.PAYC_KL_PCS);
            parameters.Add("P_PAYC_NDB_PCS", obj.PAYC_NDB_PCS);
            parameters.Add("P_PAYC_NDB_REMARK_CD", obj.PAYC_NDB_REMARK_CD);
            parameters.Add("P_PAYC_ICES_EDIT_NAME", obj.PAYC_ICES_EDIT_NAME);
            parameters.Add("P_PAYC_ICES_IND", obj.PAYC_ICES_IND);
            parameters.Add("P_PAYC_PRED_IND", obj.PAYC_PRED_IND);
            parameters.Add("P_PAYC_ICES_EDIT_ACTION", obj.PAYC_ICES_EDIT_ACTION);
            parameters.Add("P_PAYC_ADVN_NOTIF", obj.PAYC_ADVN_NOTIF);
            parameters.Add("P_PAYC_MCR_ROUTED", obj.PAYC_MCR_ROUTED);

            parameters.Add("P_PAYC_BIFURCATED", obj.PAYC_BIFURCATED);
            parameters.Add("P_PAYC_NS88_COMPLIANCE", obj.PAYC_NS88_COMPLIANCE);
            parameters.Add("P_PAYC_ADDITIONAL_EDITS", obj.PAYC_ADDITIONAL_EDITS);
            parameters.Add("P_PAYC_COMMENTS", obj.PAYC_COMMENTS);
            parameters.Add("P_FURTHER_INST", obj.FURTHER_INST);
            parameters.Add("P_NOTES", obj.NOTES);
            parameters.Add("P_USER_ID", obj.LST_UPDT_BY);
            parameters.Add("P_CHANGE_REQ_ID", obj.CHANGE_REQ_ID);
            parameters.Add("P_CHANGE_DESC", obj.CHANGE_DESC);

            var retVal = await ExecuteAsync("usp_PAYC_INS_UPD_DRIVER", parameters, 600);

            return retVal;
        }
        public async Task<IEnumerable<PayCode_ChangeHistory_Dto>> GetPayCodeChangeHistory(PayCode_Procedures_Param_Dto obj)
        {
            var parameter = new DynamicParameters();
            parameter.Add("p_PAYC_HIERARCHY_KEY", obj.p_PAYC_HIERARCHY_KEY);
            parameter.Add("p_MS_ID", obj.p_MS_ID);
            var data = await QueryAsync<PayCode_ChangeHistory_Dto>("usp_Get_PIMS_APP_PAY_CODE_CHANGE_HISTORY_BY_PIMS_ID_PRC", parameter, 600);
            return data;
        }
        public async Task<IEnumerable<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>> GetAllPayCodeHierarchyCodesXwalk()
        {
            var data = await QueryAsync<PayCode_PIMSHierarchyCode_V_Xwalk_Dto>("USP_GET_ALL_PAYC_HIERARCHY_CODES_XWALK_V", null, 600);
            return data;
        }

    }
}
