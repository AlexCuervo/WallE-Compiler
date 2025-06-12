public class TextBoxEx : TextBox
{
    private const int WM_VSCROLL = 0x115;
    private const int WM_MOUSEWHEEL = 0x20A;
    public event EventHandler? ScrollOccurred;

    protected override void WndProc(ref Message m)
    {
        // Llamamos al método base para que el control procese el mensaje normalmente.
        base.WndProc(ref m);

        // Detectamos si el mensaje es de tipo scroll vertical o rueda del ratón.
        if (m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL)
        {
            // Disparamos el evento personalizado para notificar a los suscriptores que se ha producido un scroll.
            OnScrollOccurred(EventArgs.Empty);
        }
    }
    protected virtual void OnScrollOccurred(EventArgs e)
    {
        ScrollOccurred?.Invoke(this, e);
    }
}