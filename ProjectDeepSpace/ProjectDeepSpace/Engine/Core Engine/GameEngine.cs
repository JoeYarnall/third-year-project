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
    public class GameEngine
    {
        #region Variables, Getters and Setters

        private GlobalVariables globalVariables;
        private UIEngine uiEngine;

        private int ID;
        private Random random = new Random();
        
        #endregion Variables, Getters and Setters

        public GameEngine(ref UIEngine uiEngine, ref GlobalVariables globalVariables)
        {
            this.globalVariables = globalVariables;
            this.uiEngine = uiEngine;

            ID = 1;
        }

        #region AI Specific Game Engine Methods

        public bool QueryHealth(GameObject myObj, float threshold)
        {
            if (myObj.HealthPoints <= (myObj.MaxHealthPoints * threshold))
                return true;
            else
                return false;
        }

        public bool QueryGameObjectTypeExists(GameObjectType type)
        {
            foreach (GameObject myObj in globalVariables.SceneGraph)
            {
                if (myObj.Type == type)
                    return true;
            }

            return false;
        }
        
        public Vector2 ClosestTargetInMoveRangeOfAType(GameObject myObj, GameObjectType type)
        {
            Vector2 loc = new Vector2(0, 0);
            int minRange = 999;
            Vector2Range target;

            foreach (GameObject tempObj in globalVariables.SceneGraph)
            {
                if (tempObj.Type == type)
                {
                    foreach( Vector2 tempVector in globalVariables.GetNeighbours(globalVariables.GetHex(tempObj.HexLoc)))
                    {
                        if (tempVector != tempObj.HexLoc && globalVariables.IsLocationFree(tempVector))
                        {
                            AStarMove(myObj.Team, myObj.HexLoc, tempVector, myObj.MoveRange(), out target);

                            if (target.range <= myObj.MoveRange() && target.range <= minRange)
                            {
                                loc = target.loc;
                                minRange = target.range;
                            }
                        }
                    }
                }
            }
            return loc;
        }

        public Vector2 ClosestTargetOutOfMoveRangeOfAType(GameObject myObj, GameObjectType type)
        {
            Vector2 loc = new Vector2(0, 0);
            int minRange = 999;
            Vector2Range target;

            foreach (GameObject tempObj in globalVariables.SceneGraph)
            {
                if (tempObj.Type == type)
                {
                    foreach (Vector2 tempVector in globalVariables.GetNeighbours(globalVariables.GetHex(tempObj.HexLoc)))
                    {
                        if (tempVector != tempObj.HexLoc && globalVariables.IsLocationFree(tempVector))
                        {
                            AStarMove(myObj.Team, myObj.HexLoc, tempVector, 999, out target);

                            if (target.range <= minRange)
                            {
                                loc = target.loc;
                                minRange = target.range;
                            }
                        }
                    }
                }
            }



            return loc;
        }

        public Vector2 FurthestTargetInMoveRangeOfAType(GameObject myObj, GameObjectType type)
        {
            Vector2 loc = new Vector2(0, 0);
            int minRange = 999;
            Vector2Range target;

            foreach (GameObject tempObj in globalVariables.SceneGraph)
            {
                if (tempObj.Type == type)
                {
                    foreach (Vector2 tempVector in globalVariables.GetNeighbours(globalVariables.GetHex(tempObj.HexLoc)))
                    {
                        if (tempVector != tempObj.HexLoc && globalVariables.IsLocationFree(tempVector))
                        {
                            AStarMove(myObj.Team, myObj.HexLoc, tempVector, myObj.MoveRange(), out target);

                            if (target.range <= myObj.MoveRange() && target.range >= minRange)
                            {
                                loc = target.loc;
                                minRange = target.range;
                            }
                        }
                    }
                }
            }
            return loc;
        }

        public GameObject ClosestTargetInAttackRange(GameObject myObj)
        {
            GameObject value = myObj;
            int minRange = 999;

            foreach (GameObject tempObj in globalVariables.SceneGraph)
            {
                if (tempObj.Team != myObj.Team && tempObj.Team != Player.Neutral)
                {
                    Vector2Range temp = AStarAttack(myObj.HexLoc, tempObj.HexLoc);

                    if (temp.range <= myObj.MyWeapon.Range && temp.range <= minRange)
                    {
                        value = tempObj;
                        minRange = temp.range;
                    }
                }
            }

            return value;
        }

        public GameObject FurthestTargetInAttackRange(GameObject myObj)
        {
            GameObject value = myObj;
            int maxRange = 0;

            foreach (GameObject tempObj in globalVariables.SceneGraph)
            {
                if (tempObj.Team != myObj.Team && tempObj.Team != Player.Neutral)
                {
                    Vector2Range temp = AStarAttack(myObj.HexLoc, tempObj.HexLoc);

                    if (temp.range <= myObj.MyWeapon.Range && temp.range >= maxRange)
                    {
                        value = tempObj;
                        maxRange = temp.range;
                    }
                }
            }

            return value;
        }

        public bool AIMoveOutOfRange(GameObject myObj, Vector2 loc)
        {
            bool canBeMoved = false;
            int distance;
            Stack<Vector3> stack;

            if (globalVariables.IsLocationFree(loc))
            {
                AStarMove(myObj.Team, myObj.HexLoc, loc, myObj.MoveRange(), out distance);
                AStarMove(myObj.Team, myObj.HexLoc, loc, myObj.MoveRange(), out stack);

                while (stack.Count > myObj.MoveRange())
                {
                    stack.Pop();
                    loc = globalVariables.WorldLocToHexLoc(stack.Peek());
                }

                uiEngine.CombatLogNewMessage(myObj.Type.ToString() + myObj.ID.ToString() + " Moved From " + myObj.HexLoc.ToString() + " To " + loc.ToString());
                myObj.HexLoc = loc;
                myObj.Move(stack);
                myObj.ActionPoints -= (myObj.MyEngine.Cost * distance);
                canBeMoved = true;
            }

            return canBeMoved;
        }

        #endregion AI Specific Game Engine Methods

        #region Human Specific Game Engine Methods

        public void HumanAttack()
        {
            foreach (GameObject myObj in globalVariables.SceneGraph)
            {
                if (myObj.Team == Player.AI)
                {
                    Vector2Range temp = AStarAttack(globalVariables.ActiveObject.HexLoc, myObj.HexLoc);

                    if (temp.range <= globalVariables.ActiveObject.MyWeapon.Range)
                    {
                        globalVariables.AttackTargets.Add(temp.loc);
                    }
                }
            }
        }

        public void HumanMove()
        {
            Vector2 startLoc = globalVariables.ActiveObject.HexLoc;
            Vector2 finishLoc = globalVariables.TargetHex;
            int range = globalVariables.ActiveObject.MoveRange();
            List<MapNode> map;

            AStarMove(globalVariables.ActiveObject.Team, startLoc, finishLoc, range, out map);

            List<MapNode> movePath = new List<MapNode>();
            MapNode currentNode = new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc));

            globalVariables.MovePathOutOfRange = new List<Vector2>();
            globalVariables.MovePathInRange = new List<Vector2>();

            int pathLength;
            int inRange;
            int outOfRange;

            #region TRIM MAP
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

            pathLength = movePath.Count;

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
                            globalVariables.MovePathOutOfRange.Add(currentNode.loc);
                            outOfRange--;
                        }
                        else if (inRange > 0)
                        {
                            globalVariables.MovePathInRange.Add(currentNode.loc);
                            inRange--;
                        }

                        currentNode = temp;
                        break;
                    }
                }
            }
            #endregion SORT MAP
        }

        #endregion Human Specific Game Engine Methods

        #region Game Engine Methods

        public bool MoveGameObject(GameObject myObj, Vector2 loc)
        {
            bool canBeMoved = false;
            int distance;
            Stack<Vector3> stack;

            if (globalVariables.IsLocationFree(loc))
            {
                AStarMove(myObj.Team, myObj.HexLoc, loc, myObj.MoveRange(), out distance);
                AStarMove(myObj.Team, myObj.HexLoc, loc, myObj.MoveRange(), out stack);
                uiEngine.CombatLogNewMessage(myObj.Type.ToString() + myObj.ID.ToString() + " Moved From " + myObj.HexLoc.ToString() + " To " + loc.ToString());
                myObj.HexLoc = loc;
                myObj.Move(stack);
                myObj.ActionPoints -= (myObj.MyEngine.Cost * distance);
                canBeMoved = true;
            }

            return canBeMoved;
        }

        public bool AttackGameObject(GameObject attacker, GameObject defender)
        {
            int damageDealt;

            damageDealt = attacker.MyWeapon.Strength;

            //Remove AP From Attacker
            attacker.ActionPoints -= attacker.MyWeapon.Cost;

            //Make Accuracy Roll
            if (GetRandomNumber() <= attacker.MyWeapon.Accuracy)
            {
                defender.HealthPoints -= damageDealt;
                uiEngine.CombatLogNewMessage(attacker.Type.ToString() + attacker.ID.ToString() + " Hit " + defender.Type.ToString() + defender.ID.ToString() + " For " + damageDealt.ToString() + " Damage");
            }
            else
            {
                uiEngine.CombatLogNewMessage(attacker.Type.ToString() + attacker.ID.ToString() + " Missed " + defender.Type.ToString() + defender.ID.ToString());
            }

            //Remove Defender if HP <= 0
            if (defender.HealthPoints <= 0)
            {
                bool tmp = DeleteGameObject(defender.ID);

                if (globalVariables.ActiveSecondaryObject != null && globalVariables.ActiveSecondaryObject.ID == defender.ID)
                {
                    globalVariables.ActiveSecondaryObject = null;
                }
                
                uiEngine.CombatLogNewMessage(defender.Type.ToString() + defender.ID.ToString() + " Was Destroyed By " + attacker.Type.ToString() + attacker.ID.ToString());
            }

            return true;
        }

        public bool CreateGameObject(Vector2 loc, GameObjectType type)
        {
            bool canBeCreated = false;

            if (globalVariables.IsLocationFree(loc))
            {
                switch (type)
                {
                    case GameObjectType.Planet:
                        globalVariables.SceneGraph.Add(new Planet(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Star:
                        globalVariables.SceneGraph.Add(new Star(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Asteroid:
                        globalVariables.SceneGraph.Add(new Asteroid(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.AI_Base:
                        globalVariables.SceneGraph.Add(new AIBase(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Human_Base:
                        globalVariables.SceneGraph.Add(new HumanBase(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Small_AI_Ship:
                        globalVariables.SceneGraph.Add(new SmallAIShip(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Small_Human_Ship:
                        globalVariables.SceneGraph.Add(new SmallHumanShip(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Medium_AI_Ship:
                        globalVariables.SceneGraph.Add(new MediumAIShip(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Medium_Human_Ship:
                        globalVariables.SceneGraph.Add(new MediumHumanShip(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Large_AI_Ship:
                        globalVariables.SceneGraph.Add(new LargeAIShip(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;

                    case GameObjectType.Large_Human_Ship:
                        globalVariables.SceneGraph.Add(new LargeHumanShip(ID++, globalVariables.HexLocToWorldLoc(loc), loc));
                        break;
                }
                canBeCreated = true;
            }

            return canBeCreated;
        }

        public bool DeleteGameObject(int ID)
        {
            bool canBeDeleted = false;

            foreach (GameObject myObj in globalVariables.SceneGraph)
            {
                if (myObj.ID == ID)
                {
                    globalVariables.SceneGraph.Remove(myObj);
                    canBeDeleted = true;
                    break;
                }
            }

            return canBeDeleted;
        }

        public GameObject GetGameObject(Vector2 loc)
        {
            foreach (GameObject myObj in globalVariables.SceneGraph)
            {
                if (myObj.HexLoc.Equals(loc))
                {
                    return myObj;
                }
            }

            return null;
        }
        
        #endregion Game Engine Methods

        #region Internal Game Engine Methods

        private Vector2Range AStarAttack(Vector2 startLoc, Vector2 finishLoc)
        {
            AStarNode current = new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, finishLoc));
            List<AStarNode> openSet = new List<AStarNode>();
            List<AStarNode> closedSet = new List<AStarNode>();
            List<MapNode> map = new List<MapNode>();

            openSet.Add(current);
            map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc)));

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
                foreach (Vector2 temp in globalVariables.GetNeighbours(globalVariables.GetHex(new Vector2((int)current.loc.X, (int)current.loc.Y))))
                {
                    bool existsInClosedSet = false;
                    bool existsInOpenSet = false;
                    bool isBetter = false;

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
            #endregion A STAR

            return new Vector2Range(current.loc, current.gScore);
        }

        private void AStarMove(Player team, Vector2 startLoc, Vector2 finishLoc, int maxRange, out Vector2Range target)
        {
            AStarNode current = new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, finishLoc));
            List<AStarNode> openSet = new List<AStarNode>();
            List<AStarNode> closedSet = new List<AStarNode>();
            List<MapNode> map = new List<MapNode>();

            openSet.Add(current);
            map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc)));

            if (GetGameObject(finishLoc) == null)
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
                    foreach (Vector2 temp in globalVariables.GetNeighbours(globalVariables.GetHex(new Vector2((int)current.loc.X, (int)current.loc.Y))))
                    {
                        if (GetGameObject(temp) == null ||GetGameObject(temp).Team == team)
                        {
                            bool existsInClosedSet = false;
                            bool existsInOpenSet = false;
                            bool isBetter = false;

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
            }

            target = new Vector2Range(current.loc, current.gScore);
        }      
        private void AStarMove(Player team, Vector2 startLoc, Vector2 finishLoc, int maxRange, out List<MapNode> map)
        {
            AStarNode current = new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, finishLoc));
            List<AStarNode> openSet = new List<AStarNode>();
            List<AStarNode> closedSet = new List<AStarNode>();
            
            map = new List<MapNode>();

            openSet.Add(current);
            map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc)));

            if (GetGameObject(finishLoc) == null)
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
                    foreach (Vector2 temp in globalVariables.GetNeighbours(globalVariables.GetHex(new Vector2((int)current.loc.X, (int)current.loc.Y))))
                    {
                        if (GetGameObject(temp) == null ||GetGameObject(temp).Team == team)
                        {
                            bool existsInClosedSet = false;
                            bool existsInOpenSet = false;
                            bool isBetter = false;

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
            }
        }
        private void AStarMove(Player team, Vector2 startLoc, Vector2 finishLoc, int maxRange, out int distance)
        {
            AStarNode current = new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, finishLoc));
            MapNode currentNode = new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc));
            List<AStarNode> openSet = new List<AStarNode>();
            List<AStarNode> closedSet = new List<AStarNode>();
            List<MapNode> map = new List<MapNode>();

            openSet.Add(current);
            map.Add(currentNode);

            if (GetGameObject(finishLoc) == null)
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
                    foreach (Vector2 temp in globalVariables.GetNeighbours(globalVariables.GetHex(new Vector2((int)current.loc.X, (int)current.loc.Y))))
                    {
                        if (GetGameObject(temp) == null || GetGameObject(temp).Team == team)
                        {
                            bool existsInClosedSet = false;
                            bool existsInOpenSet = false;
                            bool isBetter = false;

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
            }

            distance = 0;

            foreach (MapNode temp in map)
            {
                if (temp.loc.Equals(finishLoc))
                {
                    distance = temp.gScore;
                    break;
                }
            }
        }
        private void AStarMove(Player team, Vector2 startLoc, Vector2 finishLoc, int maxRange, out Stack<Vector3> stack)
        {
            AStarNode current = new AStarNode(startLoc, 0, CalculateHeuristic(startLoc, finishLoc));
            List<AStarNode> openSet = new List<AStarNode>();
            List<AStarNode> closedSet = new List<AStarNode>();
            stack = new Stack<Vector3>();
            List<MapNode> map = new List<MapNode>();

            openSet.Add(current);
            map.Add(new MapNode(startLoc, startLoc, 0, CalculateHeuristic(startLoc, finishLoc)));

            if (GetGameObject(finishLoc) == null)
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
                    foreach (Vector2 temp in globalVariables.GetNeighbours(globalVariables.GetHex(new Vector2((int)current.loc.X, (int)current.loc.Y))))
                    {
                        if (GetGameObject(temp) == null ||GetGameObject(temp).Team == team)
                        {
                            bool existsInClosedSet = false;
                            bool existsInOpenSet = false;
                            bool isBetter = false;

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
            }

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
                        stack.Push(globalVariables.HexLocToWorldLoc(currentNode.loc));
                        currentNode = temp;
                        break;
                    }
                }
            }
        }
        
        private double GetRandomNumber()
        {
            return random.NextDouble();
        }
        private int CalculateHeuristic(Vector2 pos, Vector2 target)
        {
            int x = (int)(target.X - pos.X);

            if (x < 0)
            {
                x = (int)Math.Abs(x);
            }

            int y = (int)(target.Y - pos.Y);

            if (y < 0)
            {
                y = (int)Math.Abs(y);
            }

            return (int)(Math.Floor(Math.Sqrt(x^2 + y^2)));
        }

        #endregion Internal Game Engine Methods
    }
}