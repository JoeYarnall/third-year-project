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
    public class SceneGraph
    {
        #region Variables, Getters and Setters

        private List<HumanBase> humanBases;
        private List<MediumHumanShip> humanShips;
        private List<AIBase> aiBases;
        private List<MediumAIShip> aiShips;
        private List<Planet> neutralPlanets;
        private List<Star> neutralStars;

        public List<MediumAIShip> AIShips
        {
            get { return aiShips; }
        }

        public List<AIBase> AIBases
        {
            get { return aiBases; }
        }

        #endregion Variables, Getters and Setters

        public SceneGraph()
        {
            humanBases = new List<HumanBase>();
            humanShips = new List<MediumHumanShip>();
            aiBases = new List<AIBase>();
            aiShips = new List<MediumAIShip>();
            neutralPlanets = new List<Planet>();
            neutralStars = new List<Star>();
        }

        #region Main Methods

        /**
         * A method that draws all models
         */
        public void Draw(ContentManager content, BasicEffect effect, ICameraManagerService cameraManagerService)
        {
            foreach (HumanBase myBase in humanBases)
            {
                if (myBase.ModelLoaded == false)
                {
                    myBase.LoadModel(content, effect);
                }

                myBase.DrawModel(content, effect, cameraManagerService);
            }

            foreach (AIBase myBase in aiBases)
            {
                if (myBase.ModelLoaded == false)
                {
                    myBase.LoadModel(content, effect);
                }

                myBase.DrawModel(content, effect, cameraManagerService);
            }

            foreach (MediumHumanShip myShip in humanShips)
            {
                if (myShip.ModelLoaded == false)
                {
                    myShip.LoadModel(content, effect);
                }

                myShip.DrawModel(content, effect, cameraManagerService);
            }

            foreach (MediumAIShip myShip in aiShips)
            {
                if (myShip.ModelLoaded == false)
                {
                    myShip.LoadModel(content, effect);
                }

                myShip.DrawModel(content, effect, cameraManagerService);
            }

            foreach (Star myStar in neutralStars)
            {
                if (myStar.ModelLoaded == false)
                {
                    myStar.LoadModel(content, effect);
                }

                myStar.DrawModel(content, effect, cameraManagerService);
            }

            foreach (Planet myPlanet in neutralPlanets)
            {
                if (myPlanet.ModelLoaded == false)
                {
                    myPlanet.LoadModel(content, effect);
                }

                myPlanet.DrawModel(content, effect, cameraManagerService);
            }
        } //DONE

        /**
         * A method that moves a GameObject from it's current location to a new target location.
         * The method returns true if the operation was successful and false if it wasn't.
         */
        public Boolean MoveGameObject(int ID, Vector3 targetLoc)
        {
            Boolean canBeMoved = false;

            if (IsLocationFree(targetLoc))
            {
                GetGameObject(ID).Loc = targetLoc;
                canBeMoved = true;
            }

            return canBeMoved;
        } //DONE (NO PATHING)

        /**
         * A method that creates a GameObject of a specified type at a target location. 
         * The method returns true if the operation was successful and false if it wasn't. 
         */
        public Boolean CreateGameObject(Vector3 loc, GameObjectType type, int ID)
        {
            Boolean canBeCreated = false;

            if (IsIDFree(ID) && IsLocationFree(loc))
            {
                switch (type)
                {
                    case GameObjectType.PLANET:
                        neutralPlanets.Add(new Planet(ID, loc));
                        break;

                    case GameObjectType.STAR:
                        neutralStars.Add(new Star(ID, loc));
                        break;

                    case GameObjectType.HUMANSHIP:
                        humanShips.Add(new MediumHumanShip(ID, loc));
                        break;

                    case GameObjectType.HUMANBASE:
                        humanBases.Add(new HumanBase(ID, loc));
                        break;

                    case GameObjectType.AISHIP:
                        aiShips.Add(new MediumAIShip(ID, loc));
                        break;

                    case GameObjectType.AIBASE:
                        aiBases.Add(new AIBase(ID, loc));
                        break;
                }
                canBeCreated = true;
            }

            return canBeCreated;
        } //DONE

        /**
         * A method that deletes a GameObject given it's ID.
         * The method returns true if the operation was successful and false if it wasn't.
         */
        public Boolean DeleteGameObject(int ID)
        {
            Boolean canBeDeleted = false;

            if (IDExists(ID))
            {
                foreach (HumanBase myBase in humanBases)
                {
                    if (myBase.ID == ID)
                    {
                        humanBases.Remove(myBase);
                        break;
                    }
                }

                foreach (AIBase myBase in aiBases)
                {
                    if (myBase.ID == ID)
                    {
                        aiBases.Remove(myBase);
                        break;
                    }
                }

                foreach (MediumHumanShip myShip in humanShips)
                {
                    if (myShip.ID == ID)
                    {
                        humanShips.Remove(myShip);
                        break;
                    }
                }

                foreach (MediumAIShip myShip in aiShips)
                {
                    if (myShip.ID == ID)
                    {
                        aiShips.Remove(myShip);
                        break;
                    }
                }

                foreach (Star myStar in neutralStars)
                {
                    if (myStar.ID == ID)
                    {
                        neutralStars.Remove(myStar);
                        break;
                    }   
                }

                foreach (Planet myPlanet in neutralPlanets)
                {
                    if (myPlanet.ID == ID)
                    {
                        neutralPlanets.Remove(myPlanet);
                        break;
                    }
                }

                canBeDeleted = true;
            }

            return canBeDeleted;
        } //DONE
        
        /**
         * A method that, given a GameObjectID, will find the object it is realted to or return null.
         */
        public GameObject GetGameObject(int ID)
        {
            foreach (HumanBase myBase in humanBases)
            {
                if (myBase.ID == ID)
                    return myBase;
            }

            foreach (AIBase myBase in aiBases)
            {
                if (myBase.ID == ID)
                    return myBase;
            }

            foreach (MediumHumanShip myShip in humanShips)
            {
                if (myShip.ID == ID)
                    return myShip;
            }

            foreach (MediumAIShip myShip in aiShips)
            {
                if (myShip.ID == ID)
                    return myShip;
            }

            foreach (Star myStar in neutralStars)
            {
                if (myStar.ID == ID)
                    return myStar;
            }

            foreach (Planet myPlanet in neutralPlanets)
            {
                if (myPlanet.ID == ID)
                    return myPlanet;
            }

            return null;
        } //DONE

        /**
         * A method that, given a location, will find the GameObject located there or return null. 
         */
        public GameObject GetGameObject(Vector3 loc)
        {
            foreach (HumanBase myBase in humanBases)
            {
                if (myBase.Loc.Equals(loc))
                    return myBase;
            }

            foreach (AIBase myBase in aiBases)
            {
                if (myBase.Loc.Equals(loc))
                    return myBase;
            }

            foreach (MediumHumanShip myShip in humanShips)
            {
                if (myShip.Loc.Equals(loc))
                    return myShip;
            }

            foreach (MediumAIShip myShip in aiShips)
            {
                if (myShip.Loc.Equals(loc))
                    return myShip;
            }

            foreach (Star myStar in neutralStars)
            {
                if (myStar.Loc.Equals(loc))
                    return myStar;
            }

            foreach (Planet myPlanet in neutralPlanets)
            {
                if (myPlanet.Loc.Equals(loc))
                    return myPlanet;
            }

            return null;
        } //DONE

        #endregion Main Methods

        #region Utility Methods

        /**
         * A method that checks if an ID is free.
         * The method returns true if it is free and false if it has already been taken.
         */
        public Boolean IsIDFree(int ID)
        {
            foreach (HumanBase myBase in humanBases)
            {
                if (myBase.ID == ID)
                    return false; 
            }

            foreach (AIBase myBase in aiBases)
            {
                if (myBase.ID == ID)
                    return false;
            }

            foreach (MediumHumanShip myShip in humanShips)
            {
                if (myShip.ID == ID)
                    return false;
            }

            foreach (MediumAIShip myShip in aiShips)
            {
                if (myShip.ID == ID)
                    return false;
            }

            foreach (Star myStar in neutralStars)
            {
                if (myStar.ID == ID)
                    return false;
            }

            foreach (Planet myPlanet in neutralPlanets)
            {
                if (myPlanet.ID == ID)
                    return false;
            }

            return true;
        } //DONE

        /**
         * A method that checks if a location is free.
         * The method returns true if it is free and false if it is already occupied.
         */
        public Boolean IsLocationFree(Vector3 loc)
        {
            foreach (HumanBase myBase in humanBases)
            {
                if (myBase.Loc.Equals(loc))
                    return false;
            }

            foreach (AIBase myBase in aiBases)
            {
                if (myBase.Loc.Equals(loc))
                    return false;
            }

            foreach (MediumHumanShip myShip in humanShips)
            {
                if (myShip.Loc.Equals(loc))
                    return false;
            }

            foreach (MediumAIShip myShip in aiShips)
            {
                if (myShip.Loc.Equals(loc))
                    return false;
            }

            foreach (Star myStar in neutralStars)
            {
                if (myStar.Loc.Equals(loc))
                    return false;
            }

            foreach (Planet myPlanet in neutralPlanets)
            {
                if (myPlanet.Loc.Equals(loc))
                    return false;
            }

            return true;
        } //DONE

        /**
         * A method that checks if an ID exists.
         * The method returns true if it exists and false if it is unbound.
         */
        public Boolean IDExists(int ID)
        {
            foreach (HumanBase myBase in humanBases)
            {
                if (myBase.ID == ID)
                    return true;
            }

            foreach (AIBase myBase in aiBases)
            {
                if (myBase.ID == ID)
                    return true;
            }

            foreach (MediumHumanShip myShip in humanShips)
            {
                if (myShip.ID == ID)
                    return true;
            }

            foreach (MediumAIShip myShip in aiShips)
            {
                if (myShip.ID == ID)
                    return true;
            }

            foreach (Star myStar in neutralStars)
            {
                if (myStar.ID == ID)
                    return true;
            }

            foreach (Planet myPlanet in neutralPlanets)
            {
                if (myPlanet.ID == ID)
                    return true;
            }

            return false;
        } //DONE

        #endregion Utility Methods

    }
}
