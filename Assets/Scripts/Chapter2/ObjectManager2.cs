using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class ObjectManager2 : MonoBehaviour
{
    public PostProcessProfile normalProfile;
    public PostProcessVolume volumn;
    private HintManager hm;
    private Chapter2Controller c2c;
    //public GameObject effectCamera;
    [Header("Summer")]
    public GameObject flare;
    public GameObject vLight1;
    public GameObject vLight2;
    public GameObject weddingPhoto;
    public Transform flarePos1, flarePos2;
    public PostProcessProfile summerProfile;
    private float srcVLight1Intensity, srcVLight2Intensity;
    private float srcVLight1Range, srcVLight2Range;

    [Header("Autumn")]
    public PostProcessProfile autumnProfile;
    public GameObject leaves;
    public GameObject movingPhoto;

    [Header("Winter")]
    public PostProcessProfile winterProfile;
    public GameObject snow;
    public GameObject playingPhoto;
    public FrostEffect fe;

    [Header("Panels")]
    public GameObject flowershopPanel;
    public GameObject housePanel;
    public GameObject parkPanel;
    public GameObject chapter2Panel;

    [Header("CrowdEffects")]
    public GameObject crowdPanel;
    public GameObject effectPanel1;
    public GameObject effectPanel2;
    public List<Sprite> effects;
    public List<Sprite> crowds;
    public AudioSource crowdScreamingAS;

    [Header("Max's House")]
    public GameObject beforeScene;
    public GameObject afterScene;
    public GameObject blackPanel;
    public GameObject teaboxLid;
    public GameObject kettleOnTable;
    public GameObject kettleOnScreen;
    //public GameObject wholeGlass;
    //public GameObject shatteredGlass;
    public GameObject cup;
    public Transform pourKettle;
    private bool teaboxTouched = false;
    private bool kettleTouched = false;
    private bool cupTouched = false;
    private Vector3 originalLocalPostion;
    private Quaternion originalLocalRotation;
    public AudioSource glassShutteredAS;
    public AudioSource messSoundAS;
    private bool glassSwiped = false;
    public GameObject glassPiece1;
    public GameObject glassPiece1_target;
    public GameObject glassPiece2;
    public GameObject glassPiece2_target;
    public GameObject glassCollider;
    public GameObject endPanel;

    // Start is called before the first frame update
    public void Start()
    {
        volumn.profile = normalProfile;

        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        c2c = GameObject.FindGameObjectWithTag("Chap2Client").GetComponent<Chapter2Controller>();
        hm.InputNewWords("", "Go to the flowershop");

        //summer
        flare.SetActive(false);
        vLight1.SetActive(false);
        vLight2.SetActive(false);
        weddingPhoto.SetActive(false);

        srcVLight1Intensity = vLight1.GetComponent<Light>().intensity;
        srcVLight2Intensity = vLight2.GetComponent<Light>().intensity;
        srcVLight1Range = vLight1.GetComponent<Light>().range;
        srcVLight2Range = vLight2.GetComponent<Light>().range;

        //autumn
        leaves.SetActive(false);
        movingPhoto.SetActive(false);

        //winter
        snow.SetActive(false);
        playingPhoto.SetActive(false);
        fe.enabled = false;

        //panels
#if SKIP_TRANSITION
        flowershopPanel.SetActive(false);
#else
        flowershopPanel.SetActive(true);
#endif
        housePanel.SetActive(false);
        parkPanel.SetActive(false);

        //crowd effects
        crowdPanel.SetActive(false);
        effectPanel1.SetActive(false);
        effectPanel2.SetActive(false);

        //max's house
        kettleOnScreen.SetActive(false);
        //shatteredGlass.SetActive(false);
        beforeScene.SetActive(true);
        afterScene.SetActive(false);
        blackPanel.SetActive(false);
    }

    public IEnumerator ChangeToSummer()
    {
        hm.InputNewWords("", "");
        flowershopPanel.SetActive(false);
        flare.SetActive(true);
        vLight1.SetActive(true);
        vLight2.SetActive(true);

        volumn.profile = summerProfile;
        flare.transform.localPosition = flarePos1.localPosition;
        flare.transform.DOLocalMove(flarePos2.localPosition, 5);

        vLight1.GetComponent<Light>().intensity = 0;
        vLight2.GetComponent<Light>().intensity = 0;
        vLight1.GetComponent<Light>().range = 0;
        vLight2.GetComponent<Light>().range = 0;
        DOTween.To(() => vLight1.GetComponent<Light>().intensity, x => vLight1.GetComponent<Light>().intensity = x, srcVLight1Intensity, 2);
        DOTween.To(() => vLight2.GetComponent<Light>().intensity, x => vLight2.GetComponent<Light>().intensity = x, srcVLight2Intensity, 2);
        DOTween.To(() => vLight1.GetComponent<Light>().range, x => vLight1.GetComponent<Light>().range = x, srcVLight1Range, 2);
        DOTween.To(() => vLight2.GetComponent<Light>().range, x => vLight2.GetComponent<Light>().range = x, srcVLight2Range, 2);

        yield return new WaitForSeconds(4);

        DOTween.To(() => vLight1.GetComponent<Light>().intensity, x => vLight1.GetComponent<Light>().intensity = x, 0, 1);
        DOTween.To(() => vLight2.GetComponent<Light>().intensity, x => vLight2.GetComponent<Light>().intensity = x, 0, 1);
        DOTween.To(() => vLight1.GetComponent<Light>().range, x => vLight1.GetComponent<Light>().range = x, 0, 1);
        DOTween.To(() => vLight2.GetComponent<Light>().range, x => vLight2.GetComponent<Light>().range = x, 0, 1);

        yield return new WaitForSeconds(1);

        flare.SetActive(false);
        vLight1.SetActive(false);
        vLight2.SetActive(false);

        volumn.profile = normalProfile;

        weddingPhoto.SetActive(true);
        Color curColor = weddingPhoto.GetComponent<Renderer>().material.color;
        weddingPhoto.GetComponent<Renderer>().material.color = new Color(curColor.r, curColor.g, curColor.b, 0);
        curColor.a = 1;
        weddingPhoto.GetComponent<Renderer>().material.DOColor(curColor, 4);
        //weddingPhoto.transform.DOLocalJump(weddingPhoto.transform.localPosition + new Vector3(0, 0.1f, 0), 0.2f, 5, 5);

        yield return new WaitForSeconds(8);
        weddingPhoto.SetActive(false);

        hm.InputNewWords("", "Go to Max's house");
        housePanel.SetActive(true);
    }

    public IEnumerator ChangeToAutumn()
    {
        housePanel.SetActive(false);
        hm.InputNewWords("", "");
        leaves.SetActive(true);
        leaves.GetComponent<ParticleSystem>().Play();

        volumn.profile = autumnProfile;

        yield return new WaitForSeconds(5);

        //leaves.SetActive(false);
        leaves.GetComponent<ParticleSystem>().Stop();
        volumn.profile = normalProfile;
        movingPhoto.SetActive(true);
        Color curColor = movingPhoto.GetComponent<Renderer>().material.color;
        movingPhoto.GetComponent<Renderer>().material.color = new Color(curColor.r, curColor.g, curColor.b, 0);
        curColor.a = 1;
        movingPhoto.GetComponent<Renderer>().material.DOColor(curColor, 4);

        yield return new WaitForSeconds(8);
        movingPhoto.SetActive(false);
        leaves.SetActive(false);

        hm.InputNewWords("", "Go to the park");
        parkPanel.SetActive(true);
    }

    public IEnumerator ChangeToWinter()
    {
        parkPanel.SetActive(false);
        hm.InputNewWords("", "");
        snow.SetActive(true);
        snow.GetComponent<ParticleSystem>().Play();
        volumn.profile = winterProfile;

        fe.enabled = true;

        DOTween.To(() => fe.FrostAmount, x => fe.FrostAmount = x, 0.25f, 2);

        yield return new WaitForSeconds(4);

        DOTween.To(() => fe.FrostAmount, x => fe.FrostAmount = x, 0, 1);

        yield return new WaitForSeconds(1);

        fe.enabled = false;
        volumn.profile = normalProfile;
        snow.GetComponent<ParticleSystem>().Stop();

        playingPhoto.SetActive(true);
        Color curColor = playingPhoto.GetComponent<Renderer>().material.color;
        playingPhoto.GetComponent<Renderer>().material.color = new Color(curColor.r, curColor.g, curColor.b, 0);
        curColor.a = 1;
        playingPhoto.GetComponent<Renderer>().material.DOColor(curColor, 4);

        yield return new WaitForSeconds(8);
        playingPhoto.SetActive(false);
        snow.SetActive(false);

        chapter2Panel.SetActive(true);
        curColor = chapter2Panel.GetComponent<Image>().color;
        curColor.a = 0;
        chapter2Panel.GetComponent<Image>().color = curColor;
        curColor.a = 1;
        chapter2Panel.GetComponent<Image>().DOColor(curColor, 1);
        yield return new WaitForSeconds(2);
        curColor.a = 0;
        chapter2Panel.GetComponent<Image>().DOColor(curColor, 1);
        yield return new WaitForSeconds(1);
        chapter2Panel.SetActive(false);

        hm.InputNewWords("You were your way home from the park when you heard shouting in the distance", "Go back home");
        //flowershopPanel.SetActive(true);
        c2c.GenerateCrowds();
    }

    public void initCrowd()
    {
        int crownNum = Random.Range(0, 3);
        Color curColor = crowdPanel.GetComponent<Image>().color;
        curColor.a = 0;
        crowdPanel.GetComponent<Image>().color = curColor;
        crowdPanel.GetComponent<Image>().sprite = crowds[crownNum];
        crowdPanel.SetActive(true);

        curColor = effectPanel1.GetComponent<Image>().color;
        curColor.a = 0;
        effectPanel1.GetComponent<Image>().color = curColor;
        effectPanel2.GetComponent<Image>().color = curColor;
        effectPanel1.GetComponent<Image>().sprite = effects[0];
        effectPanel2.GetComponent<Image>().sprite = effects[0];
        effectPanel1.SetActive(true);
        effectPanel2.SetActive(true);

        crowdScreamingAS.volume = 0;
        crowdScreamingAS.Play();
    }

    public void updateCrowd(float proportion)
    {
        Color curColor = crowdPanel.GetComponent<Image>().color;
        curColor.a = proportion;
        crowdPanel.GetComponent<Image>().color = curColor;

        if(proportion <= 0.5)
        {
            float scaledProportion = proportion / 0.5f;
            curColor = effectPanel1.GetComponent<Image>().color;
            curColor.a = 1 - scaledProportion;
            effectPanel1.GetComponent<Image>().color = curColor;
            curColor.a = scaledProportion;
            effectPanel2.GetComponent<Image>().color = curColor;
            effectPanel1.GetComponent<Image>().sprite = effects[0];
            effectPanel2.GetComponent<Image>().sprite = effects[1];
        }else
        {
            float scaledProportion = (proportion - 0.5f) / 0.5f;
            curColor = effectPanel1.GetComponent<Image>().color;
            curColor.a = 1 - scaledProportion;
            effectPanel1.GetComponent<Image>().color = curColor;
            curColor.a = scaledProportion;
            effectPanel2.GetComponent<Image>().color = curColor;
            effectPanel1.GetComponent<Image>().sprite = effects[1];
            effectPanel2.GetComponent<Image>().sprite = effects[2];
        }

        crowdScreamingAS.volume = proportion;
    }

    public void destroyCrowd()
    {
        effectPanel1.SetActive(false);
        effectPanel2.SetActive(false);
        crowdScreamingAS.Stop();
    }

    public void observeTeabox()
    {
        if (!teaboxTouched)
        {
            teaboxLid.GetComponent<Animator>().SetTrigger("OpenLid");
        }
        teaboxTouched = true;
        if (kettleTouched)
            hm.InputNewWords("The teabox is empty. Maybe we only have hot water tonight.", "");
        else
            hm.InputNewWords("The teabox is empty. Just pour some hot water in...", "Touch the kettle");
    }

    public GameObject observeKettle()
    {
        if (kettleTouched)
        {
            hm.InputNewWords("", "The kettle is empty");
            return null;
        }
        kettleOnScreen.SetActive(true);
        kettleOnTable.SetActive(false);
        kettleTouched = true;
        hm.InputNewWords("","Pour the water into the cup");
        return kettleOnScreen;
    }

    public void testPourWater()
    {
        StartCoroutine(blackOut());
    }

    public IEnumerator pourWater(bool fromscreen = true)
    {
        cup.GetComponent<BoxCollider>().enabled = false;
        cup.GetComponent<MeshCollider>().enabled = true;

        originalLocalPostion = kettleOnTable.transform.localPosition;
        originalLocalRotation = kettleOnTable.transform.localRotation;
        if (fromscreen)
        {
            kettleOnTable.transform.position = kettleOnScreen.transform.position;
            kettleOnTable.transform.rotation = kettleOnScreen.transform.rotation;
        }
        kettleOnScreen.SetActive(false);
        kettleOnTable.GetComponent<BoxCollider>().enabled = false;
        kettleOnTable.SetActive(true);

        //fly in the air and pour water
        kettleOnTable.transform.DOLocalMove(pourKettle.localPosition, 2);
        kettleOnTable.transform.DOLocalRotateQuaternion(pourKettle.localRotation, 2);

        yield return new WaitForSeconds(2);

        Vector3 oldRot = pourKettle.localEulerAngles;
        Vector3 newRot = pourKettle.localEulerAngles + new Vector3(0, -60, 0);
        kettleOnTable.transform.DOLocalRotate(newRot, 2);

        yield return new WaitForSeconds(3);
        kettleOnTable.transform.DOLocalRotate(oldRot, 0.5f);
        cup.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //move back
        kettleOnTable.transform.DOLocalMove(originalLocalPostion, 2);
        kettleOnTable.transform.DOLocalRotateQuaternion(originalLocalRotation, 2);
        yield return new WaitForSeconds(2);
        kettleOnTable.GetComponent<BoxCollider>().enabled = true;

        cup.GetComponent<BoxCollider>().enabled = true;
        cup.GetComponent<MeshCollider>().enabled = false;

        cupTouched = true;
    }

    public IEnumerator blackOut()
    {
        if (!cupTouched)
        {
            hm.InputNewWords("Teacup is empty. Pour some water into it", "");
            yield return null;
        }else{

cup.GetComponent<BoxCollider>().enabled = false;

        hm.InputNewWords("You only have hot water now. But you two are together", "Tell each other what you are thinking");

	yield return new WaitForSeconds(10);

hm.InputNewWords("", "");

        beforeScene.SetActive(false);
        afterScene.SetActive(true);
        blackPanel.SetActive(true);

        crowdPanel.SetActive(true);
        c2c.crowdBgAS.volume = 1;
        crowdScreamingAS.volume = 1;
        crowdScreamingAS.Play();
        glassShutteredAS.volume = 1;
        glassShutteredAS.Play();
        messSoundAS.volume = 1;
        messSoundAS.Play();

        yield return new WaitForSeconds(8);

        DOTween.To(() => c2c.crowdBgAS.volume, x => c2c.crowdBgAS.volume = x, 0f, 3);
        DOTween.To(() => crowdScreamingAS.volume, x => crowdScreamingAS.volume = x, 0f, 3);
        DOTween.To(() => messSoundAS.volume, x => messSoundAS.volume = x, 0f, 3);

        yield return new WaitForSeconds(5);
        crowdPanel.SetActive(false);
        blackPanel.SetActive(false);

        hm.InputNewWords("The rose is in ruins.", "Clean the glass and save the rose");
}
    }

    public IEnumerator swipeGlass()
    {
        if (glassSwiped)
        {
            endPanel.SetActive(true);
            yield return null;
        }else{

        glassCollider.GetComponent<BoxCollider>().enabled = false;

        glassPiece1.transform.DOLocalMove(glassPiece1_target.transform.localPosition, 2);
        glassPiece1.transform.DOLocalRotateQuaternion(glassPiece1_target.transform.localRotation, 2);
        glassPiece2.transform.DOLocalMove(glassPiece2_target.transform.localPosition, 2);
        glassPiece2.transform.DOLocalRotateQuaternion(glassPiece2_target.transform.localRotation, 2);
        yield return new WaitForSeconds(2);

        glassSwiped = true;
        glassCollider.GetComponent<BoxCollider>().enabled = true;

        hm.InputNewWords("", "Pick up the rose");
}
    }
}
