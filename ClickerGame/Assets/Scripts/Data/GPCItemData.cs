// 2025-11-12 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;

/// <summary>
/// GPCItemData는 GPC/GPS 업그레이드 아이템의 데이터를 저장하는 ScriptableObject입니다.
/// Inspector에서 각 티어별 업그레이드 수치를 조정할 수 있습니다.
///
/// 공식:
/// - 효과 = baseEffect × (effectGrowthRate ^ currentLevel) × tierMultiplier
/// - 비용 = baseCost × (costGrowthRate ^ currentLevel) × (tierMultiplier ^ costTierExponent)
///
/// 밸런스:
/// - 각 티어는 최대 20레벨까지 업그레이드 가능
/// - 10개의 티어로 구성
/// - 30일 플레이타임 기준 밸런싱
/// </summary>
[CreateAssetMenu(fileName = "GPCItemData", menuName = "ClickerGame/GPCItemData", order = 1)]
public class GPCItemData : ScriptableObject
{
    [Header("기본 정보")]
    [Tooltip("아이템 이름 (예: '기본 지압법')")]
    public string itemName = "기본 아이템";

    [Tooltip("업그레이드 타입 (GPC 또는 GPS)")]
    public EUpgradeType upgradeType = EUpgradeType.GPC;

    [Tooltip("티어 (1~10)")]
    [Range(1, 10)]
    public int tier = 1;

    [Header("레벨 설정")]
    [Tooltip("최대 업그레이드 레벨")]
    public int maxLevel = 20;

    // 런타임 레벨은 저장하지 않음 (ScriptableObject 에셋을 수정하지 않기 위해)
    [System.NonSerialized] private int _runtimeLevel = 0;

    [Header("효과 설정")]
    [Tooltip("기본 효과 (Tier 1 Level 1의 효과)")]
    public double baseEffect = 1.0;

    [Tooltip("레벨당 효과 증가율 (1.15 = 15% 증가)")]
    public double effectGrowthRate = 1.15;

    [Header("비용 설정")]
    [Tooltip("기본 비용 (Tier 1 Level 1의 비용)")]
    public double baseCost = 10.0;

    [Tooltip("레벨당 비용 증가율 (GPC: 1.3, GPS: 1.35 권장)")]
    public double costGrowthRate = 1.3;

    [Tooltip("티어별 비용 지수 (GPC: 2.5, GPS: 2.8 권장)")]
    public double costTierExponent = 2.5;

    [Header("티어별 배수 (자동 계산됨)")]
    [Tooltip("티어별 효과/비용 배수")]
    [SerializeField] private double _tierMultiplier = 1.0;

    /// <summary>
    /// 현재 레벨을 가져옵니다.
    /// </summary>
    public int CurrentLevel => _runtimeLevel;

    /// <summary>
    /// 티어별 배수를 가져옵니다.
    /// Tier 1: 1, Tier 2: 3, Tier 3: 10, Tier 4: 30, Tier 5: 100
    /// Tier 6: 300, Tier 7: 1000, Tier 8: 3000, Tier 9: 10000, Tier 10: 30000
    /// </summary>
    public double TierMultiplier => _tierMultiplier;

    /// <summary>
    /// 현재 레벨에서의 효과를 계산합니다.
    /// 효과 = baseEffect × (effectGrowthRate ^ currentLevel) × tierMultiplier (올림 처리)
    /// </summary>
    public double GetCurrentEffect()
    {
        if (_runtimeLevel == 0) return 0; // 레벨 0일 때는 효과 없음
        double effect = baseEffect * System.Math.Pow(effectGrowthRate, _runtimeLevel) * _tierMultiplier;
        return System.Math.Ceiling(effect);
    }

    /// <summary>
    /// 다음 레벨로 업그레이드 시 증가할 효과를 계산합니다.
    /// </summary>
    public double GetNextLevelEffect()
    {
        if (_runtimeLevel >= maxLevel) return 0;
        double effect = baseEffect * System.Math.Pow(effectGrowthRate, _runtimeLevel + 1) * _tierMultiplier;
        return System.Math.Ceiling(effect);
    }

    /// <summary>
    /// 다음 레벨로 업그레이드 시 추가되는 효과량을 계산합니다.
    /// 올림 처리 후 차이를 계산하면 0이 될 수 있으므로, 올림 전 차이를 계산 후 올림합니다.
    /// </summary>
    public double GetEffectIncrease()
    {
        if (_runtimeLevel >= maxLevel) return 0;

        // 올림 전 원래 값으로 차이를 계산
        double currentRaw = baseEffect * System.Math.Pow(effectGrowthRate, _runtimeLevel) * _tierMultiplier;
        double nextRaw = baseEffect * System.Math.Pow(effectGrowthRate, _runtimeLevel + 1) * _tierMultiplier;
        double increaseRaw = nextRaw - currentRaw;

        // 증가량을 올림 (최소 1 보장)
        return System.Math.Max(1, System.Math.Ceiling(increaseRaw));
    }

    /// <summary>
    /// 현재 레벨에서 다음 레벨로 업그레이드하는데 필요한 비용을 계산합니다.
    /// 비용 = baseCost × (costGrowthRate ^ (currentLevel + 1)) × (tierMultiplier ^ costTierExponent) (올림 처리)
    /// </summary>
    public double GetUpgradeCost()
    {
        if (_runtimeLevel >= maxLevel) return double.MaxValue;
        double cost = baseCost * System.Math.Pow(costGrowthRate, _runtimeLevel + 1) * System.Math.Pow(_tierMultiplier, costTierExponent);
        return System.Math.Ceiling(cost);
    }

    /// <summary>
    /// 업그레이드를 수행합니다.
    /// </summary>
    /// <returns>업그레이드 성공 여부</returns>
    public bool Upgrade()
    {
        if (_runtimeLevel >= maxLevel) return false;
        _runtimeLevel++;
        return true;
    }

    /// <summary>
    /// 레벨을 초기화합니다. (게임 리셋 시 사용)
    /// </summary>
    public void ResetLevel()
    {
        _runtimeLevel = 0;
    }

    /// <summary>
    /// 레벨을 직접 설정합니다. (세이브 로드 시 사용)
    /// </summary>
    public void SetLevel(int level)
    {
        _runtimeLevel = UnityEngine.Mathf.Clamp(level, 0, maxLevel);
    }

    /// <summary>
    /// 최대 레벨에 도달했는지 확인합니다.
    /// </summary>
    public bool IsMaxLevel()
    {
        return _runtimeLevel >= maxLevel;
    }

    /// <summary>
    /// Unity Editor에서 값이 변경될 때 호출됩니다.
    /// 티어에 따라 자동으로 tierMultiplier를 계산합니다.
    /// </summary>
    private void OnValidate()
    {
        // 티어별 배수 자동 계산
        _tierMultiplier = CalculateTierMultiplier(tier);

        // 티어 범위 제한
        if (tier < 1) tier = 1;
        if (tier > 10) tier = 10;
    }

    /// <summary>
    /// 티어에 따른 배수를 계산합니다.
    /// </summary>
    private double CalculateTierMultiplier(int tierValue)
    {
        switch (tierValue)
        {
            case 1: return 1;
            case 2: return 3;
            case 3: return 10;
            case 4: return 30;
            case 5: return 100;
            case 6: return 300;
            case 7: return 1000;
            case 8: return 3000;
            case 9: return 10000;
            case 10: return 30000;
            default: return 1;
        }
    }
}

/// <summary>
/// 업그레이드 타입 열거형
/// </summary>
public enum EUpgradeType
{
    GPC,  // Gold Per Click
    GPS   // Gold Per Second
}
