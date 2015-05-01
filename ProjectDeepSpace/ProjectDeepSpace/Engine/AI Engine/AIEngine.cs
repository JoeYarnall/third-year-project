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
    public class AIEngine
    {
        GameEngine gameEngine;
        GlobalVariables globalVariables;

        TaskSequenceLoop seqAttackInRange;
        TaskSequence seqHumanSmallMoveInRange;
        TaskSequence seqHumanMediumMoveInRange;
        TaskSequence seqHumanLargeMoveInRange;
        TaskSequence seqHumanBaseMoveInRange;
        TaskSequence seqHumanSmallMoveOutOfRange;
        TaskSequence seqHumanMediumMoveOutOfRange;
        TaskSequence seqHumanLargeMoveOutOfRange;
        TaskSequence seqHumanBaseMoveOutOfRange;

        TaskSelector selAITURN;

        GameObject[] tempObj;
        int counter;

        public AIEngine(ref GameEngine gameEngine, ref GlobalVariables globalVariables)
        {
            this.gameEngine = gameEngine;
            this.globalVariables = globalVariables;

            BehaviorNode[] tempBNode;

            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetInAttackRange(ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAAttackTarget(ref gameEngine, ref globalVariables);

            seqAttackInRange = new TaskSequenceLoop(tempBNode, new LCQueryAPForAttack(ref gameEngine, ref globalVariables));



            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetInMoveRange(GameObjectType.Human_Base, ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAMoveToTarget(ref gameEngine, ref globalVariables);

            seqHumanBaseMoveInRange = new TaskSequence(tempBNode);

            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetInMoveRange(GameObjectType.Small_Human_Ship, ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAMoveToTarget(ref gameEngine, ref globalVariables);

            seqHumanSmallMoveInRange = new TaskSequence(tempBNode);

            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetInMoveRange(GameObjectType.Medium_Human_Ship, ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAMoveToTarget(ref gameEngine, ref globalVariables);

            seqHumanMediumMoveInRange = new TaskSequence(tempBNode);

            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetInMoveRange(GameObjectType.Large_Human_Ship, ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAMoveToTarget(ref gameEngine, ref globalVariables);

            seqHumanLargeMoveInRange = new TaskSequence(tempBNode);



            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetOutOfMoveRange(GameObjectType.Human_Base, ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAMoveToTargetOutOfRange(ref gameEngine, ref globalVariables);

            seqHumanBaseMoveOutOfRange = new TaskSequence(tempBNode);

            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetOutOfMoveRange(GameObjectType.Small_Human_Ship, ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAMoveToTargetOutOfRange(ref gameEngine, ref globalVariables);

            seqHumanSmallMoveOutOfRange = new TaskSequence(tempBNode);

            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetOutOfMoveRange(GameObjectType.Medium_Human_Ship, ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAMoveToTargetOutOfRange(ref gameEngine, ref globalVariables);

            seqHumanMediumMoveOutOfRange = new TaskSequence(tempBNode);

            tempBNode = new BehaviorNode[2];
            tempBNode[0] = new LAClosestTargetOutOfMoveRange(GameObjectType.Large_Human_Ship, ref gameEngine, ref globalVariables);
            tempBNode[1] = new LAMoveToTargetOutOfRange(ref gameEngine, ref globalVariables);

            seqHumanLargeMoveOutOfRange = new TaskSequence(tempBNode);


            tempBNode = new BehaviorNode[5];
            tempBNode[0] = seqAttackInRange;
            tempBNode[1] = seqHumanBaseMoveInRange;
            tempBNode[2] = seqHumanSmallMoveInRange;
            tempBNode[3] = seqHumanMediumMoveInRange;
            tempBNode[4] = seqHumanLargeMoveInRange;

            selAITURN = new TaskSelector(tempBNode);
        }

        public void NewTurn()
        {
            counter = 0;

            foreach (GameObject myObj in globalVariables.SceneGraph)
            {
                if (myObj.Team == Player.AI)
                {
                    counter++;
                }
            }

            tempObj = new GameObject[counter];
            counter = 0;

            foreach (GameObject myObj in globalVariables.SceneGraph)
            {
                if (myObj.Team == Player.AI)
                {
                    tempObj[counter] = myObj;
                    counter++;
                }
            }

            counter = 0;
        }

        public bool TurnTick()
        {
            bool bSuccess;

            globalVariables.ActiveAITargetHex = null;
            globalVariables.ActiveAITargetObject = null;
            globalVariables.ActiveAIObject = tempObj[counter];
            selAITURN.Update(out bSuccess);
            counter++;

            if (counter >= tempObj.Count())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}