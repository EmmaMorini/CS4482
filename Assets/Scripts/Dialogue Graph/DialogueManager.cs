using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Dialogue promptText;
    public DialogueTraverser traverser;
    public GameObject fourResponseButtons;
    public GameObject threeResponseButtons;
    public GameObject twoResponseOptions;

    public GameObject oneResponseOption;

    public TMP_Dropdown languageSelectionDropdown;

    public I18nLanguage language;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    public void UpdateLanguage(){
        language = (I18nLanguage)Enum.Parse(typeof(I18nLanguage), languageSelectionDropdown.options[languageSelectionDropdown.value].text);
        Debug.Log("Language set to: "+Enum.GetName(typeof(I18nLanguage), language));
        UpdateUI();
    }

    void UpdateUI(){
        
        promptText.StartDialogue(traverser.GetText(language));
        fourResponseButtons.SetActive(false);
        threeResponseButtons.SetActive(false);
        twoResponseOptions.SetActive(false);
        oneResponseOption.SetActive(false);

        List<string> options = traverser.GetOptions(language);
        OptionFiller filler = null;
        switch(options.Count){
            case 4:
                Debug.Log("Four options");
                fourResponseButtons.SetActive(true);
                filler = fourResponseButtons.GetComponentInChildren<OptionFiller>();

                break;
            case 3:
                Debug.Log("Three options");
                threeResponseButtons.SetActive(true);
                filler = threeResponseButtons.GetComponentInChildren<OptionFiller>();
                break;
            case 2:
                Debug.Log("Two options");
                twoResponseOptions.SetActive(true);
                filler = twoResponseOptions.GetComponentInChildren<OptionFiller>();
                break;
            case 1:
                Debug.Log("One option");
                oneResponseOption.SetActive(true);
                filler = oneResponseOption.GetComponentInChildren<OptionFiller>();
                break;
        }
        if(filler != null){
            foreach(var (v, i) in filler.GetButtons().Zip(traverser.GetOptions(language), (x, y)=>(x, y)).Select((v, i)=>(v, i)))
            {
                Debug.Log("Set listener");
                v.x.onClick.AddListener(() =>{
                    Debug.Log("Click option " + i);
                    traverser.SelectOption(i);

                    if(traverser.InEndState()){
                        Debug.Log("End state. Buff "+traverser.GetEndState());
                        switch(traverser.GetEndState()) {
                            case NodeType.DialogueNodeType.BuffHealth:
                            PlayerStats.Buffs.MaxHealth *= 1.10f;
                            break;
                            case NodeType.DialogueNodeType.BuffDamage:
                            PlayerStats.Buffs.Damage *= 1.10f;
                            break;
                            case NodeType.DialogueNodeType.BuffSpeed:
                            PlayerStats.Buffs.MoveSpeed *= 1.10f;
                            break;
                        }
                        SceneManager.LoadScene(traverser.sceneOnFinish);
                    }
                    UpdateUI();
                });
                Debug.Log("Set button text" + v.y);
                v.x.GetComponentInChildren<TMP_Text>().text = v.y;
            }
        }
    }
}
