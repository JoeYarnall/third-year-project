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
    public struct MapNode
    {
        public Vector2 loc;
        public Vector2 cameFrom;
        public int gScore;
        public int hScore;
        public int fScore;

        public MapNode(Vector2 loc, Vector2 cameFrom, int gScore, int hScore)
        {
            this.loc = loc;
            this.cameFrom = cameFrom;
            this.gScore = gScore;
            this.hScore = hScore;
            this.fScore = this.gScore + this.hScore;
        }
    }
}
