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
    class TaskSequenceLoop : BehaviorNode
    {
        private BehaviorNode[] nodeList;
        private BehaviorNode condition;

        public TaskSequenceLoop(BehaviorNode[] nodeList, BehaviorNode condition)
        {
            this.nodeList = nodeList;
            this.condition = condition;
        }

        public override void Update(out bool bSuccess)
        {
            bSuccess = true;

            do
            {
                foreach (BehaviorNode n in nodeList)
                {
                    n.Update(out bSuccess);
                    
                    if (!bSuccess)
                    {
                        break;
                    }
                }

                if (bSuccess)
                {
                    condition.Update(out bSuccess);
                }
            } while (bSuccess);
        }
    }
}
