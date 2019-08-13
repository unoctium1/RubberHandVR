using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFromSlider : MonoBehaviour
{
    private Text textField;
    [SerializeField]
    private Slider s;
    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<Text>();
        SetText(s.value);
    }

    public void SetText(float v)
    {
        textField.text = v.ToString();
    }
}
