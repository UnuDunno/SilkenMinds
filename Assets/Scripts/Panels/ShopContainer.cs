using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopContainer : MonoBehaviour
{
    [SerializeField] private GameObject cardContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Button buyCardButton;

    public void PlaceCardOnShop()
    {
        Instantiate(cardPrefab, cardContainer.transform);
    }

    public void AddListenerBuyButton(UnityAction action)
    {
        buyCardButton.onClick.RemoveAllListeners();
        buyCardButton.onClick.AddListener(action);
    }
}
