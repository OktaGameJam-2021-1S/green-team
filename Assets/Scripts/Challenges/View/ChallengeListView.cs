using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeListView : MonoBehaviour
{
    [SerializeField] Animator _ListViewAnimator;
    [SerializeField] Image _ToggleButtonIcon;

    bool bIsEnabled;

    [SerializeField] Sprite _ToggleOn;
    [SerializeField] Sprite _ToggleOff;

    private void Start()
    {
        bIsEnabled = false;
    }

    public void ToggleChallengeListView()
    {
        if(bIsEnabled)
        {
            _ListViewAnimator.Play("Hide");
            _ToggleButtonIcon.sprite = _ToggleOn;
        }
        else
        {
            _ListViewAnimator.Play("Show");
            _ToggleButtonIcon.sprite = _ToggleOff;
        }
        bIsEnabled = !bIsEnabled;
    }
}
