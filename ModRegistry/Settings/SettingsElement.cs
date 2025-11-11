using System;
using System.Collections.Generic;
using System.Text;
using VMM.ModRegistry.Settings;

namespace VMM.ModRegistry.Settings
{
    public abstract class SettingsElement<T> : ISettingsElement
    {
        public string Name { get; set; }
        public T Value { get; set; }

        public Action<T> OnChanged { get; set; }

        public object GetValue() => Value;

        public void SetValue(object value)
        {
            if (value is T typedValue)
            {
                Value = typedValue;
                OnChanged?.Invoke(typedValue);
            }
            else
            {
                throw new InvalidCastException($"Invalid value type. Expected {typeof(T)}, got {value?.GetType()}");
            }
        }

    }
}