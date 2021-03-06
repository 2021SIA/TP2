using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TP2.Genes;
using System.Diagnostics.CodeAnalysis;

namespace TP2.Models
{
    public class Character : IComparable<Character>
    {
        public enum Type
        {
            WARRIOR,ARCHER,DEFENDER,ROGUE
        }
        public Type CharacterType { get; }

        private IGene[] genes;
        public ReadOnlySpan<IGene> Genes { get => genes; }

        //Gen 1
        private HeightGene height;
        public double Height { get => height.Value; }
        //Gen 2
        private ItemGene helmet;
        public Item Helmet { get => helmet.Value; }
        //Gen 3
        private ItemGene chest;
        public Item Chest { get => chest.Value; }
        //Gen 4
        private ItemGene gloves;
        public Item Gloves { get => gloves.Value; }
        //Gen 5
        private ItemGene weapon;
        public Item Weapon { get => weapon.Value; }
        //Gen 6
        private ItemGene boots;
        public Item Boots { get => boots.Value; }

        public Character(Type characterType)
        {
            this.CharacterType = characterType;
            height = HeightGene.Random();
            helmet = ItemGene.Random(Item.Type.HELMET);
            chest = ItemGene.Random(Item.Type.CHEST);
            gloves = ItemGene.Random(Item.Type.GLOVES);
            weapon = ItemGene.Random(Item.Type.WEAPON);
            boots = ItemGene.Random(Item.Type.BOOTS);

            genes = new IGene[] { height, helmet, chest, gloves, weapon, boots };
        }

        public Character(Type characterType,IGene[] genes)
        {
            this.CharacterType = characterType;
            this.genes = genes;

            height = (HeightGene)genes[0];
            helmet = (ItemGene)genes[1];
            chest = (ItemGene)genes[2];
            gloves = (ItemGene)genes[3];
            weapon = (ItemGene)genes[4];
            boots = (ItemGene)genes[5];
        }

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

        private double fitness = double.NaN;
        public double Fitness
        {
            get
            {
                if (!double.IsNaN(fitness))
                    return fitness;
                switch (CharacterType)
                {
                    case Type.WARRIOR: return fitness = 0.6 * Attack + 0.6 * Defense;
                    case Type.ARCHER: return fitness = 0.9 * Attack + 0.1 * Defense;
                    case Type.DEFENDER: return fitness = 0.3 * Attack + 0.8 * Defense;
                    case Type.ROGUE: return fitness = 0.8 * Attack + 0.3 * Defense;
                    default: return -1;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if(obj == this)
            {
                return true;
            }
            if(!(obj is Character other))
            {
                return false;
            }
            return Math.Abs(this.Height - other.Height) < 0.01 && 
                this.Helmet.Equals(other.Helmet) && 
                this.Chest.Equals(other.Chest) && 
                this.Weapon.Equals(other.Weapon) && 
                this.Gloves.Equals(other.Gloves) &&
                this.Boots.Equals(other.Boots);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(
                this.Height,
                this.Helmet.GetHashCode(),
                this.Chest.GetHashCode(),
                this.Weapon.GetHashCode(),
                this.Gloves.GetHashCode(),
                this.Boots.GetHashCode());
        }

        public int CompareTo([AllowNull] Character other)
        {
            return Fitness.CompareTo(other.Fitness);
        }
    }
}
