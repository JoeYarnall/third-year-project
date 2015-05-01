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
    public struct AStarNode
    {
        public Vector2 loc;
        public int gScore;
        public int hScore;
        public int fScore;

        public AStarNode(Vector2 loc, int gScore, int hScore)
        {
            this.loc = loc;
            this.gScore = gScore;
            this.hScore = hScore;
            fScore = this.gScore + this.hScore;
        }
    }
}
