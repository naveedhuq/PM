using System;
using System.Windows;
using System.Windows.Threading;
using DevExpress.Xpf.Grid;


namespace PM.Shared
{
    public class BestFitHelper
    {
        public static readonly DependencyProperty DoBestFitProperty = DependencyProperty.RegisterAttached("DoBestFit", typeof(bool), typeof(BestFitHelper), new FrameworkPropertyMetadata(DoBestFitPropertyChanged));
        public static void SetDoBestFit(UIElement element, bool value)
        {
            element.SetValue(DoBestFitProperty, value);
        }
        public static bool GetDoBestFit(UIElement element)
        {
            return (bool)element.GetValue(DoBestFitProperty);
        }

        private static void DoBestFitPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TableView view = source as TableView;

            if ((bool)e.NewValue)
            {
                view.CellValueChanged += View_CellValueChanged;
                view.Dispatcher.BeginInvoke(new Action(() => view.BestFitColumns()), DispatcherPriority.Render);
            }
            else
            {
                view.CellValueChanged -= View_CellValueChanged;
            }
        }

        private static void View_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            TableView view = sender as TableView;
            view.Dispatcher.BeginInvoke(new Action(() => view.BestFitColumn(e.Column)), DispatcherPriority.Render);
        }
    }
}
