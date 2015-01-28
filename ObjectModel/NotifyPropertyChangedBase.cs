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

        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
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
