using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    // ReSharper disable once InconsistentNaming
    public class UIDependences : MonoBehaviour
    {
        [Header("Top")]
        public RectTransform TopPanel;
        public Slider HpSlider;

        
        [Space]
        [Header("BottomLeft")]
        public RectTransform BottomLeftPanel;
        
        
        [Space]
        [Header("BottomRight")]
        public RectTransform BottomRightPanel;
        
        

    }
}