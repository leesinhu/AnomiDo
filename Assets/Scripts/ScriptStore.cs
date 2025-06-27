using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStore : MonoBehaviour
{
    protected virtual void Awake()
    {
        sentenceList_info1.Add("누군가 넘어졌어. 줄이 막히지 않도록 <color=#FFC90E>넘어진 승객을 선 밖으로 밀어내.</color>");
        sentenceList_info2.Add("냉각포 충전 중이야. 급한 상황이라면 <color=#FFC90E>네 손</color>으로 직접 처리하는 것도 방법이겠지..");
        sentenceList_info3.Add("방금 멀쩡한 승객을 넘어뜨렸는데, 그건 네 전략이야, 실수야?");
        sentenceList_info4.Add("대기줄의 누군가가 불만이 많아 보이네. 단독 행동을 할 것 같아.");
        sentenceList_info5.Add("남은 정차 시간 <color=#FFC90E>30초</color>. 도미니언의 속도는 <color=#FFC90E>체온이 낮을수록 빨라진다</color>는 거, 기억하지?");
        sentenceList_info6.Add("<color=#FFC90E>5명</color> 남았어. 승객들한테 빨리 빨리 좀 움직이라고 말해줄래? ..아니, 그냥 해본 소리야.");
        sentenceList_info7.Add("발을 조심하지 않으면 방금처럼 네가 승객을 넘어뜨리게 되는 거야.. 항상 승객들과 <color=#FFC90E>안전 거리</color>를 유지해.");
        sentenceList_info8.Add("좋아. 무단 이탈자를 넘어뜨리는 일은 규정 상에도 명시된 적법한 절차니까 안심해도 돼.");
        sentenceList_info9.Add("아, 내가 무단 이탈자를 얼려 버렸거든? 냉각이 풀리면 <color=#FFC90E>가능한 한 빨리</color> 제압해 줘.");
        sentenceList_info10.Add("<color=#FFC90E>연쇄충돌</color> 발생. 일이 번거로워지겠군.");
        sentenceList_info11.Add("저 친구, 얼음 속에 <color=#FFC90E>너무 오래</color> 갇혀 있었잖아. 다음부턴 손이 놀고 있으면 얼음부터 깨.");
        sentenceList_info12.Add("대기줄이 너무 지체되고 있어. 넘어진 도미니언이 줄을 막고 있진 않은지도 잘 살펴보자.");
        sentenceList_info13.Add("후.. 무단 이탈자가 게이트를 통과했네. 정직하게 서 있는 승객들이 <color=#FFC90E>열</color>을 꽤 받았겠어..");
        sentenceList_info14.Add("조종실 자리가 비어 있다. 냉각포를 사용하려면 비행선으로 돌아가야 한다.");

        stDictionary.Add("info1", sentenceList_info1);
        stDictionary.Add("info2", sentenceList_info2);
        stDictionary.Add("info3", sentenceList_info3);
        stDictionary.Add("info4", sentenceList_info4);
        stDictionary.Add("info5", sentenceList_info5);
        stDictionary.Add("info6", sentenceList_info6);
        stDictionary.Add("info7", sentenceList_info7);
        stDictionary.Add("info8", sentenceList_info8);
        stDictionary.Add("info9", sentenceList_info9);
        stDictionary.Add("info10", sentenceList_info10);
        stDictionary.Add("info11", sentenceList_info11);
        stDictionary.Add("info12", sentenceList_info12);
        stDictionary.Add("info13", sentenceList_info13);
        stDictionary.Add("info14", sentenceList_info14);
    }

    //Dictionary 명칭(string)과 저장한 문장들(string
    //List)
    public Dictionary<string, List<string>> stDictionary = new Dictionary<string, List<string>>();

    List<string> sentenceList_info1 = new List<string>();
    List<string> sentenceList_info2 = new List<string>();
    List<string> sentenceList_info3 = new List<string>();
    List<string> sentenceList_info4 = new List<string>();
    List<string> sentenceList_info5 = new List<string>();
    List<string> sentenceList_info6 = new List<string>();
    List<string> sentenceList_info7 = new List<string>();
    List<string> sentenceList_info8 = new List<string>();
    List<string> sentenceList_info9 = new List<string>();
    List<string> sentenceList_info10 = new List<string>();
    List<string> sentenceList_info11 = new List<string>();
    List<string> sentenceList_info12 = new List<string>();
    List<string> sentenceList_info13 = new List<string>();
    List<string> sentenceList_info14 = new List<string>();
}
