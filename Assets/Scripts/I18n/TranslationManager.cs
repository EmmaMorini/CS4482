using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class TranslationManager : MonoBehaviour
{
    public static HashSet<I18nTextController> translations = new HashSet<I18nTextController>();


    public TMP_Dropdown language_selection;

    public void Start(){
        language_selection.onValueChanged.AddListener(this.SetLanguage);
        language_selection.ClearOptions();
        language_selection.AddOptions(((I18nLanguage[])Enum.GetValues(typeof(I18nLanguage))).Select(l=>new TMP_Dropdown.OptionData(Enum.GetName(typeof(I18nLanguage), l))).ToList());
    }

    public void SetLanguage(I18nLanguage language) {
        Debug.Log("Set new language to "+language);
        foreach(I18nTextController controller in translations) {
            controller.SetLanguage(language);
        }
    }

    public void SetLanguage(int language) {
        Array vals = Enum.GetValues(typeof(I18nLanguage));
        SetLanguage((I18nLanguage)vals.GetValue(language % vals.Length));
    }

}
