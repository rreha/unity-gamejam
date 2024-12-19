using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    public List<Buff> allBuffs = new List<Buff>();

    void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }



        // Örnek buff eklemeleri
        allBuffs.Add(new Buff
        {
            buffType = BuffType.ExplosionDamageIncrease,
            buffName = "Explosion Damage +5",
            buffDescription = "Increases explosion damage by 5.",
            buffValue = 5,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.FireDamageIncrease,
            buffName = "Fire Damage +1",
            buffDescription = "Increases fire damage per second by 1.",
            buffValue = 1,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.ElectricChainTimeIncrease,
            buffName = "Electric Chain Time +1",
            buffDescription = "Increases electric chain time by 1.",
            buffValue = 1,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.HealthIncrease,
            buffName = "Health +10",
            buffDescription = "Increases maximum health by 10.",
            buffValue = 10,
            targetType = Buff.TargetType.Player
        });

        allBuffs.Add(new Buff
        {
            buffType = BuffType.AttackSpeedIncrease,
            buffName = "Attack Speed +0,5",
            buffDescription = "Increases attack speed by 0,5.",
            buffValue = 0.5f,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.ElectricDamageIncrease,
            buffName = "Electric Chain Damage +2",
            buffDescription = "Increases electric chain damage by 2.",
            buffValue = 2,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.ManaIncrease,
            buffName = "Max Mana +20",
            buffDescription = "Increases maximum mana by 20.",
            buffValue = 20,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.ManaRegenIncrease,
            buffName = "Mana Regen +2",
            buffDescription = "Increases mana regeneration rate by 2 per second.",
            buffValue = 2,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.SteamDamagePerSecondIncrease,
            buffName = "Steam Damage Per Second +1",
            buffDescription = "Increases steam damage per second by 1.",
            buffValue = 1,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.WaterElectricDamagePerSecondIncrease,
            buffName = "Arcflow Damage Per Second +1",
            buffDescription = "Increases Arcflow Damage Per Second by 1.",
            buffValue = 1,
            targetType = Buff.TargetType.Player
        });
        allBuffs.Add(new Buff
        {
            buffType = BuffType.RadiusIncrease,
            buffName = "Radius Increase",
            buffDescription = "Increases radius by 0.8.",
            buffValue = 0.8f,
            targetType = Buff.TargetType.Player
        });
        // Diðer bufflarý buraya ekleyin
    }

    // Rastgele 3 farklý buff seçen metod
    public List<Buff> GetRandomBuffs(int numberOfBuffs)
    {
        List<Buff> selectedBuffs = new List<Buff>();
        List<Buff> availableBuffs = new List<Buff>(allBuffs);

        for (int i = 0; i < numberOfBuffs; i++)
        {
            if (availableBuffs.Count == 0)
                break;

            int randomIndex = Random.Range(0, availableBuffs.Count);
            selectedBuffs.Add(availableBuffs[randomIndex]);
            availableBuffs.RemoveAt(randomIndex); // Ayný buff'ýn tekrar seçilmemesi için
        }

        return selectedBuffs;
    }
}
