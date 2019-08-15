using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HandVR
{
    namespace BlockGame
    {

        /// <summary>
        /// Example setup for a test object - Creates a variety of colored blocks which the user must then sort into various matching receptacles
        /// </summary>
        public class BlockGameManager : MonoBehaviour, ITest
        {

            #region PROPERTIES
            /// <summary>
            /// Returns array of all output results
            /// </summary>
            public IList<ITestData> Results { get; private set; }

            /// <summary>
            /// True if test is in progress
            /// </summary>
            public bool IsRunning { get; private set; } = false;
            #endregion //PROPERTIES

            const string instructions = "When the test begins, several blocks will appear in front of you. As fast as you can, sort the blocks into the bins of their corresponding color.";

            [SerializeField]
            private BlockGameBox magentaBox;
            [SerializeField]
            private BlockGameBox greenBox;
            [SerializeField]
            private BlockGameBox orangeBox;
            [SerializeField]
            private GameObject[] blockPrefabs;
            [SerializeField]
            private TextMesh resultLabel;
            [SerializeField]
            private int blocksToSpawn;

            #region PRIVATE_FIELDS
            private IList<Block> activeBlocks;
            private int trials;
            private float minutes;
            private bool isSetup = false;
            #endregion //PRIVATE_FIELDS

            #region PUBLIC_METHODS
            /// <summary>
            /// Sets how many trials the user must perform - Use set minutes instead
            /// </summary>
            /// <param name="numTests"></param>
            [System.Obsolete] public void SetNumTests(int numTests)
            {
                trials = numTests;
            }

            /// <summary>
            /// Sets how long the test lasts for. Once numMins have elapsed, no more trials will be started
            /// </summary>
            /// <param name="numMins"></param>
            public void SetNumMinutes(float numMins)
            {
                minutes = numMins;
            }

            /// <summary>
            /// Begins a test. Trials will continue to be created until numMins has elapsed
            /// </summary>
            /// <returns></returns>
            public IEnumerator StartTest()
            {
                resultLabel.gameObject.SetActive(false);
                IsRunning = true;
                gameObject.SetActive(false);
                yield return new Core.InteractableText.TextMessageYield(instructions);
                this.gameObject.SetActive(true);
                while (!isSetup)
                {
                    yield return null;
                }
                float initTime = Time.time;
                float stopTime = minutes * 60f;
                while ((Time.time - initTime) < stopTime)
                {
                    yield return Trial();
                    //Debug.Log(Results[Results.Count-1]);
                }
                resultLabel.text = "Finished!";
                resultLabel.gameObject.SetActive(true);
                Core.GameManager.instance.totalResults.Add(new Core.GameManager.TotalData { label = "Block Game", data = Results });
                yield return new WaitForSeconds(0.5f);
                resultLabel.gameObject.SetActive(false);
                IsRunning = false;

            }

            /// <summary>
            /// Ends test early
            /// </summary>
            public void StopTest()
            {
                StopAllCoroutines();
                IsRunning = false;
            }

            /// <summary>
            /// Single instance of a trial - spawns in a set number of blocks, which users must then sort into matching receptacles. Trial ends when no active blocks are left in the scene
            /// </summary>
            /// <returns></returns>
            public IEnumerator Trial()
            {
                resultLabel.gameObject.SetActive(false);
                for (int i = 0; i < blocksToSpawn; i++)
                {
                    Vector3 position = Random.insideUnitSphere * 0.2f;
                    position.z += 0.5f;
                    GameObject b = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)], transform, true);
                    b.transform.position = position;
                    Block block = b.GetComponent<Block>();
                    activeBlocks.Add(block);
                    AnchorColors colorLabel = (AnchorColors)Random.Range(0, 3);
                    switch (colorLabel)
                    {
                        case AnchorColors.green:
                            block.Setup(greenBox);
                            break;
                        case AnchorColors.magenta:
                            block.Setup(magentaBox);
                            break;
                        case AnchorColors.orange:
                            block.Setup(orangeBox);
                            break;
                    }
                }
                float _time = Time.time;
                while (activeBlocks.Count != 0)
                {
                    yield return null;
                }
                float finalTime = Time.time - _time;
                Results.Add(new BlockData
                {
                    time = finalTime
                });
                resultLabel.text = "Sucess!";
                resultLabel.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
            #endregion //PUBLIC_METHODS

            #region PRIVATE/INTERNAL_METHODS
            /// <summary>
            /// Removes a block from the list of currently active blocks
            /// </summary>
            /// <param name="b">Block to remove</param>
            internal void RemoveBlock(Block b)
            {
                if (activeBlocks.Contains(b))
                {
                    activeBlocks.Remove(b);
                }
            }
            #endregion //PRIVATE/INTERNAL_METHODS

            #region UNITY_MONOBEHAVIOUR_METHODS
            // Start is called before the first frame update
            void Start()
            {
                Results = new List<ITestData>();
                resultLabel.gameObject.SetActive(false);
                activeBlocks = new List<Block>();
                //Debug.Log("Boxes finished building" + this.name);
                isSetup = true;

            }
            #endregion //UNITY_MONOBEHAVIOUR_METHODS

        }


        /// <summary>
        /// Struct for output data - stores time taken to complete each trial
        /// </summary>
        [System.Serializable]
        public struct BlockData : ITestData
        {
            public float time;

            public override string ToString()
            {
                return time.ToString();
            }
        }

        /// <summary>
        /// Colors of blocks & boxes
        /// </summary>
        internal enum AnchorColors
        {
            magenta, orange, green
        }

        

        
    }
}

