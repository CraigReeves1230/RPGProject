using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = System.Object;

[CreateAssetMenu(fileName = "New Player", menuName = "Player"), InlineEditor()]
public class PlayerObject : ScriptableObject, IEquippable
{
    public enum ColliderShape
    {
        Box,
        Circle,
        Polygon,
        None
    }

    public enum PlayerType
    {
        PlayableCharacter,
        Enemy,
        Other
    }

    // basic info
    [Title("Basic Information", TitleAlignment = TitleAlignments.Centered)]
    public PlayerType playerType;

    // graphics
    [Title("Player Graphics and Collision", TitleAlignment = TitleAlignments.Centered), Required,
     PreviewField(Sirenix.OdinInspector.ObjectFieldAlignment.Left)]
    public Sprite sprite;

    public AnimatorOverrideController animator;

    [PropertySpace(SpaceAfter = 0, SpaceBefore = 5)]
    public ColliderShape colliderType;

    [PropertySpace(SpaceAfter = 5, SpaceBefore = 5), PreviewField(Sirenix.OdinInspector.ObjectFieldAlignment.Left)]
    public Sprite face;

    // character stats
    [Title("Player Stats", TitleAlignment = TitleAlignments.Centered)]
    public string charName;

    public string defaultName;

    private int currentLevel;
    private bool invCleared;

    public int startingLevel;
    private int totalEXP;
    private int currentHP;
    public int startingMaxHP;

    private int currentMaxHP;

    private int currentMP;
    public int startingMaxMP;

    private int currentMaxMP;

    public int startingAttackPower;
    public int startingMagicPower;
    public int startingDefense;
    public int startingSpeed;

    private int currentAttackPower;
    private int currentMagicPower;
    private int currentDefense;
    private int currentSpeed;

    // growth rates
    public float maxHPPercentGrowthRate;
    public float maxMPPercentGrowthRate;
    public float attackPowerPercentGrowthRate;
    public float magicPowerPercentGrowthRate;
    public float defensePercentGrowthRate;

    public float speedPercentGrowthRate;

    // handling exp
    private int currentEXP;
    private int baseEXPToNext;
    private int[] expToNextLevel;
    private int maxLevel;
    private int EXPLeftUntilNextLevel;

    public LevelUpAction onLevelUp;

    // Equipment
    [Title("Equipment Settings", TitleAlignment = TitleAlignments.Centered)]
    public List<EquipmentSubType> canEquip;

    public EquipmentOutfitObject equipmentOutfit;

    // custom variables
    [Title("Custom Variables/Stats", TitleAlignment = TitleAlignments.Centered)]
    public CustomVariables customVariables;
    

    // skills and abilities
    [Title("Skills/Abilities", TitleAlignment = TitleAlignments.Centered)]
    public BattleCommandList commandList;


    void Awake()
    {
        startingMaxHP = 100;
        startingMaxMP = 50;
        currentLevel = 1;
        startingLevel = 1;
        startingAttackPower = 10;
        startingMagicPower = 10;
        startingDefense = 10;
        startingSpeed = 10;
        maxHPPercentGrowthRate = 8.2f;
        maxMPPercentGrowthRate = 5f;
        attackPowerPercentGrowthRate = 7;
        magicPowerPercentGrowthRate = 7;
        defensePercentGrowthRate = 7;
        speedPercentGrowthRate = 7f;
        baseEXPToNext = 1000;
    }

