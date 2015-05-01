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
    public enum GameObjectType
    {
        Small_Human_Ship, Medium_Human_Ship, Large_Human_Ship, Human_Base, Small_AI_Ship, Medium_AI_Ship, Large_AI_Ship, AI_Base, Planet, Star, Asteroid
    };
}
