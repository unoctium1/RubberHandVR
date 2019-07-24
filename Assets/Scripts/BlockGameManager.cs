using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HandVR
{
    namespace BlockGame
    {
        public class BlockGameManager : MonoBehaviour, ITest
        {
            public IList<ITestData> Results { get; private set; }

            internal IDictionary<AnchorColors, BlockGameBox> Boxes { get; private set; }
            [SerializeField]
            private BlockGameBox[] boxes;
            [SerializeField]
            private GameObject[] blockPrefabs;

            public void SetNumTests(int numTests)
            {
                throw new System.NotImplementedException();
            }

            public IEnumerator StartTest()
            {
                throw new System.NotImplementedException();
            }

            public void StopTest()
            {
                throw new System.NotImplementedException();
            }

            // Start is called before the first frame update
            void Start()
            {
                Results = new List<ITestData>();
                Boxes = new Dictionary<AnchorColors, BlockGameBox>();

                foreach (BlockGameBox b in boxes)
                {
                    Boxes[b.AnchorLabel] = b;
                }

            }

        }

        internal enum AnchorColors
        {
            magenta, orange, green
        }

        
    }
}

