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
    public GameObject crowdSprite;
    public AudioClip crowdClip;
    private int crowdNum = 2;
    private float crowdRange = 0.5f;
    private List<Vector3> generatedGrowds;
    private int crowdCount = 0;
    private bool onTheWayToHouse = false;
    private AudioSource audioSource;

    //Global variables
    public GameObject ARCamera;
    private ObjectManager2 om2;

    //Transition variables
    private bool duringTransition = true;

    //private HintManager hm;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();
        //hm = GameObject.FindGameObjectWithTag("Client").GetComponent<HintManager>();

        //hm.InputNewWords("", "Go to the flowershop");
        crowdSprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (onTheWayToHouse)
        {
            //Debug.Log(ARCamera.transform.position + " " + generatedGrowds[0] + " " + houseTrans.position + " " + parkTrans.position);
            float minDistance = 100;
            for(int i = 0; i < crowdNum; i++)
            {
                minDistance = Mathf.Min(minDistance, Vector3.Distance(generatedGrowds[i], ARCamera.transform.position));
            }
float proportion;
            if(minDistance < crowdRange)
            {
               proportion = 1.0f - minDistance / crowdRange;
                //Debug.Log(proportion);

            }else{
proportion = 0;
}
                Color curColor = crowdSprite.GetComponent<Image>().color;
                curColor.a = proportion;
                crowdSprite.GetComponent<Image>().color = curColor;
                audioSource.volume = proportion;
        }
    }

    public void TransitToSummer()
    {
        if(duringTransition)
            StartCoroutine(om2.ChangeToSummer());
    }

    public void TransitToAutumn()
    {
        if (duringTransition)
            StartCoroutine(om2.ChangeToAutumn());
    }

    public void TransitToWinter()
    {
        if (duringTransition)
            StartCoroutine(om2.ChangeToWinter());
        duringTransition = false;
    }

    // - - - -
    public void GenerateCrowds()
    {
        generatedGrowds = new List<Vector3>();
        Vector3 distance = (parkTrans.position - houseTrans.position) / (crowdNum + 1);
        for(int i = 0; i < crowdNum; i++)
        {
            //GameObject newCrowd = Instantiate(crowd);
            //newCrowd.transform.position = ;
            generatedGrowds.Add(houseTrans.position + distance * (i + 1));
        }
        onTheWayToHouse = true;
        crowdSprite.SetActive(true);
        audioSource.clip = crowdClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void DestroyCrowds()
    {
        //for(int i = crowdNum - 1; i >= 0; i--)
        //{
        //    Destroy(generatedGrowds[i]);
        //}
        crowdSprite.SetActive(false);
        onTheWayToHouse = false;
        audioSource.loop = false;
        audioSource.Stop();
    }

}
