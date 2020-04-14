using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStateController : MonoBehaviour
{
    private TCPTestServer server;

    private void Start()
    {
        server = this.GetComponent<TCPTestServer>();
    }
    private int state = 0;
    private int chap = 2;

    private int roseNum = 0;
    private int glassNum = 0;

    private bool maxIsWaiting = false;
    private bool annaIsWaiting = false;
    //Chapter 1 Version 1
    //0 - start
    //1 - one reached spot a
    //2 - the other reached spot b
    //3 - max reached park
    //4 - max selected music
    //5 - anna reached park
    //6 - anna gave gift

    //public void SetState(string stateName)
    //{
    //    Debug.Log(stateName + " " + state);
    //    if (state < 2 && (stateName == "AnnaFlowerShopReached") || stateName == ("MaxHouseReached"))
    //    {
    //        state++;
    //        if (state == 2)
    //        {
    //            //send message to lead max to park
    //            server.Broadcast("MaxToPark");
    //        }
    //    }

    //    else if (state == 2 && stateName == "MaxParkReached")
    //    {
    //        state++;
    //        //send message to start music selection
    //        server.Broadcast("MaxSelectMusic");
    //    }

    //    else if (state == 3 && stateName == "MaxMusicSelectedMusic1")
    //    {
    //        state++;
    //        //send message to lead anna to park
    //        server.Broadcast("AnnaToParkMusic1");
    //    }

    //    else if (state == 3 && stateName == "MaxMusicSelectedMusic2")
    //    {
    //        state++;
    //        //send message to lead anna to park
    //        server.Broadcast("AnnaToParkMusic2");
    //    }

    //    else if (state == 4 && stateName == "AnnaParkReached")
    //    {
    //        state++;
    //        //send message to give gift
    //        server.Broadcast("AnnaPickGift");
    //    }

    //    else if (state == 5 && stateName == "AnnaGiftGivenRose")
    //    {
    //        state++;
    //        //send message to make anna and max talk
    //        server.Broadcast("StartTalkingCoin");
    //    }

    //    else if (state == 5 && stateName == "AnnaGiftGivenCoin")
    //    {
    //        state++;
    //        //send message to make anna and max talk
    //        server.Broadcast("StartTalkingRose");
    //    }
    //}

    //Chapter 1 Version 2
    public void SetState(string stateName)
    {
        Debug.Log(stateName + " " + state);
        if (stateName == "ToChapter2")
        {
            chap = 2;
            state = 0;
        }
        if (chap == 1)
        {

            if (state < 2 && stateName == "AnnaFlowerShopReached")
            {
                state++;
                if (state == 2)
                    server.Broadcast("StartTutorial");
            }
            else if (state < 2 && stateName == "MaxHouseReached")
            {
                state++;
                if (state == 2)
                    server.Broadcast("StartTutorial");
            }

            else if (state == 2 && stateName == "MusicComposed")
            {
                state++;
                server.Broadcast("GoToPark");
            }

            //max reaches first
            else if (state == 3 && stateName == "MaxParkReached")
            {
                state++;
                server.Broadcast("MakeMaxWait");
                maxIsWaiting = true;
            }
            //anna reaches later
            else if (state == 3 && stateName == "AnnaParkReached")
            {
                state++;
                server.Broadcast("MakeAnnaWait");
                annaIsWaiting = true;
            }
            else if (state == 4 && ((stateName == "MaxParkReached" && annaIsWaiting) || (stateName == "AnnaParkReached" && maxIsWaiting)))
            {
                annaIsWaiting = false;
                maxIsWaiting = false;
                state++;
                server.Broadcast("PickUpFlower");
            }
            else if (state == 5 && stateName.Contains("PickedUpFlower"))
            {
                roseNum++;
                if (roseNum == 10)
                {
                    state++;
                    server.Broadcast(stateName[stateName.Length - 1] + "GiveFlower");
                }
                else
                {
                    server.Broadcast(stateName[stateName.Length - 1] + "FlowerPicked");
                }
            }
            else if (state == 6 && stateName == "FlowerGiven")
            {
                state++;
                server.Broadcast("FlowerGiven");
            }
            else if (state == 7 && stateName == "EndChapter1")
            {
                state++;
                server.Broadcast("EndChapter1");
            }
        }else if(chap == 2)
        {
            if(state < 2 && stateName == "MaxGetBackHouse")
            {
                state++;
                if(state == 2)
                    server.Broadcast("BothArriveHouse");
            }else if(state < 2 && stateName == "AnnaGetBackHome")
            {
                state++;
                if (state == 2)
                    server.Broadcast("BothArriveHouse");
            }
            else if (state == 4 && stateName == "AnnaPourWater")
            {
                state++;
                server.Broadcast("AnnaPourWater");
            }
            else if (state == 2 && stateName == "AnnaOpenTeabox")
            {
                state++;
                server.Broadcast("AnnaOpenTeabox");
            }else if(state == 3 && stateName == "MaxSawOutside")
            {
                state++;
                server.Broadcast("MaxSawOutside");
            }
            else if(state == 5 && stateName == "OneDrinkTea")
            {
                state ++;
            }
            else if(state == 6 && stateName == "OneDrinkTea")
            {
                state++;
                server.Broadcast("TwoDrinkTea");
            }
            else if (state == 7 && stateName == "AnnaSwipeGlass")
            {
                glassNum++;
                if (glassNum == 4)
                {
                    state++;
                }
                server.Broadcast("AnnaSwipeGlass");
            }
            else if (state == 7 && stateName == "MaxSwipeGlass")
            {
                glassNum++;
                if (glassNum == 4)
                {
                    state++;
                }
                server.Broadcast("MaxSwipeGlass");
            }
            else if (state == 8 && stateName == "AnnaTouchBook")
            {
                state++;
                server.Broadcast("AnnaTouchBook");
            }
        }
    }

}