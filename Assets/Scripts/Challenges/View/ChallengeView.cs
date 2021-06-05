using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeView : MonoBehaviour
{
    string sBaseText = "{0}/{1}";


    [SerializeField] TextMeshProUGUI challengeTitle;
    [SerializeField] TextMeshProUGUI challengeText;
    [SerializeField] Slider challengeProgressBar;
    [SerializeField] TextMeshProUGUI challengeProgressText;

    public void SetupView(Challenge pChallenge)
    {
        challengeTitle.text = pChallenge.Name;
        challengeProgressBar.maxValue = pChallenge.Value;
        challengeProgressText.text = string.Format(sBaseText, ChallengeController.GetChallengeProgression(pChallenge), pChallenge.Value);
        challengeProgressBar.value = 0;
    }

}
