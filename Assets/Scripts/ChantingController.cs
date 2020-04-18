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
    //public int recognizedNum = 0;
    private Coroutine bouncingCoroutine;
    private Chapter3Controller c3c;
    private bool recognized = false;
    // Start is called before the first frame update
    void Start()
    {
        c3c = GameObject.FindGameObjectWithTag("Chap3Client").GetComponent<Chapter3Controller>();
        SpeechRecognizerListener listener = this.GetComponent<SpeechRecognizerListener>();
        listener.onFinalResults.AddListener(End);
        listener.onPartialResults.AddListener(Hit);

        //StartChanting();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartChanting()
    {
        recognized = false;

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


    public void Hit(string input)
    {
        input.ToLower();
        if(!panels[3].activeSelf)panels[3].SetActive(true);
        if(!panels[4].activeSelf)panels[4].SetActive(input.Contains("us"));
        if(!panels[5].activeSelf)panels[5].SetActive(input.Contains("our"));
        if(!panels[6].activeSelf)panels[6].SetActive(input.Contains("husband"));
        if(!panels[7].activeSelf)panels[7].SetActive(input.Contains("back"));
        int recognizedNum = 0;
        for (int i = 3; i < 8; i++)
            if (panels[i].activeSelf) recognizedNum++;
        if (recognizedNum == 5 && !recognized)
        {
            recognized = true;
            SpeechRecognizer.StopIfRecording();
        }
    }

    public void End(string input)
    {
        StartCoroutine(disappear());
    }

    private IEnumerator disappear()
    {
        StopCoroutine(bouncingCoroutine);
        panels[1].SetActive(false);
        yield return new WaitForSeconds(2);
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

        for (int i = 0; i < 8; i++)
        {
            if (i == 1) continue;
            Color endColor = panels[i].GetComponent<Image>().color;
            endColor.a = 1;
            panels[i].GetComponent<Image>().color = endColor;
        }

        c3c.ChantingCompleted();
    }
}
