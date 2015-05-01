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
    public class GameManager : Microsoft.Xna.Framework.GameComponent, IGameManagerService
    {
        #region Variables, Getters and Setters

        private ContentManager content;
        private GameBoard myGameBoard;
        private CombatCalculator myCombatCalculator;
        private AIEngine myAIEngine;

        private IKeyboardManagerService keyboardManagerService;
        private IHexManagerService hexManagerService;
        private IUIManagerService uiManagerService;

        private int ID;
        private int turnCounter;
        private Phase currentPhase;
        private Player activePlayer;
        private GameObject activeObject;
        private Vector2? activeHex;
        private Order activeOrder;
        private Boolean firstTime;

        public Vector2? ActiveHex
        {
            get { return activeHex; }
        }

        public GameObject ActiveObject
        {
            get { return activeObject; }
        }

        public Phase CurrentPhase
        {
            get { return currentPhase; }
        }

        public Player ActivePlayer
        {
            get { return activePlayer; }
        }

        public Order ActiveOrder
        {
            get { return activeOrder; }
        }

        #endregion Variables, Getters and Setters

        public GameManager(Game game, ContentManager content, ref GameBoard myGameBoard)
            : base(game)
        {
            this.myGameBoard = myGameBoard;
            myCombatCalculator = new CombatCalculator();
            this.content = content;

            currentPhase = Phase.Begin;
            activePlayer = Player.Human;
            turnCounter = 0;
            activeObject = null;
            activeOrder = Order.None;
            firstTime = true;
            ID = 0;

            #region Load AI Engine

            myAIEngine = new AIEngine();

            #endregion Load AI Engine

            #region Load Pawns

            myGameBoard.CreateGameObject(ID, GameObjectType.AISHIP, new Vector2(0, 7));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AIBASE, new Vector2(1, 8));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AISHIP, new Vector2(2, 7));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AIBASE, new Vector2(3, 8));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AISHIP, new Vector2(4, 7));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AIBASE, new Vector2(5, 8));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AISHIP, new Vector2(6, 7));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AIBASE, new Vector2(7, 8));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AISHIP, new Vector2(8, 7));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.AIBASE, new Vector2(9, 8));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANSHIP, new Vector2(1, 2));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANBASE, new Vector2(2, 1));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANSHIP, new Vector2(3, 2));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANBASE, new Vector2(4, 1));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANSHIP, new Vector2(5, 2));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANBASE, new Vector2(6, 1));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANSHIP, new Vector2(7, 2));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANBASE, new Vector2(8, 1));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANSHIP, new Vector2(9, 2));
            ID++;

            myGameBoard.CreateGameObject(ID, GameObjectType.HUMANBASE, new Vector2(10, 1));
            ID++;

            #endregion Load Pawns
        }

        public override void Initialize()
        {
            LoadServices();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (keyboardManagerService == null || hexManagerService == null)
            {
                LoadServices();
            }
            else
            {
                switch (activePlayer)
                {
                    case Player.Human:
                        HumanTurn();
                        break;

                    case Player.AI:
                        AITurn();
                        break;
                }
            }

            base.Update(gameTime);
        }

        #region Human Turn Logic

        private void HumanTurn()
        {
            switch (currentPhase)
            {
                case Phase.Begin:
                    HumanTurnBeginPhase();
                    break;

                case Phase.Main:
                    HumanTurnMainPhase();
                    break;

                case Phase.End:
                    HumanTurnEndPhase();
                    break;
            }
        }

        private void HumanTurnBeginPhase()
        {
            turnCounter++;
            //Increment Base Timers
            //Create New Ships from Bases        
            currentPhase = Phase.Main;
        }

        private void HumanTurnMainPhase()
        {
            #region NONE NONE
            if (activeObject == null && activeOrder == Order.None)
            {
                if (keyboardManagerService.IsEndTurnClicked())
                {
                    currentPhase = Phase.End;
                }
                else
                {
                    UpdateSelectionLogic();
                }
            }
            #endregion NONE NONE

            else if (activeObject != null)
            {
                #region HUMANSHIP NONE
                if (activeObject is MediumHumanShip && activeOrder == Order.None)
                {
                    if (keyboardManagerService.IsAttackClicked())
                    {
                        if (activeObject.CanAttack())
                        {
                            activeOrder = Order.Attack;
                        }
                        else
                        {
                            uiManagerService.SplashBoxNewMessage("This Unit Has Run Out Of Action Points");
                        }
                    }
                    else if (keyboardManagerService.IsMoveClicked())
                    {
                        if (activeObject.CanMove())
                        {
                            activeOrder = Order.Move;
                        }
                        else
                        {
                            uiManagerService.SplashBoxNewMessage("This Unit Has Run Out Of Action Points");
                        }
                    }
                    else
                    {
                        UpdateSelectionLogic();
                    }
                }
                #endregion HUMANSHIP NONE

                #region HUMANSHIP MOVE
                else if (activeObject is MediumHumanShip && activeOrder == Order.Move)
                {
                    if (!hexManagerService.OldTargetHex.Equals(hexManagerService.TargetHex) || firstTime)
                    {
                        myGameBoard.ResetMovePath();
                        myGameBoard.HumanMoveAStar((Vector2)ActiveHex, hexManagerService.TargetHex, activeObject.MoveRange());
                        firstTime = false;
                    }

                    if (keyboardManagerService.IsCancelClicked())
                    {
                        myGameBoard.ResetMovePath();
                        activeOrder = Order.None;
                        firstTime = true;
                    }
                    else if (!activeHex.Equals(hexManagerService.PickedHex))
                    {
                        if (myGameBoard.MovePathInRange.Contains((Vector2)hexManagerService.PickedHex) && myGameBoard.MovePathOutOfRange.Count == 0 && myGameBoard.GetGameObject((Vector2)hexManagerService.PickedHex) == null)
                        {
                            myGameBoard.MoveGameObject(activeObject.ID, (Vector2)hexManagerService.PickedHex);
                            activeObject.ActionPoints -= myGameBoard.MovePathInRange.Count * activeObject.MyEngine.Cost;
                            myGameBoard.ResetMovePath();
                            activeOrder = Order.None;
                            firstTime = true;
                        }
                        else
                        {
                            uiManagerService.SplashBoxNewMessage("Move Out of Range or Invalid");
                            myGameBoard.ResetMovePath();
                            hexManagerService.PickedHex = ActiveHex;
                            firstTime = true;
                        }
                    }
                }
                #endregion HUMANSHIP MOVE

                #region HUMANSHIP ATTACK
                else if (activeObject is MediumHumanShip && activeOrder == Order.Attack)
                {
                    if (firstTime)
                    {
                        myGameBoard.HumanAttackAStar((Vector2)ActiveHex, activeObject.MyWeapon.Range);
                        firstTime = false;
                    }


                    if (keyboardManagerService.IsCancelClicked())
                    {
                        myGameBoard.ResetAttackTarget();
                        activeOrder = Order.None;
                        firstTime = true;
                    }
                    else if (!activeHex.Equals(hexManagerService.PickedHex))
                    {
                        if (hexManagerService.PickedHex != null && myGameBoard.AttackTargets.Contains((Vector2)hexManagerService.PickedHex))
                        {
                            GameObject tmp = myGameBoard.GetGameObject((Vector2)hexManagerService.PickedHex);

                            if (myCombatCalculator.Attack(ref activeObject, ref tmp))
                            {
                                myGameBoard.DeleteGameObject(tmp.ID);
                            }

                            myGameBoard.ResetAttackTarget();
                            activeOrder = Order.None;
                            firstTime = true;
                            hexManagerService.PickedHex = activeHex;
                        }
                        else
                        {
                            uiManagerService.SplashBoxNewMessage("Target Out Of Range Or Invalid");
                            hexManagerService.PickedHex = ActiveHex;
                        }
                    }
                }
                #endregion HUMANSHIP ATTACK

                #region HUMANBASE NONE
                else if (activeObject is HumanBase && activeOrder == Order.None)
                {
                    if (keyboardManagerService.IsAttackClicked())
                    {
                        activeOrder = Order.Attack;
                    }
                    else
                    {
                        UpdateSelectionLogic();
                    }
                }
                #endregion HUMANBASE NONE

                #region HUMANBASE ATTACK
                else if (activeObject is HumanBase && activeOrder == Order.Attack)
                {
                    if (firstTime)
                    {
                        myGameBoard.HumanAttackAStar((Vector2)ActiveHex, activeObject.MyWeapon.Range);
                        firstTime = false;
                    }


                    if (keyboardManagerService.IsCancelClicked())
                    {
                        myGameBoard.ResetAttackTarget();
                        activeOrder = Order.None;
                        firstTime = true;
                    }
                    else if (!activeHex.Equals(hexManagerService.PickedHex))
                    {
                        if (hexManagerService.PickedHex != null && myGameBoard.AttackTargets.Contains((Vector2)hexManagerService.PickedHex))
                        {
                            GameObject tmp = myGameBoard.GetGameObject((Vector2)hexManagerService.PickedHex);

                            if (myCombatCalculator.Attack(ref activeObject, ref tmp))
                            {
                                myGameBoard.DeleteGameObject(tmp.ID);
                            }

                            myGameBoard.ResetAttackTarget();
                            activeOrder = Order.None;
                            firstTime = true;
                            hexManagerService.PickedHex = activeHex;
                        }
                        else
                        {
                            uiManagerService.SplashBoxNewMessage("Target Out Of Range Or Invalid");
                            hexManagerService.PickedHex = ActiveHex;
                        }
                    }
                }
                #endregion HUMANBASE ATTACK

                #region AISHIP NONE
                else if (activeObject is MediumAIShip && activeOrder == Order.None)
                {
                    UpdateSelectionLogic();
                }
                #endregion AISHIP NONE

                #region AIBASE NONE
                else if (activeObject is AIBase && activeOrder == Order.None)
                {
                    UpdateSelectionLogic();
                }
                #endregion AIBASE NONE

                #region STAR NONE
                else if (activeObject is Star && activeOrder == Order.None)
                {
                    UpdateSelectionLogic();
                }
                #endregion STAR NONE

                #region PLANET NONE
                else if (activeObject is Planet && activeOrder == Order.None)
                {
                    UpdateSelectionLogic();
                }
                #endregion PLANET NONE
            }
        }

        private void HumanTurnEndPhase() 
        {
            currentPhase = Phase.Begin;
            activePlayer = Player.AI;
        }

        #endregion Human Turn Logic

        #region AI Turn Logic

        private void AITurn()
        {
            //TODO: AITURN FLASH UP 

            myGameBoard.AITurn();

            currentPhase = Phase.Begin;
            activePlayer = Player.Human;
        }

        #endregion AI Turn Logic

        private void UpdateSelectionLogic()
        {
            activeHex = hexManagerService.PickedHex;

            if (activeHex != null)
            {
                activeObject = myGameBoard.GetGameObject((Vector2)hexManagerService.PickedHex);
            }
        }

        private void LoadServices()
        {
            keyboardManagerService = (IKeyboardManagerService)Game.Services.GetService(typeof(IKeyboardManagerService));
            hexManagerService = (IHexManagerService)Game.Services.GetService(typeof(IHexManagerService));
            uiManagerService = (IUIManagerService)Game.Services.GetService(typeof(IUIManagerService));
        }
    }
}