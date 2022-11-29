using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class I18nTextController : MonoBehaviour
{
    public I18nText data;
    public string key;

    public TMP_Text visual;


    public void Start() {
        SetLanguage(I18nLanguage.English);
        TranslationManager.translations.Add(this);
    }

    public void OnDestroy() {
        TranslationManager.translations.Remove(this);
    }

    public void SetLanguage(I18nLanguage language) {
        Debug.Log(data.translations + " " +data.translations[key]);
        visual.text = data.translations[key].GetTranslation(language);
    }
}
