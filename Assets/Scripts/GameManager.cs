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
        private GameObject panelPrefab;
        [SerializeField]
        private GameObject panelParent;
        private IList<GamePanel> panels;

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

            panels = new List<GamePanel>();

            float offset = 0;
            foreach(Test t in tests)
            {
                GameObject p = Instantiate(panelPrefab, panelParent.transform);
                RectTransform rt = p.GetComponent<RectTransform>();
                Vector3 pos = rt.position;
                pos.y += offset;
                offset += rt.rect.height;
                GamePanel panel = p.GetComponent<GamePanel>();
                panel.Init(t.testObject, t.testName);
                panels.Add(panel);
            }
        }

        [System.Serializable]
        private struct Test
        {
            public GameObject testObject;
            public string testName;
        }
    }
}
