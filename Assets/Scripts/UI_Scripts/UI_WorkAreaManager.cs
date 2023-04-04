using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorkAreaManager : MonoBehaviour
{
    [SerializeField] private GameDataManager gameDataManager = null;
    [SerializeField] private UI_Manager uI_Manager = null;
    [SerializeField] private UI_SectionDetailManager uI_SectionDetailManager = null;

    [SerializeField] private GameObject workSectionContentGo = null;
    [SerializeField] private GameObject workSectionPrefab = null;

    [SerializeField] private RectTransform scrollView_WorkSectionsRT = null;
    private List<Dictionary<string, object>> workSectionDataList = null;
    float contentDefaultHeight = 0f;

    private void Start()
    {
        contentDefaultHeight = workSectionContentGo.GetComponent<RectTransform>().rect.height;
    }
    public void SetSectionContent(int _selectedMapNum)
    {
        Button[] btns = workSectionContentGo.GetComponentsInChildren<Button>();
        if (btns != null)
        {
            foreach (Button btn in btns)
            {
                Destroy(btn.gameObject);
            }
            
        }
        gameDataManager.GetMapDataList(out workSectionDataList);
        Vector3 workSectionPrefabPos = workSectionContentGo.transform.position;
        workSectionPrefabPos.x += 224f;
        workSectionPrefabPos.y -= 20f;
        int cnt = 1;
        workSectionContentGo.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, contentDefaultHeight);
        foreach (Dictionary<string, object> workSectionDics in workSectionDataList)
        {
            if (workSectionDics["\"MapNumber\""].ToString() == $"{_selectedMapNum}")
            {
                if (cnt > 2) scrollView_WorkSectionsRT.sizeDelta = new Vector2(0f, scrollView_WorkSectionsRT.rect.height + 220f);

                GameObject go = Instantiate(workSectionPrefab, workSectionPrefabPos, Quaternion.identity, workSectionContentGo.transform);
                go.GetComponent<UI_ButtonWorkSection>().SetUI_Managers(uI_SectionDetailManager, uI_Manager);
                go.name = $"Button_WorkSection_{workSectionDics["\"MapNumber\""].ToString()}_{workSectionDics["\"SectionNumber\""].ToString()}";
                go.GetComponentsInChildren<TextMeshProUGUI>()[0].text = workSectionDics["\"SectionTitle\""].ToString();
                workSectionPrefabPos.y -= 260f;
                ++cnt;

            }


        }
    }
}
