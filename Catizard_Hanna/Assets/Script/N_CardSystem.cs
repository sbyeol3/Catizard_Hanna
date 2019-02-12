using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class N_CardSystem : MonoBehaviour
{

    public bool isGame = true, isCurse = false, isSOS = false;
    public int GameMinute = 5, HeroSpeed = 1, cat_wait = 4;
    public Slider HeroSlider;
    public Animator HeroAnimator;
    public GameObject HeroSOS, HeroCurse, HeroDual;
    public RectTransform Hero;
    public RectTransform[] HeroABC;
    public Transform Cat, SP_bar;
    public GridView gridView;
    public Slider SP_Slider;
    public GameObject[] Cat_graphic;
   

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
        Cat_graphic[1].SetActive(false);
        Cat_graphic[0].SetActive(true);
    }

    private void Update()
    {
        Vector3 temp = Cat.position;
        SP_bar.position = new Vector3(temp.x + 0.05f, temp.y - 0.4f);
    }

    IEnumerator HeroTimer()
    {
        while (isGame)
        {
            if (HeroSlider.value <= 0)
            {
                isGame = false;
            }
            if (HeroSlider.value < HeroSlider.maxValue && isCurse)
            {
                HeroSlider.value += HeroSpeed;
            }
            else
            {
                HeroSlider.value -= HeroSpeed;
            }

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
        isSOS = true;
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
        isSOS = false;
        HeroDual.SetActive(false);
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

                Cat.position = new Vector3(xSize-7.4f, ySize+2.3f); // 수동으로 변경할 부분 좌표계
            }

            for (int i = 0; i < cat_wait; i++)
            {
                if (SP_Slider.value < 100)
                {
                    SP_Slider.value++;
                }
            yield return new WaitForSecondsRealtime(1f);

            }
        }
    }

    public void Cat_rest()
    {
        Cat_graphic[0].SetActive(false);
        Cat_graphic[1].SetActive(true);
        cat_wait = 12;
        Invoke("Cat_SPplus", 12f);

    }

    void Cat_SPplus()
    {
        if (SP_Slider.value <= 70)
        {

            SP_Slider.value += 30;
        }
        else
            SP_Slider.value = 100;
        cat_wait = 4;
        Cat_graphic[0].SetActive(true);
        Cat_graphic[1].SetActive(false);
    }

    public void Cat_curse()
    {
        if (SP_Slider.value < 30)
            return;

        isCurse = true;
        Hero.localScale = new Vector3(-1, 1, 1);
        HeroCurse.SetActive(true);
        SP_Slider.value -= 30;
        if (isSOS)
        {
            HeroSOS.SetActive(false);
            HeroDual.SetActive(true);
        }
        Invoke("After_curse", 15f);

    }

    void After_curse()
    {
        isCurse = false;
        HeroCurse.SetActive(false);
        HeroDual.SetActive(false);
        if (isSOS)
            HeroSOS.SetActive(true);
        Hero.localScale = new Vector3(1, 1, 1);
    }

    



    
}
