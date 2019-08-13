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

            public bool IsRunning { get; private set; } = false;

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

            private IList<Block> activeBlocks;

            [SerializeField]
            private int blocksToSpawn;

            private int trials;
            private float minutes;
            private bool isSetup = false;

            [System.Obsolete]
            public void SetNumTests(int numTests)
            {
                trials = numTests;
            }

            public void SetNumMinutes(float numMins)
            {
                minutes = numMins;
            }

            public IEnumerator StartTest()
            {
                resultLabel.gameObject.SetActive(false);
                IsRunning = true;
                gameObject.SetActive(false);
                yield return new InteractableText.TextMessageYield(instructions);
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
                GameManager.instance.totalResults.Add(new GameManager.TotalData { label = "Block Game", data = Results });
                yield return new WaitForSeconds(0.5f);
                resultLabel.gameObject.SetActive(false);
                IsRunning = false;

            }

            public void StopTest()
            {
                StopAllCoroutines();
                IsRunning = false;
            }

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

            internal void RemoveBlock(Block b)
            {
                if (activeBlocks.Contains(b))
                {
                    activeBlocks.Remove(b);
                }
            }

            // Start is called before the first frame update
            void Start()
            {
                Results = new List<ITestData>();
                resultLabel.gameObject.SetActive(false);
                activeBlocks = new List<Block>();
                //Debug.Log("Boxes finished building" + this.name);
                isSetup = true;

            }

            

        }

        [System.Serializable]
        public struct BlockData : ITestData
        {
            public float time;

            public override string ToString()
            {
                return time.ToString();
            }
        }

        internal enum AnchorColors
        {
            magenta, orange, green
        }

        

        
    }
}

