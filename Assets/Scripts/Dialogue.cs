using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float charactersPerSecond = 40;
    public AudioSource talkSound;

    private int index;
    // Start is called before the first frame update

    public void StartDialogue(string text)
    {
        StopAllCoroutines();
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine(text));
    }

    IEnumerator TypeLine(string text)
    {
        foreach(char c in text.ToCharArray())
        {
            textComponent.text += c;
            if(char.IsLetter(c)){
                talkSound.PlayOneShot(talkSound.clip);
            }
            yield return new WaitForSeconds(1/charactersPerSecond);
        }
    }
}
