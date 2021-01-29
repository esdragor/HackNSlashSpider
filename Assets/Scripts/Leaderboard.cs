using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CotcSdk;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Text LeaderboardText = null;

    Login loginScript;
    public long Value = -1;

    // Start is called before the first frame update
    void Start()
    {
        loginScript = FindObjectOfType<CotcGameObject>().GetComponent<Login>();
        UserBestScores();
        BestHighScores();



        FindScoreUser();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PostScore(long Score)
    {
        // currentGamer is an object retrieved after one of the different Login functions.
        if (Score < Value || Value < 0)
        {
            loginScript.ConnectionParameters.gamerSetup.Scores.Domain("private").Post(Score, "intermediateMode", ScoreOrder.LowToHigh,
            "context for score", true)
            .Done(postScoreRes =>
            {
                Debug.Log("Post score: " + postScoreRes.ToString());
            }, ex =>
            {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
                Debug.LogError("Could not post score: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
            });
            BestHighScores();
        }
    }

    void UserBestScores()
    {
        // currentGamer is an object retrieved after one of the different Login functions.

        loginScript.ConnectionParameters.gamerSetup.Scores.Domain("private").ListUserBestScores()
        .Done(listUserBestScoresRes =>
        {
            foreach (var score in listUserBestScoresRes)
                Debug.Log(score.Key + ": " + score.Value.Value);
        }, ex =>
        {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not get user best scores: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }

    void BestHighScores()
    {
        // currentGamer is an object retrieved after one of the different Login functions.
        string Leader = "Leaderboard global:\n";
        loginScript.ConnectionParameters.gamerSetup.Scores.Domain("private").BestHighScores("intermediateMode", 10, 1)
        .Done(bestHighScoresRes =>
        {
            foreach (var score in bestHighScoresRes)
            {
                Debug.Log(score.Rank + ". " + score.GamerInfo["profile"]["displayName"] + ": " + score.Value);
                Leader += score.Rank + ". " + score.GamerInfo["profile"]["displayName"] + ": " + score.Value + "s\n";
            }
            LeaderboardText.text = Leader;
        }, ex =>
        {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not get best high scores: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }


    long FindScoreUser()
    {
        string _User = "";
        loginScript.ConnectionParameters.gamerSetup.Profile.Get()
        .Done(profileRes =>
        {
            Debug.Log("Profile data: " + profileRes["displayName"]);
            _User = profileRes["displayName"];
        }, ex =>
        {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not get profile data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
        // currentGamer is an object retrieved after one of the different Login functions.
        loginScript.ConnectionParameters.gamerSetup.Scores.Domain("private").BestHighScores("intermediateMode", 10, 1)
        .Done(bestHighScoresRes =>
        {
            foreach (var score in bestHighScoresRes)
            {
                if (_User == score.GamerInfo["profile"]["displayName"])
                {
                    Value = score.Value;
                }
            }
        }, ex =>
        {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not get best high scores: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
        return Value;
    }
}
