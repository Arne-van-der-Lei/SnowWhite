using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowWhite
{
    class Blocks
    {
        public Map Block { get; }
        public Map Pallet { get; }
        public string Name { get; }
        
        public Blocks(Map block,Map pallet,string name)
        {
            Block = block;
            Pallet = pallet;
            Name = name;
        }
    }
}
