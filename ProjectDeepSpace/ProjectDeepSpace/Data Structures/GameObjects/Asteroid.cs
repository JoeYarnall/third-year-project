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
    public class Asteroid : GameObject
    {
        public Asteroid(int ID, Vector3 loc, Vector2 hexLoc)
        {
            iD = ID;
            team = Player.Neutral;
            type = GameObjectType.Asteroid;
            state = ObjectState.Waiting;

            modelLoaded = false;

            this.hexLoc = hexLoc;
            this.loc = loc;
            this.sourceLoc = loc;

            maxHP = 999;
            maxAP = 0;

            myWeapon = new NoWeapon();
            myEngine = new NoEngine();

            ResetHP();
            ResetAP();
        }

        public override void LoadModel(ContentManager content, BasicEffect effect)
        {
            myModel = content.Load<Model>("Models/Asteroid");
            modelTexture = content.Load<Texture2D>("Models/moon");

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect.Clone();
                }
            }

            modelLoaded = true;
        }

        public override void DrawModel(ContentManager content, BasicEffect effect, ref GlobalVariables globalVariables)
        {
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    currentEffect.World = 
                        Matrix.CreateScale(0.02f, 0.02f, 0.02f) *
                        Matrix.CreateTranslation(loc + new Vector3(0, 0, 0.2f));
                    
                    currentEffect.View = globalVariables.ViewMatrix;
                    currentEffect.Projection = globalVariables.ProjectionMatrix;
                    currentEffect.Texture = modelTexture;
                    currentEffect.TextureEnabled = true;
                    currentEffect.LightingEnabled = true;
                    currentEffect.DirectionalLight0.Enabled = true;
                    currentEffect.DirectionalLight0.Direction = loc - globalVariables.LightSource;
                    currentEffect.DirectionalLight0.Direction.Normalize();
                    currentEffect.AmbientLightColor = new Vector3(0.66f, 0.66f, 0.66f);
                }

                mesh.Draw();
            }
        }
    }
}
