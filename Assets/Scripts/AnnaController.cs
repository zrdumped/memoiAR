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

    public GameObject rose;
    public GameObject coin;

    //public GameObject isAnnaText;

    public GameObject RoseInCase, CoinInCase;

    private ClientStateController csc;
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
        hintText.text = "Anna, you are selling flowers in the park";
    }

    public void pickupFlowers()
    {
        Hint.SetActive(true);
        hintText.text = "Anna, your flowers fell on the ground. Pick them up.";
    }

    public void leadAnnaToPark()
    {
        //isAnnaText.SetActive(true);
        Hint.SetActive(true);
        hintText.text = "Anna, go to park selling flowers";
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

    public void makeAnnaTalk(int musicNum, int giftNum)
    {
        string word = "";

        if (giftNum == 0)
        {
            word += "Tell Max why you gave him the rose.\n";
            RoseInCase.SetActive(true);
        }
        else
        {
            word += "Tell Max why you gave him the coin.\n";
            CoinInCase.SetActive(true);
        }

        word += "How did the music make you feel?";
        subTitle.GetComponent<Text>().text = "Charmed, Curious, Understood, Happy";

        directionContent.GetComponent<Text>().text = word;
        annaTopic.SetActive(true);
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
