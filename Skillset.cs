
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skillset", menuName = "Skills & Abilities/Skillset"), InlineEditor()]
public class Skillset : ScriptableObject
{
    public List<Skill> container;
}
