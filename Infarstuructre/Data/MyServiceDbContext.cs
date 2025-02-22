using Domin.Entity;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.Data
{
   public class MyServiceDbContext :IdentityDbContext<ApplicationUser>
    {
       
        public MyServiceDbContext(DbContextOptions<MyServiceDbContext> options ) : base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employees>()
    .HasKey(e => e.Id);

            builder.Entity<Employees>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

           
            builder.Entity<Category>().Property(x => x.Id).HasDefaultValueSql("(newid())");
            builder.Entity<LogCategory>().Property(x => x.Id).HasDefaultValueSql("(newid())");
            builder.Entity<SubCategory>().Property(x => x.Id).HasDefaultValueSql("(newid())");
            builder.Entity<LogSubCategory>().Property(x => x.Id).HasDefaultValueSql("(newid())");
            builder.Entity<LogBook>().Property(x => x.Id).HasDefaultValueSql("(newid())");
            //builder.Entity<VwUser>().HasNoKey().ToView("VwUsers");
            builder.Entity<VwUser>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwUsers");
            });
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<LogCategory> LogCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<LogSubCategory> LogSubCategories { get; set; }
        public DbSet<LogBook> LogBooks { get; set; }
        public DbSet<VwUser> VwUsers { get; set; }
        public DbSet<EmployeeRequest> employeeRequests { get; set; }
        public DbSet<Employees> employees { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Provider> providers { get; set; }
        public DbSet<Request> requests { get; set; }
        public DbSet<Service> services { get; set; }
    }
}
