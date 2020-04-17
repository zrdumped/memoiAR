using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using KKSpeech;

public class ChantingController : MonoBehaviour
{
    //public GameObject bg;
    //public GameObject slider;
    //public GameObject outline;
    //public GameObject give;
    //public GameObject us;
    //public GameObject our;
    //public GameObject huaband;
    //public GameObject back;
    public List<GameObject> panels;
    public int recognizedNum = 0;
    private Coroutine bouncingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        SpeechRecognizerListener listener = this.GetComponent<SpeechRecognizerListener>();
        listener.onFinalResults.AddListener(Hit);
        listener.onPartialResults.AddListener(Hit);

        StartChanting();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            Hit();
    }

    public void StartChanting()
    {
        SpeechRecognizer.StartRecording(true);
        
        Vector3 sliderRect = panels[1].GetComponent<RectTransform>().localPosition;
        sliderRect.x = -panels[1].GetComponent<RectTransform>().rect.width / 2;
        panels[1].GetComponent<RectTransform>().localPosition = sliderRect;

        bouncingCoroutine = StartCoroutine(BounceSlider());

        int i = 0;
        for(; i < 3; i++)
        {
            panels[i].SetActive(true);
        }
        for(; i < 8; i++)
        {
            panels[i].SetActive(false);
        }
    }

    public IEnumerator BounceSlider()
    {
        while (true)
        {
            int endX = Random.Range(-(int)panels[1].GetComponent<RectTransform>().rect.width, 0);
            panels[1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(endX, 0), 0.3f);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void Hit()
    {
        panels[recognizedNum + 3].SetActive(true);
        recognizedNum++;
        if(recognizedNum == 5)
        {
            StartCoroutine(disappear());
        }
    }

    public void Hit(string input)
    {
        input.ToLower();
        panels[3].SetActive(input.Contains("give"));
        panels[4].SetActive(input.Contains("us"));
        panels[5].SetActive(input.Contains("our"));
        panels[6].SetActive(input.Contains("husband"));
        panels[7].SetActive(input.Contains("back"));
        recognizedNum = 0;
        for (int i = 3; i < 8; i++)
            if (panels[i].activeSelf) recognizedNum++;
        if (recognizedNum == 5)
            StartCoroutine(disappear());
    }

    private IEnumerator disappear()
    {
        SpeechRecognizer.StopIfRecording();

        panels[1].SetActive(false);
        StopCoroutine(bouncingCoroutine);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 8; i++)
        {
            if (i == 1) continue;
            Color endColor = panels[i].GetComponent<Image>().color;
            endColor.a = 0;
            panels[i].GetComponent<Image>().DOColor(endColor, 1);
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 8; i++)
        {
            panels[i].SetActive(false);
        }
    }
}
