using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Command", menuName = "Skills & Abilities/Command"), InlineEditor()]
public class BattleCommand : ScriptableObject
{
    public string name;
    public BattleAction battleAction;
    public MenuAction menuAction;
}
