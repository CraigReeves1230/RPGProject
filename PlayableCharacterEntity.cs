using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cache = UnityEngine.Cache;

public class PlayableCharacterEntity : ControllableEntity, IEquippable
{    
    // character stats
    public string charName;
    public string defaultName;
    
    private int currentLevel = 1;
    
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
    
    // Equipment
    public List<EquipmentSubType> canEquip;
    public EquipmentOutfitObject equipmentOutfit;

    // handling exp
    private int currentEXP;
    private int baseEXPToNext = 1000;
    private int[] expToNextLevel;
    private int maxLevel;
    private int EXPLeftUntilNextLevel;
    
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        
        // may have to be called during game, which is why it is a separate function
        init();
    }

    public void init()
    {
        DontDestroyOnLoad(gameObject);
        
        // get max level determined by game database
        maxLevel = GameManager.instance.gameDatabase.maxLevel == 0 ? 99 : GameManager.instance.gameDatabase.maxLevel;
        
        // calculate exp to next level for each level
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXPToNext;

        for(int i = 2; i < expToNextLevel.Length; i++)
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
            for (int i = 1; i < startingLevel; i++)
            {
                // give necessary experience to reach next level
                AddEXP(expToNextLevel[i]);
            }
        }
        
        // start with full health and magic
        currentHP = currentMaxHP;
        currentMP = currentMaxMP;
    }

    protected void Update()
    {
        base.Update();
        
        // only characters in party are active
        gameObject.SetActive(Array.Exists(GameManager.instance.party, element => element == this));  
        
        // run button
        if (GameManager.instance.hasControl())
        {
            if (GameManager.instance.getSecondaryFireKeyDown())
            {
                isRunning = true;
            }
            if (GameManager.instance.getSecondaryFireKeyUp())
            {
                isRunning = false;
            }
        }
    }

    
    void AddEXP(int expToAdd)
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
        }

        // return amount of levels gained
        return levels;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WalkThrough"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
        }
    }

    
    // Equip
    public void equip(string slotHandle, string itemHandle, InventoryObject inventory = null)
    {
        // get inventory. party inventory is default inventory
        if (inventory == null)
        {
            inventory = GameManager.instance.partyInventory;
        }

        if (inventory == null)
        {
            Debug.Log("ERROR: Inventory not found.");
            return;
        }

        // get item from handle in inventory
        var item = (EquipmentObject) inventory.getItemByHandle(itemHandle);
        
        // check if item exists in inventory
        if (item == null)
        {
            Debug.Log("ERROR: No such item: " + itemHandle + " could be found in inventory.");
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
                Debug.Log("ERROR: This item isn't able to be equipped by this individual.");
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
            Debug.Log("ERROR: Equipment Outfit not found. To equip, entity must have an Equipment Outfit.");
            return;
        }
        
        // check to see if slot exists and is free
        var targetSlot = equipmentOutfit.getSlotByHandle(slotHandle);
        if (targetSlot == null)
        {
            Debug.Log("No slot with the handle: " + slotHandle + " could be found in the outfit.");
            return;
        }
        
        if (targetSlot.item != null)
        {
            Debug.Log("Slot: " + slotHandle + " is taken.");
            return;
        }

        
        if (equipmentOutfit.addItem(item, targetSlot, this, inventory))
        {
            Debug.Log("Equip Success!");
            // add bonuses to player
            currentAttackPower += item.attackBonus;
            currentDefense += item.defenseBonus;
            currentSpeed -= item.speedCost;
        }   
    }


    public void unEquip(string slotHandle, InventoryObject inventory = null)
    {
        if (inventory == null)
        {
            inventory = GameManager.instance.partyInventory;
        }
        
        // find slot
        var slot = equipmentOutfit.getSlotByHandle(slotHandle);

        // check if slot is already empty
        if (slot.item == null)
        {
            Debug.Log("ERROR: Slot is already empty.");
            return;
        }

        var itemBeingRemoved = slot.item;
        
        // remove item from slot
        if (equipmentOutfit.removeItem(slot, this, inventory))
        {
            // remove bonuses from player
            currentAttackPower -= itemBeingRemoved.attackBonus;
            currentDefense -= itemBeingRemoved.defenseBonus;
            currentSpeed += itemBeingRemoved.speedCost;
        }
    }

    
    public void setAttribute(string attribute, int setting)
    {
        throw new NotImplementedException();
    }

    public int getAttribute(string attribute)
    {
        throw new NotImplementedException();
    }
}
