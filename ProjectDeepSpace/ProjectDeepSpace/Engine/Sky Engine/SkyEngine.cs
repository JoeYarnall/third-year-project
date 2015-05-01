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
    public class SkyEngine
    {
        #region Variables, Getters and Setters

        private GlobalVariables globalVariables;
        private ContentManager content;
        private Game game;
        private SkyStar mySkyStar;
        private BasicEffect basicEffect;
        private Random random = new Random();
        
        #endregion Variables, Getters and Setters
        
        public SkyEngine(Game game, ContentManager content, ref GlobalVariables globalVariables)
        {
            this.content = content;
            this.globalVariables = globalVariables;
            this.game = game;
            
            Vector3[] skyStars = new Vector3[150];

            for(int i = 0; i < 75; i++)
            {
                skyStars[i] = GetRandomLocation(1);
            }
            for (int i = 75; i < 150; i++)
            {
                skyStars[i] = GetRandomLocation(2);
            }

            mySkyStar = new SkyStar(skyStars);
        }

        public void LoadContent()
        {
            basicEffect = new BasicEffect(game.GraphicsDevice);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            RasterizerState stat = new RasterizerState();
            stat.CullMode = CullMode.None;
            game.GraphicsDevice.RasterizerState = stat;

            if (mySkyStar.ModelLoaded == false)
            {
                mySkyStar.LoadModel(content, basicEffect);
            }

            mySkyStar.DrawModel(content, basicEffect, ref globalVariables);

        }

        private Vector3 GetRandomLocation(int area)
        {
            double x;
            double y;
            double z;

            if (area == 1) //Area 1 z < -4 (bellow board)
            {
                if (random.NextDouble() <= 0.5)
                {
                    x = (globalVariables.X / 2) + (random.NextDouble() * globalVariables.X * 0.75);
                }
                else
                {
                    x = (globalVariables.X / 2) - (random.NextDouble() * globalVariables.X * 0.75);
                }

                if (random.NextDouble() <= 0.5)
                {
                    y = (globalVariables.Y / 2) + (random.NextDouble() * globalVariables.Y * 0.75);
                }
                else
                {
                    y = (globalVariables.Y / 2) - (random.NextDouble() * globalVariables.Y * 0.75);
                }

                z = -4f - (random.NextDouble() * 10);
            }
            else //Area 2 y < max board y + 4 (behind board)
            {
                if (random.NextDouble() <= 0.5)
                {
                    x = (globalVariables.X / 2) + (random.NextDouble() * globalVariables.X * 0.75);
                }
                else
                {
                    x = (globalVariables.X / 2) - (random.NextDouble() * globalVariables.X * 0.75);
                }

                y = globalVariables.GetHex(new Vector2(0, globalVariables.Y - 1)).TopLeft.Y + 4f + (float)(random.NextDouble() * 10);

                if (random.NextDouble() <= 0.5f)
                {
                    z = (random.NextDouble() * 15);
                }
                else
                {
                    z = -(random.NextDouble() * 15);
                }
            }

            return new Vector3((float)x, (float)y, (float)z);
        }
    }
}
