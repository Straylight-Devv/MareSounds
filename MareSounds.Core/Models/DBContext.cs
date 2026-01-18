using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MareSounds.Core.Models
{
    public class DBContext : DbContext
    {
        static string _dbLocation = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\MareSounds\\sqlite.db";

        public DbSet<Mare> Mares { get; set; }
        public DbSet<VoiceClip> VoiceClips { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                .UseSqlite($"Data Source={_dbLocation}")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .LogTo(msg => { Debug.WriteLine(msg); }, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mare>()
                .HasMany(m => m.VoiceClips)
                .WithOne(vc => vc.Mare)
                .HasForeignKey(vc => vc.MareId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
