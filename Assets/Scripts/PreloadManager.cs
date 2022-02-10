using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreloadManager : MonoBehaviour
{
    [SerializeField] Image imgFill;
    float fMinProcess = 0;
    bool bCheck = false;

    void Awake()
    {
        if (!Logic.bWarmup)
        {
            Shader.WarmupAllShaders();
            AllInOne.Instance.Init();
            GifManager.Instance.Init();
            Logic.bWarmup = true;
            Logic.bShowAds = false;
        }
        imgFill.fillAmount = 0;
        //		if (GameData.LateVersion == "") {
        //			GameData.LateVersion = Application.version;
        //			if (!GameData.SyncData) {
        //
        //
        //				Debug.Log ("tdz version moi nay");
        //
        //				GameData.SyncData = true;
        //			}
        //		} else {
        //			if (!Application.version.Equals (GameData.LateVersion)) {
        //				if (!GameData.SyncData) {
        //
        //					Debug.Log ("Sync data version moi nay");
        //
        //					GameData.SyncData = true;
        //				}
        //			} else {
        //			}
        //		}
        AllInOne.Instance.Init();
    }

    void Start()
    {
        if (Logic.isPause)
            Logic.UNPAUSE();
        fMinProcess = Random.Range(0.2F, 0.6F);
        //Manager.Instance.HideWaiting ();
        float time = Logic.bShowAds ? 0.1F : 0.8F;
        TaskUtil.Schedule(this, _Start, time);
        StartCoroutine(FakeProcess());
    }

    IEnumerator FakeProcess()
    {
        bool done = false;
        float timer = 0;
        while (!done)
        {
            timer += Time.unscaledDeltaTime;
            float ratio = timer / 0.8F;
            ratio *= fMinProcess;
            imgFill.fillAmount = ratio;
            if (timer >= 0.8F)
                done = true;
            yield return null;
        }
    }

    void _Start()
    {
        if (CacheScene.indexScene == 100)
        {
            if (!TutorialData.bTutorialStart)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            if (!Logic.bShowAds)
            {
                bCheck = true;
                LoadLevels(CacheScene.indexScene, true);
            }
            else
            {
                Logic.bShowAds = false;
                bCheck = false;
                AllInOne.Instance.ShowAdmobFULL(LoadLevelNow, Logic.sWhere, LevelData.IDLevel);
            }
        }
    }

    void LoadLevelNow(bool b)
    {
        LoadLevels(CacheScene.indexScene, true);
    }

    public void LoadLevels(int indexLevel, bool bLoadNow)
    {
        StartCoroutine(LoadRoutine(indexLevel, bLoadNow));
    }

    private string loadProgress = "Loading...";
    private string lastLoadProgress = null;

    private IEnumerator LoadRoutine(int indexLevel, bool bLoadNow)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(indexLevel, LoadSceneMode.Single);
        op.allowSceneActivation = bLoadNow;
        bCheck = bLoadNow;
        while (!op.isDone)
        {
            if (op.progress < 0.9f)
            {
                loadProgress = "Loading: " + (op.progress * 100f).ToString("F0") + "%";
                Debug.Log(loadProgress);
            }
            else
            { // if progress >= 0.9f the scene is loaded and is ready to activate.
                if (bCheck)
                {
                    op.allowSceneActivation = true;
                }
                //loadProgress = "Loading ready for activation, Press any key to continue";
            }
            if (lastLoadProgress != loadProgress)
            {
                lastLoadProgress = loadProgress;
                Debug.Log(loadProgress);
            } // Don't spam console.
            if (op.progress >= fMinProcess)
                imgFill.fillAmount = op.progress + 0.1F;

            yield return null;
        }
        loadProgress = "Load complete.";
        Debug.Log(loadProgress);
    }
}
