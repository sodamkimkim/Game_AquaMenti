using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    private Button btnWorkStart_ = null;
    [SerializeField]
    private GameObject outGameObjectsGo_= null;
    [SerializeField]
    private GameObject inGameObjectsGo_ = null;

    private void Awake()
    {
        btnWorkStart_.onClick.AddListener(StartWork);
    }
    public void StartWork()
    {
        outGameObjectsGo_.SetActive(false);
        inGameObjectsGo_.SetActive(true);
    }
} // end of class
