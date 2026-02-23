namespace UiPrueba1.Controls
{
    /// <summary>
    /// Línea separadora reutilizable para formularios.
    /// Uso: &lt;controls:FormDivider Margin="0,14,0,12"/&gt;
    /// </summary>
    public class FormDivider : BoxView
    {
        public FormDivider()
        {
            Color = Color.FromArgb("#C8C8C8");
            HeightRequest = 1;
        }
    }
}
