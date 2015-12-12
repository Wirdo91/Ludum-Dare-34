using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Teams
{
    TEAM1,
    TEAM2
}

public class BattleSystem : MonoBehaviour {

    List<SnowMan> _team1, _team2;

    [SerializeField]
    GameObject _snowManPrefab;

    public void InitializeBattle(List<NonActiveSnowMan> army1, List<NonActiveSnowMan> army2)
    {
        _team1 = new List<SnowMan>();
        _team2 = new List<SnowMan>();

        SpawnArmies(army1, army2);
    }

    void SpawnArmies(List<NonActiveSnowMan> army1, List<NonActiveSnowMan> army2)
    {
        foreach (NonActiveSnowMan unit in army1)
        {
            GameObject go = Instantiate(_snowManPrefab);
            go.transform.position = Vector3.zero;
            go.GetComponent<SnowMan>().SetValues(unit);
            go.GetComponent<SnowMan>().Create();

            _team1.Add(go.GetComponent<SnowMan>());
        }
        foreach (NonActiveSnowMan unit in army2)
        {
            GameObject go = Instantiate(_snowManPrefab);
            go.transform.position = Vector3.forward * 10;
            go.GetComponent<SnowMan>().SetValues(unit);
            go.GetComponent<SnowMan>().Create();

            _team2.Add(go.GetComponent<SnowMan>());
        }
    }

    public SnowMan GetNearestSnowManOfOppesiteTeam(SnowMan requesting)
    {
        switch(requesting.CurrentTeam)
        {
            case Teams.TEAM1:
                return GetNearestSnowMan(_team2, requesting);
            case Teams.TEAM2:
                return GetNearestSnowMan(_team1, requesting);
            default:
                Debug.LogWarning("The requested team does not exist");
                return null;
        }
    }

    SnowMan GetNearestSnowMan(List<SnowMan> snowMen, SnowMan requesting)
    {
        if (snowMen.Count <= 0)
        {
            Debug.LogWarning("The list is empty, returning null");
            return null;
        }

        int bestIndex = 0;
        float bestDistance = float.PositiveInfinity;

        for (int i = 0; i < snowMen.Count; i++)
        {
            if (Vector3.Distance(snowMen[i].transform.position, requesting.transform.position) < bestDistance)
            {
                bestIndex = i;
            }
        }

        return snowMen[bestIndex];
    }
}
