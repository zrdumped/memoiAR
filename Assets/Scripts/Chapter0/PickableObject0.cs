using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject0 : MonoBehaviour
{
    public enum ObjectType { Rose, Violin };
    public ObjectType type;

    private PickHand hand;

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

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(this.name + " " + collision.transform.name);
        if (collision.gameObject.name != "ARCameraSlot") return;

        hand = collision.gameObject.GetComponent<PickHand>();

        if (type == ObjectType.Rose || type == ObjectType.Violin)
        {
            csc.Chapter0Ended();
        }
    }
}
