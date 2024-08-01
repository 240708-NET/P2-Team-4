using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Models.Items {
    public class Item 
    {
        [Key]
        public int Id{get; set;}
        public string Name {get; set;}

        public string Type{get; set;}

        public string Description {get; set;}

        public Item(){
            Name = "";
            Type = "";
            Description = "";
        }
    }

    

} 