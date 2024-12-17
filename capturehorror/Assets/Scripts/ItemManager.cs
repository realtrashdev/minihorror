using System.Collections;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] items;
    public bool switching;
    WeaponBobAndSway swayScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swayScript = GetComponent<WeaponBobAndSway>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!switching)
        {
            GetInput();
        }

        Lerp();
    }

    void GetInput()
    {
        for (int i = 0; i < items.Length; i++)
        {
            string keycodeName = "Alpha" + (i + 1).ToString();
            KeyCode code = (KeyCode)System.Enum.Parse(typeof(KeyCode), keycodeName, true);

            if (Input.GetKeyDown(code))
            {
                if (!items[i].activeSelf)
                {
                    StartCoroutine(SwitchItem(i));
                }
            }
        }
    }

    IEnumerator SwitchItem(int index)
    {
        //disable sway and reset rotation to prevent helicopter arms
        swayScript.enabled = false;
        transform.localEulerAngles = Vector3.zero;
        switching = true;

        yield return new WaitForSeconds(1);

        //turn off active item
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].activeSelf)
            {
                items[i].SetActive(false);
            }
        }

        //enable item
        items[index].SetActive(true);

        switching = false;
        swayScript.enabled = true;
    }

    void Lerp()
    {
        if (switching)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(60, 0, 0), 10 * Time.deltaTime);
        }
    }
}
