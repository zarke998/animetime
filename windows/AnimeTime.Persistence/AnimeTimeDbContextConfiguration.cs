using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Persistence
{
    public class AnimeTimeDbContextConfiguration : DbConfiguration
    {
        public AnimeTimeDbContextConfiguration()
        {
            this.SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }
}
