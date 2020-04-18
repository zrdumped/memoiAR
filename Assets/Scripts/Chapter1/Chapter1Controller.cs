using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Controller : MonoBehaviour
{
    private ClientStateController csc;
    // Start is called before the first frame update
    void Start()
    {
        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParkFound()
    {
        csc.ParkFound();
    }

    public void HouseFound()
    {
        csc.HouseFound();
    }

    public void FlowershopFound()
    {
        csc.FlowerShopFound();
    }

    public void FlowerGiven()
    {
        csc.FlowerGiven();
    }
}
