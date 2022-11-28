using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTraverser : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueTree tree;
    private DialogueNode currentState;
    void Start()
    {
        currentState = tree.GetEntryNode();
        Debug.Log(currentState);
    }

    public void SelectOption(int optionNumber)
    {
        if(currentState == null || currentState.responses[optionNumber].nextNodeId == "<END>"){
            currentState = null;
            return;
        }
        Debug.Log("New state: "+currentState.responses[optionNumber].nextNodeId);
        currentState = tree.nodes[currentState.responses[optionNumber].nextNodeId];
    }

    public string GetText(I18nLanguage language)
    {
        if(currentState == null){
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
}
