using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project2.Models.User {
    public class Character 
    {
        [Key]
        public int Id{get; set;}
        public string Name {get; set;}

         public Character(){
            Name = "";
         }
    }

   

} 