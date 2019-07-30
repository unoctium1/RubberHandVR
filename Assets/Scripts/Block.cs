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
            private Renderer mesh;

            // Start is called before the first frame update
            void Start()
            {
                manager = GetComponentInParent<BlockGameManager>();
                anchorableBehaviour = GetComponent<AnchorableBehaviour>();
                mesh = GetComponent<Renderer>();
                
                
            }

            internal void Setup()
            {
                mesh.materials[0] = manager.Boxes[colorLabel].BoxMat;
                anchorableBehaviour.anchorGroup = manager.Boxes[colorLabel].GroupLabel;
                anchorableBehaviour.OnAttachedToAnchor += OnAttached;
            }

            private void OnAttached()
            {
                manager.RemoveBlock(this);
                Destroy(this.gameObject);
            }
        }
    }
}

