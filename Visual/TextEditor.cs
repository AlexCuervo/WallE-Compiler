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
            Dock = DockStyle.Fill,
            BackColor = Color.DarkSlateBlue,
            ForeColor = Color.LightCyan
        };

        inputTextBox.TextChanged += (s, e) => panelLineas.Invalidate();
        inputTextBox.TextChanged += (s, e) => userCode = inputTextBox.Text;
        inputTextBox.MouseWheel += (s, e) => panelLineas.Invalidate();
        inputTextBox.KeyDown += (s, e) => panelLineas.Invalidate();

        panelLineas = new Panel
        {
            Width = 40,
            Dock = DockStyle.Left,
            ForeColor = Color.LightCyan
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
            string numeroLinea = (firstLine + i + 1).ToString();

            e.Graphics.DrawString(numeroLinea, inputTextBox.Font, Brushes.Gray, new PointF(3, offsetY + i * lineHeight));
        }
    }

}