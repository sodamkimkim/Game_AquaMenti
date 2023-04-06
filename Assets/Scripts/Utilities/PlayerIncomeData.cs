using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataForSaveLoad
{
    /// <summary>
    /// : player가 게임을 통해 벌어들인 Income 정보 저장 (Total Game Money, 별점 등)
    /// - static
    /// </summary>
    public static class PlayerIncomeData
    {
        private static float totalIncome_; // player가 게임에서 얻은 총 수입
        private static float totalStar_; // player가 게임에서 얻은 총 별점

    } // end of class
} // end of namespace