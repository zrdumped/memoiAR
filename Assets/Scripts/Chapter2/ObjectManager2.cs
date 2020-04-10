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
    public Material transitionMaterial;
    private HintManager hm;
    private Chapter2Controller c2c;
    private ClientStateController csc;
    //public GameObject effectCamera;
    [Header("Summer")]
    public GameObject flowershop;
    public GameObject flare;
    public GameObject vLight1;
    public GameObject vLight2;
    public GameObject weddingPhoto;
    public GameObject weddingPhotoPos;
    public Transform flarePos1, flarePos2;
    public PostProcessProfile summerProfile;
    private float srcVLight1Intensity, srcVLight2Intensity;
    private float srcVLight1Range, srcVLight2Range;

    [Header("Autumn")]
    public GameObject house;
    public GameObject houseEnv;
    public PostProcessProfile autumnProfile;
    public GameObject leaves;
    public GameObject movingPhoto;
    private Vector3 movingPhotoOriginalPos;
    private Quaternion movingPhotoOriginalRot;
    public GameObject movingPhotoPos;

    [Header("Winter")]
    public GameObject park;
    public PostProcessProfile winterProfile;
    public GameObject snow;
    public GameObject playingPhoto;
    public GameObject playingPhotoPos;
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
    public GameObject ruinedRose;
    public Transform ruinedRosePos;
    public GameObject beforeScene;
    public GameObject afterScene;
    public GameObject blackPanel;
    public GameObject teaboxLid;
    public GameObject kettleOnTable;
    public GameObject kettleOnScreen;
    //public GameObject wholeGlass;
    //public GameObject shatteredGlass;
    public List<GameObject> cups;
    public List<Transform> kettlePoses;
    private bool teaboxTouched = false;
    private bool kettleTouched = false;
    private bool teaboxCouldBeTouched = false;
    private bool cupTouched = false;
    private Vector3 originalLocalPostion;
    private Quaternion originalLocalRotation;
    public AudioSource glassShutteredAS;
    public AudioSource messSoundAS;
    private bool glassSwiped = false;
    public List<GameObject> glassPiece;
    public List<GameObject> glassPiece_target;
    private int piecesCount = 0;
    public GameObject glassCollider;
    public GameObject endPanel;
    public bool couldDrink = false;

    private List<string> annaWords = new List<string>
    {
        "Is Max alright?",
        "Why they are trying to destroy your life?",
        "They won't even let you drink hot water."
    };

    private List<string> maxWords = new List<string>
    {
        "Why are they ruining your home? Is Anna still ok?",
        "Why do they hate you two so much?",
        "What did you do wrong?"
    };

    // Start is called before the first frame update
    public void Start()
    {
        volumn.profile = normalProfile;

#if SHOW_HM
        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        //hm.InputNewWords("", "");
        hm.InputNewWords("A few years later and one thing lead to another...", "Go to the flowershop");
#endif
        c2c = GameObject.FindGameObjectWithTag("Chap2Client").GetComponent<Chapter2Controller>();

        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();


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
        movingPhotoOriginalPos = movingPhoto.transform.localPosition;
        movingPhotoOriginalRot = movingPhoto.transform.localRotation;

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
        beforeScene.SetActive(false);
        afterScene.SetActive(false);
        blackPanel.SetActive(false);
    }

    public IEnumerator ChangeToSummer()
    {
        StartCoroutine(showGradually(flowershop));
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

        weddingPhoto.SetActive(true);
        weddingPhoto.transform.DOLocalMove(weddingPhotoPos.transform.localPosition, 5);
        weddingPhoto.transform.DOLocalRotate(weddingPhotoPos.transform.localEulerAngles, 5);

        yield return new WaitForSeconds(4);

        DOTween.To(() => vLight1.GetComponent<Light>().intensity, x => vLight1.GetComponent<Light>().intensity = x, 0, 1);
        DOTween.To(() => vLight2.GetComponent<Light>().intensity, x => vLight2.GetComponent<Light>().intensity = x, 0, 1);
        DOTween.To(() => vLight1.GetComponent<Light>().range, x => vLight1.GetComponent<Light>().range = x, 0, 1);
        DOTween.To(() => vLight2.GetComponent<Light>().range, x => vLight2.GetComponent<Light>().range = x, 0, 1);

        yield return new WaitForSeconds(1);

        weddingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 1);

        flare.SetActive(false);
        vLight1.SetActive(false);
        vLight2.SetActive(false);

        volumn.profile = normalProfile;


        //Color curColor = weddingPhoto.GetComponent<Renderer>().material.color;
        //weddingPhoto.GetComponent<Renderer>().material.color = new Color(curColor.r, curColor.g, curColor.b, 0);
        //curColor.a = 1;
        //weddingPhoto.GetComponent<Renderer>().material.DOColor(curColor, 4);
        //weddingPhoto.transform.DOLocalJump(weddingPhoto.transform.localPosition + new Vector3(0, 0.1f, 0), 0.2f, 5, 5);

        yield return new WaitForSeconds(8);
        weddingPhoto.SetActive(false);
