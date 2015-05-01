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
    class TaskSelector : BehaviorNode
    {
        private BehaviorNode[] nodeList;

        public TaskSelector(BehaviorNode[] nodeList)
        {
            this.nodeList = nodeList;
        }

        public override void Update(out bool bSuccess)
        {
            bSuccess = false;

            foreach (BehaviorNode n in nodeList)
            {
                n.Update(out bSuccess);
                if (bSuccess)
                {
                    break;
                }
            }
        }
    }
}
