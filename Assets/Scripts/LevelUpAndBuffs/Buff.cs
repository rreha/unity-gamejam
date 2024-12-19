using UnityEngine;

public enum BuffType
{
    ExplosionDamageIncrease,
    FireDamageIncrease,
    ElectricChainTimeIncrease,
    HealthIncrease,
    AttackSpeedIncrease,
    ElectricDamageIncrease,
    ManaIncrease,
    SteamDamagePerSecondIncrease,
    ManaRegenIncrease,
    WaterElectricDamagePerSecondIncrease,
    RadiusIncrease
    
    // Diðer buff türlerini buraya ekleyebilirsiniz
}

[System.Serializable]
public class Buff
{
    public BuffType buffType;
    public string buffName;
    public string buffDescription;
    public float buffValue; // Örneðin, explosionDamage +1 gibi
    public TargetType targetType;

    public enum TargetType
    {
        Player,
    }

    // Buff'u uygulayan metod
    public void ApplyBuff(Player playerStats)
    {
        switch (buffType)
        {
            case BuffType.ExplosionDamageIncrease:
                playerStats.IncreaseExplosionDamage(buffValue);
                break;
            case BuffType.FireDamageIncrease:
                playerStats.IncreaseFireDamage(buffValue);
                break;
            case BuffType.HealthIncrease:
                playerStats.IncreaseMaxHP(buffValue);
                break;
            case BuffType.AttackSpeedIncrease:
                playerStats.IncreaseAttackSpeed(buffValue);
                break;
            case BuffType.ElectricDamageIncrease:
                playerStats.IncreaseElectricDamage(buffValue);
                break;
            case BuffType.ManaIncrease:
                playerStats.IncreaseMaxMana(buffValue);
                break;
            case BuffType.ManaRegenIncrease:
                playerStats.IncreaseManaRegenRate(buffValue);
                break;
            case BuffType.ElectricChainTimeIncrease:
                playerStats.IncreaseElectricChainAmount(buffValue);
                break;
            case BuffType.SteamDamagePerSecondIncrease:
                playerStats.IncreaseSteamDamagePerSecond(buffValue);
                break;
            case BuffType.WaterElectricDamagePerSecondIncrease:
                playerStats.IncreaseWaterElectricDamage(buffValue);
                break;
            case BuffType.RadiusIncrease:
                playerStats.RadiusIncrease(buffValue);
                break ;

                // Diðer buff türlerini buraya ekleyebilirsiniz
        }
    }
}
