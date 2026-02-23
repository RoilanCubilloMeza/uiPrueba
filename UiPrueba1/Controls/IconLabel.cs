namespace UiPrueba1.Controls
{
    /// <summary>
    /// Label preconfigurado para iconos Unicode en formularios.
    /// Defaults: FontSize=14, TextColor=Gray500, VerticalOptions=Center.
    /// Solo se sobreescriben los valores que difieran del default.
    /// <para>Uso: &lt;controls:IconLabel Text="&amp;#x260E;" FontSize="12" Margin="0,0,5,0"/&gt;</para>
    /// </summary>
    public class IconLabel : Label
    {
        public IconLabel()
        {
            FontSize        = 14;
            TextColor       = Color.FromArgb("#6B7280"); // Gray500
            VerticalOptions = LayoutOptions.Center;
        }
    }
}
