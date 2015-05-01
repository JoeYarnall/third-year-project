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
    class LAClosestTargetInAttackRange : Leaf
    {
        public LAClosestTargetInAttackRange(ref GameEngine gameEngine, ref GlobalVariables globalVariables)
        {
            this.globalVariables = globalVariables;
            this.gameEngine = gameEngine;
        }

        public override void Update(out bool bSuccess)
        {
            if (globalVariables.ActiveAIObject != null)
            {
                globalVariables.ActiveAITargetObject = gameEngine.ClosestTargetInAttackRange(globalVariables.ActiveAIObject);
                bSuccess = true;

                if (globalVariables.ActiveAITargetObject.Equals(globalVariables.ActiveAIObject))
                {
                    bSuccess = false;
                    globalVariables.ActiveAITargetObject = null;
                }
            }
            else
            {
                bSuccess = false;
            }
        }
    }
}
