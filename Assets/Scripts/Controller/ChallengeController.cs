using System.Collections.Generic;
using UnityEngine;

public class ChallengeController : MonoBehaviour
{
    private List<Challenge> _actualChallenges;



    private bool CheckAmount(Challenge challenge)
    {
        int progression = GetChallengeProgression((ChallengeType)challenge.ChallengeType);


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
            case ComparisonType.Range:
                return progression > challenge.Value && progression < challenge.MaxValue;
        }
        return false;
    }

    private int GetChallengeProgression(ChallengeType _type)
    {
        switch (_type)
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


    private int GetDemolishedBuildings()
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

    private int GetDamageDealt()
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

    private int GetNaturalizedBuildings()
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

    private int GetPlantCount()
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
