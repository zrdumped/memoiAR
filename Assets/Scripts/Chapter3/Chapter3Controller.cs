using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chapter3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    //public AudioSource audioSource;
    public GameObject audioSourcePref;
    [Header("Chanting")]
    private HintManager hm;
    private ObjectManager3 om3;
    private ClientStateController csc;
    public ChantingController cc;
    private int chantingNum = 0;
    private bool inside = true;
    private bool isInjail = false;

    private bool duringTransition = false;

    public GameObject jail;
    public GameObject camera;

    public GameObject door;

    private int crowdNum = 6;//on each half axis
    private float crowdDistance = 0.5f;
    private float crowdRange = 0.6f;

    public GameObject crowd;
    public GameObject target;
    public List<GameObject> generatedGrowds;
    public GameObject house;
    private bool onTheWayToR = false;
    public Vector3 housePos;
    private GameObject minObject;
    private bool inCrowd = false;

    public GameObject embracePanel;
    //private float minDistance;

    void Start()
    {
#if SHOW_HM
        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();
#endif
        om3 = GameObject.FindGameObjectWithTag("ObjectManager3").GetComponent<ObjectManager3>();

        duringTransition = true;

        //GenerateCrowd();
    }

    // Update is called once per frame
    void Update()
    {
        if (onTheWayToR)
        {
            float MinDistance = 100;
            for (int i = 0; i < generatedGrowds.Count; i++)
            {
                Vector3 pos1 = generatedGrowds[i].transform.position;
                //Debug.Log(pos1);
                Vector3 pos2 = target.transform.position;
                pos2.y = pos1.y;

                generatedGrowds[i].transform.LookAt(pos2);

                float proportion = 1 - Vector3.Distance(pos1, pos2) / crowdRange;
                om3.updateCrowd(proportion, generatedGrowds[i], target);

                MinDistance = Mathf.Min(MinDistance, Vector3.Distance(pos1, pos2));
            }
            om3.UpdateCrowdEffects(-1);

        }
    }

    public void JailFound()
    {
        if (csc.isAnna())
        {
            om3.destroyCrowd();

            for (int i = 0; i < generatedGrowds.Count; i++)
            {
                Destroy(generatedGrowds[i]);
            }
            csc.AnnaArriveJail();
            //StartCoroutine(FirstChanting());
        }
        else
        {
            isInjail = true;
            StartCoroutine(MaxWriting());
            csc.MaxArriveJail();
        }
    }

    public IEnumerator MaxEndWrite()
    {
#if SHOW_HM
        hm.InputNewWords("You try to imagine how she could possibly find your letter.", "");
#endif
        yield return null;
        //yield return new WaitForSeconds(3);

        //StartCoroutine(FirstChanting());

        //yield return new WaitForSeconds(10);

        //ChantingCompleted();

        //yield return new WaitForSeconds(10);

        //ChantingCompleted();

        //yield return new WaitForSeconds(10);

        //ChantingCompleted();
    }

    public IEnumerator MaxWriting()
    {
#if SHOW_HM
        hm.InputNewWords("Minutes turn into hours. You can hear crying and terrified whispers.", "");
#endif

        changeSound(om3.jailSound, true);

        yield return new WaitForSeconds(3);
#if SHOW_HM
        hm.InputNewWords("There’s something by the wall.", "Touch the paper");
#endif

    }

    public void ChantingCompleted()
    {
        chantingNum++;
        if (chantingNum == 1)
            csc.FirstChantEnd();
        //StartCoroutine(SecondChanting());
        else if (chantingNum == 2)
            csc.SecondChantEnd();
        //StartCoroutine(ThirdChanting());
        else if (chantingNum == 3)
            csc.FinalChantEnd();
            //StartCoroutine(FinalChanting());
    }

    public IEnumerator FirstChanting()
    {
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("A crowd gathers outside a building on Ronsenstrasse.", "");
        else
            hm.InputNewWords("Something is happening outside", "");
#endif
        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("A chant starts from the front of the crowd.", "");
        else
            hm.InputNewWords("There’s a crowd of women outside. Maybe Annaliese is out there.", "Look through the window");
#endif

        changeSound(om3.firstChanting);

        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("", "");
#endif
        if (csc.isAnna())
            cc.StartChanting();
    }

    public IEnumerator SecondChanting()
    {
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("The chants grow louder and spread throughout the crowd.", "");
        else
            hm.InputNewWords("It sounds like Annaliese.", "");
#endif
        changeSound(om3.secondChanting);

        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("More women join you.", "Chant again");
#endif

        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("", "");
        if (csc.isAnna())
            cc.StartChanting();
#endif
    }

    public IEnumerator ThirdChanting()
    {
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("One voice fighting together for your husbands.", "");
        else
            hm.InputNewWords("It is Annaliese! Although you can’t see her you can hear her voice.", "Call out to Annaliese");
#endif
        changeSound(om3.thirdChanting);

        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("They will not take them away from you.", "Chant louder");
#endif

        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("", "");
#endif
        if (csc.isAnna())
            cc.StartChanting();
    }

    public IEnumerator FinalChanting()
    {
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("The street burns with rage. Everyone chants for their husbands. Give us our husbands back!", "");
        else
            hm.InputNewWords("Where is she? You need to see her.", "");
        #endif
        AudioSource group = changeSound(om3.groupChanting);

        yield return new WaitForSeconds(5);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("You chant for what seems like hours. Others rest while newcomers chant. (But you don't give up)", "");
        else
            hm.InputNewWords("They are trying to save you.", "");
#endif
        yield return new WaitForSeconds(5);
#if SHOW_HM
        if (csc.isMax())
            hm.InputNewWords("The chanting goes on for hours.", "");
#endif

        yield return new WaitForSeconds(5);

        group.DOFade(0, 2);

        yield return new WaitForSeconds(2);

        group.Stop();
        changeSound(om3.womenGroupNoise);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("When the chant finally ends, the crowd stays. You won't leave until you get your husbands", "Keep patient");
        else
            hm.InputNewWords("It sounds hopeful, what is happening?", "");
#endif
        yield return new WaitForSeconds(3);


        changeSound(om3.happierSound);
        door.GetComponent<Animator>().SetTrigger("OpenDoor");
        changeSound(om3.doorOpen);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("Finally you hear shouts of joy from the front of the crowd. Men file out.", "Look for Max");
        else
        {
            hm.InputNewWords("They’re letting you go! You can hardly believe it.", "Look for Annaliese");
        }
#endif
        yield return new WaitForSeconds(5);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("You find Max", "");
        else
            hm.InputNewWords("You find Anna", "");
#endif

        //csc.EndChapter3();
        embracePanel.SetActive(true);

        yield return new WaitForSeconds(3);

        if (isMax())
        {
            embracePanel.SetActive(false);
            om3.StartRedo();
#if SHOW_HM
            hm.InputNewWords("Show Anna the letter you wrote for her.", "Spin the paper to flip");
#endif
        }
    }


