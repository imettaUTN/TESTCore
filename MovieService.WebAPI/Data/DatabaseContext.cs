using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MovieService.WebAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<Movie> Movies { get; set; }
    }

    public class Movie
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
