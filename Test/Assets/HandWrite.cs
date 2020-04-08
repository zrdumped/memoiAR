using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWrite : MonoBehaviour
{
    public GameObject Trail;
    private GameObject hand;
    public Plane paper;
    // Start is called before the first frame update
    void Start()
    {
        paper = new Plane(Camera.main.transform.forward * -1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
      || Input.GetMouseButtonDown(0))
        {
            hand = Instantiate(Trail);
            Ray handRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            RaycastHit hit;
            if (paper.Raycast(handRay, out distance))
                hand.transform.position = handRay.GetPoint(distance);
            hand.GetComponent<TrailRenderer>().enabled = true;

        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
          || Input.GetMouseButton(0))
        {
            Ray handRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (paper.Raycast(handRay, out distance))
                hand.transform.position = handRay.GetPoint(distance);
        }
    }
}
