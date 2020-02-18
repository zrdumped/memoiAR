using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientStateController : MonoBehaviour
{
    //0 - not assigned
    //1 - Anna
    //2 - Max
    int character = 0;

    private TCPTestClient client;
    private AnnaController anna;
    private MaxController max;

    private bool newState = false;
    private string newStateName = "";

    public GameObject isMaxText, isAnnaText;


    //1 || 2
    private int musicSelected = 0;

    private void Start()
    {
        client = this.GetComponent<TCPTestClient>();
        anna = this.GetComponent<AnnaController>();
        max = this.GetComponent<MaxController>();
    }

    private void Update()
    {
        if (newState)
        {
            newState = false;
            SetState(newStateName);
        }
    }


    public void NetworkSetState(string stateName)
    {
        newState = true;
        newStateName = stateName;
    }

    private void SetState(string stateName)
    {
        Debug.Log(stateName + " " + character);
        if (stateName == "AnnaStartTutorial" && character == 1)
        {
            anna.startTutorial();
        }
        else if (stateName == "MaxStartTutorial" && character == 2)
        {
            max.startTutorial();
        }
    }

    //Chapter 1 Version 1
    //private void SetState(string stateName)
    //{
    //    Debug.Log(stateName + " " + character);
    //    if(stateName == "MaxToPark" && character == 2)
    //    {
    //        //max to park
    //        max.leadMaxToPark();
    //    }

    //    else if (stateName == "MaxSelectMusic" && character == 2)
    //    {
    //        //max select music
    //        max.makeMaxSelectMusic();
    //    }

    //    else if(stateName == "AnnaToParkMusic1" && character == 1)
    //    {
    //        //anna to park
    //        if (character == 1) musicSelected = 1;
    //        anna.leadAnnaToPark();
    //    }

    //    else if (stateName == "AnnaToParkMusic2" && character == 1)
    //    {
    //        //anna to park
    //        if (character == 1) musicSelected = 2;
    //        anna.leadAnnaToPark();
    //    }

    //    else if(stateName == "AnnaPickGift" && character == 1)
    //    {
    //        //pick gift
    //        anna.makeAnnaSelectGift();
    //    }

    //    else if(stateName == "StartTalkingRose")
    //    {
    //        //show up board and start talking
    //        if (character == 2) max.makeMaxTalk(0);
    //        else if (character == 1) anna.makeAnnaTalk(musicSelected, 0);
    //    }

    //    else if (stateName == "StartTalkingCoin")
    //    {
    //        //show up board and start talking
    //        if (character == 2) max.makeMaxTalk(1);
    //        else if (character == 1) anna.makeAnnaTalk(musicSelected, 1);
    //    }
    //}

    public void FlowerShopFound()
    {
        if (character == 0 && client.IsConnected)
        {
            isAnnaText.SetActive(true);
            character = 1;
            anna.enabled = true;
            client.ClientSendMessage("!AnnaFlowerShopReached");
        }
    }

    public void HouseFound()
    {
        if (character == 0 && client.IsConnected)
        {
            isMaxText.SetActive(true);
            character = 2;
            max.enabled = true;
            client.ClientSendMessage("!MaxHouseReached");
        }
    }

    public void ParkFound()
    {
        if(character == 2)
        {
            max.stopLeadMaxToPark();
            client.ClientSendMessage("!MaxParkReached");
        }

        else if(character == 1)
        {
            anna.stopLeadAnnaToPark();
            client.ClientSendMessage("!AnnaParkReached");
        }
    }

    public void MusicSelected(int musicNum)
    {
        if(musicNum == 1)
            client.ClientSendMessage("!MaxMusicSelectedMusic1");
        else if (musicNum == 2)
            client.ClientSendMessage("!MaxMusicSelectedMusic2");
    }

    public void GiftGiven(int giftNum)
    {
        if(giftNum == 0)
            client.ClientSendMessage("!AnnaGiftGivenRose");
        else if (giftNum == 1)
            client.ClientSendMessage("!AnnaGiftGivenCoin");
    }
}
