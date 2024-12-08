using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//operation.isDone; // �۾��� �Ϸ� ������ bool������ ��ȯ
//operation.progress; // ���������� float�� 0 ~ 1�� ��ȯ 0������,1����Ϸ�
// operation.allowSceneActivation; // true�� �ε��� �Ϸ�Ǹ� �ٷ� �����ѱ�� false�� progress�� 0.9f���� ����
public class SceneLoad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_LoadingText;
    private float m_DelayProgress;
    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        m_LoadingText.text = "0" + "%";
        yield return null;

        // �񵿱� �ε� ����
        AsyncOperation operation = SceneManager.LoadSceneAsync("Stage1"); // ��������1�� �񵿱������ �ҷ���
        operation.allowSceneActivation = false;
      
        // �ε� ��ȯ�� ������Ʈ ����
     while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // ���������������� �ε� ���� �ð��ɸ��� ����
            m_DelayProgress = Mathf.Lerp(m_DelayProgress, progress, Time.deltaTime * 0.5f); 

            int percentage = Mathf.RoundToInt(m_DelayProgress * 100);

            m_LoadingText.text = percentage.ToString() + "%";
            

            if (percentage >= 100)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
           

        }

    }
}
