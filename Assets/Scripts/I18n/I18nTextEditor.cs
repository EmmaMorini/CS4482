using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class I18nTextEditor : EditorWindow
{

    Vector2 selectTextKeyPosition;
    int selectedText;
    bool showTextKeys = true;
    string textKeyTarget;

    Dictionary<string, bool> showTextKeyContent = new Dictionary<string, bool>();

    [MenuItem("Window/I18N Editor")]
    static void Init() {
        I18nTextEditor window = (I18nTextEditor)EditorWindow.GetWindow(typeof(I18nTextEditor));
        window.Show();
    }
    void OnGUI() {
        using(new EditorGUILayout.VerticalScope()){
            EditorGUILayout.LabelField("Select I18nText instance");
            //select I18nText
            I18nText[] texts = Resources.FindObjectsOfTypeAll<I18nText>();
            string[] validLabels = texts.Select((t, i) => t.name).ToArray();
            List<string> labels = new List<string>(new string[]{"<NONE>"});
            labels.AddRange(validLabels);

            
            selectedText = EditorGUILayout.Popup(selectedText, labels.ToArray());

            I18nText selection = selectedText<=0?null:texts[selectedText-1];
            if(selection != null) {
                using(new EditorGUILayout.HorizontalScope()){
                
                    // select key
                    using(new EditorGUILayout.VerticalScope()){
                        showTextKeys = EditorGUILayout.Foldout(showTextKeys, "Text Keys");
                        if(showTextKeys){
                            using(new EditorGUILayout.ScrollViewScope(selectTextKeyPosition)){
                                using(new EditorGUI.IndentLevelScope()){
                                    // sort keys for consistent displaying
                                    List<string> keys = new List<string>();
                                    foreach(string k in selection.translations.Keys) {
                                        keys.Add(k);
                                    }
                                    keys.Sort();
                                    foreach(var (k, i) in keys.Select((k, i)=>(k, i))){
                                        showTextKeyContent[k] = EditorGUILayout.Foldout(showTextKeyContent.GetValueOrDefault(k, false), i+". "+k);
                                        
                                        if(showTextKeyContent[k]) {
                                            using(new EditorGUI.IndentLevelScope()){
                                                foreach(I18nLanguage l in Enum.GetValues(typeof(I18nLanguage))) {
                                                    using(new EditorGUILayout.HorizontalScope()){
                                                        EditorGUILayout.LabelField(Enum.GetName(typeof(I18nLanguage), l));
                                                        LanguageMap keyTranslations = selection.translations.GetValueOrDefault(k, new LanguageMap());
                                                        string currentValue = keyTranslations.GetTranslationOrDefault(l, "");
                                                        string enteredValue = EditorGUILayout.TextArea(currentValue);
                                                        if(!enteredValue.Equals(currentValue)){
                                                            Undo.RecordObject(selection, string.Format("Update {0} translation for {1}", Enum.GetName(typeof(I18nLanguage), l), k));
                                                            keyTranslations.SetTranslation(l, enteredValue);
                                                            selection.SetDirty();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    };
                                        
                                
                                }
                            }
                        }
                        using(new EditorGUILayout.HorizontalScope()){
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.LabelField("Add or remove text key", GUILayout.Width(135));
                            textKeyTarget = EditorGUILayout.TextField(textKeyTarget);
                            if(GUILayout.Button("+")) {
                                showTextKeys = true;
                                if(!selection.translations.ContainsKey(textKeyTarget)){
                                    Undo.RecordObject(selection, string.Format("Add new text key {0}", textKeyTarget));
                                    selection.translations.Add(textKeyTarget, new LanguageMap());
                                }else{
                                    this.ShowNotification(new GUIContent(string.Format("The text key \"{0}\" already exists in {1}", textKeyTarget, selection.name)));
                                }
                            }
                            if(GUILayout.Button("-")){
                                if(selection.translations.ContainsKey(textKeyTarget)){
                                    Undo.RecordObject(selection, string.Format("Remove text key {0}", textKeyTarget));
                                    selection.translations.Remove(textKeyTarget);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
