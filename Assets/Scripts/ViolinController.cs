using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolinController : MonoBehaviour
{
    public GameObject bow;
   
    private int currentNum = 0;

    public ParticleSystem musicNode;
    public ParticleSystem musicFlash;

    private AudioSource violinPlayer;

    private ObjectManager om;
    private ObjectManager2 om2;

    private bool timeToPause = false;

    private bool enumratorRunning = false;

    private float minVelocity = 0, maxVelocity = 0.3f, curVelocity = 0;

    private float totalS = 0, totalT = 0;

    private ClientStateController csc;

    public GameObject camera;


    // Start is called before the first frame update
    void Start()
    {
        violinPlayer = gameObject.GetComponent<AudioSource>();
        //om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
        csc = GameObject.FindGameObjectWithTag("Client").GetComponent<ClientStateController>();
        if (csc.chapNum == 1)
            om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
        else if (csc.chapNum == 2)
           om2 = GameObject.FindGameObjectWithTag("ObjectManager2").GetComponent<ObjectManager2>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase) {
                case TouchPhase.Began:
                    if (!enumratorRunning)
                    {
                        enumratorRunning = true;
                        StartCoroutine(PlayViolin());
                    }
                    timeToPause = false;
                    break;
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    RaycastHit hit;
                    Ray ray;
                    if(csc.chapNum == 1)
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    else
                        ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name == "Touchpad")
                    {
                        violinPlayer.volume = Mathf.Lerp(0, 1, (curVelocity - minVelocity) / (maxVelocity - minVelocity));
                        totalS += Mathf.Abs(bow.transform.position.x - hit.point.x);
                        totalT += Time.deltaTime;
                        if (totalT > 0.5f)
                        {
                            curVelocity = totalS / totalT;
                            totalT = 0;
                            totalS = 0;
                        }
                        Vector3 oldLocal = bow.transform.localPosition;
                        bow.transform.position = hit.point;
                        bow.transform.localPosition = new Vector3(bow.transform.localPosition.x, oldLocal.y, oldLocal.z);
                        //bow.transform.position = new Vector3(worldToLocal.x, bow.transform.position.y, bow.transform.position.z);
                        Debug.Log(curVelocity);
                    }
                    break;
                case TouchPhase.Ended:
                    totalT = 0;
                    totalS = 0;
                    timeToPause = true;
                    break;
            }
        
        }

        else if (Input.GetMouseButtonDown(0))
        {
            if (!enumratorRunning)
            {
                enumratorRunning = true;
                StartCoroutine(PlayViolin());
            }
            timeToPause = false;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            timeToPause = true;
        }

        else if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray;
            if (csc.chapNum == 1)
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            else
                ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name == "Touchpad")
            {
                violinPlayer.volume = Mathf.Lerp(0, 1, (curVelocity - minVelocity) / (maxVelocity - minVelocity));
                totalS += Mathf.Abs(bow.transform.position.x - hit.point.x);
                totalT += Time.deltaTime;
                if (totalT > 0.5f)
                {
                    curVelocity = totalS / totalT;
                    totalT = 0;
                    totalS = 0;
                }
                Vector3 oldLocal = bow.transform.localPosition;
                bow.transform.position = hit.point;
                bow.transform.localPosition = new Vector3(bow.transform.localPosition.x, oldLocal.y, oldLocal.z);
                //bow.transform.position = new Vector3(worldToLocal.x, bow.transform.position.y, bow.transform.position.z);
                //Debug.Log(curVelocity);
            }
            //Debug.Log("move " + hit.transform.gameObject.name);
        }
    }

    private float playTime = 0;
    private IEnumerator PlayViolin()
    {
        //Debug.Log("dwqdqwd");
        if(csc.chapNum == 1)
            violinPlayer.clip = om.nodeMusic[om.musicSelected[currentNum]];
        else
            violinPlayer.clip = om2.nodeMusic[om2.musicSelected[currentNum]];
        while (true)
        {
            if (timeToPause)
            {
                violinPlayer.Pause();
                musicNode.Stop();
                musicFlash.Stop();
            }
            else
            {
                if (!violinPlayer.isPlaying)
                {
                    violinPlayer.Play();
                    musicNode.Play();
                    musicFlash.Play();
                    //get componet in children does not work
                    var color = musicFlash.transform.GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime;
                    if (csc.chapNum == 1)
                        color.color = om.psColor[om.musicSelected[currentNum]].colorOverLifetime.color;
                    else
                        color.color = om2.psColor[om2.musicSelected[currentNum]].colorOverLifetime.color;
                }
                playTime += Time.fixedDeltaTime;
                if(csc.chapNum == 1 && playTime > om.nodeMusic[om.musicSelected[currentNum]].length)
                {
                    if (currentNum == 2)
                    {
                        csc.EndChapter1();
                        break;
                    }
                    //currentNum = 0;
                    else
                        currentNum++;
                    playTime = 0;
                    violinPlayer.clip = om.nodeMusic[om.musicSelected[currentNum]];
                    var color = musicFlash.transform.GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime;
                    color.color = om.psColor[om.musicSelected[currentNum]].colorOverLifetime.color;
                }
                else if (csc.chapNum == 2 && playTime > om2.nodeMusic[om2.musicSelected[currentNum]].length)
                {
                    if (currentNum == 2)
                    {
                        om2.endPanel.SetActive(true);
                        break;
                    }
                    //currentNum = 0;
                    else
                        currentNum++;
                    playTime = 0;
                    violinPlayer.clip = om2.nodeMusic[om2.musicSelected[currentNum]];
                    var color = musicFlash.transform.GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime;
                    color.color = om2.psColor[om2.musicSelected[currentNum]].colorOverLifetime.color;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
