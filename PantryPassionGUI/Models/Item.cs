using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantryPassionGUI.Models
{
    public class Item
    {
        public Item(string name, string ean="", int averageLifespanDays=0, string itemSize="")
        {
            Name = name;
            EAN = ean;
            AverageLifespanDays = averageLifespanDays;
            ItemSize = itemSize;

        }

        public string Name { get; set; }
        public string EAN { get; set; }
        public int AverageLifespanDays { get; set; }
        public string ItemSize { get; set; }
    }
}
