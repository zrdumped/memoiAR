using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWrite : MonoBehaviour
{
    public GameObject Trail;
    private GameObject hand;
    public GameObject paper;
    public GameObject camera;
    private bool startWriting = false;
    private List<GameObject> trails;
    private bool firstWrite = true;
    private ObjectManager2 om2;
    // Start is called before the first frame update
    void Start()
    {
        //paper = new Plane(Camera.main.transform.forward * -1, transform.position);
        paper.SetActive(true);
        trails = new List<GameObject>();

        om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
      || Input.GetMouseButtonDown(0))
        {
            if (firstWrite)
            {
                om2.maxStartWrite();
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
        else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
          || Input.GetMouseButton(0)) && startWriting)
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
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
 || Input.GetMouseButtonUp(0))
        {
            hand.GetComponent<ParticleSystem>().Stop();
            startWriting = false;
        }
    }

    public void deleteTrails()
    {
        for(int i = trails.Count - 1; i >= 0; i--)
        {
            Destroy(trails[i]);
        }
        paper.SetActive(false);
    }
}
