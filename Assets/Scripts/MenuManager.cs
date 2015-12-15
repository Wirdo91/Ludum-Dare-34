using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    TerrainHandler terrain;
    [SerializeField]
    BattleSystem battle;
    [SerializeField]
    Camera _menuCamera;
    [SerializeField]
    GameObject[] _deactivateOnMenu;
    [SerializeField]
    UnityEngine.UI.Text _hintText;

    void Start()
    {
        _hintText.text = GlobalVariables.instance.GetRandomHint();

        SpawnShowArmy();

        battle.OnWinCondition += OnGameOver;

        if (_deactivateOnMenu != null)
        {
            foreach (GameObject go in _deactivateOnMenu)
            {
                go.SetActive(false);
            }
        }

        battle.DeactivatePlayers();
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnGameOver(Teams team)
    {
        /*
        Application.LoadLevel(Application.loadedLevel);
        this.gameObject.SetActive(true);

        if (_deactivateOnMenu != null)
        {
            foreach (GameObject go in _deactivateOnMenu)
            {
                go.SetActive(false);
            }
        }*/
    }

    public void NewGame()
    {
        terrain.ResetTerrain();
        battle.ResetBattle();

        if (_deactivateOnMenu != null)
        {
            foreach (GameObject go in _deactivateOnMenu)
            {
                go.SetActive(true);
            }
        }

        _menuCamera.enabled = false;
        this.gameObject.SetActive(false);
    }

    void SpawnShowArmy()
    {
        List<NonActiveSnowMan> army1 = new List<NonActiveSnowMan>();
        army1.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1));
        army1.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1));
        army1.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1));
        army1.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1));
        army1.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM1));

        List<NonActiveSnowMan> army2 = new List<NonActiveSnowMan>();
        army2.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM2));
        army2.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM2));
        army2.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM2));
        army2.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM2));
        army2.Add(new NonActiveSnowMan(3, 2, 1, Teams.TEAM2));


        battle.InitializeBattle(army1, army2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
