using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Weather : MonoBehaviour
{
    private ParticleSystem rain;
    private ParticleSystem snow;
    private ParticleSystem fog;
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
    private bool isRaining;
    private bool isSnowing;
    private bool isFogging;
    private float transitionSpeed = 10f;
    private float rainRate;
    private float fogRate;
    private float snowRate;
    
    // Start is called before the first frame update
    void Start()
    {
        rain = GameObject.Find("RainGenerator").GetComponent<ParticleSystem>();
        rainEmission = rain.emission;
        
        snow = GameObject.Find("SnowGenerator").GetComponent<ParticleSystem>();
        snowEmission = snow.emission;
        
        fog = GameObject.Find("FogGenerator").GetComponent<ParticleSystem>();
        fogEmission = fog.emission;
        
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
            rainRate = Mathf.MoveTowards(rainRate, 200f, t * transitionSpeed);
        }
        else
        {
            var t = Time.time - startTime;
            rainRate = Mathf.MoveTowards(rainRate, 0, t * (transitionSpeed / 4));
        }
        
        // handle beginning snow
        if (isSnowing)
        {
            var t = Time.time - startTime;
            snowRate = Mathf.MoveTowards(snowRate, 250f, t * transitionSpeed);
        }
        else
        {
            var t = Time.time - startTime;
            snowRate = Mathf.MoveTowards(snowRate, 0, t * (transitionSpeed / 4));
        }
        
        // handle beginning fog
        if (isFogging)
        {
            var t = Time.time - startTime;
            fogRate = Mathf.MoveTowards(fogRate, 10f, t * transitionSpeed);
        }
        else
        {
            var t = Time.time - startTime;
            fogRate = Mathf.MoveTowards(fogRate, 0, t * (transitionSpeed / 4));
        }
        
        // maintain emissions
        rainEmission.rateOverTime = rainRate;
        snowEmission.rateOverTime = snowRate;
        fogEmission.rateOverTime = fogRate;
    }
    
    
    public void setRain(bool setting, bool darkenScene)
    {
        startTime = Time.time;
        rain.gameObject.SetActive(true);
        isRaining = setting;
        darkened = darkenScene;
    }
    
    public void setSnow(bool setting)
    {
        startTime = Time.time;
        snow.gameObject.SetActive(true);
        isSnowing = setting;
    }

    public void setFog(bool setting)
    {
        startTime = Time.time;
        fog.gameObject.SetActive(true);
        isFogging = setting;
    }
}
