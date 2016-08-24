using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowWhite
{
     
    class Chunk
    {
        public const int SIZE = 32;
        private Vector4[,,] Block;
        private World World;
        public bool IsEmpty;
        public Vector3 Position { get; }
        public Chunk(World world,Vector3 pos)
        {
            Block = new Vector4[32,32,32];
            World = world;
            Position = pos;
        }

        public void setblockAt(int x, int y, int z,Vector4 color)
        {
            Block[x,y,z] = color;

            IsEmpty = false;
        }

        public Color GetBlockAt(int x, int y, int z)
        {
            Color col = new Color((int)Block[x, y, z].X, (int)Block[x, y, z].Y, (int)Block[x, y, z].Z, (int)Block[x, y, z].W);
            return col;
        }
    }
}
