using Microsoft.EntityFrameworkCore;
using Project2.Models.Actor;

namespace Project2.Data {
    public class DataContext : DbContext {
        public DbSet<GameActor> Enemies => Set<GameActor>();

        //  Constructor
        public DataContext() {

        }
        
        //  Constructor [For Non-API use]
        public DataContext(DbContextOptions<DataContext> pDBCOptions) : base(pDBCOptions) {
            
        }

        //  SubMethod of Constructor - On Configuring [For Non-API use]
        protected override void OnConfiguring(DbContextOptionsBuilder pDBCOptionsBuilder) {
            pDBCOptionsBuilder.UseSqlServer(File.ReadAllText("../Project2.Data/ConnectionString"));
        }
    }
}