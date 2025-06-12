public class CanvasManager
{
    public Panel canvasPanel = new();
    public Color actualColor = Color.Black;
    public int gridSize => (int)gridSizeSelector.Value;
    public NumericUpDown gridSizeSelector = new();
    public int actualX;
    public int actualY;
    public int brushSize = 1;
    int currentHighlightedX = -1;
    int currentHighlightedY = -1;

    public CanvasManager()
    {
        CreateCanvas();
    }

    public void Reset()
    {
        actualX = 0;
        actualY = 0;
        actualColor = Color.Black;
        brushSize = 1;

    }
    void CreateCanvas()
    {
        canvasPanel = new Panel
        {
            Location = new Point(1000, 100),
            Size = new Size(800, 800),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        CreateGridControls();
    }

    void CreateGridControls()
    {
        gridSizeSelector = new NumericUpDown
        {
            Minimum = 2,
            Maximum = 50,
            Value = 5,
            Location = new Point(750, 860),
            Size = new Size(100, 20)
        };


        gridSizeSelector.ValueChanged += (s, e) => GenerateGrid();

        var label = new Label
        {
            Text = "Tamaño del grid:",
            Location = new Point(750, 840),
            AutoSize = true
        };


        GenerateGrid();
    }

    void GenerateGrid()
    {
        canvasPanel.Controls.Clear();

        int gridSize = (int)gridSizeSelector.Value;
        int cellSize = canvasPanel.Width / gridSize;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                var cell = new Button
                {
                    Size = new Size(cellSize, cellSize),
                    Location = new Point(col * cellSize, row * cellSize),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Tag = new Point(col, row) // Guardamos posición
                };

                cell.Click += OnCellClicked!;
                canvasPanel.Controls.Add(cell);
            }
        }

    }

    void OnCellClicked(object sender, EventArgs e)
    {
        var cell = (Button)sender;
        var position = (Point)cell.Tag!;

        cell.BackColor = actualColor;
    }

    public void SetCellColor(int x, int y, Color color)
    {
        if (x >= 0 && x < gridSizeSelector.Value &&
            y >= 0 && y < gridSizeSelector.Value)
        {
            int index = y * (int)gridSizeSelector.Value + x;
            if (index < canvasPanel.Controls.Count)
            {
                canvasPanel.Controls[index].BackColor = color;
            }
        }
    }

    public void ClearGrid(Color? color = null)
    {
        foreach (Control control in canvasPanel.Controls)
        {
            if (control is Button cell)
            {
                cell.BackColor = color ?? Color.White;
            }
        }
    }
    public void PaintCellsWithBrush(int centerX, int centerY, int brushSize)
    {
        if (brushSize % 2 == 0)
            throw new ArgumentException("El tamaño de la brocha debe ser un número impar.", nameof(brushSize));

        int radius = brushSize / 2;


        for (int row = centerY - radius; row <= centerY + radius; row++)
        {
            for (int col = centerX - radius; col <= centerX + radius; col++)
            {
                SetCellColor(col, row, actualColor);
            }
        }
    }
    public Button? GetCellAt(int x, int y)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
            return null;
        int index = y * gridSize + x;
        if (index >= canvasPanel.Controls.Count)
            return null;
        return canvasPanel.Controls[index] as Button;
    }
    public bool IsCellColor(Color targetColor, int x, int y)
    {
        Button? cell = GetCellAt(x, y);

        if (cell is null)
        {
            return false;
        }

        return cell.BackColor.ToArgb() == targetColor.ToArgb();
    }
    public int CountCellsInRectangle(Color targetColor, int x1, int y1, int x2, int y2)
    {
        int count = 0;

        int minX = Math.Min(x1, x2);
        int maxX = Math.Max(x1, x2);
        int minY = Math.Min(y1, y2);
        int maxY = Math.Max(y1, y2);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Button? cell = GetCellAt(x, y);
                if (cell != null)
                {
                    if (cell.BackColor == targetColor)
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }
    public void FloodFillRegion()
    {
        Button? startCell = GetCellAt(actualX, actualY);
        if (startCell == null)
            return;

        Color originalColor = startCell.BackColor;
        Color newColor = actualColor;

        if (originalColor == newColor)
            return;

        Stack<Point> pixelsToCheck = new Stack<Point>();
        pixelsToCheck.Push(new Point(actualX, actualY));

        while (pixelsToCheck.Count > 0)
        {
            Point pt = pixelsToCheck.Pop();
            Button? cell = GetCellAt(pt.X, pt.Y);
            if (cell == null)
                continue;

            if (cell.BackColor != originalColor)
                continue;

            cell.BackColor = newColor;

            pixelsToCheck.Push(new Point(pt.X + 1, pt.Y));
            pixelsToCheck.Push(new Point(pt.X - 1, pt.Y));
            pixelsToCheck.Push(new Point(pt.X, pt.Y + 1));
            pixelsToCheck.Push(new Point(pt.X, pt.Y - 1));
        }
    }

    public void PaintAlongDirection(int directionX, int directionY, int steps)
    {
        int posX = actualX;
        int posY = actualY;
        for (int i = 0; i < steps; i++)
        {
            int currentX = posX + i * directionX;
            int currentY = posY + i * directionY;


            if (GetCellAt(currentX, currentY) == null)
                break;
            actualX = currentX;
            actualY = currentY;
            UpdateHighlightedCell();

            PaintCellsWithBrush(currentX, currentY, brushSize);
        }
    }

    public void DrawCircle(int centerX, int centerY, int radio)
    {
        // Empleamos la variante donde x parte de 'radio' y y de 0,
        // y usamos un parámetro de decisión para determinar el siguiente punto.
        int x = radio;
        int y = 0;
        int decisionParam = 1 - radio;

        // Mientras y sea menor o igual que x, se cubre la octante superior
        while (y <= x)
        {
            // Se pintan los 8 puntos simétricos en el círculo
            PaintCellsWithBrush(centerX + x, centerY + y, brushSize);
            PaintCellsWithBrush(centerX + y, centerY + x, brushSize);
            PaintCellsWithBrush(centerX - y, centerY + x, brushSize);
            PaintCellsWithBrush(centerX - x, centerY + y, brushSize);
            PaintCellsWithBrush(centerX - x, centerY - y, brushSize);
            PaintCellsWithBrush(centerX - y, centerY - x, brushSize);
            PaintCellsWithBrush(centerX + y, centerY - x, brushSize);
            PaintCellsWithBrush(centerX + x, centerY - y, brushSize);

            y++;

            // Actualización del parámetro de decisión:
            if (decisionParam <= 0)
            {
                decisionParam += 2 * y + 1;
            }
            else
            {
                x--;
                decisionParam += 2 * (y - x) + 1;
            }
        }
    }
    public void MoveAndDrawRectangle(int directionX, int directionY, int steps, int rectHeight, int rectWidth)
    {
        // Se asume que actualX y actualY son la posición actual en el canvas,
        // definidas en la clase (por ejemplo, public int actualX, actualY;)
        int finalX = actualX;
        int finalY = actualY;

        // Mover 'steps' veces en la dirección especificada.
        for (int i = 0; i < steps; i++)
        {
            int nextX = finalX + directionX;
            int nextY = finalY + directionY;

            // Verificamos que la siguiente posición esté dentro del canvas.
            if (GetCellAt(nextX, nextY) == null)
            {
                // Si la celda no existe, salimos del bucle.
                throw new ErrorDisplay("WallE movement went out of canvas");
            }

            finalX = nextX;
            finalY = nextY;
        }

        // Actualizamos la posición actual (si se desea que el "puntero" se mueva).
        actualX = finalX;
        actualY = finalY;

        // Dibuja el rectángulo centrado en la casilla final.
        DrawRectangleBorderCentered(finalX, finalY, rectHeight, rectWidth);
    }
    public void DrawRectangleBorderCentered(int centerX, int centerY, int rectWidth, int rectHeight)
    {

        rectHeight += 2;
        rectWidth += 2;
        int startX = centerX - rectWidth / 2;
        int startY = centerY - rectHeight / 2;

        // Recorrer todas las celdas del rectángulo
        for (int y = 0; y < rectHeight; y++)
        {
            for (int x = 0; x < rectWidth; x++)
            {
                // Pinta la celda si estamos en un borde (fila 0, última fila, columna 0 o última columna)
                if (x == 0 || x == rectWidth - 1 || y == 0 || y == rectHeight - 1)
                {
                    PaintCellsWithBrush(startX + x, startY + y, brushSize);
                }
            }
        }
    }
    public void UpdateHighlightedCell()
    {
        // Si hay una celda previamente resaltada, se quita su resaltado.
        if (currentHighlightedX != -1 && currentHighlightedY != -1)
        {
            ClearCellHighlight(currentHighlightedX, currentHighlightedY);
        }

        // Se actualizan las coordenadas de la celda resaltada.
        currentHighlightedX = actualX;
        currentHighlightedY = actualY;

        // Se aplica el resaltado a la nueva celda.
        HighlightCurrentCell();
    }
    void HighlightCurrentCell(int borderThickness = 4)
    {
        Button? cell = GetCellAt(actualX, actualY);

        if (cell != null)
        {
            cell.FlatStyle = FlatStyle.Flat;

            cell.FlatAppearance.BorderColor = InvertColor(cell.BackColor);
            cell.FlatAppearance.BorderSize = borderThickness;
        }
    }
    void ClearCellHighlight(int x, int y)
    {
        Button? cell = GetCellAt(x, y);
        if (cell != null)
        {
            cell.FlatAppearance.BorderColor = Color.Black;
            cell.FlatAppearance.BorderSize = 1;
        }
    }

    Color InvertColor(Color color)
    {
        return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
    }
}