#if SHOW_HM
        if (c2c.isMax())
            hm.InputNewWords("Your apartment never had fresh flowers before she moved in", "Go to Max's Apartment");
        else if (c2c.isAnna())
            hm.InputNewWords("Over time, his apartment became our apartment", "Go to Max's Apartment");
#endif
        housePanel.SetActive(true);
    }

    public IEnumerator ChangeToAutumn()
    {
        StartCoroutine(showGradually(house));

        housePanel.SetActive(false);
        leaves.SetActive(true);
        leaves.GetComponent<ParticleSystem>().Play();

        volumn.profile = autumnProfile;

        movingPhoto.SetActive(true);
        movingPhoto.transform.DOLocalMove(movingPhotoPos.transform.localPosition, 5);
        movingPhoto.transform.DOLocalRotate(movingPhotoPos.transform.localEulerAngles, 5);

        yield return new WaitForSeconds(5);

        movingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 1);
        yield return new WaitForSeconds(1);
        movingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 2);

        //leaves.SetActive(false);
        leaves.GetComponent<ParticleSystem>().Stop();
        volumn.profile = normalProfile;
        //movingPhoto.SetActive(true);
        //Color curColor = movingPhoto.GetComponent<Renderer>().material.color;
        //movingPhoto.GetComponent<Renderer>().material.color = new Color(curColor.r, curColor.g, curColor.b, 0);
        //curColor.a = 1;
        //movingPhoto.GetComponent<Renderer>().material.DOColor(curColor, 4);

        yield return new WaitForSeconds(8);
        movingPhoto.SetActive(false);
        leaves.SetActive(false);
        houseEnv.SetActive(false);
        beforeScene.SetActive(false);

#if SHOW_HM
        if (c2c.isMax())
            hm.InputNewWords("You looked for Anna's face whenever you played in the park", "Go to The Park");
        else if (c2c.isAnna())
            hm.InputNewWords("Selling flowers was always a great excuse to go see him play", "Go to The Park");
