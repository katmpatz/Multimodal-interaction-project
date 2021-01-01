﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using SimpleJSON;

public class WeatherManager : MonoBehaviour
{
    public string apiKey = "3002bc312f0944b99d15b79863b810fb";
    public string currentWeatherApi = "api.openweathermap.org/data/2.5/onecall?";


    [Header("UI")]
    public TextMeshPro temperature;
    public TextMeshPro dayFull;
    public TextMeshPro description;
    public TextMeshProUGUI statusText;
    private LocationInfo lastLocation;
    public GameObject ARObject;
    public GameObject weatherPanel;
    //public GameObject targetMain;
    //public GameObject target1;
    //public GameObject target2;
    Vector3 positionWindow;

    private CalPosition cp;

    void Start()
    {
        print("Start");
        weatherPanel.gameObject.SetActive(true);
        //positionWindow = new Vector3(0.0f, 1.0f, 0.0f);
        // UpdateWeatherData();
        // cp = gameObject.GetComponent<CalPosition>();
        
    }

    void Update()
    {
        //positionWindow.Set((targetMain.transform.position.x + target1.transform.position.x) / 2, (targetMain.transform.position.y + target2.transform.position.y) / 2, (targetMain.transform.position.z + target2.transform.position.z) / 2);
        //weatherPanel.transform.position = positionWindow;
        
    }

        public void UpdateWeatherData()
    {
        StartCoroutine(FetchWeatherDataFromApi(0)); //Stockholm
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
/*
    public void accessWeather(int dateNumber)
    {
        print("intermediate phase");
        StartCoroutine(FetchWeatherDataFromApi(dateNumber));
        print("intermediate phase 2");

    }
*/
    public IEnumerator FetchWeatherDataFromApi(int dateNumber)
    {
        //Stockholm latitude and longitude
        var latitude = "59.334591";
        var longitude = "18.063240";
        string url = currentWeatherApi + "lat=" + latitude + "&lon=" + longitude + "&cnt=7" + "&appid=" + apiKey + "&units=metric";
        UnityWebRequest fetchWeatherRequest = UnityWebRequest.Get(url);
        yield return fetchWeatherRequest.SendWebRequest();

        print("weather 1");
        if (fetchWeatherRequest.isNetworkError || fetchWeatherRequest.isHttpError)
        {
            //Check and print error 
            statusText.text = fetchWeatherRequest.error;
        }
        else
        {

            //weatherPanel.gameObject.SetActive(true);
            
            //ARObject.gameObject.SetActive(false);


            Debug.Log(fetchWeatherRequest.downloadHandler.text);
            var response = JSON.Parse(fetchWeatherRequest.downloadHandler.text);
            print("weather");

            description.text = response["daily"][0]["weather"][0]["description"];

            //convert timestamp to date string
            var dayJson = response["daily"][dateNumber]["dt"];
            var doubleDay = Convert.ToDouble(dayJson);
            var dateT = UnixTimeStampToDateTime(doubleDay);
            dayFull.text = dateT.ToString("ddd, dd MMM yy");

            temperature.text = response["daily"][dateNumber]["temp"]["day"] + " °C";
        }
    }
}