using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStore : MonoBehaviour
{
    protected virtual void Awake()
    {
        sentenceList_info1.Add("������ �Ѿ�����. ���� ������ �ʵ��� <color=#FFC90E>�Ѿ��� �°��� �� ������ �о.</color>");
        sentenceList_info2.Add("�ð��� ���� ���̾�. ���� ��Ȳ�̶�� <color=#FFC90E>�� ��</color>���� ���� ó���ϴ� �͵� ����̰���..");
        sentenceList_info3.Add("��� ������ �°��� �Ѿ�߷ȴµ�, �װ� �� �����̾�, �Ǽ���?");
        sentenceList_info4.Add("������� �������� �Ҹ��� ���� ���̳�. �ܵ� �ൿ�� �� �� ����.");
        sentenceList_info5.Add("���� ���� �ð� <color=#FFC90E>30��</color>. ���̴Ͼ��� �ӵ��� <color=#FFC90E>ü���� �������� ��������</color>�� ��, �������?");
        sentenceList_info6.Add("<color=#FFC90E>5��</color> ���Ҿ�. �°������� ���� ���� �� �����̶�� �����ٷ�? ..�ƴ�, �׳� �غ� �Ҹ���.");
        sentenceList_info7.Add("���� �������� ������ ���ó�� �װ� �°��� �Ѿ�߸��� �Ǵ� �ž�.. �׻� �°���� <color=#FFC90E>���� �Ÿ�</color>�� ������.");
        sentenceList_info8.Add("����. ���� ��Ż�ڸ� �Ѿ�߸��� ���� ���� �󿡵� ��õ� ������ �����ϱ� �Ƚ��ص� ��.");
        sentenceList_info9.Add("��, ���� ���� ��Ż�ڸ� ��� ���Ȱŵ�? �ð��� Ǯ���� <color=#FFC90E>������ �� ����</color> ������ ��.");
        sentenceList_info10.Add("<color=#FFC90E>�����浹</color> �߻�. ���� ���ŷο����ڱ�.");
        sentenceList_info11.Add("�� ģ��, ���� �ӿ� <color=#FFC90E>�ʹ� ����</color> ���� �־��ݾ�. �������� ���� ��� ������ �������� ��.");
        sentenceList_info12.Add("������� �ʹ� ��ü�ǰ� �־�. �Ѿ��� ���̴Ͼ��� ���� ���� ���� �������� �� ���캸��.");
        sentenceList_info13.Add("��.. ���� ��Ż�ڰ� ����Ʈ�� ����߳�. �����ϰ� �� �ִ� �°����� <color=#FFC90E>��</color>�� �� �޾Ұھ�..");
        sentenceList_info14.Add("������ �ڸ��� ��� �ִ�. �ð����� ����Ϸ��� ���༱���� ���ư��� �Ѵ�.");

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

    //Dictionary ��Ī(string)�� ������ �����(string
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
