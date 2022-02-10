using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Intance = null;
    [SerializeField] MapScript mapScript = null;
    [SerializeField] Text textLap = null;
    [SerializeField] int maxLap = 0;
    [HideInInspector] public int countLap = 0;
    [SerializeField] UILogMap uiLogMap = null;
    [SerializeField] UIDistance uiDistance = null;
    [SerializeField] Transform tPlayer = null;
    bool bEnd = false;
    [SerializeField] bool bDontCreate = false;
    [SerializeField] bool bTest = false;
    [SerializeField] int indexMap = 1;
    bool bTutorial = false;
    public int lateLevel = 0;

    void Awake()
    {
        LevelData.bNext = false;
        bTutorial = TutorialData.bTutorialStart;
        if (Intance == null)
            Intance = this;
        countLap = 1;
        //Create Map
        if (!bDontCreate)
        {
            CreateMap();
        }

        SetTextLap();
    }

    void CreateMap()
    {
        if (!bTest)
        {
            indexMap = LevelData.IDLevel + 1;
        }
        else
        {
        }

        string path = "";
        if (bTutorial)
        {
            path = "Maps/Tutorial";
            //			string s = "playLevel1_" + "Tutorial";
            //			FBManagerEvent.Instance.PostEventCustom (s);
        }
        else
        {
            path = "Maps/Map" + indexMap;
            //			string s = "playLevel1_" + indexMap;
            //			FBManagerEvent.Instance.PostEventCustom (s);
        }

        GameObject map = Instantiate(Resources.Load<GameObject>(path));
        map.transform.position = Vector2.zero;
        mapScript = map.GetComponent<MapScript>();
        maxLap = mapScript.iLap;
    }

    void Start()
    {
        mapScript.TakeLimimit();
        TakeDistance();
    }

    public void _Start()
    {
        uiLogMap.Show(countLap);
        SetTextLap();
        if (bTutorial)
        {
            string s = "playLevel1_" + "Tutorial";
            FBManagerEvent.Instance.PostEventCustom(s);
        }
        else
        {
            string s = "playLevel1_" + indexMap;
            lateLevel = indexMap;
            FBManagerEvent.Instance.PostEventCustom(s);
        }
    }

    public void NextLap()
    {
        if (bEnd)
            return;
        EndRace();
        countLap++;
        if (countLap > maxLap)
        {
            SetTextLapDone();
            //PlayerControl.Instance.EndRace ();
            TaskUtil.Schedule(this, _NextLap, 1F);
            bEnd = true;
            EnemyLogic.bStop = true;
            return;
        }

        if (countLap == maxLap)
        {
            uiLogMap.Show("Lap Point");
            AIEnemyManager.Instance.LapPoint();
        }
        else
        {
            uiLogMap.Show(countLap);
        }

        SetTextLap();
        TakeDistance();
    }

    void _NextLap()
    {
        int rankP = LeaderboardManager.Instance.GetRankPlayer();
        if (rankP == 0)
        {
            PlayerControl.Instance.EndRace(false);
        }
        else
        {
            UIManager.Instance.Lose();
        }
    }

    void SetTextLap()
    {
        textLap.text = "<color=orange>" + countLap + "</color>" + "/"
                       + "<color=lime>" + maxLap + "</color>";
    }

    void SetTextLapDone()
    {
        textLap.text = "<color=lime>" + maxLap + "</color>" + "/"
                       + "<color=lime>" + maxLap + "</color>";
    }

    #region Distance

    void TakeDistance()
    {
        bTakeDistance = true;
        timer = 0;
    }

    float timer = 0;
    bool bTakeDistance = false;

    void Update()
    {
        if (!bTakeDistance)
            return;
        timer += Time.deltaTime;
        if (timer >= GameConfig.durationUpdateDistance)
        {
            uiDistance.Change(mapScript.ratioDistance(tPlayer.position.x));
            timer = 0;
        }
    }

    void EndRace()
    {
        uiDistance.Change(1F);
        TakeDistance();
    }

    public float GetRatioDistance(float posX)
    {
        return mapScript.ratioDistance(posX);
    }

    #endregion


    #region GetX

    public float GetXStart
    {
        get { return mapScript.GetXStart; }
    }

    public float GetRangeX
    {
        get { return mapScript.GetRangeX; }
    }

    #endregion

    #region Win_Lose

    public void OnEnd(bool bWin)
    {
        if (bWin)
        {
            LevelData.bNext = true;
            string keyNext = LevelData.keyLevel + "" + (LevelData.IDLevel + 1);
            bool b = LevelData.GetUnlock(keyNext);
            if (!b)
                LevelData.SetUnlock(keyNext, true);
//            LogManager.LogLevel(LevelData.IDLevel, LevelDifficulty.Easy, UIManager.Instance.TimeGame, PassLevelStatus.Pass, "Level");
        }
        else
        {
            LevelData.bNext = false;
            //   LogManager.LogLevel(LevelData.IDLevel, LevelDifficulty.Easy, UIManager.Instance.TimeGame, PassLevelStatus.Fail, "Level");
        }
    }

    public void SetStar(int star)
    {
        string keyLevel = LevelData.keyLevel + "" + LevelData.IDLevel;
        int lateStar = LevelData.GetStar(keyLevel);
        if (star > lateStar)
        {
            LevelData.SetStar(keyLevel, star);
        }
    }

    #endregion

    public void DestroyMap()
    {
        mapScript.DestroyMap();
    }
}