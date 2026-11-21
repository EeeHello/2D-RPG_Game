using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat_MajorGroup major;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;

    /// <summary>
    /// Method by order (Personal preference)
    /// 1. HP
    /// 2. Phys dmg
    /// 3. Armor/mitigation
    /// 4. ArmorReduction
    /// 5. Evasion
    /// 6. Elemental dmg
    /// 7. Elemental RES
    /// </summary>
    /// <returns></returns>
    public float GetMaxHealth()
    {
        float baseMaxHP = maxHealth.GetValue();
        float bonusMaxHP = major.vitality.GetValue() * 5f;

        float finalMaxHP = baseMaxHP + bonusMaxHP;

        return finalMaxHP;
    }

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1) //  with out, you can get info out of this method when called
    {
        float baseDamage = offense.physicalDamage.GetValue();
        float bonusDamage = major.strength.GetValue();

        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue();

        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critDamage.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f;

        float resultedCritDamage = (baseCritPower + bonusCritPower) / 100f; // toatal crit power multiplier (e.g 150/ 100 = 1.5f - multiplier)

        isCrit = Random.Range(0, 100) < critChance;

        float finalDMG = isCrit ? totalBaseDamage * resultedCritDamage : totalBaseDamage;

        return finalDMG * scaleFactor;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue(); // Bonus armor from vitality: adds +1 per VIT

        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Mathf.Clamp01(1 - armorReduction);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100f);
        float mitigationCap = 0.85f; // Max mitigation capped at 85%, need testing so can be change

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        // toatal armor reduction multiplier (e.g 30 / 100 = 0.3f - multiplier)
        float finalArmorReduction = offense.armorReduction.GetValue();

        return finalArmorReduction / 100f;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f; // each Point gives 0.5f of evasion

        float totalEvasion = baseEvasion + bonusEvasion;

        float evasionCap = 80f; // capped at 80%, need testing

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        float fireDMG = offense.fireDMG.GetValue();
        float iceDMG = offense.iceDMG.GetValue();
        float lightningDMG = offense.lightningDMG.GetValue();

        float bonusElementalDMG = major.intelligent.GetValue(); // Bonus E DMG from INT : adds + 1 per INT 

        float highestDMG = fireDMG;
        element = ElementType.Fire;

        if (iceDMG > highestDMG)
        {
            highestDMG = iceDMG;
            element = ElementType.Ice;
        }

        if (lightningDMG > highestDMG)
        {
            highestDMG = lightningDMG;
            element = ElementType.Lightning;
        }

        if (highestDMG <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = (fireDMG == highestDMG) ? 0 : fireDMG * 0.5f;
        float bonusIce = (iceDMG == highestDMG) ? 0 : iceDMG * 0.5f;
        float bonusLightning = (lightningDMG == highestDMG) ? 0 : lightningDMG * 0.5f;

        float weakerElementsDMG = bonusFire + bonusIce + bonusLightning;

        float finalElementalDMG = highestDMG + weakerElementsDMG + bonusElementalDMG;

        return finalElementalDMG * scaleFactor;
    }

    public float GetElementalRES(ElementType element)
    {
        float baseRES = 0;
        float bonusRES = major.intelligent.GetValue() * 0.5f; // Bonus RES from INT: 0.5% per INT

        switch (element)
        {
            case ElementType.Fire:
                baseRES = defense.fireRES.GetValue();
                break;
            case ElementType.Ice:
                baseRES = defense.iceRES.GetValue();
                break;
            case ElementType.Lightning:
                baseRES = defense.lightningRES.GetValue();
                break;
        }

        float resistance = baseRES + bonusRES;
        float resistanceCap = 75f;

        float finalRES = Mathf.Clamp(resistance, 0, resistanceCap) / 100; // Convert value into 0 to 1 multiplier

        return finalRES;
    }
}
