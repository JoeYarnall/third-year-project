using System;
using System.Collections;
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
    public class ModelManager : Microsoft.Xna.Framework.DrawableGameComponent, IModelManagerService
    {
        #region Variables, Getters and Setters

        private ContentManager content;
        private ICameraManagerService cameraManagerService;
        private SceneGraph mySceneGraph;
        private BasicEffect basicEffect;

        #endregion Variables, Getters and Setters

        public ModelManager(Game game, ContentManager content, ref SceneGraph mySceneGraph)
            : base(game)
        {
            this.mySceneGraph = mySceneGraph;
            this.content = content;
        }

        public override void Initialize()
        {
            LoadServices();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            basicEffect = new BasicEffect(GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            mySceneGraph.Draw(content, basicEffect, cameraManagerService);

            base.Draw(gameTime);
        }

        private void LoadServices()
        {
            cameraManagerService = (ICameraManagerService)Game.Services.GetService(typeof(ICameraManagerService));
        } //DONE

    }
}
