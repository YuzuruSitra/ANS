using System;
using UnityEngine;
using Item;
using UI;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [Serializable]
        public struct ItemPrefabSet
        {
            public ItemKind _kind;
            public GameObject _prefab;
            public GameObject _predict;
            public Sprite _sprite;
            public int _count;
        }
        [SerializeField] private ItemPrefabSet[] _itemSets;
        [SerializeField] private ItemUIHandler _itemUIHandler;
        private const int ErrorValue = -1;
        private int _currentItemNum = ErrorValue;
        private Action<Sprite> _onItemNumChanged;
        private Action<int> _onItemCountChanged;
        public GameObject CurrentPredict => _currentItemNum == ErrorValue ? null : _itemSets[_currentItemNum]._predict;

        private void Start()
        {
            _itemUIHandler.SetInventoryFrame(_itemSets);
            for (var i = 0; i < _itemSets.Length; i++)
            {
                if ( _itemSets[i]._predict == null) continue;
                _itemSets[i]._predict = Instantiate(_itemSets[i]._predict);
            }
            
            _onItemNumChanged += _itemUIHandler.ChangeItemImage;
            _onItemCountChanged += _itemUIHandler.ChangeItemCount;
        }

        private void OnDestroy()
        {
            _onItemNumChanged -= _itemUIHandler.ChangeItemImage;
            _onItemCountChanged -= _itemUIHandler.ChangeItemCount;
        }

        public void IncreaseItemNum()
        {
            var startIndex = Math.Max(_currentItemNum, 0);

            for (var i = 1; i <= _itemSets.Length; i++)
            {
                var newIndex = (startIndex + i) % _itemSets.Length;

                if (_itemSets[newIndex]._count <= 0) continue;
                ChangeItemNum(newIndex);
                return;
            }

            ChangeItemNum(ErrorValue);
        }

        public void DecreaseItemNum()
        {
            var startIndex = _currentItemNum == ErrorValue ? _itemSets.Length - 1 : _currentItemNum;

            for (var i = 1; i <= _itemSets.Length; i++)
            {
                var newIndex = (startIndex - i + _itemSets.Length) % _itemSets.Length;

                if (_itemSets[newIndex]._count <= 0) continue;
                ChangeItemNum(newIndex);
                return;
            }

            ChangeItemNum(ErrorValue);
        }

        private void ChangeItemNum(int value)
        {
            if (_currentItemNum != ErrorValue) ChangePredictActive(_itemSets[_currentItemNum]._predict, false);
            _currentItemNum = value;
            var sprite = _currentItemNum != ErrorValue ? _itemSets[value]._sprite : null;
            _onItemNumChanged?.Invoke(sprite);
        }

        // アイテムをインベントリに追加する
        public void AddItem(ItemKind item)
        {
            Debug.Log("Add Inventory : " + item);
            for (var i = 0; i < _itemSets.Length; i++)
            {
                if (_itemSets[i]._kind != item) continue;
                _itemSets[i]._count++;
                if (_itemSets[i]._count == 1) _itemUIHandler.ChangeVisibleFrame(_itemSets);
                _onItemCountChanged?.Invoke(_itemSets[i]._count);
                if (_currentItemNum == ErrorValue) ChangeItemNum(i);
                break;
            }
        }

        public GameObject UseItem()
        {
            if (_currentItemNum == ErrorValue) return null;
            var outItem = _itemSets[_currentItemNum]._prefab;
            _itemSets[_currentItemNum]._count = Math.Max(0, _itemSets[_currentItemNum]._count - 1);
            if (_itemSets[_currentItemNum]._count <= 0) _itemUIHandler.ChangeVisibleFrame(_itemSets);
            _onItemCountChanged?.Invoke(_itemSets[_currentItemNum]._count);
            if (_itemSets[_currentItemNum]._count <= 0) ChangeItemNum(ErrorValue);
            return outItem;
        }

        public void ChangePredictActive(GameObject predict, bool isVisible)
        {
            if (predict == null) return;
            predict.SetActive(isVisible);
        }
        
    }
}

// リファクタリング予定