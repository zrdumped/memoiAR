using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnaController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject annaToPark;
    public GameObject annaTopic;
    public GameObject annaPickGift;

    public GameObject rose;
    public GameObject coin;

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
        if (coin.activeSelf == true)
        {
            coin.GetComponent<Gift>().startCalDistance();
            annaPickGift.SetActive(false);
        }
        else if (rose.activeSelf == true)
        {
            rose.GetComponent<Gift>().startCalDistance();
            annaPickGift.SetActive(false);
        }

        //csc.GiftGiven();
    }

    public void makeAnnaTalk()
    {
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
