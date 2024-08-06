using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project2.Models.Actor;
namespace Project2.Models.Items {
    public class Inventory 
    {
        [Key]
        public int Id{get; set;}
        public int ItemId {get; set;}

        public int ActorPlayerId{get; set;}
        public ActorPlayer player {get; set;} = null!;

        public List<Item> InventoryItems {get; } = new List<Item>();
        public Inventory(){}
    }

        
        
}
