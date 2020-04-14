using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace EntryLabelFloating.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryLabelFloatingView : ContentView
    {
        int _fontSizePlaceholder = 16;
        int _fontSizeTitle = 12;
        int _marginTop = -30;
        private int tapCount;

        public event EventHandler ImagePassowordClicked;
        public EntryLabelFloatingView()
        {
            InitializeComponent();
            LabelFloating.TranslationX = 30;
            LabelFloating.FontSize = _fontSizePlaceholder;

            var tgr = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tgr.Tapped += ImagePassowordOn_Clicked;
            imagePassoword.GestureRecognizers.Add(tgr);
        }

        public virtual void ImagePassowordOn_Clicked(object sender, EventArgs e)
        {
            ImagePassowordClicked?.Invoke(sender, e);

            tapCount++;
            var imageSender = (Image)sender;
            if (tapCount % 2 == 0)
            {
                customEntry.IsPassword = true;
                imageSender.Source = "visivel.png";
            }
            else
            {
                customEntry.IsPassword = false;
                imageSender.Source = "invisivel.png";
            }
        }

        public string EntryText
        {
            get => (string)GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(string), string.Empty, BindingMode.TwoWay, null, HandleBindingPropertyChangedDelegate);

        public string EntryImage
        {
            get { return (string)GetValue(EntryImageProperty); }
            set { SetValue(EntryImageProperty, value); }
        }

        public static readonly BindableProperty EntryImageProperty = BindableProperty.Create(nameof(EntryImage), typeof(string), typeof(string), string.Empty, BindingMode.TwoWay, null, HandleBindingImagemPropertyChangedDelegate);

        static void HandleBindingImagemPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as EntryLabelFloatingView;
            if (control.EntryImage != null)
            {
                control.customEntry.EntryImage = control.EntryImage;
            }
        }

        public ImageSource ImagePassowordSource
        {
            get { return (ImageSource)GetValue(ImagePassowordSourceProperty); }
            set { SetValue(ImagePassowordSourceProperty, value); }
        }

        public static readonly BindableProperty ImagePassowordSourceProperty = BindableProperty.Create(nameof(ImagePassowordSource), typeof(ImageSource), 
            typeof(EntryLabelFloatingView), defaultBindingMode: BindingMode.TwoWay,propertyChanged: (bindable, oldVal, newVal) =>
                                                                                    {
                                                                                         var matEntry = (EntryLabelFloatingView)bindable;
                                                                                         matEntry.imagePassoword.Source = (ImageSource) newVal;
                                                                                    });

        public string lblTextTitle
        {
            get => (string)GetValue(lblTextTitleProperty);
            set => SetValue(lblTextTitleProperty, value);
        }

        public static readonly BindableProperty lblTextTitleProperty = BindableProperty.Create(nameof(lblTextTitle), typeof(string), typeof(string), string.Empty, BindingMode.TwoWay, null);

        static async void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as EntryLabelFloatingView;
            if (!control.customEntry.IsFocused)
            {
                if (!string.IsNullOrEmpty((string)newValue))
                {
                    await control.LabelFloatingToTransition(false);
                }
                else
                {
                    await control.LabelFloatingPlaceholderTransitionTo(false);
                }
            }
        }

        public static readonly BindableProperty EntryReturnTypeProperty =
            BindableProperty.Create(nameof(EntryReturnType), typeof(ReturnType), typeof(EntryLabelFloatingView), ReturnType.Default);
        public ReturnType EntryReturnType
        {
            get => (ReturnType)GetValue(EntryReturnTypeProperty);
            set => SetValue(EntryReturnTypeProperty, value);
        }

        public static readonly BindableProperty EntryIsPasswordProperty =
            BindableProperty.Create(nameof(EntryIsPassword), typeof(bool), typeof(EntryLabelFloatingView), default(bool));

        public bool EntryIsPassword
        {
            get { return (bool)GetValue(EntryIsPasswordProperty); }
            set { SetValue(EntryIsPasswordProperty, value); }
        }

        public static readonly BindableProperty EntryKeyboardProperty =
            BindableProperty.Create(nameof(EntryKeyboard), typeof(Keyboard), typeof(EntryLabelFloatingView), Keyboard.Default, coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default);
        public Keyboard EntryKeyboard
        {
            get { return (Keyboard)GetValue(EntryKeyboardProperty); }
            set { SetValue(EntryKeyboardProperty, value); }
        }

        public new void Focus()
        {
            if (IsEnabled)
            {
                customEntry.Focus();
            }
        }

        async void CustomEntry_Focused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(EntryText))
            {
                customEntry.IsEntrySelected = true;
                LabelFloating.Text = $" {LabelFloating.Text} ";
                LabelFloating.TextColor = Color.Blue;
                await LabelFloatingToTransition(true);
            }
        }

        async void CustomEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(EntryText))
            {
                customEntry.IsEntrySelected = false;
                LabelFloating.Text = LabelFloating.Text.Trim();
                LabelFloating.TextColor = Color.FromHex("#808080");
                await LabelFloatingPlaceholderTransitionTo(true);
            }
        }

        async Task LabelFloatingToTransition(bool animated)
        {
            if (animated)
            {
                var t1 = LabelFloating.TranslateTo(30, _marginTop, 100);
                LabelFloating.BackgroundColor = Color.FromHex("#F5F5F5");
                var t2 = SizeTo(_fontSizeTitle);
                await Task.WhenAll(t1, t2);
            }
            else
            {
                LabelFloating.TranslationX = 0;
                LabelFloating.TranslationY = -35;
                LabelFloating.FontSize = 14;
            }
        }

        async Task LabelFloatingPlaceholderTransitionTo(bool animated)
        {
            if (animated)
            {
                var t1 = LabelFloating.TranslateTo(30, 0, 100);
                var t2 = SizeTo(_fontSizePlaceholder);
                await Task.WhenAll(t1, t2);
            }
            else
            {
                LabelFloating.TranslationX = 10;
                LabelFloating.TranslationY = 0;
                LabelFloating.FontSize = _fontSizePlaceholder;
            }
        }

        void LabelFloating_Tapped(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                customEntry.Focus();
            }
        }

        Task SizeTo(int fontSize)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            Action<double> callback = input => { LabelFloating.FontSize = input; };
            double startingHeight = LabelFloating.FontSize;
            double endingHeight = fontSize;
            uint rate = 5;
            uint length = 100;
            Easing easing = Easing.Linear;

            LabelFloating.Animate("invis", callback, startingHeight, endingHeight, rate, length, easing, (v, c) => taskCompletionSource.SetResult(c));

            return taskCompletionSource.Task;
        }

        public event EventHandler Completed;
        void CustomEntry_Completed(object sender, EventArgs e)
        {
            Completed?.Invoke(this, e);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsEnabled))
            {
                customEntry.IsEnabled = IsEnabled;
            }
        }
    }
}