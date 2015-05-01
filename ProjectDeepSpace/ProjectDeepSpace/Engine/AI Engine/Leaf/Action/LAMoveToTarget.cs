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
    class LAMoveToTarget : Leaf
    {
        public LAMoveToTarget(ref GameEngine gameEngine, ref GlobalVariables globalVariables)
        {
            this.globalVariables = globalVariables;
            this.gameEngine = gameEngine;
        }

        public override void Update(out bool bSuccess)
        {
            if (globalVariables.ActiveAIObject != null && globalVariables.ActiveAITargetHex != null)
            {
                if (globalVariables.ActiveAIObject.CanMove())
                {
                    bSuccess = gameEngine.MoveGameObject(globalVariables.ActiveAIObject, (Vector2)globalVariables.ActiveAITargetHex);
                }
                else
                {
                    bSuccess = false;
                }
            }
            else
            {
                bSuccess = false;
            }
        }
    }
}
