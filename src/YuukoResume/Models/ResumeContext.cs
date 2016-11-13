using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.AspNetCore.Extensions.BlobStorage.Models;

namespace YuukoResume.Models
{
    public class ResumeContext : DbContext, IBlobStorageDbContext
    {
        private IServiceProvider services;
        public ResumeContext(IServiceProvider services, DbContextOptions opt) : base(opt)
        {
            this.services = services;
        }

        public DbSet<Blob> Blobs { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInternalServiceProvider(services);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.SetupBlobStorage();

            builder.Entity<Certificate>(e => 
            {
                e.HasIndex(x => x.PRI);
            });

            builder.Entity<Education>(e =>
            {
                e.HasIndex(x => x.From);
            });

            builder.Entity<Experience>(e =>
            {
                e.HasIndex(x => x.From);
            });

            builder.Entity<Log>(e => 
            {
                e.HasIndex(x => x.IsRead);
                e.HasIndex(x => x.Time);
            });

            builder.Entity<Project>(e =>
            {
                e.HasIndex(x => x.From);
            });

            builder.Entity<Skill>(e =>
            {
                e.HasIndex(x => x.Performance);
                e.HasIndex(x => x.Level);
            });
        }
    }
}
