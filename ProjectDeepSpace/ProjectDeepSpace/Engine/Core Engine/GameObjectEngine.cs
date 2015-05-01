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
    public class GameObjectEngine
    {
        #region Variables, Getters and Setters

        private GlobalVariables globalVariables;
        private ContentManager content;
        private Game game;
        
        private BasicEffect basicEffect;

        #endregion Variables, Getters and Setters

        public GameObjectEngine(Game game, ContentManager content, ref GlobalVariables globalVariables)
        {
            this.globalVariables = globalVariables;
            this.content = content;
            this.game = game;
        }

        public void LoadContent()
        {
            basicEffect = new BasicEffect(game.GraphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            globalVariables.GameObjectState = ObjectState.Waiting;

            foreach (GameObject myObject in globalVariables.SceneGraph)
            {
                myObject.Tick(gameTime);

                if (myObject.State == ObjectState.Moving)
                {
                    globalVariables.GameObjectState = ObjectState.Moving;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            RasterizerState stat = new RasterizerState();
            stat.CullMode = CullMode.None;
            game.GraphicsDevice.RasterizerState = stat;

            foreach (GameObject myObject in globalVariables.SceneGraph)
            {
                if (myObject.ModelLoaded == false)
                    myObject.LoadModel(content, basicEffect);

                myObject.DrawModel(content, basicEffect, ref globalVariables);
            }
        }
    }
}
