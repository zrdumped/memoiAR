using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxController : CharacterController
{
    // Start is called before the first frame update
    //public GameObject maxToPark;
   // public GameObject maxTopic, directionContent, subTitle;

   // public GameObject maxSelectMusic;

    public AudioClip music1, music2;

  //  public GameObject RoseInCase, CoinInCase;
    private string ins, story;

    private bool readyToPark = false;

    //public GameObject isMaxText;

    //1 || 2
    private int musicSelected = 0;

    private int roseCount = 0;
    private List<string> maxList = new List<string>
    {
        "Did you help her yet?",
        "Did you happen to catch her name?",
        "She’s cute. Do you think she likes music?",
        "Has she thanked you yet?",
        "Nice! Why does she have so many flowers?" 
    };

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTutorial()
    {
        story = "One morning at home...";
        ins = "Place music notes on the paper to compose a song.";
        hm.InputNewWords(story, ins);
        return;
    }

    public void sendFlower()
    {
        story = "Well you certainly couldn’t finish helping her by just standing there.";
        ins = "Scan the image on Annaliese’s iPad to give her the flowers";
        hm.InputNewWords(story, ins);
        return;
    }

    public void wait()
    {
        story = "You are setting up the instruments in the park";
        ins = "";
        hm.InputNewWords(story, ins);
    }

    public void pickupFlowers()
    {
        story = "Looks like someone dropped their flowers!";
        ins = "Help her. Don't be a jerk.";
        hm.InputNewWords(story, ins);
    }

    public void leadMaxToPark()
    {
        om.parkPanel.SetActive(true);
        //isMaxText.SetActive(true);
        story = "Why not try out your latest piece at the park?";
        ins = "Go to the park to play music";
        hm.InputNewWords(story, ins);
    }

    public void ConfirmComposing()
    {
        story = "You composed some music";
        ins = "Pick up the violin case to confirm";
        hm.InputNewWords(story, ins);
    }

    public void maxReadyToPark()
    {
        readyToPark = true;
    }

    public bool maxIsReadyToPark()
    {
        return readyToPark;
    }

    public void stopLeadMaxToPark()
    {
        //Hint.SetActive(false);
    }

    //public void makeMaxSelectMusic()
    //{
    //    maxSelectMusic.SetActive(true);
    //}

    //public void maxSelectedMusic()
    //{
    //    maxSelectMusic.SetActive(false);
    //    csc.MusicSelected(musicSelected);
    //}

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

    public void showFlowertext()
    {
        if(roseCount < maxList.Count)
        {
            story = maxList[roseCount];
            ins = "";
            hm.InputNewWords(story, ins);
        }
        roseCount++;
    }

    public void makeMaxTalk()
    {
        story = "";
        ins = "";
        hm.InputNewWords(story, ins);
        //if (giftNum == 0)
        //{
        //    directionContent.GetComponent<Text>().text = "The Flower was Pretty.\nTell Annaliese why you chose this music.";
        //    subTitle.GetComponent<Text>().text = "Curious, Flirtatious, Coy";
        //    RoseInCase.SetActive(true);
        //}
        //else
        //{
        //    directionContent.GetComponent<Text>().text = "You appreciated the Coin.\nTell Annaliese why you chose this music.";
        //    subTitle.GetComponent<Text>().text = "Curious, Grateful, Reflective";
        //    CoinInCase.SetActive(true);
        //}
        //this.GetComponent<AudioSource>().Stop();
        //maxTopic.SetActive(true);
    }

}
