using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientChapter1 : MonoBehaviour
{
    private TCPTestClient c;
    // Start is called before the first frame update
    void Start()
    {
        c = this.GetComponent<TCPTestClient>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getPark()
    {
        c.ClientSendMessage("!Park");
    }

    public void getHouse()
    {
        c.ClientSendMessage("!House");
    }

    public void getFlowerShop()
    {
        c.ClientSendMessage("!FlowerShop");
    }
}
