using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Slider))]
public class ShapeSlider : MonoBehaviour{
    
    [Header("Do not include the suffixes of the BlendShape Name")]
    public string blendShapeName;
    private Slider slider;
    // Start is called before the first frame update
    private void Start(){
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(value => HeadCustomization.Instance.ChangeBlendShapeValue(blendShapeName, value));
    }


}
