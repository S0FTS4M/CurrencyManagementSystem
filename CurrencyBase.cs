using System;
using Forge.Configs;
using Forge.Models.Operators;
using Newtonsoft.Json;
using Forge.Helpers.Interfaces;
using UnityEngine;

namespace Forge.Models.CurrencyManagement
{
    public interface ICurrencyBase<T> : IInitializable
    {
        T Amount { get; }
        string Name { get; }

        event EventHandler<CurrencyEventArgs<T>> CurrencyChanged;

        bool CanAfford(T amount);
        void SetAmount(T amount);
        void Increase(IIncreaser<T> increaser);
        void Decrease(IDecreaser<T> decreaser);
        void Increase(T increaseAmount);
        void Decrease(T decreaseAmount);
        void NotifyIfCurrencyChanged();
        void ResetToDefaults();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CurrencyBase<T, Op> : ICurrencyBase<T> where Op : IOperator<T>, new()
    {
        #region Variables

        private bool _isCurrencyChanged;

        private CurrencyConfig<T> _currencyConfig;

        private T _deltaIncrease;

        private T _deltaDecrease;

        #endregion

        #region Props

        [JsonProperty]
        public T Amount { get; private set; }

        public string Name { get; private set; }

        private Op op = new Op();

        #endregion

        #region Events

        public event EventHandler<CurrencyEventArgs<T>> CurrencyChanged;

        #endregion

        #region Methods

        public virtual void Initialize(ScriptableObject so, bool isLoaded)
        {
            this._currencyConfig = (CurrencyConfig<T>)so;

            if (isLoaded == false)
            {
                Amount = _currencyConfig.Amount;
            }

            Name = _currencyConfig.CurrencyName;
        }

        public void SetAmount(T amount)
        {
            Amount = amount;

            _isCurrencyChanged = true;
        }

        public void Increase(IIncreaser<T> increaser)
        {
            Amount = op.Add(Amount, increaser.IncreaseAmount);

            _isCurrencyChanged = true;
            _deltaIncrease = op.Add(_deltaIncrease, increaser.IncreaseAmount);
        }

        public void Decrease(IDecreaser<T> decreaser)
        {
            if (CanAfford(decreaser.DecreaseAmount) == false) return;

            Amount = op.Subtract(Amount, decreaser.DecreaseAmount);

            _isCurrencyChanged = true;
            _deltaDecrease = op.Add(_deltaDecrease, decreaser.DecreaseAmount);
        }

        public void Increase(T increaseAmount)
        {
            Amount = op.Add(Amount, increaseAmount);

            _isCurrencyChanged = true;
            _deltaIncrease = op.Add(_deltaIncrease, increaseAmount);
        }

        public void Decrease(T decreaseAmount)
        {
            if (CanAfford(decreaseAmount) == false) return;

            Amount = op.Subtract(Amount, decreaseAmount);

            _isCurrencyChanged = true;
            _deltaDecrease = op.Add(_deltaDecrease, decreaseAmount);
        }

        public bool CanAfford(T amount)
        {
            return op.LessThanEqual(amount, Amount);
        }

        public void NotifyIfCurrencyChanged()
        {
            if (_isCurrencyChanged)
            {
                var eventArgs = new CurrencyEventArgs<T>
                {
                    DeltaIncrease = _deltaIncrease,
                    DeltaDecrease = _deltaDecrease
                };

                CurrencyChanged?.Invoke(this, eventArgs);
                
                _isCurrencyChanged = false;
                _deltaIncrease = op.Cast(0.0);
                _deltaDecrease = op.Cast(0.0);
            }
        }

        public void ResetToDefaults()
        {
            Amount = _currencyConfig.Amount;
        }

        #endregion
    }
}
