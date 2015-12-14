using UnityEngine;
using System.Collections;

public class SnowHandler : MonoBehaviour
{
    ParticleSystem _snowSystem;
    ParticleSystem _snowSystem2;
    ParticleSystem _snowFogSystem;
    ParticleSystem _snowGroundSystem;

    // Use this for initialization
    void Start ()
    {
        _snowSystem = this.GetComponent<ParticleSystem>();
        _snowSystem2 = this.GetComponentsInChildren<ParticleSystem>()[1];
        _snowGroundSystem = this.GetComponentsInChildren<ParticleSystem>()[2];
        _snowFogSystem = this.GetComponentsInChildren<ParticleSystem>()[3];
    }



    public void HeavySnow()
    {
        _snowSystem.playbackSpeed = 2;
        _snowSystem2.playbackSpeed = 2;
        _snowSystem.emissionRate = 5000;
        _snowSystem2.emissionRate = 1000;
    }

    public void MediumSnow()
    {
        _snowSystem.playbackSpeed = 1.5f;
        _snowSystem2.playbackSpeed = 1.5f;
        _snowSystem.emissionRate = 2500;
        _snowSystem2.emissionRate = 500;
    }

    public void MildSnow()
    {
        _snowSystem.playbackSpeed = 1;
        _snowSystem2.playbackSpeed = 1;
        _snowSystem.emissionRate = 1000;
        _snowSystem2.emissionRate = 250;
    }

    public void NoSnow()
    {
        _snowSystem.emissionRate = 0;
        _snowSystem2.emissionRate = 0;
    }

    public void HeavySnowFog()
    {
        _snowFogSystem.playbackSpeed = 2;
        _snowFogSystem.emissionRate = 300;
    }

    public void MediumSnowFog()
    {
        _snowFogSystem.playbackSpeed = 1.5f;
        _snowFogSystem.emissionRate = 150;
    }

    public void MildSnowFog()
    {
        _snowFogSystem.playbackSpeed = 1;
        _snowFogSystem.emissionRate = 50;
    }

    public void NoSnowFog()
    {
        _snowFogSystem.playbackSpeed = 1;
        _snowFogSystem.emissionRate = 0;
    }

    public void HeavyGroundFog()
    {
        _snowGroundSystem.emissionRate = 100;
    }

    public void GroundFog()
    {
        _snowGroundSystem.emissionRate = 50;
    }

    public void NoGroundFog()
    {
        _snowGroundSystem.emissionRate = 0;
    }
}
