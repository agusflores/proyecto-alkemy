using Microsoft.EntityFrameworkCore;
using OngProject.DataAccess;
using OngProject.Repositories;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngProject.Test
{
    public class TestHelper
    {
        private readonly OngDbContext _ongDbContext;

        public TestHelper()
        {
            var builder = new DbContextOptionsBuilder<OngDbContext>();
            builder.UseInMemoryDatabase(databaseName: "OngDbContextInMemory");

            var dbContextOptions = builder.Options;
            _ongDbContext = new OngDbContext(dbContextOptions);

            _ongDbContext.Database.EnsureDeleted();
            _ongDbContext.Database.EnsureCreated();
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork(_ongDbContext);
        }

        public OngDbContext GetOngDbContext()
        {
            return _ongDbContext;
        }
    }
}
