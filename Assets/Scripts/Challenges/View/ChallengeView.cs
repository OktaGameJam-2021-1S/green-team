using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeView : MonoBehaviour
{
    string sBaseText = "{0}/{1}";

    Challenge _ReferenceChallenge;

    [SerializeField] Image challengeImage;
    [SerializeField] Slider challengeProgressBar;
    [SerializeField] TextMeshProUGUI challengeProgressText;
    [SerializeField] GenericDictionary<ChallengeType, Sprite> _challengeSprite;

    public void SetupView(Challenge pChallenge)
    {
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


        if (ChallengeController.CheckAmount(_ReferenceChallenge))
        {
            challengeProgressText.color = Color.green;
        }

    }

}
