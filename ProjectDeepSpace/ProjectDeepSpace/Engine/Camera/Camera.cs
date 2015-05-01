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
    public class Camera
    {
        #region Variables, Getters and Setters

        GlobalVariables globalVariables;
        ContentManager content;
        Game game;

        #endregion Variables, Getters and Setters

        public Camera(Game game, ContentManager content, ref GlobalVariables globalVariables)
        {
            this.globalVariables = globalVariables;
            this.content = content;
            this.game = game;

            globalVariables.ViewMatrix = Matrix.CreateLookAt(globalVariables.CameraPosition, globalVariables.CameraTarget, new Vector3(0, 0, 1));
            globalVariables.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(70), game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 40.0f);
        }

        public void Update(GameTime gameTime)
        {
            globalVariables.ViewMatrix = Matrix.CreateLookAt(globalVariables.CameraPosition, globalVariables.CameraTarget, new Vector3(0, 0, 1));
        }

    }
}
