using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowWhite
{
    static class Util
    {

        public static Blocks GenerateBlockFromFile(string name)
        {

            List<Vector4> list = new List<Vector4>();

            FileStream str = new FileStream("./Content/" + name + ".vox", FileMode.Open);

            BinaryReader read = new BinaryReader(str);

            Vector3 size;
            Map pal = new Map();
            Map map = new Map();

            Console.WriteLine(read.ReadChars(4));
            Console.WriteLine(read.ReadUInt32());

            Console.WriteLine(read.ReadChars(4));
            Console.WriteLine(read.ReadUInt32());
            Console.WriteLine(read.ReadUInt32());

            String key = new String(read.ReadChars(4)) ;

            Console.WriteLine(read.ReadUInt32());
            Console.WriteLine(read.ReadUInt32());

            for (int i = 0; i < 3; i++)
            {
                switch (key)
                {
                    case "SIZE":
                        size = ReadSizeChunk(read);
                        break;
                    case "XYZI":
                        map = ReadXYZIChunk(read,name);
                        break;
                    case "RGBA":
                        pal = ReadRGBAChunk(read,name);
                        break;
                }

                key = new String(read.ReadChars(4));
                if (key != "")
                {
                    Console.WriteLine(read.ReadUInt32());
                    Console.WriteLine(read.ReadUInt32());
                }else
                {
                    i = 3;
                }
            }

            return new Blocks(map, pal,name);

        }

        private static Map ReadRGBAChunk(BinaryReader read, string name)
        {

            List<Vector4> items = new List<Vector4>();
            for (int i = 0; i < 256; i++)
            {
                items.Add(new Vector4(read.ReadByte(), read.ReadByte(), read.ReadByte(), read.ReadByte()));
            }
            return new Map(items, name + "_block");
        }

        private static Map ReadXYZIChunk(BinaryReader read,string name)
        {
            List<Vector4> items = new List<Vector4>();
            int number = read.ReadInt32();

            for(int i = 0; i< number; i++)
            {
                items.Add(new Vector4(read.ReadByte(), read.ReadByte(), read.ReadByte(), read.ReadByte()));
            }
            return new Map(items, name + "_pallet");
        }

        private static Vector3 ReadSizeChunk(BinaryReader read)
        {
            Vector3 size = new Vector3();
            size.X = read.ReadUInt32();
            size.Y = read.ReadUInt32();
            size.Z = read.ReadUInt32();
            return size;
        }
    }
}
