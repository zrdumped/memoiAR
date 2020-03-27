using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Controller : MonoBehaviour
{
    // Start is called before the first frame update
    private ObjectManager2 om2;
    //private HintManager hm;
    void Start()
    {
        om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();
        //hm = GameObject.FindGameObjectWithTag("Client").GetComponent<HintManager>();

        //hm.InputNewWords("", "Go to the flowershop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransitToSummer()
    {
        StartCoroutine(om2.ChangeToSummer());
    }

    public void TransitToAutumn()
    {
        StartCoroutine(om2.ChangeToAutumn());
    }

    public void TransitToWinter()
    {
        StartCoroutine(om2.ChangeToWinter());
    }
}
