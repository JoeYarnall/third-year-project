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
    public class LogicEngine
    {
        #region Variables, Getters and Setters

        private Game game;
        private ContentManager content;

        private GlobalVariables globalVariables;
        private GameEngine gameEngine;
        private AIEngine aiEngine;
        private UIEngine uiEngine;
        private HelpEngine helpEngine;

        #endregion Variables, Getters and Setters

        public LogicEngine(Game game, ContentManager content, ref GlobalVariables globalVariables, ref GameEngine gameEngine, ref HelpEngine helpEngine, ref AIEngine aiEngine, ref UIEngine uiEngine)
        {
            this.game = game;
            this.content = content;

            this.globalVariables = globalVariables;
            this.gameEngine = gameEngine;
            this.aiEngine = aiEngine;
            this.uiEngine = uiEngine;
            this.helpEngine = helpEngine;
        }

        public void Update(GameTime gameTime)
        {
            if (globalVariables.GameOverState != VictoryState.NoVictor)
            {
                Tick_GameEnd(gameTime);
            }
            else if (globalVariables.GameState == GameState.AITurn)
            {
                Tick_AITurn(gameTime);
            }
            else if (globalVariables.GameState == GameState.HumanTurn_NoTarget_Passive)
            {
                Tick_HumanTurn_NoTarget_Passive(gameTime);
            }
            else if (globalVariables.GameState == GameState.HumanTurn_Target_Passive)
            {
                Tick_HumanTurn_Target_Passive(gameTime);
            }
            else if (globalVariables.GameState == GameState.HumanTurn_Target_HasTarget)
            {
                Tick_HumanTurn_Target_HasTarget(gameTime);
            }
            else if (globalVariables.GameState == GameState.HumanTurn_Target_Attack)
            {
                Tick_HumanTurn_Target_Attack(gameTime);
            }
            else if (globalVariables.GameState == GameState.HumanTurn_Target_Move)
            {
                Tick_HumanTurn_Target_Move(gameTime);
            }
            else
            {
                Tick_NoGameState(gameTime);
            }
        }

        /**************************
        * GGGGG AAAAA M    M EEEEE
        * G     A   A MM  MM E
        * G  GG AAAAA M MM M EEEE
        * G   G A   A M    M E
        * GGGGG A   A M    M EEEEE
        **************************/
        public void OnGameStart()
        {
            //gameEngine.CreateGameObject(new Vector2(5, 5), GameObjectType.Planet);
            //gameEngine.CreateGameObject(new Vector2(9, 5), GameObjectType.Star);

            //gameEngine.CreateGameObject(new Vector2(1, 3), GameObjectType.Small_Human_Ship);
            //gameEngine.CreateGameObject(new Vector2(14, 2), GameObjectType.Medium_Human_Ship);
            //gameEngine.CreateGameObject(new Vector2(4, 1), GameObjectType.Large_Human_Ship);

            //gameEngine.CreateGameObject(new Vector2(1, 9), GameObjectType.Small_AI_Ship);
            //gameEngine.CreateGameObject(new Vector2(14, 8), GameObjectType.Medium_AI_Ship);
            //gameEngine.CreateGameObject(new Vector2(10, 7), GameObjectType.Large_AI_Ship);

            gameEngine.CreateGameObject(new Vector2(0, 0), GameObjectType.Human_Base);
            gameEngine.CreateGameObject(new Vector2(39, 19), GameObjectType.AI_Base);

            #region ASTEROID GAMBIT ASTAR MOVE TEST
            gameEngine.CreateGameObject(new Vector2(6, 3), GameObjectType.Small_Human_Ship);
            
            gameEngine.CreateGameObject(new Vector2(5, 3), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(5, 4), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(5, 5), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(5, 6), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(5, 7), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(5, 8), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(5, 9), GameObjectType.Asteroid);

            gameEngine.CreateGameObject(new Vector2(7, 3), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(7, 4), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(7, 5), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(7, 6), GameObjectType.Asteroid);
            
            gameEngine.CreateGameObject(new Vector2(8, 6), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(9, 6), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(10, 6), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(11, 6), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(8, 8), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(9, 8), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(10, 8), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(11, 8), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(7, 8), GameObjectType.Asteroid);
            gameEngine.CreateGameObject(new Vector2(7, 9), GameObjectType.Asteroid);
            #endregion ASTEROID GAMBIT ASTAR MOVE TEST

            #region AI vs Humans

            gameEngine.CreateGameObject(new Vector2(17, 3), GameObjectType.Small_Human_Ship);
            gameEngine.CreateGameObject(new Vector2(18, 3), GameObjectType.Small_Human_Ship);
            gameEngine.CreateGameObject(new Vector2(19, 3), GameObjectType.Small_Human_Ship);

            gameEngine.CreateGameObject(new Vector2(12, 1), GameObjectType.Large_Human_Ship);
            gameEngine.CreateGameObject(new Vector2(12, 2), GameObjectType.Large_Human_Ship);

            gameEngine.CreateGameObject(new Vector2(17, 6), GameObjectType.Small_AI_Ship);
            gameEngine.CreateGameObject(new Vector2(18, 6), GameObjectType.Small_AI_Ship);
            gameEngine.CreateGameObject(new Vector2(19, 6), GameObjectType.Small_AI_Ship);
            gameEngine.CreateGameObject(new Vector2(17, 7), GameObjectType.Small_AI_Ship);
            gameEngine.CreateGameObject(new Vector2(18, 7), GameObjectType.Small_AI_Ship);
            gameEngine.CreateGameObject(new Vector2(19, 7), GameObjectType.Small_AI_Ship);

            #endregion AI vs Humans

        }

        public void OnGameEnd()
        {

        }

        public void OnHumanTurnEnd()
        {
            globalVariables.NewAITurn();
            aiEngine.NewTurn();
        }

        public void OnAITurnEnd()
        {
            globalVariables.NewHumanTurn();
        }

        private void CheckWinConditions(GameTime gameTime)
        {
            bool bHumanVictory = true;
            bool bAIVictory = true;

            foreach (GameObject myObj in globalVariables.SceneGraph)
            {
                if (myObj is AIBase)
                {
                    bHumanVictory = false;
                }
                else if (myObj is HumanBase)
                {
                    bAIVictory = false;
                }
            }

            if (bHumanVictory & bAIVictory) //DRAW
            {
                globalVariables.GameOverState = VictoryState.Draw;
                OnGameEnd();
            }
            else if (bHumanVictory) //HUMAN VICTORY
            {
                globalVariables.GameOverState = VictoryState.HumanVictory;
                OnGameEnd();
            }
            else if (bAIVictory) //AI VICTORY
            {
                globalVariables.GameOverState = VictoryState.AIVictory;
                OnGameEnd();
            }
            else //NO VICTOR
            {
                globalVariables.GameOverState = VictoryState.NoVictor;
            }
        }

        /*************************
        * U   U SSSSS EEEEE RRRRR
        * U   U S     E     R   R
        * U   U SSSSS EEE   RRRRR
        * U   U     S E     R RR
        * UUUUU SSSSS EEEEE R   R
        *************************/
        public void OnLMBClicked()
        {
            //HumanTurnNoTargetPassive --> HumanTurnTargetPassive || HumanTurnNoTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_NoTarget_Passive)
            {
                globalVariables.ActiveHex = globalVariables.TargetHex;
                globalVariables.ActiveObject = gameEngine.GetGameObject((Vector2)globalVariables.ActiveHex);

                if (globalVariables.ActiveObject != null)
                {
                    globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                }
                else
                {
                    globalVariables.GameState = GameState.HumanTurn_NoTarget_Passive;
                }
            }

            //HumanTurnTargetPassive --> HumanTurnTargetPassive || HumanTurnNoTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_Target_Passive)
            {
                globalVariables.ActiveHex = globalVariables.TargetHex;
                globalVariables.ActiveObject = gameEngine.GetGameObject((Vector2)globalVariables.ActiveHex);

                if (globalVariables.ActiveObject != null)
                {
                    globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                }
                else
                {
                    globalVariables.GameState = GameState.HumanTurn_NoTarget_Passive;
                }
            }

            //HumanTurnTargetHasTarget --> HumanTurnTargetPassive || HumanTurnNoTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_Target_HasTarget)
            {
                globalVariables.ActiveSecondaryHex = null;
                globalVariables.ActiveSecondaryObject = null;
                globalVariables.AttackTargets = new List<Vector2>();
                globalVariables.ActiveHex = globalVariables.TargetHex;
                globalVariables.ActiveObject = gameEngine.GetGameObject((Vector2)globalVariables.ActiveHex);

                if (globalVariables.ActiveObject != null)
                {
                    globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                }
                else
                {
                    globalVariables.GameState = GameState.HumanTurn_NoTarget_Passive;
                }
            }

            //HumanTurnTargetMove --> HumanTurnTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_Target_Move)
            {
                globalVariables.ActiveSecondaryHex = globalVariables.TargetHex;

                if (globalVariables.MovePathInRange.Contains((Vector2)globalVariables.ActiveSecondaryHex))
                {
                    if (gameEngine.MoveGameObject((GameObject)globalVariables.ActiveObject, (Vector2)globalVariables.ActiveSecondaryHex))
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                        globalVariables.ActiveHex = globalVariables.ActiveSecondaryHex;
                        globalVariables.MovePathInRange = new List<Vector2>();
                        globalVariables.MovePathOutOfRange = new List<Vector2>();
                    }
                }
                else
                {
                    uiEngine.SplashBoxNewMessage("Move Target Not In Range Or Is Invalid!");
                }

                globalVariables.ActiveSecondaryHex = null;
            }

            //HumanTurnTargetAttack --> HumanTurnTargetHasTarget
            if (globalVariables.GameState == GameState.HumanTurn_Target_Attack)
            {
                globalVariables.ActiveSecondaryHex = globalVariables.TargetHex;

                if (globalVariables.AttackTargets.Contains((Vector2)globalVariables.ActiveSecondaryHex))
                {
                    globalVariables.ActiveSecondaryObject = gameEngine.GetGameObject((Vector2)globalVariables.ActiveSecondaryHex);
                    globalVariables.AttackTargets = new List<Vector2>();
                    globalVariables.AttackTargets.Add(globalVariables.TargetHex);
                    globalVariables.GameState = GameState.HumanTurn_Target_HasTarget;
                }
                else
                {
                    uiEngine.SplashBoxNewMessage("Attack Target Not In Range Or Is Invalid!");
                }

                globalVariables.ActiveSecondaryHex = null;
            }
        } 

        public void OnRMBClicked()
        {

        }

        public void OnAttackClicked()
        {
            if (globalVariables.GameObjectState != ObjectState.Moving)
            {

                //HumanTurnTargetPassive && Team == Human && CanAttack == True --> HumanTurnTargetAttack
                if (globalVariables.GameState == GameState.HumanTurn_Target_Passive && globalVariables.ActiveObject.Team == Player.Human)
                {
                    if (globalVariables.ActiveObject.CanAttack())
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_Attack;
                        gameEngine.HumanAttack();
                    }
                    else
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                        uiEngine.SplashBoxNewMessage("This Unit Has Run Out Of Action Points and Cannot Attack!");
                    }
                }

                //HumanTurnTargetHasTarget && Team == Human && CanAttack == True --> HumanTurnTargetAttack
                if (globalVariables.GameState == GameState.HumanTurn_Target_HasTarget && globalVariables.ActiveObject.Team == Player.Human)
                {
                    if (globalVariables.ActiveObject.CanAttack())
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_Attack;
                        globalVariables.AttackTargets = new List<Vector2>();
                        globalVariables.ActiveSecondaryHex = null;
                        globalVariables.ActiveSecondaryObject = null;
                        gameEngine.HumanAttack();
                    }
                    else
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                        uiEngine.SplashBoxNewMessage("This Unit Has Run Out Of Action Points and Cannot Attack!");
                    }
                }
            }
        } 

        public void OnMoveClicked()
        {
            if (globalVariables.GameObjectState != ObjectState.Moving)
            {
                //HumanTurnTargetPassive && Team == Human && CanMove == True --> HumanTurnTargetMove
                if (globalVariables.GameState == GameState.HumanTurn_Target_Passive && globalVariables.ActiveObject.Team == Player.Human)
                {
                    if (globalVariables.ActiveObject.CanMove())
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_Move;
                        gameEngine.HumanMove();
                    }
                    else
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                        uiEngine.SplashBoxNewMessage("This Unit Has Run Out Of Action Points and Cannot Move!");
                    }
                }
            }
        }

        public void OnFireClicked()
        {
            if (globalVariables.GameState == GameState.HumanTurn_Target_HasTarget)
            {
                if (globalVariables.ActiveObject.CanAttack())
                {
                    if (gameEngine.AttackGameObject(globalVariables.ActiveObject, globalVariables.ActiveSecondaryObject))
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_HasTarget;
                    }
                }
                else
                {
                    globalVariables.GameState = GameState.HumanTurn_NoTarget_Passive;
                    globalVariables.ActiveSecondaryObject = null;
                    globalVariables.ActiveSecondaryHex = null;
                    globalVariables.AttackTargets = new List<Vector2>();
                    uiEngine.SplashBoxNewMessage("This Unit Has Run Out Of Action Points and Cannot Attack!");
                }
            }
            
        }

        public void OnCancelClicked()
        {
            //HumanTurnNoTargetPassive --> HumanTurnNoTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_NoTarget_Passive)
            {
                globalVariables.ActiveObject = null;
                globalVariables.ActiveHex = null;
                globalVariables.GameState = GameState.HumanTurn_NoTarget_Passive;
            }

            //HumanTurnTargetPassive --> HumanTurnNoTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_Target_Passive)
            {
                globalVariables.ActiveObject = null;
                globalVariables.ActiveHex = null;
                globalVariables.GameState = GameState.HumanTurn_NoTarget_Passive;
            }

            //HumanTurnTargetAttack --> HumanTurnTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_Target_Attack)
            {
                globalVariables.AttackTargets = new List<Vector2>();
                globalVariables.MovePathInRange = new List<Vector2>();
                globalVariables.MovePathOutOfRange = new List<Vector2>();
                globalVariables.GameState = GameState.HumanTurn_Target_Passive;
            }

            //HumanTurnTargetHasTarget --> HumanTurnTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_Target_HasTarget)
            {
                globalVariables.ActiveSecondaryObject = null;
                globalVariables.ActiveSecondaryHex = null;
                globalVariables.AttackTargets = new List<Vector2>();
                globalVariables.MovePathInRange = new List<Vector2>();
                globalVariables.MovePathOutOfRange = new List<Vector2>();
                globalVariables.GameState = GameState.HumanTurn_Target_Passive;
            }

            //HumanTurnTargetMove --> HumanTurnTargetPassive
            if (globalVariables.GameState == GameState.HumanTurn_Target_Move)
            {
                globalVariables.AttackTargets = new List<Vector2>();
                globalVariables.MovePathInRange = new List<Vector2>();
                globalVariables.MovePathOutOfRange = new List<Vector2>();
                globalVariables.GameState = GameState.HumanTurn_Target_Passive;
            }
        } 

        public void OnEndTurnClicked()
        {
            //HumanTurnNoTargetPassive --> AITurn
            if (globalVariables.GameObjectState == ObjectState.Waiting)
            {
                if (globalVariables.GameState == GameState.HumanTurn_NoTarget_Passive)
                {
                    globalVariables.GameState = GameState.AITurn;
                    OnHumanTurnEnd();
                }
                else if (globalVariables.GameState == GameState.HumanTurn_Target_Passive)
                {
                    globalVariables.GameState = GameState.AITurn;
                    OnHumanTurnEnd();
                }
            }
            else
            {
                uiEngine.SplashBoxNewMessage("You Can't End Your Turn At This Time");
            }
        }

        public void OnMSWIncreasing()
        {
            if (globalVariables.CameraPosition.Z >= 3)
            {
                globalVariables.CameraPosition -= new Vector3(0, -0.3f, 0.3f);
            }
        }

        public void OnMSWDecreasing()
        {
            if (globalVariables.CameraPosition.Z <= 12)
            {
                globalVariables.CameraPosition += new Vector3(0, -0.3f, 0.3f);
            }
        }

        public void OnCameraUpClicked()
        {
            float tmp = globalVariables.GetHex(new Vector2(globalVariables.X - 1, globalVariables.Y - 1)).TopRight.Y;

            if (globalVariables.CameraTarget.Y <= tmp)
            {
                globalVariables.CameraPosition += new Vector3(0, 0.15f, 0);
                globalVariables.CameraTarget += new Vector3(0, 0.15f, 0);
            }
        } 

        public void OnCameraDownClicked()
        {
            if (globalVariables.CameraTarget.Y >= 0)
            {
                globalVariables.CameraPosition -= new Vector3(0, 0.15f, 0);
                globalVariables.CameraTarget -= new Vector3(0, 0.15f, 0);
            }
        } 

        public void OnCameraLeftClicked()
        {
            if (globalVariables.CameraTarget.X >= 0)
            {
                globalVariables.CameraPosition -= new Vector3(0.15f, 0, 0);
                globalVariables.CameraTarget -= new Vector3(0.15f, 0, 0);
            }
        } 

        public void OnCameraRightClicked()
        {
            float tmp = globalVariables.GetHex(new Vector2(globalVariables.X - 1, globalVariables.Y - 1)).MiddleRight.X;

            if (globalVariables.CameraTarget.X <= tmp)
            {
                globalVariables.CameraPosition += new Vector3(0.15f, 0, 0);
                globalVariables.CameraTarget += new Vector3(0.15f, 0, 0);
            }
        }

        public void OnHelpClicked()
        {
            if (!globalVariables.BDrawHelpEngine && !globalVariables.BUpdateHelpEngine) //Help InActive
            {
                globalVariables.HelpEngineActive(); //Make Help Active
            }
            else if (globalVariables.BDrawHelpEngine && globalVariables.BUpdateHelpEngine) //Help Active
            {
                globalVariables.HelpEngineInActive(); //Make Help InActive
            }
        }

        public void OnNextClicked()
        {
            if (globalVariables.BDrawHelpEngine && globalVariables.BUpdateHelpEngine) //Help Active
            {
                helpEngine.OnNextClicked();
            }
        }

        public void OnPreviousClicked()
        {
            if (globalVariables.BDrawHelpEngine && globalVariables.BUpdateHelpEngine) //Help Active
            {
                helpEngine.OnPreviousClicked();
            }
        }

        /*******************************
        * TTTTT IIIII CCCCC K   K SSSSS 
        *   T     I   C     K KK  S     
        *   T     I   C     KK    SSSSS 
        *   T     I   C     K KK      S 
        *   T   IIIII CCCCC K   K SSSSS 
        *******************************/
        private void Tick_AITurn(GameTime gameTime)
        {
            if (globalVariables.GameObjectState != ObjectState.Moving)
            {
                //RUN AI STEP
                if (aiEngine.TurnTick())
                {
                    if (globalVariables.ActiveObject != null)
                    {
                        globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                    }
                    else
                    {
                        globalVariables.GameState = GameState.HumanTurn_NoTarget_Passive;
                    }

                    OnAITurnEnd();
                }
                else
                {
                    globalVariables.GameState = GameState.AITurn;
                }

                if (globalVariables.ActiveHex != null)
                {
                    globalVariables.ActiveObject = gameEngine.GetGameObject((Vector2)globalVariables.ActiveHex);
                }

                CheckWinConditions(gameTime);
            }
        }

        private void Tick_HumanTurn_NoTarget_Passive(GameTime gameTime)
        {
            CheckWinConditions(gameTime);
        }

        private void Tick_HumanTurn_Target_Passive(GameTime gameTime)
        {
            CheckWinConditions(gameTime);
        }

        private void Tick_HumanTurn_Target_HasTarget(GameTime gameTime)
        {
            if (globalVariables.ActiveSecondaryObject == null)
            {
                globalVariables.GameState = GameState.HumanTurn_Target_Passive;
                globalVariables.AttackTargets = new List<Vector2>();
            }

            CheckWinConditions(gameTime);
        }

        private void Tick_HumanTurn_Target_Attack(GameTime gameTime)
        {
            if (globalVariables.AttackTargets.Count == 0)
            {
                globalVariables.GameState = GameState.HumanTurn_Target_Passive;
            }

            CheckWinConditions(gameTime);
        }

        private void Tick_HumanTurn_Target_Move(GameTime gameTime)
        {
            if (globalVariables.OldTargetHex.Equals(globalVariables.TargetHex))
            {
                gameEngine.HumanMove();
            }

            CheckWinConditions(gameTime);
        }

        private void Tick_NoGameState(GameTime gameTime)
        {
            globalVariables.GameState = GameState.HumanTurn_NoTarget_Passive;
            globalVariables.ActiveHex = null;
            globalVariables.ActiveObject = null;
            globalVariables.ActiveSecondaryHex = null;
            globalVariables.ActiveSecondaryObject = null;
            globalVariables.AttackTargets = new List<Vector2>();
            globalVariables.MovePathInRange = new List<Vector2>();
            globalVariables.MovePathOutOfRange = new List<Vector2>();

            CheckWinConditions(gameTime);
        }

        private void Tick_GameEnd(GameTime gameTime)
        {

        }
    }
}