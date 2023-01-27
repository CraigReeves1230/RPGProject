using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Game Database", menuName = "Game Database"), InlineEditor()]
public class GameData : ScriptableObject
{
    [BoxGroup("Message Window"), PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
    public DialogManager.DisplayStyleOptions messageWindowStyle;

    [BoxGroup("Message Window"), PropertySpace(SpaceAfter = 10, SpaceBefore = 5)]
    public Sprite[] faces;

    [BoxGroup("Items, Inventory and Currency"), InlineEditor(), PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
    public CurrencyObject mainCurrency;

    [BoxGroup("Items, Inventory and Currency"), InlineEditor(), PropertySpace(SpaceAfter = 5, SpaceBefore = 0)]
    public InventoryObject defaultInventory;
    
    [BoxGroup("Items, Inventory and Currency"), InlineEditor(), PropertySpace(SpaceAfter = 5, SpaceBefore = 5)]
    public List<InventoryObject> otherInventories;

    [BoxGroup("Items, Inventory and Currency"), PropertySpace(SpaceAfter = 10, SpaceBefore = 5)]
    public List<ItemObject> allItems;
    
    [BoxGroup("Players and Enemies"), PropertySpace(SpaceAfter = 10, SpaceBefore = 5)]
    public List<PlayerObject> allPlayers;

    [BoxGroup("Game World"), PropertySpace(SpaceAfter = 10, SpaceBefore = 5)]
    public int maxLevel = 99;

    [BoxGroup("Game World"), PropertySpace(SpaceAfter = 5, SpaceBefore = 0)]
    public int followLeaderLimit = 3;

    [BoxGroup("Game World"), PropertySpace(SpaceAfter = 10, SpaceBefore = 0)]
    public CustomVariables customVariables;
}
    
    


