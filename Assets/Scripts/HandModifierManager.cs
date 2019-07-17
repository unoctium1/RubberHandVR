﻿using System.Collections;
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
        public float UniformScaleFactor { get; private set; }

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