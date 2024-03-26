using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.DAL.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
