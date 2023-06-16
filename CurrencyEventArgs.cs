using System;

namespace Forge.Models.CurrencyManagement
{
    public class CurrencyEventArgs<T> : EventArgs
    {
        public T DeltaIncrease;
        public T DeltaDecrease;
    }
}