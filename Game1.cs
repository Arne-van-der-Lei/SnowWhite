using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnowWhite;

namespace SnowWhite
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        Vector3 cameraPosition = new Vector3(200, 200, 200);
        Camera cam;

        Solid LittleHouse;
        Blocks littleHouse;
        Blocks Monument;

        World world;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.ToggleFullScreen();
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            
            cam = new Camera(this, new Vector3(6, 6, 2.8f), Vector3.Zero, 5f);
            LittleHouse = new Solid(this, "LittleHouse");
            littleHouse = Util.GenerateBlockFromFile("LittleHouse");
            Monument = Util.GenerateBlockFromFile("monu1");

            world = new World();

            world.setBlocksAt(0, 0, 0, Monument);


            world.GenerateModel();
            Components.Add(cam);
            Components.Add(LittleHouse);


            effect = new BasicEffect(graphics.GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            LittleHouse.Load();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            LittleHouse.Draw(cam);

            VertexPositionColorNormal[] verts = world.verts.ToArray();
            
            effect.VertexColorEnabled = true;
            effect.View = cam.View;
            effect.World = Matrix.CreateTranslation(0, 0, 0);
            effect.Projection = cam.Projection;
            effect.EnableDefaultLighting();

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,verts,0,verts.Length/3);
            }

            /*spriteBatch.Begin();
            SpriteFont spf = Content.Load<SpriteFont>("arial");
            spriteBatch.DrawString(spf, cam.isInAir.ToString(),new Vector2(0,0),new Color());

            */
            base.Draw(gameTime);
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }

    public struct VertexPositionColorNormal:IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;
        

        public VertexPositionColorNormal(Vector3 pos, Color col, Vector3 nor) 
        {
            Position = pos;
            Color = col;
            Normal = nor;
        }
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return new VertexDeclaration
                (
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                    new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
                );
            }
        }
    }
}
