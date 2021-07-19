using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeadCustomization : Singleton<HeadCustomization>
{
    public GameObject target;
    public string suffixMax = "Max", suffixMin = "Min";

    private HeadCustomization() {}

    private SkinnedMeshRenderer smr;
    private Mesh mesh;

    private Dictionary<string, BlendShape> blendShapeData = new Dictionary<string, BlendShape>();

    private void Start() {
        Initialize();
    }
    #region Public Func
    
    public void ChangeBlendShapeValue(string blendShapeName, float value){
       
        if(!blendShapeData.ContainsKey(blendShapeName)) {Debug.LogError(blendShapeName +" doesn't exist"); return;}
    
        value = Mathf.Clamp(value, -100, 100);

        BlendShape blendShapeKey = blendShapeData[blendShapeName];

        if(value >= 0){
            if(blendShapeKey.positiveIndex == -1) return;
            smr.SetBlendShapeWeight(blendShapeKey.positiveIndex, value);
            
            if(blendShapeKey.negativeIndex == -1) return;
            smr.SetBlendShapeWeight(blendShapeKey.negativeIndex, 0);
        }

        else{
             if(blendShapeKey.positiveIndex == -1) return;
            smr.SetBlendShapeWeight(blendShapeKey.positiveIndex, 0);
            
            if(blendShapeKey.negativeIndex == -1) return;
            smr.SetBlendShapeWeight(blendShapeKey.negativeIndex, -value);

        }
    
    }

    #endregion

    #region Private Func
    private void Initialize(){
        smr = GetSkinendMeshRenderer();
        mesh = smr.sharedMesh;

        ParseBlendShapeToData();
    }
    private SkinnedMeshRenderer GetSkinendMeshRenderer(){
        SkinnedMeshRenderer _smr = target.GetComponentInChildren<SkinnedMeshRenderer>();
        return _smr;
    }

    private void ParseBlendShapeToData(){
        List<string> blendShapeNames = Enumerable.Range(0, mesh.blendShapeCount).Select(x => mesh.GetBlendShapeName(x)).ToList();
    
        for (int i = 0; blendShapeNames.Count > 0;)
        {
            string altSuffix, noSuffix;
            noSuffix = blendShapeNames[i].TrimEnd(suffixMax.ToCharArray()).TrimEnd(suffixMin.ToCharArray()).Trim();
        
            string positiveName = string.Empty, negativeName = string.Empty;
            bool exist = false;

            int positiveIndex = -1, negativeIndex = -1;

            if (blendShapeNames[i].EndsWith(suffixMax))
            {
                altSuffix = noSuffix + " " + suffixMin;

                positiveName = blendShapeNames[i];
                negativeName = altSuffix;

                if (blendShapeNames.Contains(altSuffix))
                {
                    exist = true;
                } 

                positiveIndex = mesh.GetBlendShapeIndex(positiveName);

                if (exist) negativeIndex = mesh.GetBlendShapeIndex(altSuffix);
             
                
            }

            else if (blendShapeNames[i].EndsWith(suffixMin))
            {
                 altSuffix = noSuffix + " " + suffixMax;

                negativeName = blendShapeNames[i];
                positiveName = altSuffix;

                if (blendShapeNames.Contains(altSuffix))
                {
                    exist = true;
                } 

                negativeIndex = mesh.GetBlendShapeIndex(negativeName);

                if (exist) positiveIndex = mesh.GetBlendShapeIndex(altSuffix);
            }

            else positiveIndex = mesh.GetBlendShapeIndex(blendShapeNames[i]);

            blendShapeData.Add(noSuffix, new BlendShape(positiveIndex, negativeIndex));

            //remove 'i' data from list

            if (positiveName != string.Empty) blendShapeNames.Remove(positiveName);
            if (negativeName != string.Empty) blendShapeNames.Remove(negativeName);
        }// loop end
    }// end data gather(useless now, but for later if this shit works)
    
    #endregion
}
