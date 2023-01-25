using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Command List", menuName = "Skills & Abilities/Command List"), InlineEditor()]
public class BattleCommandList : ScriptableObject
{
    public List<BattleCommand> container;
}
