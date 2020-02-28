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

    private ObjectManager om;


    //1 || 2
    private int musicSelected = 0;

    private void Start()
    {
        client = this.GetComponent<TCPTestClient>();
        anna = this.GetComponent<AnnaController>();
        max = this.GetComponent<MaxController>();

        om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
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
        if (stateName == "StartTutorial" && character == 1)
        {
            om.StartTutorial();
            anna.startTutorial();
        }
        else if (stateName == "StartTutorial" && character == 2)
        {
            om.StartTutorial();
            max.startTutorial();
        }
        else if (stateName == "GoToPark" && character == 1)
        {
            anna.makeAnnaConfirm();
        }
        else if (stateName == "GoToPark" && character == 2)
        {
            max.leadMaxToPark();
        }
        else if (stateName == "MakeAnnaWait" && character == 1)
        {
            anna.wait();
        }
        else if (stateName == "MakeMaxWait" && character == 2)
        {
            max.wait();
        }
        else if (stateName == "PickUpFlower" && character == 1)
        {
            om.OpenViolinCase();
            om.RosesFallOnGround();
            anna.pickupFlowers();
        }
        else if (stateName == "PickUpFlower" && character == 2)
        {
            om.StartPickingup();
            max.pickupFlowers();
        }
        else if (stateName.Contains("FlowerPicked") && character != 0)
        {
            om.rosesOnTheGround[stateName[0] - (int)'0'].SetActive(false);
        }
        else if (stateName.Contains("GiveFlower") && character == 1)
        {
            om.rosesOnTheGround[stateName[0] - (int)'0'].SetActive(false);
            anna.receiveFlower();
        }
        else if (stateName.Contains("GiveFlower") && character == 2)
        {
            om.rosesOnTheGround[stateName[0] - (int)'0'].SetActive(false);
            max.sendFlower();
        }
        else if(stateName == "FlowerGiven" && character == 1)
        {
            anna.flowerGiven();
            anna.makeAnnaTalk();
            om.RosesCome();
        }
        else if (stateName == "FlowerGiven" && character == 2)
        {
            om.RosesLeave();
            max.makeMaxTalk();
        }
        else if (stateName == "EndChapter1")
        {
            om.stopGame();
        }
    }

    public void hideButtons()
    {
        om.hideButtons();
    }

    public bool isAnna()
    {
        return character == 1;
    }

    public bool isMax()
    {
        return character == 2;
    }


    public void FlowerGiven()
    {
        client.ClientSendMessage("!FlowerGiven");
    }

    public void FlowerPicked(int roseNum)
    {
        client.ClientSendMessage("!PickedUpFlower" + roseNum);
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

    public void MusicComposed()
    {
        client.ClientSendMessage("!MusicComposed");
    }

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
        if (character == 2 && max.maxIsReadyToPark())
        {
            om.flyAndOpenViolinCase();
            max.stopLeadMaxToPark();
            client.ClientSendMessage("!MaxParkReached");
        }

        else if(character == 1 && anna.annaIsReadyToPark())
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

    public void EndChapter1()
    {
        client.ClientSendMessage("!EndChapter1");
    }
}
