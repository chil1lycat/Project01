// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// UI_ConfirmPopup은 확인 팝업을 관리하는 UI 클래스입니다.
/// [1] 처음에는 비활성화 상태로 시작합니다.
/// [2] 다른 클래스에서 콜백 함수를 설정할 수 있습니다.
/// [3] 원하는 메시지를 표시하고 CloseButton을 클릭하면 팝업이 닫히고 연결된 콜백 함수가 호출됩니다.
/// [4] UI_Base를 상속받아 공통 규칙을 따릅니다.
/// </summary>

public class UI_ConfirmPopup : UI_Base
{
    [SerializeField] private TMP_Text _messageText; // 메시지를 표시하는 TextMeshPro
    [SerializeField] private Button _closeButton; // 팝업을 닫는 버튼

    private System.Action _onCloseCallback; // 팝업 닫기 시 호출되는 콜백 함수

    protected override void Awake()
    {
        base.Awake();
        // 처음에는 비활성화 상태로 시작
        gameObject.SetActive(false);

        // 자식 객체 및 컴포넌트 동적 탐색
        _messageText = FindChildComponent<TMP_Text>("MessageText");
        _closeButton = FindChildComponent<Button>("CloseButton");
    }

    protected override void Start()
    {
        base.Start();
        BindUIEvents();
    }

    /// <summary>
    /// UI 이벤트를 바인딩하는 함수입니다.
    /// </summary>
    protected override void BindUIEvents()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(() => ClosePopup());
        }
    }

    /// <summary>
    /// 팝업을 표시하고 메시지와 콜백을 설정합니다.
    /// </summary>
    /// <param name="message">팝업에 표시할 메시지</param>
    /// <param name="onCloseCallback">팝업 닫기 시 호출할 콜백 함수</param>
    public void Show(string message, System.Action onCloseCallback)
    {
        _messageText.text = message;
        _onCloseCallback = onCloseCallback;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 팝업을 닫고 콜백 함수를 호출합니다.
    /// </summary>
    private void ClosePopup()
    {
        gameObject.SetActive(false);
        _onCloseCallback?.Invoke();
    }
}
