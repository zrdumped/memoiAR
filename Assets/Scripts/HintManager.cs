using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public List<Sprite> backgrounds;

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
        //if (storyText == "")
        //    InstructionSlot1.GetComponent<RectTransform>().position = subUpPos;
        //else
        //    InstructionSlot1.GetComponent<RectTransform>().position = subDownPos;

        //if (printCoroutine != null)
        //     StopCoroutine(printCoroutine);
        //printCoroutine = StartCoroutine(typeWords(storyText, insText));

        flipWords(storyText, insText);
    }

    private void flipWords(string storyText, string insText)
    {
        Color textColor = Story.color;
        textColor.a = 0;
        Story.color = textColor;

        Color textColor2 = Story.color;
        textColor2.a = 0;
        Instruction.color = textColor2;

        Color endColor = textColor;
        endColor.a = 1;

        Story.text = storyText;
        Instruction.text = insText;

        typeSoundPlayer.Play();

        DOTween.To(() => textColor, x => Story.color = x, endColor, 1);
        DOTween.To(() => textColor2, x => Instruction.color = x, endColor, 1);
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

    public void changeBackground(int chapNum)
    {
        this.GetComponent<Image>().sprite = backgrounds[chapNum];
    }
}
