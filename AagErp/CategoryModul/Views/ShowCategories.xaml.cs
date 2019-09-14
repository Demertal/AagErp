using System.Windows;
using System.Windows.Controls;
using ModelModul.Models;

namespace CategoryModul.Views
{
    /// <summary>
    /// Логика взаимодействия для ShowCategories.xaml
    /// </summary>
    public partial class ShowCategories : UserControl
    {
        public Category SelectedCategory
        {
            get => (Category)GetValue(SelectedCategoryProperty);
            set => SetValue(SelectedCategoryProperty, value);
        }

        public static readonly DependencyProperty SelectedCategoryProperty;

        public static readonly RoutedEvent SelectedCategoryChangedEvent;

        public event RoutedPropertyChangedEventHandler<Category> SelectedCategoryChanged
        {
            add => AddHandler(SelectedCategoryChangedEvent, value);
            remove => RemoveHandler(SelectedCategoryChangedEvent, value);
        }

        private static void SelectedCategoryChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ShowCategories ctl = (ShowCategories)d;
            Category newValue = (Category)e.NewValue;
            Category oldValue = (Category)e.OldValue;
            if (newValue == null)
            {
                ctl.Etv.SelectedItemTv.IsSelected = false;
                ctl.Etv.SelectedItemTv = null;
                ctl.Etv.SelectedItem = null;
            }
            ctl.OnSelectedCategoryChanged(
                new RoutedPropertyChangedEventArgs<Category>(oldValue, newValue, SelectedCategoryChangedEvent));
        }

        protected virtual void OnSelectedCategoryChanged(RoutedPropertyChangedEventArgs<Category> e)
        {
            RaiseEvent(e);
        }

        static ShowCategories()
        {
            SelectedCategoryProperty = DependencyProperty.Register("SelectedCategory", typeof(Category),
                typeof(ShowCategories), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedCategoryChangedCallback));
            SelectedCategoryChangedEvent = EventManager.RegisterRoutedEvent("SelectedCategoryChanged",
                RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Category>), typeof(ShowCategories));
        }

        public ShowCategories()
        {
            InitializeComponent();
        }
    }
}
