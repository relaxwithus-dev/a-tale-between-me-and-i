using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI
{
    public enum QuestStateEnum
    {
        Can_Start,
        In_Progress,
        Can_Finish,
        Finished,
        Null
    }

    public enum QuestStepStatusEnum
    {
        Null,
        In_Progress,
        Finished
    }
}
