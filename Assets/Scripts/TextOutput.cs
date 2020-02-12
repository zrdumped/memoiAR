using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOutput : MonoBehaviour
{
    public Text targetText;

    private string text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetText.text = text;
    }

    public void setText(string text)
    {
        this.text = text;
    }
}
