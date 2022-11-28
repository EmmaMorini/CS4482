using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LanguageMap : ISerializationCallbackReceiver
{
    [NonSerialized]
    public Dictionary<I18nLanguage, string> translations = new Dictionary<I18nLanguage, string>();

    [SerializeReference]
    private I18nLanguage[] languages;

    [SerializeField]
    private string[] translationList;
    public void OnAfterDeserialize()
    {
        translations = new Dictionary<I18nLanguage, string>();

        foreach(var (k, v) in languages.Zip(translationList, (l, t)=>(l, t))) {
            translations.Add(k, v);
        }
    }

    public void OnBeforeSerialize()
    {
        languages = new I18nLanguage[translations.Count];
        translationList = new string[translations.Count];
        foreach(var (p, i) in translations.Select((p, i) => (p, i))) {
            languages[i] = p.Key;
            translationList[i] = p.Value;
        }
    }

    public void SetTranslation(I18nLanguage language, string translation) {
        this.translations[language] = translation;
    }

    public string GetTranslation(I18nLanguage language) {
        return this.translations[language];
    }

    public string GetTranslationOrDefault(I18nLanguage language, string default_translation) {
        return this.translations.GetValueOrDefault(language, default_translation);
    }
}