#endif
        parkPanel.SetActive(true);
    }

    public IEnumerator ChangeToWinter()
    {
        StartCoroutine(showGradually(park));

        playingPhoto.SetActive(true);
        playingPhoto.transform.DOLocalMove(playingPhotoPos.transform.localPosition, 5);
        playingPhoto.transform.DOLocalRotate(playingPhotoPos.transform.localEulerAngles, 5);
        parkPanel.SetActive(false);
        //hm.InputNewWords("", "");
        snow.SetActive(true);
        snow.GetComponent<ParticleSystem>().Play();
        volumn.profile = winterProfile;

        fe.enabled = true;

        DOTween.To(() => fe.FrostAmount, x => fe.FrostAmount = x, 0.25f, 2);

        yield return new WaitForSeconds(4);

        DOTween.To(() => fe.FrostAmount, x => fe.FrostAmount = x, 0, 1);

        yield return new WaitForSeconds(1);

        playingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 1);
        yield return new WaitForSeconds(1);
        playingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 2);
        yield return new WaitForSeconds(1);
        playingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 3);

        fe.enabled = false;
        volumn.profile = normalProfile;
        snow.GetComponent<ParticleSystem>().Stop();

        //playingPhoto.SetActive(true);
        //Color curColor = playingPhoto.GetComponent<Renderer>().material.color;
        //playingPhoto.GetComponent<Renderer>().material.color = new Color(curColor.r, curColor.g, curColor.b, 0);
        //curColor.a = 1;
        //playingPhoto.GetComponent<Renderer>().material.DOColor(curColor, 4);

        yield return new WaitForSeconds(8);
        playingPhoto.SetActive(false);
        snow.SetActive(false);

        chapter2Panel.SetActive(true);
        Color curColor = chapter2Panel.GetComponent<Image>().color;
        curColor.a = 0;
        chapter2Panel.GetComponent<Image>().color = curColor;
        curColor.a = 1;
        chapter2Panel.GetComponent<Image>().DOColor(curColor, 1);
        yield return new WaitForSeconds(2);
        curColor.a = 0;
        chapter2Panel.GetComponent<Image>().DOColor(curColor, 1);
        yield return new WaitForSeconds(1);
        chapter2Panel.SetActive(false);

#if SHOW_HM
        if (c2c.isMax())
            hm.InputNewWords("You hear shouting and glass shattering. It isn't safe here", "Get Anna home");
        else if (c2c.isAnna())
            hm.InputNewWords("Something is wrong", "Go back home");
