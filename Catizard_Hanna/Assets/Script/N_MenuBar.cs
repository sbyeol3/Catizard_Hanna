using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class N_MenuBar : MonoBehaviour
{

    public Text CardSum, GoldAmount;

    // Start is called before the first frame update
    void Start()
    {
        GoldAmount.text = "" + N_PlayerInfo.Gold;
        CardSum.text = "" + N_PlayerInfo.CardSum;
    }

    public void GotoMain()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void GotoStore()
    {
        // 상점 화면으로 이동
    }

    public void GotoDeck()
    {
        // 덱 화면으로 이동
    }

    public void GotoStoryBook()
    {
        SceneManager.LoadScene("StoryBookScene");
    }

    public void GotoHelp()
    {
        SceneManager.LoadScene("HelpScene");
    }

    public void Quit()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else

        Application.Quit();   // 종료한다

#endif
    }

}
