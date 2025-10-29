// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// UI_ProcessItem은 프로세스 업그레이드 아이템을 관리하는 UI 클래스입니다.
/// [1] GameManager의 코드를 이해합니다.
/// [2] Text 대신 TextMeshPro를 사용합니다.
/// [3] private 변수에도 [SerializeField]를 사용하여 Unity Editor에서 디버깅이 가능하도록 합니다.
/// [4] UnityEngine, UnityEngine.UI, TMPro 네임스페이스를 필수적으로 사용합니다.
/// [5] UI_ConfirmPopup을 활용하여 확인 팝업을 표시합니다.
/// [6] 팝업이 닫힐 때 Stage를 1 증가시킵니다.
/// [7] UI_ConfirmPopup은 차일드 오브젝트가 아닙니다.
/// </summary>

public class UI_ProcessItem : UI_Base
{
    [SerializeField] private EProcessUpgradeType _processUpgradeType; // ProcessUpgrade 타입, 유니티 에디터에서 설정 가능
    [SerializeField] private Button _clickButton; // 프로세스 업그레이드 버튼
    [SerializeField] private UI_ConfirmPopup _confirmPopup; // 확인 팝업 참조

    protected override void Awake()
    {
        base.Awake();
        // 자식 객체 및 컴포넌트 동적 탐색
        _clickButton = FindChildComponent<Button>("ClickButton");

        // UI_ConfirmPopup을 씬에서 찾음 (차일드 오브젝트가 아님)
        _confirmPopup = FindAnyObjectByType<UI_ConfirmPopup>(FindObjectsInactive.Include);
    }

    protected override void Start()
    {
        base.Start();
        BindUIEvents();
    }

    protected override void Update()
    {
        base.Update();
        // 추가적인 프레임별 로직이 필요하다면 구현
    }

    /// <summary>
    /// UI 이벤트를 바인딩하는 함수입니다.
    /// </summary>
    protected override void BindUIEvents()
    {
        if (_clickButton != null)
        {
            _clickButton.onClick.RemoveAllListeners();
            _clickButton.onClick.AddListener(() => ShowConfirmPopup());
        }
    }

    /// <summary>
    /// 확인 팝업을 표시하고, 팝업이 닫힐 때 Stage를 증가시킵니다.
    /// </summary>
    private void ShowConfirmPopup()
    {
        if (_confirmPopup != null)
        {
            _confirmPopup.Show("Are you sure you want to upgrade the stage?", () =>
            {
                GameManager.Instance.Stage += 1;
            });
        }
        else
        {
            Debug.LogError("UI_ConfirmPopup not found in the scene.");
        }
    }
}
