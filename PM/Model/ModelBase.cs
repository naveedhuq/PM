using DevExpress.Mvvm;
using System;
using System.ComponentModel;
using System.Linq.Expressions;


namespace PM.Model
{
    public abstract class ModelBase<T> : INotifyPropertyChanged
    {
        public bool IsDirty { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged = null;
        public virtual void NotifyPropertyChanged<TResult>(Expression<Func<T, TResult>> property)
        {
            string propertyName = ((MemberExpression)property.Body).Member.Name;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                IsDirty = true;

                var typeName = typeof(T).FullName;
                Messenger.Default.Send(
                    message: propertyName,
                    token: string.Format("{0}:{1}>>PropertyChanged", typeName, propertyName));
            }
        }

        public abstract void SaveChanges();
        protected abstract void Populate(T item);
    }
}
