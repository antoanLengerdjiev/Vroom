
using Microsoft.EntityFrameworkCore;
using System;
using Vroom.Data.Models;
using Vroom.Models;

namespace Vroom.Data.Contracts
{
    public interface IVroomDbContext : IDisposable
    {
        DbSet<Make> Makes { get; set; }

        DbSet<Model> Models { get; set; }

        DbSet<Bike> Bikes { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }

        DbSet<T> Set<T>()
            where T : class;
    }
}
