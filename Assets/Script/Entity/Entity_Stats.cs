using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat_MajorGroup major;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;

    public float GetMaxHealth()
    {
        float baseMaxHP = maxHealth.GetValue();
        float bonusMaxHP = major.vitality.GetValue() * 5f;

        float finalMaxHP = baseMaxHP + bonusMaxHP;

        return finalMaxHP;
    }

    public float GetPhysicalDamage(out bool isCrit) //  with out, you can get info out of this method when called
    {
        float baseDamage = offense.physicalDamage.GetValue();
        float bonusDamage = major.strength.GetValue();

        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance =  offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue(); 

        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critDamage.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f;

        float resultedCritDamage = (baseCritPower + bonusCritPower) / 100; // toatal crit power multiplier (e.g 150/ 100 = 1.5f - multiplier)

        isCrit = Random.Range(0, 100) < critChance; 

        float finalDMG = isCrit ? totalBaseDamage * resultedCritDamage : totalBaseDamage;

        return finalDMG;
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
}
