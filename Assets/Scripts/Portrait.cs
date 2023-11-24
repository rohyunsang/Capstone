using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour // Application ratio fix
{
    void Awake(){
        Screen.SetResolution(564, 960, true);
        Screen.fullScreen = true;
    }
}
