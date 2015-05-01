using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectDeepSpace
{
    class SkyStar
    {
        private bool modelLoaded;
        private Model myModel;
        private Texture2D modelTexture;
        private Vector3[] locs;

        public bool ModelLoaded
        {
            get { return modelLoaded; }
        }

        public SkyStar(Vector3[] loc)
        {
            this.locs = loc;
        }

        public void LoadModel(ContentManager content, BasicEffect effect)
        {
            myModel = content.Load<Model>("Sky/SkyStar");
            modelTexture = content.Load<Texture2D>("Sky/SkyStarTexture");

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect.Clone();
                }
            }

            modelLoaded = true;
        }

        public void DrawModel(ContentManager content, BasicEffect effect, ref GlobalVariables globalVariables)
        {
            for (int i = 0; i < locs.Count(); i++)
            {
                foreach (ModelMesh mesh in myModel.Meshes)
                {
                    foreach (BasicEffect currentEffect in mesh.Effects)
                    {
                        currentEffect.World =
                            Matrix.CreateScale(0.001f, 0.001f, 0.001f) *
                            Matrix.CreateTranslation(locs[i]);

                        currentEffect.View = globalVariables.ViewMatrix;
                        currentEffect.Projection = globalVariables.ProjectionMatrix;
                        currentEffect.Texture = modelTexture;
                        currentEffect.TextureEnabled = true;
                        currentEffect.LightingEnabled = true;
                        currentEffect.AmbientLightColor = new Vector3(1f, 1f, 1f);
                    }

                    mesh.Draw();
                }
            }
        }
    }
}
