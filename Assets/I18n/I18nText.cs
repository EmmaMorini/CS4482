using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class I18nText : ScriptableObject, ISerializationCallbackReceiver
{
    [NonSerialized]
    // hashtable<string, hashtable<Language, string>>
    public Dictionary<string, LanguageMap> translations = new Dictionary<string, LanguageMap>();
    
    [SerializeField]
    private string[] languageKeys;

    [SerializeField]
    private LanguageMap[] languageMaps;

    public void OnBeforeSerialize()
    {
        languageKeys = new string[translations.Count];
        languageMaps = new LanguageMap[translations.Count];

        foreach(var (p, i) in translations.Select((p, i)=>(p, i))){
            languageKeys[i] = p.Key;
            languageMaps[i] = p.Value;
        }
    }

    public void OnAfterDeserialize()
    {
        translations.Clear();

        foreach( var(k, v) in languageKeys.Zip(languageMaps, (k, v)=>(k, v))) {
            translations.Add(k, v);
        }
    }
}
