using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> flowers;

    public List<AudioClip> nodeMusic;

    public AudioSource audioSource;

    public ClientStateController csc;

    public List<GameObject> hiddenThingsInFlowerShop, hiddenThingsInHouse, rosesOnTheGround, rosesInTheHand;
    private List<Vector3> rosesInTheHandPos, rosesInTheHandRot;

    private void Start()
    {
        rosesInTheHandPos = new List<Vector3>();
        rosesInTheHandRot = new List<Vector3>();
        foreach (GameObject go in hiddenThingsInFlowerShop)
        {
            go.SetActive(false);
        }
        //disableHouse();
        for (int i = 0; i < rosesOnTheGround.Count; i++)
        {
            rosesOnTheGround[i].GetComponent<PickableObject>().roseNum = i;
            rosesOnTheGround[i].SetActive(false);
        }
        foreach (GameObject go in rosesInTheHand)
        {
            go.SetActive(false);
            rosesInTheHandPos.Add(go.transform.localPosition);
            rosesInTheHandRot.Add(go.transform.localEulerAngles);
        }
    }

    public void disableHouse()
    {
        foreach (GameObject go in hiddenThingsInHouse)
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

    public void RosesFallOnGround()
    {
        StartCoroutine(RosesFallAnimation());
    }

    public void RosesLeave()
    {
        StartCoroutine(RosesLeaveAnimation());
    }

    public void RosesCome()
    {
        StartCoroutine(RosesComeAnimation());
    }

    public IEnumerator RosesFallAnimation()
    {
        foreach (GameObject go in rosesInTheHand)
        {
            go.SetActive(true);
            go.transform.localPosition += new Vector3(0, 0.4f, 0);
        }

        foreach (GameObject go in rosesInTheHand)
        {
            go.transform.DOLocalMove(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-1, -1.5f), Random.Range(-0.1f, 0.1f)), 3);
            go.transform.DOLocalRotate(new Vector3(go.transform.eulerAngles.x, go.transform.eulerAngles.y, go.transform.eulerAngles.z + Random.Range(-30, 30)), 3);
        }

        yield return new WaitForSeconds(3);

        for (int i = 0; i < rosesInTheHand.Count; i++)
        {
            rosesInTheHand[i].SetActive(false);
            rosesInTheHand[i].transform.localPosition = rosesInTheHandPos[i];
            rosesInTheHand[i].transform.localEulerAngles = rosesInTheHandRot[i];
        }

        StartPickingup();
    }

    public IEnumerator RosesLeaveAnimation()
    {
        foreach (GameObject go in rosesInTheHand)
        {
            if (go.activeSelf)
            {
                go.transform.DOLocalMove(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-1f, -1.5f)), 3);
                go.transform.DOLocalRotate(new Vector3(go.transform.eulerAngles.x, go.transform.eulerAngles.y, go.transform.eulerAngles.z + Random.Range(-30, 30)), 3);
            }
        }
        yield return new WaitForSeconds(3);
        foreach (GameObject go in rosesInTheHand)
        {
            if (go.activeSelf)
            {
                go.SetActive(false);
            }
        }
    }

    public IEnumerator RosesComeAnimation()
    {
        for(int i = 0; i < rosesInTheHand.Count; i++)
        {
            GameObject go = rosesInTheHand[i];
            if (!go.activeSelf)
            {
                go.transform.localPosition += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(1f, 1.5f));
                go.transform.localEulerAngles += new Vector3(go.transform.eulerAngles.x, go.transform.eulerAngles.y, go.transform.eulerAngles.z + Random.Range(-30, 30));
                go.SetActive(true);
                go.transform.DOLocalMove(rosesInTheHandPos[i], 3);
                go.transform.DOLocalRotate(rosesInTheHandRot[i], 3);
            }
        }

        yield return new WaitForSeconds(3);
    }
}
