using System;

public class NodeType {
    [Serializable]
    public enum DialogueNodeType{
        None,
        BuffHealth,
        BuffDamage,
        BuffSpeed
    }
}