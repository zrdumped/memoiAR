using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager3 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject endPanel;
    private HintManager hm;
    private Chapter3Controller c3c;
    [Header("Anna Chants")]
    public AudioClip firstChanting;
    public AudioClip secondChanting;
    public AudioClip thirdChanting;
    public AudioClip groupChanting;
    public AudioClip womenGroupNoise;
    public AudioClip happierSound;

    [Header("Max In Jail")]
    public AudioClip jailSound;
    public AudioClip doorOpen;
    public GameObject paper;
    public GameObject paperOnScreen;
    public GameObject writePanel;
    private bool readyToPinch = false;
    void Start()
    {
        paperOnScreen.SetActive(false);
        writePanel.SetActive(false);

        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        c3c = GameObject.FindGameObjectWithTag("Chap3Client").GetComponent<Chapter3Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToPinch)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                readyToPinch = false;
                StartCoroutine(c3c.MaxEndWrite());
            }

        }
    }

    public void StartWriting()
    {
        paper.SetActive(false);
        paperOnScreen.SetActive(true);
        writePanel.SetActive(true);
        hm.InputNewWords("This is your last chance to write to Anna.", "Write something");
    }

    public void MaxWrite()
    {
        readyToPinch = true;
        hm.InputNewWords("She may never see these words but you mean them.", "Pinch the paper");
    }

    public void EndWrite()
    {
        paperOnScreen.GetComponent<HandWrite>().deleteTrails();
        paperOnScreen.SetActive(false);
        writePanel.SetActive(false);
        StartCoroutine(c3c.MaxEndWrite());
    }

}
