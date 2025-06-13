public partial class TextEditor : UserControl
{
    private TextBoxEx inputTextBox = new();
    private string? userCode;
    public string? getUserCode => userCode;
    private Panel panelLineas = new();

    public void SetUserCode(string newCode)
    {
        userCode = newCode;
        inputTextBox.Text = newCode;
    }
    public TextEditor()
    {
        CreateInputControls();
    }
    void CreateInputControls()
    {
        inputTextBox = new TextBoxEx
        {
            Multiline = true,
            Font = new Font("Consolas", 12),
            WordWrap = false,
            Location = new Point(40, 100),
            Size = new Size(9000, 500),
            ScrollBars = ScrollBars.Both,
            AcceptsTab = true,
            AcceptsReturn = true,
            Dock = DockStyle.Fill
        };

        inputTextBox.TextChanged += (s, e) => panelLineas.Invalidate();
        inputTextBox.TextChanged += (s, e) => userCode = inputTextBox.Text;
        inputTextBox.MouseWheel += (s, e) => panelLineas.Invalidate();
        inputTextBox.KeyDown += (s, e) => panelLineas.Invalidate();
        // inputTextBox.KeyUp += (s, e) => panelLineas.Invalidate();

        panelLineas = new Panel
        {
            Width = 40,
            Dock = DockStyle.Left,             // Se coloca a la izquierda del TextBox
            // BackColor = Color.FromArgb(240, 240, 240),// Color de fondo similar a VS Code
        };

        panelLineas.Paint += PanelLineas_Paint!;

        Controls.Add(inputTextBox);
        Controls.Add(panelLineas);
    }

    private void PanelLineas_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.Clear(panelLineas.BackColor);

        int firstCharIndex = inputTextBox.GetCharIndexFromPosition(new Point(0, 0));

        int firstLine = inputTextBox.GetLineFromCharIndex(firstCharIndex);

        int lineHeight = TextRenderer.MeasureText("A", inputTextBox.Font).Height;

        Point pos = inputTextBox.GetPositionFromCharIndex(firstCharIndex);
        int offsetY = pos.Y;

        int visibleLines = inputTextBox.ClientSize.Height / lineHeight;

        for (int i = 0; i <= visibleLines; i++)
        {
            // Se suma 1 para que la numeración comience en 1 en lugar de 0.
            string numeroLinea = (firstLine + i + 1).ToString();
            // Dibujamos el número en el panel. Ajustamos la posición para que quede alineado.
            e.Graphics.DrawString(numeroLinea, inputTextBox.Font, Brushes.Gray, new PointF(3, offsetY + i * lineHeight));
        }
    }

}