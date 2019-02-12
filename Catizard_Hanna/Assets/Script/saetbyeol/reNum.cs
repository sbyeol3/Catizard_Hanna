using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reNum : MonoBehaviour // 스페이스바 누르는 횟수
{
    private int shape;
    public GameObject wallUI;
    public int spaceNum;
    private int n;
    public bool doChange;

    void Start()
    {
        spaceNum = 0; // 0으로 초깃값 설정
        n = 0;
        doChange = false;

        StartCoroutine("change");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 누르면
        {
            doChange = true;
            n++; // n값 1 증가
            spaceNum = n % 4; // n module 4 ( 0도 90도 180도 270도)
            Debug.Log(spaceNum);
        }
    }

    IEnumerator change()
    {
        while (true)
        {
            shape = spaceNum;
            yield return 0.1f;
            if (doChange)
            {
                switch (shape)
                {
                    case 0:
                        {
                            wallUI.transform.Rotate(0, 0, 90);
                            doChange = false;
                            break;
                        }
                    case 1:
                        {
                            wallUI.transform.Rotate(0, 0, 90);
                            doChange = false;
                            break;
                        }
                    case 2:
                        {
                            wallUI.transform.Rotate(0, 0, 90);
                            doChange = false;
                            break;
                        }
                    case 3:
                        {
                            wallUI.transform.Rotate(0, 0, 90);
                            doChange = false;
                            break;
                        }
                } // switch
            }
        }

    }
}