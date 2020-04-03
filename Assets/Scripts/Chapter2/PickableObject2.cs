using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject2 : MonoBehaviour
{
    public enum ObjectType { Teabox, Kettle, Cup, Pieces };
    public ObjectType type;

    private ObjectManager2 om2;

    private PickHand hand;
//    [Header("Teabox")]
//    public GameObject animatedLid;
//    [Header("Kettle")]


    // Start is called before the first frame update
    void Start()
    {
        om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(this.name + " " + collision.transform.name);
        if (collision.gameObject.name != "ARCameraSlot") return;

        hand = collision.gameObject.GetComponent<PickHand>();

        if (type == ObjectType.Teabox && hand.holdingObj == null)
        {
            om2.observeTeabox();
        }else if(type == ObjectType.Kettle && hand.holdingObj == null)
        {
            hand.holdingObj = om2.observeKettle();
        }else if(type == ObjectType.Cup && hand.holdingObj.GetComponent<PickableObject2>().type == ObjectType.Kettle)
        {
            StartCoroutine(om2.pourWater());
            hand.releaseStaff();
        }else if (type == ObjectType.Cup && hand.holdingObj == null)
        {
            StartCoroutine(om2.blackOut());
        }
        else if (type == ObjectType.Pieces)
        {
            StartCoroutine(om2.swipeGlass());
        }
    }
}