#endif
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

    public void ShowOutside()
    {
        Color curColor = crowdPanel.GetComponent<Image>().color;
        curColor.a = 1;
        crowdPanel.GetComponent<Image>().color = curColor;
        crowdPanel.SetActive(true);

        curColor = effectPanel1.GetComponent<Image>().color;
        curColor.a = 1;
        effectPanel1.GetComponent<Image>().color = curColor;
        effectPanel1.GetComponent<Image>().sprite = effects[0];
        effectPanel1.SetActive(true);

        crowdScreamingAS.volume = 1;
        crowdScreamingAS.Play();

        houseEnv.SetActive(false);
        beforeScene.SetActive(false);

        hm.InputNewWords("People watch you in your doorway. It is definitely not safe to go outside", "Head back to the Apartment");
    }

    public void HideOutside()
    {
        destroyCrowd();
        escapeInHouse();
    }

    public void updateCrowd(float proportion)
    {
        Color curColor = crowdPanel.GetComponent<Image>().color;
        curColor.a = proportion;
        crowdPanel.GetComponent<Image>().color = curColor;

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
        } else if (proportion >= 0.7)
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

        crowdScreamingAS.volume = proportion;
    }

    public void destroyCrowd()
    {
        crowdPanel.SetActive(false);
        effectPanel1.SetActive(false);
        effectPanel2.SetActive(false);
        crowdScreamingAS.Stop();
    }

    public void escapeInHouse()
    {
        houseEnv.SetActive(true);
        beforeScene.SetActive(true);
        StartCoroutine(showGradually(houseEnv));
        StartCoroutine(showGradually(beforeScene));
    }

    public void observeTeabox(bool fromServer = false)
    {
        if (!teaboxCouldBeTouched) return;
        if (teaboxTouched) return;
        if (c2c.isMax() && !fromServer) return;
        if (c2c.isAnna())
        {
            csc.teaboxOpened();
        }
        teaboxLid.GetComponent<Animator>().SetTrigger("OpenLid");
        teaboxTouched = true;
        //if (kettleTouched)
        //    hm.InputNewWords("The teabox is empty. We only have hot water tonight.", "");
        //else
        //    hm.InputNewWords("The teabox is empty. Just pour some hot water in...", "Touch the kettle");
#if SHOW_HM
        if (c2c.isMax())
            hm.InputNewWords("There's no tea, but there might be some outside. Will they take this away from you too?", "Tell Annaliese you're going out.");
        else if (c2c.isAnna())
            hm.InputNewWords("There's no tea, but there might be some at the store", "Ask Max if he could get some outside");
#endif
        c2c.maxBuyTea();
    }

    public GameObject observeKettle()
    {
        if (c2c.isMax()) return null;
        if (kettleTouched)
        {
            hm.InputNewWords("", "The kettle is empty");
            return null;
        }
        kettleOnScreen.SetActive(true);
        kettleOnTable.SetActive(false);
        kettleTouched = true;
#if SHOW_HM
        if (c2c.isMax())
            hm.InputNewWords("Were things always this bad? How will you keep Anna safe?", "Pour the water into the cup");
        else if (c2c.isAnna())
            hm.InputNewWords("Max looks like he has something on his mind.", "Pour the water into cups");
#endif
        return kettleOnScreen;
    }

    public void testPourWater()
    {
        StartCoroutine(swipeGlass());
        //ShowOutside();
    }

    public IEnumerator pourWater(bool fromscreen = true)
    {
        if (c2c.isAnna())
        {
            csc.waterPoured();
        }
        cups[0].GetComponent<BoxCollider>().enabled = false;
        cups[1].GetComponent<BoxCollider>().enabled = false;
        cups[0].transform.GetChild(0).gameObject.SetActive(true);
        cups[1].transform.GetChild(0).gameObject.SetActive(true);
        //cup.GetComponent<MeshCollider>().enabled = true;

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

        for (int i = 0; i < cups.Count; i++)
        {
            //fly in the air and pour water
            kettleOnTable.transform.DOLocalMove(kettlePoses[i].localPosition, 2);
            kettleOnTable.transform.DOLocalRotateQuaternion(kettlePoses[i].localRotation, 2);

            yield return new WaitForSeconds(2);

            Vector3 oldRot = kettlePoses[i].localEulerAngles;
            Vector3 newRot = kettlePoses[i].localEulerAngles + new Vector3(0, -60, 0);
            kettleOnTable.transform.DOLocalRotate(newRot, 2);

            yield return new WaitForSeconds(3);
            kettleOnTable.transform.DOLocalRotate(oldRot, 0.5f);
            
            yield return new WaitForSeconds(0.5f);
        }
        //move back
        kettleOnTable.transform.DOLocalMove(originalLocalPostion, 2);
        kettleOnTable.transform.DOLocalRotateQuaternion(originalLocalRotation, 2);
        yield return new WaitForSeconds(2);
        kettleOnTable.GetComponent<BoxCollider>().enabled = true;

        cups[0].GetComponent<BoxCollider>().enabled = true;
        cups[1].GetComponent<BoxCollider>().enabled = true;
        //cup.GetComponent<MeshCollider>().enabled = false;

        cupTouched = true;

        teaboxCouldBeTouched = true;
    }


    public void drinkTea()
    {
        if (!couldDrink) return;
        cups[0].GetComponent<BoxCollider>().enabled = false;
        cups[1].GetComponent<BoxCollider>().enabled = false;
#if SHOW_HM
        if (c2c.isMax())
            hm.InputNewWords("How's the 'Tea'? Things may be imperfect but could you do it alone?", "Tell Anna what's on your mind");
        else
            hm.InputNewWords("Was Max brave for trying to go out? Is this still 'tea time' for you?", "Tell Max what he needs to hear");
#endif
        csc.OneDrinkTea();
    }

    public IEnumerator blackOut()
    {
        yield return new WaitForSeconds(10);

        hm.InputNewWords("", "");
        //Debug.Log(2);
        afterScene.SetActive(true);
        blackPanel.SetActive(true);
        //Debug.Log(3);
        crowdPanel.SetActive(true);
        c2c.crowdBgAS.volume = 1;
        crowdScreamingAS.volume = 1;
        crowdScreamingAS.Play();
        glassShutteredAS.volume = 1;
        glassShutteredAS.Play();
        messSoundAS.volume = 1;
        messSoundAS.Play();
        //Debug.Log(4);
        yield return new WaitForSeconds(8);
        //Debug.Log(5);
        DOTween.To(() => c2c.crowdBgAS.volume, x => c2c.crowdBgAS.volume = x, 0f, 3);
        DOTween.To(() => crowdScreamingAS.volume, x => crowdScreamingAS.volume = x, 0f, 3);
        DOTween.To(() => messSoundAS.volume, x => messSoundAS.volume = x, 0f, 3);
        //Debug.Log(6);
        yield return new WaitForSeconds(5);
        beforeScene.SetActive(false);
        crowdPanel.SetActive(false);
        blackPanel.SetActive(false);
        //Debug.Log(7);
        hm.InputNewWords("So it's come to this.", "Clean up the glass");
    }

    public IEnumerator swipeGlass()
    {
        glassCollider.GetComponent<BoxCollider>().enabled = false;
        csc.SwipeGlass();
        if (piecesCount < glassPiece.Count)
        {
            glassPiece[piecesCount].transform.DOLocalMove(glassPiece_target[piecesCount].transform.localPosition, 1);
            glassPiece[piecesCount].transform.DOLocalRotateQuaternion(glassPiece_target[piecesCount].transform.localRotation, 1);
            yield return new WaitForSeconds(1);
            piecesCount++;
#if SHOW_HM
            if (c2c.isAnna())
            {
                if (piecesCount == glassPiece.Count)
                    hm.InputNewWords("The rose looks pretty damaged", "Pick up the rose");
                else
                    hm.InputNewWords(annaWords[piecesCount], "");
            }
            else
            {
                if (piecesCount == glassPiece.Count)
                    hm.InputNewWords("The rose looks pretty damaged", "");
                else
                    hm.InputNewWords(maxWords[piecesCount], "");

            }
#endif

        }

        else
        {
            if(c2c.isAnna())
                hm.InputNewWords("You should be able to press it", "");
            else
                hm.InputNewWords("Anna should be able to press it", "");

            movingPhoto.SetActive(true);
            movingPhoto.transform.localPosition = movingPhotoOriginalPos;
            movingPhoto.transform.localRotation = movingPhotoOriginalRot;

            showGradually(movingPhoto);
            //yield return new WaitForSeconds(2);

            Vector3 curScale = ruinedRosePos.localScale;
            curScale.y = 0.1f;


            movingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 1);
            yield return new WaitForSeconds(1);
            movingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 2);
            yield return new WaitForSeconds(1);
            movingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 3);
            yield return new WaitForSeconds(1);



            ruinedRose.transform.DOLocalMove(ruinedRosePos.localPosition, 1);
            ruinedRose.transform.DOLocalRotateQuaternion(ruinedRosePos.localRotation, 1);
            ruinedRose.transform.DOScale(curScale, 2);
            yield return new WaitForSeconds(1);

            movingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 4);
            yield return new WaitForSeconds(1);
            ruinedRose.SetActive(false);
            //movingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 5);
            //yield return new WaitForSeconds(1);
            //movingPhoto.GetComponent<Animator>().SetInteger("FlipOnePage", 6);
            yield return new WaitForSeconds(3);
            endPanel.SetActive(true);
        }

        glassCollider.GetComponent<BoxCollider>().enabled = true;
    }

    public IEnumerator showGradually(GameObject parent)
    {
        Renderer[] rendererd = parent.GetComponentsInChildren<Renderer>();
        List<Material> materials = new List<Material>();
        foreach(Renderer r in rendererd)
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

    public IEnumerator showGradually_SkinMesh(GameObject parent)
    {
        SkinnedMeshRenderer[] rendererd = parent.GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Material> materials = new List<Material>();
        foreach (SkinnedMeshRenderer r in rendererd)
        {
            Material m = Instantiate(transitionMaterial);
            m.SetTexture("_MainTex", r.material.GetTexture("_MainTex"));
            materials.Add(r.material);
            r.material = m;

            m.DOFloat(1, "_Alpha", 2);
        }

        yield return new WaitForSeconds(2);

        int count = 0;
        foreach (SkinnedMeshRenderer r in rendererd)
        {
            r.material = materials[count];
            count++;
        }
    }
}
