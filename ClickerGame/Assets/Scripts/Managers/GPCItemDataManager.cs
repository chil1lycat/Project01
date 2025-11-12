// 2025-11-12 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GPCItemDataManager는 모든 GPCItemData를 관리하는 매니저입니다.
/// 게임 시작 시 모든 GPCItemData를 로드하고,
/// 세이브/로드 시 데이터를 저장/복원하며,
/// 초기화 시 모든 레벨을 리셋합니다.
/// </summary>
public class GPCItemDataManager : Singleton<GPCItemDataManager>
{
    [Header("GPC/GPS 아이템 데이터")]
    [Tooltip("모든 GPC/GPS 아이템 데이터 목록")]
    [SerializeField] private List<GPCItemData> _allItemData = new List<GPCItemData>();

    /// <summary>
    /// 모든 아이템 데이터 목록을 가져옵니다.
    /// </summary>
    public List<GPCItemData> AllItemData => _allItemData;

    protected override void Awake()
    {
        base.Awake();

        // Resources 폴더에서 모든 GPCItemData를 자동으로 로드
        if (_allItemData.Count == 0)
        {
            LoadAllItemDataFromResources();
        }
    }

    /// <summary>
    /// Resources 폴더에서 모든 GPCItemData를 로드합니다.
    /// </summary>
    private void LoadAllItemDataFromResources()
    {
        GPCItemData[] itemDataArray = Resources.LoadAll<GPCItemData>("");
        _allItemData.AddRange(itemDataArray);

        Debug.Log($"[GPCItemDataManager] Loaded {_allItemData.Count} GPCItemData assets from Resources.");
    }

    /// <summary>
    /// 모든 아이템 데이터의 레벨을 초기화합니다.
    /// </summary>
    public void ResetAllLevels()
    {
        foreach (var itemData in _allItemData)
        {
            if (itemData != null)
            {
                itemData.ResetLevel();
            }
        }

        Debug.Log("[GPCItemDataManager] All item levels have been reset to 0.");
    }

    /// <summary>
    /// 특정 타입과 티어의 아이템 데이터를 찾습니다.
    /// </summary>
    public GPCItemData FindItemData(EUpgradeType upgradeType, int tier)
    {
        return _allItemData.Find(data =>
            data != null &&
            data.upgradeType == upgradeType &&
            data.tier == tier
        );
    }

    /// <summary>
    /// 모든 아이템 데이터의 레벨 정보를 Dictionary로 반환합니다.
    /// Key: "GPC_1", "GPS_2" 등의 형식
    /// Value: 현재 레벨
    /// </summary>
    public Dictionary<string, int> GetAllLevels()
    {
        Dictionary<string, int> levels = new Dictionary<string, int>();

        foreach (var itemData in _allItemData)
        {
            if (itemData != null)
            {
                string key = $"{itemData.upgradeType}_{itemData.tier}";
                levels[key] = itemData.CurrentLevel;
            }
        }

        return levels;
    }

    /// <summary>
    /// Dictionary로부터 모든 아이템 데이터의 레벨을 복원합니다.
    /// </summary>
    public void LoadAllLevels(Dictionary<string, int> levels)
    {
        if (levels == null) return;

        foreach (var itemData in _allItemData)
        {
            if (itemData != null)
            {
                string key = $"{itemData.upgradeType}_{itemData.tier}";
                if (levels.ContainsKey(key))
                {
                    // 레벨을 직접 설정
                    itemData.SetLevel(levels[key]);
                }
                else
                {
                    // 세이브 데이터에 없으면 0으로 초기화
                    itemData.ResetLevel();
                }
            }
        }

        Debug.Log("[GPCItemDataManager] All item levels have been loaded from save data.");
    }

    /// <summary>
    /// 특정 아이템 데이터를 목록에 추가합니다.
    /// (런타임에서 동적으로 추가할 때 사용)
    /// </summary>
    public void AddItemData(GPCItemData itemData)
    {
        if (itemData != null && !_allItemData.Contains(itemData))
        {
            _allItemData.Add(itemData);
        }
    }

    /// <summary>
    /// 특정 아이템 데이터를 목록에서 제거합니다.
    /// </summary>
    public void RemoveItemData(GPCItemData itemData)
    {
        if (itemData != null)
        {
            _allItemData.Remove(itemData);
        }
    }
}
