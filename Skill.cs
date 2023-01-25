using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills & Abilities/Skill"), InlineEditor()]
public class Skill : ScriptableObject
{
    public string name;
    
    [TextArea(15, 10)]
    public string description;
    
    public AnimationClip animation;

    public BattleAction battleAction;
    public MenuAction menuAction;
}
