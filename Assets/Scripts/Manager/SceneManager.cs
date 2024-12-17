using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference Map;
    public CanvasGroup canvasGroup;
    public float fadeSpeed;
   
    public async void LoadRoom(object roomData)
    {
        var roomDataSO = (RoomDataSO)roomData;
        if(roomDataSO is RoomDataSO)
        {
            Debug.Log(roomDataSO.roomType);
        }
        currentScene=roomDataSO.sceneToLoad;
        //ж�ص�ǰ����
        await UnloadScene();
        //���س���
        await LoadScene();

    }
    private async Awaitable LoadScene()
    {
        
        StartCoroutine(StartFade(1));//��ʼ����
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);//���س�������ǰ������ж�ص�ǰ����
        await s.Task;//�ȴ��������
        //������ɺ�ʼ����
        StartCoroutine(StartFade(0));
        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);//�����
        }
    } 
    private async Awaitable UnloadScene()
    {  
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());//ж�ص�ǰ����
        
    }
    public async void LoadMap()
    {
       await UnloadScene();
        currentScene = Map;
        await LoadScene();
    }
    private IEnumerator StartFade(int amount)
    {
        yield return StartCoroutine(Fade(amount));
        
    }
    private IEnumerator Fade(int amount)
    {
        while (canvasGroup.alpha != amount)
        {
            switch (amount){
                case 1:
                    canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                    break;
                case 0:
                    canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                    break;
            }
            yield return null;
        }

    }
}
