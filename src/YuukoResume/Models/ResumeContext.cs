using Microsoft.EntityFrameworkCore;
using Pomelo.AspNetCore.Extensions.BlobStorage.Models;

namespace YuukoResume.Models
{
    public class ResumeContext : DbContext, IBlobStorageDbContext
    {
        public ResumeContext(DbContextOptions opt) : base(opt)
        {
        }

        public DbSet<Blob> Blobs { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }

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
