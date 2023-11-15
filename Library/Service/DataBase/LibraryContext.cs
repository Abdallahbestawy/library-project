using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DataBase
{
	public class LibraryContext:IdentityDbContext<ApplicationUser>
    {
		public LibraryContext(DbContextOptions<LibraryContext> options):base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<VwUser>().HasNoKey().ToView("VwUsers");
		}
		public DbSet<Book> Books { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<LogBook> LogBooks { get; set; }
		public DbSet<LogCategory> LogCategories { get; set; }
		public DbSet<SubCategory> SubCategories { get; set; }
		public DbSet<LogSubCategory> LogSubCategories { get; set; }
		public DbSet<VwUser> VwUsers { get; set; }
	}
}
