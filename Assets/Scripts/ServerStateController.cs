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
        if (state < 2 && stateName == "AnnaFlowerShopReached")
        {
            state++;
            server.Broadcast("AnnaStartTutorial");
        }
        else if (state < 2 && stateName == "MaxHouseReached")
        {
            state++;
            server.Broadcast("MaxStartTutorial");
        }
    }

}