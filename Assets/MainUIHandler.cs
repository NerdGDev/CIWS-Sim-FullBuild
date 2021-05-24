using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIHandler : MonoBehaviour
{
    Canvas canvas;

    public Text InfoData;
    public Text QueueData;

    public Slider SimSpeedSlider;

    public Button LaunchOne;
    public Button LaunchFour;
    public Button LaunchTwe;
    public Button LaunchTweFour;

    public GameObject IconObject;

    Visualise visual;


    Dictionary<GameObject, GameObject> IconManager = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        Physics.queriesHitTriggers = true;
        canvas = GetComponent<Canvas>();
        SimSpeedSlider.onValueChanged.AddListener(delegate { UpdateSimSpeed(); });
        LaunchOne.onClick.AddListener(delegate { FindObjectOfType<GameManager>().LaunchNewMissile(1); });
        LaunchFour.onClick.AddListener(delegate { FindObjectOfType<GameManager>().LaunchNewMissile(4); });
        LaunchTwe.onClick.AddListener(delegate { FindObjectOfType<GameManager>().LaunchNewMissile(12); });
        LaunchTweFour.onClick.AddListener(delegate { FindObjectOfType<GameManager>().LaunchNewMissile(24); });
    }

    public void UpdateSimSpeed()
    {
        Time.timeScale = SimSpeedSlider.value;
    }

    private void Update()
    {
        if (visual != null)
        {
            SetData();
        }
        else
        {
            InfoData.text = "No Target";
            QueueData.text = "";
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.GetComponent<Visualise>())
                {
                    visual = objectHit.GetComponent<Visualise>();
                }

                // Do something with the object that was hit by the raycast.
            }
        }

    }

    private void FixedUpdate()
    {
        UpdateIcons();
    }

    public void UpdateIcons()
    {
        foreach (CommandController cc in FindObjectsOfType<CommandController>())
        {
            if (IconManager.ContainsKey(cc.gameObject))
            {
                UpdateIcon(IconManager[cc.gameObject], cc.gameObject);
            }
            else
            {
                GameObject go = Instantiate(IconObject, canvas.transform);
                go.GetComponentInChildren<Text>().text = "Command Center";
                IconManager.Add(cc.gameObject, go);
                UpdateIcon(IconManager[cc.gameObject], cc.gameObject);
            }
        }

        foreach (FireControlSystem cc in FindObjectsOfType<FireControlSystem>())
        {
            if (IconManager.ContainsKey(cc.gameObject))
            {
                UpdateIcon(IconManager[cc.gameObject], cc.gameObject);
            }
            else
            {
                GameObject go = Instantiate(IconObject, canvas.transform);
                go.GetComponentInChildren<Text>().text = "CIWS Weapon";
                IconManager.Add(cc.gameObject, go);
                UpdateIcon(IconManager[cc.gameObject], cc.gameObject);
            }
        }

        foreach (SearchRadarController cc in FindObjectsOfType<SearchRadarController>())
        {
            if (IconManager.ContainsKey(cc.gameObject))
            {
                UpdateIcon(IconManager[cc.gameObject], cc.gameObject);
            }
            else
            {
                GameObject go = Instantiate(IconObject, canvas.transform);
                go.GetComponentInChildren<Text>().text = "Search Radar";
                IconManager.Add(cc.gameObject, go);
                UpdateIcon(IconManager[cc.gameObject], cc.gameObject);
            }
        }

        foreach (Missile cc in FindObjectsOfType<Missile>())
        {
            if (IconManager.ContainsKey(cc.gameObject))
            {
                UpdateIcon(IconManager[cc.gameObject], cc.gameObject);
            }
            else
            {
                GameObject go = Instantiate(IconObject, canvas.transform);
                go.GetComponentInChildren<Text>().text = "Missile";
                go.GetComponentInChildren<Text>().color = Color.red;
                IconManager.Add(cc.gameObject, go);
                UpdateIcon(IconManager[cc.gameObject], cc.gameObject);
                StartCoroutine(MissileIconHandle(cc.gameObject, go));
            }
        }
        List<GameObject> DeadList = new List<GameObject>();
        foreach (GameObject item in IconManager.Keys)
        {
            Debug.LogError(item);
            if (item == null)
            {
                DeadList.Add(item);
            }
        }

        foreach (GameObject deadItem in DeadList)
        {
            IconManager.Remove(deadItem);
        }



    }

    IEnumerator MissileIconHandle(GameObject go, GameObject icon) 
    {
        yield return new WaitForFixedUpdate();
        while (true) 
        {
            Debug.LogWarning(go);
            if(go == null) 
            {
                if (IconManager.ContainsKey(go)) 
                {
                    IconManager.Remove(go);
                    
                }
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        Destroy(icon);
    }

    void UpdateIcon(GameObject icon, GameObject target) 
    {
        icon.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(target.transform.position);
    }

    public void SetData()
    {
        Dictionary<string, string> dataFields;
        List<string> shortData;
        visual.GetDataFields(out dataFields, out shortData);
        InfoData.text = FormatListData(dataFields);
        QueueData.text = FormatListData(shortData);

    }

    string FormatListData(Dictionary<string, string> dataDict)
    {
        string data = "";
        foreach (var item in dataDict)
        {
            data += item.Key;
            data += " : ";
            data += item.Value;
            data += "\n";
        }
        return data;
    }
    string FormatListData(List<string> dataDict)
    {
        string data = "";
        foreach (var item in dataDict)
        {
            data += item;
            data += "\n";
        }
        return data;
    }
}

