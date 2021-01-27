using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMat : MonoBehaviour
{
    [SerializeField] string mat_need;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = BundleLibrary.Singleton.GetSpecificMat(mat_need).color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
