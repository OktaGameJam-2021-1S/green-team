using System.Collections.Generic;
using UnityEngine;

public class ChallengeController : MonoBehaviour
{

    public GameConfiguration _configurationAsset;
    public List<ChallengeView> _actualChallenges;

    [SerializeField]  private int _actualPoints;

    [SerializeField] GameObject _challengeViewPrefab;
    [SerializeField] Transform _ChallengelistRoot;

    private void Start()
    {
        SetupChallengeViews();
        NetworkController.Instance.OnGameState += UpdateChallenges;
        _actualPoints = 0;
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
            case ChallengeType.Naturalize:
                return GetNaturalizedBuildings();
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

    private void UpdateChallenges(GameStateNetwork state)
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
        BuildingNetworkSync pBuilding;
        List<BuildingNetworkSync> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            demolishedCount += (pBuilding.DamageTaken > pBuilding.MaxDamage)? 1: 0;
        }

        return demolishedCount;
    }

    private static int GetDamageDealt()
    {
        int demolishedCount = 0;
        BuildingNetworkSync pBuilding;
        List<BuildingNetworkSync> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            demolishedCount += pBuilding.DamageTaken;
        }

        return demolishedCount;
    }

    private static int GetNaturalizedBuildings()
    {
        int naturalizedBuildings = 0;
        BuildingNetworkSync pBuilding;
        List<BuildingNetworkSync> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            naturalizedBuildings += (pBuilding.PlantCount > pBuilding.MaxPlant) ? 1 : 0;
        }

        return naturalizedBuildings;
    }

    private static int GetPlantCount()
    {
        int plantCount = 0;
        BuildingNetworkSync pBuilding;
        List<BuildingNetworkSync> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            plantCount += pBuilding.PlantCount;
        }

        return plantCount;
    }

    private static int GetGraffitiCount()
    {
        int graffitiCount = 0;
        BuildingNetworkSync pBuilding;
        List<BuildingNetworkSync> lBuildings = GameController.Instance.Buildings;
        for (int i = 0; i < lBuildings.Count; i++)
        {
            pBuilding = lBuildings[i];
            graffitiCount += pBuilding.GraffitiCount;
        }

        return graffitiCount;
    }
}
