using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{

    public GameConfiguration _configurationAsset;
    public List<ChallengeView> _actualChallenges;

    [SerializeField]  private int _actualPoints;

    [SerializeField] GameObject _challengeViewPrefab;
    [SerializeField] Transform _ChallengelistRoot;

    private void Start()
    {
        SetupChallengeViews();
        // NetworkController.Instance.OnGameState += UpdateChallenges;
        _actualPoints = 0;
    }

    public int CalculateFinalScore()
    {
        int score = _actualPoints;

        for(int i = 0; i < _configurationAsset.Scores.Count; i++)
        {
            Score pScore = _configurationAsset.Scores[i];
            int iValue = 0;
            switch ((ScoreType)pScore.ScoreType)
            {
                case ScoreType.PeopleInCity:
                    iValue = GetPeopleInCity();
                    break;
                case ScoreType.CityWithoutPeople:
                    iValue = (GetPeopleInCity() > 0) ? 0 : 1;
                    break;
                case ScoreType.IdealBuilding:
                    iValue = GetIdealBuildings();
                    break;
                case ScoreType.NonIdealBuilding:
                    iValue = GetNonIdealBuildings();
                    break;
            }

            iValue *= pScore.Amount;
            score += iValue;
        }
        return score;
    }
       
    public static bool CheckAmount(Challenge challenge)
    {
        int progression = GetChallengeProgression(challenge);

        if (progression == -1)
            return false;

        switch ((ComparisonType) challenge.ComparisonType)
        {
            case ComparisonType.Equal:
                return progression == challenge.Value;
            case ComparisonType.GreaterEqual:
                return progression >= challenge.Value;
            case ComparisonType.LesserEqual:
                return progression <= challenge.Value;
        }
        return false;
    }

    public static int GetChallengeProgression(Challenge challenge)
    {
        ChallengeType pType = (ChallengeType)challenge.ChallengeType;
        switch (pType)
        {
            case ChallengeType.Demolish:
                return GetDemolishedBuildings();
            case ChallengeType.Damage:
                return GetDamageDealt();
            case ChallengeType.GrowPlant:
                return GetPlantCount();
            case ChallengeType.Graffiti:
                return GetGraffitiCount();
            default:
                return -1;
        }
    }

    private void OnChallengeComplete(Challenge pChallenge)
    {
        switch ((RewardType)pChallenge.RewardType)
        {
            case RewardType.Score:
                _actualPoints += pChallenge.RewardAmount;
                break;
        }
    }

    public void UpdateChallenges()
    {
        for (int i = 0; i < _actualChallenges.Count; i++)
        {
            _actualChallenges[i].UpdateProgress();
        }
    }

    private void SetupChallengeViews()
    {
        _actualChallenges = new List<ChallengeView>();
        for (int i = 0; i < _configurationAsset.Challenges.Count; i++)
        {
            var challengeView = Instantiate(_challengeViewPrefab, _ChallengelistRoot).GetComponent<ChallengeView>();
            challengeView.SetupView(_configurationAsset.Challenges[i]);
            challengeView.OnChallengeComplete += OnChallengeComplete;
            _actualChallenges.Add(challengeView);
        }
    }

    private static int GetDemolishedBuildings()
    {
        int demolishedCount = 0;
        BuildingController pBuilding;
        List<BuildingController> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            demolishedCount += (pBuilding.Demolished)? 1: 0;
        }

        return demolishedCount;
    }

    private static int GetDamageDealt()
    {
        int demolishedCount = 0;
        BuildingController pBuilding;
        List<BuildingController> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            demolishedCount += pBuilding.DamageTaken;
        }

        return demolishedCount;
    }

    private static int GetPlantCount()
    {
        int plantCount = 0;
        BuildingController pBuilding;
        List<BuildingController> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            plantCount += pBuilding.Naturalized;
        }

        return plantCount;
    }

    private static int GetGraffitiCount()
    {
        int graffitiCount = 0;
        BuildingController pBuilding;
        List<BuildingController> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            graffitiCount += pBuilding.Graffiti;
        }

        return graffitiCount;
    }

    private static int GetPeopleInCity()
    {
        int peopleCount = 0;
        BuildingController pBuilding;
        List<BuildingController> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            peopleCount += pBuilding.PeopleInBuilding;
        }
        return peopleCount;
    }

    private static int GetIdealBuildings()
    {
        int idealBuildings = 0;
        BuildingController pBuilding;
        List<BuildingController> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            idealBuildings += pBuilding.IsIdeal() ? 1 : 0;
        }
        return idealBuildings;
    }

    private static int GetNonIdealBuildings()
    {
        return GameController.Instance.Buildings.Count - GetIdealBuildings();
    }
}