    [Button, PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
    public void CreatePlayer()
    {
        // sprite is required
        if (sprite == null)
        {
            GameManager.instance.errorMsg("Sprite is required.");
            return;
        }

        var newObject = new GameObject(this.name);

        var anim = newObject.AddComponent<Animator>();
        if (animator != null)
        {
            anim.runtimeAnimatorController = animator;
        }

        var sr = newObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Sprites";
        var rb = newObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (colliderType == ColliderShape.Box)
        {
            newObject.AddComponent<BoxCollider2D>();
        }
        else if (colliderType == ColliderShape.Circle)
        {
            newObject.AddComponent<CircleCollider2D>();
        }
        else if (colliderType == ColliderShape.Polygon)
        {
            newObject.AddComponent<PolygonCollider2D>();
        }

        if (playerType == PlayerType.PlayableCharacter)
        {
            var pce = newObject.AddComponent<PlayableCharacterEntity>();
            pce.player = this;
            GameManager.instance.party.Add(pce);
        }
    }

    public void init()
    {
        // reset stats
        currentLevel = 1;
        startingLevel = startingLevel < 1 ? 1 : startingLevel;
        currentMaxHP = 0;
        currentMaxHP = 0;
        currentAttackPower = 0;
        currentMagicPower = 0;
        currentSpeed = 0;
        currentEXP = 0;
        currentDefense = 0;
        currentHP = 0;
        currentMP = 0;
        maxLevel = 0;
        EXPLeftUntilNextLevel = 0;
        invCleared = false;
        
        // hydrate custom variable dictionaries
        if (customVariables != null)
        {
            CustomVariables.hydrateDictionaries(customVariables);
        }

        // get max level determined by game database
        maxLevel = GameManager.instance.gameDatabase.maxLevel == 0 ? 99 : GameManager.instance.gameDatabase.maxLevel;

        // calculate exp to next level for each level
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXPToNext;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }

        // current metrics should be same as starting metrics since starting at level one
        currentAttackPower = startingAttackPower;
        currentDefense = startingDefense;
        currentMagicPower = startingMagicPower;
        currentSpeed = startingSpeed;
        currentMaxHP = startingMaxHP;
        currentMaxMP = startingMaxMP;

        // if player is not starting off in level 1, give sufficient experience and level ups to reach desired level
        if (startingLevel > currentLevel)
        {
            for (int i = 0; i < startingLevel; i++)
            {
                // give necessary experience to reach next level
                AddEXP(expToNextLevel[i]);
            }
        }

        // start with full health and magic
        currentHP = currentMaxHP;
        currentMP = currentMaxMP;

        // check for initially equipped items and refresh equip
        if (equipmentOutfit != null)
        {
            refreshEquipmentAndInventory();
        }
    }


    public void AddEXP(int expToAdd)
    {
        if (currentLevel < maxLevel)
        {
            // add to total and current exp
            currentEXP += expToAdd;
            totalEXP += expToAdd;


            var levelsToGain = 0;

            // check if levels gained
            while (true)
            {
                EXPLeftUntilNextLevel = expToNextLevel[(currentLevel + levelsToGain)] - currentEXP;

                // if there is none left over, gain level
                if (EXPLeftUntilNextLevel <= 0)
                {
                    currentEXP -= expToNextLevel[(currentLevel + levelsToGain)];
                    levelsToGain++;
                }
                else
                {
                    break;
                }
            }

            // add to current level however many levels gained from experience. 
            LevelUp(levelsToGain);
        }
    }

    public int LevelUp(int levels = 1)
    {
        for (int i = 1; i <= levels; i++)
        {
            // increase level
            currentLevel++;

            // increase player stats
            var tempMaxHP = currentMaxHP + Mathf.CeilToInt(currentMaxHP * (maxHPPercentGrowthRate * 0.01f));
            var tempMaxMP = currentMaxMP + Mathf.CeilToInt(currentMaxMP * (maxMPPercentGrowthRate * 0.01f));
            currentMaxHP = tempMaxHP > 9999 ? 9999 : tempMaxHP;
            currentMaxMP = tempMaxMP > 999 ? 999 : tempMaxMP;

            var tempAtkPwr = currentAttackPower +
                             Mathf.CeilToInt(currentAttackPower * (attackPowerPercentGrowthRate * 0.01f));
            var tempMagPwr = currentMagicPower +
                             Mathf.CeilToInt(currentMagicPower * (magicPowerPercentGrowthRate * 0.01f));
            var tempDef = currentDefense + Mathf.CeilToInt(currentDefense * (defensePercentGrowthRate * 0.01f));
            var tempSpd = currentSpeed + Mathf.CeilToInt(currentSpeed * (speedPercentGrowthRate * 0.01f));

            currentAttackPower = tempAtkPwr > 999 ? 999 : tempAtkPwr;
            currentMagicPower = tempMagPwr > 999 ? 999 : tempMagPwr;
            currentDefense = tempDef > 999 ? 999 : tempDef;
            currentSpeed = tempSpd > 999 ? 999 : tempSpd;

            // run on level  up  action
            if (onLevelUp != null)
            {
                onLevelUp.onLevelUp(this);
            }
        }

        // return amount of levels gained
        return levels;
    }

