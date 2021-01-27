using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BundleLibrary : MonoBehaviour
{
    AssetBundle Kart;
    Material[] mats;

    public static BundleLibrary Singleton;
    // Start is called before the first frame update
    void Awake()
    {
        Kart = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "kartmat"));
        mats = Kart.LoadAllAssets<Material>();
        Singleton = this;
    }

    public Material GetSpecificMat(string _mat)
    {
        foreach (Material mat in mats)
        {
            if (mat.name == _mat)
                 return mat;
        }
        return null;
    }
}
