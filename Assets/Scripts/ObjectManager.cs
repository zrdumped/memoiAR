using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> flowers;

    public List<AudioClip> nodeMusic;

    public AudioSource audioSource;

    public ClientStateController csc;

    public List<int> musicSelected = new List<int> { 1, 2, 3 };
    public List<ParticleSystem> psColor;

    public GameObject violin;

    public GameObject violinCaseHolding, violinCaseTarget;

    public List<GameObject> hiddenThingsInFlowerShop, hiddenThingsInHouse, rosesOnTheGround, rosesInTheHand;
    private List<Vector3> rosesInTheHandPos, rosesInTheHandRot;

    public GameObject promptText;

    public GameObject annaPanel, maxPanel;

    private void Start()
    {
        rosesInTheHandPos = new List<Vector3>();
        rosesInTheHandRot = new List<Vector3>();
        disableFlowerShop();
        disableHouse();
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
        violin.SetActive(false);
        violinCaseHolding.SetActive(false);
        violinCaseTarget.SetActive(false);

        promptText.SetActive(false);
        annaPanel.SetActive(false);
        maxPanel.SetActive(false);
    }

    public void disableHouse()
    {
        foreach (GameObject go in hiddenThingsInHouse)
        {
            go.SetActive(false);
        }
    }

    public void disableFlowerShop()
    {
        foreach (GameObject go in hiddenThingsInFlowerShop)
        {
            go.SetActive(false);
        }
    }

    public void showFlowerInHand()
    {
        foreach (GameObject go in rosesInTheHand)
        {
            go.SetActive(true);
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

    public void showPrompt(string showText = "Oh No! Your flowers!")
    {
        GameObject newText = Instantiate(promptText);
        newText.transform.parent = promptText.transform.parent;
        newText.GetComponent<Text>().text = showText;
        newText.SetActive(true);
        newText.GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-100, 100), Random.Range(-200, 300), 0);
        newText.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, Random.Range(-30, 30));
        newText.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
        StartCoroutine(promptMove(newText));
    }

    private IEnumerator promptMove(GameObject target)
    {
        target.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 3);
        target.GetComponent<RectTransform>().DOLocalRotate(new Vector3(0, 0, Random.Range(-30, 30)) +
            new Vector3(0, 0, Random.Range(-30, 30)), 3);
        target.GetComponent<RectTransform>().DOLocalMove(target.GetComponent<RectTransform>().localPosition + 
            new Vector3(Random.Range(-40, 40), Random.Range(-80, 80), 0), 3);
        yield return new WaitForSeconds(4);
        Destroy(target);
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
        yield return new WaitForSeconds(1);
        maxPanel.SetActive(true);
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

        yield return new WaitForSeconds(4);
        annaPanel.SetActive(true);
    }

    public void flyAndOpenViolinCase()
    {
        violinCaseHolding.transform.parent = violinCaseTarget.transform.parent;
        violinCaseHolding.GetComponent<Animator>().SetTrigger("Open");
        violinCaseHolding.transform.DOLocalMove(violinCaseTarget.transform.localPosition, 3);
        violinCaseHolding.transform.DORotateQuaternion(violinCaseTarget.transform.rotation, 3);
        violinCaseHolding.transform.DOScale(violinCaseTarget.transform.localScale, 3);
    }

    public void OpenViolinCase()
    {
        violinCaseTarget.SetActive(true);
        violinCaseTarget.GetComponent<Animator>().SetTrigger("Open");
    }

    public void StartViolin()
    {
        maxPanel.SetActive(false);
        violin.SetActive(true);
    }
}
