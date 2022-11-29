using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu]
[Serializable]
public class DialogueTree : ScriptableObject, ISerializationCallbackReceiver
{
    // Dictionary of node id:node data
    [NonSerialized]
    public Dictionary<string, DialogueNode> nodes = new Dictionary<string, DialogueNode>();

    [SerializeField]
    public string[] nodeIds;

    [SerializeField]
    public DialogueNode[] nodeList;

    [SerializeField]
    public string entryPointId;

    [SerializeReference]
    public I18nText textSource;

    public void AddNode(string name){

        if(nodes.Count == 0){
            entryPointId = name;
        }
        textSource.translations.Add(name, new LanguageMap());
        DialogueNode newNode = new DialogueNode();
        newNode.responses = new ResponseOption[]{};
        newNode.label = name;
        nodes.Add(name, newNode);
    }

    public void OnBeforeSerialize()
    {
        this.nodeIds = new List<string>(nodes.Keys).ToArray();
        this.nodeList = new List<DialogueNode>(nodes.Values).ToArray();
        // Debug.Log("Serializing " + string.Join(", ", this.nodeIds) + " "+this.nodeIds.Length);
        // Debug.Log("Serializing " + string.Join<DialogueNode>(", ", this.nodeList) + " "+this.nodeList.Length);
        // Debug.Log("Serializing from" +this.nodes + " "+this.nodes.Count);
    }

    public void OnAfterDeserialize()
    {
        // Debug.Log("Deserializing " + string.Join(", ", this.nodeIds) + " "+this.nodeIds.Length);
        // Debug.Log("Deserializing " + this.nodeList.Length + " null count: "+this.nodeList.Where((d)=>d == null).Count());
        // Debug.Log("Serializing from" +this.nodes + " "+this.nodes.Count);
        nodes = nodeIds
            .Zip(nodeList, (k, v) =>(k, v))
            .Aggregate(new Dictionary<string, DialogueNode>(), (d, next) => { d.Add(next.k, next.v); return d; }, d=>d);
    }

    public DialogueNode GetEntryNode()
    {
        Debug.Log(this.nodes);
        Debug.Log(this.entryPointId);
        Debug.Log(this.nodes.ContainsKey(this.entryPointId));
        Debug.Log(this.nodes[this.entryPointId]);
        Debug.Log(this.nodeList);
        return this.nodes[this.entryPointId];
    }

    public string GetText(I18nLanguage language, DialogueNode node)
    {
        if(node == null) {
            return "End of dialogue";
        }
        return this.textSource.translations[node.label].translations[language];
    }

    public List<string> GetOptions(I18nLanguage language, DialogueNode node)
    {   
        List<string> options = node.responses
        .Select((v) => {return textSource.translations[v.responseID].translations[language];})
        .ToList();

        return options;
    }
}
