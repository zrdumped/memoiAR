using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWrite : MonoBehaviour
{
    public GameObject Trail;
    private GameObject hand;
    public GameObject paper;
    public GameObject camera;
    public GameObject credit;
    private bool startWriting = false;
    private List<GameObject> trails;
    private bool firstWrite = true;
    private ObjectManager2 om2;
    private ObjectManager3 om3;
    private ClientStateController csc;
    private bool secondWrite = false;
    // Start is called before the first frame update
    void Start()
    {
        //paper = new Plane(Camera.main.transform.forward * -1, transform.position);
        paper.SetActive(true);
        trails = new List<GameObject>();

        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();

        if(csc.chapNum == 2)
            om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();
        else if(csc.chapNum == 3)
            om3 = GameObject.FindGameObjectWithTag("ObjectManager3").GetComponent<ObjectManager3>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.touchCount);
        if (om3 != null)
        {
            if (Input.touchCount == 2 && Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) < 200)
            {
                if (secondWrite)
                {
                    for (int i = trails.Count - 1; i >= 0; i--)
                    {
                        trails[i].layer = LayerMask.NameToLayer("Invisible");
                    }
                    StartCoroutine(om3.FlipPage());
                }
                else
                    om3.EndWrite();
            }
            else if (Input.touchCount == 2)
                return;
        }

       if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            if (firstWrite)
            {
                if (csc.chapNum == 2)
                {
                    om2.maxStartWrite();
                }
                else if (csc.chapNum == 3)
                {
                    om3.MaxWrite();
                }
                firstWrite = false;
            }
            hand = Instantiate(Trail);
            trails.Add(hand);
            var PSmain = hand.GetComponent<ParticleSystem>().main;
            PSmain.customSimulationSpace = camera.transform;
            Ray handRay = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(handRay, out hit) && hit.transform.name == paper.name)
                hand.transform.position = hit.point;
            //hand.GetComponent<TrailRenderer>().enabled = true;
            startWriting = true;
        }
        else if (((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)|| Input.GetMouseButton(0)) && startWriting)
        {
            Ray handRay = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(handRay, out hit) && hit.transform.gameObject.name == paper.transform.name)
            {
                //Vector3 oldLocal = hand.transform.localPosition;
                hand.transform.position = hit.point;
                //hand.transform.localPosition = new Vector3(hand.transform.localPosition.x, hand.transform.localPosition.y, oldLocal.z);
                //bow.transform.position = new Vector3(worldToLocal.x, bow.transform.position.y, bow.transform.position.z);
                //Debug.Log(curVelocity);
            }
        }
        else if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0))
        {
            hand.GetComponent<ParticleSystem>().Stop();
            startWriting = false;
        }
    }

    public void deleteTrails()
    {
        if (csc.chapNum == 2)
        {
            for (int i = trails.Count - 1; i >= 0; i--)
            {
                Destroy(trails[i]);
            }
            paper.SetActive(false);
        }
        else
        {
            for (int i = trails.Count - 1; i >= 0; i--)
            {
                trails[i].layer = LayerMask.NameToLayer("Invisible");
            }
            paper.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void redo()
    {
        for (int i = trails.Count - 1; i >= 0; i--)
        {
            trails[i].layer = LayerMask.NameToLayer("Default");
        }
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
