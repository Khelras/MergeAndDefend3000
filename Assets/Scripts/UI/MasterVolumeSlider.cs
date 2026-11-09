using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text text;

    const string MasterVolumeKey = "MasterVolume";

    void Start()
    {
        float saved = PlayerPrefs.GetFloat(MasterVolumeKey, 100f);
        text.SetText($"{saved:0}");
        slider.minValue = 0f;
        slider.maxValue = 100f;
        slider.SetValueWithoutNotify(saved);

        AudioListener.volume = saved / 100f;
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        AudioListener.volume = value / 100f;
        PlayerPrefs.SetFloat(MasterVolumeKey, value);
        text.SetText($"{value:0}");
    }
}