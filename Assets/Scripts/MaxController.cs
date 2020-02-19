using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxController : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject maxToPark;
    public GameObject maxTopic, directionContent, subTitle;

    public GameObject Hint;
    public Text hintText;

    public GameObject maxSelectMusic;

    public AudioClip music1, music2;

    public GameObject RoseInCase, CoinInCase;

    //public GameObject isMaxText;

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

    public void startTutorial()
    {
        return;
    }

    public void wait()
    {
        Hint.SetActive(true);
        hintText.text = "Max, you are setting up the instruments in the park";
    }

    public void pickupFlowers()
    {
        Hint.SetActive(true);
        hintText.text = "Max, someone's flowers fell on the ground. Help her.";
    }

    public void leadMaxToPark()
    {
        //isMaxText.SetActive(true);
        Hint.SetActive(true);
        hintText.text = "Max, go to the park playing the music you composed";
    }

    public void stopLeadMaxToPark()
    {
        Hint.SetActive(false);
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
            directionContent.GetComponent<Text>().text = "The Flower was Pretty.\nTell Annaliese why you chose this music.";
            subTitle.GetComponent<Text>().text = "Curious, Flirtatious, Coy";
            RoseInCase.SetActive(true);
        }
        else
        {
            directionContent.GetComponent<Text>().text = "You appreciated the Coin.\nTell Annaliese why you chose this music.";
            subTitle.GetComponent<Text>().text = "Curious, Grateful, Reflective";
            CoinInCase.SetActive(true);
        }
        this.GetComponent<AudioSource>().Stop();
        maxTopic.SetActive(true);
    }
}
