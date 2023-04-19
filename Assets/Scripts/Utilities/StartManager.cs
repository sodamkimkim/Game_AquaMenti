using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    private UI_Manager uI_Manager_ = null;
    [SerializeField]
    private Button btnWorkStart_ = null;
    [SerializeField]
    private GameManager gameManager_= null;
    [SerializeField]
    private PlayerMovement playerMovement = null;
    [SerializeField]
    private GameObject[] spawnPoints_1_ = null;
    [SerializeField]
    private GameObject[] spawnPoints_2_ = null;

    private int selectedMapNum_ = 0;
    private int selectedSectionNum_ = 0;

    private void Awake()
    {
        btnWorkStart_.onClick.AddListener(StartWork);
    }
    private void StartWork()
    {
        if (!gameManager_.isStartGame_)
            gameManager_.ActiveInGameObjects();
        gameManager_.ActiveInGameUi();
        //Cursor.lockState = CursorLockMode.Confined;

        uI_Manager_.GetMapSectionNumber(out selectedMapNum_, out selectedSectionNum_);
        if (selectedMapNum_ == 1 && selectedSectionNum_ == 1)
        {
            playerMovement.SetPosition(GetSpwan_1Pos(), GetSpwan_1Rot());
        }
        else if (selectedMapNum_ == 1 && selectedSectionNum_ == 2)
        {
            playerMovement.SetPosition(GetSpwan_1Pos(), GetSpwan_1Rot());
        }
        else if (selectedMapNum_ == 1 && selectedSectionNum_ == 3)
        {
            playerMovement.SetPosition(GetSpwan_1Pos(), GetSpwan_1Rot());
        }
        else if (selectedMapNum_ == 2 && selectedSectionNum_ == 1)
        {
            playerMovement.SetPosition(GetSpwan_2Pos(), GetSpwan_2Rot());
        }
        uI_Manager_.GoToWorkDetailGo();

    }
    private Vector3 GetSpwan_1Pos()
    {
        return spawnPoints_1_[selectedSectionNum_ - 1].transform.position;
    }
    private Quaternion GetSpwan_1Rot()
    {
        return spawnPoints_1_[selectedSectionNum_ - 1].transform.rotation;
    }
    private Vector3 GetSpwan_2Pos()
    {
        return spawnPoints_2_[selectedSectionNum_ - 1].transform.position;
    }
    private Quaternion GetSpwan_2Rot()
    {
        return spawnPoints_2_[selectedSectionNum_ - 1].transform.rotation;
    }

} // end of class
