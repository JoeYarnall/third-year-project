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
    class LongRangeLaser : Weapon
    {
        public LongRangeLaser()
        {
            this.name = "Long Range Laser";
            this.strength = 10;
            this.range = 10;
            this.cost = 10;
            this.accuracy = 0.5f;
            this.type = WeaponType.Laser;
        }
    }
}
