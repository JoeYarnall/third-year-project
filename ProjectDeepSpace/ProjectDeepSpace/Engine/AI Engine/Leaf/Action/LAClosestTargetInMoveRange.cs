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
    class LAClosestTargetInMoveRange : Leaf
    {
        private GameObjectType type;
        
        public LAClosestTargetInMoveRange(GameObjectType type, ref GameEngine gameEngine, ref GlobalVariables globalVariables)
        {
            this.globalVariables = globalVariables;
            this.gameEngine = gameEngine;
            this.type = type;
        }

        public override void Update(out bool bSuccess)
        {
            if (globalVariables.ActiveAIObject != null)
            {
                globalVariables.ActiveAITargetHex = gameEngine.ClosestTargetInMoveRangeOfAType(globalVariables.ActiveAIObject, type);
                bSuccess = true;

                if (globalVariables.ActiveAITargetHex.Equals(new Vector2(0, 0)))
                {
                    bSuccess = false;
                    globalVariables.ActiveAITargetHex = null;
                }
            }
            else  
            {
                bSuccess = false;
            }
        }
    }
}
