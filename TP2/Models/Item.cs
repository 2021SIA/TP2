using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace TP2.Models
{
    public class Item
    {
        public enum Type
        {
            WEAPON,BOOTS,HELMET,GLOVES,CHEST
        }
        public int Id { get; }
        public double Force { get; }
        public double Agility { get; }
        public double Expertise { get; }
        public double Resistance { get; }
        public double Health { get; }
        public Type ItemType { get; }

        public static IReadOnlyDictionary<Type, IReadOnlyList<Item>> Items { get; private set; }

        public static void LoadItems(IEnumerable<Item> items)
        {
            Items = items
                .ToLookup(i => i.ItemType)
                .ToImmutableDictionary( g => g.Key, 
                                        g => (IReadOnlyList<Item>)g.ToImmutableList());
        }

        public Item(int id, double force, double agility, double expertise, double resistance, double health, Type itemType)
        {
            this.Id = id;
            this.Force = force;
            this.Agility = agility;
            this.Expertise = expertise;
            this.Resistance = resistance;
            this.Health = health;
            this.ItemType = itemType;
        }
    }
}
