using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] GameObject _baseFloor;
    [SerializeField] GameObject _topFloor;
    [SerializeField] List<GameObject> _variableFloors;

    [SerializeField] Transform _spawnRoot;

    private BuildingFloor _lastFloorCreated;

    private List<BuildingFloor> _floors;

    private BuildingNetwork _buildingNetworkReference;

    private int _numInterationsDone;

    public void Setup(BuildingNetwork pBuildingData)
    {
        _buildingNetworkReference = pBuildingData;
        _numInterationsDone = 0;
        int amount = _buildingNetworkReference.height - 1;

        _lastFloorCreated = Instantiate(_baseFloor, _spawnRoot).GetComponent<BuildingFloor>();
        _floors.Add(_lastFloorCreated);
        while (amount > 0)
        {
            amount--;

            int index = Random.Range(0, _variableFloors.Count);

            var prefab = _variableFloors[index];

            var spawned = Instantiate(prefab, _spawnRoot);
            spawned.transform.position = new Vector3(spawned.transform.position.x, _lastFloorCreated.transform.position.y + (_lastFloorCreated.Collider.size.y*_lastFloorCreated.transform.localScale.y), spawned.transform.position.z);
            _lastFloorCreated = spawned.GetComponent<BuildingFloor>();
            _floors.Add(_lastFloorCreated);
        }

        var topFloor = Instantiate(_topFloor, _spawnRoot);
        topFloor.transform.position = new Vector3(topFloor.transform.position.x, _lastFloorCreated.transform.position.y + (_lastFloorCreated.Collider.size.y * _lastFloorCreated.transform.localScale.y), topFloor.transform.position.z);

    }


    public void DealDamageFloor()
    {
        if (_numInterationsDone > _buildingNetworkReference.height)
        {
            DemolishBuilding();
        }
        else
        {
            BuildingFloor floor = GetRandomAvaibleFloor();
            floor.DamageFloor();
        }
    }

    public void NaturalizeFloor()
    {
        if (_numInterationsDone > _buildingNetworkReference.height)
        {
            DemolishBuilding();
        }
        else
        {
            BuildingFloor floor = GetRandomAvaibleFloor();
            floor.Naturalize();
        }
    }

    public void DemolishBuilding()
    {
        Debug.Log("Puft");
    }

    private BuildingFloor GetRandomAvaibleFloor()
    {
        List<BuildingFloor> availablesFloors = new List<BuildingFloor>();
        for(int i = 0; i < _floors.Count; i++)
        {
            if (_floors[i].Interactable)
                availablesFloors.Add(_floors[i]);
        }

        int index = Random.Range(0, availablesFloors.Count);

        return availablesFloors[index];


    }

    private void Start()
    {
        //Setup(new BuildingNetwork());
    }

}
