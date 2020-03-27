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

    private int chapNum = 0;

    private TCPTestClient client;
    private AnnaController anna;
    private AnnaController2 anna2;
    private MaxController max;
    private MaxController2 max2;

    private bool newState = false;
    private string newStateName = "";

    public GameObject isMaxText, isAnnaText;

    private ObjectManager om;
    private ObjectManager2 om2;
    private GameManager gm;
    private HintManager hm;

    private Chapter0Controller c0c;

    public GameObject chapter0Panel;
    public GameObject chapter1Panel;
    public GameObject chapter2Panel;

    //1 || 2
    private int musicSelected = 0;

    private void Start()
    {
        client = this.GetComponent<TCPTestClient>();
        gm = GameObject.FindGameObjectWithTag("Client").GetComponent<GameManager>();
        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        //anna = GameObject.FindGameObjectWithTag("Chap1Client").GetComponent<AnnaController>();
        //max = GameObject.FindGameObjectWithTag("Chap1Client").GetComponent<MaxController>();

        //om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();

        chapter1Panel.SetActive(false);
        chapter2Panel.SetActive(false);
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
        //om.hideButtons();
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
            om.flowerShopPanel.SetActive(false);
            //isAnnaText.SetActive(true);
            character = 1;
            anna.enabled = true;
            client.ClientSendMessage("!AnnaFlowerShopReached");
        }
    }

    public void HouseFound()
    {
        if (character == 0 && client.IsConnected)
        {
            om.housePanel.SetActive(false);
            //isMaxText.SetActive(true);
            character = 2;
            max.enabled = true;
            client.ClientSendMessage("!MaxHouseReached");
        }
    }

    public void ParkFound()
    {
        if (character == 2 && max.maxIsReadyToPark())
        {
            om.parkPanel.SetActive(false);
            om.flyAndOpenViolinCase();
            max.stopLeadMaxToPark();
            client.ClientSendMessage("!MaxParkReached");
        }

        else if(character == 1 && anna.annaIsReadyToPark())
        {
            om.parkPanel.SetActive(false);
            anna.stopLeadAnnaToPark();
            client.ClientSendMessage("!AnnaParkReached");
        }
    }

    public void EndChapter1()
    {
        client.ClientSendMessage("!EndChapter1");
    }

    public void ConnectToServer(int signal)
    {
        client.ConnectToTcpServer();
        if (signal == 1)
        {
            isAnnaText.SetActive(true);
            //om.flowerShopPanel.SetActive(true);
        }
        else if(signal == 2)
        {
            isMaxText.SetActive(true);
            //om.housePanel.SetActive(true);
        }
        chapNum = 0;
        chapter0Panel.SetActive(true);
        gm.SwitchScene("Chapter0");
    }

    public void SetupAfterSceneLoaded()
    {
        if (chapNum == 0)
        {
            //anna = GameObject.FindGameObjectWithTag("Chap0Client").GetComponent<AnnaController>();
            //max = GameObject.FindGameObjectWithTag("Chap0Client").GetComponent<MaxController>();
            //om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
            c0c = GameObject.FindGameObjectWithTag("Chap0Client").GetComponent<Chapter0Controller>();
            hm.InputNewWords("Put on your favourite accessory to wear", "");
            hm.enableButton();
            if (isAnnaText.activeSelf)
            {
                c0c.setUpAsAnna();
            }else if (isMaxText.activeSelf)
            {
                c0c.setUpAsMax();
            }
            chapter0Panel.SetActive(false);
        }
        else if(chapNum == 1)
        {
            anna = GameObject.FindGameObjectWithTag("Chap1Client").GetComponent<AnnaController>();
            max = GameObject.FindGameObjectWithTag("Chap1Client").GetComponent<MaxController>();
            om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
            if (isAnnaText.activeSelf)
            {
                om.flowerShopPanel.SetActive(true);
                hm.InputNewWords("Now you are Annaliese. Bring the Rose to the Flowershop.", "");
            }
            else if (isMaxText.activeSelf)
            {
                om.housePanel.SetActive(true);
                hm.InputNewWords("Now you are Max. Bring your violin home to 4076 Auguststrasse.", "");
            }
            hm.disableButton();

            chapter1Panel.SetActive(false);
        }
        else if(chapNum == 2)
        {
            anna2 = GameObject.FindGameObjectWithTag("Chap2Client").GetComponent<AnnaController2>();
            max2 = GameObject.FindGameObjectWithTag("Chap2Client").GetComponent<MaxController2>();
            om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();

            hm.disableButton();

            chapter2Panel.SetActive(false);
        }
    }

    public void Chapter0Ended()
    {
        chapter2Panel.SetActive(true);
        chapNum = 2;
        gm.SwitchScene("Chapter2");
    }
}
