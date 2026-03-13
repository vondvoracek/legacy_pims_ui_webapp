using System;
using System.Collections.Generic;
using System.Text;

namespace MI.PIMS.BO.Dtos
{
    public class UserInfoDto
    {
        public UserInfoDto(){}

        public string MS_ID { get; set; }
        public string Sup_MSID { get; set; }
        public string Sup_Name { get; set; }
        public string Lname { get; set; }
        public string Fname { get; set; }
        public string MI { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Div_Code { get; set; }
        public string Division_Name { get; set; }
        public string Department_Name { get; set; }
        public string App_RoleID { get; set; }
        public string App_GroupID { get; set; }
        public string Active { get; set; }
        public DateTime? Lst_Updt_Dt { get; set; }
        public string Lst_Updt_By { get; set; }
        public string Manualupdt { get; set; }
        public int? Role_ID { get; set; }
        public bool? IsAdmin { get; set; }
        public string Display_Name { get; set; }
    }


    public class UserInfo_T_Dto
    {
        public string MS_ID { get; set; }
        public string SUP_MSID { get; set; }
        public string SUP_NAME { get; set; }
        public string LNAME { get; set; }
        public string FNAME { get; set; }
        public string MI { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string DIV_CODE { get; set; }
        public string DIVISION_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string ACTIVE { get; set; }
        public DateTime LST_UPDT_DT { get; set; }
        public string LST_UPDT_BY { get; set; }
        public string MANUALUPDT { get; set; }
        public DateTime LST_ACCESS_DT { get; set; }
        public string AUTOSAVESET { get; set; }
        public string DISPLAYNAME { get; set; }
        public int APP_ROLEID { get; set; }
        public string APP_ROLEIDS { get; set; }
        public string APP_ROLENAME { get; set; }
        public string PIMS_USER { get; set; }
    }

    public class UserInfo_PG_AddDto
    {
        public string p_ms_id { get; set; }
        public string p_sup_msid { get; set; }
        public string p_sup_name { get; set; }
        public string p_lname { get; set; }
        public string p_fname { get; set; }
        public string p_mi { get; set; }
        public string p_email { get; set; }
        public string p_phone { get; set; }
        public string p_fax { get; set; }
        public string p_div_code { get; set; }
        public string p_division_name { get; set; }
        public string p_department_name { get; set; }
        public string p_active { get; set; }
        public string p_lst_updt_by { get; set; }
        public string p_manualupdt { get; set; }
        public string p_autosaveset { get; set; }
        public string p_displayname { get; set; }
        public int p_app_roleid { get; set; }
        public string p_pims_user { get; set; }
    }


    public class UserInfo_AddDto
    {
        public string P_MS_ID { get; set; }
        public string P_SUP_MSID { get; set; }
        public string P_SUP_NAME { get; set; }
        public string P_LNAME { get; set; }
        public string P_FNAME { get; set; }
        public string P_MI { get; set; }
        public string P_EMAIL { get; set; }
        public string P_PHONE { get; set; }
        public string P_FAX { get; set; }
        public string P_DIV_CODE { get; set; }
        public string P_DIVISION_NAME { get; set; }
        public string P_DEPARTMENT_NAME { get; set; }
        public string P_ACTIVE { get; set; }        
        public string P_LST_UPDT_BY { get; set; }
        public string P_MANUALUPDT { get; set; }
        public string P_AUTOSAVESET { get; set; }
        public string P_DISPLAYNAME { get; set; }
        public int P_APP_ROLEID { get; set; }
        public string P_APP_ROLEIDS { get; set; }
        public string P_PIMS_USER { get; set; }
    }

    public class AppRoleUserAssign_Dto
    {
        public string p_MS_ID { get; set; }
        public string p_APP_ROLEIDS { get; set; }
        public string p_LST_UPDT_BY { get; set; }
    }
    public class DeleteRoleUserAssignParam_Dto
    {
        public string p_MS_ID { get; set; }
        public string p_APP_ROLEID { get; set; }
        public string p_LST_UPDT_BY { get; set; }
    }

    public class UserAccess_HistDto
    {
        public string Module_Name { get; set; }
        public string UserAction { get; set; }
        public string UserSelection { get; set; }
        public DateTime Lst_Updt_Dt { get; set; }
        public string Lst_Updt_By { get; set; }
        public int Page_ID { get; set; }
    }
    public class CTT_UserAccess_HistDto
    {

        public string MODULE_NAME { get; set; }
        public string USERACTION { get; set; }
        public string USERSELECTION { get; set; }
        public string LST_UPDT_BY { get; set; }
        public string PAGE_ID { get; set; }
    }

    public class TogglePIMSUserStatusParam_Dto
    {
        public string p_MS_ID { get; set; }
        public string p_PIMS_USER { get; set; }
        public string p_LST_UPDT_BY { get; set; }
    }
}
