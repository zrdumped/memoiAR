using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public Text Story;
    public Text Instruction;
    public GameObject InstructionSlot1;
    public GameObject InstructionSlot2;
    public GameObject button;

    private float delay = 0.06f;

    //public AudioClip typeSound;
    private AudioSource typeSoundPlayer;

    private Coroutine printCoroutine;

    private Vector3 subDownPos, subUpPos;

    //private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag("Client").GetComponent<GameManager>();
        typeSoundPlayer = this.GetComponentInChildren<AudioSource>();
        button.SetActive(false);
        Story.text = "";
        Instruction.text = "";

        subUpPos = InstructionSlot2.GetComponent<RectTransform>().position;
        subDownPos = InstructionSlot1.GetComponent<RectTransform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test()
    {
        InputNewWords("This is your Flower Shop.", "Sort the flowers.");
    }

    public void InputNewWords(string storyText, string insText)
    {
        if (storyText == "")
            InstructionSlot1.GetComponent<RectTransform>().position = subUpPos;
        else
            InstructionSlot1.GetComponent<RectTransform>().position = subDownPos;

        if (printCoroutine != null)
            StopCoroutine(printCoroutine);
        printCoroutine = StartCoroutine(typeWords(storyText, insText));
    }

    private IEnumerator typeWords(string storyText, string insText)
    {
        Story.text = "";
        Instruction.text = "";
        typeSoundPlayer.Play();
        for (int i = 0; i < storyText.Length; i++)
        {
            Story.text += storyText[i];
            if (storyText[i] == ' ')
                yield return new WaitForSeconds(delay * 2);
            else
                yield return new WaitForSeconds(delay + Random.Range(-delay, 0));
        }

        for (int i = 0; i < insText.Length; i++)
        {
            Instruction.text += insText[i];
            if (insText[i] == ' ')
                yield return new WaitForSeconds(delay * 2);
            else
                yield return new WaitForSeconds(delay + Random.Range(-delay, 0));
        }
        typeSoundPlayer.Stop();
    }

    public void enableButton()
    {
        button.SetActive(true);
    }

    public void disableButton()
    {
        button.SetActive(false);
    }
}
