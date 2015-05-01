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
    public class SmallHumanShip : GameObject
    {
        public SmallHumanShip(int ID, Vector3 loc, Vector2 hexLoc)
        {
            iD = ID;
            team = Player.Human;
            type = GameObjectType.Small_Human_Ship;
            state = ObjectState.Waiting;

            modelLoaded = false;

            this.hexLoc = hexLoc;
            this.loc = loc;
            this.sourceLoc = loc;
            rot = Quaternion.CreateFromRotationMatrix(RotateToFace(loc, loc + new Vector3(0, 1, 0), Vector3.UnitZ));
            timePerMove = new TimeSpan(0, 0, 0, 0, 300);

            maxHP = 25;
            maxAP = 25;
            ResetHP();
            ResetAP();

            myWeapon = new CustomWeapon("Micro Missile", 5, 2, 5, 0.3f, WeaponType.Missile);
            myEngine = new CustomEngine("Micro Combustion Engine", 5);
        }

        public override void LoadModel(ContentManager content, BasicEffect effect)
        {
            myModel = content.Load<Model>("Models/Player_Small");
            modelTexture = content.Load<Texture2D>("Models/GALVPLAT");

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
                        Matrix.CreateScale(0.001f, 0.001f, 0.001f) *
                        Matrix.CreateRotationX(MathHelper.Pi) *
                        Matrix.CreateRotationZ(MathHelper.Pi) *
                        Matrix.CreateFromQuaternion(rot) *
                        Matrix.CreateTranslation(loc + new Vector3(0.0f, 0.0f, 0.0f));

                    currentEffect.View = globalVariables.ViewMatrix;
                    currentEffect.Projection = globalVariables.ProjectionMatrix;
                    currentEffect.Texture = modelTexture;
                    currentEffect.TextureEnabled = true;
                    currentEffect.DirectionalLight0.Enabled = true;
                    currentEffect.EnableDefaultLighting();
                    currentEffect.DirectionalLight0.Direction =  loc - globalVariables.LightSource;
                    currentEffect.DirectionalLight0.Direction.Normalize();
                    currentEffect.EmissiveColor = new Vector3(0.27f, 0.51f, 0.71f);
                }
                mesh.Draw();
            }
        }
    }
}