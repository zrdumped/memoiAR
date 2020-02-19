using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickHand : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject holdingObj = null;

    public Transform slot;

    public 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void holdStaff(GameObject other)
    {
        holdingObj = other;
        holdingObj.transform.parent = this.transform;
        other.transform.position = slot.position;
        other.transform.rotation = slot.rotation;
        other.transform.localScale = new Vector3(
            slot.localScale.x * other.transform.localScale.x,
            slot.localScale.y * other.transform.localScale.y,
            slot.localScale.z * other.transform.localScale.z);
        //Vector3 srcScale = other.transform.localScale;
        //srcScale.x *= 0.1f;
        //srcScale.y *= 0.1f;
        //srcScale.z *= 0.1f;
        //other.transform.localScale = srcScale;
    }

    public void releaseStaff()
    {
        if (holdingObj == null) return;
        Destroy(holdingObj);
        holdingObj = null;
    }
}
