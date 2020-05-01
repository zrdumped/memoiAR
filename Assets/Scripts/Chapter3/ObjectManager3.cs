using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class ObjectManager3 : MonoBehaviour
{
    // Start is called before the first frame update
    private HintManager hm;
    private Chapter3Controller c3c;

    public Material transitionMaterial;

    public GameObject parkPanel;
    public GameObject housePanel;
    public GameObject flowershopPanel;
    public GameObject chapter3Panel;

    public PostProcessVolume volumn;
    public PostProcessProfile normalProfile;

    public GameObject audioSource;

    public AudioClip openAlbum;
    public AudioClip pickUpKettle;
    public AudioClip pourWaterAC;

    [Header("Anna Chants")]
    public AudioClip firstChanting;
    public AudioClip secondChanting;
    public AudioClip thirdChanting;
    public AudioClip groupChanting;
    public AudioClip womenGroupNoise;
    public AudioClip happierSound;

    [Header("Max In Jail")]
    public AudioClip jailSound;
    public AudioClip doorOpen;
    public GameObject paper;
    public GameObject paperOnScreen;
    public GameObject paperOfCredit;
    public GameObject writePanel;
    private bool readyToPinch = false;

    [Header("Light Rain In Park")]
    public GameObject park;
    public GameObject rain;
    public PostProcessProfile rainProfile;
    public GameObject badViolinPhoto;
    public GameObject badViolinPhotoPos;
    public AudioClip rainSound;

    [Header("Heavy Rain In Flowershop")]
    public GameObject flowershop;
    public GameObject storm;
    public PostProcessProfile heavyrainProfile;
    public GameObject badLifePhoto;
    public GameObject badLifePhotoPos;
    public AudioClip stormSound;

    [Header("Teabox")]
    private bool teaboxTouched = false;
    public GameObject teaboxLid;

    [Header("Kettle")]
    public GameObject kettleOnTable;
    public GameObject kettleOnScreen;
    public bool kettleTouched = false;

    [Header("Cup")]
    public List<GameObject> cups;
    public List<Transform> kettlePoses;

    [Header("Way to R")]
    public AudioClip noise;

    [Header("CrowdEffects")]
    public GameObject effectPanel1;
    public GameObject effectPanel2;
    public List<Sprite> effects;
    public List<Texture> crowds;
    public List<AudioClip> clips;
    private int crowdNum = -1;
    private int clipNum = -1;
    private List<AudioSource> curAS;

    void Start()
    {
        curAS = new List<AudioSource>();
        curAS.Add(null);
        curAS.Add(null);
        curAS.Add(null);


        paperOnScreen.SetActive(false);
        writePanel.SetActive(false);
#if SHOW_HM
        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        hm.InputNewWords("Things only get worse. Each day takes you further from normal life.", "Go to Park");
#endif
        c3c = GameObject.FindGameObjectWithTag("Chap3Client").GetComponent<Chapter3Controller>();
        parkPanel.SetActive(true);

        //rain in park
        rain.SetActive(false);

        //storm
        storm.SetActive(false);

        kettleOnScreen.SetActive(false);

        //StartCoroutine(ChangeToRain());

    }

    // Update is called once per frame
    void Update()
    {
        if (readyToPinch)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                readyToPinch = false;
                StartCoroutine(c3c.MaxEndWrite());
            }

        }
    }

    public void test()
    {
        StartCoroutine(test2());
    }
    public IEnumerator test2()
    {
        EndWrite();
        yield return new WaitForSeconds(3);
        StartRedo();
        yield return new WaitForSeconds(3);
        FlipPage();
    }

    public void StartWriting()
    {
        paper.SetActive(false);
        paperOnScreen.SetActive(true);
        writePanel.SetActive(true);
#if SHOW_HM
        hm.InputNewWords("This is your last chance to write to Anna.", "Write something");
#endif
    }

    public void StartRedo()
    {
        //paperOnScreen.SetActive(true);
        paperOnScreen.GetComponent<MeshRenderer>().enabled = true;
        paperOnScreen.GetComponent<HandWrite>().redo();
    }

    public void MaxWrite()
    {
        readyToPinch = true;
#if SHOW_HM
        hm.InputNewWords("She may never see these words, but you mean them.", "Pinch the paper after writing.");
#endif
    }

    public void EndWrite()
    {
        paperOnScreen.GetComponent<HandWrite>().deleteTrails();
        paperOnScreen.GetComponent<MeshRenderer>().enabled = false;
        writePanel.SetActive(false);
        StartCoroutine(c3c.MaxEndWrite());
    }

    public IEnumerator ChangeToRain()
    {
#if SHOW_HM
        if(c3c.isMax())
            hm.InputNewWords("With your violin gone, you looked to Anna for comfort", "go to the flower shop");
        else
            hm.InputNewWords("His dream shattered, you comforted him as much as possible", "go to the flower shop");
#endif
        parkPanel.SetActive(false);

        StartCoroutine(showGradually(park));
        volumn.profile = rainProfile;

        //effects start
        rain.SetActive(true);
        rain.GetComponent<ParticleSystem>().Play();
        AudioSource rainAS = PlayMusic(rainSound);

        badViolinPhoto.SetActive(true);
        badViolinPhoto.transform.DOLocalMove(badViolinPhotoPos.transform.localPosition, 5);
        badViolinPhoto.transform.DOLocalRotate(badViolinPhotoPos.transform.localEulerAngles, 5);

        yield return new WaitForSeconds(8);

        badViolinPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 4);
        PlayMusic(openAlbum);
        volumn.profile = normalProfile;

        //effects end
        rain.GetComponent<ParticleSystem>().Stop();
        rainAS.Stop();

        yield return new WaitForSeconds(8);
        badViolinPhoto.SetActive(false);

        rain.SetActive(false);
        flowershopPanel.SetActive(true);
    }

    public IEnumerator ChangeToStorm()
    {
#if SHOW_HM
        if (c3c.isMax())
            hm.InputNewWords("You joined the men in the factories", "go back home");
        else
            hm.InputNewWords("A country at war has no need for flower shops. You held onto your pressed flowers.", "go back home");
        
#endif
        flowershopPanel.SetActive(false);

        StartCoroutine(showGradually(flowershop));
        volumn.profile = heavyrainProfile;

        //effects start
        storm.SetActive(true);
        storm.GetComponent<ParticleSystem>().Play();
        AudioSource stormAS = PlayMusic(stormSound);

        yield return new WaitForSeconds(1);

        heavyrainProfile.GetSetting<Bloom>().intensity.value = 80;
        DOTween.To(() => 80.0f, x => heavyrainProfile.GetSetting<Bloom>().intensity.value = x, 2.0f, 1);

        badLifePhoto.SetActive(true);
        badLifePhoto.transform.DOLocalMove(badLifePhotoPos.transform.localPosition, 5);
        badLifePhoto.transform.DOLocalRotate(badLifePhotoPos.transform.localEulerAngles, 5);

        yield return new WaitForSeconds(7);

        badLifePhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 5);
        PlayMusic(openAlbum);
        volumn.profile = normalProfile;

        //effects end
        storm.GetComponent<ParticleSystem>().Stop();
        stormAS.Stop();

        yield return new WaitForSeconds(8);
        badLifePhoto.SetActive(false);

        storm.SetActive(false);
        housePanel.SetActive(true);
    }

    public IEnumerator comeBackHome()
    {
        housePanel.SetActive(false);
        chapter3Panel.SetActive(true);
        Color curColor = chapter3Panel.GetComponent<Image>().color;
        curColor.a = 0;
        chapter3Panel.GetComponent<Image>().color = curColor;
        curColor.a = 1;
        chapter3Panel.GetComponent<Image>().DOColor(curColor, 1);
        yield return new WaitForSeconds(2);
        curColor.a = 0;
        chapter3Panel.GetComponent<Image>().DOColor(curColor, 1);
        yield return new WaitForSeconds(1);
        chapter3Panel.SetActive(false);

#if SHOW_HM
        if (c3c.isMax())
            hm.InputNewWords("One day, they came. They herded you into a room on the street of roses, Rosenstrasse.", "Go to Rosenstrasse");
        else if (c3c.isAnna())
            hm.InputNewWords("You’d greet Max after work with tea as usual.", "");
#endif
        yield return new WaitForSeconds(5);
#if SHOW_HM
        if (c3c.isAnna())
            hm.InputNewWords("Max’s shift ended an hour ago. He should be home any minute.", "Touch the tea box");
#endif
    }

    public IEnumerator showGradually(GameObject parent)
    {
        Renderer[] rendererd = parent.GetComponentsInChildren<Renderer>();
        List<Material> materials = new List<Material>();
        foreach (Renderer r in rendererd)
        {
            Material m = Instantiate(transitionMaterial);
            m.SetTexture("_MainTex", r.material.GetTexture("_MainTex"));
            materials.Add(r.material);
            r.material = m;

            m.DOFloat(1, "_Alpha", 2);
        }

        yield return new WaitForSeconds(2);

        int count = 0;
        foreach (Renderer r in rendererd)
        {
            r.material = materials[count];
            count++;
        }
    }

    public AudioSource PlayMusic(AudioClip ac)
    {
        AudioSource newAS = Instantiate(audioSource).GetComponent<AudioSource>() as AudioSource;
        newAS.clip = ac;
        newAS.Play();
        return newAS;
    }

    public void observeTeabox()
    {
        if (teaboxTouched) return;
        if (c3c.isMax()) return;
        teaboxLid.GetComponent<Animator>().SetTrigger("OpenLid");
        teaboxTouched = true;
#if SHOW_HM
        hm.InputNewWords("Empty: looks like it's 'tea' with no tea left again.", "Touch the kettle");
#endif
        DOTween.To(() => 0, x => normalProfile.GetSetting<ColorGrading>().temperature.value = x, -10, 1);
        DOTween.To(() => 0, x => normalProfile.GetSetting<ColorGrading>().postExposure.value = x, -0.6f, 1);
        return;
    }

    public GameObject observeKettle()
    {
        if (!teaboxTouched) return null;
        if (c3c.isMax()) return null;
        if (kettleTouched)
        {
            hm.InputNewWords("", "The kettle is already empty");
            return null;
        }
        PlayMusic(pickUpKettle);
        kettleOnScreen.SetActive(true);
        kettleOnTable.SetActive(false);
        kettleTouched = true;
#if SHOW_HM
        hm.InputNewWords("Tea for two: one for Max and one for you.", "Touch the tea cup to pour water");
#endif
        DOTween.To(() => -10, x => normalProfile.GetSetting<ColorGrading>().temperature.value = x, -20, 1);
        DOTween.To(() => -0.6f, x => normalProfile.GetSetting<ColorGrading>().postExposure.value = x, -1.2f, 1);
        return kettleOnScreen;
    }

    public IEnumerator pourWater()
    {
        if (c3c.isMax()) yield break;
#if SHOW_HM
        hm.InputNewWords("Max would usually be home now.", "");
#endif
        cups[0].GetComponent<BoxCollider>().enabled = false;
        cups[1].GetComponent<BoxCollider>().enabled = false;
        cups[0].transform.GetChild(0).gameObject.SetActive(true);
        cups[1].transform.GetChild(0).gameObject.SetActive(true);
        //cup.GetComponent<MeshCollider>().enabled = true;

        Vector3 originalLocalPostion = kettleOnTable.transform.localPosition;
        Quaternion originalLocalRotation = kettleOnTable.transform.localRotation;

        kettleOnTable.transform.position = kettleOnScreen.transform.position;
        kettleOnTable.transform.rotation = kettleOnScreen.transform.rotation;

        kettleOnScreen.SetActive(false);
        kettleOnTable.GetComponent<BoxCollider>().enabled = false;
        kettleOnTable.SetActive(true);

        for (int i = 0; i < cups.Count; i++)
        {
            //fly in the air and pour water
            kettleOnTable.transform.DOLocalMove(kettlePoses[i].localPosition, 2);
            kettleOnTable.transform.DOLocalRotateQuaternion(kettlePoses[i].localRotation, 2);

            yield return new WaitForSeconds(2);

            Vector3 oldRot = kettlePoses[i].localEulerAngles;
            Vector3 newRot = kettlePoses[i].localEulerAngles + new Vector3(0, -60, 0);
            kettleOnTable.transform.DOLocalRotate(newRot, 2);
            AudioSource pourWaterAS = PlayMusic(pourWaterAC);

            yield return new WaitForSeconds(3);
            pourWaterAS.Stop();
            kettleOnTable.transform.DOLocalRotate(oldRot, 0.5f);

            yield return new WaitForSeconds(2);

        }
        //move back
        kettleOnTable.transform.DOLocalMove(originalLocalPostion, 2);
        kettleOnTable.transform.DOLocalRotateQuaternion(originalLocalRotation, 2);
        yield return new WaitForSeconds(2);
        kettleOnTable.GetComponent<BoxCollider>().enabled = true;

        cups[0].GetComponent<BoxCollider>().enabled = true;
        cups[1].GetComponent<BoxCollider>().enabled = true;
        //cup.GetComponent<MeshCollider>().enabled = false;

#if SHOW_HM
        hm.InputNewWords("It's getting late. Why hasn't Max come home yet?", "");
#endif
        //light change
        DOTween.To(() => -20, x => normalProfile.GetSetting<ColorGrading>().temperature.value = x, -30, 1);
        DOTween.To(() => -1.2f, x => normalProfile.GetSetting<ColorGrading>().postExposure.value = x, -2, 1);
        yield return new WaitForSeconds(3);
        //noise
        AudioSource noiseAS = PlayMusic(noise);
        noiseAS.loop = true;

#if SHOW_HM
        hm.InputNewWords("Something is happening outside.", "Go to Rosenstrasse");
#endif
        c3c.GenerateCrowd();
    }

    private void OnDestroy()
    {
        heavyrainProfile.GetSetting<Bloom>().intensity.value = 2;
        normalProfile.GetSetting<ColorGrading>().temperature.value = 0; //from -30
        normalProfile.GetSetting<ColorGrading>().postExposure.value = 0; //from -2
    }

    public void initCrowd(GameObject crowd)
    {
        crowd.SetActive(true);
        crowdNum++;
        if (crowdNum == 3) crowdNum = 0;
        Color curColor = crowd.GetComponentInChildren<Renderer>().material.color;
        curColor.a = 0;
        crowd.GetComponentInChildren<Renderer>().material.color = curColor;
        crowd.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", crowds[crowdNum]);

        curColor = effectPanel1.GetComponent<Image>().color;
        curColor.a = 0;
        effectPanel1.GetComponent<Image>().color = curColor;
        effectPanel2.GetComponent<Image>().color = curColor;
        effectPanel1.GetComponent<Image>().sprite = effects[0];
        effectPanel2.GetComponent<Image>().sprite = effects[0];
        effectPanel1.SetActive(true);
        effectPanel2.SetActive(true);


    }

    public void updateCrowd(float proportion, GameObject crowd, GameObject target)
    {
        if(proportion < 0)
        {
            if (crowd.activeSelf)
                crowd.SetActive(false);
            return;
        }
        else
        {
            if (!crowd.activeSelf)
            {
                Vector3 pos = crowd.transform.position;
                pos.y = target.transform.position.y;
                crowd.transform.position = pos;
                crowd.SetActive(true);

                crowdNum++;
                if (crowdNum == crowds.Count) crowdNum = 0;
                crowd.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", crowds[crowdNum]);

                //PlayMusic(clips[crowdNum]);
                for(int i = 0; i < 3; i++)
                {
                    if(curAS[i]==null || !curAS[i].isPlaying)
                    {
                        clipNum++;
                        if (clipNum == clips.Count) clipNum = 0;
                        curAS[i] = PlayMusic(clips[clipNum]);
                        break;
                    }
                }
            }
        }

        Color curColor = crowd.GetComponentInChildren<Renderer>().material.color;
        curColor.a = proportion;
        crowd.GetComponentInChildren<Renderer>().material.color = curColor;

        //crowdScreamingAS.volume = proportion;
    }

    public void UpdateCrowdEffects(float proportion)
    {
        if (proportion < 0)
        {
            effectPanel1.SetActive(false);
            effectPanel2.SetActive(false);
            return;
        }
        else
        {
            if (!effectPanel1.activeSelf)
            {
                effectPanel1.SetActive(true);
                effectPanel2.SetActive(true);
            }
        }
        Color curColor;
        if (proportion <= 0.3)
        {
            float scaledProportion = proportion / 0.3f;
            curColor = effectPanel1.GetComponent<Image>().color;
            curColor.a = scaledProportion;
            effectPanel1.GetComponent<Image>().color = curColor;
            effectPanel1.GetComponent<Image>().sprite = effects[0];
            curColor.a = 0;
            effectPanel2.GetComponent<Image>().color = curColor;
            effectPanel2.GetComponent<Image>().sprite = effects[0];
        }
        else if (proportion >= 0.7)
        {
            float scaledProportion = (proportion - 0.7f) / 0.3f;
            curColor = effectPanel1.GetComponent<Image>().color;
            curColor.a = 1 - scaledProportion;
            effectPanel1.GetComponent<Image>().color = curColor;
            effectPanel1.GetComponent<Image>().sprite = effects[1];
            curColor.a = scaledProportion;
            effectPanel2.GetComponent<Image>().color = curColor;
            effectPanel2.GetComponent<Image>().sprite = effects[2];
        }
        else
        {
            float scaledProportion = (proportion - 0.3f) / 0.4f;
            curColor = effectPanel1.GetComponent<Image>().color;
            curColor.a = 1 - scaledProportion;
            effectPanel1.GetComponent<Image>().color = curColor;
            effectPanel1.GetComponent<Image>().sprite = effects[0];
            curColor.a = scaledProportion;
            effectPanel2.GetComponent<Image>().color = curColor;
            effectPanel2.GetComponent<Image>().sprite = effects[1];
        }
    }

    public void destroyCrowd()
    {
        for (int i = 0; i < curAS.Count; i++)
        {
            curAS[i].Stop();
        }
        effectPanel1.SetActive(false);
        effectPanel2.SetActive(false);
    }

    public IEnumerator FlipPage()
    {
        Vector3 rot = paperOnScreen.transform.localEulerAngles;
        rot.y += 180;
        paperOnScreen.transform.DOLocalRotate(rot, 2);
        rot = paperOfCredit.transform.localEulerAngles;
        rot.y += 180;
        paperOfCredit.transform.DOLocalRotate(rot, 2);
        yield return new WaitForSeconds(5);
        c3c.EndChapter3();
    }
}
