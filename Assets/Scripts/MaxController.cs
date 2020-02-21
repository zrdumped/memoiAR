﻿using System.Collections;
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
    public Text subText;

    public GameObject maxSelectMusic;

    public AudioClip music1, music2;

    public GameObject RoseInCase, CoinInCase;

    private PickHand hand;

    //public GameObject isMaxText;

    private ClientStateController csc;
    //1 || 2
    private int musicSelected = 0;
    void Start()
    {
        csc = this.GetComponent<ClientStateController>();
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<PickHand>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTutorial()
    {
        hintText.text = "10am was far too early for anyone to be awake. The Cabaret was hot last night. Even so, you still had to practice for your next gig.";
        subText.text = "Compose music for your upcoming gig";
        return;
    }

    public void sendFlower()
    {
        hintText.text = "Well you certainly couldn’t finish helping her by just standing there.";
        subText.text = "Scan the image on Annaliese’s iPad to give her the flowers";
        return;
    }

    public void wait()
    {
        Hint.SetActive(true);
        hintText.text = "You are setting up the instruments in the park";
        subText.text = "";
    }

    public void pickupFlowers()
    {
        Hint.SetActive(true);
        hintText.text = "Looks like someone dropped their flowers. Do you CHOOSE to help her?";
        subText.text = "Why Not?";
    }

    public void leadMaxToPark()
    {
        //isMaxText.SetActive(true);
        hand.releaseStaff();
        Hint.SetActive(true);
        hintText.text = "Why not try out your latest piece at the park? People sometimes gave money, but performing was its own reward.";
        subText.text = "Go to the park to play music";
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

    public void makeMaxTalk()
    {
        Hint.SetActive(true);
        hintText.text = "Why did you help her ? \nWhy was she carrying so many roses ? \nDid you happen to catch her name?";
        subText.text = "";
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
