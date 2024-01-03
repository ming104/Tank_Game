using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [Header("땅 배열")]
    public GameObject[] GroundArray;
    /*
        [0][1][2]
        [3][4][5]
        [6][7][8]
    */

    //플레이어 탱크
    [Header("플레이어")]
    [SerializeField] private GameObject Player;

    // 타일 크기
    [Header("타일 크기")]
    [SerializeField] private float UnitSize;

    // 시야 밖에 타일없으면 타일 갱신함
    [Header("시야")]
    float HalfSight = 25;

    [Header("전체 타일크기")]
    Vector3[] border;

    // Start is called before the first frame update
    void Start()
    {
        border = new Vector3[]
        {
            new Vector3(-UnitSize * 1.5f, 0 , UnitSize * 1.5f), //0
            new Vector3(UnitSize * 1.5f, 0 , -UnitSize * 1.5f), //1
        };
    }

    // Update is called once per frame
    void Update()
    {
        BounderyCheck();
    }

    void BounderyCheck()
    {
        // 오른쪽
        if (border[1].x < Player.transform.position.x + HalfSight)
        {
            border[0] += Vector3.right * UnitSize;
            border[1] += Vector3.right * UnitSize;

            MoveWorld(0);
        }

        // 왼쪽
        else if (border[0].x > Player.transform.position.x - HalfSight)
        {
            border[0] -= Vector3.right * UnitSize;
            border[1] -= Vector3.right * UnitSize;

            MoveWorld(2);
        }

        // 위쪽
        if (border[0].z < Player.transform.position.z + HalfSight)
        {
            border[0] += Vector3.forward * UnitSize;
            border[1] += Vector3.forward * UnitSize;

            MoveWorld(1);
        }

        // 아래쪽
        if (border[1].z > Player.transform.position.z - HalfSight)
        {
            border[0] -= Vector3.forward * UnitSize;
            border[1] -= Vector3.forward * UnitSize;

            MoveWorld(3);
        }
    }

    // 타일 움직이기
    void MoveWorld(int dir)
    {
        GameObject[] _GroundArray = new GameObject[9]; // 배열 생성
        System.Array.Copy(GroundArray, _GroundArray, 9); // 배열 9개 복사

        switch (dir) // 스위치 문으로 4방향 이동코드 작성
        {
            case 0: // 오른쪽으로 이동 시
                {
                    for (int i = 0; i < 9; i++) // 9번을 실행함
                    {
                        int revise = i - 3; //이걸 하면

                        if (revise < 0)
                        {
                            /*
                            [0][1][2] // 얘네가 포함됨 -3, -2, -1
                            [3][4][5]
                            [6][7][8] 
                            */
                            GroundArray[9 + revise] = _GroundArray[i]; // 그래서 6, 7, 8 뽑힘 + 여기다가 i값인 0, 1, 2가 들어감
                            _GroundArray[i].transform.position += Vector3.right * UnitSize * 3; // 이동시키는 코드
                        }
                        else
                        {
                            GroundArray[revise] = _GroundArray[i]; // 나머지 해당이되지 않은 3,4,5,6,7,8은 숫자가 0,1,2,3,4,5로 바뀌어 들어감
                        }
                        /*
                            [0][1][2]    [3][4][5]                                                  [0 (3)][1 (4)][2 (5)]
                            [3][4][5] => [6][7][8] 이렇게 바뀜 하지만 바뀐 채로 초기화가 되기 때문에 => [3 (6)][4 (7)][5 (8)] 배열은 저렇게 초기화 됨
                            [6][7][8]    [0][1][2]                                                  [6 (0)][7 (1)][8 (2)]
                        */
                    }
                    break;
                }
            case 1: // 위쪽으로 이동 시
                {
                    for (int i = 0; i < 9; i++) // 9번 실행
                    {
                        int revise = i % 3; // 이걸 하면

                        if (revise == 2)
                        {
                            /*
                            [0][1][2] 2
                            [3][4][5] 5
                            [6][7][8] 8 이 걸리게 됨
                            */
                            GroundArray[i - 2] = _GroundArray[i]; //빼게되면 0 ,3 ,6이 뽑힘 + 여기다가 i값인 2,5,8이 뽑힘
                            _GroundArray[i].transform.position += Vector3.forward * UnitSize * 3;
                        }
                        else
                        {
                            GroundArray[i + 1] = _GroundArray[i];
                        }
                    }
                    break;
                }
            case 2: // 왼쪽으로 이동 시
                {
                    for (int i = 0; i < 9; i++)
                    {
                        int revise = i + 3;

                        if (revise > 8)
                        {
                            GroundArray[revise - 9] = _GroundArray[i];
                            _GroundArray[i].transform.position -= Vector3.right * UnitSize * 3;
                        }
                        else
                        {
                            GroundArray[revise] = _GroundArray[i];
                        }
                    }
                    break;
                }
            case 3: // 아래쪽으로 이동시
                {
                    for (int i = 0; i < 9; i++)
                    {
                        int revise = i % 3;

                        if (revise == 0)
                        {
                            GroundArray[i + 2] = _GroundArray[i];
                            _GroundArray[i].transform.position -= Vector3.forward * UnitSize * 3;
                        }
                        else
                        {
                            GroundArray[i - 1] = _GroundArray[i];
                        }
                    }
                    break;
                }
        }
    }
}
