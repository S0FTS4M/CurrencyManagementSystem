using Forge.Configs;
using Forge.Helpers;
using Forge.Models.Operators;
using UnityEngine;

namespace Forge.Models.CurrencyManagement
{
    public class MainCurrencyBase : CurrencyBase<double, DoubleOperator>
    {
        #region Methods

        public override void Initialize(ScriptableObject currencyConfig, bool isLoaded)
        {
            base.Initialize(currencyConfig, isLoaded);

            var gameBase = Verfices.Request<IGameBase>();
            gameBase.MainCurrencyContainer.Add(this);
        }

        #endregion
    }
}