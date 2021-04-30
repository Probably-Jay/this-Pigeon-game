using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Scott Jarvis, 22/04/21
public class VolumeHandler : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource AMB;
    public AudioSource SFX;

    public Slider BGM_Slider;
    public Slider AMB_Slider;
    public Slider SFX_Slider;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        BGM.volume = BGM_Slider.value;
        AMB.volume = AMB_Slider.value;
        SFX.volume = SFX_Slider.value;
    }
}
