using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    protected HintManager hm;

    protected ClientStateController csc;

    protected ObjectManager om;

    protected PickHand hand;

    protected GameManager gm;

    // Start is called before the first frame update
    public virtual void Start()
    {
        try {
            csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();
            gm = GameObject.FindGameObjectWithTag("Client").GetComponent<GameManager>();
            hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<PickHand>();
            hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
            om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
        }
        catch
        {
            hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<PickHand>();
            om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
