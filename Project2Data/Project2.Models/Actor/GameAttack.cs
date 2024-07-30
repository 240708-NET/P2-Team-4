namespace Project2.Models.Actor {
    public class GameAttack {
        //  _Database Variables
        //public string Id { get; set; }

        //  Attack Variables
        public string Attack_Name { get; private set; }
        public string Attack_Action { get; private set; }
        public string Attack_Type { get; private set; } // Melee, Ranged, Spell, Saving Throw
        public string Attack_Mod  { get; private set; } // Melee/Ranged/Spell Mod, Saving Throw DC

        //  Damage Variables
        public List<string> Attack_Damages { get; private set; }

        public GameAttack() {
            Attack_Name = "";
            Attack_Action = "";
            Attack_Type = "";
            Attack_Mod = "";

            Attack_Damages = new List<string>();
        }

        //  Constructor
        /// <summary>
        /// Basic Attack Structure
        /// </summary>
        /// <param name="pName">Name of the attack</param>
        /// <param name="pAction">Action used for the attack</param>
        /// <param name="pType">Attack type [Melee, Ranged, Spell, Saving Throw]</param>
        /// <param name="pMod">Attack modification [Attack Mod, Saving Throw DC]</param>
        /// <param name="pDamages">Damage information (Dice/[Mod]/Type)</param>
        public GameAttack(string pName, string pAction, string pType, int pMod, string pDamages) {
            //  Part - Setup Attack
            Attack_Name = "" + pName;
            Attack_Type = "" + pType;
            Attack_Action = "" + pAction;
            Attack_Mod = "" + pMod;
            
            //  Part - Setup Damage
            Attack_Damages = new List<string>();

            //  SubPart - Split pDamages into individual damages
            string[] attackArr = pDamages.Split(", ");
            for (int i = 0; i < attackArr.Length; i++) {
                string[] damageArr = attackArr[i].Split("_");                
                Attack_Damages.Add(attackArr[i]);
            }
        }

        //  Copy Constructor
        public GameAttack(GameAttack pAtk) {
            //  Part - Setup Attack
            Attack_Name = "" + pAtk.Attack_Name;
            Attack_Type = "" + pAtk.Attack_Type;
            Attack_Action = "" + pAtk.Attack_Action;
            Attack_Mod = "" + pAtk.Attack_Mod;
            
            //  Part - Setup Damage
            Attack_Damages = new List<string>();

            foreach(string damage in pAtk.Attack_Damages) {
                Attack_Damages.Add(damage);
            }
        }

        //  MainMethod - Get Damage
        public int GetDamage(Random pRand, string pDmg) {
            int damage = 0;
            string[] damageArr = pDmg.Split("_");

            //  Part - Add Dice Damage
            if (damageArr[0] != "0") {
                string[] diceArr = damageArr[0].Split("d");

                for (int i = 0; i < int.Parse(diceArr[0]); i++) {
                    damage += pRand.Next(1, int.Parse(diceArr[1])+1);
                }
            }

            //  Part - Add Mod Damage
            damage += int.Parse(damageArr[1]);

            return damage;
        }
    }
}