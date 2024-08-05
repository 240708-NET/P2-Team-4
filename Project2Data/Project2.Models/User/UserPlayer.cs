using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project2.Models.Actor;

namespace Project2.Models.User {
    public class UserPlayer {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<ActorPlayer> UserPlayers { get; } = new List<ActorPlayer>();


        public UserPlayer() {
            Name = "";
        }
    }
} 