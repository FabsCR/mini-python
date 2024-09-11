using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MiniPython.Grammar; // Asegúrate de tener el namespace correcto
using Antlr4.Runtime;
using generated;
using HtmlAgilityPack;

namespace MiniPython
{
    public partial class MainWindow : Window
    {
        private readonly CustomErrorListener _errorListener = new CustomErrorListener(); // Usamos tu CustomErrorListener

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            this.KeyDown += MainWindow_KeyDown;
        }

        private void UpdateLineNumbers(RichTextBox lineNumbers, RichTextBox codeEditor)
        {
            lineNumbers.Document.Blocks.Clear();

            // Get the number of lines in the code editor
            var paragraphs = codeEditor.Document.Blocks.OfType<Paragraph>().ToList();
            for (int i = 1; i <= paragraphs.Count; i++)
            {
                // Create a paragraph for each line number with consistent line height
                Paragraph lineParagraph = new Paragraph(new Run(i.ToString()))
                {
                    LineHeight = 5 // Set consistent line height here
                };

                // Add the paragraph to the line numbers RichTextBox
                lineNumbers.Document.Blocks.Add(lineParagraph);
            }
        }

        private void AddNewTab_Click(object sender, MouseButtonEventArgs e)
        {
            TabItem newTab = CreateTab("Sin Titulo.mnpy", "");
            TabControl.Items.Insert(TabControl.Items.Count - 1, newTab);
            TabControl.SelectedItem = newTab;
        }

