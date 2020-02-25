using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public enum ObjectType { Flower, Basket, Node, MusicSheet, RoseOnGround, ViolinCase};
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
    public GameObject nodesOnSheet;
    public ParticleSystem musicNodeParticleSystem;
    public ParticleSystem sheetGlowParticleSystem;
    public List<PickableObject> musicSheets;
    public int activeMusicSheetNum = 0;
    public int nodeNum = -1;
    //private int musicNum = 0;

    [Header("RoseOnGround")]
    public int roseNum;
    private int pickupRoseNum = 0;

    private bool readyToConfirmMusic = false;

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
            //if (musicNum > 3) return;
            //pick up the node
            hand.releaseStaff();
            hand.holdStaff(Instantiate(this.gameObject));
            om.audioSource.clip = om.nodeMusic[nodeType];
            om.audioSource.Play();
        }

        else if(type == ObjectType.MusicSheet && hand.holdingObj != null && hand.holdingObj.GetComponent<PickableObject>().type == ObjectType.Node)
        {
            om.audioSource.Stop();
            if (nodeNum == -1)
            {
                musicSheets[0].activeMusicSheetNum++;
                sheetGlowParticleSystem.Play();
            }
            var col = sheetGlowParticleSystem.transform.GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime;
            col.color = hand.holdingObj.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color;

            nodeNum = hand.holdingObj.GetComponent<PickableObject>().nodeType;
            nodesOnSheet.SetActive(true);
            if(musicSheets[0].activeMusicSheetNum == 3)
            {
                //musicNodeParticleSystem.Play();
                StartCoroutine(musicSheets[0].PlayComposedMusic());
                readyToConfirmMusic = true;
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

        else if(type == ObjectType.ViolinCase && readyToConfirmMusic)
        {
            hand.holdStaff(this.gameObject);
            this.gameObject.GetComponent<Collider>().enabled = false;
            csc.MusicComposed();
            om.disableHouse();
        }
    }

    private IEnumerator PlayComposedMusic()
    {
        csc.AskMaxToComfirm();
        for (int i = 0; i < 3; i++)
        {
            musicSheets[i].musicNodeParticleSystem.Play();
            om.audioSource.clip = om.nodeMusic[musicSheets[i].nodeNum];
            om.audioSource.Play();
            while (om.audioSource.isPlaying)
                yield return new WaitForFixedUpdate();
            musicSheets[i].musicNodeParticleSystem.Stop();
        }
    }
}
