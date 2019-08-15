using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HandVR
{
    /// <summary>
    /// Used for controlling parameters for hand modifiers - basically a collection of variables and static methods
    /// </summary>
    public class HandModifierManager : MonoBehaviour
    {
        public static HandModifierManager instance = null;

        [SerializeField]
        private Leap.Unity.LeapXRServiceProvider cameraProvider;
        [SerializeField]
        private ModificationExamples.DisplacementPostProcessProvider displacementProvider;

        private float targetDisplacement = 1.0f;

        [SerializeField]
        private float _scaleFactor;
        [SerializeField, Tooltip("Use none for uniform scale")]
        private Axis _axisToScale = Axis.none;

        #region PROPERTIES
        /// <summary>
        /// Returns true if displacement is off
        /// </summary>
        public bool IsDisplacementReset
        {
            get
            {
                if (Mathf.Abs(displacementProvider.currentDisplacement - 1.0f) < 0.05f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns true if displacment is at the target value
        /// </summary>
        public bool IsDisplacementFinished
        {
            get
            {
                if (Mathf.Abs(displacementProvider.currentDisplacement - targetDisplacement) < 0.05f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public float ScaleFactor { get => _scaleFactor; }
        public Axis AxisToScale { get => _axisToScale; }

        public Vector3 InitCameraOffset { get; private set; }
        #endregion //PROPERTIES

        #region UNITY_MONOBEHAVIOUR_METHODS
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            cameraProvider.deviceOffsetMode = Leap.Unity.LeapXRServiceProvider.DeviceOffsetMode.ManualHeadOffset;
            InitCameraOffset = new Vector3(cameraProvider.deviceTiltXAxis, cameraProvider.deviceOffsetYAxis, cameraProvider.deviceOffsetZAxis);
            cameraProvider.deviceOffsetMode = Leap.Unity.LeapXRServiceProvider.DeviceOffsetMode.Default;
        }
        #endregion //UNITY_MONOBEHAVIOUR_METHODS

        /// <summary>
        /// Activates the displacementPostProcessProvider and sets its displacement to the value
        /// </summary>
        /// <param name="displacement">Value to set displacement to</param>
        public void ActivateDisplacementHands(float displacement)
        {
            targetDisplacement = displacement;
            displacementProvider.displacement = displacement;
            displacementProvider.active = true;
        }

        /// <summary>
        /// Truns off displacement post process provider
        /// </summary>
        public void DeactivateDisplacementHands()
        {
            displacementProvider.active = false;
        }

        /// <summary>
        /// Changes the size of the given gameobject over a set time
        /// </summary>
        /// <param name="go">GameObject to adjust</param>
        /// <param name="targetSize">Size to eventually reach</param>
        /// <param name="time">Time to scale gameobject</param>
        /// <param name="axis">Optional axis to scale along - leave empty for uniform scaling</param>
        /// <returns></returns>
        public static IEnumerator LerpSize(GameObject go, float targetSize, float time, Axis axis = Axis.none)
        {
            Vector3 initSize = go.transform.localScale;
            Vector3 finalSize = initSize;


            switch (axis)
            {
                case Axis.x:
                    finalSize.x = targetSize;
                    break;
                case Axis.y:
                    finalSize.y = targetSize;
                    break;
                case Axis.z:
                    finalSize.z = targetSize;
                    break;
                default:
                    finalSize = Vector3.one * targetSize;
                    break;
            }

            float initTime = 0;
            while (initTime < time)
            {
                Vector3 scale = Vector3.Lerp(initSize, finalSize, initTime / time);
                go.transform.localScale = scale;
                initTime += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }

        /// <summary>
        /// Changes the position of the given gameobject over a set time
        /// </summary>
        /// <param name="go">GameObject to adjust</param>
        /// <param name="targetPosition">Size to eventually reach - Note that this adjsuts localPosition, not global</param>
        /// <param name="time">Time to move gameobject</param>
        /// <returns></returns>
        public static IEnumerator LerpPosition(GameObject go, Vector3 targetPosition, float time)
        {
            Vector3 initPosition = go.transform.localPosition;

            float initTime = 0;
            while (initTime < time)
            {
                Vector3 newPos = Vector3.Lerp(initPosition, targetPosition, initTime / time);
                go.transform.localPosition = newPos;
                initTime += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }

        /// <summary>
        /// Alternate method for setting displacement - sets camera offset to given values
        /// </summary>
        /// <param name="xTilt">X rotation offset</param>
        /// <param name="yOffset">Y offset for displacement</param>
        /// <param name="zOffset">Z offset for displacement</param>
        /// <param name="time">Time to set displacement</param>
        /// <returns></returns>
        public IEnumerator LerpCameraProvider(float xTilt, float yOffset, float zOffset, float time)
        {
            Vector3 targetPosition = new Vector3(xTilt, yOffset, zOffset);
            yield return LerpCameraProvider(targetPosition, time);
        }

        /// <summary>
        /// Alternate method for setting displacement - sets camera offset to given values
        /// </summary>
        /// <param name="targetPosition">Target position for camera offset</param>
        /// <param name="time">Time to set displacement</param>
        /// <returns></returns>
        public IEnumerator LerpCameraProvider(Vector3 targetPosition, float time)
        {
            if (cameraProvider.deviceOffsetMode != Leap.Unity.LeapXRServiceProvider.DeviceOffsetMode.ManualHeadOffset)
            {
                cameraProvider.deviceOffsetMode = Leap.Unity.LeapXRServiceProvider.DeviceOffsetMode.ManualHeadOffset;
            }
            Vector3 initPosition = XRProviderOffsetToVector3(cameraProvider);

            float initTime = 0;
            while (initTime < time)
            {
                Vector3 newPos = Vector3.Lerp(initPosition, targetPosition, initTime / time);
                Vector3ToXRProvider(cameraProvider, newPos);
                initTime += Time.deltaTime;
                yield return null;
            }

            if(targetPosition == InitCameraOffset)
            {
                cameraProvider.deviceOffsetMode = Leap.Unity.LeapXRServiceProvider.DeviceOffsetMode.Default;
            }
            yield return null;
        }

        /// <summary>
        /// Utility function to return the offset of the given camera provider as a vector3
        /// </summary>
        /// <param name="camProv">Camera provider to get offset</param>
        /// <returns>Camera provider offset as a vector</returns>
        public static Vector3 XRProviderOffsetToVector3(Leap.Unity.LeapXRServiceProvider camProv)
        {
            return new Vector3(camProv.deviceTiltXAxis, camProv.deviceOffsetYAxis, camProv.deviceOffsetZAxis);
        }

        /// <summary>
        /// Utility function to set the offset for the given camera provider from the given Vector
        /// </summary>
        /// <param name="camProv">Camera provider to set offset</param>
        /// <param name="target">target offset as vector</param>
        public static void Vector3ToXRProvider(Leap.Unity.LeapXRServiceProvider camProv, Vector3 target)
        {
            if (camProv.deviceOffsetMode != Leap.Unity.LeapXRServiceProvider.DeviceOffsetMode.ManualHeadOffset)
            {
                camProv.deviceOffsetMode = Leap.Unity.LeapXRServiceProvider.DeviceOffsetMode.ManualHeadOffset;
            }
            camProv.deviceTiltXAxis = target.x;
            camProv.deviceOffsetYAxis = target.y;
            camProv.deviceOffsetZAxis = target.z;
        }

        public enum Axis
        {
            x, y, z, none
        }
    }
}