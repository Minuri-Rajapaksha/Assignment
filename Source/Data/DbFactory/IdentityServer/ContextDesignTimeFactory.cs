using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Data.DbFactory.IdentityServer
{
    public class ContextDesignTimeFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            return new IdentityContext(new SqlConnection(@"Server=DESKTOP-B2S9J8F\SQLEXPRESS;Database=Adra;Trusted_Connection=True;"));          
        }
    }
}
