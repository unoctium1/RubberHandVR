using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR
{

    public class DisplacementPostProcessProvider : PostProcessProvider
    {

        [Header("Displacement")]

        [Range(0f, 5f)]
        public float displacement = 1f;

        private bool IsRightHand { get
            {
                return GameManager.instance.isRightHand;
            }
        }


        public float currentDisplacement = 1.0f;

        public bool active = false;

        private float prevFrame = 1f;

        public override void ProcessFrame(ref Frame inputFrame)
        {

            var projectionAmount = active ? displacement : 1.0f;

            foreach (var hand in inputFrame.Hands)
            {
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
