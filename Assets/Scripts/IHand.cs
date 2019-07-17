using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR
{
    public interface IHand
    {
        bool IsStarted { get; }

        IEnumerator Start();
        IEnumerator Reset();
    }
}
