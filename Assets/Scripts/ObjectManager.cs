using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> flowers;

    public List<AudioClip> nodeMusic;

    public AudioSource audioSource;

    public ClientStateController csc;

    public List<GameObject> hiddenThingsInFlowerShop, hiddenThingsInHouse, rosesOnTheGround;

    private void Start()
    {
        foreach(GameObject go in hiddenThingsInFlowerShop)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in hiddenThingsInHouse)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in rosesOnTheGround)
        {
            go.SetActive(false);
        }
    }

    public void StartTutorial()
    {
        foreach (GameObject go in hiddenThingsInFlowerShop)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in hiddenThingsInHouse)
        {
            go.SetActive(true);
        }
    }

    public void StartPickingup()
    {
        foreach (GameObject go in rosesOnTheGround)
        {
            go.SetActive(true);
        }
    }
}
