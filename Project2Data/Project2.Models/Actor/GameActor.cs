using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project2.Models.Items;
using Project2.Models.Combats;

namespace Project2.Models.Actor {
    public class GameActor {
        //  _Server Variables
        [Key]
        public int Id { get; set; }

        public int ItemId{ get; set;}
        //public int InventoryId{ get; set;}

        // public Item item {get;set;}
        public Combat? combat{get; set;} 

        
        
        //  Attribute Variables
        [NotMapped]
        public Dictionary<string, int> D_AttrScr { get; set; }
        [NotMapped]
        public Dictionary<string, int> D_AttrMod { get; set; }
        public string Attributes { get; set; }


        //  Combat Variables
        [NotMapped]
        public GameAttack Atk_Unarmed { get; set; }
        public string AttackUnarmed { get; set; }

        [NotMapped]
        public List<GameAttack> Atk_List { get; set; }
        public string AttackList { get; set; }

        //  Defense Variables
        [NotMapped]
        public int Def_UnarmoredAC { get; set; }

        [NotMapped]
        public string Def_ArmoredName { get; set; }
        [NotMapped]
        public int Def_ArmoredAC { get; set; }
        public string DefenseArmor { get; set; }

        [NotMapped]
        public int Def_AC => (Def_ArmoredAC != -1) ? Def_ArmoredAC : Def_UnarmoredAC;

        //  Health Variables
        public string Health { get; set; }
        public string HealthDice { get; set; }
        [NotMapped]
        public int HealthBase { get; set; }
        [NotMapped]
        public int HealthCurr { get; set; }
        [NotMapped]
        public string HealthStr => $"{HealthCurr}/{HealthBase}";

        //  Name Variables
        public string Name { get; set; }
        [NotMapped]
        public bool Proper { get; set; }
        [NotMapped]
        public string Article { get; set; }

        //  Skill Variables
        public int Proficiency { get; set; }

        [NotMapped]
        public string ActorStr => $"{Id}, {Name}, {Attributes}, {AttackUnarmed}";

        //  Default Constructor
        public GameActor() {
            //  Setup Attribute
            Attributes = "";
            D_AttrScr = new Dictionary<string, int>();
            D_AttrMod = new Dictionary<string, int>();

            //  Setup Combat
            AttackUnarmed = "";
            Atk_Unarmed = new GameAttack();

            AttackList = "";
            Atk_List = new List<GameAttack>();

            //  Setup Defense
            Def_UnarmoredAC = 10;

            DefenseArmor = "";
            Def_ArmoredName = "";
            Def_ArmoredAC = 0;

            //  Setup Health
            HealthDice = "";
            HealthBase = 0;
            HealthCurr = 0;
            Health = $"{HealthCurr}/{HealthBase}";

            //  Setup Name
            Name = "";
            Article = "";

            //  Setup Skill
            Proficiency = 2;
        }

