namespace MI.PIMS.UI.Areas.Admin.Models
{
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
