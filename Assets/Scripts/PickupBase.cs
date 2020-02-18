using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupBase : MonoBehaviour
{
    // Start is called before the first frame update
    protected GameObject targetCamera;
    protected float acceptableDistance;
    protected InputField disInput;
    public GameObject screenImage;

    public List<GameObject> targetStaff;



    protected void GenerateTargets()
    {
        targetCamera = GameObject.FindGameObjectWithTag("MainCamera");
        disInput = GameObject.FindGameObjectWithTag("DistanceInputField").GetComponent<InputField>();
        return;
    }

    protected virtual void TouchedObject(int objNum)
    {
        return;
    }

    protected virtual void TouchDetect()
    {
        //Debug.Log(targetCamera.name);
        for(int i = 0; i < targetStaff.Count; i++)
        {
            if (float.Parse(disInput.text) > Vector3.Distance(targetCamera.transform.position, targetStaff[i].transform.position))
                TouchedObject(i);
        }
    }

    protected virtual void showImage(Sprite shown)
    {
        screenImage.GetComponent<Image>().sprite = shown;
        screenImage.SetActive(true);
    }

    protected virtual void hideImage()
    {
        screenImage.SetActive(false);
    }
}
