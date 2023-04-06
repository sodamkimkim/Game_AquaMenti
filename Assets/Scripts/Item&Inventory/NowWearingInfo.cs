using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NowWearingInfo : MonoBehaviour
{
    /// <summary>
    /// 현재 착용 중인 아이템 정보 기록에 사용할 structure
    /// </summary>
    public struct NowWearingItem
    { // # staff : [0], # spell : [1]
        // item정보
        public string itemCategory_;
        public string itemName_;
        public string itemImg_; // Asset에 저장된 image파일 이름
        public string itemDescription_;
        public NowWearingItem(string _itemCategory, string _itemName, string _itemImg, string _itemDescription)
        {
            this.itemCategory_ = _itemCategory;
            this.itemName_ = _itemName;
            this.itemImg_ = _itemImg;
            this.itemDescription_ = _itemDescription;
        } // end of construct
        public override string ToString()
        {
            return $"itemCategory_ : {itemCategory_}, itemName_ : {itemName_}, itemImg_ : {itemName_}, itemDescription_ : {itemDescription_}";
        }
    } // end of struct
    /// <summary>
    ///  : 현재 착용중인 아이템 정보 저장 arr
    ///  - [0] : Staff, [1] : Spell
    /// </summary>
    private NowWearingItem[] nowWearingArr_ = new NowWearingItem[2];

    private void Awake()
    {
        // # 기본 Staff와 기본 Spell로 NowWearing정보 초기화
        // TODO
        //nowWearingArr_[0] = 
        //nowWearingArr_[1] = 
    }
    /// <summary>
    /// : 현재 착용중인 Staff정보 저장
    /// </summary>
    /// <param name="_selectItem"></param>
    public void SetNowWearingStaff(NowWearingItem _selectItem)
    {
        nowWearingArr_[0] = _selectItem;
        Debug.Log(nowWearingArr_[0].ToString());
        Debug.Log(nowWearingArr_[1].ToString());
        // TODO
        // UI업데이트 함수 만들어서 값 넣어주기
    }

    /// <summary>
    /// : 현재 착용중인 Spell정보 저장
    /// </summary>
    /// <param name="_selectItem"></param>
    public void SetNowWearingSpell(NowWearingItem _selectItem)
    {
        nowWearingArr_[1] = _selectItem;
        // TODO
        // UI업데이트 함수 만들어서 값 넣어주기
        Debug.Log(nowWearingArr_[0].ToString());
        Debug.Log(nowWearingArr_[1].ToString());
    }
} // end of class
