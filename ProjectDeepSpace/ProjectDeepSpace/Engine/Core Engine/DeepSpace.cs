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
    public class DeepSpace : Microsoft.Xna.Framework.Game
    {
        #region Variables, Getters and Setters

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;

        private GlobalVariables globalVariables;

        private PlayerInput playerInput;
        private AIEngine aiEngine;
        private GameEngine gameEngine;
        private LogicEngine logicEngine;
        private SkyEngine skyEngine;
        private GameObjectEngine gameObjectEngine;
        private GameBoardEngine gameBoardEngine;
        private UIEngine uiEngine;
        private HelpEngine helpEngine;

        private Camera camera;

        #endregion Variables, Getters and Setters

        public DeepSpace()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            device = GraphicsDevice;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            this.IsMouseVisible = false;
            graphics.ApplyChanges();
            Window.Title = "Project Deep Space";

            globalVariables = new GlobalVariables(40, 20);
            gameBoardEngine = new GameBoardEngine(this, Content, ref globalVariables);
            gameObjectEngine = new GameObjectEngine(this, Content, ref globalVariables);
            uiEngine = new UIEngine(this, Content, ref globalVariables);
            skyEngine = new SkyEngine(this, Content, ref globalVariables);
            gameEngine = new GameEngine(ref uiEngine, ref globalVariables);
            helpEngine = new HelpEngine(this, Content, ref gameEngine, ref globalVariables);
            aiEngine = new AIEngine(ref gameEngine, ref globalVariables);
            logicEngine = new LogicEngine(this, Content, ref globalVariables, ref gameEngine, ref helpEngine, ref aiEngine, ref uiEngine);
            playerInput = new PlayerInput(this, Content, ref globalVariables, ref logicEngine);
            camera = new Camera(this, Content, ref globalVariables);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameBoardEngine.LoadContent();
            gameObjectEngine.LoadContent();
            uiEngine.LoadContent();
            helpEngine.LoadContent();
            skyEngine.LoadContent();
        }
        
        protected override void BeginRun()
        {
            logicEngine.OnGameStart();

            base.BeginRun();
        }
        
        protected override void Update(GameTime gameTime)
        {
            //UPDATE PLAYER INPUT
            playerInput.Update(gameTime);
            //UPDATE GAME LOGIC
            logicEngine.Update(gameTime);

            if (globalVariables.BUpdateGameBoardEngine)
            {
                gameBoardEngine.Update(gameTime);
            }
            if (globalVariables.BUpdateGameObjectEngine)
            {
                gameObjectEngine.Update(gameTime);
            }
            if (globalVariables.BUpdateUIEngine)
            {
                uiEngine.Update(gameTime);
            }
            if (globalVariables.BUpdateskyEngine)
            {
                skyEngine.Update(gameTime);
            }
            if (globalVariables.BUpdateHelpEngine)
            {
                helpEngine.Update(gameTime);
            }
            
            //UPDATE CAMERA
            camera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (globalVariables.BDrawskyEngine)
            {
                skyEngine.Draw(gameTime);
            }
            if (globalVariables.BDrawGameBoardEngine)
            {
                gameBoardEngine.Draw(gameTime);
            }
            if (globalVariables.BDrawGameObjectEngine)
            {
                gameObjectEngine.Draw(gameTime);
            }
            if (globalVariables.BDrawUIEngine)
            {
                uiEngine.Draw(gameTime);
            }
            if (globalVariables.BDrawHelpEngine)
            {
                helpEngine.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}