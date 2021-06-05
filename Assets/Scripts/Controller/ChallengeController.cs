using System.Collections.Generic;
using UnityEngine;

public class ChallengeController : MonoBehaviour
{
    private List<Challenge> _actualChallenges;



    private bool CheckAmount(Challenge challenge)
    {
        int progression = GetChallengeProgression((ChallengeType)challenge.ChallengeType);
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
                // Update to get from the GameController the amount of demolished buildings.
                return 1;
            case ChallengeType.Naturalize:
                // Update to get from the GameController the amount of naturalized buildings.
                return 0;
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

}
