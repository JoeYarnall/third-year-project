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
    public class CameraManager : Microsoft.Xna.Framework.GameComponent, ICameraManagerService
    {
        
        #region Variables, Getters and Setters

        private IKeyboardManagerService keyboardManagerService;

        private Matrix viewMatrix;
        private Matrix projectionMatrix;
        private Vector3 cameraPosition;
        private Vector3 cameraTarget;

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }
        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
            set { projectionMatrix = value; }
        }
        public Vector3 CameraPosition
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }
        public Vector3 CameraTarget
        {
            get { return cameraTarget; }
            set { cameraTarget = value; }
        }

        #endregion Variables, Getters and Setters

        public CameraManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            LoadServices();

            cameraPosition = new Vector3(0, -6f, 6f);
            cameraTarget = new Vector3(0, 0, 0);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, new Vector3(0, 0, 1));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(40), Game.GraphicsDevice.Viewport.AspectRatio, 1.0f, 200.0f);
             
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (keyboardManagerService == null)
            {
                LoadServices();
            }
            else
            {
                if (keyboardManagerService.IsCameraUpClicked())
                {
                    Vector3 x = CameraPosition;
                    CameraPosition = (x + new Vector3(0, 0.1f, 0));
                    Vector3 y = CameraTarget;
                    CameraTarget = (y + new Vector3(0, 0.1f, 0));
                }
                if (keyboardManagerService.IsCameraLeftClicked())
                {
                    Vector3 x = CameraPosition;
                    CameraPosition = (x - new Vector3(0.1f, 0, 0));
                    Vector3 y = CameraTarget;
                    CameraTarget = (y - new Vector3(0.1f, 0, 0));
                }
                if (keyboardManagerService.IsCameraDownClicked())
                {
                    Vector3 x = CameraPosition;
                    CameraPosition = (x - new Vector3(0, 0.1f, 0));
                    Vector3 y = CameraTarget;
                    CameraTarget = (y - new Vector3(0, 0.1f, 0));
                }
                if (keyboardManagerService.IsCameraRightClicked())
                {
                    Vector3 x = CameraPosition;
                    CameraPosition = (x + new Vector3(0.1f, 0, 0));
                    Vector3 y = CameraTarget;
                    CameraTarget = (y + new Vector3(0.1f, 0, 0));
                }
            }

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, new Vector3(0, 0, 1));
            
            base.Update(gameTime);
        }

        private void LoadServices()
        {
            keyboardManagerService = (IKeyboardManagerService)Game.Services.GetService(typeof(IKeyboardManagerService));
        }
    }
}
