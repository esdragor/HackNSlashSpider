using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CotcSdk;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct infosConnect
{
    public string GamerID;
    public string gamerSecret;
}

public class Login : MonoBehaviour
{
    [SerializeField] private InputField mailInput = null;
    [SerializeField] private InputField PwdInput = null;
    public infosConnect ConnectionParameters = new infosConnect();

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

                //    int ptr1 = infos.IndexOf("gamer_id");
                //string cutstr = infos.Substring(ptr1, infos.Length - ptr1);
                //    int ptr2 = cutstr.IndexOf(",");

                //    Debug.Log("\n\nPOIRE:\n" + infos.Substring(ptr1, ptr2));


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
