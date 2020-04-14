using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWrite : MonoBehaviour
{
    public GameObject Trail;
    private GameObject hand;
    public GameObject paper;
    private bool startWriting = false;
    // Start is called before the first frame update
    void Start()
    {
        //paper = new Plane(Camera.main.transform.forward * -1, transform.position);
        paper.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
      || Input.GetMouseButtonDown(0))
        {
            hand = Instantiate(Trail);
            Ray handRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(handRay, out hit) && hit.transform.name == paper.name)
                hand.transform.position = hit.point;
            hand.GetComponent<TrailRenderer>().enabled = true;
            startWriting = true;
        }
        else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
          || Input.GetMouseButton(0)) && startWriting)
        {
            Ray handRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(handRay, out hit) && hit.transform.name == paper.name)
                hand.transform.position = hit.point;
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
 || Input.GetMouseButtonUp(0))
        {
            startWriting = false;
        }
    }
}
