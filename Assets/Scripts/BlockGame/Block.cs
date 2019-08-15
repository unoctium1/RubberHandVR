using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HandVR
{
    namespace BlockGame
    {

        /// <summary>
        /// Behaviour of blocks in the block sorting task
        /// </summary>
        public class Block : MonoBehaviour
        {
            [SerializeField]
            internal AnchorColors colorLabel;

            [SerializeField]
            private BlockGameManager manager;
            [SerializeField]
            private BlockGameBox box;

            private AnchorableBehaviour anchorableBehaviour;
            private Renderer mesh;

            // Start is called before the first frame update
            void Start()
            {
                manager = GetComponentInParent<BlockGameManager>();
                anchorableBehaviour = GetComponent<AnchorableBehaviour>();
                mesh = GetComponent<Renderer>();
                
                
            }

            internal void Setup(BlockGameBox _box)
            {
                box = _box;
                if(mesh == null)
                {
                    mesh = GetComponent<Renderer>();
                }
                if(anchorableBehaviour == null)
                {
                    anchorableBehaviour = GetComponent<AnchorableBehaviour>();
                }
                colorLabel = box.AnchorLabel;
                mesh.material = box.BoxMat;
                anchorableBehaviour.anchorGroup = box.GroupLabel;
                anchorableBehaviour.OnAttachedToAnchor += OnAttached;
            }

            private void OnAttached()
            {
                manager.RemoveBlock(this);
                anchorableBehaviour.anchor.anchoredObjects.Clear();
                Destroy(this.gameObject);
            }
        }
    }
}

