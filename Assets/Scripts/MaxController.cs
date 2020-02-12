using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject maxToPark;
    public GameObject maxTopic;
    public GameObject maxSelectMusic;

    private ClientStateController csc;
    void Start()
    {
        csc = this.GetComponent<ClientStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void leadMaxToPark()
    {
        maxToPark.SetActive(true);
    }

    public void stopLeadMaxToPark()
    {
        maxToPark.SetActive(false);
    }

    public void makeMaxSelectMusic()
    {
        maxSelectMusic.SetActive(true);
    }

    public void maxSelectedMusic()
    {
        maxSelectMusic.SetActive(false);
        csc.MusicSelected();
    }

    public void makeMaxTalk()
    {
        maxTopic.SetActive(true);
    }
}
