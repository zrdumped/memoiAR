using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    public ClientStateController csc;
    //0 - rose, 1 - coin
    public int giftNum;
    //0.2
    public InputField distance;
    public Text output;
    public GameObject target;

    private bool startCal = false;

    public bool detectedObj = false;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().material.renderQueue = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        output.text = this.transform.position + " " + target.transform.position + " " + startCal + " " + detectedObj;
        if (startCal && detectedObj)
        {
            if (float.Parse(distance.text) > target.transform.position.magnitude)
                given();
        }
    }

    private void given()
    {
        csc.GiftGiven(giftNum);
        this.gameObject.SetActive(false);
        startCal = false;
    }

    public void startCalDistance()
    {
        startCal = true;
    }

    public void GotBoj()
    {
        detectedObj = true;
    }

    public void LostBoj()
    {
        detectedObj = false;
    }
}
