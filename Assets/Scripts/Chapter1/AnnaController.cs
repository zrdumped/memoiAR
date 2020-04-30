using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnaController : CharacterController
{
    // Start is called before the first frame update
    //public GameObject annaToPark;
    //public GameObject annaTopic, directionContent, subTitle;
    //public GameObject annaPickGift;



    //public GameObject rose;
    //public GameObject coin;

    public PickableObject flowerBunch;

    //public GameObject isAnnaText;

    //public GameObject RoseInCase, CoinInCase;

    public GameObject FlowerGiveMarker;

    private bool readyToPark;

    private string story, ins;

    private int roseCount = 0;
    private List<string> annaList = new List<string>
    {
        "Is there anyone you could ask for help?",
        "How nice of him to help!",
        "Why does he have a violin?",
        "Do you like his outfit?",
        "You’ve seen him perform in the park before!",
        "He looks handsome",
        "He’s dressed strangely. Tell him."
    };

    private List<GameObject> flowers;

    // Update is called once per frame
    void Update()
    {

    }

    public void receiveFlower()
    {
        om.HideAnnaFLowers(roseCount);
        FlowerGiveMarker.SetActive(true);
        story = "A nice man helped you pick up flowers";
        ins = "Show him your basket to get your flowers back";
        hm.InputNewWords(story, ins);
    }


    public void flowerGiven()
    {
        om.ShowAnnaFLowers(roseCount);
        FlowerGiveMarker.SetActive(false);
    }

    public void startTutorial()
    {
        story = "As usual, you start your day in the flower shop.";
        ins = "Sort the flowers.";
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
        story = "Oh No! Your flowers!";
        ins = "Pick up your flowers";
        hm.InputNewWords(story, ins);
    }

    public void leadAnnaToPark()
    {
        om.parkPanel.SetActive(true);
        //isAnnaText.SetActive(true);
        story = "It's a great chance to make money now.";
        ins = "Go to the park to sell flowers";
        hm.InputNewWords(story, ins);
    }

    public void annaReadyToPark()
    {
        readyToPark = true;
    }

    public void makeAnnaConfirm()
    {
        flowerBunch.readyToConfirmFlower = true;
        story = "Time to sell flowers";
        ins = "Pick up the flowers";
        hm.InputNewWords(story, ins);
    }

    public bool annaIsReadyToPark()
    {
        return readyToPark;
    }


    public void stopLeadAnnaToPark()
    {
        //Hint.SetActive(false);
    }

    //public void makeAnnaSelectGift()
    //{
    //    annaPickGift.SetActive(true);
    //}

    //public void annaSelectedGift()
    //{
    //    if (coin.activeSelf)
    //    {
    //        coin.GetComponent<Gift>().startCalDistance();
    //        annaPickGift.SetActive(false);
    //    }
    //    else if (rose.activeSelf)
    //    {
    //        rose.GetComponent<Gift>().startCalDistance();
    //        annaPickGift.SetActive(false);
    //    }

    //    //csc.GiftGiven();
    //}

    public void showFlowertext()
    {
        if (roseCount < annaList.Count)
        {
            story = annaList[roseCount];
            ins = "";
            hm.InputNewWords(story, ins);
        }
        roseCount++;
    }

    public void makeAnnaTalk()
    {
        story = "";
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

    //public void annaSelectRose()
    //{
    //    coin.SetActive(false);
    //    rose.SetActive(true);
    //}

    //public void annaSelectCoin()
    //{
    //    coin.SetActive(true);
    //    rose.SetActive(false);
    //}
}
