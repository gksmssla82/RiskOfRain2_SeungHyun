using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//operation.isDone; // 작업의 완료 유무를 bool형으로 반환
//operation.progress; // 진행정도를 float형 0 ~ 1로 반환 0진행중,1진행완료
// operation.allowSceneActivation; // true면 로딩이 완료되면 바로 신을넘기고 false면 progress가 0.9f에서 멈춤
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

        // 비동기 로딩 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync("Stage1"); // 스테이지1을 비동기식으로 불러옴
        operation.allowSceneActivation = false;
      
        // 로딩 전환률 업데이트 루프
     while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // 포토폴리오용으로 로딩 조금 시간걸리게 만듬
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
