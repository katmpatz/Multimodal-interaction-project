using System;
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
    public TextMeshProUGUI temperature;
    public TextMeshProUGUI dayFull;
    public TextMeshProUGUI description;
    public TextMeshProUGUI statusText;
    private LocationInfo lastLocation;


    void Start()
    {
        print("Start");
        UpdateWeatherData();
    }

    private IEnumerator FetchLocationData()
    {
        print("location");
        // First, check if user has location service enabled 
        if (!Input.location.isEnabledByUser) yield break;
        // Start service before querying location 
        Input.location.Start();

        // Wait until service initializes 
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        // Service didn't initialize in 20 seconds 
        if (maxWait < 1)
        {
            statusText.text = "Location Timed out";
            yield break;
        }
        // Connection has failed 
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            statusText.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            lastLocation = Input.location.lastData;
            UpdateWeatherData();
        }
        Input.location.Stop();
    }


    private void UpdateWeatherData()
    {
        StartCoroutine(FetchWeatherDataFromApi("59.334591", "18.063240"));//lastLocation.latitude.ToString(), lastLocation.longitude.ToString())); 
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    private IEnumerator FetchWeatherDataFromApi(string latitude, string longitude)
    {
        string url = currentWeatherApi + "lat=" + latitude + "&lon=" + longitude + "&cnt=7" + "&appid=" + apiKey + "&units=metric";
        //string url = "api.openweathermap.org/data/2.5/forecast/daily?lat=35&lon=139&cnt=10&appid=3002bc312f0944b99d15b79863b810fb";
        UnityWebRequest fetchWeatherRequest = UnityWebRequest.Get(url);
        yield return fetchWeatherRequest.SendWebRequest();
        if (fetchWeatherRequest.isNetworkError || fetchWeatherRequest.isHttpError)
        {
            //Check and print error 
            statusText.text = fetchWeatherRequest.error;
        }
        else
        {
            Debug.Log(fetchWeatherRequest.downloadHandler.text);
            var response = JSON.Parse(fetchWeatherRequest.downloadHandler.text);

            description.text = response["daily"][0]["weather"][0]["description"];

            //convert timestamp to date string
            var dayJson = response["daily"][0]["dt"];
            var doubleDay = Convert.ToDouble(dayJson);
            var dateT = UnixTimeStampToDateTime(doubleDay);
            dayFull.text = dateT.ToString("ddd, dd MMM yy");

            temperature.text = response["daily"][0]["temp"]["day"] + " °C";
        }
    }
}