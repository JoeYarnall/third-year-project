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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SkyboxManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Variables, Getters and Setters

        private ICameraManagerService cameraManagerService;

        private GameBoard myGameBoard;
        private ContentManager content;
        private Effect effect;
        private Texture2D[] skyboxTextures;
        private Model skyboxModel;
        private Vector3 centre;
        
        #endregion Variables, Getters and Setters


        public SkyboxManager(Game game, ContentManager content, ref GameBoard myGameBoard)
            : base(game)
        {
            this.content = content;
            this.myGameBoard = myGameBoard;
        }

        public override void Initialize()
        {
            LoadServices();

            centre = (Vector3)myGameBoard.GetHex((int)(myGameBoard.X / 2), (int)(myGameBoard.Y / 2)).Centre;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            effect = content.Load<Effect>("EffectsAndFonts/effects");
            skyboxModel = LoadModel("Skybox/skybox", out skyboxTextures);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead; 

            DrawSkybox();

            base.Draw(gameTime);
        }

        private Model LoadModel(string assetName, out Texture2D[] textures)
        {

            Model newModel = content.Load<Model>(assetName);
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;

            foreach (ModelMesh mesh in newModel.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    textures[i++] = currentEffect.Texture;
                }
            }

            foreach (ModelMesh mesh in newModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect.Clone();
                }
            }

            return newModel;
        }

        private void DrawSkybox()
        {
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            Game.GraphicsDevice.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            Game.GraphicsDevice.DepthStencilState = dss;

            Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            int i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * (Matrix.CreateScale(2f, 2f, 2f) * Matrix.CreateTranslation(centre));
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(cameraManagerService.ViewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(cameraManagerService.ProjectionMatrix);
                    currentEffect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            Game.GraphicsDevice.DepthStencilState = dss;
        }

        private void LoadServices()
        {
            cameraManagerService = (ICameraManagerService)Game.Services.GetService(typeof(ICameraManagerService));
        }
    }
}
