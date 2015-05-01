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
    public class UIEngine
    {
        #region Variables, Getters and Setters

        GlobalVariables globalVariables;
        SpriteBatch spriteBatch;
        ContentManager content;
        Game game;

        Texture2D cursorTexture;
        Vector2 textureCentre;

        SpriteFont uiSplashBoxFont;
        SpriteFont uiCombatLogFont;
        SpriteFont uiAPBarFont;
        SpriteFont uiHPBarFont;

        SplashBox mySplashBox;
        CombatLog myCombatLog;
        UnitFrame myUnitFrame;
        TargetUnitFrame myTargetUnitFrame;
        UnitPortraitFrame myUnitPortraitFrame;
        HPBar myHPBar;
        APBar myAPBar;
        TargetHPBar myTargetHPBar;
        TargetAPBar myTargetAPBar;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        #endregion Variables, Getters and Setters

        public UIEngine(Game game, ContentManager content, ref GlobalVariables globalVariables)
        {
            this.content = content;
            this.globalVariables = globalVariables;
            this.game = game;
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            cursorTexture = content.Load<Texture2D>("UI/Cursor");
            textureCentre = new Vector2(cursorTexture.Width / 2, cursorTexture.Height / 2);

            uiSplashBoxFont = content.Load<SpriteFont>("EffectsAndFonts/UISplashBoxFont");
            uiCombatLogFont = content.Load<SpriteFont>("EffectsAndFonts/UICombatLogFont");
            uiAPBarFont = content.Load<SpriteFont>("EffectsAndFonts/UIAPBarFont");
            uiHPBarFont = content.Load<SpriteFont>("EffectsAndFonts/UIHPBarFont");

            mySplashBox = new SplashBox(game.GraphicsDevice, uiSplashBoxFont);
            myCombatLog = new CombatLog(game.GraphicsDevice, content, uiCombatLogFont);
            myUnitFrame = new UnitFrame(game.GraphicsDevice, content);
            myTargetUnitFrame = new TargetUnitFrame(game.GraphicsDevice, content);
            myTargetAPBar = new TargetAPBar(content, game.GraphicsDevice, uiHPBarFont);
            myTargetHPBar = new TargetHPBar(content, game.GraphicsDevice, uiHPBarFont);
            myUnitPortraitFrame = new UnitPortraitFrame(content, game.GraphicsDevice, uiCombatLogFont);
            myHPBar = new HPBar(content, game.GraphicsDevice, uiHPBarFont);
            myAPBar = new APBar(content, game.GraphicsDevice, uiAPBarFont);
        }

        public void Update(GameTime gameTime)
        {
            UpdateMouse();
            UpdateFPS(gameTime);

            mySplashBox.Update(gameTime.ElapsedGameTime);

            if (globalVariables.ActiveObject == null)
            {
                myUnitFrame.Active = false;
                myUnitPortraitFrame.Active = false;
                myHPBar.Active = false;
                myAPBar.Active = false;
            }
            else
            {
                myUnitFrame.Active = true;
                myUnitPortraitFrame.Active = true;
                myHPBar.Active = true;
                myAPBar.Active = true;

                myUnitPortraitFrame.Update(globalVariables.ActiveObject.Type);
                myHPBar.Update(globalVariables.ActiveObject.MaxHealthPoints, globalVariables.ActiveObject.HealthPoints);
                myAPBar.Update(globalVariables.ActiveObject.MaxActionPoints, globalVariables.ActiveObject.ActionPoints);
            }

            if (globalVariables.ActiveSecondaryObject == null)
            {
                myTargetUnitFrame.Active = false;
                myTargetAPBar.Active = false;
                myTargetHPBar.Active = false;
            }
            else
            {
                myTargetUnitFrame.Active = true;
                myTargetAPBar.Active = true;
                myTargetHPBar.Active = true;

                myTargetHPBar.Update(globalVariables.ActiveSecondaryObject.MaxHealthPoints, globalVariables.ActiveSecondaryObject.HealthPoints);
                myTargetAPBar.Update(globalVariables.ActiveSecondaryObject.MaxActionPoints, globalVariables.ActiveSecondaryObject.ActionPoints);
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            mySplashBox.Draw(spriteBatch);
            myCombatLog.Draw(spriteBatch);
            myUnitFrame.Draw(spriteBatch);
            myTargetUnitFrame.Draw(spriteBatch);
            myUnitPortraitFrame.Draw(spriteBatch);
            myHPBar.Draw(spriteBatch);
            myAPBar.Draw(spriteBatch);
            myTargetHPBar.Draw(spriteBatch);
            myTargetAPBar.Draw(spriteBatch);

            //DrawFPS();
            //DrawGameState();
            DrawMouse();

            spriteBatch.End();
        }

        public void SplashBoxNewMessage(String msg)
        {
            mySplashBox.NewMessage(msg);
        }
        public void CombatLogNewMessage(String msg)
        {
            myCombatLog.NewMessage(msg);
        }

        private void UpdateFPS(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }
        private void DrawFPS()
        {
            frameCounter++;

            string fps = string.Format("FPS: {0}", frameRate);

            spriteBatch.DrawString(uiSplashBoxFont, fps, new Vector2(5, 0), Color.DeepPink);
        }
        private void DrawGameState()
        {
            spriteBatch.DrawString(uiSplashBoxFont, globalVariables.GameState.ToString(), new Vector2(5, 20), Color.DeepPink);
        }
        private void UpdateMouse()
        {
            globalVariables.MouseLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
        private void DrawMouse()
        {
            spriteBatch.Draw(cursorTexture, globalVariables.MouseLocation, null, Color.White, 0.0f, textureCentre, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}