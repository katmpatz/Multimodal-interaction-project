using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class calPosWeather : MonoBehaviour
{
    //tracking targets-- markers
    public Transform target1;
    public Transform target2;
    public Transform targetMain;

    //public TMPro.TextMeshProUGUI mytext;
    public GameObject WeatherObject;
    // public GameObject PlaceholderCube1;
    //public GameObject PlaceholderCube2;
    //public GameObject PlaceholderCubeMain;

    Vector3 positionWindow;
    float gap, height;

    void Start()
    {
        //positionWindow = new Vector3(0.0f, 1.0f, 0.0f);
    }

    void Update()
    {
        //calculate the width and height of the Cube and scale it
        gap = Vector3.Distance(targetMain.position, target1.position);
        height = Vector3.Distance(targetMain.position, target2.position);
        WeatherObject.transform.localScale = new Vector3(1.0f * gap, 0.00034f, 1.0f * height);

        //place the Cube by markers' positions
       // positionWindow.Set((targetMain.position.x + target1.position.x) / 2, (targetMain.position.y + target2.position.y) / 2, (targetMain.position.z + target2.position.z) / 2);
        //WeatherObject.transform.position = positionWindow;



    }
}

