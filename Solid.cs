using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SnowWhite
{
    class Solid : GameComponent
    {
        string modelName;
        Model mesh;
        public Vector3 position { set; get; }

        public Solid(Game game, String modelName) : base(game)
        {
            this.modelName = modelName;
        }

        public void Load()
        {
            this.mesh = this.Game.Content.Load<Model>(modelName);
        }

        public void Draw(Camera cam)
        {
            foreach (var mesh in mesh.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.View = cam.View;
                    effect.World = Matrix.CreateTranslation(position) * Matrix.CreateScale(1/16);
                    effect.Projection = cam.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
