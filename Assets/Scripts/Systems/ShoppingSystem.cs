using UnityEngine;
using Zenject;
using Data.House;
using Data.Player;
using Data.NPCShop;
using Data;

namespace Systems
{
    public class ShoppingSystem
    {
        [Inject] private GlobalData _globalData;
        
        public void BuyItem(HouseItemData item, Characters character, bool isLast)
        {
            int shells = _globalData.Get<SavablePlayerData>().Shells;
            if (shells < item.Price) Debug.Log("не хватает ракушек!");
            else
            {
                _globalData.Edit<SavablePlayerData>(data => {data.Shells -= item.Price;});
                Debug.Log("ракушек теперь " + _globalData.Get<SavablePlayerData>().Shells);
                if (isLast) PurchaseLastItem (character);
            }
        }
        private void PurchaseLastItem(Characters character)
        {
            _globalData.Edit<NPCData>(data => {data.BoughtLastItem[character] = true;});
        }
    }
}
