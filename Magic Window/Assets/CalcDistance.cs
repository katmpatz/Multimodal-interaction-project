using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CalcDistance : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target1;
    public Transform target2;
    public Transform targetMain;

    //public TMPro.TextMeshProUGUI mytext;
    public GameObject ARObject;
    // public GameObject PlaceholderCube1;
    //public GameObject PlaceholderCube2;
    //public GameObject PlaceholderCubeMain;

    Vector3 positionWindow;
    float gap, height;

    void Start()
    {
        positionWindow = new Vector3(0.0f, 1.0f, 0.0f);
    }

    void Update()
    {

        //  gap = Vector3.Distance(PlaceholderCubeMain.transform.position, PlaceholderCube1.transform.position);
        gap = Vector3.Distance(targetMain.position, target1.position);
        height = Vector3.Distance(targetMain.position, target2.position);

        //height = Vector3.Distance(PlaceholderCubeMain.transform.position, PlaceholderCube2.transform.position);


        //Debug.Log("target1 is " + screenPos.x + " pixels from the left");
        //Debug.Log("target2 is " + screenPos.x + " pixels from the left");

        // mytext.text ="\n gap width = " + Vector3.Distance(target.position, target2.position);

        //gap = Vector3.Distance(target.position, target2.position);

        ARObject.transform.localScale = new Vector3(1.0f * gap, 0.00034f, 1.0f * height);

        positionWindow.Set((targetMain.position.x + target1.position.x) / 2, targetMain.position.y, (targetMain.position.z + target2.position.z) / 2);
        ARObject.transform.position = positionWindow;

        //ARObject.transform.position = new Vector3((targetMain.position.x + target1.position.x) / 2, targetMain.position.y, (targetMain.position.z + target2.position.z) / 2);




        //mytext.text =
        //   "\n width = " + gap
        //   + "\n height = " + height;

           
    }
}
