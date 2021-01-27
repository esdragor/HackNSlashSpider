using CotcSdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    Login loginScript;
    // Start is called before the first frame update
    void Start()
    {
        ResumeSession();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResumeSession()
    {
        var cotc = FindObjectOfType<CotcGameObject>();

        loginScript = cotc.GetComponent<Login>();

        cotc.GetCloud().Done(cloud =>
        {
            cloud.ResumeSession(
                gamerId: loginScript.ConnectionParameters.GamerID /*"5873a117b69fa8c942c7df08"*/,
                gamerSecret: loginScript.ConnectionParameters.gamerSecret /*"c3b7c6fab599919b0c24487bf46d0e6069472df0"*/)
            .Done(gamer =>
            {
                Debug.Log("Signed in succeeded (ID = " + gamer.GamerId + ")");
                Debug.Log("Login data: " + gamer);
                Debug.Log("Server time: " + gamer["servertime"]);
            }, ex =>
            {
                // The exception should always be CotcException
                CotcException error = (CotcException)ex;
                Debug.LogError("Failed to login: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
            });
        });
    }

}
