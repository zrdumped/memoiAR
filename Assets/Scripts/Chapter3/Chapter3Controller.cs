using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chapter3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioSource audioSourceBG;
    [Header("Chanting")]
    private HintManager hm;
    private ObjectManager3 om3;
    private ClientStateController csc;
    public ChantingController cc;
    private int chantingNum = 0;
    private bool inside = true;
    private bool isInjail = false;

    public GameObject jail;
    public GameObject camera;

    public GameObject door;
    void Start()
    {
        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        om3 = GameObject.FindGameObjectWithTag("ObjectManager3").GetComponent<ObjectManager3>();
        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (csc.isMax() && isInjail)
        {
            Vector3 jailPos = jail.transform.position;
            jailPos.y = 0;

            Vector3 camPos = camera.transform.position;
            camPos.y = 0;

            float distance = Vector3.Distance(jailPos, camPos);

            if (inside && distance >= 1)
            {
                inside = false;
                hm.InputNewWords("No one escapes from here.", "");
            }else if(!inside && distance < 1)
            {
                inside = true;
                hm.InputNewWords("", "");
            }
        }
    }

    public void JailFound()
    {
        if (csc.isAnna())
            StartCoroutine(FirstChanting());
        else
        {
            isInjail = true;
            StartCoroutine(MaxWriting());
        }

        audioSourceBG.Play();
    }

    public IEnumerator MaxEndWrite()
    {
        //audioSource.clip = om3.jailSound;
        //audioSource.Play();


        hm.InputNewWords("You try to imagine how she could possibly find your letter.", "");

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
        hm.InputNewWords("Tense minutes turn into hours. Crying and terrified whispers surround you.", "");
        audioSource.clip = om3.jailSound;
        audioSource.Play();

        yield return new WaitForSeconds(3);

        hm.InputNewWords("There’s something by the wall.", "Touch the paper");
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
        if (csc.isAnna())
            hm.InputNewWords("A crowd gathers outside the building holding the Jews.", "");
        else
            hm.InputNewWords("Something is happening outside", "");

        yield return new WaitForSeconds(3);
        if (csc.isAnna())
            hm.InputNewWords("A chant starts from the front of the crowd.", "");
        else
            hm.InputNewWords("A crowd of women? One man spots his wife. Maybe Anna is out there.", "look through the window");

        audioSource.clip = om3.firstChanting;
        audioSource.Play();

        yield return new WaitForSeconds(3);
        if (csc.isAnna())
            hm.InputNewWords("", "");
        if (csc.isAnna())
            cc.StartChanting();
    }

    public IEnumerator SecondChanting()
    {
        if (csc.isAnna())
            hm.InputNewWords("You hear the chant grow and spread through the crowd.", "");
        else
            hm.InputNewWords("It feels like Anna's voice.", "");
        audioSource.clip = om3.secondChanting;
        audioSource.Play();

        yield return new WaitForSeconds(3);
        if (csc.isAnna())
            hm.InputNewWords("More and more women join you.", "Say it again");

        yield return new WaitForSeconds(3);
        if (csc.isAnna())
            hm.InputNewWords("", "");
        if (csc.isAnna())
            cc.StartChanting();
    }

    public IEnumerator ThirdChanting()
    {
        if (csc.isAnna())
            hm.InputNewWords("One voice fighting for one thing: your husbands.", "");
        else
            hm.InputNewWords("Yes, it must be her. although you can't see her, you hear her,", "tell Anna where you are");
        audioSource.clip = om3.thirdChanting;
        audioSource.Play();

        yield return new WaitForSeconds(3);
        if (csc.isAnna())
            hm.InputNewWords("They will not take this from you. ", "Say it out again to the loudest");

        yield return new WaitForSeconds(3);
        if (csc.isAnna())
            hm.InputNewWords("", "");
        if (csc.isAnna())
            cc.StartChanting();
    }

    public IEnumerator FinalChanting()
    {
        if (csc.isAnna())
            hm.InputNewWords("The street burns with rage. Everyone chants for their husbands. Give us our husbands back!", "");
        else
            hm.InputNewWords("Where is Anna! You need to see her.", "");
        audioSource.clip = om3.groupChanting;
        audioSource.Play();

        yield return new WaitForSeconds(5);
        if (csc.isAnna())
            hm.InputNewWords("You chant for what seems like hours. Others rest while newcomers chant. (But you don't give up)", "");
        else
            hm.InputNewWords("They are trying to save you.", "");
        yield return new WaitForSeconds(5);

        if (csc.isMax())
            hm.InputNewWords("The chanting goes on for hours.", "");

        yield return new WaitForSeconds(5);

        audioSource.DOFade(0, 2);

        yield return new WaitForSeconds(2);

        audioSource.Stop();
        audioSource.clip = om3.womenGroupNoise;
        audioSource.Play();
        audioSourceBG.Stop();
        if (csc.isAnna())
            hm.InputNewWords("When the chant finally ends, the crowd stays. You won't leave until you get your husbands", "Keep patient");
        else
            hm.InputNewWords("it sounds hopeful, what is happening?", "");
        yield return new WaitForSeconds(3);

        audioSource.clip = om3.happierSound;
        audioSource.Play(); door.GetComponent<Animator>().SetTrigger("OpenDoor");
        if (csc.isAnna())
            hm.InputNewWords("Finally you hear shouts of joy from the front of the crowd. Men file out.", "Look for Max");
        else
        {

            audioSource.clip = om3.doorOpen;
            audioSource.Play();
            hm.InputNewWords("They’re letting you go! You can hardly believe it.", "Get out to find Anna");
        }
        yield return new WaitForSeconds(5);
        if (csc.isAnna())
            hm.InputNewWords("You find Max", "");
        else
            hm.InputNewWords("You find Anna", "");

        yield return new WaitForSeconds(3);

        om3.endPanel.SetActive(true);
    }
}
