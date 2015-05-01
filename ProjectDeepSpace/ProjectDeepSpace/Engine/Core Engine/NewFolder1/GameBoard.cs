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
    public class GameBoard
    {
        #region Variables, Getters and Setters

        private SceneGraph mySceneGraph;
        private AStarSearch myAStar;

        private int x;
        private int y;
        private Hex[,] gameBoard;
        private List<Vector2> movePathInRange;
        private List<Vector2> movePathOutOfRange;
        private List<Vector2> attackTargets;

        public List<Vector2> MovePathInRange
        {
            get { return movePathInRange; }
        }

        public List<Vector2> MovePathOutOfRange
        {
            get { return movePathOutOfRange; }
        }

        public List<Vector2> AttackTargets
        {
            get { return attackTargets; }
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        #endregion Variables, Getters and Setters

        public GameBoard(int x, int y, ref SceneGraph mySceneGraph)
        {
            this.x = x;
            this.y = y;
            
            this.mySceneGraph = mySceneGraph;
            this.myAStar = new AStarSearch();

            SetupBoard();
        } //DONE

        #region Main Methods

        /**
         * A method that resets the board after a move command.
         */
        public void ResetMovePath()
        {
            movePathInRange = new List<Vector2>();
            movePathOutOfRange = new List<Vector2>();
        } //DONE

        /**
         * A method that sets up the board for an attack command.
         */
        public void ResetAttackTarget()
        {
            attackTargets = new List<Vector2>();
        } //DONE

        /**
         * A method that is used in the move command.
         */
        public void HumanMoveAStar(Vector2 startLoc, Vector2 finishLoc, int range)
        {
            myAStar.AStarMoveHuman(startLoc, finishLoc, range, this, out movePathInRange, out movePathOutOfRange);
        } //DONE

        /**
         * A recursive method that is used in the PreAttack method.
         */
        public void HumanAttackAStar(Vector2 startLoc, int range)
        {
            List<Vector2> tmp = new List<Vector2>();

            myAStar.AStarAttack(startLoc, range, this, mySceneGraph.AIBases, out attackTargets);

            myAStar.AStarAttack(startLoc, range, this, mySceneGraph.AIShips, out tmp);

            attackTargets.AddRange(tmp);

        } //DONE

        public void AITurn()
        {
            foreach (AIBase myObj in mySceneGraph.AIBases)
            {

            }

            foreach (MediumAIShip myObj in mySceneGraph.AIShips)
            {

            }
        }

        /**
         * A method that moves a game object. 
         */
        public Boolean MoveGameObject(int ID, Vector2 targetLoc)
        {
                return mySceneGraph.MoveGameObject(ID, HexLocToWorldLoc(targetLoc));
        } //DONE

        /**
         * A method that creates a game object. 
         */
        public Boolean CreateGameObject(int ID, GameObjectType type, Vector2 loc)
        {
                return mySceneGraph.CreateGameObject(HexLocToWorldLoc(loc), type, ID);
        } //DONE

        /**
         * A method that deletes a game object. 
         */
        public Boolean DeleteGameObject(int ID)
        {
                return mySceneGraph.DeleteGameObject(ID);
        } //DONE

        /**
         * A method that gets a game object using an ID. 
         */
        public GameObject GetGameObject(int ID)
        {
                return mySceneGraph.GetGameObject(ID);
        } //DONE

        /**
         * A method that gets a game object given a hex location. 
         */
        public GameObject GetGameObject(Vector2 loc)
        {
                return mySceneGraph.GetGameObject(HexLocToWorldLoc(loc));
        } //DONE

        #endregion Main Methods

        #region Utility Methods

        /**
         * A method that converts hex locations to world locations. 
         */
        public Vector3 HexLocToWorldLoc(Vector2 hexLoc)
        {
            return GetHex((int)hexLoc.X, (int)hexLoc.Y).Centre;
        } //DONE

        /**
         * A method that converts world locations to hex locations. 
         */
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

        /**
         * A method that returns the hex at a given location.
         */
        public Hex GetHex(int i, int j)
        {
            return gameBoard[i, j];
        } //DONE

        /**
         * A method that returns the hex at a given location.
         */
        public Hex GetHex(Vector2 vector)
        {
            return gameBoard[(int)vector.X, (int)vector.Y];
        } //DONE

        /**
         * A method that creates the GameBoard.
         */
        private void SetupBoard()
        {
            gameBoard = new Hex[x, y];

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
                    gameBoard[i, j] = new Hex(topLeft, topRight, middleLeft, middleRight, bottomLeft, bottomRight, centre, i, j); 
                }
            }
        } //DONE

        /**
         * A method that given a Hex will return the Hex to the North of it.
         */
        private Hex GetNorth(Hex x)
        {
            try
            {
                return GetHex(x.X + 0, x.Y + 1);
            }
            catch (IndexOutOfRangeException e)
            {
                return x;
            }
        } //DONE

        /**
         * A method that given a Hex will return the Hex to the NorthEast of it.
         */
        private Hex GetNorthEast(Hex x)
        {
            try
            {
                if (x.X % 2 != 0) //Even Column
                {
                    return GetHex(x.X + 1, x.Y + 1);
                }
                else //Odd Column
                {
                    return GetHex(x.X + 1, x.Y + 0);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                return x;
            }
        } //DONE

        /**
         * A method that given a Hex will return the Hex to the SouthEast of it.
         */
        private Hex GetSouthEast(Hex x)
        {
            try
            {
                if (x.X % 2 != 0) //Even Column
                {
                    return GetHex(x.X + 1, x.Y + 0);
                }
                else //Odd Column
                {
                    return GetHex(x.X + 1, x.Y - 1);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                return x;
            }
        } //DONE

        /**
         * A method that given a Hex will return the Hex to the South of it.
         */
        private Hex GetSouth(Hex x)
        {
            try
            {
                return GetHex(x.X + 0, x.Y - 1);
            }
            catch (IndexOutOfRangeException e)
            {
                return x;
            }
        } //DONE

        /**
         * A method that given a Hex will return the Hex to the SouthWest of it.
         */
        private Hex GetSouthWest(Hex x)
        {
            try
            {
                if (x.X % 2 != 0) //Even Column
                {
                    return GetHex(x.X - 1, x.Y + 0);
                }
                else //Odd Column
                {
                    return GetHex(x.X - 1, x.Y - 1);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                return x;
            }
        } //DONE

        /**
         * A method that given a Hex will return the Hex to the NorthWest of it.
         */
        private Hex GetNorthWest(Hex x)
        {
            try
            {
                if (x.X % 2 != 0) //Even Column
                {
                    return GetHex(x.X - 1, x.Y + 1);
                }
                else //Odd Column
                {
                    return GetHex(x.X - 1, x.Y + 0);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                return x;
            }
        } //DONE

        /**
         * A method that given a Hex will return all neighbouring Hexs.
         */
        public List<Vector2> GetNeighbours(Hex x)
        {
            List<Vector2> neighbours = new List<Vector2>();

            if (!GetNorth(x).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetNorth(x).X, (int)GetNorth(x).Y));
            }

            if (!GetNorthEast(x).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetNorthEast(x).X, (int)GetNorthEast(x).Y));
            }

            if (!GetSouthEast(x).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetSouthEast(x).X, (int)GetSouthEast(x).Y));
            }

            if (!GetSouth(x).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetSouth(x).X, (int)GetSouth(x).Y));
            }

            if (!GetSouthWest(x).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetSouthWest(x).X, (int)GetSouthWest(x).Y));
            }

            if (!GetNorthWest(x).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetNorthWest(x).X, (int)GetNorthWest(x).Y));
            }

            return neighbours;
        } //DONE

        /**
         * A method that given a Hex will return all neighbouring Hexs.
         */
        public List<Vector2> GetNeighbours(Vector2 x)
        {
            List<Vector2> neighbours = new List<Vector2>();

            if (!GetNorth(GetHex(x)).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetNorth(GetHex(x)).X, (int)GetNorth(GetHex(x)).Y));
            }

            if (!GetNorthEast(GetHex(x)).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetNorthEast(GetHex(x)).X, (int)GetNorthEast(GetHex(x)).Y));
            }

            if (!GetSouthEast(GetHex(x)).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetSouthEast(GetHex(x)).X, (int)GetSouthEast(GetHex(x)).Y));
            }

            if (!GetSouth(GetHex(x)).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetSouth(GetHex(x)).X, (int)GetSouth(GetHex(x)).Y));
            }

            if (!GetSouthWest(GetHex(x)).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetSouthWest(GetHex(x)).X, (int)GetSouthWest(GetHex(x)).Y));
            }

            if (!GetNorthWest(GetHex(x)).Equals(x))
            {
                neighbours.Add(new Vector2((int)GetNorthWest(GetHex(x)).X, (int)GetNorthWest(GetHex(x)).Y));
            }

            return neighbours;
        } //DONE

        #endregion Utility Methods
    }
}
