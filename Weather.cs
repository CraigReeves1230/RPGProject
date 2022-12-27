using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Weather : MonoBehaviour
{
    public ParticleSystem rain;
    public ParticleSystem snow;
    public ParticleSystem fog;
    public Tilemap[] tilemaps;
    public bool rainByDefault;
    public bool snowByDefault;
    public bool fogByDefault;
    public bool sceneDarkenedByDefault;
    public float startTime;
    private bool darkened;
    private float darkenSpeed = 0.5f;
    private ParticleSystem.EmissionModule rainEmission;
    private ParticleSystem.EmissionModule snowEmission;
    private ParticleSystem.EmissionModule fogEmission;
    
    // rain variables
    private bool isRaining;
    private float rainRate;
    private float rainTransitionSpeed = 10f;
    private float rainStrength = 550f;
    
    // fog variables
    private bool isFogging;
    private float fogRate;
    private float fogTransitionSpeed = 10f;
    private float fogStrength = 10f;
    
    // snow variables
    private bool isSnowing;
    private float snowRate;
    private float snowTransitionSpeed = 10f;
    private float snowStrength = 300f;
    

    // Start is called before the first frame update
    void Start()
    {
        // position to follow camera
        rain.gameObject.SetActive(rainByDefault);
        snow.gameObject.SetActive(snowByDefault);
        fog.gameObject.SetActive(fogByDefault);
        isRaining = rainByDefault;
        isSnowing = snowByDefault;
        isFogging = fogByDefault;

        // screen darkened by default
        darkened = sceneDarkenedByDefault;

        // if not raining, snowing or fogging by default set emissions to 0
        rainRate = !rainByDefault ? 0f : 200f;
        snowRate = !snowByDefault ? 0f : 250f;
        fogRate = !fogByDefault ? 0f : 10f;

        // set emission rates
        rainEmission = rain.GetComponent<ParticleSystem>().emission;
        snowEmission = snow.GetComponent<ParticleSystem>().emission;
        fogEmission = fog.GetComponent<ParticleSystem>().emission; 

        rainEmission.rateOverTime = rainRate;
        snowEmission.rateOverTime = snowRate;
        fogEmission.rateOverTime = fogRate;
    }

    void FixedUpdate()
    {
        // handle screen darkening
        if (darkened)
        {
            var t = Time.time - startTime;
            foreach (var tilemap in tilemaps)
            {
                tilemap.color = Color.Lerp(tilemap.color, Color.gray, t * darkenSpeed);
            }

        }
        else
        {
            var t = Time.time - startTime;
            foreach (var tilemap in tilemaps)
            {
                tilemap.color = Color.Lerp(tilemap.color, Color.white, t * (darkenSpeed / 2));
            }
        }

        // handle beginning rain
        if (isRaining)
        {
            var t = Time.time - startTime;
            rainRate = Mathf.MoveTowards(rainRate, rainStrength, t * rainTransitionSpeed);
            rain.transform.position = positionSnowOrRain();
        }
        else
        {
            var t = Time.time - startTime;
            rainRate = Mathf.MoveTowards(rainRate, 0, t * (rainTransitionSpeed / 4));
        }

        // handle beginning snow
        if (isSnowing)
        {
            var t = Time.time - startTime;
            snowRate = Mathf.MoveTowards(snowRate, snowStrength, t * snowTransitionSpeed);
            snow.transform.position = positionSnowOrRain();
        }
        else
        {
            var t = Time.time - startTime;
            snowRate = Mathf.MoveTowards(snowRate, 0, t * (snowTransitionSpeed / 4));
        }

        // handle beginning fog
        if (isFogging)
        {
            var t = Time.time - startTime;
            fogRate = Mathf.MoveTowards(fogRate, fogStrength, t * fogTransitionSpeed);
            fog.transform.position = positionFog();
        }
        else
        {
            var t = Time.time - startTime;
            fogRate = Mathf.MoveTowards(fogRate, 0, t * (fogTransitionSpeed / 4));
        }

        // maintain emissions
        rainEmission.rateOverTime = rainRate;
        snowEmission.rateOverTime = snowRate;
        fogEmission.rateOverTime = fogRate;
    }

    private Vector2 positionSnowOrRain()
    {
        var x = Camera.main.transform.position.x;
        var y = Camera.main.transform.position.y + Camera.main.orthographicSize * 2;
        
        return new Vector2(x, y);
    }

    private Vector2 positionFog()
    {
        var x = Camera.main.transform.position.x;
        var y = Camera.main.transform.position.y - Camera.main.orthographicSize * 1.5f;
        
        return new Vector2(x, y);
    }

    public void setRain(bool setting, bool darkenScene, float darknessSpeed, float rainTransSpeed, float rainIntensity)
    {
        startTime = Time.time;
        rain.gameObject.SetActive(true);
        isRaining = setting;
        darkened = darkenScene;
        darkenSpeed = darknessSpeed;
        rainTransitionSpeed = rainTransSpeed;
        rainStrength = rainIntensity;
    }
    
    public void setSnow(bool setting, bool darkenScene, float darknessSpeed, float snowTransSpeed, float snowIntensity)
    {
        startTime = Time.time;
        snow.gameObject.SetActive(true);
        isSnowing = setting;
        darkened = darkenScene;
        darkenSpeed = darknessSpeed;
        snowTransitionSpeed = snowTransSpeed;
        snowStrength = snowIntensity;
    }

    public void setFog(bool setting, bool darkenScene, float darknessSpeed, float fogTransSpeed, float fogIntensity)
    {
        startTime = Time.time;
        fog.gameObject.SetActive(true);
        isFogging = setting;
        darkened = darkenScene;
        darkenSpeed = darknessSpeed;
        fogTransitionSpeed = fogTransSpeed;
        fogStrength = fogIntensity;
    }
}
