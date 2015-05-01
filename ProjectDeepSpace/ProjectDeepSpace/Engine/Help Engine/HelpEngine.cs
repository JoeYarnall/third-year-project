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
    public class HelpEngine
    {
        #region Variables, Getters and Setters

        GlobalVariables globalVariables;
        GameEngine gameEngine;
        SpriteBatch spriteBatch;
        ContentManager content;
        Game game;

        Background myBackground;

        #endregion Variables, Getters and Setters

        public HelpEngine(Game game, ContentManager content, ref GameEngine gameEngine, ref GlobalVariables globalVariables)
        {
            this.content = content;
            this.globalVariables = globalVariables;
            this.gameEngine = gameEngine;
            this.game = game;

            globalVariables.HelpSystemState = HelpState.GameHelp1;

            myBackground = new Background(game.GraphicsDevice, content);
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public void OnNextClicked()
        {
            if (globalVariables.HelpSystemState == HelpState.GameHelp1)
            {
                globalVariables.HelpSystemState = HelpState.GameHelp2;
            }
            else if (globalVariables.HelpSystemState == HelpState.GameHelp2)
            {
                globalVariables.HelpSystemState = HelpState.GameHelp3;
            }
            else if (globalVariables.HelpSystemState == HelpState.GameHelp3)
            {
                globalVariables.HelpSystemState = HelpState.GameHelp3;
            }
        }

        public void OnPreviousClicked()
        {
            if (globalVariables.HelpSystemState == HelpState.GameHelp1)
            {
                globalVariables.HelpSystemState = HelpState.GameHelp1;
            }
            else if (globalVariables.HelpSystemState == HelpState.GameHelp2)
            {
                globalVariables.HelpSystemState = HelpState.GameHelp1;
            }
            else if (globalVariables.HelpSystemState == HelpState.GameHelp3)
            {
                globalVariables.HelpSystemState = HelpState.GameHelp2;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (globalVariables.HelpSystemState == HelpState.GameHelp1)
            {
                
            }
            else if (globalVariables.HelpSystemState == HelpState.GameHelp2)
            {
                
            }
            else if (globalVariables.HelpSystemState == HelpState.GameHelp3)
            {
                
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (globalVariables.HelpSystemState == HelpState.GameHelp1)
            {
                myBackground.Draw(spriteBatch);
            }
            else if (globalVariables.HelpSystemState == HelpState.GameHelp2)
            {
                myBackground.Draw(spriteBatch);
            }
            else if (globalVariables.HelpSystemState == HelpState.GameHelp3)
            {
                myBackground.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}