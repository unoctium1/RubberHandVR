using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR
{

    public class ScalePostProcessProvider : PostProcessProvider
    {

        [Header("Projection")]

        [Range(0f, 5f)]
        public float scale = 1f;


        public bool isRightHand = true;

        public bool active = false;

        private float prevFrameVal = 1f;

        public override void ProcessFrame(ref Frame inputFrame)
        {

            var scalarAmount = active ? scale : 1.0f;

            foreach (var hand in inputFrame.Hands)
            {
                if (isRightHand == hand.IsRight)
                {

                    var currentFrameVal = Mathf.Lerp(prevFrameVal, scalarAmount, Time.deltaTime);
                    Vector scale = Vector.Ones * currentFrameVal;
                    LeapTransform newHand = hand.Basis;
                    //newHand.scale = scale;
                    hand.Transform(newHand);
                    prevFrameVal = currentFrameVal;
                }
            }
        }

    }
}
