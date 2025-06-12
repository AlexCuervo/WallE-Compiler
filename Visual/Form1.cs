
namespace WallE_Compiler;

public partial class Form1 : Form
{

    private TextEditor inputTextBox = new();
    private Button submitButton = new();
    private CanvasManager canvas = new();
    public Form1()
    {
        InitializeCustomComponents();
    }

    private void InitializeCustomComponents()
    {
        ClientSize = new Size(1920, 1080);
        Text = "Wall-E Compiler";

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
            Size = new Size(150, 30)
        };

        submitButton.Click += OnSubmitClicked!;

        Controls.Add(inputTextBox);
        Controls.Add(submitButton);


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
            }
            catch (ErrorDisplay error)
            {
                MessageBox.Show(error.getMessage);
            }

        }



        canvas.UpdateHighlightedCell();
    }

    void CreateCanvasControls()
    {
        Controls.Add(canvas.canvasPanel);
        Controls.Add(canvas.gridSizeSelector);
    }


}
