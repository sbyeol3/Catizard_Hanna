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

    // Start is called before the first frame update
    void Awake()
    {
        HeroSlider.maxValue = GameMinute * 120;
        StartCoroutine("HeroTimer");
    }

    IEnumerator HeroTimer()
    {
        print("start");
        while (isGame)
        {
            if (HeroSlider.value <= 0)
            {
                isGame = false;
            }
            HeroSlider.value -= HeroSpeed;
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

    public void On_SOS()
    {
        HeroSpeed = 2;
        Invoke("Off_SOS", 10f);
    }

    public void Off_SOS()
    {
        HeroSpeed = 1;
    }

}
