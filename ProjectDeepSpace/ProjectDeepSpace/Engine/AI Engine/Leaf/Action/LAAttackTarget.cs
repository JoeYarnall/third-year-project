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
    class LAAttackTarget : Leaf
    {
        public LAAttackTarget(ref GameEngine gameEngine, ref GlobalVariables globalVariables)
        {
            this.globalVariables = globalVariables;
            this.gameEngine = gameEngine;
        }

        public override void Update(out bool bSuccess)
        {
            if (globalVariables.ActiveAIObject != null || globalVariables.ActiveAITargetObject != null)
            {
                if (globalVariables.ActiveAIObject.CanAttack())
                {
                    bSuccess = gameEngine.AttackGameObject(globalVariables.ActiveAIObject, globalVariables.ActiveAITargetObject);
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