private AudioSource changeSound(AudioClip clip, bool loop = false)
    {
        GameObject AS = Instantiate(audioSourcePref);
        AS.GetComponent<AudioSource>().clip = clip;
        AS.GetComponent<AudioSource>().loop = loop;
        AS.GetComponent<AudioSource>().Play();
        return AS.GetComponent<AudioSource>();
    }

    public void FlowerShopFound()
    {
        if (duringTransition)
            StartCoroutine(om3.ChangeToStorm());
    }

    public void HouseFound()
    {
        if (duringTransition)
        {
            StartCoroutine(om3.comeBackHome());
            duringTransition = false;
        }
    }

    public void ParkFound()
    {
        if (duringTransition)
            StartCoroutine(om3.ChangeToRain());
    }

    public bool isAnna()
    {
        return csc.isAnna();
    }

    public bool isMax()
    {
        return csc.isMax();
    }

    public void GenerateCrowd()
    {
        generatedGrowds = new List<GameObject>();
        Vector3 pos1 = house.transform.position;
        housePos = house.transform.position;
        for (float i = pos1.x - crowdNum * crowdDistance; i <= pos1.x + crowdNum * crowdDistance; i += crowdDistance)
        {
            for (float j = pos1.z - crowdNum * crowdDistance; j <= pos1.z + crowdNum * crowdDistance; j += crowdDistance)
            {
                if (Mathf.Abs(i - pos1.x) < 0.01 && Mathf.Abs(j - pos1.z) < 0.01)
                    continue;
                GameObject newCrowd = Instantiate(crowd);
                newCrowd.GetComponentInChildren<Renderer>().material = Instantiate(crowd.GetComponentInChildren<Renderer>().sharedMaterial);
                Color c = newCrowd.GetComponentInChildren<Renderer>().material.color;
                c.a = 0;
                newCrowd.GetComponentInChildren<Renderer>().material.color = c;
                newCrowd.transform.position = new Vector3(i, house.transform.position.y, j);
                newCrowd.SetActive(false);
                generatedGrowds.Add(newCrowd);
            }
        }
        onTheWayToR = true;
    }

    public void EndChapter3()
    {
        csc.EndChapter3();
    }
}
