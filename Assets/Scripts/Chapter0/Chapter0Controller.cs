using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class Chapter0Controller : MonoBehaviour
{
    public GameObject annaDes, maxDes;
    private HintManager hm;
    private GameManager gm;

    private ClientStateController csc;

    public GameObject mirror;

    public PostProcessVolume postVolumn;
    public PostProcessProfile normalProfile;

    public GameObject violinPanel, rosePanel;

    public Material roseM, violinM;

    public GameObject rose, violin;

    public GameObject slot;
    public GameObject line;

    public GameObject frontImage_IOS;
    public GameObject frontImage_Android;

    private WebCamTexture image;

    public GameObject arCamera;

public GameObject mirrorCamera;

    //1 anna 2 max
    private int character = 0;


    //public ParticleSystem instructorPS;

    // Start is called before the first frame update
    void Start()
    {
        annaDes.SetActive(false);
        maxDes.SetActive(false);

        hm = GameObject.FindGameObjectWithTag("Hint").GetComponent<HintManager>();
        gm = GameObject.FindGameObjectWithTag("Client").GetComponent<GameManager>();
        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();

        mirror.SetActive(true);

        violinPanel.SetActive(false);
        rosePanel.SetActive(false);

        //switch camera
        WebCamDevice[] devices = WebCamTexture.devices;
        string frontCameraName = "";
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
                frontCameraName = devices[i].name;
            Debug.Log(devices[i].name);
        }
        if (frontCameraName == "")
            frontCameraName = devices[0].name;
Debug.Log(frontCameraName);
        image = new WebCamTexture(frontCameraName, Screen.width, Screen.height);
#if UNITY_IOS
        frontImage_IOS.SetActive(true);
#else
        frontImage_Android.SetActive(true);
#endif

        //Quaternion rotation = Quaternion.Euler (0, 90, 0);
        //Matrix4x4 rotationMatrix = Matrix4x4.TRS (Vector3.zero, rotation, new Vector3(1, 1, 1));
        //frontImage.GetComponent<RawImage>().material.SetMatrix ("_Rotation", rotationMatrix);

#if UNITY_IOS
        frontImage_IOS.GetComponent<RawImage>().texture = image;
        frontImage_IOS.GetComponent<RawImage>().material.mainTexture = image;
#else
        frontImage_Android.GetComponent<RawImage>().texture = image;
        frontImage_Android.GetComponent<RawImage>().material.mainTexture = image;
#endif
        image.Play();
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_IOS
        if (image.isPlaying)
        {
            frontImage_IOS.GetComponent<RawImage>().texture = image;
            frontImage_IOS.GetComponent<RawImage>().material.mainTexture = image;
        }
#endif
    }

    public void setUpAsAnna()
    {
        annaDes.SetActive(true);
        hm.GetComponentInChildren<Button>().onClick.AddListener(() => startTutorial());
        character = 1;
    }

    public void setUpAsMax()
    {
        maxDes.SetActive(true);
        hm.GetComponentInChildren<Button>().onClick.AddListener(() => startTutorial());
        character = 2;
    }

    private void startTutorial()
    {
        image.Stop();
#if UNITY_IOS
        frontImage_IOS.SetActive(false);
#else
        frontImage_Android.SetActive(false);
#endif
        mirrorCamera.SetActive(false);
        arCamera.SetActive(true);


        //csc.Chapter0Ended();
        StartCoroutine(rim());

        postVolumn.profile = normalProfile;
        mirror.SetActive(false);


        if (character == 1)
        {
            rose.SetActive(true);
            annaDes.SetActive(false);
            rosePanel.SetActive(true);
        }
        else if (character == 2)
        {
            violin.SetActive(true);
            maxDes.SetActive(false);
            violinPanel.SetActive(true);
        }

        hm.disableButton();
        hm.InputNewWords("", "");
    }

    public void startIns()
    {
        //instructorPS.Play();
        rosePanel.SetActive(false);
        violinPanel.SetActive(false);
    }

    private IEnumerator rim()
    {
        while (true)
        {
            GameObject newLine = Instantiate(line);
            newLine.transform.position = slot.transform.position;
            newLine.GetComponent<TrailRenderer>().enabled = true;

            if (character == 1)
            {
                newLine.transform.DOMove(rose.transform.position, 2);
            }
            else if (character == 2)
            {
                newLine.transform.DOMove(violin.transform.position, 2);
            }

            roseM.DOFloat(5.6f, "_ReflectionRate", 1);
            violinM.DOFloat(0.6f, "_ReflectionRate", 1);
            yield return new WaitForSeconds(1);
            roseM.DOFloat(0.7f, "_ReflectionRate", 1);
            violinM.DOFloat(2f, "_ReflectionRate", 1);
            yield return new WaitForSeconds(1);

            Destroy(newLine);
        }
    }
}
