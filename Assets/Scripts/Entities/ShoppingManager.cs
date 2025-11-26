using UnityEngine;
using TMPro;
using Zenject;
using Data;
using Data.NPCShop;
using Entities.UI;

namespace Entities.NPC
{
    public class ShoppingManager : MonoBehaviour
    {
        [SerializeField] private NPCInfoForShop[] allNpcInfo;

        [SerializeField] private GameObject _uiItemPref;
        [SerializeField] private GameObject _friendshipLevelIconPref;
        [SerializeField] private GameObject _linePref;
        [SerializeField] private GameObject _curvePref;

        [SerializeField] private Transform _firstFriendshipLevelIcon;
        [SerializeField] private RectTransform _content;

        private Vector2 _levelsOffset;
        private Vector2 _itemsOffset;
        private float _parentSizeY;

        private UIStateSystem _uiStateSystem;
        private GlobalData _globalData;

        [Inject]
        private void Construct(UIStateSystem uiStateSystem, GlobalData globalData)
        {
            _uiStateSystem = uiStateSystem;
            _globalData = globalData;
            SetOffsets();
        }

        public void OpenNPCShop_int(int num) => OpenNPCShop((Characters)num);
        public void OpenNPCShop(Characters character)
        {
            ClearUiContent();

            NPCInfoForShop npcInfoForShop = allNpcInfo[(int)character];
            int levelCount = npcInfoForShop.LevelCount;
            Vector2 _firstLevelIconPos = _firstFriendshipLevelIcon.position;

            for (int i = 1; i <= levelCount; i ++)
            {
                if (i != levelCount) 
                {
                    GameObject gm = Instantiate(_friendshipLevelIconPref, _firstLevelIconPos + 
                        _levelsOffset * i, Quaternion.identity, _content);

                    gm.GetComponentInChildren<TMP_Text>().text = (i+1).ToString();
                }

                Vector2 linePos = _firstLevelIconPos + _levelsOffset * i;
                linePos.y -= _levelsOffset.y/2;
                Instantiate(_linePref, linePos, Quaternion.identity, _content);
            }
            foreach (ShopItem item in npcInfoForShop.ShopItemList)
            {
                Vector2 position;
                if (item.IsLast) 
                {
                    position = new(_firstLevelIconPos.x, _firstLevelIconPos.y + levelCount * _levelsOffset.y);
                    _content.offsetMax = new (_content.offsetMax.x, position.y - _parentSizeY);
                }
                else 
                {
                    position = _firstLevelIconPos + _itemsOffset + _levelsOffset * (item.NeedLevel-1);
                    Instantiate(_curvePref, position-(_itemsOffset/2), Quaternion.identity, _content);
                }

                Instantiate(_uiItemPref, position, Quaternion.identity, _content);
            }

            _uiStateSystem.SwitchCurrentStateAsync(UIState.Shop);
        }

        private void SetOffsets()
        {
            RectTransform lineR = _linePref.GetComponent<RectTransform>();
            RectTransform lvlR = _friendshipLevelIconPref.GetComponent<RectTransform>();
            RectTransform curveR = _curvePref.GetComponent<RectTransform>();
            RectTransform itemR = _friendshipLevelIconPref.GetComponent<RectTransform>();

            _levelsOffset = new (0, lineR.sizeDelta.y + lvlR.sizeDelta.y);
            _itemsOffset = new (-curveR.sizeDelta.x - lvlR.sizeDelta.x/2 - itemR.sizeDelta.x/2, 
                curveR.sizeDelta.y/2+curveR.sizeDelta.y/10);
            _parentSizeY = _content.parent.GetComponent<RectTransform>().sizeDelta.y;
        }

        private void ClearUiContent()
        {
            foreach (Transform child in _content) {
                if (child != _firstFriendshipLevelIcon) 
                    Destroy(child.gameObject);
            }
        }
    }
}