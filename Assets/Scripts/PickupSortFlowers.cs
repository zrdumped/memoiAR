using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSortFlowers : PickupBase
{
    //the first one is the bunch of flowers;
    //2, 3, 4 represents different vases;
    //public List<GameObject> pickableStaff;

    //1, 2, 3 represents different flower
    public List<Sprite> flowersSprite;

    private bool isHolding = false;

    private int FlowerNum;

    private List<int> FlowerNumInVase;

    public List<GameObject> FlowersInVase;

    private void Start()
    {
        GenerateTargets();
        FlowerNumInVase = new List<int> { 0, 0, 0 };
    }

    private void Update()
    {
        TouchDetect();
    }

    protected override void TouchedObject(int objNum)
    {
        //generate flower in hand randomly
        if(objNum == 0 && !isHolding)
        {
            isHolding = true;
            FlowerNum = Random.Range(1, 4);
            showImage(flowersSprite[FlowerNum - 1]);
        }
        //detect putting the flower into the correct vase
        else if(objNum != 0 && isHolding)
        {
            if(FlowerNum == objNum)
            {
                //right
                isHolding = false;
                FlowerNumInVase[FlowerNum - 1]++;
                if(FlowerNumInVase[FlowerNum - 1] <= 3)
                {
                    FlowersInVase[(FlowerNum - 1) * 3 + FlowerNumInVase[FlowerNum - 1] - 1].SetActive(true);
                }
                hideImage();
            }
            else
            {
                //wrong, do nothing now
            }
        }
    }
}
