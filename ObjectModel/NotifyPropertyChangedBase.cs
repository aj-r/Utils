using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Utils.ObjectModel
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property.
        /// </summary>
        /// <param name="propertyExpression">A lambda expression that contains the property name, e.g. () => PropertyName</param>
        protected void RaisePropertyChanged(Expression<Func<object>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed. If this parameter is omitted, it is automatically set to the name of the calling property or method.
        /// </param>
        /// <remarks>
        /// You should rarely need specify the propertyName parameter explicitly when calling this method.
        /// If you want to raise the PropertyChanged event for a property from outside that property's setter,
        /// you should use the RaisePropertyChanged(Expression&lt;Func&lt;object&gt;&gt;) overload instead.
        /// </remarks>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field to the specified value. If the field is NOT already equal to the value
        /// (using the default equality comparer), then a PropertyChanged event will be raised.
        /// </summary>
        /// <typeparam name="T">The type of the field to set.</typeparam>
        /// <param name="field">The field to set.</param>
        /// <param name="value">The value to set the field to.</param>
        /// <param name="propertyName">
        /// The name of the property for which the PropertyChanged event will be raised.
        /// If this parameter is omitted, its value will be set to the name of the property or method where this method is called from.
        /// </param>
        /// <returns>True if the property was changed; otherwise false.</returns>
        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            return SetValue(ref field, value, null, propertyName);
        }

        /// <summary>
        /// Sets the field to the specified value. If the field is NOT already equal to the value
        /// (using the default equality comparer), then a PropertyChanged event will be raised.
        /// </summary>
        /// <typeparam name="T">The type of the field to set.</typeparam>
        /// <param name="field">The field to set.</param>
        /// <param name="value">The value to set the field to.</param>
        /// <param name="beforePropertyChanged">An action to run immediately before the PropertyChanged event is fired.</param>
        /// <param name="propertyName">
        /// The name of the property for which the PropertyChanged event will be raised.
        /// If this parameter is omitted, its value will be set to the name of the property or method where this method is called from.
        /// </param>
        /// <returns>True if the property was changed; otherwise false.</returns>
        protected bool SetValue<T>(ref T field, T value, Action beforePropertyChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            if (beforePropertyChanged != null)
                beforePropertyChanged();
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            return true;
        }

        /// <summary>
        /// Gets the name of a property in a lambda expression.
        /// </summary>
        /// <param name="propertyExpression">A lambda expression in the form: () => SomeProperty</param>
        /// <returns>A string that represents the property name.</returns>
        protected string GetPropertyName(Expression<Func<object>> propertyExpression)
        {
            MemberExpression expr;
            if (propertyExpression.Body is UnaryExpression)
                expr = (MemberExpression)((UnaryExpression)propertyExpression.Body).Operand;
            else if (propertyExpression.Body is MemberExpression)
                expr = (MemberExpression)propertyExpression.Body;
            else
                throw new ArgumentException("propertyExpression must be in the form '() => SomeProperty'", "propertyExpression");
            return expr.Member.Name;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
