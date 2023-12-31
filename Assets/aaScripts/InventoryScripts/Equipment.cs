using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public static Equipment Instance;

    public GameObject WeaponSprite;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public EquipmentContent equipmentContent;
    public UpdateInventory updateInventory;


    public delegate void OnEquipmentChanged();
    public OnEquipmentChanged onEquipmentChanged;

    // agrega un equipo al equipamiento del personaje
    public void AddEquip(Equip equip)
    {
        int slot = (int)equip.equipType;
        if (equipmentContent.CharacterEquipment[slot] != null)
        {
            Inventory.Instance.removeItem(equip);
            Inventory.Instance.addItem(equipmentContent.CharacterEquipment[slot]);
            equipmentContent.CharacterEquipment[slot] = equip;
        }
        else
        {
            Inventory.Instance.removeItem(equip);
            equipmentContent.CharacterEquipment[slot] = equip;
        }

        if(equip.equipType == Equip.EquipmentType.Weapon)
        {
            WeaponSprite.GetComponent<SpriteRenderer>().sprite = equip.ItemSprite;
        }

        onEquipmentChanged?.Invoke();
    }

    // desequipa un equipo del personaje
    public void Unequip(Equip equip)
    {
        if (Inventory.Instance.addItem(equip))
        {
            equipmentContent.CharacterEquipment[(int)equip.equipType] = null;
            onEquipmentChanged?.Invoke();
        }
    }

    // agrega una mochila al equipamiento
    public void addBackpack(Backpack backPack)
    {
        int num = Array.IndexOf(equipmentContent.Backpacks, null);



        if (Array.IndexOf(equipmentContent.Backpacks, null) != -1)
        {
            Inventory.Instance.removeItem(backPack);
            equipmentContent.Backpacks[Array.IndexOf(equipmentContent.Backpacks, null)] = backPack;
            updateInventory.addSlot(backPack.space);
        }

        onEquipmentChanged?.Invoke();
    }

    // borra una mochila del equipamiento
    public void removeBackpack(Backpack backPack)
    {
        if (updateInventory.removeSlot(backPack.space))
        {
            Inventory.Instance.addItem(backPack);
            equipmentContent.Backpacks[Array.IndexOf(equipmentContent.Backpacks, backPack)] = null;

        }
        onEquipmentChanged?.Invoke();
    }


    // devuelve el tama�o de las mochilas
    public int getBackpacksCapacity()
    {
        int backpackCapacity = 0;
        for (int i = 0; i < equipmentContent.Backpacks.Length; i++)
        {
            if (equipmentContent.Backpacks[i] != null)
            {
                backpackCapacity += equipmentContent.Backpacks[i].space;
            }

        }
        print("BACKPAKS SPACE:" + backpackCapacity);
        return backpackCapacity;
    }



    //Funciones que devulven los stats de los items.
    public int getEquipmentHp()
    {
        int value = 0;
        for (int i = 0; i < equipmentContent.CharacterEquipment.Length; i++)
        {
            if (equipmentContent.CharacterEquipment[i] != null)
            {
                value += equipmentContent.CharacterEquipment[i].HP;
            }
        }
        return value;
    }
    public int getEquipmentStamina()
    {
        int value = 0;
        for (int i = 0; i < equipmentContent.CharacterEquipment.Length; i++)
        {
            if (equipmentContent.CharacterEquipment[i] != null)
            {
                value += equipmentContent.CharacterEquipment[i].Stamina;
            }
        }
        return value;
    }
    public int getEquipmentMana()
    {
        int value = 0;
        for (int i = 0; i < equipmentContent.CharacterEquipment.Length; i++)
        {
            if (equipmentContent.CharacterEquipment[i] != null)
            {
                value += equipmentContent.CharacterEquipment[i].Mana;
            }
        }
        return value;
    }
    public int getEquipmentDmg()
    {
        int value = 0;
        for (int i = 0; i < equipmentContent.CharacterEquipment.Length; i++)
        {
            if (equipmentContent.CharacterEquipment[i] != null)
            {
                value += equipmentContent.CharacterEquipment[i].Dmg;
            }
        }
        return value;
    }
    public int getEquipmentArmor()
    {
        int value = 0;
        for (int i = 0; i < equipmentContent.CharacterEquipment.Length; i++)
        {
            if (equipmentContent.CharacterEquipment[i] != null)
            {
                value += equipmentContent.CharacterEquipment[i].Armor;
            }
        }
        return value;
    }
}
