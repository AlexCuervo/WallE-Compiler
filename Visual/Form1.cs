
namespace WallE_Compiler;

public partial class Form1 : Form
{

    private TextEditor inputTextBox = new();
    private Button submitButton = new();
    private CanvasManager canvas = new();
    private Button exportButton = new Button();
    private Button importButton = new Button();
    public Form1()
    {
        InitializeCustomComponents();
    }

    private void InitializeCustomComponents()
    {
        ClientSize = new Size(1920, 1080);
        Text = "Wall-E Compiler";
        BackColor = Color.DarkSlateBlue;
        CreateInputControls();
        CreateCanvasControls();
    }

    void CreateInputControls()
    {

        inputTextBox.Location = new Point(40, 30);
        inputTextBox.Size = new Size(900, 800);

        submitButton = new Button
        {
            Text = "Run",
            Location = new Point(40, 850),
            Size = new Size(150, 30),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.LightCyan
        };

        submitButton.Click += OnSubmitClicked!;

        exportButton = new Button
        {
            Text = "Export",
            Location = new Point(450, 850),
            Size = new Size(120, 30),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.LightCyan
        };

        exportButton.Click += ExportButton_Click!;

        importButton = new Button
        {
            Text = "Import",
            Location = new Point(250, 850),
            Size = new Size(120, 30),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.LightCyan
        };

        importButton.Click += ImportButton_Click!;

        Controls.Add(inputTextBox);
        Controls.Add(submitButton);
        Controls.Add(importButton);
        Controls.Add(exportButton);
    }
    void ExportButton_Click(object sender, EventArgs e)
    {
        using SaveFileDialog saveFileDialog = new();
        saveFileDialog.Filter = "Archivos GW (*.gw)|*.gw|Todos los archivos (*.*)|*.*";
        saveFileDialog.DefaultExt = "gw";
        saveFileDialog.Title = "Guardar contenido como archivo .gw";

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(saveFileDialog.FileName, inputTextBox.getUserCode);
            MessageBox.Show("Exportación realizada correctamente.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    private void ImportButton_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Archivos GW (*.gw)|*.gw|Todos los archivos (*.*)|*.*";
        openFileDialog.Title = "Selecciona un archivo .gw para cargar";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                string content = File.ReadAllText(openFileDialog.FileName);
                inputTextBox.SetUserCode(content);
                MessageBox.Show("Archivo cargado correctamente.", "Importar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    void OnSubmitClicked(object sender, EventArgs e)
    {

        Compiler.Reset();
        canvas.Reset();
        canvas.ClearGrid();

        Scope.Reset();

        string userCode = inputTextBox.getUserCode is null ? " " : inputTextBox.getUserCode;
        userCode = userCode.Replace('\r', ' ');
        userCode = userCode.Replace('\t', ' ');
        ASTExecutor.canvas = canvas;

        Compiler.InputCode(userCode);
        try
        {
            Compiler.Run();
        }
        catch (ErrorDisplay error)
        {
            MessageBox.Show(error.getMessage);
        }

        if (Compiler.check)
        {
            try
            {
                ASTExecutor.Execute(Compiler.programAST);
                canvas.UpdateHighlightedCell();
            }
            catch (ErrorDisplay error)
            {
                MessageBox.Show(error.getMessage);
            }

        }
    }

    void CreateCanvasControls()
    {
        Controls.Add(canvas.canvasPanel);
        Controls.Add(canvas.gridSizeSelector);
    }


}
