using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace StuckInBetween.HUDElements {
    public class ManiteBar : VisualElement {
        public int LowValue { get; set; }
        public int HighValue { get; set; }
        public int Value { get; set; }
        public new class UxmlFactory : UxmlFactory<ManiteBar, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription _lowValue = new UxmlIntAttributeDescription { name = "Low Value", defaultValue = 0 };
            UxmlIntAttributeDescription _highValue = new UxmlIntAttributeDescription { name = "High Value", defaultValue = 100 };
            UxmlIntAttributeDescription _value = new UxmlIntAttributeDescription { name = "Value", defaultValue = 50 };
            
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ManiteBar;
                ate.LowValue = _lowValue.GetValueFromBag(bag, cc);
                ate.HighValue = _highValue.GetValueFromBag(bag, cc);
                ate.Value = _value.GetValueFromBag(bag, cc);

                ate.Clear();
                VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("UI Documents/ManiteBar");
                VisualElement maniteBar = vt.Instantiate();
                ate.Add(maniteBar);
            }
        }
    }
}
