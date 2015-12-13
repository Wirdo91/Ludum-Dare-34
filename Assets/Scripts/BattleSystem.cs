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
    HomeBase _team1Base, _team2Base;

    [SerializeField]
    GameObject _snowManPrefab;

    void Start()
    {
        _team1 = new List<SnowMan>();
        _team2 = new List<SnowMan>();

        HomeBase[] bases = GameObject.FindObjectsOfType<HomeBase>();
        if (bases.Length > 2)
        {
            Debug.LogWarning("The number of existing bases (" + bases.Length + "), is to great, only 2 will be used");
        }
        else if (bases.Length < 2)
        {
            Debug.LogWarning("The number of existing bases (" + bases.Length + "), is to few. Battle will be deactivated");
            this.enabled = false;
            return;
        }
        foreach (HomeBase homeBase in bases)
        {
            if (_team1Base == null)
            {
                if (homeBase.BaseTeam == Teams.TEAM1)
                {
                    _team1Base = homeBase;
                    continue;
                }
            }
            if (_team1Base == null)
            {
                if (homeBase.BaseTeam == Teams.TEAM2)
                {
                    _team2Base = homeBase;
                    continue;
                }
            }
        }
        SpawnSnowMan(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1), Vector3.zero);
        SpawnSnowMan(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1), Vector3.forward * 4);
        SpawnSnowMan(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1), Vector3.forward * 8);
        SpawnSnowMan(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1), Vector3.forward * 12);
        SpawnSnowMan(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1), Vector3.back * 4);
        SpawnSnowMan(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1), Vector3.back * 8);
        SpawnSnowMan(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1), Vector3.back * 12);
        SpawnSnowMan(new NonActiveSnowMan(1, 2, 3, Teams.TEAM2), Vector3.left * 10);
    }

    public void InitializeBattle(List<NonActiveSnowMan> army1, List<NonActiveSnowMan> army2)
    {
        SpawnArmies(army1, army2);
    }

    public void SpawnSnowMan(NonActiveSnowMan snowMan, Vector3 startPosition)
    {
        GameObject go = Instantiate(_snowManPrefab);
        go.transform.position = startPosition;
        go.GetComponent<SnowMan>().SetValues(snowMan);
        go.GetComponent<SnowMan>().Create();

        switch (snowMan.CurrentTeam)
        {
            case Teams.TEAM1:
                _team1.Add(go.GetComponent<SnowMan>());
                break;
            case Teams.TEAM2:
                _team2.Add(go.GetComponent<SnowMan>());
                break;
            default:
                Debug.LogWarning("The requested team does not exist");
                break;
        }
    }

    void Update()
    {
        for (int i = _team1.Count - 1; i >= 0; i--)
        {
            if (_team1[i] == null)
            {
                _team1.RemoveAt(i);
            }
        }
        for (int i = _team2.Count - 1; i >= 0; i--)
        {
            if (_team2[i] == null)
            {
                _team2.RemoveAt(i);
            }
        }

        if (_team1Base == null || _team2Base == null)
        {
            Debug.Log((_team1Base ?? _team2Base).BaseTeam + " Won !!!");
        }
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

    public Transform GetNearestTarget(SnowMan requesting)
    {
        SnowMan nearestSnowMan = GetNearestSnowManOfOppesiteTeam(requesting);
        if (nearestSnowMan == null)
        {
            return GetEnemyBase(requesting).transform;
        }

        if (Vector3.Distance(requesting.transform.position, nearestSnowMan.transform.position) < 
            Vector3.Distance(requesting.transform.position, GetEnemyBase(requesting).transform.position))
        {
            return nearestSnowMan.transform;
        }
        else
        {
            return GetEnemyBase(requesting).transform;
        }
    }

    HomeBase GetEnemyBase(SnowMan requesting)
    {
        switch (requesting.CurrentTeam)
        {
            case Teams.TEAM1:
                return _team2Base;
            case Teams.TEAM2:
                return _team1Base;
            default:
                Debug.LogWarning("The requested team does not exist");
                return null;
        }
    }

    SnowMan GetNearestSnowManOfOppesiteTeam(SnowMan requesting)
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
            //Debug.LogWarning("The list is empty, returning null");
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
