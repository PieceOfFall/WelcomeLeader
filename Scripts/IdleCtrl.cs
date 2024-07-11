using UnityEngine;

public class IdleCtrl : MonoBehaviour
{
    public GameObject IdleObj;
    public float timeoutSeconds = 5; // �û��޲����ĳ�ʱʱ�䣨�룩
    private float lastInteractionTime; // ��¼������ʱ��

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        // ����Ƿ��м������������ƶ�
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
        {
            if(IdleObj.activeSelf)
            {
                ResetTimer();
                IdleObj.SetActive(false);
            } else
            {
                ResetTimer();
            }
        }

        // ����Ƿ�ʱ
        if (Time.time - lastInteractionTime > timeoutSeconds && !IdleObj.activeSelf)
        {
            IdleObj.SetActive(true);
        }
    }

    private void OnEnable()
    {
        ResetTimer();
    }

    // ���ü�ʱ��
    void ResetTimer()
    {
        lastInteractionTime = Time.time;
    }
}
