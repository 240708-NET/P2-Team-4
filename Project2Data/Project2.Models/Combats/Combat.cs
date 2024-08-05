using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project2.Models.Actor;

namespace Project2.Models.Combats {
    public class Combat 
    {
        [Key]
        public int Id {get; set;}
        public int ActorPlayerId{get; set;}
        
        public int ActorEnemyId{get; set;}
        public ActorEnemy enemy {get; set;} = null!;
        public ActorPlayer player {get; set;} = null!;

    public Combat(){}
    }
    

} 