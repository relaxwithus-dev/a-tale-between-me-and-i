using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ATBMI.Interaction
{
    public interface IInteractDataFactory
    {
        InteractData CreateInteractData(int index, Button button);
    }
}