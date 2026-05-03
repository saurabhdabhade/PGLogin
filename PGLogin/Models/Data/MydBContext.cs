using Microsoft.EntityFrameworkCore;
using PGLogin.Models;

namespace PGLogin.Models.Data
{
    public class MydBContext : DbContext
    {
        public MydBContext(DbContextOptions<MydBContext> options) : base(options)
        {

        }
        public DbSet<Booking> bookings { get; set; }
        public  DbSet<Candidate> candidates { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<Register> registers { get; set; }  
        public DbSet<Login> Login { get; set; } = default!;
        public DbSet<Area> areas { get; set; }
        public DbSet<PG> pGs { get; set; }
        public DbSet<User> users { get; set; }  
        public DbSet<Page_Master> page_Masters  { get; set; }
        public DbSet<Role_Master> role_Masters { get; set; }
        public DbSet<Role_Page_Mapper> role_Page_Mappers  { get; set; }

    }
}
