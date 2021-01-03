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
    public TextMeshPro temperature;
    public TextMeshPro dayFull;
    public TextMeshPro description;
    public TextMeshProUGUI statusText;
    private LocationInfo lastLocation;
    public GameObject ARObject;
    public GameObject weatherPanel;
    public Sprite cloudy;
    public Sprite sun;
    public Sprite veryCloudy;
    public Sprite rain;
    public Sprite storm;
    public Sprite snow;
    public Sprite mist;
    public SpriteRenderer sr;

    Vector3 positionWindow;

    private CalPosition cp;

    void Start()
    {
        print("Start");
        weatherPanel.gameObject.SetActive(true);
        sr = transform.Find("weatherNew/weatherIcon").gameObject.GetComponent<SpriteRenderer>();
        UpdateWeatherData();

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
            Debug.Log(fetchWeatherRequest.downloadHandler.text);
            var response = JSON.Parse(fetchWeatherRequest.downloadHandler.text);

            //Get description for weather
            description.text = response["daily"][dateNumber]["weather"][0]["description"];

            //convert timestamp to date string
            var dayJson = response["daily"][dateNumber]["dt"];
            var doubleDay = Convert.ToDouble(dayJson);
            var dateT = UnixTimeStampToDateTime(doubleDay);
            dayFull.text = dateT.ToString("ddd, dd MMM yy");

            //temperature.text = response["daily"][dateNumber]["temp"]["day"] + " °C";

            var tempJson = response["daily"][dateNumber]["temp"]["day"];
            var roundTemp = Math.Round(Convert.ToDouble(tempJson));
            temperature.text = roundTemp + " °C";

            //Get icons for weather
            var icon = response["daily"][dateNumber]["weather"][0]["icon"];
            switch (icon)
            {
                case "01d":
                    sr.sprite = sun;
                    break;
                case "02d":
                    sr.sprite = cloudy;
                    break;
                case "03d":
                    sr.sprite = veryCloudy;
                    break;
                case "04d":
                    sr.sprite = veryCloudy;
                    break;
                case "09d":
                    sr.sprite = rain;
                    break;
                case "10d":
                    sr.sprite = rain;
                    break;
                case "11d":
                    sr.sprite = storm;
                    break;
                case "13d":
                    sr.sprite = snow;
                    break;
                case "50d":
                    sr.sprite = mist;
                    break;
            }
        }
    }
}