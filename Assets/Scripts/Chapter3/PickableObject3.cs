using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject3 : MonoBehaviour
{
    public enum ObjectType { Teabox, Kettle, Cup, Paper };
    public ObjectType type;

    private ObjectManager3 om3;

    private PickHand hand;
    //    [Header("Teabox")]
    //    public GameObject animatedLid;
    //    [Header("Kettle")]


    // Start is called before the first frame update
    void Start()
    {
        om3 = GameObject.FindGameObjectWithTag("ObjectManager3").GetComponent<ObjectManager3>();
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

        if (type == ObjectType.Paper)
        {
            om3.StartWriting();
        }else if (type == ObjectType.Teabox && hand.holdingObj == null)
        {
            om3.observeTeabox();
        }
        else if (type == ObjectType.Kettle && hand.holdingObj == null)
        {
            hand.holdingObj = om3.observeKettle();
        }
        else if (type == ObjectType.Cup && hand.holdingObj != null && hand.holdingObj.GetComponent<PickableObject3>().type == ObjectType.Kettle)
        {
            hand.releaseStaff();
            StartCoroutine(om3.pourWater());
        }
    }
    }
