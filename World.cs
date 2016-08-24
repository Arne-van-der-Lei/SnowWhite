using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowWhite
{
    class World
    {
        Chunk[,,] Chunks;
        public List<VertexPositionColorNormal> verts;

        public World()
        {
            Chunks = new Chunk[10, 10, 10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        Chunks[i, j, k] = new Chunk(this, new Vector3(i*Chunk.SIZE, j * Chunk.SIZE, k * Chunk.SIZE));
                    }
                }
            }
        }

        public void setBlockAt(int x, int y, int z, Vector4 color)
        {
            Chunks[x / Chunk.SIZE, y / Chunk.SIZE, z / Chunk.SIZE].setblockAt(x % Chunk.SIZE,y % Chunk.SIZE, z % Chunk.SIZE, color);
        }

        public void setBlocksAt(int x, int y , int z, Blocks bl)
        {
            List<Vector4> blo = bl.Block.getdata();
            List<Vector4> pal = bl.Pallet.getdata();
            
            foreach(Vector4 block in blo)
            {
                setBlockAt(x + (int)block.X, y + (int)block.Y, z + (int)block.Z, pal[(int)block.W-1]);
            }
        }

        public VertexPositionColorNormal[] GenerateModel()
        {
            verts = new List<VertexPositionColorNormal>();

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        if (Chunks[x, y, z].IsEmpty == false)
                        {
                            for (int i = 0; i < Chunk.SIZE; i++)
                            {
                                for (int j = 0; j < Chunk.SIZE; j++)
                                {
                                    for (int k = 0; k < Chunk.SIZE; k++)
                                    {
                                        makeCube(x * Chunk.SIZE + i, y * Chunk.SIZE + j, z * Chunk.SIZE + k);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return verts.ToArray();
        }

        private void makeCube(int x,int y, int z)
        {
            Color col = Chunks[x / Chunk.SIZE, y / Chunk.SIZE, z / Chunk.SIZE].GetBlockAt(x % Chunk.SIZE, y % Chunk.SIZE, z % Chunk.SIZE);
            if (col != new Color(0, 0, 0, 0))
            {
                //top
                if (Chunks[x / Chunk.SIZE, y / Chunk.SIZE, z / Chunk.SIZE].GetBlockAt(x % Chunk.SIZE,y % Chunk.SIZE, Game1.Clamp(z % Chunk.SIZE - 1, 0, 31)) == new Color())
                {
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z), col,new Vector3(0,0,-1)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z), col, new Vector3(0, 0, -1)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y + 1, z), col, new Vector3(0, 0, -1)));

                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z), col, new Vector3(0, 0, -1)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y, z), col, new Vector3(0, 0, -1)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z), col, new Vector3(0, 0, -1)));
                }

                //bottom
                if (Chunks[x / Chunk.SIZE, y / Chunk.SIZE, z / Chunk.SIZE].GetBlockAt(x % Chunk.SIZE, y % Chunk.SIZE, Game1.Clamp(z % Chunk.SIZE + 1, 0, 31)) == new Color())
                {
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z + 1), col, new Vector3(0, 0, 1)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y + 1, z + 1), col, new Vector3(0, 0, 1)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z + 1), col, new Vector3(0, 0, 1)));

                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z + 1), col, new Vector3(0, 0, 1)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z + 1), col, new Vector3(0, 0, 1)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y, z + 1), col, new Vector3(0, 0, 1)));
                }

                //back
                if (Chunks[x / Chunk.SIZE, y / Chunk.SIZE, z / Chunk.SIZE].GetBlockAt(Game1.Clamp(x % Chunk.SIZE - 1, 0, 31), y % Chunk.SIZE, z % Chunk.SIZE) == new Color())
                {
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z + 1), col, new Vector3(-1, 0, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z), col, new Vector3(- 1, 0, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y + 1, z), col, new Vector3(-1, 0, 0)));

                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z + 1), col, new Vector3(-1, 0, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y + 1, z), col, new Vector3(-1, 0, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y + 1, z + 1), col, new Vector3(-1, 0, 0)));
                }

                //front
                if (Chunks[x / Chunk.SIZE, y / Chunk.SIZE, z / Chunk.SIZE].GetBlockAt(Game1.Clamp(x % Chunk.SIZE + 1, 0, 31),y % Chunk.SIZE, z % Chunk.SIZE) == new Color())
                {
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y, z + 1), col, new Vector3(1, 0, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z), col, new Vector3(1, 0, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y, z), col, new Vector3(1, 0, 0)));

                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y, z + 1), col, new Vector3(1, 0, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z + 1), col, new Vector3(1, 0, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z), col, new Vector3(1, 0, 0)));
                }

                //left
                if (Chunks[x / Chunk.SIZE, y / Chunk.SIZE, z / Chunk.SIZE].GetBlockAt(x % Chunk.SIZE, Game1.Clamp(y % Chunk.SIZE + 1, 0, 31), z % Chunk.SIZE) == new Color())
                {
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y + 1, z + 1), col, new Vector3(0, 1, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y + 1, z), col, new Vector3(0, 1, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z), col, new Vector3(0, 1, 0)));

                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y + 1, z + 1), col, new Vector3(0, 1, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z), col, new Vector3(0, 1, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y + 1, z + 1), col, new Vector3(0, 1, 0)));
                }

                //right
                if (Chunks[x / Chunk.SIZE, y / Chunk.SIZE, z / Chunk.SIZE].GetBlockAt(x % Chunk.SIZE, Game1.Clamp(y % Chunk.SIZE - 1, 0, 31), z % Chunk.SIZE) == new Color())
                {
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z + 1), col, new Vector3(0, -1, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y, z), col, new Vector3(0, -1, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z), col, new Vector3(0, -1, 0)));

                    verts.Add(new VertexPositionColorNormal(new Vector3(x, y, z + 1), col, new Vector3(0, -1, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y, z + 1), col, new Vector3(0, -1, 0)));
                    verts.Add(new VertexPositionColorNormal(new Vector3(x + 1, y, z), col, new Vector3(0, -1, 0)));
                }
            }
        }
    }
}
