using UnityEngine;
public class ScoreManager : MonoBehaviour
{

    public void AddScore(int level, float time)
    {
        string arena = "levelTmp_" + level;
        PlayerPrefs.SetFloat(arena, time);
        PlayerPrefs.SetInt("currentLevel", level);
    }

    public float GetCurrentScore()
    {
        int level = PlayerPrefs.GetInt("currentLevel");

        string arenaTmp = "levelTmp_" + level;
        float tmp = PlayerPrefs.GetFloat(arenaTmp);

        string arena = "level_" + level;
        float last = PlayerPrefs.GetFloat(arena);

        if (tmp > last || last == 0 && tmp > 1)
        {
            PlayerPrefs.DeleteKey(arena);
            PlayerPrefs.SetFloat(arena, tmp);
            PlayerPrefs.DeleteKey(arenaTmp);
        }
        PlayerPrefs.DeleteKey("currentLevel");
        return (tmp);
    }
    public float getHighScore(int level)
    {
        string arena = "level_" + level;

        return PlayerPrefs.GetFloat(arena);
    }
}

