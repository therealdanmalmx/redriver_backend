using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using redriver_test.controllers;
using redriver_test.models;
using redriver_test.Models;


namespace redriver_test.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<BookModel> Books => Set<BookModel>();
        public DbSet<User> Users => Set<User>();
    }
}