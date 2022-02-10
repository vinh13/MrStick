using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public enum MapID
{
    None = 0,
    Map1 = 1,
    Map2 = 2,
    Map3 = 3,
    Map4 = 4,
    Map5 = 5,
}

public class LevelEditor : MonoBehaviour
{
    [SerializeField] ILevel[] btnsLevel;
    [SerializeField] Transform parent = null;
    [SerializeField] GameObject linePref = null;
    [SerializeField] LineLevel[] linesLevel;
    [SerializeField] Transform parentLevel = null;
    [SerializeField] MapID mapID = MapID.None;
    [SerializeField] DragControl dragControl = null;
    [SerializeField] UIComingSoon uiComingSoon = null;
    string keyLevel = "";
    int iLevelMax = 0;
    bool bTutorial = false;
    [SerializeField] int maxLevel = 9;
    [SerializeField] TotalStar totalStar = null;
    [SerializeField] UIUnlockReport uiReport = null;
    [SerializeField] LevelGifManager levelGifData = null;
    int levelCheckRate = 0;
    int[] levelCheck = {
        5,
        10,
        15,
        20,
        25,
        30,
        35,
        40,
        45,
        50,
        55,
        60,
        65,
        70,
        75,
        80,
        85,
        90, 95, 100,105,110,115,120,125,130,135,140,145,150

    };
    int[] starCheck = {
        7,
        21,
        35,
        47,
        65,
        79,
        92,
        105,
        117,
        130,
        142,
        155, 168, 181, 194, 209, 222,
        237, 252,
        267,267,292,307,327,332,347,362,377,387,447

    };
    int currentStar = 0;

    void OnValidate()
    {
        btnsLevel = parent.GetComponentsInChildren<ILevel>();
    }

    void Awake()
    {
        keyLevel = mapID.ToString() + "_level";
        //Test
        //		for (int i = 0; i < btnsLevel.Length; i++) {
        //			LevelData.SetUnlock (keyLevel + "" + i, true);
        //		}
        LevelData.SetUnlock(keyLevel + "" + 0, true);
        bTutorial = TutorialData.bLevelTutorial;
        levelGifData.Register(maxLevel * 3);
        levelCheckRate = LevelData.LevelCheckRate;
        if (levelCheckRate == 0)
        {
            levelCheckRate = 3;
            LevelData.LevelCheckRate = levelCheckRate;
        }

    }

    void Start()
    {
        SyncDataLevel();
        CreateLine();
        Invoke("_Start", 0.1F);
    }

    void _Start()
    {
        if (iLevelMax >= 9)
        {
            dragControl.NextPage(btnsLevel[iLevelMax].transform.localPosition.x);
        }
    }

    void SyncStar(int cur)
    {
        int total = maxLevel * 3;
        float ratio = ((float)(cur) / (float)total);
        string s1 = "" + cur;
        string s2 = "" + total;
        currentStar = cur;
        totalStar.UpdateStar(ratio, s1, s2, true);
        levelGifData.SyncGif(currentStar);
    }

    void SyncDataLevel()
    {
        int stars = 0;
        for (int i = 0; i < btnsLevel.Length; i++)
        {
            if (btnsLevel[i].Setup(i, keyLevel + "" + i, ClickLevel))
            {
                stars += btnsLevel[i].GetStar();
                iLevelMax = i;
            }
        }
        btnsLevel[iLevelMax].Select();
        SyncStar(stars);
    }

    public void CreateLine()
    {
        linesLevel = new LineLevel[btnsLevel.Length - 1];
        for (int i = 0; i < linesLevel.Length; i++)
        {
            linesLevel[i] = lineLevel;
            linesLevel[i].transform.position = Vector2.zero;
            Vector3 s = btnsLevel[i].transform.position;
            Vector3 e = btnsLevel[i + 1].transform.position;
            s.z = 0;
            e.z = 0;
            linesLevel[i].Setup(s, e);
            linesLevel[i].Active(btnsLevel[i + 1].Unlocked());
        }
    }

    LineLevel lineLevel
    {
        get
        {
            return Instantiate(linePref, parentLevel).GetComponent<LineLevel>();
        }
    }

    #region Control

    void CreateRateUS()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("UI/UIRateUS"), parent.root);
        go.GetComponent<UIRateUS>().Show();
    }

    void ClickLevel(int ID)
    {
        if (ID < maxLevel)
        {
            //Check Unlock
#if UNITY_ANDROID
            if (ID == levelCheckRate)
            {
                levelCheckRate += 2;
                LevelData.LevelCheckRate = levelCheckRate;
                if (!GameData.RateUSNow)
                {
                    CreateRateUS();
                    return;
                }
            }
#endif
            for (int i = 0; i < levelCheck.Length; i++)
            {
                if (ID == levelCheck[i] - 1)
                {
                    if (currentStar < starCheck[i])
                    {
                        uiReport.ShowUnlock(starCheck[i] - currentStar, ShowTarget);
                        CheckTarget();
                        return;
                    }
                }
            }
            AIDataLevel aiDataLevel = AIDataLevel.None;
            int idCheck = (ID + 1);
            if (idCheck <= 5)
            {
                aiDataLevel = AIDataLevel.None;
            }
            else if (idCheck <= 10)
            {
                aiDataLevel = AIDataLevel.Easy;
            }
            else if (idCheck <= 15)
            {
                aiDataLevel = AIDataLevel.Normal;
            }
            else if (idCheck <= 20)
            {
                aiDataLevel = AIDataLevel.Hard;
            }
            else if (idCheck <= 25)
            {
                aiDataLevel = AIDataLevel.Hardcore;
            }


            LevelData.aiDataLevel = aiDataLevel;

            if (bTutorial)
            {
                MenuTutorial.Instance.HideTutorial(TutorialID.Level);
                bTutorial = false;
                TutorialData.bLevelTutorial = false;
            }
            MapID indexMap = MapID.None;
            if (ID < 9)
            {
                indexMap = MapID.Map1;
            }
            else if (ID < 19)
            {
                indexMap = MapID.Map2;
            }
            else if (ID < 29)
            {
                indexMap = MapID.Map3;
            }
            else if (ID < 39)
            {
                indexMap = MapID.Map4;
            }
            else if (ID < 50)
            {
                indexMap = MapID.Map5;
            }
            else
            {
                indexMap = MapID.Map1;
            }
            LevelData.keyLevel = keyLevel;
            LevelData.IDLevel = ID;
            LevelData.mapID = indexMap;
            BeforeplayManager.Instance.Show();
        }
        else
        {
            uiComingSoon.Show();
        }
    }

    public void ShowTarget()
    {
        dragControl.NextPage(btnsLevel[targetIDMax].transform.localPosition.x);
    }

    int targetIDMax = 0;

    public void CheckTarget()
    {
        bool bDone = false;
        for (int i = 0; i < iLevelMax; i++)
        {
            if (btnsLevel[i].GetStar() < 3)
            {
                btnsLevel[i].ShowTarget();
                if (!bDone)
                {
                    targetIDMax = i;
                    bDone = true;
                }
            }
        }
    }

    #endregion

}
