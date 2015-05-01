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
    public abstract class GameObject
    {
        protected int               iD;
        protected Player            team;
        protected GameObjectType    type;
        protected ObjectState       state;

        protected bool              modelLoaded;
        protected Model             myModel;
        protected Texture2D         modelTexture;

        protected Vector2           hexLoc;
        protected Vector3           loc;
        protected Quaternion        rot;
        protected Queue<Vector3>    moveList = null;
        protected TimeSpan          timePerMove = new TimeSpan(0, 0, 0, 0, 300);
        protected TimeSpan          currentTime = new TimeSpan(0, 0, 0, 0, 1);
        protected Vector3           sourceLoc;

        protected int               hP;
        protected int               maxHP;
        protected int               aP;
        protected int               maxAP;

        protected Weapon            myWeapon;
        protected Engine            myEngine;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public  Player Team
        {
            get { return team; }
        }
        public GameObjectType Type
        {
            get { return type; }
        }
        public ObjectState State
        {
            get { return state; }
        }
        public bool ModelLoaded
        {
            get { return modelLoaded; }
        }
        public Vector2 HexLoc
        {
            get { return hexLoc; }
            set { hexLoc = value; }
        }
        public Vector3 Loc
        {
            get { return loc; }
            set { loc = value; }
        }
        public Quaternion Rot
        {
            get { return rot; }
            set { rot = value; }
        }
        public int HealthPoints
        {
            get { return hP; }
            set { hP = value; }
        }
        public int MaxHealthPoints
        {
            get { return maxHP; }
        }
        public int ActionPoints
        {
            get { return aP; }
            set { aP = value; }
        }
        public int MaxActionPoints
        {
            get { return maxAP; }
        }
        public Weapon MyWeapon
        {
            get { return myWeapon; }
        }
        public  Engine MyEngine
        {
            get { return myEngine; }
        }

        public abstract void LoadModel(ContentManager content, BasicEffect effect);
        public abstract void DrawModel(ContentManager content, BasicEffect effect, ref GlobalVariables globalVariables);
        
        public void Tick(GameTime gameTime)
        {
            if (moveList != null)
            {
                if (moveList.Count != 0) //Moves still to make
                {
                    if (Loc.Equals(moveList.Peek())) //At a Key Frame
                    {
                        currentTime -= timePerMove;
                        sourceLoc = moveList.Dequeue();
                        Loc = sourceLoc;
                    }
                    else //Not at a Key Frame
                    {
                        if (loc.Equals(sourceLoc))
                        {
                            Rot = Quaternion.CreateFromRotationMatrix(RotateToFace(Loc, moveList.Peek(), Vector3.UnitZ));
                        }

                        currentTime += gameTime.ElapsedGameTime;

                        if ((currentTime.TotalMilliseconds / timePerMove.TotalMilliseconds) > 1)
                        {
                            Loc = moveList.Peek();
                        }
                        else
                        {
                            Loc = Vector3.Lerp(sourceLoc, moveList.Peek(), (float)(currentTime.TotalMilliseconds / timePerMove.TotalMilliseconds));
                        }
                    }
                }
                else if (moveList.Count == 0) //No more moves to make
                {
                    moveList = null;
                    Loc = sourceLoc;
                    state = ObjectState.Waiting;
                }
            }
        }

        protected Matrix RotateToFace(Vector3 currentLoc, Vector3 targetLoc, Vector3 up)
        {
            Vector3 D = (currentLoc - targetLoc);
            Vector3 Right = Vector3.Cross(up, D);
            Vector3.Normalize(ref Right, out Right);
            Vector3 Backwards = Vector3.Cross(Right, up);
            Vector3.Normalize(ref Backwards, out Backwards);
            Vector3 Up = Vector3.Cross(Backwards, Right);
            Matrix rot = new Matrix(Right.X, Right.Y, Right.Z, 0, Up.X, Up.Y, Up.Z, 0, Backwards.X, Backwards.Y, Backwards.Z, 0, 0, 0, 0, 1);
            //rot = rot * Matrix.CreateRotationY(MathHelper.PiOver2);
            return rot;
        }

        public void Move(Stack<Vector3> stack)
        {
            moveList = new Queue<Vector3>();
            state = ObjectState.Moving;

            while (stack.Count != 0)
            {
                moveList.Enqueue(stack.Pop());
            }
        }

        public void ResetHP()
        {
            hP = maxHP;
        }
        public void ResetAP()
        {
            aP = maxAP;
        }

        public bool CanAttack()
        {
            if (aP >= myWeapon.Cost)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CanMove()
        {
            if (aP >= myEngine.Cost)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int MoveRange()
        {
            return (int)Math.Floor((double)(aP / myEngine.Cost));
        }

        public void NewTurn()
        {
            ResetAP();
        }

    }
}
