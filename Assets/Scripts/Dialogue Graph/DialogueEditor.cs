#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

public class DialogueEditor : EditorWindow
{
    int selectedDialogueTree;
    int selectedDialogueNode;
    string newDialogueNodeName;
    string newResponseName;
    Dictionary<ResponseOption, int> selectedTransition = new Dictionary<ResponseOption, int>();

    Vector2 scrollPosition;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowGUI(){
        DialogueEditor editor = GetWindow<DialogueEditor>();
        editor.Show();
    }

    void OnGUI() {
        DialogueTree[] trees = Resources.FindObjectsOfTypeAll<DialogueTree>();
        string[] validLabels = trees.Select((t, i)=>t.name).ToArray();
        List<string> labels = new List<string>();
        labels.Add("<NONE>");
        labels.AddRange(validLabels);

        EditorGUILayout.LabelField("Select Dialogue Tree");
        selectedDialogueTree = EditorGUILayout.Popup(selectedDialogueTree, labels.ToArray());
        DialogueTree selectedTree = selectedDialogueTree>0?trees[selectedDialogueTree - 1]:null;
        if (selectedTree)
        {
            using (var h = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Select Dialogue Node");

                if (GUILayout.Button("Entry point"))
                {
                    var entryIndex = selectedTree.nodes.Select((kv, i) => (kv, i)).Where((kv, i) => kv.kv.Key.Equals(selectedTree.entryPointId)).First().i;
                    selectedDialogueNode = entryIndex + 1;
                }

                GUILayout.FlexibleSpace();
                newDialogueNodeName = GUILayout.TextField(newDialogueNodeName, GUILayout.Width(200));
                if (GUILayout.Button("Add new"))
                {
                    Undo.RecordObject(selectedTree, "add dialogue node to tree");
                    selectedTree.AddNode(newDialogueNodeName);
                    EditorUtility.SetDirty(selectedTree);
                    Debug.Log("Added new node " + newDialogueNodeName);
                    newDialogueNodeName = "";
                    selectedDialogueNode = selectedTree.nodes.Count;
                }
            };
           
            List<string> nodeLabels = new List<string>();
            nodeLabels.Add("<NONE>");
            nodeLabels.AddRange(selectedTree.nodes.Keys);

            selectedDialogueNode = Math.Min(EditorGUILayout.Popup(selectedDialogueNode, nodeLabels.ToArray()), nodeLabels.Count - 1);
            
            DialogueNode dialogueNode = selectedDialogueNode > 0 ? selectedTree.nodes[nodeLabels[selectedDialogueNode]] : null;
            Debug.Log(dialogueNode + " "+ selectedDialogueNode + " " + selectedTree.nodes.Count);
            if (dialogueNode != null)
            {
                using(var h = new EditorGUILayout.HorizontalScope()){
                    EditorGUILayout.LabelField("Node Text");
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("Delete node")) {
                        Undo.RecordObject(selectedTree, "Delete node");
                        selectedTree.nodes.Remove(nodeLabels[selectedDialogueNode]);
                        selectedTree.textSource.translations.Remove(dialogueNode.label);
                        foreach(ResponseOption response in dialogueNode.responses){
                            selectedTree.textSource.translations.Remove(response.responseID);
                        }
                    }
                }
                foreach (I18nLanguage l in Enum.GetValues(typeof(I18nLanguage)))
                {
                    using (var h = new EditorGUILayout.HorizontalScope())
                    {
                    
                        EditorGUILayout.LabelField(Enum.GetName(typeof(I18nLanguage), l));
                        string oldTranslation = selectedTree.textSource.translations[dialogueNode.label].GetTranslationOrDefault(l, "");

                        string newTranslation = EditorGUILayout.TextField(selectedTree.textSource.translations[dialogueNode.label].GetTranslationOrDefault(l, ""));

                        if(!oldTranslation.Equals(newTranslation)) {
                            selectedTree.textSource.translations[dialogueNode.label].translations[l] = newTranslation;
                            EditorUtility.SetDirty(selectedTree.textSource);
                        }
                    }
                }

                GUILayout.Space(50);
                using (var h = new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Node Transitions");
                    GUILayout.FlexibleSpace();
                    newResponseName = GUILayout.TextField(newResponseName, GUILayout.Width(200));
                    if (GUILayout.Button("Add new transition"))
                    {
                        ResponseOption response = new ResponseOption();

                        response.responseID = newResponseName;
                        Debug.Log("Added new response "+ newResponseName+ " to "+dialogueNode.label);
                        Undo.RecordObject(selectedTree, "add new transition");
                        selectedTree.textSource.translations.Add(newResponseName, new LanguageMap());
                        dialogueNode.responses = dialogueNode.responses.Append(response).ToArray();
                        // EditorUtility.SetDirty(selectedTree);
                        newResponseName = "";
                    }
                }
                using (var s = new EditorGUILayout.ScrollViewScope(scrollPosition))
                {
                    scrollPosition = s.scrollPosition;
                    HashSet<ResponseOption> toRemove = new HashSet<ResponseOption>();
                    foreach (ResponseOption response in dialogueNode.responses)
                    {
                        using (var h = new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("X", GUILayout.Width(25)))
                            {
                                toRemove.Add(response);
                            }
                            EditorGUILayout.LabelField(response.responseID);
                            GUILayout.FlexibleSpace();

                            List<string> possibleTransitions = new List<string>();
                            possibleTransitions.Add("<END>");
                            possibleTransitions.AddRange(selectedTree.nodes.Keys);
                            if(!selectedTransition.ContainsKey(response))
                            {
                                int defaultValue = Math.Max(0, possibleTransitions.IndexOf(response.nextNodeId));
                                selectedTransition[response] = defaultValue;
                                response.nextNodeId = possibleTransitions[defaultValue];
                                EditorUtility.SetDirty(selectedTree);
                            }
                            int oldTransition = selectedTransition[response];
                            int newTransition = EditorGUILayout.Popup(selectedTransition[response], possibleTransitions.ToArray());

                            if (newTransition != oldTransition) {
                                selectedTransition[response] = newTransition;
                                Undo.RecordObject(selectedTree, "Change transition");
                                response.nextNodeId = possibleTransitions[newTransition];
                            }
                        }
                        foreach (I18nLanguage l in Enum.GetValues(typeof(I18nLanguage)))
                        {
                            using (var h = new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField(Enum.GetName(typeof(I18nLanguage), l));
                                string oldString = selectedTree.textSource.translations[response.responseID].translations.ContainsKey(l)?selectedTree.textSource.translations[response.responseID].translations[l]:null;
                                selectedTree.textSource.translations[response.responseID].translations[l] = EditorGUILayout.TextField(selectedTree.textSource.translations[response.responseID].GetTranslationOrDefault(l, ""));
                                if (oldString == null || !oldString.Equals(selectedTree.textSource.translations[response.responseID].translations[l]))
                                {
                                    EditorUtility.SetDirty(selectedTree);
                                }

                            }
                        }

                    }

                    if(toRemove.Count > 0){
                        Undo.RecordObject(selectedTree, "Remove Response Option");
                        dialogueNode.responses = dialogueNode.responses.Select((x)=>x).Where((x)=>!toRemove.Contains(x)).ToArray();
                        toRemove.ToList().ForEach((x)=>selectedTree.textSource.translations.Remove(x.responseID));
                    }
                }

            }
            // if(GUILayout.Button("Save")) {
            //     EditorUtility.SetDirty(selectedTree);
            //     EditorUtility.SetDirty(selectedTree.textSource);
            // }
        }


    }
}
#endif