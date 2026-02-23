using Microsoft.Maui.Controls.Shapes;

namespace UiPrueba1.Controls
{
   
    [ContentProperty(nameof(CardContent))]
    public class SectionCard : ContentView
    {
        private readonly VerticalStackLayout _stack;
        private Label? _titleLabel;


        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(SectionCard), string.Empty,
                propertyChanged: (b, _, __) => ((SectionCard)b).RefreshTitle());

        public static readonly BindableProperty CardContentProperty =
            BindableProperty.Create(nameof(CardContent), typeof(View), typeof(SectionCard), null,
                propertyChanged: (b, _, __) => ((SectionCard)b).RefreshCardContent());

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public View? CardContent
        {
            get => (View?)GetValue(CardContentProperty);
            set => SetValue(CardContentProperty, value);
        }


        public SectionCard()
        {
            _stack = new VerticalStackLayout { Spacing = 0 };

            base.Content = new Border
            {
                BackgroundColor = Colors.White,
                StrokeShape     = new RoundRectangle { CornerRadius = 12 },
                Stroke          = new SolidColorBrush(Color.FromArgb("#E1E1E1")),
                StrokeThickness = 1,
                Padding         = new Thickness(20),
                Shadow          = new Shadow
                {
                    Brush   = new SolidColorBrush(Color.FromArgb("#000000")),
                    Offset  = new Point(0, 2),
                    Radius  = 8,
                    Opacity = 0.07f
                },
                Content         = _stack
            };
        }


        private void RefreshTitle()
        {
            if (_titleLabel is not null)
                _stack.Remove(_titleLabel);

            _titleLabel = null;

            if (string.IsNullOrEmpty(Title)) return;

            _titleLabel = new Label
            {
                Text           = Title,
                FontSize       = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor      = Color.FromArgb("#212121"),
                Margin         = new Thickness(0, 0, 0, 14)
            };
            _stack.Insert(0, _titleLabel);
        }

        private void RefreshCardContent()
        {
            foreach (var child in _stack.Children.Where(c => c != _titleLabel).ToList())
                _stack.Remove(child);

            if (CardContent is not null)
                _stack.Add(CardContent);
        }
    }
}

