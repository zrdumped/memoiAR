using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Concept1GoldSpike : MonoBehaviour
{
    public Text networkText;
    public NetworkServer server;
    public NetworkClient client;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void seePlayer1()
    {
        if (server.serverStarted)
        {
            //networkText.text = "Player 1 is in front of you";
            server.SendMessage("Player 1, you are seen by player 2");
        }else if (client.clientStarted)
        {
            //networkText.text = "Player 1 is in front of you";
            client.SendMessage("Player 1, you are seen by player 2");
        }
    }

    public void seePlayer2()
    {
        if (server.serverStarted)
        {
            //networkText.text = "Player 1 is in front of you";
            server.SendMessage("Player 2, you are seen by player 1");
        }
        else if (client.clientStarted)
        {
            //networkText.text = "Player 1 is in front of you";
            client.SendMessage("Player 2, you are seen by player 1");
        }
    }
}
