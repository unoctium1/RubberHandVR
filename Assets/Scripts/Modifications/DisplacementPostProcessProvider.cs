using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR
{
    namespace ModificationExamples
    {
        /// <summary>
        /// Example of a modification that uses a PostProcessProvider - the hand slider class uses this in order to shift the hand position
        /// </summary>
        public class DisplacementPostProcessProvider : PostProcessProvider
        {

            [Header("Displacement")]

            [Range(0f, 5f)]
            public float displacement = 1f;

            /// <summary>
            /// Gets the active hand to modify from the game manager - true if the active hand is the right hand, false otherwise
            /// </summary>
            private bool IsRightHand
            {
                get
                {
                    return Core.GameManager.instance.isRightHand;
                }
            }


            public float currentDisplacement = 1.0f;

            /// <summary>
            /// Set active to trigger the effect
            /// </summary>
            public bool active = false;

            private float prevFrame = 1f;

            public override void ProcessFrame(ref Frame inputFrame)
            {
                //Projection amount is 1 if not active, displacement otherwise
                var projectionAmount = active ? displacement : 1.0f;

                foreach (var hand in inputFrame.Hands)
                {
                    //Check if the given hand is the hand that should be modified
                    if (IsRightHand == hand.IsRight)
                    {
                        // Calculate the position of the head and the basis to calculate shoulder position.
                        var headPos = Camera.main.transform.position;
                        var shoulderBasis = Quaternion.LookRotation(
                          Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up),
                          Vector3.up);
                        //Debug.Log("Got here");
                        // Approximate shoulder position with magic values.
                        var shoulderPos = headPos
                                          + (shoulderBasis * (new Vector3(0f, -0.2f, -0.1f)
                                          + Vector3.left * 0.1f * (hand.IsLeft ? 1f : -1f)));

                        // Calculate the projection of the hand if it extends beyond the
                        // handMergeDistance.
                        var shoulderToHand = hand.PalmPosition.ToVector3() - shoulderPos;
                        var handShoulderDist = shoulderToHand.magnitude;
                        currentDisplacement = Mathf.Lerp(prevFrame, projectionAmount, Time.deltaTime);
                        hand.SetTransform(shoulderPos + shoulderToHand * currentDisplacement,
                                          hand.Rotation.ToQuaternion());
                        prevFrame = currentDisplacement;
                    }
                }
            }

        }
    }
}
