using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
public abstract class ItemObject : ScriptableObject
{    
    protected ItemType type;

    public string name;
    public string handle;

    public int inventoryLimit;
    
    [TextArea(15, 20)]
    public string description;

    public ItemType GetItemType() => type;

}
