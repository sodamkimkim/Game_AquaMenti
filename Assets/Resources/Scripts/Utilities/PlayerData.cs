using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataForSaveLoad
{
    /// <summary>
    /// : player 기본 상태 저장
    /// - 정보 '갱신'(x) 때가 아닌 게임 '종료'(o)할 때 저장할 데이터.
    /// - ex) 위치 등
    /// </summary>
    public class PlayerData : MonoBehaviour
    {
        private Transform playerSavingTransform_;
    } // end of class
} // end of namespace