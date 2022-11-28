using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMPro.TMP_Text promptText;
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
        
        promptText.text = traverser.GetText(language);
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
        // Debug.Log(filler);
        // Debug.Log( filler.GetButtons().Count+" buttons "+traverser.GetOptions(language).Count +" options");
        if(filler != null){
            foreach(var (v, i) in filler.GetButtons().Zip(traverser.GetOptions(language), (x, y)=>(x, y)).Select((v, i)=>(v, i)))
            {
                Debug.Log("Set listener");
                v.x.onClick.AddListener(() =>{
                    Debug.Log("Click option " + i);
                    traverser.SelectOption(i);
                    UpdateUI();
                });
                Debug.Log("Set button text" + v.y);
                v.x.GetComponentInChildren<TMP_Text>().text = v.y;
            }
        }
        // foreach(Transform child in responseParent.transform)
        // {
        //     Destroy(child.gameObject);
        // }

        // foreach((string option, int index) in traverser.GetOptions(language).Select((k, i)=>(k, i)))
        // {
        //     GameObject newButton = Instantiate(responseInstance, new Vector3(250, 300 - index * 60, 0), responseInstance.transform.rotation, responseParent.transform);
        //     newButton.GetComponent<Button>().onClick.AddListener(() => { traverser.SelectOption(index); UpdateUI();});
        //     newButton.GetComponentInChildren<TMPro.TMP_Text>().text = option;

        // }
    }
}
