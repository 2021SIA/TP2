using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

namespace TP2.Genes
{
    public class ItemGene : IGene<Item>
    {
        public Item Value { get; }

        private static readonly Random rnd = new Random();

        public IGene Mutate()
        {
            return Random(Value.ItemType);
        }

        public ItemGene(Item item)
        {
            Value = item;
        }

        public static ItemGene Random(Item.Type type)
        {
            var collection = Item.Items[type];
            var index = rnd.Next(collection.Count());
            return new ItemGene(collection[index]);
        }
    }
}
