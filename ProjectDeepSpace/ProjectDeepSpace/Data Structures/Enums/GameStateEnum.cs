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
    public enum GameState
    {
        AITurn, HumanTurn_NoTarget_Passive, HumanTurn_Target_Passive, HumanTurn_Target_HasTarget, HumanTurn_Target_Move, HumanTurn_Target_Attack
    };
}
