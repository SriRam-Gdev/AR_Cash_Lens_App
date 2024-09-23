using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyDataContainer", menuName = "Currency Data Container",order = 1)]
public class currency_scriptable :ScriptableObject
{
    public List<CurrencyData> currencyData;

    [System.Serializable]
    public class CurrencyData
    {
        [SerializeField]
        public string m_CurrencyValue;
        public Binding m_Binding;
    }
    [System.Serializable]
    public class Binding
    {
        public string m_CurrencyName;
        public string m_CurrencyType;
    }
}