        //  Copy Constructor
        public GameActor(GameActor pActor) {
            //  Setup _Server
            Id = 0 + pActor.Id;

            //  Setup Attribute
            Attributes = "" + pActor.Attributes;
            D_AttrScr = new Dictionary<string, int>() {
                ["STR"] = 0,
                ["DEX"] = 0,
                ["CON"] = 0,
                ["INT"] = 0,
                ["WIS"] = 0,
                ["CHA"] = 0,
            };
            D_AttrMod = new Dictionary<string, int>() {
                ["STR"] = 0,
                ["DEX"] = 0,
                ["CON"] = 0,
                ["INT"] = 0,
                ["WIS"] = 0,
                ["CHA"] = 0,
            };

            if (Attributes.Contains(",")) {
                int[] attrArr = Array.ConvertAll(Attributes.Split(","), int.Parse);
                if (attrArr.Length == 6) {
                    D_AttrScr["STR"] = attrArr[0];
                    D_AttrScr["DEX"] = attrArr[1];
                    D_AttrScr["CON"] = attrArr[2];
                    D_AttrScr["INT"] = attrArr[3];
                    D_AttrScr["WIS"] = attrArr[4];
                    D_AttrScr["CHA"] = attrArr[5];

                    D_AttrMod["STR"] = (D_AttrScr["STR"] / 2) - 5;
                    D_AttrMod["DEX"] = (D_AttrScr["DEX"] / 2) - 5;
                    D_AttrMod["CON"] = (D_AttrScr["CON"] / 2) - 5;
                    D_AttrMod["INT"] = (D_AttrScr["INT"] / 2) - 5;
                    D_AttrMod["WIS"] = (D_AttrScr["WIS"] / 2) - 5;
                    D_AttrMod["CHA"] = (D_AttrScr["CHA"] / 2) - 5;
                }
            }

            //  Setup Combat
            AttackUnarmed = "" + pActor.AttackUnarmed;
            Atk_Unarmed = new GameAttack();

            //  Attack has valid parts (Ex: fists_strikes with_Melee_0/0_1_bludgeoning)
            string[] unarmedArr1 = AttackUnarmed.Split("/");
            string[] unarmedArr2 = unarmedArr1[0].Split("_");
            if (unarmedArr1.Length == 2 && unarmedArr2.Length == 4) {
                Atk_Unarmed = new GameAttack(unarmedArr2[0], unarmedArr2[1], unarmedArr2[2], int.Parse(unarmedArr2[3]), unarmedArr1[1]);
            }

            AttackList = "" + pActor.AttackList;
            Atk_List = new List<GameAttack>();

            if (AttackList != "") {
                string[] atklistArr1 = AttackList.Split(",");
                string[] atklistArr2;
                string[] atklistArr3;

                foreach(string attack in atklistArr1) {
                    //  Attack has valid parts (Ex: fists_strikes with_Melee_0/0_1_bludgeoning)
                    atklistArr2 = AttackUnarmed.Split("/");
                    atklistArr3 = atklistArr2[0].Split("_");
                    if (atklistArr2.Length == 2 && atklistArr3.Length == 4) {
                        Atk_List.Add(new GameAttack(atklistArr3[0], atklistArr3[1], atklistArr3[2], int.Parse(atklistArr3[3]), atklistArr2[1]));
                    }
                }
            }

            //  Setup Defense
            Def_UnarmoredAC = 10 + D_AttrMod["DEX"];

            DefenseArmor = "" + pActor.DefenseArmor;
            Def_ArmoredName = "";
            Def_ArmoredAC = -1;

            //  Armor has valid parts (Ex: Plate Armor_18, Leather Armor_11+DEX, or Breastplate_14+DEX/M2)
            string[] armorArr = DefenseArmor.Split("_");
            if (armorArr.Length == 2) {
                //  Set armor name
                Def_ArmoredName = "" + armorArr[0];

                //  Armor contains attr mod
                if (armorArr[1].Contains("+")) {
                    string[] armorACArr = armorArr[1].Split("+");

                    //  Add armor's base AC
                    Def_ArmoredAC += int.Parse(armorACArr[0]);

                    //  Armor has a maximum bonus
                    if (armorACArr[1].Contains("/")) {
                        string[] armorACArr2 = armorACArr[1].Split("/");

                        //  Add armor's attrMod
                        switch(armorACArr2[1]) {
                            case "M2":
                                Def_ArmoredAC += Math.Min(D_AttrMod[armorACArr2[0]], 2);
                                break;
                        }
                        
                    }
                    else {
                        //  Add armor's attrMod
                        Def_ArmoredAC += D_AttrMod[armorACArr[1]];
                    }
                }
                else {
                    //  Add armor's base AC
                    Def_ArmoredAC += int.Parse(armorArr[1]);
                }
            }

            //  Setup Health
            HealthDice = "" + pActor.HealthDice;

            Health = "" + pActor.Health;
            string[] healthArr = Health.Split("/");
            HealthCurr = (healthArr.Length == 2) ? int.Parse(healthArr[0]) : 0;
            HealthBase = (healthArr.Length == 2) ? int.Parse(healthArr[1]) : 0;

            //  Setup Name
            Name = "" + ((pActor.Name != null) ? pActor.Name : "");
            Article = "";

            if (Name.Contains("_")) {
                string[] nameArr = Name.Split("_");
                Name = "" + nameArr[0];
                Proper = (nameArr[1] == "True");
            }
            else {
                Proper = false;
            }

            //  Chooses a or an for article for grammar
            string first = Name.Substring(0, 1);
            Article = ((first == "a") || (first == "e") || (first == "i") || (first == "o") || (first == "u")) ? "an" : "a";

            //  Setup Skill
            Proficiency = 0 + pActor.Proficiency;
        }
    
        //  MainMethod - Attack
        /// <summary>
        /// Character-level method for attacking
        /// </summary>
        /// <param name="pRand">Reference to global random</param>
        /// <param name="pTarget">Targeted actor for the attack</param>
        public string Attack(Random pRand, GameActor pTarget) {
            int rand = (Atk_List.Count > 0) ? pRand.Next(0, Atk_List.Count) : -1;
            return AttackCalc(pRand, pTarget, (rand == -1) ? Atk_Unarmed : Atk_List[rand]);
        }

