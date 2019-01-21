using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class N_MainPage_Chap2 : MonoBehaviour, IPointerDownHandler
{
    
    public GameObject UnlockScreen, ErrorScreen;
    public Image ErrorImage;
    public Text GoldAmount;
    private Image Chap2Image;
    public Sprite[] Chap2;

    // Start is called before the first frame update
    void Start()
    {
        Chap2Image = GetComponent<Image>();
        if (!N_PlayerInfo.UnLock)
            Chap2Image.sprite = Chap2[0];
        else
            Chap2Image.sprite = Chap2[1];
    }

    // 마우스가 눌릴 때
    public void OnPointerDown(PointerEventData data)
    {
        if (!N_PlayerInfo.UnLock)
        {
            UnlockScreen.SetActive(true);
        }
        else
        {
            // 챕터2 게임진행
        }
    }

    // 챕터2 해금
    public void Unlock()
    {
        if (N_PlayerInfo.Gold >= 1000)
        {
            N_PlayerInfo.Gold -= 1000;
            GoldAmount.text = "" + N_PlayerInfo.Gold;
            N_PlayerInfo.UnLock = true;
            Chap2Image.sprite = Chap2[1];
            CloseUnlockScreen();
        }
        else
        {
            CloseUnlockScreen();
            StartCoroutine("Error");
        }
    }

    // 골드 부족할 때
    IEnumerator Error()
    {
        float valueA = 1;
        ErrorScreen.SetActive(true);
        for (int i = 1; i <= 10; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            valueA -= 0.1f;
            ErrorImage.color = new Color(1, 1, 1, valueA);
        }
        ErrorScreen.SetActive(false);
    }

    public void CloseUnlockScreen()
    {
        UnlockScreen.SetActive(false);
    }

}
