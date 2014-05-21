using System.Data.Entity;
using MonarchTestBooking.Models;

namespace MonarchTestBooking.Data
{
    public class MonarchContext : DbContext
    {

        public MonarchContext()
            : base("MonarchContext")
        {
                
        }

        public virtual DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>().HasKey(e => e.Id);
        }
    }
}
