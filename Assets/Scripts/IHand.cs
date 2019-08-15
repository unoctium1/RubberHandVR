using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR
{
    
    /// <summary>
    /// Interface for hand modifications
    /// </summary>
    public interface IHand
    {
        /// <summary>
        /// Returns true if the modification has been started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Returns true if the modification is over
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Label for the modification
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Target value for the modification. ie. scale factor, etc
        /// </summary>
        float Value { get; set; }

        /// <summary>
        /// Begins the modification effect
        /// </summary>
        /// <returns></returns>
        IEnumerator StartEffect();

        /// <summary>
        /// Resets the modification to its initial position
        /// </summary>
        /// <returns></returns>
        IEnumerator Reset();

    }
}
