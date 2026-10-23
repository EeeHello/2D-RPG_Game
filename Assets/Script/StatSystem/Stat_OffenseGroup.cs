using System;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    // Phys DMG
    public Stat physicalDamage;
    public Stat critDamage;
    public Stat critChance;

    // Elemental DMG
    public Stat fireDMG;
    public Stat iceDMG;
    public Stat lightningDMG;
}

