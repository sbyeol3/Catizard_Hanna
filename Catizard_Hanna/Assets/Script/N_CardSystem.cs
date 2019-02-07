using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class N_CardSystem : MonoBehaviour
{

    public bool isGame = true;
    public int GameMinute = 5, HeroSpeed = 1;
    public Slider HeroSlider;
    public Animator HeroAnimator;
    public GameObject HeroSOS, HeroCurse, HeroDual;
    public RectTransform Hero;
    public RectTransform[] HeroABC;
    public GridView gridView;

    private int SOS_repeat = 0;

    // Start is called before the first frame update
    void Awake()
    {
        HeroSlider.maxValue = GameMinute * 120;
        StartCoroutine("HeroTimer");
    }

    IEnumerator HeroTimer()
    {
        while (isGame)
        {
            if (HeroSlider.value <= 0)
            {
                isGame = false;
            }
            HeroSlider.value -= HeroSpeed;
            for(int i = 0; i < 3; i++)
            {
                HeroABC[i].anchorMin = new Vector2(Hero.anchorMin.x, 0);
                HeroABC[i].anchorMax = new Vector2(Hero.anchorMax.x, 0);
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    // 카드 기능 함수
    public void CardFunction(int num)
    {
        switch (num)
        {
            case 7:
                On_SOS();
                break;
        }
    }

    // 비둘기 전갈 카드 (시간 누적 O)
    public void On_SOS()
    {
        SOS_repeat++;

        if (SOS_repeat == 1)
        {
            HeroAnimator.SetBool("run", true);
            HeroSpeed = 2;
            HeroSOS.SetActive(true);
            StartCoroutine("Off_SOS");
        }
    }

    IEnumerator Off_SOS()
    {
        while (SOS_repeat > 0)
        {
            yield return new WaitForSecondsRealtime(10f);
            SOS_repeat--;
        }
        
        HeroAnimator.SetBool("run", false);
        HeroSpeed = 1;
        HeroSOS.SetActive(false);
    }

    // JPS
    public void JPS_Test()
    {
        gridView.JPS();
    }

}
