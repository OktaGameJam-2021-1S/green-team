using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeView : MonoBehaviour
{
    string sBaseText = "{0}/{1}";

    Challenge _ReferenceChallenge;

    bool bIsCompleted;

    [SerializeField] Image challengeImage;
    [SerializeField] Slider challengeProgressBar;
    [SerializeField] TextMeshProUGUI challengeProgressText;
    [SerializeField] GenericDictionary<ChallengeType, Sprite> _challengeSprite;

    public delegate void OnCompleteHandler(Challenge pChallenge);
    public event OnCompleteHandler OnChallengeComplete;

    public void SetupView(Challenge pChallenge)
    {
        bIsCompleted = false;
        _ReferenceChallenge = pChallenge;
        challengeImage.sprite = _challengeSprite[(ChallengeType)pChallenge.ChallengeType];
        challengeProgressBar.maxValue = pChallenge.Value;
        UpdateProgress();
    }


    public void UpdateProgress()
    {
        int iChallengeProgression = Mathf.Min(ChallengeController.GetChallengeProgression(_ReferenceChallenge), _ReferenceChallenge.Value);
        challengeProgressText.text = string.Format(sBaseText, iChallengeProgression, _ReferenceChallenge.Value);
        challengeProgressBar.value = iChallengeProgression;


        if (ChallengeController.CheckAmount(_ReferenceChallenge) && !bIsCompleted)
        {
            bIsCompleted = true;
            challengeProgressText.color = Color.green;
            OnChallengeComplete?.Invoke(_ReferenceChallenge);
        }

    }

}
