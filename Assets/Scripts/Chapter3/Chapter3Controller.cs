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
    private float crowdRange = 0.7f;

    public GameObject crowd;
    public GameObject target;
    public List<GameObject> generatedGrowds;
    public GameObject house;
    private bool onTheWayToR = false;
    public Vector3 housePos;
    private GameObject minObject;
    private bool inCrowd = false;
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
            for (int i = 0; i < generatedGrowds.Count; i++)
            {
                Vector3 pos1 = generatedGrowds[i].transform.position;
                //Debug.Log(pos1);
                Vector3 pos2 = target.transform.position;
                pos2.y = pos1.y;

                generatedGrowds[i].transform.LookAt(pos2);

                float proportion = 1 - Vector3.Distance(pos1, pos2) / crowdRange;
                om3.updateCrowd(proportion, minObject, target);
            }

        }
    }

    public void JailFound()
    {
        if (csc.isAnna())
        {
            om3.destroyCrowd(minObject);

            for (int i = 0; i < generatedGrowds.Count; i++)
            {
                Destroy(generatedGrowds[i]);
            }

            StartCoroutine(FirstChanting());
        }
        else
        {
            isInjail = true;
            StartCoroutine(MaxWriting());
        }
    }

    public IEnumerator MaxEndWrite()
    {
#if SHOW_HM
        hm.InputNewWords("You try to imagine how she could possibly find your letter.", "");
#endif

        yield return new WaitForSeconds(3);

        StartCoroutine(FirstChanting());

        yield return new WaitForSeconds(10);

        ChantingCompleted();

        yield return new WaitForSeconds(10);

        ChantingCompleted();

        yield return new WaitForSeconds(10);

        ChantingCompleted();
    }

    public IEnumerator MaxWriting()
    {
#if SHOW_HM
        hm.InputNewWords("Tense minutes turn into hours. Crying and terrified whispers surround you.", "");
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
            StartCoroutine(SecondChanting());
        else if (chantingNum == 2)
            StartCoroutine(ThirdChanting());
        else if (chantingNum == 3)
            StartCoroutine(FinalChanting());
    }

    public IEnumerator FirstChanting()
    {
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("A crowd gathers outside the building holding the Jews.", "");
        else
            hm.InputNewWords("Something is happening outside", "");
#endif
        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("A chant starts from the front of the crowd.", "");
        else
            hm.InputNewWords("A crowd of women? One man spots his wife. Maybe Anna is out there.", "look through the window");
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
            hm.InputNewWords("You hear the chant grow and spread through the crowd.", "");
        else
            hm.InputNewWords("It feels like Anna's voice.", "");
#endif
        changeSound(om3.secondChanting);

        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("More and more women join you.", "Say it again");
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
            hm.InputNewWords("One voice fighting for one thing: your husbands.", "");
        else
            hm.InputNewWords("Yes, it must be her. although you can't see her, you hear her,", "tell Anna where you are");
#endif
        changeSound(om3.thirdChanting);

        yield return new WaitForSeconds(3);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("They will not take this from you. ", "Say it out again to the loudest");
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
            hm.InputNewWords("Where is Anna! You need to see her.", "");
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
            hm.InputNewWords("it sounds hopeful, what is happening?", "");
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
            hm.InputNewWords("They’re letting you go! You can hardly believe it.", "Get out to find Anna");
        }
#endif
        yield return new WaitForSeconds(5);
#if SHOW_HM
        if (csc.isAnna())
            hm.InputNewWords("You find Max", "");
        else
            hm.InputNewWords("You find Anna", "");
#endif
        yield return new WaitForSeconds(3);

        csc.EndChapter3();
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
                GameObject newCrowd = Instantiate(crowd);
                newCrowd.GetComponent<Renderer>().material = Instantiate(crowd.GetComponent<Renderer>().sharedMaterial);
                Color c = newCrowd.GetComponent<Renderer>().material.color;
                c.a = 0;
                newCrowd.GetComponent<Renderer>().material.color = c;
                newCrowd.transform.position = new Vector3(i, house.transform.position.y, j);
                newCrowd.SetActive(false);
                generatedGrowds.Add(newCrowd);
            }
        }
        onTheWayToR = true;
    }
}
