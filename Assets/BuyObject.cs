using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class BuyObject : MonoBehaviour
{
    [SerializeField] Image imgPreview = null;
    [SerializeField] ObjectType currentObjectType = ObjectType.None;
    [SerializeField] Transform[] rects = new Transform[2];
    [SerializeField] ButtonBuyBoot[] btns = new ButtonBuyBoot[2];
    [SerializeField] Text textPrice = null, textCountVideo = null;
    [SerializeField] ObjectType[] listRandomObject;
    [SerializeField] int[] indexImgs;
    int countVideo = 0;
    int price = 200;
    int lateRandom = 0;

    int _GetRandom()
    {
        int index = Random.Range(1, listRandomObject.Length + 1) - 1;
        if (lateRandom == index)
        {
            index = _GetRandom();
        }
        else
        {
            lateRandom = index;
        }
        return index;
    }

    void OnEnable()
    {
        string ob = CharacterData.ObjectStart;
        if (ob == "")
        {
            currentObjectType = ObjectType.None;
        }
        else
        {
            ObjectType type = ObjectType.None;
            foreach (var e in Enum.GetValues(typeof(ObjectType)))
            {
                if (e.ToString() == ob)
                {
                    type = (ObjectType)e;
                    break;
                }
            }
            currentObjectType = type;
        }
        if (currentObjectType == ObjectType.None)
        {
            //Start Build
            ShowBuy(true);
            btns[0].Register(Reqest, TypePurchase.Coin);
            btns[1].Register(Reqest, TypePurchase.Video);
            countVideo = CharacterData.GetCountVideoBoot("buyWeapon");
            if (countVideo == 0)
            {
                price = 1000;
                countVideo = price / CoinManager.Instance.coinPerVideo;
            }
            else
            {
                price = CoinManager.Instance.coinPerVideo * countVideo;
            }
            textCountVideo.text = "" + countVideo;
            UpdatePrice();
            int tempId = _GetRandom();
            currentObjectType = listRandomObject[tempId];
            imgPreview.sprite = Resources.Load<Sprite>("Image/wp/" + indexImgs[tempId] + "a");
        }
        else
        {
            //
            ShowBuy(false);
            int tempId = GetIDSp(currentObjectType);
            imgPreview.sprite = Resources.Load<Sprite>("Image/wp/" + indexImgs[tempId] + "a");
        }
        btns[1].Block(!AllInOne.Instance.CheckVideoReward());
    }

    int GetIDSp(ObjectType _type)
    {
        int index = 0;
        for (int i = 0; i < listRandomObject.Length; i++)
        {
            if (_type == listRandomObject[i])
            {
                index = i;
                break;
            }
        }
        return index;
    }

    void UpdateCountVideo()
    {
        textCountVideo.text = "" + countVideo;
        CharacterData.SetCountVideoBoot("buyWeapon", countVideo);
    }

    void UpdatePrice()
    {
        textPrice.text = "" + price;
    }

    void ShowBuy(bool b)
    {
        rects[0].gameObject.SetActive(b);
        rects[1].gameObject.SetActive(!b);
    }

    public void Reqest(TypePurchase t)
    {
        switch (t)
        {
            case TypePurchase.Coin:
                Buy();
                break;
            case TypePurchase.Video:
                ShowVideo();
                break;
        }
    }

    void Buy()
    {
        FBManagerEvent.Instance.PostEventCustom("BuyOject");
        if (CoinManager.Instance.CheckCoin(price))
        {
            CoinManager.Instance.PurchaseCoin(-price);
            ActiveObject();
        }
    }

    void ShowVideo()
    {
        btns[1].Block(true);
        AllInOne.Instance.ShowVideoReward(CallbackVideo, "BuyWP", LevelData.IDLevel);
        FBManagerEvent.Instance.PostEventCustom("video_buyWP");
    }

    void CallbackVideo(bool b)
    {
        if (b)
        {
            countVideo--;
            UpdateCountVideo();
            countVideo = Mathf.Clamp(countVideo, 0, 20);
            if (countVideo == 0)
            {
                btns[1].Block(true);
                ActiveObject();
            }
            else
            {
                btns[1].Block(false);
                price = CoinManager.Instance.coinPerVideo * countVideo;
                UpdatePrice();
            }
        }
        else
        {
            btns[1].Block(!AllInOne.Instance.CheckVideoReward());
        }
    }

    void ActiveObject()
    {
        CharacterData.ObjectStart = currentObjectType.ToString();
        ShowBuy(false);
    }

}
