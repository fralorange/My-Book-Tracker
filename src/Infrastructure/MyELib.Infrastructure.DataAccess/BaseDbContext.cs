﻿using Microsoft.EntityFrameworkCore;
using MyELib.Infrastructure.DataAccess.Contexts.Document.Configuration;
using MyELib.Infrastructure.DataAccess.Contexts.Library.Configuration;
using MyELib.Infrastructure.DataAccess.Contexts.LibraryUser.Configuration;
using MyELib.Infrastructure.DataAccess.Contexts.User.Configuration;

namespace MyELib.Infrastructure.DataAccess
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    public class BaseDbContext : DbContext
    {
        /// <inheritdoc/>
        public BaseDbContext(DbContextOptions options) : base(options) { }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LibraryConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new LibraryUserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
