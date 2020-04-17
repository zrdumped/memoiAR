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

    public int chapNum = 0;

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
    public GameObject loadingPanel;

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

    public void SetupAfterSceneLoaded()
    {
        loadingPanel.SetActive(false);
        if (chapNum == 0)
        {
            //anna = GameObject.FindGameObjectWithTag("Chap0Client").GetComponent<AnnaController>();
            //max = GameObject.FindGameObjectWithTag("Chap0Client").GetComponent<MaxController>();
            //om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
            c0c = GameObject.FindGameObjectWithTag("Chap0Client").GetComponent<Chapter0Controller>();
            hm.InputNewWords("Put on your favourite accessory to wear", "");
            hm.enableButton();
            hm.changeBackground(0);
            if (isAnna())
            {
                c0c.setUpAsAnna();
            }
            else if (isMax())
            {
                c0c.setUpAsMax();
            }
            //chapter0Panel.SetActive(false);
        }
        else if (chapNum == 1)
        {
            anna = GameObject.FindGameObjectWithTag("Chap1Client").GetComponent<AnnaController>();
            max = GameObject.FindGameObjectWithTag("Chap1Client").GetComponent<MaxController>();
            om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
            hm.changeBackground(1);
            if (isAnna())
            {
                om.flowerShopPanel.SetActive(true);
                hm.InputNewWords("Now you are Annaliese. Bring the Rose to the Flowershop.", "");
            }
            else if (isMax())
            {
                om.housePanel.SetActive(true);
                hm.InputNewWords("Now you are Max. Bring your violin home to 4076 Auguststrasse.", "");
            }
            hm.disableButton();

            //chapter1Panel.SetActive(false);
        }
        else if (chapNum == 2)
        {
            anna2 = GameObject.FindGameObjectWithTag("Chap2Client").GetComponent<AnnaController2>();
            max2 = GameObject.FindGameObjectWithTag("Chap2Client").GetComponent<MaxController2>();
            om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();
            hm.changeBackground(2);
            hm.disableButton();

            //chapter2Panel.SetActive(false);
        }else if(chapNum == 3)
        {
            hm.changeBackground(3);
            hm.disableButton();
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
        if (chapNum == 1) {
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
            else if (stateName == "FlowerGiven" && character == 1)
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
        else if (chapNum == 2)
        {
            if(stateName == "BothArriveHouse")
            {
                om2.escapeInHouse();
                if (isMax())
                {
                    hm.InputNewWords("Your heart won't stop pounding but you need to see if Annaliese is ok.", "Talk with Annaliese");
                }
                else if (isAnna())
                {
                    hm.InputNewWords("You know Max is trying to be brave. Maybe some tea will calm you both down.", "Get some tea from the Tea Box");
                }
            }else if(stateName == "AnnaOpenTeabox")
            {
                om2.observeTeabox(true);
            }
            else if (stateName == "AnnaPourWater")
            {
                if (isMax())
                {
                    StartCoroutine(om2.pourWater(false));
                    //hm.InputNewWords("Were things always this bad? How will you keep Anna safe?", "Wait for Anna to get Tea");
                }
            }
            else if (stateName == "MaxSawOutside")
            {
                if (isMax())
                {
                    hm.InputNewWords("People watch you in your doorway. It is definitely not safe to be outside", "");
                }
                else
                {
                    hm.InputNewWords("It's still not safe to go outside. You'll have to make it without tea", "Touch the kettle");
                    om2.kettleCouldBeTouched = true;
                }
            }
            else if (stateName == "TwoDrinkTea")
            {
                StartCoroutine(om2.blackOut());
            }
            else if (stateName == "AnnaSwipeGlass")
            {
                if(isMax())
                    om2.swipeGlass(true);
            }
            else if (stateName == "MaxSwipeGlass")
            {
                if (isAnna())
                    om2.swipeGlass(true);

            }
            else if (stateName == "AnnaTouchBook")
            {
                if (isMax())
                    StartCoroutine(om2.placeRose());
            }
            else if (stateName == "MaxWriteOnTheBook")
            {
                if (isAnna())
                    StartCoroutine(om2.writeDone());
            }
        }
    }

    public bool isAnna()
    {
        return character == 1 || isAnnaText.activeSelf;
    }

    public bool isMax()
    {
        return character == 2 || isMaxText.activeSelf;
    }

    //Chapter 0 
    public void Chapter0Ended()
    {
        loadingPanel.SetActive(true);
        chapNum = 2;
        gm.SwitchScene("Chapter2");
    }

    //Chapter 1
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
#if CHAPTER_2_ONLY
        chapNum = 3;
        loadingPanel.SetActive(true);
        gm.SwitchScene("Chapter3");
#else
        chapNum = 0;
        loadingPanel.SetActive(true);
        gm.SwitchScene("Chapter0");
#endif
    }

    //Chapter 2
    public void HouseArrived()
    {
#if SHOW_HM
#if OFFLINE_MODE
        SetState("BothArriveHouse");
#else
        hm.InputNewWords("The door can't block all of the yelling, but you should be safe here.", "");
        if (isAnna())
        {
            client.ClientSendMessage("!AnnaGetBackHome");
        }else if (isMax())
        {
            client.ClientSendMessage("!MaxGetBackHouse");
        }
#endif
#endif
    }


    public void teaboxOpened()
    {
#if OFFLINE_MODE
        SetState("AnnaOpenTeabox");
#else
        client.ClientSendMessage("!AnnaOpenTeabox");
#endif
    }

    public void waterPoured()
    {
#if OFFLINE_MODE
        SetState("AnnaPourWater");
#else
        client.ClientSendMessage("!AnnaPourWater");
#endif
    }

    public void MaxSawOutside()
    {
#if OFFLINE_MODE
        SetState("MaxSawOutside");
#else
        client.ClientSendMessage("!MaxSawOutside");
#endif
    }

    public void OneDrinkTea()
    {
#if OFFLINE_MODE
        SetState("TwoDrinkTea");
#else
        client.ClientSendMessage("!OneDrinkTea");
#endif
    }

    public void SwipeGlass()
    {
#if OFFLINE_MODE
        SetState("TwoDrinkTea");
#else
        if(isAnna())
            client.ClientSendMessage("!AnnaSwipeGlass");
        else
            client.ClientSendMessage("!MaxSwipeGlass");
#endif

    }

    public void PlaceRose()
    {
        client.ClientSendMessage("!AnnaTouchBook");
    }

    public void MaxWriteOnTheBook()
    {
        client.ClientSendMessage("!MaxWriteOnTheBook");
    }
}
