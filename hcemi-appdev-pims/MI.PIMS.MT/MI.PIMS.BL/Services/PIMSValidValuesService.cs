using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{
    public class PIMSValidValuesService : IPIMSValidValuesService
    {
        private readonly PIMSValidValuesRepository _repo;

        public PIMSValidValuesService(PIMSValidValuesRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues(string p_VV_SET_NAME, string p_BUS_SEG_CD)
        {
            var data = await _repo.GetPIMSValidValues(p_VV_SET_NAME, p_BUS_SEG_CD);
            return data;
        }

        public async Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues_VVCdOnly(string p_VV_SET_NAME, string p_BUS_SEG_CD)
        {
            var data = await _repo.GetPIMSValidValues_VVCdOnly(p_VV_SET_NAME, p_BUS_SEG_CD);
            return data;
        }


        public async Task<IEnumerable<State_Mandated_Ind_Dto>> GetStateMandatedInds()
        {
            var data = await _repo.GetPIMSValidValues("STATE_MANDATED_IND", "All");
            IEnumerable<State_Mandated_Ind_Dto> state_Mandated_Ind_Dtos = data.Select(item => new State_Mandated_Ind_Dto()
            {
                STATE_MANDATED_IND = item.VV_CD,
                DESC               = item.VV_CD_DESC
            });
            return state_Mandated_Ind_Dtos;
        }

        public async Task<IEnumerable<INCL_EXCL_CD_Dto>> GetInclExclCDs()
        {
            var data = await _repo.GetPIMSValidValues("INCL_EXCL_CD", "All");
            IEnumerable<INCL_EXCL_CD_Dto> iNCL_EXCL_CD_Dtos = data.Select(item => new INCL_EXCL_CD_Dto()
            {
                INCL_EXCL_CD = item.VV_CD,
                DESC         = item.VV_CD_DESC
            });
            return iNCL_EXCL_CD_Dtos;
        }

        public async Task<IEnumerable<Allwd_plc_of_svc_Dto>> GetAllwdPlcOfSvcs()
        {
            var data = await _repo.GetPIMSValidValues("ALLWD_PLC_OF_SVC", "All");
            IEnumerable<Allwd_plc_of_svc_Dto> allwd_Plc_Of_Svc_Dtos = data.Select(item => new Allwd_plc_of_svc_Dto()
            {
                CODE = item.VV_CD,
                ALLWD_PLC_OF_SVC = item.VV_CD_DESC
            });;
            return allwd_Plc_Of_Svc_Dtos;
        }

        public async Task<IEnumerable<State_CD_Dto>> GetStateCDs()
        {
            var data = await _repo.GetPIMSValidValues("STATE_CD", "All");
            IEnumerable<State_CD_Dto> states = data.Select(item => new State_CD_Dto()
            {
                STATE_CD = item.VV_CD,
                DESC = item.VV_CD_DESC
            }).ToList();
            return states;
        }

        public async Task<IEnumerable<Delegate_Um_Dto>> GetDelegateUms()
        {
            //var data = await _repo.GetPIMSValidValues("DELEGATED_UM-DISABLED", "EnI");
            // 02/06/2023 - DY User Story 49334 - New Specs - Delegated UM _Valid Values
            var data = await _repo.GetPIMSValidValues("DELEGATED_UM", "EnI");
            IEnumerable<Delegate_Um_Dto> delegate_Um_Dtos = data.Select(item => new Delegate_Um_Dto()
            {
                DELEGATED_UM = item.VV_CD,
                DESC = item.VV_CD_DESC
            }).ToList();
            return delegate_Um_Dtos;
        }

        public async Task<IEnumerable<Prog_Mgd_By_Dto>> GetProgMgdBy()
        {
            var data = await _repo.GetPIMSValidValues("PROG_MGD_BY", "All");
            IEnumerable<Prog_Mgd_By_Dto> prog_Mgd_By_Dtos = data.Select(item => new Prog_Mgd_By_Dto()
            {
                PROG_MGD_BY = item.VV_CD,
                DESC = item.VV_CD_DESC
            }).ToList();
            return prog_Mgd_By_Dtos;
        }

        public async Task<IEnumerable<DX_Ind_Dto>> GetDXInds()
        {
            var data = await _repo.GetPIMSValidValues("DRUG_RVW_AT_LAUNCH_IND", "All");
            IEnumerable<DX_Ind_Dto> dx_Ind_Dtos = data.Select(item => new DX_Ind_Dto()
            {
                DX_IND = item.VV_CD,
                DESC = item.VV_CD_DESC
            }).ToList();
            return dx_Ind_Dtos;
        }

        public async Task<IEnumerable<St_App_Ind_Dto>> GetStAppInds()
        {
            var data = await _repo.GetPIMSValidValues("DRUG_RVW_AT_LAUNCH_IND", "All");
            IEnumerable<St_App_Ind_Dto> state_Ind_Dtos = data.Select(item => new St_App_Ind_Dto()
            {
                ST_APP_IND = item.VV_CD,
                DESC = item.VV_CD_DESC
            }).ToList();
            return state_Ind_Dtos;
        }

        public async Task<IEnumerable<Int_Ext_CD_Dto>> GetIntExtCDs()
        {
            var data = await _repo.GetPIMSValidValues("INT_EXT_CD", "All");
            IEnumerable<Int_Ext_CD_Dto> int_Ext_CD_Dtos = data.Select(item => new Int_Ext_CD_Dto()
            {
                INT_EXT_CD = item.VV_CD,
                DESC = item.VV_CD_DESC
            }).ToList();
            return int_Ext_CD_Dtos;
        }

        public async Task<IEnumerable<Ret_Factor_DD_Dto>> GetRetentionFactors()
        {
            var data = await _repo.GetPIMSValidValues("FACTOR_NM_RET", "All");
            IEnumerable<Ret_Factor_DD_Dto> retentionFactor_Dto = data.Select(item => new Ret_Factor_DD_Dto()
            {
                FACTOR_NM_RET = item.VV_CD,
                FACTOR_DESC_RET = item.VV_CD_DESC
            }).ToList();
            return retentionFactor_Dto;
        }

        public async Task<IEnumerable<Red_Factor_DD_Dto>> GetReductionFactors()
        {
            var data = await _repo.GetPIMSValidValues("FACTOR_NM_RED", "All");
            IEnumerable<Red_Factor_DD_Dto> reductionFactor_Dto = data.Select(item => new Red_Factor_DD_Dto()
            {
                FACTOR_NM_RED = item.VV_CD,
                FACTOR_DESC_RED = item.VV_CD_DESC
            }).ToList();
            return reductionFactor_Dto;
        }

        public async Task<IEnumerable<DTQ_Applies_Dto>> GetDTQApplies()
        {
            List<DTQ_Applies_Dto> gs_Current_Status_Dto = new()
            {
                new DTQ_Applies_Dto() { DTQ_CD = "Yes", DTQ_DESC = "Yes"},
                new DTQ_Applies_Dto() { DTQ_CD = "No", DTQ_DESC = "No"}                
            };

            return await Task.FromResult(gs_Current_Status_Dto);
        }

        public async Task<IEnumerable<InPatientTypeRule>> GetInPatientTypeRules()
        {
            var data = await _repo.GetPIMSValidValues("DPOC_RULE_TYPE", "IP");
            IEnumerable<InPatientTypeRule> patientTypeRules = data.Select(item => new InPatientTypeRule()
            {
                RULE_TYPE_INPAT = item.VV_CD,
                RULE_TYPE_OUTCOME_INPAT = item.VV_CD_DESC
            }).ToList();
            return patientTypeRules;
        }
        public async Task<IEnumerable<OutPatientTypeRule>> GetOutPatientTypeRules()
        {
            var data = await _repo.GetPIMSValidValues("DPOC_RULE_TYPE", "OP");
            IEnumerable<OutPatientTypeRule> patientTypeRules = data.Select(item => new OutPatientTypeRule()
            {
                RULE_TYPE_OUTPAT = item.VV_CD,
                RULE_TYPE_OUTCOME_OUTPAT = item.VV_CD_DESC
            }).ToList();
            return patientTypeRules;
        }
        public async Task<IEnumerable<OutPatientFacTypeRule>> GetOutPatientFacTypeRules()
        {
            var data = await _repo.GetPIMSValidValues("DPOC_RULE_TYPE", "OPF");
            IEnumerable<OutPatientFacTypeRule> patientTypeRules = data.Select(item => new OutPatientFacTypeRule()
            {
                RULE_TYPE_OUTPAT_FCLTY = item.VV_CD,
                RULE_TYPE_OUTCOME_OUTPAT_FCLTY = item.VV_CD_DESC
            }).ToList();
            return patientTypeRules;
        }
        public async Task<IEnumerable<RuleTypeReason>> GetRuleTypeReason()
        {
            var data = await _repo.GetPIMSValidValues("DPOC_RULE_TYPE_RSN", "All");
            IEnumerable<RuleTypeReason> patientTypeRules = data.Select(item => new RuleTypeReason()
            {
                RULE_TYPE_RSN = item.VV_CD,
                RULE_TYPE_RSN_DESC = item.VV_CD_DESC
            }).ToList();
            return patientTypeRules;
        }

        public async Task<IEnumerable<DTQ_TYPE_Dto>> GetDTQ_TYPE()
        {
            var data = await _repo.GetPIMSValidValues("DPOC_DTQ_TYPE", "All");
            IEnumerable<DTQ_TYPE_Dto> dtq_nm = data.Select(item => new DTQ_TYPE_Dto()
            {
                DTQ_TYPE = item.VV_CD,
                DTQ_TYPE_DESC = item.VV_CD_DESC
            }).ToList();
            return dtq_nm;
        }
        public async Task<IEnumerable<DTQ_RSN_Dto>> GetDTQ_RSN()
        {
            var data = await _repo.GetPIMSValidValues("DPOC_DTQ_RSN", "All");
            IEnumerable<DTQ_RSN_Dto> dpoc_dtq_type = data.Select(item => new DTQ_RSN_Dto()
            {
                DTQ_RSN = item.VV_CD,
                DTQ_RSN_DESC = item.VV_CD_DESC
            }).ToList();
            return dpoc_dtq_type;
        }

        public async Task<SOS_Site_Ind_Dto> GetSOSSiteAndService()
        {
            var data = await _repo.GetPIMSValidValues("SOS_SITE_IND", "All");
            SOS_Site_Ind_Dto sOS_Site_Ind_Dto = data.Where(d => d.VV_CD.ToLower() == "site and service").Select(item => new SOS_Site_Ind_Dto()
            {
                VV_CD = item.VV_CD,
                VV_CD_DESC = item.VV_CD_DESC
            }).FirstOrDefault();
            return sOS_Site_Ind_Dto;
        }

        public async Task<SOS_Site_Ind_Dto> GetSOSSiteOnly()
        {
            var data = await _repo.GetPIMSValidValues("SOS_SITE_IND", "All");
            SOS_Site_Ind_Dto sOS_Site_Ind_Dto = data.Where(d => d.VV_CD.ToLower() == "site only").Select(item => new SOS_Site_Ind_Dto()
            {
                VV_CD = item.VV_CD,
                VV_CD_DESC = item.VV_CD_DESC
            }).FirstOrDefault();
            return sOS_Site_Ind_Dto;
        }

        public async Task<IEnumerable<POS_APPL_Dto>> GetPLC_OF_SVC_CD()
        {
            var data = await _repo.GetPIMSValidValues("ALLWD_PLC_OF_SVC", "All");
            IEnumerable<POS_APPL_Dto> pOS_APPL_Dtos = data.Select(item => new POS_APPL_Dto()
            {
                POS_APPL = item.VV_CD,
                POS_APPL_DESC = item.VV_CD_DESC
            }); ;
            return pOS_APPL_Dtos;
        }
        public async Task<IEnumerable<POS_INCL_EXCL_CD_Dto>> GetPOS_INCL_EXCL_CD()
        {
            var data = await _repo.GetPIMSValidValues("INCL_EXCL_CD", "All");
            IEnumerable<POS_INCL_EXCL_CD_Dto> pOS_INCL_EXCL_CD_Dtos = data.Select(item => new POS_INCL_EXCL_CD_Dto()
            {
                POS_INCL_EXCL_CD = item.VV_CD,
                POS_INCL_EXCL_DESC = item.VV_CD_DESC
            });            
            return pOS_INCL_EXCL_CD_Dtos.Prepend(new POS_INCL_EXCL_CD_Dto { POS_INCL_EXCL_CD = string.Empty, POS_INCL_EXCL_DESC = string.Empty });
        }

        public async Task<IEnumerable<ATS_ISSUE_GOV_Dto>> GetStateIssueGov()
        {
            var data = await _repo.GetPIMSValidValues("STATE_ISS_GOV", "All");
            IEnumerable<ATS_ISSUE_GOV_Dto> state_issue_govs = data.Select(item => new ATS_ISSUE_GOV_Dto()
            {
                ATS_ISSUE_GOV_CD = item.VV_CD,
                DESC = item.VV_CD_DESC
            }).ToList();
            return state_issue_govs;
        }

        public async Task<IEnumerable<PIMS_Valid_Values_V_Dto>> GetPIMSValidValues()
        {
            var data = await _repo.GetPIMSValidValues();
            return data;
        }
    }
}