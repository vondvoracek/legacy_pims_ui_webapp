using System;
using System.Collections.Generic;
using System.Text;

namespace MI.PIMS.BO.Dtos
{
    public class ActiveDirectoryUserDto
    {
        public bool Active { get; set; }
        public string Supervisor_Name { get; set; }
        public string Supervisor_MS_ID { get; set; }
        public string Title { get; set; }
        public string Segment_Name { get; set; }
        public string Segment_Code { get; set; }
        public string Division_Name { get; set; }
        public string App_Role_ID { get; set; }
        public string Division_Code { get; set; }
        public string Department_ID { get; set; }
        public string Phone { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Email { get; set; }
        public string EmployeeID { get; set; }
        public string MS_ID { get; set; }
        public string Department_Name { get; set; }
        public bool Is_Selected { get; set; }
        public string Display_Name { get; set; }
        public string MI { get; set; }
        public string Fax { get; set; }
    }

    public class ActiveDirectoryUserInfoDto
    {
        public string EmployeeID { get; set; }
        public string MSID { get; set; }
        public string EMail { get; set; }
        public string Name { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string MName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Sup_MSID { get; set; }
        public string Sup_Name { get; set; }
        public string Sup_EmployeeID { get; set; }
        public string DivisionCode { get; set; }
        public string Division { get; set; }
        public string Dept_Code { get; set; }
        public string Dept { get; set; }
        public string Business { get; set; }
        public string BusinessCode { get; set; }
        public string Segment { get; set; }
        public string SegmentCode { get; set; }
        public string MarketGroup { get; set; }
        public string MarketGroupCode { get; set; }
        public string AccountExpires { get; set; }
    }
    public class ActiveDirectoryUserTO
    {
        public string MS_ID { get; set; }
        public string EmployeeID { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Display_Name { get; set; }
        public string Phone { get; set; }
        public string Department_ID { get; set; }
        public string Department_Name { get; set; }
        public string Division_Code { get; set; }
        public string Division_Name { get; set; }
        public string Segment_Code { get; set; }
        public string Segment_Name { get; set; }
        public string Title { get; set; }
        public string Supervisor_MS_ID { get; set; }
        public string Supervisor_Name { get; set; }
        public bool Active { get; set; }
        public string App_Role_ID { get; set; }
        public bool Is_Selected { get; set; }
        public string User_Principal_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Street_Address { get; set; }
        public string Zip_Code { get; set; }
        public string State { get; set; }

    }
}
