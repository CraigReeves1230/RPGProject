using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor()]
public abstract class ItemObject : ScriptableObject
{    
    protected ItemType type;

    public string name;
    public string handle;
        
    [PreviewField(Sirenix.OdinInspector.ObjectFieldAlignment.Left)]
    public Sprite graphic;
    
    public AnimationClip useAnimation;

    public int inventoryLimit;
    
    [TextArea(15, 10)]
    public string description;

    public CustomVariables customVariables;
   
    public ItemType GetItemType() => type;

}
