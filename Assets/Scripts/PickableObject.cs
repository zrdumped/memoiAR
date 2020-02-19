using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public enum ObjectType { Flower, Basket, Node, MusicSheet};
    public ObjectType type;

    [Header("FLower")]
    public int flowerType;
    public List<GameObject> flowers; 
    [Header("Basket")]
    public int basketType;
    public List<GameObject> flowerSlots;
    private List<int> flowerNums = new List<int> { 0, 0, 0};
    [Header("Node")]
    public int nodeType;

    private PickHand hand;

    // Start is called before the first frame update
    void Start()
    {
        
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

        if (type == ObjectType.Flower && hand.holdingObj == null)
        {
            //pick up the rose
            flowerType = Random.Range(0, flowers.Count);
            hand.holdStaff(Instantiate(flowers[flowerType]));
        }

        else if(type == ObjectType.Basket && hand.holdingObj.GetComponent<PickableObject>().type == ObjectType.Flower){
            if(hand.holdingObj.GetComponent<PickableObject>().flowerType == basketType)
            {
                if (flowerNums[basketType] < 3)
                    flowerSlots[flowerNums[basketType]].SetActive(true);
                flowerNums[basketType]++;
                hand.releaseStaff();
            }
        }
    }
}
