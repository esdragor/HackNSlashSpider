using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CotcSdk;
using UnityEngine.UI;
using System;

public class Achievement : MonoBehaviour
{
    [SerializeField] Text AchievementText;
    Login loginScript;

    // Start is called before the first frame update
    void Start()
    {
        loginScript = FindObjectOfType<CotcGameObject>().GetComponent<Login>();
        CheckAchievements(100);
        PrintAchievementsUnlock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckAchievements(long timerRoad)
{
    // currentGamer is an object retrieved after one of the different Login functions.
    Debug.Log(loginScript.nbRoad);
    loginScript.ConnectionParameters.gamerSetup.Achievements.Domain("private").List().Done(listAchievementsRes =>
    {
        foreach (var achievement in listAchievementsRes)
        {
            //Debug.Log("AAAA: " + achievement.Key + " : " + achievement.Value.Config.ToString());
            //Debug.Log("AAAA: " + achievement.Key + " : " + achievement.Value.Config["maxValue"]);
            if (achievement.Key == "Begin to road" && achievement.Value.Progress < 1.0f && loginScript.nbRoad > 0)
            {
                achievement.Value.Progress = 1.0f;
                loginScript.achieve[0] = true;
            }
            if (achievement.Key == "66 Road" && achievement.Value.Progress < 1.0f && loginScript.nbRoad >= int.Parse(achievement.Value.Config["maxValue"]))
            {
                achievement.Value.Progress = 1.0f;
                loginScript.achieve[1] = true;
            }
            if (achievement.Key == "Flash" && achievement.Value.Progress < 1.0f && timerRoad <= int.Parse(achievement.Value.Config["maxValue"]))
            {
                achievement.Value.Progress = 1.0f;
                loginScript.achieve[2] = true;
            }

        }
        PrintAchievementsUnlock();
        loginScript.SetUserValue("Unlock", Convert.ToInt32(loginScript.achieve[0]) + "," +
                                           Convert.ToInt32(loginScript.achieve[1]) + "," +
                                           Convert.ToInt32(loginScript.achieve[2]));
    }, ex =>
    {
        // The exception should always be CotcException
        CotcException error = (CotcException)ex;
        Debug.LogError("Could not list achievements: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
    });
}

void PrintAchievementsUnlock()
{
        // currentGamer is an object retrieved after one of the different Login functions.
        int cpt = -1;
    loginScript.ConnectionParameters.gamerSetup.Achievements.Domain("private").List().Done(listAchievementsRes =>
    {
        foreach (var achievement in listAchievementsRes)
        {
            Debug.LogWarning(achievement.Key + " : " + achievement.Value.Config.ToString() + " Percent: " + achievement.Value.Progress);
            if (achievement.Value.Progress >= 1.0f || loginScript.achieve[++cpt] == true)
            {
                AchievementText.text += achievement.Key + "\n";
            }
        }
    }, ex =>
    {
        // The exception should always be CotcException
        CotcException error = (CotcException)ex;
        Debug.LogError("Could not list achievements: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
    });
}
}
