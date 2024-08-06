using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project2.Models.Actor;


namespace Project2.Models.Items {
    public class Item 
    {
        [Key]
        public int Id{get; set;}
        public string Name {get; set;}

        public string Type{get; set;}
        
        public string Description {get; set;}

        public int InventoryId{get; set;}
        public Inventory Inventories {get;set;}

        //public List<GameActor> gameActorItems {get; } = new List<GameActor>();

        public Item(){
            Name = "";
            Type = "";
            Description = "";
        }
    }

    

} 