using Microsoft.EntityFrameworkCore;
using Project2.Models.Actor;
using Project2.Models.Items;
using Project2.Models.User;
using Project2.Models.Combats;


namespace Project2.Data {
    public class DataContext : DbContext {
        public DbSet<ActorEnemy> Enemies => Set<ActorEnemy>();
        public DbSet<ActorPlayer> Players => Set<ActorPlayer>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<UserPlayer> UserPlayer => Set<UserPlayer>();

        

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

        protected override void OnModelCreating(ModelBuilder PModelBuilder)
        {
            PModelBuilder.Entity<UserPlayer>()
                .HasMany(e => e.userPlayers)
                .WithOne(e => e.user)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);

            PModelBuilder.Entity<Item>()
                .HasMany(e => e.gameActorItems)
                .WithOne(e => e.item)
                .HasForeignKey(e => e.ItemId)
                .HasPrincipalKey(e => e.Id);

            PModelBuilder.Entity<Inventory>()
                .HasMany(e => e.InventoryItems)
                .WithOne(e => e.inventories)
                .HasForeignKey(e => e.Id)
                .HasPrincipalKey(e => e.ItemId);

            PModelBuilder.Entity<Combat>()
                .HasOne(e => e.enemy)
                .WithOne(e => e.combat)
                .HasForeignKey<Combat>(e => e.ActorEnemyId);
                //.HasPrincipalKey(e => e.Id);

            PModelBuilder.Entity<Combat>()
                .HasOne(e => e.player)
                .WithOne(e => e.combat)
                .HasForeignKey<Combat>(e => e.ActorEnemyId);
                //.HasPrincipalKey(e => e.Id);*/

        }

        //  protected override void OnConfiguration(Builder IOptionBuilder)
        // {
        //     DBCOptionBuilder.EntityFrameworkCore<Player>().HasMany(e => e.GameActor)
        //     .WithOne(e => Item)
        //     .HasForeignKey(e => e.ItemId)
        //     .HasPrincipalKey(e => e.Id);
        // }
    }
}