using System.Collections.Generic;
using UnityEngine;

public class ChallengeController : MonoBehaviour
{
    public List<Challenge> _actualChallenges;

    [SerializeField] GameObject _challengeViewPrefab;
    [SerializeField] Transform _ChallengelistRoot;

    private void Start()
    {
        PopulateActualChallenges();
        SetupChallengeViews();
    }

    private void PopulateActualChallenges()
    {
        // TODO: Get from scriptable object the quests
    }

    private void SetupChallengeViews()
    {
        for(int i = 0; i < _actualChallenges.Count; i++)
        {
            var challengeView = Instantiate(_challengeViewPrefab, _ChallengelistRoot);
            challengeView.GetComponent<ChallengeView>().SetupView(_actualChallenges[i]);
        }
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
            case ComparisonType.Greater:
                return progression > challenge.Value;
            case ComparisonType.Lesser:
                return progression < challenge.Value;
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
            default:
                return -1;
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
            demolishedCount += (pBuilding.DamageTaken() > 10)? 1: 0;
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
            demolishedCount += pBuilding.DamageTaken();
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
            naturalizedBuildings += (pBuilding.PlantCount() > 10) ? 1 : 0;
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
            plantCount += pBuilding.PlantCount();
        }

        return plantCount;
    }
}
