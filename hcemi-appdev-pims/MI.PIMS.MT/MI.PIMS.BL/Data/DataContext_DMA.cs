using MI.PIMS.BO.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MI.PIMS.BL.Data
{
    public class DataContext_DMA : DbContext
    {
        public DataContext_DMA(DbContextOptions<DataContext_DMA> options) : base(options)
        {
        }

        //Add Database Entities here...
        //public DbSet<UserInfoDto> UserInfo { get; set; }

        public DbSet<App_DataRole> App_DataRole { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Entity_Xref> Entity_Xref { get; set; }
        public DbSet<Market> Market { get; set; }
    }
}
