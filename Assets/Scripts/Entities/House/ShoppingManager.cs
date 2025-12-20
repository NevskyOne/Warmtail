using UnityEngine.UI;
using UnityEngine;
using Zenject;
using Data;
using Data.NPCShop;
using Entities.UI;
using Systems;

namespace Entities.NPC
{
    public class ShoppingManager : MonoBehaviour
    {
        [SerializeField] private NPCInfoForShop[] allNpcInfo;

        [SerializeField] private BuyButton[] _itemsButtons;
        [SerializeField] private Button[] _levelButtons;

        private UIStateSystem _uiStateSystem;
        private GlobalData _globalData;
        private NPCMethods _npcMethods;

        [Inject]
        private void Construct(UIStateSystem uiStateSystem, GlobalData globalData, NPCMethods npcMethods)
        {
            _uiStateSystem = uiStateSystem;
            _globalData = globalData;
            _npcMethods = npcMethods;
        }

        //TEST
        public void RaiseFriendship(int num) => _npcMethods.RaiseFriendship((Characters)num);


        public void OpenNPCShop(int num) => OpenNPCShop((Characters)num);
        public void OpenNPCShop(Characters character)
        {
            _npcMethods.CheckNpcData(character);

            NPCInfoForShop npcInfoForShop = allNpcInfo[(int)character];
            int levelCount = npcInfoForShop.LevelCount;
            int curLvl = _globalData.Get<NPCData>().Levels[character];
            
            for (int i = 0; i < levelCount; i ++)
            {
                ShopItem item = npcInfoForShop.ShopItemList[i];

                _itemsButtons[i].Initialize(item, character, (item.NeedLevel <= curLvl));
                if (i + 1 == levelCount)
                {
                    ShopItem nextItem = npcInfoForShop.ShopItemList[i+1];
                    _itemsButtons[i+1].Initialize(nextItem, character, 
                    ((nextItem.NeedLevel <= curLvl) && (!_globalData.Get<NPCData>().BoughtLastItem[character])));
                }

                _levelButtons[i].interactable = (i < curLvl);
            }
            if (_uiStateSystem.CurrentState != UIState.Shop) _uiStateSystem.SwitchCurrentStateAsync(UIState.Shop);
        }
    }
}