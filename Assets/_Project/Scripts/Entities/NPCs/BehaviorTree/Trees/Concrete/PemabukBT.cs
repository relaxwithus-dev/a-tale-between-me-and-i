using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class PemabukBT : EmoTrees
    {
        protected override Node SetupTree()
        {
             // Behavior - A
            Selector itemTree = new Selector(rootName, new List<Node>
            {
                new Sequence("Task Item", new List<Node>
                {
                    new CheckTargetInArea(centerPoint, zoneDetails[2].Radius, layerMask),
                    new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true),
                    new TaskGetItem(),
                    new TaskMoveToOrigin(characterAI, characterAI.Data, isWalk: true)
                }),
                new TaskIdle(characterAI)
            });
            
            // Behavior - B
            Selector dialogueTree = new Selector(rootName, new List<Node>
            {
                new Sequence("Task Dialogue", new List<Node>
                {
                    new CheckTargetInArea(centerPoint, zoneDetails[2].Radius, layerMask),
                    new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true),
                    new Selector("Task", new List<Node>
                    {
                        new TaskGetItem(),
                        new TaskDialogue(characterAI, "Yah itemnya tidak cocok :(")
                    }),
                    new TaskMoveToOrigin(characterAI, characterAI.Data, isWalk: true)
                }),
                new TaskIdle(characterAI)
            });
            
            // Zone Selector Behavior
            Selector zoneTree = new Selector(rootName, new List<Node>
            {
                new ZoneSelector("Zone", new List<Node>
                {
                    new Sequence(zoneDetails[0].Type.ToString(), new List<Node>
                    {
                        new CheckTargetInArea(centerPoint, zoneDetails[0].Radius, layerMask),
                        new TaskDialogue(characterAI,"masuk intimate")
                    }),
                    new Sequence(zoneDetails[1].Type.ToString(), new List<Node>
                    {
                        new CheckTargetInArea(centerPoint, zoneDetails[1].Radius, layerMask),
                        new TaskDialogue(characterAI,"masuk personal")
                    }),
                }),
                new TaskIdle(characterAI)
            });
            
            return dialogueTree;
        }
    }
}