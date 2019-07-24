using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HandVR
{

    /// <summary>
    /// Used for controlling parameters for hand modifiers - basically a collection of variables
    /// </summary>
    public class HandModifierManager : MonoBehaviour
    {
        public static HandModifierManager instance = null;

        [SerializeField]
        private float _scaleFactor;

        [SerializeField, Tooltip("Use none for uniform scale")]
        private Axis _axisToScale = Axis.none;

        public float ScaleFactor { get => _scaleFactor; }
        public Axis AxisToScale { get => _axisToScale; }

        // Start is called before the first frame update
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
        }

        public enum Axis
        {
            x, y, z, none
        }
    }
}