using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    Slider slider;
    float maxValue;
    float minValue;
    float value;

    private void Start()
    {
        slider = GetComponent<Slider>();
        value = slider.value;
        maxValue = slider.maxValue;
        minValue = slider.minValue;
    }

    public void UpdateValue(int num)
    {
        value = num;

        if (value < minValue)
        {
            value = minValue;
        }

        else if (value > maxValue)
        {
            value = maxValue;
        }

        slider.value = value;
    }
}
