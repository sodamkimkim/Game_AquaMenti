using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject[] map1 = null;
    [SerializeField] private GameObject[] map2 = null;

    private List<MeshPaintTarget>[] map1MeshPaintTargets_ = new List<MeshPaintTarget>[5];
    private List<MeshPaintTarget>[] map2MeshPaintTargets_ = new List<MeshPaintTarget>[3];

    private int selectedMapNum_ = 0;
    private int selectedSectionNum_ = 0;

    private void Awake()
    {
        btnWorkStart_.onClick.AddListener(StartWork);
    }
    private void Start()
    {
        for (int i = 0; i < map1.Length; i++)
        {
            map1MeshPaintTargets_[i] = map1[i].GetComponentsInChildren<MeshPaintTarget>().ToList();
        }
        for (int i = 0; i < map2.Length; i++)
        {
            map2MeshPaintTargets_[i] = map2[i].GetComponentsInChildren<MeshPaintTarget>().ToList();
        }
    }
    private void StartWork()
    {
        if (!gameManager_.isStartGame_)
            gameManager_.ActiveInGameObjects();
        gameManager_.ActiveInGameUi();
        Cursor.lockState = CursorLockMode.Confined;
        uI_Manager_.GetMapSectionNumber(out selectedMapNum_, out selectedSectionNum_);
        if (selectedMapNum_ == 1 && selectedSectionNum_ == 1)
        {
            SetSectionDraw(selectedMapNum_, selectedSectionNum_);
            playerMovement.SetPosition(GetSpwan_1Pos(), GetSpwan_1Rot());
        }
        else if (selectedMapNum_ == 1 && selectedSectionNum_ == 2)
        {
            SetSectionDraw(selectedMapNum_, selectedSectionNum_);
            playerMovement.SetPosition(GetSpwan_1Pos(), GetSpwan_1Rot());
        }
        else if (selectedMapNum_ == 1 && selectedSectionNum_ == 3)
        {
            SetSectionDraw(selectedMapNum_, selectedSectionNum_);
            playerMovement.SetPosition(GetSpwan_1Pos(), GetSpwan_1Rot());
        }
        else if (selectedMapNum_ == 1 && selectedSectionNum_ == 4)
        {
            SetSectionDraw(selectedMapNum_, selectedSectionNum_);
            playerMovement.SetPosition(GetSpwan_1Pos(), GetSpwan_1Rot());
        }
        else if (selectedMapNum_ == 1 && selectedSectionNum_ == 5)
        {
            SetSectionDraw(selectedMapNum_, selectedSectionNum_);
            playerMovement.SetPosition(GetSpwan_1Pos(), GetSpwan_1Rot());
        }
        else if (selectedMapNum_ == 2 && selectedSectionNum_ == 1)
        {
            SetSectionDraw(selectedMapNum_, selectedSectionNum_);
            playerMovement.SetPosition(GetSpwan_2Pos(), GetSpwan_2Rot());
        }
        else if (selectedMapNum_ == 2 && selectedSectionNum_ == 2)
        {
            SetSectionDraw(selectedMapNum_, selectedSectionNum_);
            playerMovement.SetPosition(GetSpwan_2Pos(), GetSpwan_2Rot());
        }
        else if (selectedMapNum_ == 2 && selectedSectionNum_ == 3)
        {
            SetSectionDraw(selectedMapNum_, selectedSectionNum_);
            playerMovement.SetPosition(GetSpwan_2Pos(), GetSpwan_2Rot());
        }
        //uI_Manager_.GoToWorkDetailGo();

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
    private void SetSectionDraw(int _mapNum, int __sectionNum)
    {
        if (_mapNum == 1)
        {
            for (int i = 0; i < map1.Length; i++)
            {
                if (__sectionNum - 1 == i)
                    foreach (MeshPaintTarget meshPaintTarget in map1MeshPaintTargets_[i])
                        meshPaintTarget.IsDrawable(true);
                else
                    foreach (MeshPaintTarget meshPaintTarget in map1MeshPaintTargets_[i])
                        meshPaintTarget.IsDrawable(false);
            }
            for (int i = 0; i < map2.Length; i++)
            {
                foreach (MeshPaintTarget meshPaintTarget in map2MeshPaintTargets_[i])
                    meshPaintTarget.IsDrawable(false);
                foreach (MeshPaintTarget meshPaintTarget in map2MeshPaintTargets_[i])
                    meshPaintTarget.IsDrawable(false);
            }
        }
        else
        {
            for (int i = 0; i < map2.Length; i++)
            {
                if (__sectionNum - 1 == i)
                    foreach (MeshPaintTarget meshPaintTarget in map2MeshPaintTargets_[i])
                        meshPaintTarget.IsDrawable(true);
                else
                    foreach (MeshPaintTarget meshPaintTarget in map2MeshPaintTargets_[i])
                        meshPaintTarget.IsDrawable(false);
            }
            for (int i = 0; i < map1.Length; i++)
            {
                foreach (MeshPaintTarget meshPaintTarget in map1MeshPaintTargets_[i])
                    meshPaintTarget.IsDrawable(false);
                foreach (MeshPaintTarget meshPaintTarget in map1MeshPaintTargets_[i])
                    meshPaintTarget.IsDrawable(false);
            }
        }

    }


} // end of class
