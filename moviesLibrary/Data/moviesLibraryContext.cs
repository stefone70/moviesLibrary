using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using moviesLibrary.Models;

namespace moviesLibrary.Data
{
    public class moviesLibraryContext : DbContext
    {
        public moviesLibraryContext (DbContextOptions<moviesLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<moviesLibrary.Models.Film> Film { get; set; }
    }
}