        private string AttackCalc(Random pRand, GameActor pTarget, GameAttack pAtk) {
            string result = "";

            int attackMod = int.Parse(pAtk.Attack_Mod);

            //  Part - Calculate if attack lands
            int dice = pRand.Next(1, 21);
            int attrMod = 0;

            switch(pAtk.Attack_Type) {
                //  Melee Attack
                case "Melee":
                    bool finesse = false;
                    attrMod = ((finesse == true && D_AttrMod["DEX"] > D_AttrMod["STR"]) ? D_AttrMod["DEX"] : D_AttrMod["STR"]);
                    int modNum = attackMod + attrMod + Proficiency;
                    string modStr = $"{((modNum != 0) ? ((modNum > 0) ? "+" : "") + modNum : "")}";

                    result += $"{Name} {pAtk.Attack_Action} {pAtk.Attack_Name}, ";
                    result += $"rolls {dice}{modStr}({(dice + attackMod + attrMod + Proficiency)})";

                    //  Part - Check vs Target AC
                    if ((dice + attackMod + attrMod + Proficiency) >= pTarget.Def_AC) {
                        result += ", and hits!";
                        result += "_" + (dice + attackMod + attrMod + Proficiency) + "/n";
                        result += DealDamage(pRand, pTarget, pAtk, attrMod);
                    }
                    else {
                        result += ", and misses!";
                        result += "_" + (dice + attackMod + attrMod + Proficiency) + "/n";
                    }

                    return result;
            }

            return "-999";
        }

        //  SubMethod of Attack - Deal Damage (param Random, Target, Attack, Mod)
        private string DealDamage(Random pRand, GameActor pTarget, GameAttack pAtk, int pMod) {
            List<string> attackDamages = pAtk.Attack_Damages;
            List<string> attackDmgStrs = new List<string>();
            List<int> attackDmgVals = new List<int>();

            //  Get Damage Actual (Gets the total values for the damages)
            for (int i = 0; i < attackDamages.Count; i++) {
                attackDmgVals.Add(pAtk.GetDamage(pRand, attackDamages[i]));
                attackDmgStrs.Add(attackDmgVals[i] + ((i == 0 && pMod != 0) ? ((pMod > 0) ? "+" : "") + pMod : "") + " " + attackDamages[i].Split("_")[2]);
                attackDmgVals[0] += ((i == 0) ? pMod : 0);
            }

            // Get Attack String (Gets the printable version of values for the damages)
            string damageStr = $"{Name} attacks for ";
            if (attackDmgStrs.Count == 1) {
                damageStr += $"{attackDmgStrs[0]} damage";
            }
            else if (attackDmgStrs.Count == 2) {
                damageStr += $"{attackDmgStrs[0]} and {attackDmgStrs[1]} damage";
            }
            else if (attackDmgStrs.Count >= 3) {
                for (int i = 0; i < attackDmgStrs.Count-1; i++) {
                    damageStr += attackDmgStrs[i] + ", ";
                }
                damageStr += "and " + attackDmgStrs[attackDmgStrs.Count-1] + " damage";
            }
            
            damageStr += "/n";

            //  Apply Damage (Applies damage if > 0)
            for (int i = 0; i < attackDamages.Count; i++) {
                if (attackDmgVals[i] > 0) {
                    damageStr += pTarget.TakeDamage(attackDmgVals[i], attackDamages[i].Split("_")[2]);
                }
            }
            
            return damageStr;
        }
      
        //  MainMethod - Take Damage (param Amount, Type)
        /// <summary>
        /// Character-level method for taking damage
        /// </summary>
        /// <param name="pAmt">Amount of damage taken</param>
        /// <param name="pType">Type of damage taken</param>
        public string TakeDamage(int pAmt, string pType) {
            if (HealthCurr > 0) {
                HealthCurr -= pAmt;

                return $"{Name} takes {pAmt} {pType} damage/n";
            }

            return "";
        }

        //  MainMethod - Restore Health
        /// <summary>
        /// Character-level method for restoring current health to base health
        /// </summary>
        public void RestoreHealth() {
            HealthCurr = 0 + HealthBase;

            //Console.WriteLine($"Player restores hp to full ({HealthCurr})");
            //Console.WriteLine("");
        }

        //  MainMethod - Restore Health
        /// <summary>
        /// Character-level method for restoring current health to base health
        /// </summary>
        /// <param name="pHp">Amount to restore</param>
        public void RestoreHealth(int pHp) {
            HealthCurr += 0 + pHp;
            HealthCurr = Math.Min(HealthBase, HealthCurr);

            //Console.WriteLine($"Player restores 2 hp to {HealthCurr}");
            //Console.WriteLine("");
        }
    }
}