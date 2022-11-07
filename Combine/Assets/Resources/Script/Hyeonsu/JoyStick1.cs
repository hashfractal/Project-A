using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick1 : MonoBehaviour
{
    // Start is called before the first frame update

    // 공개
    public Transform Player;
    public Transform Stick;         // 조이스틱.


    // 비공개
    private Vector3 StickFirstPos;  // 조이스틱의 처음 위치.
    private Vector3 JoyVec;         // 조이스틱의 벡터(방향)
    private float Radius;           // 조이스틱 배경의 반 지름.
    private PlayerMovement player;

    public float Horizontal { get { return JoyVec.x; } set { } }
    public float Vertical { get { return JoyVec.y; } }
    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = Stick.transform.position;

        // 캔버스 크기에대한 반지름 조절.
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        //if (MoveFlag)
            //Player.transform.Translate(Vector3.forward * Time.deltaTime * 5f);
    }

    // 드래그
    public void Drag(BaseEventData _Data)
    {
        PointerEventData Data = _Data as PointerEventData;
        Vector3 Pos = Data.position;

        // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
        JoyVec = (Pos - StickFirstPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float Dis = Vector3.Distance(Pos, StickFirstPos);

        // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는곳으로 이동. 
        if (Dis < Radius)
        {
            Stick.position = StickFirstPos + JoyVec * Dis;
        }

        // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
        else
            Stick.position = StickFirstPos + JoyVec * Radius;

        
    }

    // 드래그 끝.
    public void DragEnd()
    {
        Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
        JoyVec = Vector3.zero;          // 방향을 0으로.
    }

    // 클릭 중...
    public void PointerDown()
    {
        Player.GetComponent<PlayerMovement>().AimTouch = true;      
    }

    // 클릭 끝나면...
    public void PointerUp()
    {
        Player.GetComponent<PlayerMovement>().AimTouch = false;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    OnDrag(eventData);
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    MoveFlag = true;
    //    //PointerEventData Data = _Data as PointerEventData;
    //    Vector3 Pos = eventData.position;

    //    // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
    //    JoyVec = (Pos - StickFirstPos).normalized;

    //    // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
    //    float Dis = Vector3.Distance(Pos, StickFirstPos);

    //    // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는곳으로 이동. 
    //    if (Dis < Radius)
    //    {
    //        Stick.position = StickFirstPos + JoyVec * Dis;
    //    }

    //    // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
    //    else
    //        Stick.position = StickFirstPos + JoyVec * Radius;

    //    //Player.eulerAngles = new Vector3(0 , Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
    //    JoyVec = Vector3.zero;          // 방향을 0으로.
    //    MoveFlag = false;
    //}
}
