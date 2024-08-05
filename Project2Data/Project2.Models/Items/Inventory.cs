using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Project2.Models.Items {
    public class Inventory 
    {
        [Key]
        public int Id{get; set;}
        public int ItemId {get; set;}

        public List<Item> InventoryItems {get; } = new List<Item>();
        public Inventory(){}
    }

        
        
}