        private ScrollViewer GetScrollViewer(DependencyObject o)
        {
            if (o == null) return null;

            if (o is ScrollViewer)
                return (ScrollViewer)o;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);
                ScrollViewer scrollViewer = GetScrollViewer(child); // Recursión para buscar el ScrollViewer
                if (scrollViewer != null)
                    return scrollViewer;
            }

            return null;
        }

        private void AddTabWithContent(string fileName, string content)
        {
            TabItem newTab = CreateTab(fileName, content);
    
            if (newTab != null)
            {
                TabControl.Items.Insert(TabControl.Items.Count - 1, newTab); // Insertar antes del botón "+"
                TabControl.SelectedItem = newTab; // Seleccionar la nueva pestaña
            }
            else
            {
                MessageBox.Show("Error al crear la pestaña.");
            }
        }

        
          private TabItem CreateTab(string header, string content)
{
    TabItem newTab = new TabItem();
    StackPanel tabHeader = new StackPanel { Orientation = Orientation.Horizontal };
    TextBlock headerText = new TextBlock { Text = header };
    Button closeButton = new Button
    {
        Content = "X",
        Width = 16,
        Height = 16,
        Visibility = Visibility.Hidden,
        Background = Brushes.Transparent,
        BorderBrush = Brushes.Transparent
    };
    closeButton.Click += (s, ev) => CloseTab(newTab);

    tabHeader.Children.Add(headerText);
    tabHeader.Children.Add(closeButton);
    newTab.Header = tabHeader;

    // Show close button on hover
    newTab.MouseEnter += (s, ev) => closeButton.Visibility = Visibility.Visible;
    newTab.MouseLeave += (s, ev) => closeButton.Visibility = Visibility.Hidden;

    // Create a DockPanel to hold the button and code editor
    DockPanel dockPanel = new DockPanel();

    // Create the run button
    Button runButton = new Button
    {
        Content = "▶",  // Icono de ejecución
        Width = 40,
        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d2d2d")),
        Foreground = new SolidColorBrush(Colors.Green),
        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d2d2d")),
        HorizontalAlignment = HorizontalAlignment.Right,
        VerticalAlignment = VerticalAlignment.Top,
        Margin = new Thickness(10) // Espaciado
    };

    // Assign the click event to run the code
    runButton.Click += RunCode_Click;

    // Disable the run button if content is empty
    runButton.IsEnabled = !string.IsNullOrWhiteSpace(content);

    // Align the button to the right
    DockPanel.SetDock(runButton, Dock.Right);
    dockPanel.Children.Add(runButton);

    // Grid with two columns for line numbers and code editor
    Grid grid = new Grid();
    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) }); // Column for line numbers
    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Column for code editor

    // Line numbers RichTextBox
    RichTextBox lineNumbers = new RichTextBox
    {
        FontFamily = new FontFamily("Consolas"),
        FontSize = 14,
        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526")),
        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Gray")),
        IsReadOnly = true,
        VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,  // No individual scroll
        HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
        Width = 50, // Fixed width for 3-digit line numbers
    };

    // Code editor RichTextBox
    RichTextBox codeEditor = new RichTextBox
    {
        FontFamily = new FontFamily("Consolas"),
        FontSize = 14,
        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1e1e1e")),
        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White")),
        VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,  // No individual scroll
        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
        AcceptsReturn = true,
        AcceptsTab = true,
        Document = new FlowDocument(new Paragraph(new Run(content))),
        Margin = new Thickness(0),
    };

    // Set consistent line spacing for the code editor
    SetConsistentLineHeight(codeEditor, 5);
    SetConsistentLineHeight(lineNumbers, 5);

    // Event to disable the run button if the editor is empty
    codeEditor.TextChanged += (s, ev) =>
    {
        runButton.IsEnabled = !string.IsNullOrWhiteSpace(new TextRange(codeEditor.Document.ContentStart, codeEditor.Document.ContentEnd).Text.Trim());
        UpdateLineNumbers(lineNumbers, codeEditor);
    };

    // Ensure both RichTextBoxes share the same scroll
    ScrollViewer scrollViewer = new ScrollViewer
    {
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
    };

    // Add the RichTextBoxes to the Grid
    grid.Children.Add(lineNumbers);
    grid.Children.Add(codeEditor);
    Grid.SetColumn(lineNumbers, 0); // Column 0 for line numbers
    Grid.SetColumn(codeEditor, 1);  // Column 1 for code editor

    scrollViewer.Content = grid;

    // Add the ScrollViewer to the DockPanel
    DockPanel.SetDock(scrollViewer, Dock.Left);
    dockPanel.Children.Add(scrollViewer);

    newTab.Content = dockPanel;

    return newTab;
}

        private void CloseTab(TabItem tab)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea guardar los cambios antes de cerrar?", "Guardar",
                MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                SaveFile_Click(null, null); // Llamamos al método de guardar
                TabControl.Items.Remove(tab);
            }
            else if (result == MessageBoxResult.No)
            {
                TabControl.Items.Remove(tab);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MiniPython Files (.mnpy)|*.mnpy" // Corregido filtro para archivos .mnpy
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);
                TabItem newTab = CreateTab(Path.GetFileName(openFileDialog.FileName), fileContent);
                TabControl.Items.Insert(TabControl.Items.Count - 1, newTab);
                TabControl.SelectedItem = newTab;
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            TabItem selectedTab = TabControl.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                Grid grid = selectedTab.Content as Grid;
                TextBox codeEditor = grid.Children[1] as TextBox;
                string code = codeEditor.Text;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "MiniPython Files (.mnpy)|*.mnpy"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, code);
                    // Actualizamos el header de la pestaña con el nuevo nombre del archivo
                    StackPanel header = (StackPanel)selectedTab.Header;
                    ((TextBlock)header.Children[0]).Text = Path.GetFileName(saveFileDialog.FileName);
                }
            }
        }

        private async void OpenFromWeb_Click(object sender, RoutedEventArgs e)
{
    // Mostrar un cuadro de diálogo para ingresar la URL
    string url = Microsoft.VisualBasic.Interaction.InputBox(
        "Ingrese el enlace directo del archivo (.mnpy):", 
        "Abrir desde Web", 
        "", 
        -1, -1 // Centrar ventana de entrada
    );

    if (string.IsNullOrWhiteSpace(url))
    {
        MessageBox.Show("Debe ingresar una URL válida.");
        return;
    }

    // Verificar que la URL termine con .mnpy
    if (!url.EndsWith(".mnpy"))
    {
        MessageBox.Show("El enlace debe terminar en .mnpy.");
        return;
    }

    try
    {
        using (HttpClient client = new HttpClient())
        {
            // Si la URL es de GitHub, ajustar a la versión "raw"
            if (url.Contains("github.com"))
            {
                url = url.Replace("github.com", "raw.githubusercontent.com")
                         .Replace("/blob/", "/");
            }

            // Realizar la solicitud HTTP
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Error al descargar el archivo: {response.ReasonPhrase}");
                return;
            }

            // Descargar el archivo como texto
            string fileContent = await response.Content.ReadAsStringAsync();

            // Intentar extraer el nombre del archivo desde la URL
            string fileName = Path.GetFileName(url);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "ArchivoSinNombre.mnpy";
            }

            // Verificar si el archivo tiene contenido
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                MessageBox.Show("El archivo descargado está vacío.");
                return;
            }

            // Crear la pestaña y agregar el contenido
            AddTabWithContent($"(web){fileName}", fileContent);

            // Actualizar números de línea y ajustar el espaciado en la nueva pestaña
            TabItem selectedTab = TabControl.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                // Obtener el editor de código y los números de línea
                DockPanel dockPanel = selectedTab.Content as DockPanel;
                ScrollViewer scrollViewer = dockPanel.Children[1] as ScrollViewer;
                Grid grid = scrollViewer.Content as Grid;

                RichTextBox lineNumbers = grid.Children[0] as RichTextBox;
                RichTextBox codeEditor = grid.Children[1] as RichTextBox;

                // Actualizar números de línea
                UpdateLineNumbers(lineNumbers, codeEditor);

                // Ajustar el espaciado de línea
                SetConsistentLineHeight(codeEditor, 5); // Ajustar a tu preferencia
                SetConsistentLineHeight(lineNumbers, 5);
            }
        }
    }
    catch (HttpRequestException httpEx)
    {
        MessageBox.Show("Error de red al intentar descargar el archivo: " + httpEx.Message);
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error al descargar el archivo: " + ex.Message);
    }
}

        private void RunCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener la pestaña seleccionada
                TabItem selectedTab = TabControl.SelectedItem as TabItem;
                if (selectedTab != null)
                {
                    // Acceder al contenido de la pestaña
                    DockPanel dockPanel = selectedTab.Content as DockPanel;
                    if (dockPanel != null)
                    {
                        // Acceder al grid dentro del DockPanel
                        ScrollViewer scrollViewer = dockPanel.Children[1] as ScrollViewer;
                        Grid grid = scrollViewer.Content as Grid;

                        // Obtener el editor de código
                        RichTextBox codeEditor = grid.Children[1] as RichTextBox;
                        if (codeEditor != null)
                        {
                            // Extraer el texto del editor de código
                            string code = new TextRange(codeEditor.Document.ContentStart, codeEditor.Document.ContentEnd).Text;

                            // Inicializar el analizador léxico y sintáctico
                            var lexer = new MiniPythonLexer(new AntlrInputStream(code));
                            var tokens = new CommonTokenStream(lexer);
                            var parser = new MiniPythonParser(tokens);

                            // Limpiar errores anteriores
                            _errorListener.ErrorMsgs.Clear();
                            ConsoleOutput.Document.Blocks.Clear();

                            // Eliminar los listeners por defecto y agregar el custom
                            parser.RemoveErrorListeners();
                            lexer.RemoveErrorListeners();
                            parser.AddErrorListener(_errorListener);
                            lexer.AddErrorListener(_errorListener);

                            // Ejecutar el análisis
                            var result = parser.program();

                            // Mostrar errores o éxito en la consola
                            if (_errorListener.HasErrors())
                            {
                                ShowInConsole(_errorListener.ToString());
                            }
                            else
                            {
                                ShowInConsole("Código ejecutado correctamente.");
                            }

                            // Mostrar la consola si está oculta
                            if (ConsolePanel.Visibility == Visibility.Collapsed)
                            {
                                ToggleConsoleVisibility(null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción no controlada y mostrarla en la consola
                ShowInConsole($"Error al ejecutar el código: {ex.Message}");
            }
        }
        
private void ShowInConsole(string message)
{
    // Limpiamos el contenido anterior de la consola
    ConsoleOutput.Document.Blocks.Clear();

    string[] lines = message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

    foreach (var line in lines)
    {
        Paragraph paragraph = new Paragraph();

        // Si el mensaje contiene la palabra "line", intentamos crear un hipervínculo
        if (line.Contains("line"))
        {
            int lineIndex = line.IndexOf("line");
            int colonIndex = line.IndexOf(":");

            if (lineIndex != -1 && colonIndex != -1)
            {
                // Extraemos el número de línea y columna del mensaje
                string lineNumberText = line.Substring(lineIndex + 5, colonIndex - lineIndex - 5);
                string columnNumberText = line.Substring(colonIndex + 1, line.IndexOf(" ", colonIndex) - colonIndex - 1);

                // Convertimos a números enteros
                int lineNumber = int.Parse(lineNumberText);
                int columnNumber = int.Parse(columnNumberText);

                // Añadimos el texto antes del hipervínculo
                paragraph.Inlines.Add(new Run(line.Substring(0, lineIndex)));

                // Creamos el hipervínculo
                Hyperlink hyperlink = new Hyperlink(new Run($"line {lineNumber}:{columnNumber}"))
                {
                    Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#87CEEB"), // Color del hipervínculo
                    ToolTip = $"Ir a la línea {lineNumber}, columna {columnNumber}"
                };

                // Asignamos el evento Click al hipervínculo
                hyperlink.Click += (sender, e) => NavigateToError(lineNumber, columnNumber);

                paragraph.Inlines.Add(hyperlink);

                // Añadimos el texto después del hipervínculo
                paragraph.Inlines.Add(new Run(line.Substring(colonIndex + columnNumberText.Length + 1)));
            }
            else
            {
                // Si no hay un formato adecuado, simplemente añadimos la línea como un texto normal
                paragraph.Inlines.Add(new Run(line));
            }
        }
        else
        {
            // Si no contiene "line", añadimos la línea completa como texto normal
            paragraph.Inlines.Add(new Run(line));
        }

        // Añadimos el párrafo a la consola
        ConsoleOutput.Document.Blocks.Add(paragraph);
    }

    // Nos aseguramos de que la consola esté visible
    if (ConsolePanel.Visibility == Visibility.Collapsed)
    {
        ConsolePanel.Visibility = Visibility.Visible;
        ToggleConsoleButton.Content = "▼";  // Cambia el botón a ocultar
        Grid.SetRow(ToggleConsoleButton, 2); // Mueve el botón justo encima de la consola
    }
}

        
private void NavigateToError(int lineNumber, int columnNumber)
{
    // Obtenemos la pestaña seleccionada
    TabItem selectedTab = TabControl.SelectedItem as TabItem;
    if (selectedTab != null)
    {
        // Accedemos al contenido de la pestaña
        DockPanel dockPanel = selectedTab.Content as DockPanel;
        if (dockPanel != null)
        {
            // Accedemos al grid dentro del DockPanel
            ScrollViewer scrollViewer = dockPanel.Children[1] as ScrollViewer;
            Grid grid = scrollViewer.Content as Grid;

            // Obtenemos el editor de código
            RichTextBox codeEditor = grid.Children[1] as RichTextBox;

            if (codeEditor != null)
            {
                // Buscamos el puntero de texto en la línea y columna especificada
                TextPointer startPointer = GetTextPointerAtLineAndColumn(codeEditor, lineNumber, columnNumber);

                if (startPointer != null)
                {
                    // Resaltamos el área del error
                    TextPointer endPointer = startPointer.GetPositionAtOffset(5); // Resaltamos 5 caracteres
                    codeEditor.Selection.Select(startPointer, endPointer);
                    codeEditor.Focus();

                    // Desplazamos el editor hasta la posición del error
                    codeEditor.ScrollToVerticalOffset(codeEditor.Selection.Start.GetCharacterRect(LogicalDirection.Forward).Top);
                }
            }
        }
    }
}

// Esta función obtiene el TextPointer basado en el número de línea y columna
        private TextPointer GetTextPointerAtLineAndColumn(RichTextBox codeEditor, int line, int column)
        {
            TextPointer pointer = codeEditor.Document.ContentStart;
            int currentLine = 1;

            // Recorremos las líneas para llegar a la línea correcta
            while (pointer != null && currentLine < line)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
                {
                    Block block = pointer.GetAdjacentElement(LogicalDirection.Forward) as Block;
                    if (block is Paragraph)
                    {
                        currentLine++;
                    }
                }
                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }

            // Cuando estamos en la línea correcta, ahora buscamos la columna
            if (pointer != null)
            {
                for (int i = 0; i < column && pointer != null; i++)
                {
                    pointer = pointer.GetNextInsertionPosition(LogicalDirection.Forward);
                }
            }

            return pointer;
        }


        
        private void ToggleConsoleVisibility(object sender, RoutedEventArgs e)
        {
            if (ConsolePanel.Visibility == Visibility.Visible)
            {
                ConsolePanel.Visibility = Visibility.Collapsed;
                ToggleConsoleButton.Content = "▲"; // Cambia el contenido a "▲" cuando la consola está oculta

                // Mover el botón a la parte inferior de la ventana
                Grid.SetRow(ToggleConsoleButton, 4);
            }
            else
            {
                ConsolePanel.Visibility = Visibility.Visible;
                ToggleConsoleButton.Content = "▼"; // Cambia el contenido a "▼" cuando la consola está visible

                // Mover el botón justo encima de la consola
                Grid.SetRow(ToggleConsoleButton, 2);
            }
        }
        
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.O)
                {
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        // Abrir desde la web (Ctrl + Shift + O)
                        OpenFromWeb_Click(null, null);
                    }
                    else
                    {
                        // Abrir archivo local (Ctrl + O)
                        OpenFile_Click(null, null);
                    }

                    e.Handled = true; // Previene otros eventos
                }
            }
        }
        private void SetConsistentLineHeight(RichTextBox richTextBox, double lineHeight)
        {
            foreach (Block block in richTextBox.Document.Blocks)
            {
                if (block is Paragraph paragraph)
                {
                    paragraph.LineHeight = lineHeight;
                }
            }
        }

    }
}