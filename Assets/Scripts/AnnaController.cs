using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnaController : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject annaToPark;
    public GameObject annaTopic, directionContent, subTitle;
    public GameObject annaPickGift;

    private HintManager hm;
    private string ins, story;

    public GameObject rose;
    public GameObject coin;

    //public GameObject isAnnaText;

    public GameObject RoseInCase, CoinInCase;

    public GameObject FlowerGiveMarker;

    private ClientStateController csc;

    private PickHand hand;
    void Start()
    {
        csc = this.GetComponent<ClientStateController>();
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<PickHand>();
        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void receiveFlower()
    {
        FlowerGiveMarker.SetActive(true);
    }


    public void flowerGiven()
    {
        FlowerGiveMarker.SetActive(false);
    }

    public void startTutorial()
    {
        story = "Annalieses Flowers: your flower shop. It was yours, to make the world a little more beautiful (and to feed yourself).";
        ins = "Sort the flowers that you sold that sunny April day";
        hm.InputNewWords(story, ins);
        return;
    }

    public void wait()
    {
        story = "You are selling flowers in the park";
        ins = "";
        hm.InputNewWords(story, ins);
    }

    public void pickupFlowers()
    {
        story = "Oh no! Your flowers!";
        ins = "Don’t let them get away";
        hm.InputNewWords(story, ins);
    }

    public void leadAnnaToPark()
    {
        //isAnnaText.SetActive(true);
        story = "Office workers and families would concentrate at the park for lunch: a great chance to make extra money.";
        ins = "Go to the park to sell flowers";
        hm.InputNewWords(story, ins);
    }

    public void stopLeadAnnaToPark()
    {
        //Hint.SetActive(false);
    }

    public void makeAnnaSelectGift()
    {
        annaPickGift.SetActive(true);
    }

    public void annaSelectedGift()
    {
        if (coin.activeSelf)
        {
            coin.GetComponent<Gift>().startCalDistance();
            annaPickGift.SetActive(false);
        }
        else if (rose.activeSelf)
        {
            rose.GetComponent<Gift>().startCalDistance();
            annaPickGift.SetActive(false);
        }

        //csc.GiftGiven();
    }

    public void makeAnnaTalk()
    {
        story = "How did his helping you make you feel? \nWhy does he have an open violin case? \nDid you happen to catch his name?";
        ins = "";
        hm.InputNewWords(story, ins);
        //string word = "";

        //if (giftNum == 0)
        //{
        //    word += "Tell Max why you gave him the rose.\n";
        //    RoseInCase.SetActive(true);
        //}
        //else
        //{
        //    word += "Tell Max why you gave him the coin.\n";
        //    CoinInCase.SetActive(true);
        //}

        //word += "How did the music make you feel?";
        //subTitle.GetComponent<Text>().text = "Charmed, Curious, Understood, Happy";

        //directionContent.GetComponent<Text>().text = word;
        //annaTopic.SetActive(true);
    }

    public void annaSelectRose()
    {
        coin.SetActive(false);
        rose.SetActive(true);
    }

    public void annaSelectCoin()
    {
        coin.SetActive(true);
        rose.SetActive(false);
    }
}
