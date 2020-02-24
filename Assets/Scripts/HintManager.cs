using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public Text Story;
    public Text Instruction;

    private float delay = 0.08f;

    //public AudioClip typeSound;
    private AudioSource typeSoundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        typeSoundPlayer = this.GetComponentInChildren<AudioSource>();
        Story.text = "";
        Instruction.text = "";
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
        StartCoroutine(typeWords(storyText, insText));
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

        insText = "[HINT] " + insText;
        for (int i = 0; i < insText.Length; i++)
        {
            Instruction.text += insText[i];
            if (storyText[i] == ' ')
                yield return new WaitForSeconds(delay * 2);
            else
                yield return new WaitForSeconds(delay + Random.Range(-delay, 0));
        }
        typeSoundPlayer.Stop();
    }
}
