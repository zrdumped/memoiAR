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
    public GameObject flare, vLight1, vLight2;
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
        flowershopPanel.SetActive(true);
        housePanel.SetActive(false);
        parkPanel.SetActive(false);
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

        hm.InputNewWords("You spent a nice day here with each other", "Time to go back home");
        //flowershopPanel.SetActive(true);
        c2c.GenerateCrowds();
    }
}
