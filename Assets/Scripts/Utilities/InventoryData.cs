using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataForSaveLoad
{
    /// <summary>
    ///  : player가 가진 모든 아이템 정보를 저장하는 클래스
    ///  - 현재 착용 중(NowWearing)인 아이템 정보도 포함되어 있음.
    ///  - save됨
    /// </summary>
    public class InventoryData : MonoBehaviour
    {
        public InGameAllItemInfo.EItemCategory itemCategory_;
        public string itemName_;
        public bool isUnLock_; // true: 사용 가능 false: 사용불가능(잠김)
        public bool isNowWearing_; // true: 장착 중, false: 장착 x
    } // end of class
} // end of namespace