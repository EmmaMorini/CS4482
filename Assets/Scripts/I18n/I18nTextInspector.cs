#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class I18nTextInspector : Editor
{
    public override void OnInspectorGUI()
    {
        I18nText text = (target as I18nText);

        EditorGUILayout.LabelField(text.name);
        using(new EditorGUI.IndentLevelScope()){
            foreach(string key in text.translations.Keys) {
                EditorGUILayout.LabelField(key);
                using(new EditorGUI.IndentLevelScope()){
                    foreach(I18nLanguage lang in text.translations[key].translations.Keys) {
                        EditorGUILayout.LabelField(Enum.GetName(typeof(I18nLanguage), lang));
                        using(new EditorGUI.IndentLevelScope()){
                            EditorGUILayout.LabelField(text.translations[key].GetTranslation(lang));
                        }
                    }
                }
            }
        }
    }
}
#endif