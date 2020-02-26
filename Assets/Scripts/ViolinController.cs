using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolinController : MonoBehaviour
{
    public GameObject bow;
   
    private int currentNum = 0;

    public ParticleSystem musicNode;

    private AudioSource violinPlayer;

    private ObjectManager om;

    private bool timeToPause = false;

    private bool enumratorRunning = false;

    private float minVelocity = 0, maxVelocity = 0.3f, curVelocity = 0;

    private float totalS = 0, totalT = 0;

    // Start is called before the first frame update
    void Start()
    {
        om = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
        violinPlayer = gameObject.GetComponent<AudioSource>();
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
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            //Debug.Log("move " + hit.transform.gameObject.name);
        }
    }

    private float playTime = 0;
    private IEnumerator PlayViolin()
    {
        Debug.Log("dwqdqwd");
        violinPlayer.clip = om.nodeMusic[om.musicSelected[currentNum]];
        while (true)
        {
            if (timeToPause)
            {
                violinPlayer.Pause();
            }
            else
            {
                if(!violinPlayer.isPlaying)
                    violinPlayer.Play();
                playTime += Time.fixedDeltaTime;
                if(playTime > om.nodeMusic[om.musicSelected[currentNum]].length)
                {
                    if (currentNum == 2)
                        currentNum = 0;
                    else
                        currentNum++;
                    playTime = 0;
                    violinPlayer.clip = om.nodeMusic[om.musicSelected[currentNum]];
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
