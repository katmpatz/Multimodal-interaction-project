using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformScale : MonoBehaviour
{

    private Renderer rend;
    public GameObject testObj;
    private Vector3 scaleChange, positionChange;

    // Start is called before the first frame update
    void Start()
    {
        rend = testObj.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //change renderer size
        rend.transform.localScale = new Vector3(0.05f, 0.03f, 0.03f);
    }
}
