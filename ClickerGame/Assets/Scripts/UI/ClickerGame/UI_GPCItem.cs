// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.
// 2025-11-12 Updated: ScriptableObject 데이터 기반으로 변경

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI_GPCItem은 GPC/GPS 업그레이드 아이템의 UI를 관리하는 클래스입니다.
/// ScriptableObject (GPCItemData)를 통해 업그레이드 데이터를 관리합니다.
///
/// 기능:
/// [1] GPCItemData를 기반으로 업그레이드 수치 표시 및 처리
/// [2] 버튼 클릭 시 업그레이드 실행 및 골드 차감
/// [3] UI 텍스트 자동 업데이트 (아이템명, 현재 효과, 업그레이드 효과, 비용)
/// [4] 업그레이드 가능 여부 체크 (골드 부족, 최대 레벨)
/// </summary>
public class UI_GPCItem : UI_Base
{
    [Header("데이터")]
    public GPCItemData _itemData; // 업그레이드 아이템 데이터 (Inspector에서 설정)

    [Header("UI 요소 (자동으로 찾음)")]
    private Button _clickButton; // 업그레이드 버튼
    private TMP_Text _itemNameText; // 아이템 이름 텍스트
    private TMP_Text _currentStatText; // 현재 효과 텍스트
    private TMP_Text _upgradeStatText; // 업그레이드 효과 텍스트
    private TMP_Text _upgradeCostText; // 업그레이드 비용 텍스트

    protected override void Awake()
    {
        base.Awake();
        // 자식 오브젝트 동적 탐색
        _clickButton = FindChildComponent<Button>("ClickButton");
        _itemNameText = FindChildComponent<TMP_Text>("GPCITEMText");
        _currentStatText = FindChildComponent<TMP_Text>("GPCSTATText");
        _upgradeStatText = FindChildComponent<TMP_Text>("GPCUPGStat");
        _upgradeCostText = FindChildComponent<TMP_Text>("GPCUPGCost");
    }

    protected override void Start()
    {
        base.Start();
        BindUIEvents();
        RefreshUI();
    }

    protected override void Update()
    {
        base.Update();
        // 매 프레임 버튼 활성화 상태 체크 (골드가 충분한지)
        UpdateButtonState();
    }

    /// <summary>
    /// UI 이벤트를 바인딩합니다.
    /// </summary>
    protected override void BindUIEvents()
    {
        if (_clickButton != null)
        {
            _clickButton.onClick.RemoveAllListeners();
            _clickButton.onClick.AddListener(OnUpgradeButtonClick);
        }
    }

    /// <summary>
    /// UI를 새로고침합니다.
    /// </summary>
    protected override void RefreshUI()
    {
        base.RefreshUI();

        if (_itemData == null)
        {
            Debug.LogWarning($"[UI_GPCItem] ItemData가 설정되지 않았습니다: {gameObject.name}");
            return;
        }

        // 아이템 이름 표시
        if (_itemNameText != null)
        {
            _itemNameText.text = _itemData.itemName;
        }

        // 현재 이 아이템이 터치당 얼마나 골드를 벌게 해주는지 표시
        if (_currentStatText != null)
        {
            double currentEffect = _itemData.GetCurrentEffect();
            string effectType = _itemData.upgradeType == EUpgradeType.GPC ? "G/터치" : "G/초";

            if (_itemData.CurrentLevel == 0 || currentEffect == 0)
            {
                // 레벨 0이거나 효과가 0이면 "0G/터치" 형식으로 표시
                _currentStatText.text = $"0{effectType}";
            }
            else
            {
                // 현재 이 아이템이 제공하는 효과 표시
                _currentStatText.text = $"+{FormatNumber(currentEffect)}{effectType}";
            }
        }

        // 업그레이드 효과 표시
        if (_upgradeStatText != null)
        {
            if (_itemData.IsMaxLevel())
            {
                _upgradeStatText.text = "MAX";
            }
            else
            {
                double effectIncrease = _itemData.GetEffectIncrease();
                string effectType = _itemData.upgradeType == EUpgradeType.GPC ? "G/터치" : "G/초";
                _upgradeStatText.text = $"+{FormatNumber(effectIncrease)}{effectType}";
            }
        }

        // 업그레이드 비용 표시
        if (_upgradeCostText != null)
        {
            if (_itemData.IsMaxLevel())
            {
                _upgradeCostText.text = "MAX";
            }
            else
            {
                double upgradeCost = _itemData.GetUpgradeCost();
                _upgradeCostText.text = $"{FormatNumber(upgradeCost)}G";
            }
        }

        UpdateButtonState();
    }

    /// <summary>
    /// 업그레이드 버튼 클릭 시 호출됩니다.
    /// </summary>
    private void OnUpgradeButtonClick()
    {
        if (_itemData == null) return;

        // 최대 레벨 체크
        if (_itemData.IsMaxLevel())
        {
            Debug.Log($"[UI_GPCItem] {_itemData.itemName}는 이미 최대 레벨입니다.");
            return;
        }

        // 골드 체크
        double upgradeCost = _itemData.GetUpgradeCost();
        if (GameManager.Instance.Gold < upgradeCost)
        {
            Debug.Log($"[UI_GPCItem] 골드가 부족합니다. 필요: {upgradeCost}, 보유: {GameManager.Instance.Gold}");
            return;
        }

        // 업그레이드 실행
        double effectIncrease = _itemData.GetEffectIncrease();

        // 골드 차감
        GameManager.Instance.Gold -= upgradeCost;

        // 효과 적용
        if (_itemData.upgradeType == EUpgradeType.GPC)
        {
            GameManager.Instance.GoldPerClick += effectIncrease;
        }
        else // GPS
        {
            GameManager.Instance.GoldPerSecond += effectIncrease;
        }

        // 레벨 증가
        _itemData.Upgrade();

        // UI 갱신
        RefreshUI();

        Debug.Log($"[UI_GPCItem] {_itemData.itemName} 업그레이드 완료! Lv.{_itemData.CurrentLevel}, 효과 증가: +{effectIncrease}");
    }

    /// <summary>
    /// 버튼 활성화 상태를 업데이트합니다.
    /// </summary>
    private void UpdateButtonState()
    {
        if (_clickButton == null || _itemData == null) return;

        // 최대 레벨이면 비활성화
        if (_itemData.IsMaxLevel())
        {
            _clickButton.interactable = false;
            return;
        }

        // 골드가 충분하면 활성화, 아니면 비활성화
        double upgradeCost = _itemData.GetUpgradeCost();
        _clickButton.interactable = GameManager.Instance.Gold >= upgradeCost;
    }

    /// <summary>
    /// 숫자를 읽기 쉬운 형식으로 변환합니다.
    /// 모든 숫자는 올림하여 정수로 표시합니다.
    /// 예: 1000 -> 1K, 1000000 -> 1M, 1000000000 -> 1B
    /// </summary>
    private string FormatNumber(double number)
    {
        // 올림 처리
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

    /// <summary>
    /// 외부에서 ItemData를 설정할 수 있는 함수입니다.
    /// </summary>
    public void SetItemData(GPCItemData itemData)
    {
        _itemData = itemData;
        RefreshUI();
    }

    /// <summary>
    /// 현재 ItemData를 반환합니다.
    /// </summary>
    public GPCItemData GetItemData()
    {
        return _itemData;
    }
}
