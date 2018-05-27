using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Film.Models;
using Microsoft.AspNetCore.Identity;

namespace Film.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Films { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CategoryFilm>().HasKey(c => new {c.CategoryId, c.FilmId});
            builder.Entity<CategoryFilm>().HasOne(c => c.Category).WithMany(c => c.Films).HasForeignKey(c => c.CategoryId);
            builder.Entity<CategoryFilm>().HasOne(c => c.Film).WithMany(c => c.Categories).HasForeignKey(c => c.FilmId);

            base.OnModelCreating(builder);
        }
    }
}
