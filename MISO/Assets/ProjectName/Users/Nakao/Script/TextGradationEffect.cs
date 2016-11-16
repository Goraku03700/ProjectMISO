using UnityEngine;
using System.Collections;

namespace UnityEngine.UI 
{ 
    [AddComponentMenu("UI/Effects/Custom/Gradient", 14)] 
    public class TextGradationEffect : BaseMeshEffect 
    { 
        [SerializeField] 
        [Range(-1.5f, 1.5f)] 
        public float Offset = 0f; 
 
        [SerializeField] 
        public Color32 TopColor = Color.white; 
 
        [SerializeField] 
        public Color32 BottomColor = Color.black;


        protected TextGradationEffect() 
        { } 
 
        //------------------------------------------------------------- 
        //! エフェクト計算. 
        //------------------------------------------------------------- 
        private void ModifyVertices(VertexHelper vh) 
        { 
            UIVertex v = new UIVertex(); 
 
            float fRate = 0.0f; 
            int flipIndex = 0; 
            for (int i = 0; i < vh.currentVertCount; i++) 
            { 
                vh.PopulateUIVertex(ref v, i); 
 
                if (flipIndex == 0) 
                { 
                    fRate = 1.0f; 
                }
                else if (flipIndex == 3)
                {
                    fRate = 1.0f;
                }
                else
                {
                    fRate = 0.0f;
                } 
                if (++flipIndex >= 4) 
                { 
                    flipIndex = 0; 
                } 
                v.color = Color32.Lerp( 
                    BottomColor, 
                    TopColor, 
                    fRate + Offset); 
                vh.SetUIVertex(v, i); 
 
            } 
        } 
 
        //------------------------------------------------------------- 
        //! エフェクト適用. 
        //------------------------------------------------------------- 
        public override void ModifyMesh(VertexHelper vh) 
        { 
            if (!IsActive()) 
                return; 
 
           ModifyVertices(vh); 
        } 
    } 
}
