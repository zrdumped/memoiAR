using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject maxToPark;
    public GameObject maxTopic;
    public GameObject maxSelectMusic;

    public AudioClip music1, music2;

    public GameObject RoseInCase, CoinInCase;

    private ClientStateController csc;
    //1 || 2
    private int musicSelected = 0;
    void Start()
    {
        csc = this.GetComponent<ClientStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void leadMaxToPark()
    {
        maxToPark.SetActive(true);
    }

    public void stopLeadMaxToPark()
    {
        maxToPark.SetActive(false);
    }

    public void makeMaxSelectMusic()
    {
        maxSelectMusic.SetActive(true);
    }

    public void maxSelectedMusic()
    {
        maxSelectMusic.SetActive(false);
        csc.MusicSelected(musicSelected);
    }

    public void maxHearMusic1()
    {
        this.GetComponent<AudioSource>().clip = music1;
        musicSelected = 1;
        this.GetComponent<AudioSource>().Play();
    }

    public void maxHearMusic2()
    {
        this.GetComponent<AudioSource>().clip = music2;
        musicSelected = 2;
        this.GetComponent<AudioSource>().Play();
    }

    public void makeMaxTalk(int giftNum)
    {
        if (giftNum == 0)
        {
            maxTopic.GetComponent<Text>().text = "How Do You Feel About The Rose?";
            RoseInCase.SetActive(true);
        }
        else
        {
            maxTopic.GetComponent<Text>().text = "How Do You Feel About The Coin?";
            CoinInCase.SetActive(true);
        }
        this.GetComponent<AudioSource>().Stop();
        maxTopic.SetActive(true);
    }
}
