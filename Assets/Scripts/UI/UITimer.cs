using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITimer : MonoBehaviour
{
    public GameConfiguration _configurationAsset;
    private float _fMaxTime;
    private float _fDeltaTime;
    private TextMeshProUGUI _textField;
    
    public float MaxTime => _fMaxTime;
    public float DeltaTime => _fDeltaTime;

    private void Start()
    {
        _fDeltaTime = _fMaxTime = _configurationAsset.SecondsToGameEnd;
        _textField = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        _fDeltaTime -= Time.deltaTime;
        if (_fDeltaTime >= 0)
        {
            _textField.text = ConvertSecondsToMinutes(_fDeltaTime);
        }
        else
        {
            OnTimerEnd();
        }
    }

    private void OnTimerEnd()
    {
        if (!GameController.Instance.IsGameEnded)
        {
            GameController.Instance.GameEnd();
            _textField.gameObject.SetActive(false);
        }
    }

    private string ConvertSecondsToMinutes(float fSeconds)
    {
        string sBaseText = "{0}:{1}";

        return string.Format(sBaseText, (int)fSeconds / 60, ((int)(fSeconds % 60)).ToString("00"));
    }

}
