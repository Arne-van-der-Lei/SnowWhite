using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SnowWhite
{
    class Camera : GameComponent
    {
        private Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private float cameraSpeed;
        private Vector3 cameraLookAt;
        public bool isInAir;
        private Vector3 moveVector;

        private Vector3 mouseRotationBuffer;
        private MouseState currentMouseState;
        private MouseState previousMouseState;



        // Properties

        public Vector3 Position
        {
            get { return cameraPosition; }
            set
            {
                cameraPosition = value;
                UpdateLookAt();
            }
        }

        public Vector3 Rotation
        {
            get { return cameraRotation; }
            set
            {
                cameraRotation = value;
                UpdateLookAt();
            }
        }

        public Matrix Projection
        {
            get;
            protected set;
        }

        public Matrix View
        {

            get
            {
                return Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.UnitZ);
            }
        }

        //Constructor 
        public Camera(Game game, Vector3 position, Vector3 rotation, float speed)
            : base(game)
        {
            cameraSpeed = speed;

            // projection matrix
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                Game.GraphicsDevice.Viewport.AspectRatio,
                0.05f,
                1000.0f);

            // set camera positiona nd rotation
            MoveTo(position, rotation);

            previousMouseState = Mouse.GetState();
        }

        // set Camera's position and rotation
        private void MoveTo(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }

        //update the look at vector
        private void UpdateLookAt()
        {
            // build rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) * Matrix.CreateRotationZ(cameraRotation.Z);
            // Look at ofset, change of look at
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitY, rotationMatrix);
            // update our cameras look at vector
            cameraLookAt = cameraPosition + lookAtOffset;
        }

        // Simulated movement
        private Vector3 PreviewMove(Vector3 amount)
        {
            // Create rotate matrix
            Matrix rotate = Matrix.CreateRotationZ(cameraRotation.Z);
            // Create a movement vector
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            
            return cameraPosition + movement;
        }

        // Actually move the camera
        private void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);
        }

        // updat method
        public override void Update(GameTime gameTime)
        {
            // smooth mouse?
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentMouseState = Mouse.GetState();

            KeyboardState ks = Keyboard.GetState();

            // input
            moveVector.X = 0;
            moveVector.Y = 0;

            if (ks.IsKeyDown(Keys.W))
                moveVector.Y = 1;
            if (ks.IsKeyDown(Keys.S))
                moveVector.Y = -1;
            if (ks.IsKeyDown(Keys.A))
                moveVector.X = -1;
            if (ks.IsKeyDown(Keys.D))
                moveVector.X = 1;

            
            
            //normalize it
            //so that we dont move faster diagonally
            
            
            if (moveVector != Vector3.Zero)
            {

                moveVector.Normalize();
                // now smooth and speed
                moveVector.X *= dt * cameraSpeed;
                moveVector.Y *= dt * cameraSpeed;

                // move camera
                Move(moveVector);
            }
            // Handle mouse input

            float deltaX;
            float deltaZ;

            if (currentMouseState != previousMouseState)
            {
                //Cache mouse location
                deltaX = currentMouseState.X - (Game.GraphicsDevice.Viewport.Width / 2);
                deltaZ = currentMouseState.Y - (Game.GraphicsDevice.Viewport.Height / 2);

                // smooth mouse ? rotation
                mouseRotationBuffer.X += 0.1f * deltaX * dt;
                mouseRotationBuffer.Z += 0.1f * -deltaZ * dt;

                Rotation = new Vector3(mouseRotationBuffer.Z, 0, -mouseRotationBuffer.X);

                deltaX = 0;
                deltaZ = 0;

            }

            // Alt + F4 to close now.
            // Makes sure the mouse doesn't wander across the screen (might be a little buggy by showing the mouse)
            Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);

            previousMouseState = currentMouseState;

            base.Update(gameTime);
        }
        
    }
}