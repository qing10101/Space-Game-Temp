using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipViewHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int index = (int)transform.position.x/400;
        ShipViewer.shipNames[index] = gameObject.name;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
