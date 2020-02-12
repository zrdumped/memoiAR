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
        if(stateName == "MaxToPark" && character == 2)
        {
            //max to park
            max.leadMaxToPark();
        }

        else if (stateName == "MaxSelectMusic" && character == 2)
        {
            //max select music
            max.makeMaxSelectMusic();
        }

        else if(stateName == "AnnaToPark" && character == 1)
        {
            //anna to park
            anna.leadAnnaToPark();
        }

        else if(stateName == "AnnaPickGift" && character == 1)
        {
            //pick gift
            anna.makeAnnaSelectGift();
        }

        else if(stateName == "StartTalking")
        {
            //show up board and start talking
            if (character == 2) max.makeMaxTalk();
            else if (character == 1) anna.makeAnnaTalk();
        }
    }

    public void FlowerShopFound()
    {
        if(character == 0)
        {
            character = 1;
            anna.enabled = true;
            client.ClientSendMessage("!AnnaFlowerShopReached");
        }
    }

    public void HouseFound()
    {
        if (character == 0)
        {
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

    public void MusicSelected()
    {
        client.ClientSendMessage("!MaxMusicSelected");
    }

    public void GiftGiven()
    {
        client.ClientSendMessage("!AnnaGiftGiven");
    }
}
