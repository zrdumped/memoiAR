using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnaController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject annaToPark;
    public GameObject annaTopic, directionContent, subTitle;
    public GameObject annaPickGift;

    public GameObject rose;
    public GameObject coin;

    public GameObject isAnnaText;

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

    public void leadAnnaToPark()
    {
        isAnnaText.SetActive(true);
        annaToPark.SetActive(true);
    }

    public void stopLeadAnnaToPark()
    {
        annaToPark.SetActive(false);
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
