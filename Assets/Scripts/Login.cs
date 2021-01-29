using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CotcSdk;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public struct infosConnect
{
    public string GamerID;
    public string gamerSecret;
    public Gamer gamerSetup;
}

public class Login : MonoBehaviour
{
    [SerializeField] private InputField mailInput = null;
    [SerializeField] private InputField PwdInput = null;
    public infosConnect ConnectionParameters = new infosConnect();
    public List<bool> achieve;

    public int nbRoad = 0;

    // Start is called before the first frame update
    void Start()
    {
        mailInput.text = "pommedeterre@gmail.com";
        PwdInput.text = "1234";
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUserValue(string key, string _value)
    {
        // currentGamer is an object retrieved after one of the different Login functions.

        Bundle value = new Bundle(_value);
        ConnectionParameters.gamerSetup.GamerVfs.Domain("private").SetValue(key, value)
        .Done(setUserValueRes =>
        {
            Debug.Log("User data set: " + setUserValueRes.ToString());
        }, ex =>
        {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not set user data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }

    public void GetUserValue(string key)
    {
        // currentGamer is an object retrieved after one of the different Login functions.

        ConnectionParameters.gamerSetup.GamerVfs.Domain("private").GetValue(key)
        .Done(getUserValueRes =>
        {
            Bundle result = getUserValueRes["result"][key];
            string nb = result.ToString();
            Debug.Log("User data: " + nb.Substring(1, nb.Length - 2));
            if (key == "nbGames")
                nbRoad = int.Parse(nb.Substring(1, nb.Length - 2));
            if (key == "Unlock")
            {
                string result2 = nb.Substring(1, nb.Length - 2);

                achieve[0] = Convert.ToBoolean(int.Parse((result2[0]).ToString()));
                achieve[1] = Convert.ToBoolean(int.Parse((result2[2]).ToString()));
                achieve[2] = Convert.ToBoolean(int.Parse((result2[4]).ToString()));
            }
        }, ex =>
        {
            SetUserValue("nbGames", "0");
            SetUserValue("Unlock", "0,0,0");
            // The exception should always be CotcException
            //CotcException error = (CotcException)ex;
            //Debug.LogError("Could not get user data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }

    public void LoginNetwork()
    {
        var cotc = FindObjectOfType<CotcGameObject>();

        cotc.GetCloud().Done(cloud =>
        {
            Promise<Gamer> user = cloud.Login(
                network: "email",
                networkId: mailInput.text,
                networkSecret: PwdInput.text);
            user.Done(gamer =>
            {
                Debug.Log("Signed in succeeded (ID = " + gamer.GamerId + ")\n");
                Debug.Log("Login data: " + gamer);
                Debug.Log("Server time: " + gamer["servertime"]);

                ConnectionParameters.GamerID = gamer.GamerId;
                ConnectionParameters.gamerSecret = gamer.GamerSecret;
                ConnectionParameters.gamerSetup = gamer;
                GetUserValue("Unlock");



                SceneManager.LoadScene("SampleScene");




            }, ex =>
            {
                // The exception should always be CotcException
                CotcException error = (CotcException)ex;
                Debug.LogError("Failed to login: " + error.ErrorCode + " (" + error.Message + ")");



                Debug.Log("BAD");



            });
        });
    }

}