    public void equip(string slotHandle, string itemHandle, InventoryObject inventory = null, bool forRefresh = false)
    {
        // get inventory. party inventory is default inventory
        if (inventory == null)
        {
            inventory = GameManager.instance.gameDatabase.defaultInventory;
        }

        if (inventory == null)
        {
            GameManager.instance.errorMsg("ERROR: Inventory not found.");
            return;
        }

        // get item from handle in inventory
        var item = (EquipmentObject) inventory.getItemByHandle(itemHandle);

        // check if item exists in inventory
        if (item == null)
        {
            GameManager.instance.errorMsg("ERROR: No such item: " + itemHandle + " could be found in inventory.");
            return;
        }

        var itemIsEquippable = false;


        // determine if item can be equipped
        if (item.equipmentSubType.Count > 0)
        {
            foreach (var type in canEquip)
            {
                if (item.equipmentSubType.Contains(type))
                {
                    itemIsEquippable = true;
                    break;
                }
            }

            if (!itemIsEquippable)
            {
                GameManager.instance.errorMsg("ERROR: This item isn't able to be equipped by this individual.");
            }
        }
        else
        {
            // equipment that has no sub type is equippable by anyone
            itemIsEquippable = true;
        }

        // do not go further if item can't be equipped
        if (!itemIsEquippable) return;

        // check if outfit is available
        if (equipmentOutfit == null)
        {
            GameManager.instance.errorMsg(
                "ERROR: Equipment Outfit not found. To equip, entity must have an Equipment Outfit.");
            return;
        }

        // check to see if slot exists and is free
        var targetSlot = equipmentOutfit.getSlotByHandle(slotHandle);
        if (targetSlot == null)
        {
            GameManager.instance.errorMsg("ERROR: No slot with the handle: " + slotHandle +
                                          " could be found in the outfit.");
            return;
        }

        if (targetSlot.item != null)
        {
            GameManager.instance.errorMsg("ERROR: Slot: " + slotHandle + " is taken.");
            return;
        }


        if (equipmentOutfit.addItem(item, targetSlot, this, inventory, forRefresh))
        {
            // add bonuses to player
            currentAttackPower += item.attackBonus;
            currentDefense += item.defenseBonus;
            currentSpeed -= item.speedCost;
        }
    }

    public void unEquip(string slotHandle, InventoryObject inventory = null, bool forRefresh = false)
    {
        if (inventory == null)
        {
            inventory = GameManager.instance.gameDatabase.defaultInventory;
        }

        // find slot
        var slot = equipmentOutfit.getSlotByHandle(slotHandle);

        // check if slot is already empty
        if (slot.item == null)
        {
            GameManager.instance.errorMsg("ERROR: Slot is already empty.");
            return;
        }

        var itemBeingRemoved = slot.item;

        // remove item from slot
        if (equipmentOutfit.removeItem(slot, this, inventory, forRefresh))
        {
            // remove bonuses from player
            if (!forRefresh)
            {
                currentAttackPower -= itemBeingRemoved.attackBonus;
                currentDefense -= itemBeingRemoved.defenseBonus;
                currentSpeed += itemBeingRemoved.speedCost;
            }
        }
    }

    private void refreshEquipmentAndInventory()
    {
        // clear player's outfit from inventory equippedBys
        if (!invCleared)
        {
            foreach (var slot in GameManager.instance.gameDatabase.defaultInventory.container)
            {
                if (slot.equippedBy.Contains(equipmentOutfit))
                {
                    for (int i = 0; i < slot.equippedBy.Count; i++)
                    {
                        if (slot.equippedBy[i] == equipmentOutfit)
                        {
                            slot.equippedBy.RemoveAt(i);
                        }

                    }
                }
            }

            invCleared = true;
        }

        // unequip everything
        List<string[]> tempEquipped = new List<string[]>();
        foreach (var slot in equipmentOutfit.container)
        {
            slot.isBlocked = false;
            // unequip and then re-equip item
            if (slot.item != null)
            {
                var item = slot.item.handle;
                var slotHandle = slot.handle;
                tempEquipped.Add(new[] {slotHandle, item});

                unEquip(slot.handle, null, true);
            }
        }

        // re-equip everything
        foreach (var row in tempEquipped)
        {
            InventorySlot theInvSlot = null;
            foreach (var sl in GameManager.instance.gameDatabase.defaultInventory.container)
            {
                if (sl.item.handle == row[1])
                {
                    theInvSlot = sl;
                }
            }

            if (theInvSlot != null && theInvSlot.amount > theInvSlot.equippedBy.Count)
            {
                equip(row[0], row[1], null, true);
            }
            else
            {
                GameManager.instance.errorMsg("ERROR: Not enough left in inventory to be equipped.");
                row[1] = null; // item was never equipped
            }
        }

        // re-hydrate inventory equippedBys
        foreach (var invSlot in GameManager.instance.gameDatabase.defaultInventory.container)
        {
            foreach (var row in tempEquipped)
            {
                if (invSlot.item.handle == row[1])
                {
                    if (!invSlot.equippedBy.Contains(equipmentOutfit))
                    {
                        invSlot.equippedBy.Add(equipmentOutfit);
                    }
                }
            }
        }
    }
}

