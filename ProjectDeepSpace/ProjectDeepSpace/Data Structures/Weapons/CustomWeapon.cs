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
    class CustomWeapon : Weapon
    {
        public CustomWeapon(string name, int strength, int range, int cost, float accuracy, WeaponType type)
        {
            this.name = name;
            this.strength = strength;
            this.range = range;
            this.cost = cost;
            this.accuracy = accuracy;
            this.type = type;
        }
    }
}
