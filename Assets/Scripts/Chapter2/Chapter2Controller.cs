using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter2Controller : MonoBehaviour
{
    //Crowd passing variables
    //public GameObject crowd;
    public Transform parkTrans;
    public Transform houseTrans;
    private int crowdNum = 6;//on each half axis
    private float crowdDistance = 0.5f;
    private float crowdRange = 0.3f;
    private List<GameObject> generatedGrowds;
    private bool onTheWayToHouse = false;
    private bool inCrowd = false;
    public AudioSource crowdBgAS;
    private float houseParkDistance;
    private Vector3 housePos;
    public bool maxGoOut = false;
    public bool maxGoingBack = false;
    public GameObject target;
    public GameObject crowd;

    //Max House
    //public GameObject teaboxLid;

    //Global variables
    public GameObject ARCamera;
    private ObjectManager2 om2;

    //Transition variables
    private bool duringTransition = true;

    private HintManager hm;
    private ClientStateController csc;

    private Coroutine showingOutside;
    // Start is called before the first frame update
    void Start()
    {
        om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();
        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();

        //hm.InputNewWords("", "Go to the flowershop");
#if SKIP_TRANSITION
        duringTransition = false;
        //onTheWayToHouse = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if SKIP_GOHOME
#else
        if (onTheWayToHouse)
        {
            Vector3 cameraPos = ARCamera.transform.position;
            cameraPos.y = 0;
            float remainingDistance = Vector3.Distance(housePos, cameraPos);
            crowdBgAS.volume = 1 - remainingDistance / houseParkDistance;
            if(remainingDistance < 0.5)
            {
                onTheWayToHouse = false;
                crowdBgAS.volume = 0.1f;

                csc.HouseArrived();

                om2.destroyCrowd();

                return;
            }

            //calculate the min distance
            float MinDistance = 100;
            for (int i = 0; i < generatedGrowds.Count; i++)
            {
                Vector3 pos1 = generatedGrowds[i].transform.position;
                //Debug.Log(pos1);
                Vector3 pos2 = target.transform.position;
                pos2.y = pos1.y;

                generatedGrowds[i].transform.LookAt(pos2);

                float proportion = 1 - Vector3.Distance(pos1, pos2) / crowdRange;
                om2.updateCrowd(proportion, generatedGrowds[i], target);

                MinDistance = Mathf.Min(MinDistance, Vector3.Distance(pos1, pos2));
            }
            om2.UpdateCrowdEffects(1 - MinDistance / crowdRange);
        }
#endif
        if(maxGoOut && isMax())
        {
            Vector3 cameraPos = ARCamera.transform.position;
            cameraPos.y = 0;
            float remainingDistance = Vector3.Distance(housePos, cameraPos);
            if (remainingDistance > 2)
            {
                maxGoOut = false;
                //maxGoingBack = true;
                showingOutside = StartCoroutine(om2.ShowOutside());
                return;
            }

            //calculate the min distance
            float MinDistance = 100;
            for (int i = 0; i < generatedGrowds.Count; i++)
            {
                Vector3 pos1 = generatedGrowds[i].transform.position;
                //Debug.Log(pos1);
                Vector3 pos2 = target.transform.position;
                pos2.y = pos1.y;

                generatedGrowds[i].transform.LookAt(pos2);

                float proportion = 1 - Vector3.Distance(pos1, pos2) / crowdRange;
                om2.updateCrowd(proportion, generatedGrowds[i], target);

                MinDistance = Mathf.Min(MinDistance, Vector3.Distance(pos1, pos2));
            }
            om2.UpdateCrowdEffects(1 - MinDistance / crowdRange);
        }

        if (maxGoingBack)
        {
            Vector3 cameraPos = ARCamera.transform.position;
            cameraPos.y = 0;
            float remainingDistance = Vector3.Distance(housePos, cameraPos);
            if (remainingDistance < 0.5)
            {
                maxGoingBack = false;
                StopCoroutine(showingOutside);
                om2.HideOutside();
                csc.MaxSawOutside();
            }
        }
    }

    public void FlowerShopFound()
    {
        if(duringTransition)
            StartCoroutine(om2.ChangeToSummer());
    }

    public void HouseFound()
    {
        housePos = houseTrans.position;
        housePos.y = 0;
        if (duringTransition)
        {
            StartCoroutine(om2.ChangeToAutumn());
        }
        else if (onTheWayToHouse)
        {
            onTheWayToHouse = false;
            crowdBgAS.volume = 0.1f;
            //teaboxLid.GetComponent<Animator>().SetTrigger("OpenLid");

            om2.destroyCrowd();
            
            csc.HouseArrived();
        }
    }

    public void ParkFound()
    {
        if (duringTransition)
            StartCoroutine(om2.ChangeToWinter());
        duringTransition = false;
#if SKIP_TRANSITION
        hm.InputNewWords("You were your way home from the park when you heard shouting in the distance", "Go back home");
        //flowershopPanel.SetActive(true);
        GenerateCrowds();
#endif
    }

    // - - - -
    public void GenerateCrowds()
    {
        //om2.beforeScene.SetActive(true);
        generatedGrowds = new List<GameObject>();
        Vector3 pos1 = parkTrans.position;
        pos1.y = 0;
        //housePos = houseTrans.position;
        //Vector3 distance = (pos1 - housePos) / (crowdNum + 1);
        houseParkDistance = Vector3.Distance(pos1, housePos);
        //for (int i = 0; i < crowdNum; i++)
        //{
        //    //GameObject newCrowd = Instantiate(crowd);
        //    //newCrowd.transform.position = ;
        //    generatedGrowds.Add(houseTrans.position + distance * (i + 1));
        //}
        for (float i = pos1.x - crowdNum * crowdDistance; i <= pos1.x + crowdNum * crowdDistance; i += crowdDistance)
        {
            for (float j = pos1.z - crowdNum * crowdDistance; j <= pos1.z + crowdNum * crowdDistance; j += crowdDistance)
            {
                if (Mathf.Abs(i - pos1.x) < 0.01 && Mathf.Abs(j - pos1.z) < 0.01)
                    continue;
                GameObject newCrowd = Instantiate(crowd);
                newCrowd.GetComponentInChildren<Renderer>().material = Instantiate(crowd.GetComponentInChildren<Renderer>().sharedMaterial);
                Color c = newCrowd.GetComponentInChildren<Renderer>().material.color;
                c.a = 0;
                newCrowd.GetComponentInChildren<Renderer>().material.color = c;
                newCrowd.transform.position = new Vector3(i, houseTrans.position.y, j);
                newCrowd.SetActive(false);
                generatedGrowds.Add(newCrowd);
            }
        }
        onTheWayToHouse = true;
        crowdBgAS.volume = 0;
        crowdBgAS.Play();
    }

    public bool isAnna()
    {
        return csc.isAnna();
    }

    public bool isMax()
    {
        return csc.isMax();
    }

    public void maxBuyTea()
    {
        maxGoOut = true;
    }

    public void EndChapter2()
    {
        
    }
}
