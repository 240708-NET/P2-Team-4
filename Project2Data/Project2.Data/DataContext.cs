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
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<Combat> Combats => Set<Combat>();


        

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
            //One Player to many characters (ActorPlayers)
            PModelBuilder.Entity<UserPlayer>()
                .HasMany(e => e.userPlayers)
                .WithOne(e => e.user)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);

            //1 character to 1 inventoryID
            PModelBuilder.Entity<Inventory>()
                .HasOne(e => e.player)
                .WithOne(e => e.inventories)
                .HasForeignKey<Inventory>(e => e.ActorPlayerId);

            //One Inventory to many Items
            PModelBuilder.Entity<Inventory>()
                .HasMany(e => e.InventoryItems)
                .WithOne(e => e.Inventories)
                .HasForeignKey(e => e.InventoryId)
                .HasPrincipalKey(e => e.Id);

            //One Combat to one enemy
            PModelBuilder.Entity<Combat>()
                .HasOne(e => e.enemy)
                .WithOne(e => e.combat)
                .HasForeignKey<Combat>(e => e.ActorEnemyId);
                //.HasPrincipalKey(e => e.Id);

            //One Combat to one player (character)
            PModelBuilder.Entity<Combat>()
                .HasOne(e => e.player)
                .WithOne(e => e.combat)
                .HasForeignKey<Combat>(e => e.ActorPlayerId);
                //.HasPrincipalKey(e => e.Id);*/

        }

       
    }
}