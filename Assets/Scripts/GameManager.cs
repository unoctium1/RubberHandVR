using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandVR
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;

        [SerializeField]
        private Test[] tests;
        [SerializeField]
        private GameObject[] modifiers;
        [SerializeField]
        private GameObject modPanelPrefab;
        [SerializeField]
        private GameObject modPanelParent;
        private IList<ModPanel> modPanels;

        [SerializeField]
        private GameObject testPanelPrefab;
        [SerializeField]
        private GameObject testPanelParent;
        private IList<GamePanel> testPanels;

        [SerializeField]
        private GameObject leftHand;
        [SerializeField]
        private GameObject rightHand;

        private bool isRightHand = true;

        [SerializeField]
        public GameObject ActiveHand { get
            {
                if (isRightHand)
                {
                    return rightHand;
                }
                else
                {
                    return leftHand;
                }
            }
        }

        //private IList<ITest<ITestData>> tests;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;

            }
            else
            {
                Destroy(this);
            }

            testPanels = new List<GamePanel>();
            modPanels = new List<ModPanel>();
            float offset = 0;
            foreach(Test t in tests)
            {
                GameObject p = Instantiate(testPanelPrefab, testPanelParent.transform);
                RectTransform rt = p.GetComponent<RectTransform>();
                Vector3 pos = rt.position;
                pos.y += offset;
                offset += rt.rect.height;
                GamePanel panel = p.GetComponent<GamePanel>();
                panel.Init(t.testObject, t.testName);
                testPanels.Add(panel);
            }
            offset = 0;
            foreach (GameObject m in modifiers)
            {
                GameObject p = Instantiate(m, modPanelParent.transform);
                RectTransform rt = p.GetComponent<RectTransform>();
                Vector3 pos = rt.position;
                pos.y += offset;
                offset += rt.rect.height;
                ModPanel panel = p.GetComponent<ModPanel>();
                modPanels.Add(panel);
            }
        }

        public void SetRightHand()
        {
            isRightHand = true;
        }

        public void SetLeftHand()
        {
            isRightHand = false;
        }

        [System.Serializable]
        private struct Test
        {
            public GameObject testObject;
            public string testName;
        }

    }
}
