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

    bool _gameOver = false;
    public bool GameOver { get { return _gameOver; } }

    public delegate void WinCondition(Teams winningTeam);
    public event WinCondition OnWinCondition;

    [SerializeField]
    Transform _base1Position;
    [SerializeField]
    Transform _base2Position;

    [SerializeField]
    Transform _player1Anchor;
    [SerializeField]
    Transform _player2Anchor;

    [SerializeField]
    GameObject _basePrefab;

    [SerializeField]
    PlayerMovement _player1Object, _player2Object;

    public void DeactivatePlayers()
    {
        _player1Object.gameObject.SetActive(false);
        _player2Object.gameObject.SetActive(false);
    }

    public void ResetBattle()
    {
        StartCoroutine(ResetBattleSystem());
    }

    private IEnumerator ResetBattleSystem()
    {
        if (_team1 != null)
        {
            //Remove Armies
            foreach (SnowMan sm in _team1)
            {
                if (sm != null)
                {
                    sm.QuickDestroy();
                }
            }
        }
        if (_team2 != null)
        {
            foreach (SnowMan sm in _team2)
            {
                if (sm != null)
                {
                    sm.QuickDestroy();
                }
            }
        }

        yield return null;

        //Create Bases
        if (_team1Base != null)
            Destroy(_team1Base.gameObject);


        if (_team2Base != null)
            Destroy(_team2Base.gameObject);

        yield return null;

        SetBases();

        yield return null;

        //TODO Start Game
        _player1Object.gameObject.SetActive(true);
        _player2Object.gameObject.SetActive(true);

        //Check Players
        _player1Object.transform.position = _player1Anchor.transform.position;
        _player1Object.transform.rotation = _player1Anchor.transform.rotation;
        _player1Object.transform.localScale = _player1Anchor.transform.localScale;
        _player1Object.Reset();

        _player2Object.transform.position = _player2Anchor.transform.position;
        _player2Object.transform.rotation = _player2Anchor.transform.rotation;
        _player2Object.transform.localScale = _player2Anchor.transform.localScale;
        _player2Object.Reset();

        yield return null;

        SetupBattle();

        yield return null;

        GlobalVariables.instance.StartGame();
    }

    void SetBases()
    {
        if (_team1Base == null)
        {
            GameObject newBase = Instantiate(_basePrefab);
            newBase.transform.position = _base1Position.position;
            newBase.transform.rotation = _base1Position.rotation;
            newBase.transform.localScale = _base1Position.localScale;
            _team1Base = newBase.GetComponent<HomeBase>();
        }
        _team1Base.SetBaseTeam(Teams.TEAM1);

        if (_team2Base == null)
        {
            GameObject newBase = Instantiate(_basePrefab);
            newBase.transform.position = _base2Position.position;
            newBase.transform.rotation = _base2Position.rotation;
            newBase.transform.localScale = _base2Position.localScale;
            _team2Base = newBase.GetComponent<HomeBase>();
        }
        _team2Base.SetBaseTeam(Teams.TEAM2);
    }

    void Start()
    {
        SetBases();
        SetupBattle();
    }

    void SetupBattle()
    {
        HomeBase[] bases = GameObject.FindObjectsOfType<HomeBase>();

        if (GlobalVariables.instance.GameRunning)
        {
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

        _gameOver = false;
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
        if (_team1 != null)
        {
            for (int i = _team1.Count - 1; i >= 0; i--)
            {
                if (_team1[i] == null)
                {
                    _team1.RemoveAt(i);
                }
            }
        }
        else
        {
            _team1 = new List<SnowMan>();
        }
        if (_team2 != null)
        {
            for (int i = _team2.Count - 1; i >= 0; i--)
            {
                if (_team2[i] == null)
                {
                    _team2.RemoveAt(i);
                }
            }
        }
        else
        {
            _team1 = new List<SnowMan>();
        }

        if (GlobalVariables.instance.GameRunning)
        {
            if (_team1Base == null || _team2Base == null)
            {
                Debug.Log((_team1Base ?? _team2Base).BaseTeam + " won the game!");
                _gameOver = true;
                OnWinCondition((_team1Base ?? _team2Base).BaseTeam);
            }
        }
    }

    void SpawnArmies(List<NonActiveSnowMan> army1, List<NonActiveSnowMan> army2)
    {
        if (_team1 == null)
            _team1 = new List<SnowMan>();
        if (_team2 == null)
            _team2 = new List<SnowMan>();

        if (army1.Count % 2 == 0)
        {
            for (int i = 0; i < army1.Count / 2; i++)
            {
                SpawnSnowMan(army1[i], _player1Anchor.position - Vector3.left * ((4 * i) + 1));
                SpawnSnowMan(army1[army1.Count - i], _player1Anchor.position + Vector3.left * ((4 * i) + 1));
            }
        }
        else
        {
            int middleIndex = (army1.Count - (((army1.Count - 1) / 2) + 1));
            SpawnSnowMan(army1[middleIndex], _player1Anchor.position);
            for (int i = 0; i < (army1.Count - 1) / 2; i++)
            {
                SpawnSnowMan(army1[middleIndex + i], _player1Anchor.position - Vector3.left * (4 * (i + 1)));
                SpawnSnowMan(army1[middleIndex - i], _player1Anchor.position + Vector3.left * (4 * (i + 1)));
            }
        }
        if (army2.Count % 2 == 0)
        {
            for (int i = 0; i < army1.Count / 2; i++)
            {
                SpawnSnowMan(army2[i], _player2Anchor.position - Vector3.left * ((4 * i) + 1));
                SpawnSnowMan(army2[army2.Count - i], _player2Anchor.position + Vector3.left * ((4 * i) + 1));
            }
        }
        else
        {
            int middleIndex = (army2.Count - (((army2.Count - 1) / 2) + 1));
            SpawnSnowMan(army2[middleIndex], _player2Anchor.position);
            for (int i = 0; i < (army1.Count - 1) / 2; i++)
            {
                SpawnSnowMan(army2[middleIndex + i], _player2Anchor.position - Vector3.left * (4 * (i + 1)));
                SpawnSnowMan(army2[middleIndex - i], _player2Anchor.position + Vector3.left * (4 * (i + 1)));
            }
        }
    }

    public Transform GetNearestTarget(SnowMan requesting)
    {
        if (GameOver)
        {
            return null;
        }

        SnowMan nearestSnowMan = GetNearestSnowManOfOppesiteTeam(requesting);
        if (nearestSnowMan == null)
        {
            HomeBase nearestBase = GetEnemyBase(requesting);
            if (nearestBase != null)
            {
                return nearestBase.transform;
            }
            else
            {
                return null;
            }
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
