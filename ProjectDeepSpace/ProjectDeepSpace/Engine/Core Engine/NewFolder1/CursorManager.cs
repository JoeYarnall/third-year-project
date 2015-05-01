using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectDeepSpace
{
    public class CursorManager : Microsoft.Xna.Framework.DrawableGameComponent, ICursorManagerService
    {
        private ContentManager content;
        private SpriteBatch spriteBatch;
        private Texture2D cursorTexture;
        private Vector2 textureCenter;

        private ICameraManagerService cameraManagerService;

        private Vector2 position = new Vector2(0, 0);
        
        public Vector2 Position
        {
            get { return position;}
        }

        public CursorManager(Game game, ContentManager content)
            : base(game)
        {
            this.content = content;
        }

        public override void Initialize()
        {
            LoadServices();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            cursorTexture = content.Load<Texture2D>("UI/Cursor");
            textureCenter = new Vector2(
                cursorTexture.Width / 2, cursorTexture.Height / 2);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(cursorTexture, Position, null, Color.White, 0.0f,
                textureCenter, 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            position.X = mouseState.X;
            position.Y = mouseState.Y;

            if (cameraManagerService == null)
            {
                LoadServices();
            }
            else
            {
                if (position.Y < 0) //Up
                {
                    cameraManagerService.CameraPosition += new Vector3(0, 0.05f, 0);
                    cameraManagerService.CameraTarget += new Vector3(0, 0.05f, 0);
                }
                if (position.X < 0) //left
                {
                    cameraManagerService.CameraPosition -= new Vector3(0.05f, 0, 0);
                    cameraManagerService.CameraTarget -= new Vector3(0.05f, 0, 0);
                }

                if (position.Y > GraphicsDevice.PresentationParameters.BackBufferHeight) //Down
                {
                    cameraManagerService.CameraPosition -= new Vector3(0, 0.05f, 0);
                    cameraManagerService.CameraTarget -= new Vector3(0, 0.05f, 0);
                }
                if (position.X > GraphicsDevice.PresentationParameters.BackBufferWidth) //Right
                {
                    cameraManagerService.CameraPosition += new Vector3(0.05f, 0, 0);
                    cameraManagerService.CameraTarget += new Vector3(0.05f, 0, 0);
                }
            }

            Mouse.SetPosition((int)position.X, (int)position.Y);
            
            base.Update(gameTime);
        }

        public Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix)
        {
            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(Position, 0f);
            Vector3 farSource = new Vector3(Position, 1f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }

        public Boolean IsLMBClicked()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        private void LoadServices()
        {
            cameraManagerService = (ICameraManagerService)Game.Services.GetService(typeof(ICameraManagerService));
        }
    }
}
