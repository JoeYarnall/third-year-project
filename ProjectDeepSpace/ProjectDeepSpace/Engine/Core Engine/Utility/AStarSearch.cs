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
    class AStarSearch
    {
        public AStarSearch()
        {
        }

        public void AStarMoveHuman(Vector2 startLoc, Vector2 finishLoc, int range, GameBoard myGameBoard, out List<Vector2> movePathInRange, out List<Vector2> movePathOutOfRange)
        {
            movePathInRange = new List<Vector2>();
            movePathOutOfRange = new List<Vector2>();

            AStarNode current;
            List<AStarNode> openSet = new List<AStarNode>();
            List<AStarNode> closedSet = new List<AStarNode>();
            List<MapNode> map = new List<MapNode>();
            
            openSet.Add(new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, finishLoc)));
            map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc)));

            if (myGameBoard.GetGameObject(finishLoc) == null || myGameBoard.GetGameObject(finishLoc) is HumanBase || myGameBoard.GetGameObject(finishLoc) is MediumHumanShip)
            {
                #region A STAR

                while (openSet.Count > 0)
                {
                    //current = node in openset with lowest f() score
                    current = new AStarNode(startLoc, 999, CalculateHeuristic(startLoc, finishLoc));

                    foreach (AStarNode temp in openSet)
                    {
                        if (temp.fScore < current.fScore)
                        {
                            current = temp;
                        }
                    }

                    //if current = finish then break
                    if (current.loc.Equals(finishLoc))
                    {
                        break;
                    }

                    //move current from openSet to closedSet
                    closedSet.Add(current);

                    openSet.Remove(current);

                    //add neighbours to openSet and map
                    foreach (Vector2 temp in myGameBoard.GetNeighbours(myGameBoard.GetHex((int)current.loc.X, (int)current.loc.Y)))
                    {
                        if (myGameBoard.GetGameObject(temp) == null || myGameBoard.GetGameObject(temp) is HumanBase || myGameBoard.GetGameObject(temp) is MediumHumanShip)
                        {

                            Boolean existsInClosedSet = false;
                            Boolean existsInOpenSet = false;
                            Boolean isBetter = false;

                            //if temp is in closedSet break
                            foreach (AStarNode temp2 in closedSet)
                            {
                                if (temp2.loc.Equals(temp))
                                {
                                    existsInClosedSet = true;
                                    break;
                                }
                            }

                            if (existsInClosedSet == false)
                            {
                                //check if in openSet and update value if smaller
                                foreach (AStarNode temp2 in openSet)
                                {
                                    if (temp2.loc.Equals(temp))
                                    {
                                        existsInOpenSet = true;

                                        if (temp2.gScore > (current.gScore + 1))
                                        {
                                            openSet.Remove(temp2);
                                            openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, finishLoc)));
                                            isBetter = true;
                                        }

                                        break;
                                    }
                                }

                                //otherwise add to openSet
                                if (existsInOpenSet == false)
                                {
                                    openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, finishLoc)));
                                    isBetter = true;
                                }

                                //add MapNode to map
                                if (isBetter == true)
                                {
                                    foreach (MapNode temp2 in map)
                                    {
                                        if (temp2.loc.Equals(temp))
                                        {
                                            map.Remove(temp2);
                                            break;
                                        }
                                    }

                                    map.Add(new MapNode(temp, current.loc, current.gScore + 1, CalculateHeuristic(temp, finishLoc)));
                                }
                            }
                        }
                    }
                }
                #endregion A STAR

                #region TRIM MAP
                List<MapNode> movePath = new List<MapNode>();
                MapNode currentNode = new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc));

                foreach (MapNode temp in map)
                {
                    if (temp.loc.Equals(finishLoc))
                    {
                        currentNode = temp;
                    }
                }

                while (!currentNode.loc.Equals(startLoc))
                {
                    foreach (MapNode temp in map)
                    {
                        if (temp.loc.Equals(currentNode.cameFrom))
                        {
                            movePath.Add(currentNode);
                            currentNode = temp;
                            break;
                        }
                    }
                }
                #endregion TRIM MAP

                #region SORT MAP
                movePathOutOfRange = new List<Vector2>();
                movePathInRange = new List<Vector2>();

                int pathLength = movePath.Count;
                int inRange;
                int outOfRange;

                if (range > pathLength)
                {
                    inRange = pathLength;
                    outOfRange = 0;
                }
                else
                {
                    inRange = range;
                    outOfRange = pathLength - range;
                }

                foreach (MapNode temp in movePath)
                {
                    if (temp.loc.Equals(finishLoc))
                    {
                        currentNode = temp;
                    }
                }

                movePath.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc)));

                while (!currentNode.loc.Equals(startLoc))
                {
                    foreach (MapNode temp in movePath)
                    {
                        if (temp.loc.Equals(currentNode.cameFrom))
                        {
                            if (outOfRange > 0)
                            {
                                movePathOutOfRange.Add(currentNode.loc);
                                outOfRange--;
                            }
                            else if (inRange > 0)
                            {
                                movePathInRange.Add(currentNode.loc);
                                inRange--;
                            }

                            currentNode = temp;
                            break;
                        }
                    }
                }


                #endregion SORT MAP
            }
        } //DONE

        public void AStarAttack(Vector2 startLoc, int range, GameBoard myGameBoard, List<AIBase> myList, out List<Vector2> attackTargets)
        {
            attackTargets = new List<Vector2>();

            foreach (AIBase myObject in myList)
            {
                #region A STAR
                AStarNode current;
                List<AStarNode> openSet = new List<AStarNode>();
                List<AStarNode> closedSet = new List<AStarNode>();
                List<MapNode> map = new List<MapNode>();
                openSet.Add(new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc))));

                while (openSet.Count > 0)
                {
                    //current = node in openset with lowest f() score
                    current = new AStarNode(startLoc, 999, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc)));

                    foreach (AStarNode temp in openSet)
                    {
                        if (temp.fScore < current.fScore)
                        {
                            current = temp;
                        }
                    }

                    //if current = finish then break
                    if (current.loc.Equals(myGameBoard.WorldLocToHexLoc(myObject.Loc)))
                    {
                        //if current.distanceFromStart <= rangeOfWeapon add to attackTargets
                        if (current.gScore <= range)
                        {
                            attackTargets.Add(myGameBoard.WorldLocToHexLoc(myObject.Loc));
                        }

                        break;
                    }

                    //move current from openSet to closedSet
                    closedSet.Add(current);
                    openSet.Remove(current);

                    //add neighbours to openSet and map
                    foreach (Vector2 temp in myGameBoard.GetNeighbours(myGameBoard.GetHex((int)current.loc.X, (int)current.loc.Y)))
                    {

                        Boolean existsInClosedSet = false;
                        Boolean existsInOpenSet = false;
                        Boolean isBetter = false;

                        //if temp is in closedSet break
                        foreach (AStarNode temp2 in closedSet)
                        {
                            if (temp2.loc.Equals(temp))
                            {
                                existsInClosedSet = true;
                                break;
                            }
                        }

                        if (existsInClosedSet == false)
                        {
                            //check if in openSet and update value if smaller
                            foreach (AStarNode temp2 in openSet)
                            {
                                if (temp2.loc.Equals(temp))
                                {
                                    existsInOpenSet = true;

                                    if (temp2.gScore > (current.gScore + 1))
                                    {
                                        openSet.Remove(temp2);
                                        openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                                        isBetter = true;
                                    }

                                    break;
                                }
                            }

                            //otherwise add to openSet
                            if (existsInOpenSet == false)
                            {
                                openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                                isBetter = true;
                            }

                            //add MapNode to map
                            if (isBetter == true)
                            {
                                foreach (MapNode temp2 in map)
                                {
                                    if (temp2.loc.Equals(temp))
                                    {
                                        map.Remove(temp2);
                                        break;
                                    }
                                }

                                map.Add(new MapNode(temp, current.loc, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                            }
                        }
                    }
                }
                #endregion A STAR
            }
        } //DONE

        public void AStarAttack(Vector2 startLoc, int range, GameBoard myGameBoard, List<MediumAIShip> myList, out List<Vector2> attackTargets)
        {
            attackTargets = new List<Vector2>();

            foreach (MediumAIShip myObject in myList)
            {
                #region A STAR
                AStarNode current;
                List<AStarNode> openSet = new List<AStarNode>();
                List<AStarNode> closedSet = new List<AStarNode>();
                List<MapNode> map = new List<MapNode>();
                openSet.Add(new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc))));

                while (openSet.Count > 0)
                {
                    //current = node in openset with lowest f() score
                    current = new AStarNode(startLoc, 999, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc)));

                    foreach (AStarNode temp in openSet)
                    {
                        if (temp.fScore < current.fScore)
                        {
                            current = temp;
                        }
                    }

                    //if current = finish then break
                    if (current.loc.Equals(myGameBoard.WorldLocToHexLoc(myObject.Loc)))
                    {
                        //if current.distanceFromStart <= rangeOfWeapon add to attackTargets
                        if (current.gScore <= range)
                        {
                            attackTargets.Add(myGameBoard.WorldLocToHexLoc(myObject.Loc));
                        }

                        break;
                    }

                    //move current from openSet to closedSet
                    closedSet.Add(current);
                    openSet.Remove(current);

                    //add neighbours to openSet and map
                    foreach (Vector2 temp in myGameBoard.GetNeighbours(myGameBoard.GetHex((int)current.loc.X, (int)current.loc.Y)))
                    {

                        Boolean existsInClosedSet = false;
                        Boolean existsInOpenSet = false;
                        Boolean isBetter = false;

                        //if temp is in closedSet break
                        foreach (AStarNode temp2 in closedSet)
                        {
                            if (temp2.loc.Equals(temp))
                            {
                                existsInClosedSet = true;
                                break;
                            }
                        }

                        if (existsInClosedSet == false)
                        {
                            //check if in openSet and update value if smaller
                            foreach (AStarNode temp2 in openSet)
                            {
                                if (temp2.loc.Equals(temp))
                                {
                                    existsInOpenSet = true;

                                    if (temp2.gScore > (current.gScore + 1))
                                    {
                                        openSet.Remove(temp2);
                                        openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                                        isBetter = true;
                                    }

                                    break;
                                }
                            }

                            //otherwise add to openSet
                            if (existsInOpenSet == false)
                            {
                                openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                                isBetter = true;
                            }

                            //add MapNode to map
                            if (isBetter == true)
                            {
                                foreach (MapNode temp2 in map)
                                {
                                    if (temp2.loc.Equals(temp))
                                    {
                                        map.Remove(temp2);
                                        break;
                                    }
                                }

                                map.Add(new MapNode(temp, current.loc, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                            }
                        }
                    }
                }
                #endregion A STAR
            }
        } //DONE

        public void AStarAttack(Vector2 startLoc, int range, GameBoard myGameBoard, List<MediumHumanShip> myList, out List<Vector2> attackTargets)
        {
            attackTargets = new List<Vector2>();

            foreach (MediumHumanShip myObject in myList)
            {
                #region A STAR
                AStarNode current;
                List<AStarNode> openSet = new List<AStarNode>();
                List<AStarNode> closedSet = new List<AStarNode>();
                List<MapNode> map = new List<MapNode>();
                openSet.Add(new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc))));

                while (openSet.Count > 0)
                {
                    //current = node in openset with lowest f() score
                    current = new AStarNode(startLoc, 999, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc)));

                    foreach (AStarNode temp in openSet)
                    {
                        if (temp.fScore < current.fScore)
                        {
                            current = temp;
                        }
                    }

                    //if current = finish then break
                    if (current.loc.Equals(myGameBoard.WorldLocToHexLoc(myObject.Loc)))
                    {
                        //if current.distanceFromStart <= rangeOfWeapon add to attackTargets
                        if (current.gScore <= range)
                        {
                            attackTargets.Add(myGameBoard.WorldLocToHexLoc(myObject.Loc));
                        }

                        break;
                    }

                    //move current from openSet to closedSet
                    closedSet.Add(current);
                    openSet.Remove(current);

                    //add neighbours to openSet and map
                    foreach (Vector2 temp in myGameBoard.GetNeighbours(myGameBoard.GetHex((int)current.loc.X, (int)current.loc.Y)))
                    {

                        Boolean existsInClosedSet = false;
                        Boolean existsInOpenSet = false;
                        Boolean isBetter = false;

                        //if temp is in closedSet break
                        foreach (AStarNode temp2 in closedSet)
                        {
                            if (temp2.loc.Equals(temp))
                            {
                                existsInClosedSet = true;
                                break;
                            }
                        }

                        if (existsInClosedSet == false)
                        {
                            //check if in openSet and update value if smaller
                            foreach (AStarNode temp2 in openSet)
                            {
                                if (temp2.loc.Equals(temp))
                                {
                                    existsInOpenSet = true;

                                    if (temp2.gScore > (current.gScore + 1))
                                    {
                                        openSet.Remove(temp2);
                                        openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                                        isBetter = true;
                                    }

                                    break;
                                }
                            }

                            //otherwise add to openSet
                            if (existsInOpenSet == false)
                            {
                                openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                                isBetter = true;
                            }

                            //add MapNode to map
                            if (isBetter == true)
                            {
                                foreach (MapNode temp2 in map)
                                {
                                    if (temp2.loc.Equals(temp))
                                    {
                                        map.Remove(temp2);
                                        break;
                                    }
                                }

                                map.Add(new MapNode(temp, current.loc, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                            }
                        }
                    }
                }
                #endregion A STAR
            }
        } //DONE

        public void AStarAttack(Vector2 startLoc, int range, GameBoard myGameBoard, List<HumanBase> myList, out List<Vector2> attackTargets)
        {
            attackTargets = new List<Vector2>();

            foreach (HumanBase myObject in myList)
            {
                #region A STAR
                AStarNode current;
                List<AStarNode> openSet = new List<AStarNode>();
                List<AStarNode> closedSet = new List<AStarNode>();
                List<MapNode> map = new List<MapNode>();
                openSet.Add(new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc))));

                while (openSet.Count > 0)
                {
                    //current = node in openset with lowest f() score
                    current = new AStarNode(startLoc, 999, CalculateHeuristic(startLoc, myGameBoard.WorldLocToHexLoc(myObject.Loc)));

                    foreach (AStarNode temp in openSet)
                    {
                        if (temp.fScore < current.fScore)
                        {
                            current = temp;
                        }
                    }

                    //if current = finish then break
                    if (current.loc.Equals(myGameBoard.WorldLocToHexLoc(myObject.Loc)))
                    {
                        //if current.distanceFromStart <= rangeOfWeapon add to attackTargets
                        if (current.gScore <= range)
                        {
                            attackTargets.Add(myGameBoard.WorldLocToHexLoc(myObject.Loc));
                        }

                        break;
                    }

                    //move current from openSet to closedSet
                    closedSet.Add(current);
                    openSet.Remove(current);

                    //add neighbours to openSet and map
                    foreach (Vector2 temp in myGameBoard.GetNeighbours(myGameBoard.GetHex((int)current.loc.X, (int)current.loc.Y)))
                    {

                        Boolean existsInClosedSet = false;
                        Boolean existsInOpenSet = false;
                        Boolean isBetter = false;

                        //if temp is in closedSet break
                        foreach (AStarNode temp2 in closedSet)
                        {
                            if (temp2.loc.Equals(temp))
                            {
                                existsInClosedSet = true;
                                break;
                            }
                        }

                        if (existsInClosedSet == false)
                        {
                            //check if in openSet and update value if smaller
                            foreach (AStarNode temp2 in openSet)
                            {
                                if (temp2.loc.Equals(temp))
                                {
                                    existsInOpenSet = true;

                                    if (temp2.gScore > (current.gScore + 1))
                                    {
                                        openSet.Remove(temp2);
                                        openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                                        isBetter = true;
                                    }

                                    break;
                                }
                            }

                            //otherwise add to openSet
                            if (existsInOpenSet == false)
                            {
                                openSet.Add(new AStarNode(temp, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                                isBetter = true;
                            }

                            //add MapNode to map
                            if (isBetter == true)
                            {
                                foreach (MapNode temp2 in map)
                                {
                                    if (temp2.loc.Equals(temp))
                                    {
                                        map.Remove(temp2);
                                        break;
                                    }
                                }

                                map.Add(new MapNode(temp, current.loc, current.gScore + 1, CalculateHeuristic(temp, myGameBoard.WorldLocToHexLoc(myObject.Loc))));
                            }
                        }
                    }
                }
                #endregion A STAR
            }
        } //DONE

        private int CalculateHeuristic(Vector2 pos, Vector2 target)
        {
            int x = (int)(target.X - pos.X);

            if (x < 0)
                x = (int)Math.Abs(x) + 2;

            int y = (int)(target.Y - pos.Y);

            if (y < 0)
                y = (int)Math.Abs(y) + 2;

            return x + y;
        } //DONE
    }
}
