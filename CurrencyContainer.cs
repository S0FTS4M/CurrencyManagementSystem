using System;
using System.Collections.Generic;
using Forge.Models.Operators;

namespace Forge.Models.CurrencyManagement
{
    public interface ICurrencyContainer<T>
    {
        ICurrencyBase<T> CurrentCurrency { get; }

        event Action<T> CurrencyChanged;

        void Add(ICurrencyBase<T> currency);
        void NotifyIfCurrencyChanged();
        void SetCurrentCurrency(int index);
        void SetCurrentCurrency(ICurrencyBase<T> currency);
    }

    public class CurrencyContainer<T, O> : ICurrencyContainer<T> where O : IOperator<T>, new()
    {
        #region Variables
        private List<ICurrencyBase<T>> _currencies;

        #endregion

        #region Props
        public ICurrencyBase<T> CurrentCurrency { get; private set;}

        #endregion

        #region Events

        public event Action<T> CurrencyChanged;

        #endregion

        #region Constructor

        public CurrencyContainer()
        {
            _currencies = new List<ICurrencyBase<T>>();
        }

        #endregion

        #region Methods

        public void SetCurrentCurrency(int index)
        {
            SetCurrentCurrency(_currencies[index]);
        }

        public void SetCurrentCurrency(ICurrencyBase<T> currency)
        {
            if (CurrentCurrency != null)
                CurrentCurrency.CurrencyChanged -= OnCurrencyChanged;

            CurrentCurrency = currency;
            CurrentCurrency.CurrencyChanged += OnCurrencyChanged;

            OnCurrencyChanged(null, null);
        }

        /// <summary>
        /// It adds currency to currencies and also it assigns currency
        /// to currentCurrency
        /// </summary>
        /// <param name="currency">Added currency</param>
        public void Add(ICurrencyBase<T> currency)
        {
            _currencies.Add(currency);
        }

        private void OnCurrencyChanged(object sender, EventArgs cArgs)
        {
            CurrencyChanged?.Invoke(CurrentCurrency.Amount);
        }

        public void NotifyIfCurrencyChanged()
        {
            CurrentCurrency?.NotifyIfCurrencyChanged();
        }

        #endregion
    }
}