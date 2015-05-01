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
    public abstract class Weapon
    {
        protected string name;
        protected int strength;
        protected int range;
        protected int cost;
        protected float accuracy;
        protected WeaponType type;

        public string Name
        {
            get { return name; }
        }
        public int Strength
        {
            get { return strength; }
        }
        public int Range
        {
            get { return range; }
        }
        public int Cost
        {
            get { return cost; }
        }
        public float Accuracy
        {
            get { return accuracy; }
        }
        public WeaponType Type
        {
            get { return type; }
        }
    }
}
