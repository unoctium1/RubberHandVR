using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandVR
{
    namespace BlockGame
    {
        internal class BlockGameBox : MonoBehaviour
        {

            [SerializeField]
            private AnchorColors _label;
            [SerializeField]
            private Material _boxMat;
            [SerializeField]
            private Anchor _anchor;
            [SerializeField]
            private AnchorGroup _group;

            internal AnchorColors AnchorLabel { get => _label;  }
            public Material BoxMat { get => _boxMat;  }
            public Anchor Anchor { get => _anchor; }
            public AnchorGroup GroupLabel { get => _group; }

        }
    }
}

