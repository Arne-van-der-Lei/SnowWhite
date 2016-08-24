using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowWhite
{
    class Map
    {
        private List<Vector4> Blocks;
        public string name { get; }

        public Map(List<Vector4> blocks, string name)
        {
            this.Blocks = blocks;
            this.name = name;
        }

        public Map()
        {
            this.Blocks = new List<Vector4>();
            this.name = "unused";
        }

        public List<Vector4> getdata()
        {
            return Blocks;
        }
    }
}
