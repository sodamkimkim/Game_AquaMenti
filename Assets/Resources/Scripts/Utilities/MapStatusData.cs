using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataForSaveLoad
{
    /// <summary>
    /// : 맵 별 WorkSection, Parts 에 대한 획득 Income, 작업 상태 등 저장하는 클래스
    ///  - static
    ///  - 여러개의 맵이 하나의 List를 공용한다. (Swap해가며 사용)
    /// </summary>
    public static class MapStatusData
    {
        private static List<WorkSectionStatusData> workstationList_ = new List<WorkSectionStatusData>();
    } // end of class
    public class WorkSectionStatusData
    {
        // # worksection별 Income정보
        private float currentSectionIncome_;
        private float totalSectionIncome_;

        // # worksection별 별점정보
        private float currentSectionStar_;
        private float totalSectionStar_;

        // # 소속된 part정보
        private List<PartStatusData> partList_ = new List<PartStatusData>();
    } // end of class
    public struct PartStatusData
    {
        private string workSectionBelong_; // part가 소속된 worksection
      //  private string partCategory_; // ex) 옹벽윗돌 , 옹벽 -> 이건 상태 연산 & ui에서 보여줄 때 처리
        private string partName_; // ex) 옹벽윗돌1, 옹벽윗돌2, 옹벽
    } // end of structure

} // end of namespace