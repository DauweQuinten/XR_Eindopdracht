using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Android;

public class HighlightControllerParts : MonoBehaviour
{
    [SerializeField]private Material highlightMaterial = null;
    private Dictionary<ControllerPart, Material> defaultMaterialOfParts = new Dictionary<ControllerPart, Material>();
    private Dictionary<ControllerPart, string> nameOfParts = new Dictionary<ControllerPart, string>();
    private bool controllerMappingCompleted = false;

    public enum ControllerPart
    {
        Bumper,
        ButtonHome,
        ControllerBase,
        TouchPad,
        Trigger,
        ButtonA,
        ButtonB,
        ThumbStick,
        ThumbStickBase
    };



    private void Awake()
    {            
        foreach (ControllerPart part in System.Enum.GetValues(typeof(ControllerPart)))
        {
            try
            {
                GameObject partObject = gameObject.GetNamedChild(GetControllerPartString(part));
                Material material = partObject.GetComponent<MeshRenderer>().material;
                defaultMaterialOfParts.Add(part, material);
                Debug.Log($"Succesfully added {part} with material {material}");
            }
            catch
            {
                Debug.LogWarning($"Could not add {part} to the list of default materials. Did you use the right name in the GetControllerPartString function?");
            }           
        }
        Debug.Log("Material mapping done");
        controllerMappingCompleted = true;
    }



    private string GetControllerPartString(ControllerPart part)
    {
        switch (part)
        {
            case ControllerPart.Bumper:
                return "Bumper";
            case ControllerPart.ButtonHome:
                return "Button_Home";
            case ControllerPart.ControllerBase:
                return "Controller_Base";
            case ControllerPart.TouchPad:
                return "TouchPad";
            case ControllerPart.Trigger:
                return "Trigger";
            case ControllerPart.ButtonA:
                return "Button_A";
            case ControllerPart.ButtonB:
                return "Button_B";
            case ControllerPart.ThumbStick:
                return "ThumbStick";
            case ControllerPart.ThumbStickBase:
                return "ThumbStick_Base";
            default:
                return "Unknown";
        }
    }

    public void Highlight(ControllerPart part)
    {
        GameObject PartObject = gameObject.GetNamedChild(GetControllerPartString(part));
        if (PartObject == null) return;
        PartObject.GetComponent<MeshRenderer>().material = highlightMaterial;
        Debug.Log($"highlight: {part} of {gameObject.name}");
    }

    public void ResetHighLight(ControllerPart part)
    {
        GameObject PartObject = gameObject.GetNamedChild(GetControllerPartString(part));
        if (PartObject == null) return;
        PartObject.GetComponent<MeshRenderer>().material = defaultMaterialOfParts[part];
        Debug.Log($"reset highlight: {part} of {gameObject.name}");
    }

    public bool ControllerMappingCompleted()
    {
        return controllerMappingCompleted;
    }
}
