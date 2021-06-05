using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeListView : MonoBehaviour
{
    [SerializeField] Animator _ListViewAnimator;
    [SerializeField] Button _ToggleButton;
    [SerializeField] Text _ToggleButtonText;

    bool bIsEnabled;

    private void Start()
    {
        _ToggleButton.onClick.AddListener(ToggleChallengeListView);
        bIsEnabled = false;
    }

    public void ToggleChallengeListView()
    {
        if(bIsEnabled)
        {
            _ListViewAnimator.Play("Hide");
        }
        else
        {
            _ListViewAnimator.Play("Show");
        }
        bIsEnabled = !bIsEnabled;
    }
}
