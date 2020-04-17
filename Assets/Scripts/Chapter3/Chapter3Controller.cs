using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chapter3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    [Header("Chanting")]
    private HintManager hm;
    private ObjectManager3 om3;
    private ClientStateController csc;
    public ChantingController cc;
    private int chantingNum = 0;

    void Start()
    {
        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        om3 = GameObject.FindGameObjectWithTag("ObjectManager3").GetComponent<ObjectManager3>();
        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();

        StartCoroutine(FirstChanting());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JailFound()
    {
        if (csc.isAnna())
            StartCoroutine(FirstChanting());
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
        hm.InputNewWords("A crowd gathers outside the building holding the Jews.", "");

        yield return new WaitForSeconds(3);

        hm.InputNewWords("A chant starts from the front of the crowd.", "");
        audioSource.clip = om3.firstChanting;
        audioSource.Play();

        yield return new WaitForSeconds(3);

        cc.StartChanting();
    }

    public IEnumerator SecondChanting()
    {
        hm.InputNewWords("You hear the chant grow and spread through the crowd.", "");
        audioSource.clip = om3.secondChanting;
        audioSource.Play();

        yield return new WaitForSeconds(3);

        hm.InputNewWords("More and more women join you.", "Say it again");

        yield return new WaitForSeconds(3);

        cc.StartChanting();
    }

    public IEnumerator ThirdChanting()
    {
        hm.InputNewWords("One voice fighting for one thing: your husbands.", "");
        audioSource.clip = om3.thirdChanting;
        audioSource.Play();

        yield return new WaitForSeconds(3);

        hm.InputNewWords("They will not take this from you. ", "Say it out again to the loudest");

        yield return new WaitForSeconds(3);

        cc.StartChanting();
    }

    public IEnumerator FinalChanting()
    {
        hm.InputNewWords("The street burns with rage. Everyone chants for their husbands. Give us our husbands back!", "");
        audioSource.clip = om3.groupChanting;
        audioSource.Play();

        yield return new WaitForSeconds(5);

        hm.InputNewWords("You chant for what seems like hours. Others rest while newcomers chant. (But you don't give up)", "");

        yield return new WaitForSeconds(10);

        audioSource.DOFade(0, 2);

        yield return new WaitForSeconds(2);

        audioSource.Stop();
        audioSource.clip = om3.womenGroupNoise;
        audioSource.Play();
        hm.InputNewWords("When the chant finally ends, the crowd stays. You won't leave until you get your husbands", "Keep patient");

        yield return new WaitForSeconds(3);

        audioSource.clip = om3.happierSound;
        audioSource.Play();
        hm.InputNewWords("Finally you hear shouts of joy from the front of the crowd. Men file out.", "Look for Max");

        yield return new WaitForSeconds(5);

        hm.InputNewWords("You find max", "");

        yield return new WaitForSeconds(3);

        om3.endPanel.SetActive(true);
    }
}
