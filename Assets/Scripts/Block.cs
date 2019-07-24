using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HandVR
{
    namespace BlockGame
    {
        public class Block : MonoBehaviour
        {
            [SerializeField]
            internal AnchorColors colorLabel;

            [SerializeField]
            private BlockGameManager manager;

            private AnchorableBehaviour anchorableBehaviour;
            private MeshRenderer mesh;

            // Start is called before the first frame update
            void Start()
            {
                manager = GetComponentInParent<BlockGameManager>();
                anchorableBehaviour = GetComponent<AnchorableBehaviour>();
                mesh = GetComponent<MeshRenderer>();

                
            }

            internal void Setup()
            {
                mesh.material = manager.Boxes[colorLabel].BoxMat;
                anchorableBehaviour.anchorGroup = manager.Boxes[colorLabel].GroupLabel;
            }

            // Update is called once per frame
            void Update()
            {

            }
        }
    }
}

