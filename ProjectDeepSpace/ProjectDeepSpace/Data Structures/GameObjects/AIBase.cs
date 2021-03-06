﻿using System;
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
    public class AIBase : GameObject
    {
        public AIBase(int ID, Vector3 loc, Vector2 hexLoc)
        {
            iD = ID;
            team = Player.AI;
            type = GameObjectType.AI_Base;
            state = ObjectState.Waiting;

            modelLoaded = false;

            this.hexLoc = hexLoc;
            this.loc = loc;
            this.sourceLoc = loc;
            rot = Quaternion.CreateFromRotationMatrix(RotateToFace(loc, loc - new Vector3(0, 1, 0), Vector3.UnitZ));

            maxHP = 120;
            maxAP = 70;
            ResetHP();
            ResetAP();

            myWeapon = new CustomWeapon("Quantum Singularity Cannon", 7, 5, 10, 0.8f, WeaponType.Laser);
            myEngine = new NoEngine();
        }

        public override void LoadModel(ContentManager content, BasicEffect effect)
        {
            myModel = content.Load<Model>("Models/Enemy_Station");
            modelTexture = content.Load<Texture2D>("Models/METAL7");

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
                        Matrix.CreateScale(0.002f, 0.002f, 0.002f) *
                        Matrix.CreateFromQuaternion(rot) *
                        Matrix.CreateTranslation(loc + new Vector3(-0.15f, -0.17f, 0f));

                    currentEffect.View = globalVariables.ViewMatrix;
                    currentEffect.Projection = globalVariables.ProjectionMatrix;
                    currentEffect.Texture = modelTexture;
                    currentEffect.TextureEnabled = true;
                    currentEffect.DirectionalLight0.Enabled = true;
                    currentEffect.EnableDefaultLighting();
                    currentEffect.DirectionalLight0.Direction = loc - globalVariables.LightSource;
                    currentEffect.DirectionalLight0.Direction.Normalize();
                    currentEffect.EmissiveColor = new Vector3(0.78f, 0.01f, 0.52f);
                }
                mesh.Draw();
            }
        }
    }
}
