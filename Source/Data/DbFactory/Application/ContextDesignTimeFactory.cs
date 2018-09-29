using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Data.DbFactory.Application
{
    public class ContextDesignTimeFactory : IDesignTimeDbContextFactory<AdraContext>
    {
        public AdraContext CreateDbContext(string[] args)
        {
            return new AdraContext(new SqlConnection(@"Server=DESKTOP-B2S9J8F\SQLEXPRESS;Database=Adra;Trusted_Connection=True;"));          
        }
    }
}
