using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsingToolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject toolTipUIGo_ = null;
    private TextMeshProUGUI toolTipTMPro_ = null;
    private WandRaySpawner wandRaySpawner_;
    private Ladder ladder_;
    private bool isLadderMoving_ { get; set; }

    private void Awake()
    {
        wandRaySpawner_ = GameObject.FindWithTag("Player").GetComponentInChildren<WandRaySpawner>();
        ladder_ = GameObject.FindWithTag("Ladder").GetComponent<Ladder>();
        toolTipTMPro_ = toolTipUIGo_.GetComponentInChildren<TextMeshProUGUI>();
        isLadderMoving_ = false;
    }
    private void Update()
    {
        Vector3 rayHitPos_ = wandRaySpawner_.hitPos_;
        if (wandRaySpawner_.isLadder_)
        {
            toolTipTMPro_.text = "사다리 사용을 원하시면 키보드 F를 누르세요.";
            toolTipUIGo_.SetActive(true);

            if (Input.GetKey(KeyCode.F))
            {
                ladder_.isMoveable_ = true;
                toolTipUIGo_.SetActive(false);
            }
        }
        else { toolTipUIGo_.SetActive(false); }

        if (ladder_.isMoveable_)
        {
            toolTipUIGo_.SetActive(true);
            toolTipTMPro_.text = "사다리 위치를 고정하려면 마우스 왼쪽 버튼을 누르세요.";
            ladder_.SetLadderPos(new Vector3(rayHitPos_.x, ladder_.GetPos().y, rayHitPos_.z));
            // ray 와 floor가 충돌 && MouseButtonDown(0) => floor에 사다리 놓을 수 있음.
            if (Input.GetMouseButtonDown(0))
            {
                ladder_.isMoveable_ = false;
                toolTipTMPro_.text = "";
                toolTipUIGo_.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ladder_.RotateLadderLeft();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                ladder_.RotateLadderRight();
            }
        }


    }
} // end of class
