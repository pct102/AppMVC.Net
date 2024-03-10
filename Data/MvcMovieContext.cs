using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App.Models.Blog;
using App.Models.ContactInfo;

namespace MvcMovie.Data
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>( entity => {
                entity.HasIndex(c => c.Slug)
                    .IsUnique();
                    });

            // modelBuilder.Entity<PostCategory>( entity => {
            //     entity.HasIndex(c => c.Slug)
            //         .IsUnique();
            // });
        }

       public DbSet<Category> Categories { get; set; }
       public DbSet<Contact> Contacts { get; set; }
       
    //    public DbSet<Post> Posts { get; set; }
    //    public DbSet<PostCategory> PostCategories { get; set; }
    }
}
