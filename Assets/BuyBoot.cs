using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyBoot : MonoBehaviour
{
    [SerializeField] Text textCount = null;
    [SerializeField] BootType bootType = BootType.None;
    [SerializeField] ButtonBuyBoot btnCoin = null, btnVideo = null;
    [SerializeField] Text textPrice = null, textCountVideo = null;
    int countBoot = 0;
    int countVideo = 0;
    int price = 300;
    void OnEnable()
    {
        countBoot = CharacterData.GetBoot(bootType);
        UpdateText();
        btnCoin.Register(Reqest, TypePurchase.Coin);
        btnVideo.Register(Reqest, TypePurchase.Video);
        countVideo = CharacterData.GetCountVideoBoot(bootType.ToString());
        if (countVideo == 0)
        {
            price = CoinManager.Instance.priceBoot;
            countVideo = price / CoinManager.Instance.coinPerVideo;
        }
        else
        {
            price = CoinManager.Instance.coinPerVideo * countVideo;
        }
        textCountVideo.text = "" + countVideo;
        UpdatePrice();
        btnVideo.Block(!AllInOne.Instance.CheckVideoReward());
    }

    void UpdatePrice()
    {
        textPrice.text = "" + price;
    }

    void UpdateCountVideo()
    {
        textCountVideo.text = "" + countVideo;
        CharacterData.SetCountVideoBoot(bootType.ToString(), countVideo);
    }

    void UpdateText()
    {
        textCount.text = "" + countBoot;
        CharacterData.SetBoot(bootType, countBoot);
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
        if (CoinManager.Instance.CheckCoin(price))
        {
            CoinManager.Instance.PurchaseCoin(-price);
            ActiveBoot();
        }
    }

    void ShowVideo()
    {
        FBManagerEvent.Instance.PostEventCustom("videoBuyBot" + bootType.ToString());
        btnVideo.Block(true);
        btnCoin.Block(true);
        AllInOne.Instance.ShowVideoReward(CallbackVideo, "Buy_" + bootType.ToString(), LevelData.IDLevel);
    }

    void CallbackVideo(bool b)
    {
        if (b)
        {
            countVideo--;
            countVideo = Mathf.Clamp(countVideo, 0, 20);
            UpdateCountVideo();
            if (countVideo == 0)
            {
                ActiveBoot();
            }
            else
            {
                btnVideo.Block(false);
                btnCoin.Block(false);
            }
            price = CoinManager.Instance.coinPerVideo * countVideo;
            UpdatePrice();
        }
        else
        {
            btnVideo.Block(!AllInOne.Instance.CheckVideoReward());
        }
    }

    void ActiveBoot()
    {
        FBManagerEvent.Instance.PostEventCustom("BuyBot" + bootType.ToString());
        countBoot++;
        UpdateText();
        TaskUtil.Schedule(this, this.ResetBuy, 0.5F);
    }

    void ResetBuy()
    {
        price = CoinManager.Instance.priceBoot;
        countVideo = price / CoinManager.Instance.coinPerVideo;
        UpdateCountVideo();
        UpdatePrice();
        btnCoin.Block(false);
        btnVideo.Block(!AllInOne.Instance.CheckVideoReward());
    }
}
