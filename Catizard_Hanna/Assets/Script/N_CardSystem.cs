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
    public Transform Cat;
    public GridView gridView;

    private int SOS_repeat = 0;
    private float blockSize, blockBuffer;

    // Start is called before the first frame update
    void Awake()
    {
        blockSize = gridView.blockSize;
        blockBuffer = gridView.blockBuffer;
        HeroSlider.maxValue = GameMinute * 120;
        StartCoroutine("HeroTimer");
        StartCoroutine("CatMove");
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
    IEnumerator CatMove()
    {
        // 게임 시작 후 잠시동안은 움직이지 않음
        Cat.position = new Vector3(blockSize - 7.4f, 6 * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize + 2.3f);
        yield return new WaitForSecondsRealtime(4f);

        while (isGame)
        {
            
            // 다음 순서의 길이 있다면 다음 노드로 이동
            if (gridView.minIndex>=0&&gridView.isPath && gridView.CatIndex < gridView.CatPath[gridView.minIndex].Count)
            {
                gridView.CatIndex++;
                Point next = gridView.CatPath[gridView.minIndex][gridView.CatIndex];
                bool isColumn = next.column % 2 == 1 ? true : false;
                bool isRow = next.row % 2 == 1 ? true : false;
                float xSize = 0, ySize = 0;

                // 위치 지정
                if (isColumn)
                {
                    xSize = (next.column + 1) * 0.5f * (blockSize * 7f + blockBuffer) - blockSize * 3f;
                }
                else
                {
                    xSize = next.column * 0.5f * (blockSize * 7f + blockBuffer) + blockSize;
                }
                if (isRow)
                {
                    ySize = (next.row + 1) * 0.5f * -(blockSize * 7f + blockBuffer) + blockSize * 3f;
                }
                else
                {
                    ySize = next.row * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize;
                }

                // 시작 위치 변경
                gridView.temp_x = next.column;
                gridView.temp_y = next.row;

                Cat.position = new Vector3(xSize-7.4f, ySize+2.3f);
            }

            yield return new WaitForSecondsRealtime(2f);
        }
    }

}
