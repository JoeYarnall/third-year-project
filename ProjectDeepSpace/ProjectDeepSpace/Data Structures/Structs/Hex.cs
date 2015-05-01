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
    public struct Hex
    {
        #region Variables, Getters and Setters
        private int x;
        private int y;

        private Vector3 topLeft;
        private Vector3 topRight;
        private Vector3 middleLeft;
        private Vector3 middleRight;
        private Vector3 bottomLeft;
        private Vector3 bottomRight;
        private Vector3 centre;

        public Vector3 TopLeft
        {
            get { return topLeft; }
        }
        public Vector3 TopRight
        {
            get { return topRight; }
        }
        public Vector3 MiddleLeft
        {
            get { return middleLeft; }
        }
        public Vector3 MiddleRight
        {
            get { return middleRight; }
        }
        public Vector3 BottomLeft
        {
            get { return bottomLeft; }
        }
        public Vector3 BottomRight
        {
            get { return bottomRight; }
        }
        public Vector3 Centre
        {
            get { return centre; }
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

        public Hex(Vector3 topLeft, Vector3 topRight, Vector3 middleLeft, Vector3 middleRight, Vector3 bottomLeft, Vector3 bottomRight, Vector3 centre, int x, int y)
        {  
            this.x = x;
            this.y = y;

            this.topLeft = topLeft;
            this.topRight = topRight;
            this.middleLeft = middleLeft;
            this.middleRight = middleRight;
            this.bottomLeft = bottomLeft;
            this.bottomRight = bottomRight;
            this.centre = centre;
        }
    }
}
