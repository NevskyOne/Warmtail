using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Zenject;
using Systems;
using Data.House;

namespace Data.NPCShop
{
    public class BuyButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _block;
        [SerializeField] private GameObject _boughtLast;
        [SerializeField] private TMP_Text _priceTmp;
        [SerializeField] private string _currencyString;

        private ShopItem _shopItem;
        private Characters _character;
        private bool _unlocked;
        [Inject] private ShoppingSystem _shoppingSystem;
        [Inject] private GlobalData _globalData;

        public void Initialize(ShopItem shopItem, Characters character, bool unlocked)
        {
            _shopItem = shopItem;
            _unlocked = unlocked;
            _character = character;
            SetPreferencesButton();
        }
        private void SetPreferencesButton()
        {
            _icon.sprite = _shopItem.Item.Sprite;
            _priceTmp.text = _shopItem.Item.Price.ToString() + _currencyString;

            bool isLastAndBought = (_shopItem.IsLast && _globalData.Get<NPCData>().BoughtLastItem[_character]);
            _block.SetActive(!_unlocked);
            _boughtLast.SetActive(_unlocked && isLastAndBought);
            _button.interactable = (_unlocked && !isLastAndBought);
        }
        public void ClickBuy()
        {
            _shoppingSystem.BuyItem(_shopItem.Item, _character, _shopItem.IsLast);
            SetPreferencesButton();
        }
}
}
