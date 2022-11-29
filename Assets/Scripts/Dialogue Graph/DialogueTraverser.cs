using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class DialogueTraverser : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueTree tree;
    private DialogueNode currentState;

    private NodeType.DialogueNodeType endState;

    public string sceneOnFinish;
    void Start()
    {
        currentState = tree.GetEntryNode();
        Debug.Log(currentState);
    }

    public void SelectOption(int optionNumber)
    {
        if(currentState == null || currentState.responses[optionNumber].nextNodeId == "<END>"){
            endState = currentState.nodeType;
            currentState = null;
            
            return;
        }
        currentState = tree.nodes[currentState.responses[optionNumber].nextNodeId];
    }

    public string GetText(I18nLanguage language)
    {
        if(currentState == null){
            if(endState != NodeType.DialogueNodeType.None){
                return "End. Buffed "+ Enum.GetName(typeof(NodeType.DialogueNodeType), endState);    
            }
            return "End";
        }
        return tree.GetText(language, currentState);
    }

    public List<string> GetOptions(I18nLanguage language)
    {
        if(currentState == null){
            return new List<string>();
        }
        return tree.GetOptions(language, currentState);
    }

    public NodeType.DialogueNodeType GetStateGameObject(){
        return currentState.nodeType;
    }

    public bool InEndState(){
        return currentState == null;
    }

    public NodeType.DialogueNodeType GetEndState(){
        return endState;
    }
}
