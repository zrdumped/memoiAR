using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCompose : PickupBase
{
    public List<Sprite> nodesSprite;

    private bool isHolding = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTargets();
    }

    // Update is called once per frame
    void Update()
    {
        TouchDetect();
    }

    //protected override void TouchedObject(int objNum)
    //{
    //    //get nodes
    //    if (objNum > 0 && !isHolding)
    //    {
    //        isHolding = true;
    //        showImage(flowersSprite[FlowerNum - 1]);
    //    }
    //    //detect putting the flower into the correct vase
    //    else if (objNum != 0 && isHolding)
    //    {
    //        if (FlowerNum == objNum)
    //        {
    //            //right
    //            isHolding = false;
    //            FlowerNumInVase[FlowerNum - 1]++;
    //            if (FlowerNumInVase[FlowerNum - 1] <= 3)
    //            {
    //                FlowersInVase[(FlowerNum - 1) * 3 + FlowerNumInVase[FlowerNum - 1] - 1].SetActive(true);
    //            }
    //            hideImage();
    //        }
    //        else
    //        {
    //            //wrong, do nothing now
    //        }
    //    }
    //}
}
