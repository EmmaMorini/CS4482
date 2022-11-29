using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueNode
{
    [SerializeField]
    public string label; // the name of the node and the key it uses for translation text
    [SerializeField]
    public ResponseOption[] responses; // the possible responses for the node

    [SerializeReference]
    public NodeType.DialogueNodeType nodeType = NodeType.DialogueNodeType.None;

    public DialogueNode(){}
}
