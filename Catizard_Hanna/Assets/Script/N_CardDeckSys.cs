using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class N_CardDeckSys : MonoBehaviour
{

    public bool isTest;                  // 테스트 모드 : 카드 덱 상태를 Inspector창에서 설정
    public float SuffleTime = 20, BlinkTime = 5, DrawTime = 0.5f;
    public int MaxEnergy = 100;  // 에너지 최대치
    public int TotalCard;               // 총 카드 수
    public int[] CardOrder;           // 카드 순서 관리
    private int[] HandOrder = { 0, 3, 4, 5, 8 };         // 수중의 카드 순서 관리
    public Text CardNText, EnergyText;
    public Animator EnergyAnimator;
    private int CardN, CardIndex, Energy;     // 남은 카드 수, 에너지양
    public GameObject[] WhiteMark;
    public Image[] CardImage;
    public GameObject[] CardObject;
    public Sprite[] CardSprite;     // 카드 그래픽 (뒷면)

    private int left, right;

    // Start is called before the first frame update
    void Start()
    {

        // test상황이면 Inspector창에서 설정한 값으로 덱 상태 설정.
        // 아니면 N_PlayerInfo 스크립트 상에 설정된 값을 불러옴.
        if (!isTest)
        {
            TotalCard = N_PlayerInfo.CardSum;
            CardOrder = new int[TotalCard];
            // 카드를 차곡차곡 CardOrder에 쌓음.
            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < N_PlayerInfo.CardNum[i]; j++)
                {
                    CardOrder[index++] = i;
                }
            }
        }
        else
        {
            if(CardOrder.Length != TotalCard)
            {
                print("Error(Inspector) : 카드 덱 설정 오류");
            }
        }

        CardN = TotalCard;
        CardNText.text = "" + CardN;
        CardIndex = 0;
        CardShuffle();
        StartCoroutine("CardDeckSystem");
        
    }

    IEnumerator CardDeckSystem()
    {
        // 변수 초기화
        float TimeToSleep = SuffleTime - BlinkTime - (DrawTime * 5) - 1;
        int index = 0;

        yield return new WaitForSecondsRealtime(1);

        // 카드덱 시스템
        while (true)
        {
            // 카드를 버린다.
            for (int i = 0; i < 5; i++)
            {
                removeCard(i);
            }
            yield return new WaitForSecondsRealtime(0.75f);
            EnergyAnimator.SetTrigger("Blink");
            yield return new WaitForSecondsRealtime(0.25f);

            // 에너지 회복
            Energy = MaxEnergy;
            EnergyText.text = "" + Energy;

            // 카드를 뽑는다.
            left = right = 0;
            for (int i = 0; i < 5; i++)
            {
                if (CardN > 0)
                {
                    CardNText.text = "" + --CardN;
                    index = CardOrder[CardIndex++];
                    HandOrder[i] = index;
                    CardObject[i].SetActive(true);
                    CardImage[i].sprite = CardSprite[index];
                    // + 카드 드로우 애니메이션
                    yield return new WaitForSecondsRealtime(DrawTime);
                }
                // 뽑을 카드가 없으면 덱을 루프시킨다.
                else
                {
                    // + 카드 루프 애니메이션
                    CardShuffle();
                    CardN = TotalCard;
                    CardNText.text = "" + CardN;
                    CardIndex = 0;
                    i--;
                    yield return new WaitForSecondsRealtime(DrawTime);
                    TimeToSleep -= DrawTime;
                }
            }

            yield return new WaitForSecondsRealtime(TimeToSleep);

            // BlinkTime만큼 깜박인다.
            for (int i = 1; i <= BlinkTime; i++)
            {
                for (int m = 0; m < 5; m++)
                {
                    CardImage[m].color = new Color(0.5f, 0.5f, 0.5f, 1);
                }
                yield return new WaitForSecondsRealtime(0.5f);
                for (int n = 0; n < 5; n++)
                {
                    CardImage[n].color = new Color(1, 1, 1, 1);
                }
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
    }

    // 카드를 섞는다.
    void CardShuffle()
    {
        for (int i = 0; i < TotalCard; i++)
        {
            int random = Random.Range(0, TotalCard);
            int temp = CardOrder[random];
            CardOrder[random] = CardOrder[i];
            CardOrder[i] = temp;
        }
    }

    // 사용된 카드는 버려짐
    void removeCard(int index)
    {
        HandOrder[index] = -1;
        // + 카드 버리는 애니메이션
        CardObject[index].SetActive(false);
        removeMark(index);
    }

    // 마우스가 닿은 카드에는 Marking
    public void setMark(int number)
    {
        if(HandOrder[number] >= 0)
            WhiteMark[number].SetActive(true);
    }

    public void removeMark(int number)
    {
        WhiteMark[number].SetActive(false);
    }

    // 수중의 카드 정보 출력
    public int getCardType(int number)
    {
        return HandOrder[number];
    }

    // 에너지 소비
    public bool setEnergy(int e)
    {
        // 에너지 부족할 시
        if (Energy < e)
            return false;

        Energy -= e;
        EnergyText.text = "" + Energy;
        return true;
    }

    // 마우스 왼쪽으로 카드를 클릭햇을 때
    public void clickLeftCard(int number)
    {
        int CardType = HandOrder[number];
        print(CardType + "번 카드 사용");
        // + HandOrder[number]에 해당하는 액션 취하기

        // 사용된 카드는 버려짐
        removeCard(number);

        if (number < 2)
            left++;
        else if (number > 2)
            right++;

        // 카드 가운데로 모으기
        if (number == 1 && HandOrder[0] != -1)
        {
            CardType = HandOrder[0];
            removeCard(0);
            HandOrder[1] = CardType;
            CardObject[1].SetActive(true);
            CardImage[1].sprite = CardSprite[CardType];
            setMark(1);
        }
        else if (number == 3 && HandOrder[4] != -1)
        {
            CardType = HandOrder[4];
            removeCard(4);
            HandOrder[3] = CardType;
            CardObject[3].SetActive(true);
            CardImage[3].sprite = CardSprite[CardType];
            setMark(3);
        }
        else if (number == 2 && left * right < 4)
        {
            if (left > right)
            {
                for (int i = 3; i <= 4; i++)
                {
                    CardType = HandOrder[i];
                    removeCard(i);
                    HandOrder[i - 1] = CardType;
                    CardObject[i - 1].SetActive(true);
                    CardImage[i - 1].sprite = CardSprite[CardType];
                    if (right == 1)
                        break;
                }
                right++;
            }
            else
            {
                for (int i = 1; i >= 0; i--)
                {
                    CardType = HandOrder[i];
                    removeCard(i);
                    HandOrder[i + 1] = CardType;
                    CardObject[i + 1].SetActive(true);
                    CardImage[i + 1].sprite = CardSprite[CardType];
                    if (left == 1)
                        break;
                }
                left++;
            }
            setMark(2);
        }

    }

}
