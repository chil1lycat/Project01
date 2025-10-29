using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReviveButton : MonoBehaviour
{
    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Revive();
        }
    }
}
