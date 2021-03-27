using System;
using System.Collections.Generic;
using System.Text;

namespace TP2.Models
{
    public class Character
    {
        public enum Type
        {
            WARRIOR,ARCHER,DEFENDER,ROGUE
        }
        public Type CharacterType { get; }

        //Gen 1
        public double Height { get; set; }
        //Gen 2
        public Item Helmet { get; set; }
        //Gen 3
        public Item Chest { get; set; }
        //Gen 4
        public Item Gloves { get; set; }
        //Gen 5
        public Item Weapon { get; set; }
        //Gen 6
        public Item Boots { get; set; }

        public double Force
        {
            get => 100.0 * Math.Tanh(0.01 * (Chest.Force + Helmet.Force + Gloves.Force + Weapon.Force + Boots.Force));
        }
        public double Agility
        {
            get => Math.Tanh(0.01 * (Chest.Agility + Helmet.Agility + Gloves.Agility + Weapon.Agility + Boots.Agility));
        }
        public double Expertise
        {
            get => 0.6 * Math.Tanh(0.01 * (Chest.Expertise + Helmet.Expertise + Gloves.Expertise + Weapon.Expertise + Boots.Expertise));
        }
        public double Resistance
        {
            get => Math.Tanh(0.01 * (Chest.Resistance + Helmet.Resistance + Gloves.Resistance + Weapon.Resistance + Boots.Resistance));
        }
        public double Health
        {
            get => 100.0 * Math.Tanh(0.01 * (Chest.Health + Helmet.Health + Gloves.Health + Weapon.Health + Boots.Health));
        }
        public double ATM
        {
            get => 0.7 - Math.Pow(3.0 * Height - 5.0, 4) + Math.Pow(3.0 * Height - 5.0, 2) + Height / 4.0;
        }
        public double DEM
        {
            get => 1.9 + Math.Pow(2.5 * Height - 4.16, 4) - Math.Pow(2.5 * Height - 4.16, 2) - (3.0 * Height) / 10.0;
        }
        public double Attack
        {
            get => (Agility + Expertise) * Force * ATM;
        }
        public double Defense
        {
            get => (Resistance + Expertise) * Health * DEM;
        }
        public double Fitness
        {
            get
            {
                switch (CharacterType)
                {
                    case Type.WARRIOR: return 0.6 * Attack + 0.6 * Defense;
                    case Type.ARCHER: return 0.9 * Attack + 0.1 * Defense;
                    case Type.DEFENDER: return 0.3 * Attack + 0.8 * Defense;
                    case Type.ROGUE: return 0.8 * Attack + 0.3 * Defense;
                    default: return -1;
                }
            }
        }
    }
}
