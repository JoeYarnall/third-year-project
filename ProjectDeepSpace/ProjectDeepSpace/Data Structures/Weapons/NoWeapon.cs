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
    class NoWeapon : Weapon
    {
        public NoWeapon()
        {
            this.name = "No Weapon";
            this.strength = 0;
            this.range = 0;
            this.cost = 999;
            this.accuracy = 0f;
            this.type = WeaponType.None;
        }
    }
}
