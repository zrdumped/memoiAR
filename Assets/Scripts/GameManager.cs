using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public GameObject client;
    // Start is called before the first frame update
    private ClientStateController csc;
    void Start()
    {
        //SwitchScene("Chapter0");
        csc = this.GetComponent<ClientStateController>();

        //loadingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScene(string sceneName)
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(loadScene(sceneName));
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        //csc.SetupAfterSceneLoaded();
    }

    private IEnumerator loadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        csc.SetupAfterSceneLoaded();
    }
}
