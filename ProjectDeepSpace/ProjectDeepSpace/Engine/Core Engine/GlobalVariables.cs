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
    public class GlobalVariables
    {
        private int                 turnCounter;
        private VictoryState        gameOverState;
        private GameState           gameState;
        private HelpState           helpSystemState;
        private ObjectState         gameObjectState;

        private bool                bUpdateHelpEngine;
        private bool                bDrawHelpEngine;
        private bool                bUpdateUIEngine;
        private bool                bDrawUIEngine;
        private bool                bUpdateskyEngine;
        private bool                bDrawskyEngine;
        private bool                bUpdateGameObjectEngine;
        private bool                bDrawGameObjectEngine;
        private bool                bUpdateGameBoardEngine;
        private bool                bDrawGameBoardEngine;

        private Vector2             mouseLocation;

        private Vector3             lightSource;
        private Matrix              viewMatrix;
        private Matrix              projectionMatrix;
        private Vector3             cameraPosition;
        private Vector3             cameraTarget;
        
        private Vector2             targetHex;
        private Vector2             oldTargetHex;
        
        private GameObject          activeObject;
        private Vector2?            activeHex;
        private GameObject          activeSecondaryObject;
        private Vector2?            activeSecondaryHex;

        private GameObject          activeAIObject;
        private GameObject          activeAITargetObject;
        private Vector2?            activeAITargetHex;
               
        private List<Vector2>       movePathInRange;
        private List<Vector2>       movePathOutOfRange;
        private List<Vector2>       attackTargets;
        
        private int                 x;
        private int                 y;
        private Hex[,]              gameBoard;
        private List<GameObject>    sceneGraph;


        public int TurnCounter
        {
            get { return turnCounter; }
            set { turnCounter = value; }
        }
        public VictoryState GameOverState
        {
            get { return gameOverState; }
            set { gameOverState = value; }
        }
        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }
        public HelpState HelpSystemState
        {
            get { return helpSystemState; }
            set { helpSystemState = value; }
        }
        public ObjectState GameObjectState
        {
            get { return gameObjectState; }
            set { gameObjectState = value; }
        }

        public bool BUpdateHelpEngine
        {
            get { return bUpdateHelpEngine; }
            set { bUpdateHelpEngine = value; }
        }
        public bool BDrawHelpEngine
        {
            get { return bDrawHelpEngine; }
            set { bDrawHelpEngine = value; }
        }
        public bool BUpdateUIEngine
        {
            get { return bUpdateUIEngine; }
            set { bUpdateUIEngine = value; }
        }
        public bool BDrawUIEngine
        {
            get { return bDrawUIEngine; }
            set { bDrawUIEngine = value; }
        }
        public bool BUpdateskyEngine
        {
            get { return bUpdateskyEngine; }
            set { bUpdateskyEngine = value; }
        }
        public bool BDrawskyEngine
        {
            get { return bDrawskyEngine; }
            set { bDrawskyEngine = value; }
        }
        public bool BUpdateGameObjectEngine
        {
            get { return bUpdateGameObjectEngine; }
            set { bUpdateGameObjectEngine = value; }
        }
        public bool BDrawGameObjectEngine
        {
            get { return bDrawGameObjectEngine; }
            set { bDrawGameObjectEngine = value; }
        }
        public bool BUpdateGameBoardEngine
        {
            get { return bUpdateGameBoardEngine; }
            set { bUpdateGameBoardEngine = value; }
        }
        public bool BDrawGameBoardEngine
        {
            get { return bDrawGameBoardEngine; }
            set { bDrawGameBoardEngine = value; }
        }

        public Vector2 MouseLocation
        {
            get { return mouseLocation; }
            set { mouseLocation = value; }
        }

        public Vector3 LightSource
        {
            get { return lightSource; }
            set { lightSource = value; }
        }
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }
        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
            set { projectionMatrix = value; }
        }
        public Vector3 CameraPosition
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }
        public Vector3 CameraTarget
        {
            get { return cameraTarget; }
            set { cameraTarget = value; }
        }

        public Vector2 TargetHex
        {
            get { return targetHex; }
            set { targetHex = value; }
        }
        public Vector2 OldTargetHex
        {
            get { return oldTargetHex; }
            set { oldTargetHex = value; }
        }

        public GameObject ActiveObject
        {
            get { return activeObject; }
            set { activeObject = value; }
        }
        public Vector2? ActiveHex
        {
            get { return activeHex; }
            set { activeHex = value; }
        }
        public GameObject ActiveSecondaryObject
        {
            get { return activeSecondaryObject; }
            set { activeSecondaryObject = value; }
        }
        public Vector2? ActiveSecondaryHex
        {
            get { return activeSecondaryHex; }
            set { activeSecondaryHex = value; }
        }

        public GameObject ActiveAIObject
        {
            get { return activeAIObject; }
            set { activeAIObject = value; }
        }
        public GameObject ActiveAITargetObject
        {
            get { return activeAITargetObject; }
            set { activeAITargetObject = value; }
        }
        public Vector2? ActiveAITargetHex
        {
            get { return activeAITargetHex; }
            set { activeAITargetHex = value; }
        }

        public List<Vector2> MovePathInRange
        {
            get { return movePathInRange; }
            set { movePathInRange = value; }
        }
        public List<Vector2> MovePathOutOfRange
        {
            get { return movePathOutOfRange; }
            set { movePathOutOfRange = value; }
        }
        public List<Vector2> AttackTargets
        {
            get { return attackTargets; }
            set { attackTargets = value; }
        }

        public int X
        {
            get { return x; }
        }
        public int Y
        {
            get { return y; }
        }
        public List<GameObject> SceneGraph
        {
            get { return sceneGraph; }
        }

        public GlobalVariables(int x, int y)
        {
            this.x = x;
            this.y = y;
            SetupBoard();

            turnCounter = 0;
            gameOverState = VictoryState.NoVictor;
            gameState = GameState.HumanTurn_NoTarget_Passive;
            helpSystemState = HelpState.GameHelp1;
            gameObjectState = ObjectState.Waiting;
                  
            BDrawHelpEngine = false;
            BUpdateHelpEngine = false;
            BDrawGameBoardEngine = true;
            BDrawGameObjectEngine = true;
            BDrawskyEngine = true;
            BDrawUIEngine = true;
            BUpdateGameBoardEngine = true;
            BUpdateGameObjectEngine = true;
            BUpdateskyEngine = true;
            BUpdateUIEngine = true;

            lightSource = (Vector3)GetHex(new Vector2((int)(X / 2), (int)(Y / 2))).Centre + new Vector3(0, 0, 6);
            CameraPosition = new Vector3(0, -2f, 4f);
            CameraTarget = new Vector3(0, 0, 0);
            
            targetHex = new Vector2(0, 0);
            oldTargetHex = new Vector2(0, 0);
            activeObject = null;
            activeHex = null;
            activeSecondaryObject = null;
            activeSecondaryHex = null;

            activeAIObject = null;
            activeAITargetObject = null;
            activeAITargetHex = null;

            movePathInRange = new List<Vector2>();
            movePathOutOfRange = new List<Vector2>();
            attackTargets = new List<Vector2>();

            sceneGraph = new List<GameObject>();
        }

        private void SetupBoard()
        {
            gameBoard = new Hex[x, y];

            Hex tmp;

            Vector3 topLeft;
            Vector3 topRight;
            Vector3 middleLeft;
            Vector3 middleRight;
            Vector3 bottomLeft;
            Vector3 bottomRight;
            Vector3 centre;

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    if (i % 2 != 0) //Even Column
                    {
                        topLeft = new Vector3((0.25f + (0.75f * i)), (1.299f + (0.866f * j)), 0);
                        topRight = new Vector3((0.75f + (0.75f * i)), (1.299f + (0.866f * j)), 0);
                        middleLeft = new Vector3((0f + (0.75f * i)), (0.866f + (0.866f * j)), 0);
                        middleRight = new Vector3((1.0f + (0.75f * i)), (0.866f + (0.866f * j)), 0);
                        bottomLeft = new Vector3((0.25f + (0.75f * i)), (0.433f + (0.866f * j)), 0);
                        bottomRight = new Vector3((0.75f + (0.75f * i)), (0.433f + (0.866f * j)), 0);
                        centre = new Vector3((0.5f + (0.75f * i)), (0.866f + (0.866f * j)), 0);
                    }
                    else //Odd Column
                    {
                        topLeft = new Vector3((0.25f + (0.75f * i)), (0.866f + (0.866f * j)), 0);
                        topRight = new Vector3((0.75f + (0.75f * i)), (0.866f + (0.866f * j)), 0);
                        middleLeft = new Vector3((0f + (0.75f * i)), (0.433f + (0.866f * j)), 0);
                        middleRight = new Vector3((1.0f + (0.75f * i)), (0.433f + (0.866f * j)), 0);
                        bottomLeft = new Vector3((0.25f + (0.75f * i)), (0f + (0.866f * j)), 0);
                        bottomRight = new Vector3((0.75f + (0.75f * i)), (0f + (0.866f * j)), 0);
                        centre = new Vector3((0.5f + (0.75f * i)), (0.433f + (0.866f * j)), 0);
                    }

                    tmp = new Hex(topLeft, topRight, middleLeft, middleRight, bottomLeft, bottomRight, centre, i, j);

                    gameBoard[i, j] = tmp;
                }
            }
        }

        public void HelpEngineActive()
        {
            BDrawHelpEngine = true;
            BUpdateHelpEngine = true;

            BDrawGameBoardEngine = false;
            BDrawGameObjectEngine = false;
            BDrawskyEngine = false;
            BDrawUIEngine = false;
            BUpdateGameBoardEngine = false;
            BUpdateGameObjectEngine = false;
            BUpdateskyEngine = false;
            BUpdateUIEngine = false;

            helpSystemState = ProjectDeepSpace.HelpState.GameHelp1;
        }

        public void HelpEngineInActive()
        {
            BDrawHelpEngine = false;
            BUpdateHelpEngine = false;

            BDrawGameBoardEngine = true;
            BDrawGameObjectEngine = true;
            BDrawskyEngine = true;
            BDrawUIEngine = true;
            BUpdateGameBoardEngine = true;
            BUpdateGameObjectEngine = true;
            BUpdateskyEngine = true;
            BUpdateUIEngine = true;
        }

        public Hex GetHex(Vector2 vector)
        {
            return gameBoard[(int)vector.X, (int)vector.Y];
        }
        public Hex GetNorth(Hex x)
        {   
            try
            {
                if (x.Y == Y - 1)
                    return x;
                else
                    return GetHex(new Vector2(x.X + 0, x.Y + 1));
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace);
                throw new ArgumentException();
            }
        }
        public Hex GetNorthEast(Hex x)
        {
            try
            {
                if (x.X % 2 != 0) //Even Column
                {
                    if (x.X == X - 1 || x.Y == Y - 1)
                        return x;
                    else
                        return GetHex(new Vector2(x.X + 1, x.Y + 1));
                }
                else //Odd Column
                {
                    if (x.X == X - 1)
                        return x;
                    else 
                        return GetHex(new Vector2(x.X + 1, x.Y + 0));
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace);
                throw new ArgumentException();
            }
        }
        public Hex GetSouthEast(Hex x)
        {
            try
            {
                if (x.X % 2 != 0) //Even Column
                {
                    if (x.X == X - 1)
                        return x;
                    else
                        return GetHex(new Vector2(x.X + 1, x.Y + 0));
                }
                else //Odd Column
                {
                    if (x.X == X - 1 || x.Y == 0)
                        return x;
                    else
                        return GetHex(new Vector2(x.X + 1, x.Y - 1));
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace);
                throw new ArgumentException();
            }
        }
        public Hex GetSouth(Hex x)
        {
            try
            {
                if (x.Y == 0)
                    return x;
                else
                    return GetHex(new Vector2(x.X + 0, x.Y - 1));
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace);
                throw new ArgumentException();
            }
        }
        public Hex GetSouthWest(Hex x)
        {
            try
            {
                if (x.X % 2 != 0) //Even Column
                {
                    if (x.X == 0)
                        return x;
                    else 
                        return GetHex(new Vector2(x.X - 1, x.Y + 0));
                }
                else //Odd Column
                {
                    if (x.X == 0 || x.Y == 0)
                        return x;
                    else 
                        return GetHex(new Vector2(x.X - 1, x.Y - 1));
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace);
                throw new ArgumentException();
            }
        }
        public Hex GetNorthWest(Hex x)
        {
            try
            {
                if (x.X % 2 != 0) //Even Column
                {
                    if (x.X == 0 || x.Y == Y - 1)
                        return x;
                    else 
                        return GetHex(new Vector2(x.X - 1, x.Y + 1));
                }
                else //Odd Column
                {
                    if (x.X == 0)
                        return x;
                    else 
                        return GetHex(new Vector2(x.X - 1, x.Y + 0));
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace);
                throw new ArgumentException();
            }
        }
        public List<Vector2> GetNeighbours(Hex x)
        {
            List<Vector2> neighbours = new List<Vector2>();

            Hex tmpNorth;
            Hex tmpNorthEast;
            Hex tmpSouthEast;
            Hex tmpSouth;
            Hex tmpSouthWest;
            Hex tmpNorthWest;

            try
            {
                tmpNorth = GetNorth(x);
                
                if (!tmpNorth.Equals(x))
                    neighbours.Add(new Vector2((int)tmpNorth.X, (int)tmpNorth.Y));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            try
            {
                tmpNorthEast = GetNorthEast(x);

                if (!tmpNorthEast.Equals(x))
                    neighbours.Add(new Vector2((int)tmpNorthEast.X, (int)tmpNorthEast.Y));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            try
            {
                tmpSouthEast = GetSouthEast(x);

                if (!tmpSouthEast.Equals(x))
                    neighbours.Add(new Vector2((int)tmpSouthEast.X, (int)tmpSouthEast.Y));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            try
            {
                tmpSouth = GetSouth(x);

                if (!tmpSouth.Equals(x))
                    neighbours.Add(new Vector2((int)tmpSouth.X, (int)tmpSouth.Y));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            try
            {
                tmpSouthWest = GetSouthWest(x);

                if (!tmpSouthWest.Equals(x))
                    neighbours.Add(new Vector2((int)tmpSouthWest.X, (int)tmpSouthWest.Y));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            try
            {
                tmpNorthWest = GetNorthWest(x);

                if (!tmpNorthWest.Equals(x))
                    neighbours.Add(new Vector2((int)tmpNorthWest.X, (int)tmpNorthWest.Y));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return neighbours;
        }

        public Vector3 HexLocToWorldLoc(Vector2 hexLoc)
        {
            return GetHex(new Vector2((int)hexLoc.X, (int)hexLoc.Y)).Centre;
        }
        public Vector2 WorldLocToHexLoc(Vector3 worldLoc)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (gameBoard[i, j].Centre == worldLoc)
                    {
                        return new Vector2(gameBoard[i, j].X, gameBoard[i, j].Y);
                    }
                }
            }

            return new Vector2(100, 100);
        }
 
        public bool IsIDFree(int ID)
        {
            foreach (GameObject myObj in sceneGraph)
            {
                if (myObj.ID == ID)
                    return false;
            }

            return true;
        }

        public bool IsLocationFree(Vector3 loc)
        {
            foreach (GameObject myObj in sceneGraph)
            {
                if (myObj.Loc.Equals(loc))
                    return false;
            }

            return true;
        }
        public bool IsLocationFree(Vector2 loc)
        {
            foreach (GameObject myObj in sceneGraph)
            {
                if (myObj.HexLoc.Equals(loc))
                    return false;
            }

            return true;
        }

        public bool NewHumanTurn()
        {
            foreach (GameObject myObj in sceneGraph)
            {
                if (myObj.Team == Player.Human)
                {
                    myObj.NewTurn();
                }
            }

            turnCounter++;
            
            return true;
        }
        public bool NewAITurn()
        {
            foreach (GameObject myObj in sceneGraph)
            {
                if (myObj.Team == Player.AI)
                {
                    myObj.NewTurn();
                }
            }

            return true;
        }
    }
}