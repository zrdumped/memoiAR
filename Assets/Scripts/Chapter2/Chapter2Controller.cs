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
    private int crowdNum = 2;
    private float crowdRange = 0.5f;
    private List<Vector3> generatedGrowds;
    private bool onTheWayToHouse = false;
    private bool inCrowd = false;
    public AudioSource crowdBgAS;
    private float houseParkDistance;
    private Vector3 housePos;

    //Max House
    //public GameObject teaboxLid;

    //Global variables
    public GameObject ARCamera;
    private ObjectManager2 om2;

    //Transition variables
    private bool duringTransition = true;

    private HintManager hm;
    private ClientStateController csc;
    // Start is called before the first frame update
    void Start()
    {
        om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();
        hm = GameObject.FindGameObjectWithTag("Client").GetComponent<HintManager>();
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
                crowdBgAS.volume = 0.2f;

                csc.HouseArrived();

                om2.destroyCrowd();

                return;
            }

            //calculate the min distance
            float minDistance = 100;
            for (int i = 0; i < crowdNum; i++)
            {
                Vector3 pos1 = generatedGrowds[i];
                pos1.y = 0;
                Vector3 pos2 = ARCamera.transform.position;
                pos2.y = 0;
                minDistance = Mathf.Min(minDistance, Vector3.Distance(pos1, pos2));
            }
            float proportion = 1 - minDistance / crowdRange;
            if(proportion > 0 && !inCrowd)
            {
                inCrowd = true;
                om2.initCrowd();
                om2.updateCrowd(proportion);
            }else if(proportion > 0 && inCrowd)
            {
                om2.updateCrowd(proportion);
            }else if (proportion <= 0 && inCrowd)
            {
                inCrowd = false;
                om2.destroyCrowd();
            }
        }
#endif
    }

    public void FlowerShopFound()
    {
        if(duringTransition)
            StartCoroutine(om2.ChangeToSummer());
    }

    public void HouseFound()
    {
        if (duringTransition)
            StartCoroutine(om2.ChangeToAutumn());
        else if (onTheWayToHouse)
        {
            onTheWayToHouse = false;
            crowdBgAS.volume = 0.2f;
            //teaboxLid.GetComponent<Animator>().SetTrigger("OpenLid");

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
        om2.beforeScene.SetActive(true);
        generatedGrowds = new List<Vector3>();
        Vector3 pos1 = parkTrans.position;
        pos1.y = 0;
        housePos = houseTrans.position;
        housePos.y = 0;
        Vector3 distance = (pos1 - housePos) / (crowdNum + 1);
        houseParkDistance = Vector3.Distance(pos1, housePos);
        for (int i = 0; i < crowdNum; i++)
        {
            //GameObject newCrowd = Instantiate(crowd);
            //newCrowd.transform.position = ;
            generatedGrowds.Add(houseTrans.position + distance * (i + 1));
        }
        onTheWayToHouse = true;
        crowdBgAS.volume = 0;
        crowdBgAS.Play();
    }
}
