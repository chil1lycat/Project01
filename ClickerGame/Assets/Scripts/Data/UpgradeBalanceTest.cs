// 2025-11-12 AI-Tag
// 업그레이드 밸런스 테스트 스크립트

using UnityEngine;

/// <summary>
/// 업그레이드 밸런스를 테스트하고 확인하는 유틸리티 스크립트
/// </summary>
public class UpgradeBalanceTest : MonoBehaviour
{
    [Header("테스트 설정")]
    [SerializeField] private int testLevels = 20;

    [ContextMenu("GPC Tier 1 밸런스 테스트")]
    public void TestGPCTier1()
    {
        Debug.Log("=== GPC Tier 1 밸런스 테스트 ===");
        TestBalance(EUpgradeType.GPC, 1, 1.0, 1.15, 10.0, 1.3, 2.5);
    }

    [ContextMenu("GPS Tier 1 밸런스 테스트")]
    public void TestGPSTier1()
    {
        Debug.Log("=== GPS Tier 1 밸런스 테스트 ===");
        TestBalance(EUpgradeType.GPS, 1, 1.0, 1.15, 15.0, 1.35, 2.8);
    }

    [ContextMenu("모든 티어 밸런스 테스트")]
    public void TestAllTiers()
    {
        Debug.Log("=== 모든 티어 밸런스 테스트 ===");

        for (int tier = 1; tier <= 10; tier++)
        {
            Debug.Log($"\n--- Tier {tier} (배수: {GetTierMultiplier(tier)}) ---");
            Debug.Log("GPC:");
            TestBalance(EUpgradeType.GPC, tier, 1.0, 1.15, 10.0, 1.3, 2.5, 5);
            Debug.Log("GPS:");
            TestBalance(EUpgradeType.GPS, tier, 1.0, 1.15, 15.0, 1.35, 2.8, 5);
        }
    }

    private void TestBalance(EUpgradeType type, int tier, double baseEffect, double effectGrowthRate,
        double baseCost, double costGrowthRate, double costTierExponent, int levels = -1)
    {
        if (levels == -1) levels = testLevels;

        double tierMultiplier = GetTierMultiplier(tier);
        double currentEffect = 0;
        double totalEffect = 0;

        for (int level = 0; level < levels; level++)
        {
            // 현재 레벨의 효과
            currentEffect = System.Math.Ceiling(baseEffect * System.Math.Pow(effectGrowthRate, level) * tierMultiplier);

            // 다음 레벨 효과
            double nextEffect = System.Math.Ceiling(baseEffect * System.Math.Pow(effectGrowthRate, level + 1) * tierMultiplier);
            double effectIncrease = nextEffect - currentEffect;

            // 업그레이드 비용
            double cost = System.Math.Ceiling(baseCost * System.Math.Pow(costGrowthRate, level + 1) * System.Math.Pow(tierMultiplier, costTierExponent));

            totalEffect += effectIncrease;

            string effectType = type == EUpgradeType.GPC ? "G/터치" : "G/초";
            Debug.Log($"Lv{level}→{level + 1}: 비용 {FormatNumber(cost)}G, 효과 증가 +{FormatNumber(effectIncrease)}{effectType}, 누적 {FormatNumber(totalEffect)}{effectType}");
        }

        Debug.Log($"총 {levels}레벨 누적 효과: {FormatNumber(totalEffect)}{(type == EUpgradeType.GPC ? "G/터치" : "G/초")}");
    }

    private double GetTierMultiplier(int tier)
    {
        switch (tier)
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

    private string FormatNumber(double number)
    {
        number = System.Math.Ceiling(number);

        if (number < 1000)
            return number.ToString("F0");
        else if (number < 1000000)
            return System.Math.Ceiling(number / 1000).ToString("F0") + "K";
        else if (number < 1000000000)
            return System.Math.Ceiling(number / 1000000).ToString("F0") + "M";
        else if (number < 1000000000000)
            return System.Math.Ceiling(number / 1000000000).ToString("F0") + "B";
        else
            return System.Math.Ceiling(number / 1000000000000).ToString("F0") + "T";
    }
}
