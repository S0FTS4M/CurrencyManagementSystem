using Forge.Helpers.Interfaces;
using UnityEngine;

namespace Forge.Configs
{
    public class CurrencyConfig<T> : ScriptableObject
    {
        public IParent Parent;
        
        public string CurrencyName;

        public T Amount;
    }
}