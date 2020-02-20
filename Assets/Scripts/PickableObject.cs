using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public enum ObjectType { Flower, Basket, Node, MusicSheet, RoseOnGround};
    public ObjectType type;

    private ObjectManager om;

    private ClientStateController csc;

    [Header("FLower")]
    public int flowerType;
    [Header("Basket")]
    public int basketType;
    public List<GameObject> flowerSlots;
    private List<int> flowerNums = new List<int> { 0, 0, 0};
    [Header("Node")]
    public int nodeType;
    [Header("MusicSheet")]
    public List<GameObject> musicSheets;
    public ParticleSystem musicNodeParticleSystem;
    private int musicNum = 0;
    private List<int> musicSelected = new List<int> { -1, -1, -1};

    [Header("RoseOnGround")]
    public int roseNum;
    private int pickupRoseNum = 0;

    private PickHand hand;

    // Start is called before the first frame update
    void Start()
    {
        om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
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

        if (type == ObjectType.Flower && hand.holdingObj == null)
        {
            //pick up the rose
            flowerType = Random.Range(0, om.flowers.Count);
            hand.holdStaff(Instantiate(om.flowers[flowerType]));
        }

        else if(type == ObjectType.Basket && hand.holdingObj != null && hand.holdingObj.GetComponent<PickableObject>().type == ObjectType.Flower){
            if(hand.holdingObj.GetComponent<PickableObject>().flowerType == basketType)
            {
                if (flowerNums[basketType] < 3)
                    flowerSlots[flowerNums[basketType]].SetActive(true);
                flowerNums[basketType]++;
                hand.releaseStaff();
            }
        }

        else if(type == ObjectType.Node && (hand.holdingObj == null 
            || (hand.holdingObj.GetComponent<PickableObject>().type == ObjectType.Node 
            && hand.holdingObj.GetComponent<PickableObject>().nodeType != nodeType)))
        {
            //pick up the node
            hand.releaseStaff();
            hand.holdStaff(Instantiate(this.gameObject));
            om.audioSource.clip = om.nodeMusic[nodeType];
            om.audioSource.Play();
        }

        else if(type == ObjectType.MusicSheet && hand.holdingObj != null && hand.holdingObj.GetComponent<PickableObject>().type == ObjectType.Node)
        {
            if (musicNum > 3) return;
            musicSheets[musicNum].SetActive(true);
            musicSelected[musicNum] = hand.holdingObj.GetComponent<PickableObject>().nodeType;
            musicNum++;
            om.audioSource.Stop();
            if (musicNum == 3)
            {
                musicNodeParticleSystem.Play();
                StartCoroutine(PlayComposedMusic());
            }

            hand.releaseStaff();
        }

        else if(type == ObjectType.RoseOnGround)
        {
            om.rosesInTheHand[hand.roseNum].SetActive(true);
            hand.roseNum++;
            csc.FlowerPicked(roseNum);
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayComposedMusic()
    {
        for (int i = 0; i < 3; i++)
        {
            om.audioSource.clip = om.nodeMusic[musicSelected[i]];
            om.audioSource.Play();
            while (om.audioSource.isPlaying)
                yield return new WaitForFixedUpdate();
        }
        musicNodeParticleSystem.Stop();
        csc.MusicComposed();
    }
}
