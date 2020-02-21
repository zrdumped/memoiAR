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

    public GameObject Hint;
    public Text hintText;
    public Text subText;

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
        Hint.SetActive(true);
        hintText.text = "Annalieses Flowers: your flower shop. It was yours, to make the world a little more beautiful (and to feed yourself).";
        subText.text = "Sort the flowers that you sold that sunny April day";
        return;
    }

    public void wait()
    {
        Hint.SetActive(true);
        hintText.text = "You are selling flowers in the park";
        subText.text = "";
    }

    public void pickupFlowers()
    {
        Hint.SetActive(true);
        hintText.text = "Oh no! Your flowers!";
        subText.text = "Don’t let them get away";
    }

    public void leadAnnaToPark()
    {
        //isAnnaText.SetActive(true);
        hand.releaseStaff();
        Hint.SetActive(true);
        hintText.text = "Office workers and families would concentrate at the park for lunch: a great chance to make extra money.";
        subText.text = "Go to the park to sell flowers";
    }

    public void stopLeadAnnaToPark()
    {
        Hint.SetActive(false);
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
        Hint.SetActive(true);
        hintText.text = "How did his helping you make you feel? \nWhy does he have an open violin case? \nDid you happen to catch his name?";
        subText.text = "";
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
