using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorkAreaManager : MonoBehaviour
{
    [SerializeField] private GameDataManager gameDataManager_ = null;
    [SerializeField] private UI_Manager uI_Manager_ = null;
    [SerializeField] private UI_SectionDetailManager uI_SectionDetailManager_ = null;

    [SerializeField] private GameObject workSectionContentGo_ = null;
    [SerializeField] private GameObject workSectionPrefab_ = null;

    private RectTransform scrollView_WorkSectionsRT_ = null;
    private List<Dictionary<string, object>> workSectionDataList_ = null;
    private float contentDefaultHeight_ = 0f;

    private void Awake()
    {
        scrollView_WorkSectionsRT_ = workSectionContentGo_.GetComponent<RectTransform>();
        contentDefaultHeight_ = scrollView_WorkSectionsRT_.rect.height;
    }

    public void SetSectionContent(int _selectedMapNum)
    {
        Button[] btns = workSectionContentGo_.GetComponentsInChildren<Button>();
        if (btns != null)
        {
            foreach (Button btn in btns)
            {
                Destroy(btn.gameObject);
            }
            
        }
        gameDataManager_.GetMapDataList(out workSectionDataList_);
        Vector3 workSectionPrefabPos = workSectionContentGo_.transform.position;
        workSectionPrefabPos.x += 224f;
        workSectionPrefabPos.y -= 20f;
        int cnt = 1;
        workSectionContentGo_.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, contentDefaultHeight_);
        foreach (Dictionary<string, object> workSectionDics in workSectionDataList_)
        {
            if (workSectionDics["MapNumber"].ToString() == $"{_selectedMapNum}")
            {
                if (cnt > 2) scrollView_WorkSectionsRT_.sizeDelta = new Vector2(0f, scrollView_WorkSectionsRT_.rect.height + 220f);

                GameObject go = Instantiate(workSectionPrefab_, workSectionPrefabPos, Quaternion.identity, workSectionContentGo_.transform);
                go.GetComponent<UI_ButtonWorkSection>().SetUI_Managers(uI_SectionDetailManager_, uI_Manager_);
                go.name = $"Button_WorkSection_{workSectionDics["MapNumber"].ToString()}_{workSectionDics["SectionNumber"].ToString()}";
                go.GetComponentsInChildren<TextMeshProUGUI>()[0].text = workSectionDics["SectionTitle"].ToString();
                workSectionPrefabPos.y -= 260f;
                ++cnt;

            }


        }
    }
}
