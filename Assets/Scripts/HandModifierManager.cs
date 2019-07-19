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
        private float _uniformScaleFactor;

        public float UniformScaleFactor { get => _uniformScaleFactor; }

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
    }
}