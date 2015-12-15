using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainHandler : MonoBehaviour
{
    [SerializeField]
    Terrain terrain;
    int resolution = 512;

    int pickupsize;

    [SerializeField]
    SnowHandler _snowhandler;

    [SerializeField]
    GameObject[] _trees;

    [SerializeField]
    int _targetTrees;

    [SerializeField]
    AudioClip _mildwind;

    [SerializeField]
    AudioClip _heavywind;

    AudioSource windsource;

    [SerializeField]
    WindZone wind;

    float _weatherchangetimer = 30;

    // Use this for initialization
    void Start ()
    {
        terrain = this.GetComponent<Terrain>();
        resolution = terrain.terrainData.alphamapResolution;
        pickupsize = (int)(terrain.terrainData.alphamapResolution / terrain.terrainData.size.x);
        StartCoroutine(LetitSnow());
        windsource = this.gameObject.AddComponent<AudioSource>();
        windsource.loop = true;
        windsource.playOnAwake = false;
        windsource.Pause();
        windsource.volume = 0.5f;

        TreePrototype[] temptp = new TreePrototype[_trees.Length];

        for (int i = 0; i < _trees.Length; i++)
        {
            TreePrototype tp = new TreePrototype();
            tp.prefab = _trees[i];
            temptp[i] = tp;
        }

        terrain.terrainData.treePrototypes = temptp;
    }

    void Update()
    {
        _weatherchangetimer += Time.deltaTime;

        if (_weatherchangetimer > 30)
        {
            _weatherchangetimer = 0;

            int i = Random.Range(0, 10);
            Debug.Log(i);
            switch (i)
            {
                case 0:
                    NoSnow();
                    break;
                case 1:
                    MildSnow();
                    break;
                case 2:
                    MediumSnow();
                    break;
                case 3:
                    HeavySnow();
                    break;
                case 4:
                    MildStorm();
                    break;
                case 5:
                    MediumStorm();
                    break;
                case 6:
                    HeavyStorm();
                    break;
                case 7:
                    MildSnowStorm();
                    break;
                case 8:
                    MediumSnowStorm();
                    break;
                case 9:
                    HeavySnowStorm();
                    break;
            }
        }
    }

    //TODO: Add RenderSettings Fog and snow drop rate.
    public void NoSnow()
    {
        wind.windMain = 0.1f;
        wind.windTurbulence = 0.1f;

        GlobalVariables.instance.SnowRate = 0;
        _snowhandler.NoSnow();
        _snowhandler.NoSnowFog();
        _snowhandler.NoGroundFog();
        windsource.Pause();
    }

    public void MildSnow()
    {
        wind.windMain = 0.2f;
        wind.windTurbulence = 0.2f;
        wind.windPulseFrequency = 0.5f;
        wind.windPulseMagnitude = 0.01f;
        GlobalVariables.instance.SnowRate = 0.5f;
        _snowhandler.MildSnow();
        _snowhandler.NoSnowFog();
        _snowhandler.NoGroundFog();
        windsource.Pause();
    }

    public void MediumSnow()
    {
        wind.windMain = 0.35f;
        wind.windTurbulence = 0.35f;
        wind.windPulseFrequency = 0.5f;
        wind.windPulseMagnitude = 0.01f;
        GlobalVariables.instance.SnowRate = 1;
        _snowhandler.MediumSnow();
        _snowhandler.NoSnowFog();
        _snowhandler.NoGroundFog();
        windsource.Pause();
    }

    public void HeavySnow()
    {
        wind.windMain = 0.5f;
        wind.windTurbulence = 0.5f;
        wind.windPulseFrequency = 0.5f;
        wind.windPulseMagnitude = 0.01f;
        GlobalVariables.instance.SnowRate = 2;
        _snowhandler.HeavySnow();
        _snowhandler.NoSnowFog();
        _snowhandler.NoGroundFog();
        windsource.Pause();
    }

    public void MildStorm()
    {
        wind.windMain = 1f;
        wind.windTurbulence = 1f;
        wind.windPulseFrequency = 1;
        wind.windPulseMagnitude = 0.125f;
        GlobalVariables.instance.SnowRate = 0.1f;
        _snowhandler.NoSnow();
        _snowhandler.MildSnowFog();
        _snowhandler.NoGroundFog();
        windsource.clip = _mildwind;
        windsource.Play();
    }

    public void MediumStorm()
    {
        wind.windMain = 1.5f;
        wind.windTurbulence = 1.5f;
        wind.windPulseFrequency = 1.5f;
        wind.windPulseMagnitude = 0.25f;
        GlobalVariables.instance.SnowRate = 0.25f;
        _snowhandler.NoSnow();
        _snowhandler.MediumSnowFog();
        _snowhandler.GroundFog();
        windsource.clip = _mildwind;
        windsource.Play();
    }

    public void HeavyStorm()
    {
        wind.windMain = 2f;
        wind.windTurbulence = 2f;
        wind.windPulseFrequency = 2f;
        wind.windPulseMagnitude = 0.5f;
        GlobalVariables.instance.SnowRate = 0.5f;
        _snowhandler.NoSnow();
        _snowhandler.HeavySnowFog();
        _snowhandler.HeavyGroundFog();
        windsource.clip = _heavywind;
        windsource.Play();
    }

    public void MildSnowStorm()
    {
        wind.windMain = 1f;
        wind.windTurbulence = 1f;
        wind.windPulseFrequency = 1;
        wind.windPulseMagnitude = 0.125f;
        GlobalVariables.instance.SnowRate = 1f;
        _snowhandler.MildSnow();
        _snowhandler.MildSnowFog();
        _snowhandler.NoGroundFog();
        windsource.clip = _mildwind;
        windsource.Play();
    }

    public void MediumSnowStorm()
    {
        wind.windMain = 1.5f;
        wind.windTurbulence = 1.5f;
        wind.windPulseFrequency = 1.5f;
        wind.windPulseMagnitude = 0.25f;
        GlobalVariables.instance.SnowRate = 2f;
        _snowhandler.MediumSnow();
        _snowhandler.MediumSnowFog();
        _snowhandler.GroundFog();
        windsource.clip = _heavywind;
        windsource.Play();
    }

    public void HeavySnowStorm()
    {
        wind.windMain = 2f;
        wind.windTurbulence = 2f;
        wind.windPulseFrequency = 2f;
        wind.windPulseMagnitude = 0.5f;
        GlobalVariables.instance.SnowRate = 3f;
        _snowhandler.HeavySnow();
        _snowhandler.HeavySnowFog();
        _snowhandler.HeavyGroundFog();
        windsource.clip = _heavywind;
        windsource.Play();
    }

    public void ResetTerrain()
    {
        _targetTrees = Random.Range(10, 21);
        terrain.terrainData.treeInstances = new TreeInstance[0];
        for (int i = 0; i < _targetTrees; i++)
        {
            TreeInstance ti = new TreeInstance();
            Vector3 pos = new Vector3(Random.Range(1, 126), 0, Random.Range(1, 126));

            Rect rl = new Rect(30, 5, 50, 30);
            Rect rr = new Rect(50, 95, 50, 30);

            if (rl.Overlaps(new Rect(pos.x * terrain.terrainData.size.x, pos.z * terrain.terrainData.size.z, 1, 1)) || rr.Overlaps(new Rect(pos.x * terrain.terrainData.size.x, pos.z * terrain.terrainData.size.z, 1, 1)))
                continue;

            ti.position = (pos / 128.0f);
            ti.prototypeIndex = Random.Range(0, 14);
            ti.heightScale = 1;
            ti.widthScale = 1;
            ti.color = Color.white;
            ti.lightmapColor = Color.white;
            terrain.AddTreeInstance(ti);
            terrain.Flush();
        }
        float[,,] alphas = terrain.terrainData.GetAlphamaps(0, 0, resolution, resolution);
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                alphas[i, j, 0] = 1;
                alphas[i, j, 1] = 0;
            }
        }
        terrain.terrainData.SetAlphamaps(0, 0, alphas);
    }

    public float GetSnow(Transform player)
    {
        if (player.position.x < 0 || player.position.x > terrain.terrainData.size.x || player.position.z < 0 || player.position.z > terrain.terrainData.size.z)
        {
            return 0.0f;
        }
        float snowpickup = 0;


        int size = (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize) * 4 % 2 == 0) ?
            (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize) * 4) + 1 :
            (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize) * 4);

        float[,,] alphas = terrain.terrainData.GetAlphamaps(Mathf.RoundToInt((player.position.x * pickupsize) + -(size / 2)), Mathf.RoundToInt((player.position.z * pickupsize) + -(size / 2)), size, size);

        float mid = Mathf.Ceil((float)size / 2.0f);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float percentage = Mathf.Abs(1.0f - Vector2.Distance(new Vector2(i, j), new Vector2(mid, mid)) / mid) / 8.0f;
                snowpickup += alphas[i, j, 0] * percentage;

                alphas[i, j, 0] -= alphas[i, j, 0] * percentage;
                alphas[i, j, 1] = 1 - alphas[i, j, 0];

                if (alphas[i, j, 0] < 0.0f)
                {
                    alphas[i, j, 0] = 0;
                }
                if (alphas[i, j, 1] > 1.0f)
                {
                    alphas[i, j, 1] = 1;
                }
            }
        }
        terrain.terrainData.SetAlphamaps(Mathf.RoundToInt((player.position.x * pickupsize) + -(size / 2)), Mathf.RoundToInt((player.position.z * pickupsize) + -(size / 2)), alphas);

        return snowpickup;
    }

    IEnumerator LetitSnow()
    {
        float[,,] alphas;

        int updatesize = 64;
        int updatemul = 0;
        int updatemulmod = resolution / updatesize;

        int updatemulmax = updatemulmod * updatemulmod;

        while (true)
        {
            alphas = terrain.terrainData.GetAlphamaps(Mathf.FloorToInt(updatemul / updatemulmod) * updatesize, (updatemul % updatemulmod) * updatesize, updatesize, updatesize);
            for (int i = 0; i < updatesize; i++)
            {
                for (int j = 0; j < updatesize; j++)
                {
                    alphas[i, j, 0] += GlobalVariables.instance.SnowRate * Time.deltaTime;
                    alphas[i, j, 1] -= GlobalVariables.instance.SnowRate * Time.deltaTime;
                    if (alphas[i, j, 0] > 1)
                    {
                        alphas[i, j, 0] = 1;
                    }
                    if (alphas[i, j, 1] < 0)
                    {
                        alphas[i, j, 1] = 0;
                    }
                }
            }

            terrain.terrainData.SetAlphamaps(Mathf.FloorToInt(updatemul / updatemulmod) * updatesize, (updatemul % updatemulmod) * updatesize, alphas);
            updatemul++;
            if (updatemul == updatemulmax)
            {
                updatemul = 0;
            }
            yield return null;
        }

    }
}