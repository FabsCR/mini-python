using Microsoft.Win32;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MiniPython.Grammar;
using Antlr4.Runtime;
using Generated;
using MiniPython.Grammar.Checker;

namespace MiniPython
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _highlightingTimer;

        private readonly CustomErrorListener
            _errorListener = new CustomErrorListener();

        private readonly string[] keywords = { "def", "if", "else", "return", "print", "for", "while", "in", "len" };

        private readonly SolidColorBrush keywordBrush =
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d1a104"));

        private readonly SolidColorBrush symbolBrush =
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9c224b"));

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            this.KeyDown += MainWindow_KeyDown;

            _highlightingTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            _highlightingTimer.Tick += (s, e) =>
            {
                _highlightingTimer.Stop();
                var selectedTab = TabControl.SelectedItem as TabItem;
                if (selectedTab != null)
                {
                    var dockPanel = selectedTab.Content as DockPanel;
                    if (dockPanel != null)
                    {
                        var scrollViewer = dockPanel.Children[1] as ScrollViewer;
                        var grid = scrollViewer.Content as Grid;
                        var codeEditor = grid.Children[1] as RichTextBox;
                        ApplySyntaxHighlighting(codeEditor);
                    }
                }
            };
        }

        private void OpenUrl(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is string url)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }

        private void UpdateLineNumbers(RichTextBox lineNumbers, RichTextBox codeEditor)
        {
            lineNumbers.Document.Blocks.Clear();

            string codeText = new TextRange(codeEditor.Document.ContentStart, codeEditor.Document.ContentEnd).Text;

            string[] lines = codeText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            int lineCount = lines.Length;
            if (lineCount > 0 && string.IsNullOrWhiteSpace(lines[lineCount - 1]))
            {
                lineCount--;
            }

            for (int i = 1; i <= lineCount; i++)
            {
                Paragraph lineParagraph = new Paragraph(new Run(i.ToString()))
                {
                    LineHeight = 0.1
                };
                lineNumbers.Document.Blocks.Add(lineParagraph);
            }
        }

        private void ApplySyntaxHighlighting(RichTextBox codeEditor)
        {
            TextRange textRange = new TextRange(codeEditor.Document.ContentStart, codeEditor.Document.ContentEnd);
            textRange.ClearAllProperties();

            foreach (string keyword in keywords)
            {
                HighlightText(codeEditor, keyword, keywordBrush, true);
            }

            HighlightSymbolPairs(codeEditor, '(', ')', symbolBrush);
            HighlightSymbolPairs(codeEditor, '[', ']', symbolBrush);
            HighlightSymbolPairs(codeEditor, '{', '}', symbolBrush);
        }

        private TextPointer GetTextPointerAtOffset(TextPointer start, int offset)
        {
            TextPointer current = start;
            int count = 0;

            while (current != null && count < offset)
            {
                if (current.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    int runLength = current.GetTextRunLength(LogicalDirection.Forward);
                    if (count + runLength > offset)
                    {
                        return current.GetPositionAtOffset(offset - count);
                    }

                    count += runLength;
                }

                current = current.GetNextContextPosition(LogicalDirection.Forward);
            }

            return current;
        }

        private void HighlightSymbolPairs(RichTextBox richTextBox, char openSymbol, char closeSymbol,
            SolidColorBrush brush)
        {
            TextRange text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            string pattern = $@"\{openSymbol}|\{closeSymbol}";
            Regex regex = new Regex(pattern);

            Stack<int> openSymbolIndices = new Stack<int>();

            foreach (Match match in regex.Matches(text.Text))
            {
                if (match.Value == openSymbol.ToString())
                {
                    openSymbolIndices.Push(match.Index);
                }
                else if (match.Value == closeSymbol.ToString() && openSymbolIndices.Count > 0)
                {
                    int openIndex = openSymbolIndices.Pop();

                    TextPointer openStart = GetTextPointerAtOffset(richTextBox.Document.ContentStart, openIndex);
                    TextPointer openEnd = GetTextPointerAtOffset(richTextBox.Document.ContentStart, openIndex + 1);
                    if (openStart != null && openEnd != null)
                    {
                        TextRange openSymbolRange = new TextRange(openStart, openEnd);
                        openSymbolRange.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                    }

                    TextPointer closeStart = GetTextPointerAtOffset(richTextBox.Document.ContentStart, match.Index);
                    TextPointer closeEnd = GetTextPointerAtOffset(richTextBox.Document.ContentStart, match.Index + 1);
                    if (closeStart != null && closeEnd != null)
                    {
                        TextRange closeSymbolRange = new TextRange(closeStart, closeEnd);
                        closeSymbolRange.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                    }
                }
            }
        }

        private void HighlightText(RichTextBox richTextBox, string word, SolidColorBrush brush, bool isKeyword)
        {
            TextRange text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            string pattern = isKeyword ? $@"\b{Regex.Escape(word)}\b" : Regex.Escape(word);
            Regex regex = new Regex(pattern);

            foreach (Match match in regex.Matches(text.Text))
            {
                TextPointer start = GetTextPointerAtOffset(richTextBox.Document.ContentStart, match.Index);
                TextPointer end = GetTextPointerAtOffset(richTextBox.Document.ContentStart, match.Index + match.Length);

                if (start != null && end != null)
                {
                    TextRange wordRange = new TextRange(start, end);
                    wordRange.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                }
            }
        }

        private void AddNewTab_Click(object sender, MouseButtonEventArgs e)
        {
            TabItem newTab = CreateTab("Sin Titulo.mnpy", "");
            TabControl.Items.Insert(TabControl.Items.Count - 1, newTab);
            TabControl.SelectedItem = newTab;
        }

        private void AddTabWithContent(string fileName, string content)
        {
            TabItem newTab = CreateTab(fileName, content);

            if (newTab != null)
            {
                TabControl.Items.Insert(TabControl.Items.Count - 1, newTab);
                TabControl.SelectedItem = newTab;
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

            newTab.MouseEnter += (s, ev) => closeButton.Visibility = Visibility.Visible;
            newTab.MouseLeave += (s, ev) => closeButton.Visibility = Visibility.Hidden;

            DockPanel dockPanel = new DockPanel();

            Button runButton = new Button
            {
                Content = "▶",
                Width = 40,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d2d2d")),
                Foreground = new SolidColorBrush(Colors.Green),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d2d2d")),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10)
            };

            runButton.Click += RunCode_Click;

            runButton.IsEnabled = !string.IsNullOrWhiteSpace(content);

            DockPanel.SetDock(runButton, Dock.Right);
            dockPanel.Children.Add(runButton);

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition
                { Width = new GridLength(1, GridUnitType.Star) });

            RichTextBox lineNumbers = new RichTextBox
            {
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Gray")),
                IsReadOnly = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                Width = 50,
            };

            RichTextBox codeEditor = new RichTextBox
            {
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1e1e1e")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White")),
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                AcceptsReturn = true,
                AcceptsTab = true,
                Document = new FlowDocument(new Paragraph(new Run(content))),
                Margin = new Thickness(0),
            };

            SetConsistentLineHeight(codeEditor, 0.1);
            SetConsistentLineHeight(lineNumbers, 0.1);

            codeEditor.TextChanged += (s, ev) =>
            {
                runButton.IsEnabled = !string.IsNullOrWhiteSpace(
                    new TextRange(codeEditor.Document.ContentStart, codeEditor.Document.ContentEnd).Text.Trim());
                UpdateLineNumbers(lineNumbers, codeEditor);

                _highlightingTimer.Stop();
                _highlightingTimer.Start();
            };

            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            };

            grid.Children.Add(lineNumbers);
            grid.Children.Add(codeEditor);
            Grid.SetColumn(lineNumbers, 0);
            Grid.SetColumn(codeEditor, 1);

            scrollViewer.Content = grid;

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
                SaveFile_Click(null, null);
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
                Filter = "MiniPython Files (.mnpy)|*.mnpy"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);
                TabItem newTab = CreateTab(Path.GetFileName(openFileDialog.FileName), fileContent);
                TabControl.Items.Insert(TabControl.Items.Count - 1, newTab);
                TabControl.SelectedItem = newTab;

                DockPanel dockPanel = newTab.Content as DockPanel;
                ScrollViewer scrollViewer = dockPanel.Children[1] as ScrollViewer;
                Grid grid = scrollViewer.Content as Grid;

                RichTextBox lineNumbers = grid.Children[0] as RichTextBox;
                RichTextBox codeEditor = grid.Children[1] as RichTextBox;

                codeEditor.Document.Blocks.Clear();
                Paragraph paragraph = new Paragraph(new Run(fileContent));
                codeEditor.Document.Blocks.Add(paragraph);

                codeEditor.Loaded += (s, ev) => codeEditor.Document.LineHeight = 0.1;
                lineNumbers.Loaded += (s, ev) => lineNumbers.Document.LineHeight = 0.1;

                UpdateLineNumbers(lineNumbers, codeEditor);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            TabItem selectedTab = TabControl.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                DockPanel dockPanel = selectedTab.Content as DockPanel;
                if (dockPanel != null)
                {
                    ScrollViewer scrollViewer = dockPanel.Children.OfType<ScrollViewer>().FirstOrDefault();
                    if (scrollViewer != null)
                    {
                        Grid grid = scrollViewer.Content as Grid;
                        if (grid != null && grid.Children.Count > 1)
                        {
                            RichTextBox codeEditor = grid.Children[1] as RichTextBox;
                            if (codeEditor != null)
                            {
                                string code = new TextRange(codeEditor.Document.ContentStart,
                                    codeEditor.Document.ContentEnd).Text;
                                SaveFileDialog saveFileDialog = new SaveFileDialog
                                {
                                    Filter = "MiniPython Files (.mnpy)|*.mnpy"
                                };

                                if (saveFileDialog.ShowDialog() == true)
                                {
                                    File.WriteAllText(saveFileDialog.FileName, code);
                                    if (selectedTab.Header is StackPanel header &&
                                        header.Children[0] is TextBlock textBlock)
                                    {
                                        textBlock.Text = Path.GetFileName(saveFileDialog.FileName);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error: No se encontró el editor de texto en la pestaña.", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: La estructura de la pestaña es incorrecta.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: No se encontró el ScrollViewer dentro de la pestaña.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error: No se encontró el DockPanel dentro de la pestaña.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Error: No hay una pestaña seleccionada.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void OpenFromWeb_Click(object sender, RoutedEventArgs e)
        {
            string url = Microsoft.VisualBasic.Interaction.InputBox(
                "Ingrese el enlace directo del archivo (.mnpy):",
                "Abrir desde Web",
                "",
                -1, -1
            );

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Debe ingresar una URL válida.");
                return;
            }

            if (!url.EndsWith(".mnpy"))
            {
                MessageBox.Show("El enlace debe terminar en .mnpy.");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (url.Contains("github.com"))
                    {
                        url = url.Replace("github.com", "raw.githubusercontent.com")
                            .Replace("/blob/", "/");
                    }

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Error al descargar el archivo: {response.ReasonPhrase}");
                        return;
                    }

                    string fileContent = await response.Content.ReadAsStringAsync();

                    string fileName = Path.GetFileName(url);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = "ArchivoSinNombre.mnpy";
                    }

                    if (string.IsNullOrWhiteSpace(fileContent))
                    {
                        MessageBox.Show("El archivo descargado está vacío.");
                        return;
                    }

                    AddTabWithContent($"(web){fileName}", fileContent);

                    TabItem selectedTab = TabControl.SelectedItem as TabItem;
                    if (selectedTab != null)
                    {
                        DockPanel dockPanel = selectedTab.Content as DockPanel;
                        ScrollViewer scrollViewer = dockPanel.Children[1] as ScrollViewer;
                        Grid grid = scrollViewer.Content as Grid;

                        RichTextBox lineNumbers = grid.Children[0] as RichTextBox;
                        RichTextBox codeEditor = grid.Children[1] as RichTextBox;

                        UpdateLineNumbers(lineNumbers, codeEditor);
                        SetConsistentLineHeight(codeEditor, 0.1);
                        SetConsistentLineHeight(lineNumbers, 0.1);
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
                TabItem selectedTab = TabControl.SelectedItem as TabItem;
                if (selectedTab != null)
                {
                    DockPanel dockPanel = selectedTab.Content as DockPanel;
                    if (dockPanel != null)
                    {
                        ScrollViewer scrollViewer = dockPanel.Children[1] as ScrollViewer;
                        Grid grid = scrollViewer.Content as Grid;
                        RichTextBox codeEditor = grid.Children[1] as RichTextBox;
                        if (codeEditor != null)
                        {
                            string code = new TextRange(codeEditor.Document.ContentStart,
                                codeEditor.Document.ContentEnd).Text;
                            var lexer = new MiniPythonLexer(new AntlrInputStream(code));
                            var tokens = new CommonTokenStream(lexer);
                            var parser = new MiniPythonParser(tokens);
                            _errorListener.ErrorMsgs.Clear();
                            ConsoleOutput.Document.Blocks.Clear();
                            parser.RemoveErrorListeners();
                            lexer.RemoveErrorListeners();
                            parser.AddErrorListener(_errorListener);
                            lexer.AddErrorListener(_errorListener);
                            var result = parser.program();
                            if (_errorListener.HasErrors())
                            {
                                ShowInConsole(_errorListener.ToString());
                            }
                            else
                            {
                                // Ejecutar el ContextAnalizer
                                var contextAnalyzer = new ContextAnalizer();
                                contextAnalyzer.Visit(result);
                                if (contextAnalyzer.hasErrors())
                                {
                                    ShowInConsole(contextAnalyzer.ToString());
                                }
                                else
                                {
                                    ShowInConsole("Código ejecutado y analizado correctamente.");
                                }
                            }

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
                ShowInConsole($"Error al ejecutar el código: {ex.Message}");
            }
        }

        private void ShowInConsole(string message)
        {
            ConsoleOutput.Document.Blocks.Clear();

            string[] lines = message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                Paragraph paragraph = new Paragraph();
                if (line.Contains("line"))
                {
                    int lineIndex = line.IndexOf("line");
                    int colonIndex = line.IndexOf(":");

                    if (lineIndex != -1 && colonIndex != -1)
                    {
                        string lineNumberText = line.Substring(lineIndex + 5, colonIndex - lineIndex - 5);
                        string columnNumberText =
                            line.Substring(colonIndex + 1, line.IndexOf(" ", colonIndex) - colonIndex - 1);

                        int lineNumber = int.Parse(lineNumberText);
                        int columnNumber = int.Parse(columnNumberText);

                        paragraph.Inlines.Add(new Run(line.Substring(0, lineIndex)));

                        Hyperlink hyperlink = new Hyperlink(new Run($"line {lineNumber}:{columnNumber}"))
                        {
                            Foreground =
                                (SolidColorBrush)new BrushConverter().ConvertFrom("#87CEEB"),
                            ToolTip = $"Ir a la línea {lineNumber}, columna {columnNumber}"
                        };

                        hyperlink.Click += (sender, e) => NavigateToError(lineNumber, columnNumber);

                        paragraph.Inlines.Add(hyperlink);

                        paragraph.Inlines.Add(new Run(line.Substring(colonIndex + columnNumberText.Length + 1)));
                    }
                    else
                    {
                        paragraph.Inlines.Add(new Run(line));
                    }
                }
                else
                {
                    paragraph.Inlines.Add(new Run(line));
                }

                ConsoleOutput.Document.Blocks.Add(paragraph);
            }

            if (ConsolePanel.Visibility == Visibility.Collapsed)
            {
                ConsolePanel.Visibility = Visibility.Visible;
                ToggleConsoleButton.Content = "▼";
                Grid.SetRow(ToggleConsoleButton, 2);
            }
        }

        private void NavigateToError(int lineNumber, int columnNumber)
        {
            TabItem selectedTab = TabControl.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                DockPanel dockPanel = selectedTab.Content as DockPanel;
                if (dockPanel != null)
                {
                    ScrollViewer scrollViewer = dockPanel.Children[1] as ScrollViewer;
                    Grid grid = scrollViewer.Content as Grid;

                    RichTextBox codeEditor = grid.Children[1] as RichTextBox;

                    if (codeEditor != null)
                    {
                        TextPointer startPointer = GetTextPointerAtLineAndColumn(codeEditor, lineNumber, columnNumber);

                        if (startPointer != null)
                        {
                            TextPointer endPointer = startPointer.GetPositionAtOffset(5);
                            codeEditor.Selection.Select(startPointer, endPointer);
                            codeEditor.Focus();

                            codeEditor.ScrollToVerticalOffset(codeEditor.Selection.Start
                                .GetCharacterRect(LogicalDirection.Forward).Top);
                        }
                    }
                }
            }
        }

        private TextPointer GetTextPointerAtLineAndColumn(RichTextBox codeEditor, int line, int column)
        {
            TextPointer pointer = codeEditor.Document.ContentStart;
            int currentLine = 1;

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
                ToggleConsoleButton.Content = "▲";


                Grid.SetRow(ToggleConsoleButton, 4);
            }
            else
            {
                ConsolePanel.Visibility = Visibility.Visible;
                ToggleConsoleButton.Content = "▼";


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
                        OpenFromWeb_Click(null, null);
                    }
                    else
                    {
                        OpenFile_Click(null, null);
                    }

                    e.Handled = true;
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