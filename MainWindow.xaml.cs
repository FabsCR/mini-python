using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MiniPython.Grammar; // Asegúrate de tener el namespace correcto
using Antlr4.Runtime;
using generated;

namespace MiniPython
{
    public partial class MainWindow : Window
    {
        private readonly CustomErrorListener _errorListener = new CustomErrorListener(); // Usamos tu CustomErrorListener

        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void CodeEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLineNumbers();
        }

        private void UpdateLineNumbers()
        {
            TabItem selectedTab = TabControl.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                Grid grid = selectedTab.Content as Grid;
                ScrollViewer lineNumberScrollViewer = grid.Children[0] as ScrollViewer;
                TextBlock lineNumbers = lineNumberScrollViewer.Content as TextBlock;
                TextBox codeEditor = grid.Children[1] as TextBox;

                lineNumbers.Text = "";
                int totalLines = codeEditor.LineCount;

                for (int i = 1; i <= totalLines; i++)
                {
                    lineNumbers.Text += i + Environment.NewLine;
                }

                var scrollViewer = GetScrollViewer(codeEditor);
                if (scrollViewer != null)
                {
                    lineNumberScrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
                }
            }
        }

        private void AddNewTab_Click(object sender, MouseButtonEventArgs e)
        {
            TabItem newTab = CreateTab("Sin Titulo.mnpy", "");
            TabControl.Items.Insert(TabControl.Items.Count - 1, newTab);
            TabControl.SelectedItem = newTab;

            // Habilitamos el botón de ejecutar si hay al menos una pestaña
            RunButton.IsEnabled = true;
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
            TabControl.Items.Insert(TabControl.Items.Count - 1, newTab); // Insertar antes del botón "+"
            TabControl.SelectedItem = newTab;
        }

private TabItem CreateTab(string header, string content)
{
    TabItem newTab = new TabItem();
    StackPanel tabHeader = new StackPanel { Orientation = Orientation.Horizontal };
    TextBlock headerText = new TextBlock { Text = header };
    Button closeButton = new Button { Content = "X", Width = 16, Height = 16, Visibility = Visibility.Hidden };
    closeButton.Click += (s, ev) => CloseTab(newTab);
    tabHeader.Children.Add(headerText);
    tabHeader.Children.Add(closeButton);
    newTab.Header = tabHeader;

    // Hover event to show/hide close button
    newTab.MouseEnter += (s, ev) => closeButton.Visibility = Visibility.Visible;
    newTab.MouseLeave += (s, ev) => closeButton.Visibility = Visibility.Hidden;

    Grid grid = new Grid();
    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1000) });

    // ScrollViewer for line numbers
    ScrollViewer lineNumberScrollViewer = new ScrollViewer
    {
        VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
    };

    TextBlock lineNumbers = new TextBlock
    {
        FontFamily = new FontFamily("Consolas"),
        FontSize = 14,
        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526")),
        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Gray")),
        VerticalAlignment = VerticalAlignment.Stretch,
        Padding = new Thickness(5),
        TextAlignment = TextAlignment.Right
    };
    lineNumberScrollViewer.Content = lineNumbers;
    grid.Children.Add(lineNumberScrollViewer);
    Grid.SetColumn(lineNumberScrollViewer, 0);

    TextBox codeEditor = new TextBox
    {
        FontFamily = new FontFamily("Consolas"),
        FontSize = 14,
        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1e1e1e")),
        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White")),
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
        AcceptsReturn = true,
        AcceptsTab = true,
        TextWrapping = TextWrapping.Wrap,
        Text = content
    };
    codeEditor.TextChanged += CodeEditor_TextChanged;
    grid.Children.Add(codeEditor);
    Grid.SetColumn(codeEditor, 1);

    var codeScrollViewer = GetScrollViewer(codeEditor);
    
    // Sincronizar los scrolls en ambas direcciones
    if (codeScrollViewer != null)
    {
        codeScrollViewer.ScrollChanged += (s, ev) =>
        {
            if (ev.VerticalChange != 0)
            {
                lineNumberScrollViewer.ScrollToVerticalOffset(codeScrollViewer.VerticalOffset);
            }
        };

        lineNumberScrollViewer.ScrollChanged += (s, ev) =>
        {
            if (ev.VerticalChange != 0)
            {
                codeScrollViewer.ScrollToVerticalOffset(lineNumberScrollViewer.VerticalOffset);
            }
        };
    }

    newTab.Content = grid;
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

            // Deshabilitar el botón si no hay pestañas abiertas
            if (TabControl.Items.Count == 1) // Solo queda el botón "+" como pestaña
            {
                RunButton.IsEnabled = false;
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
            string url = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el enlace directo del archivo:", "Abrir desde Web", "");
            if (url.EndsWith(".mnpy"))
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string fileContent = await client.GetStringAsync(url);
                        AddTabWithContent("ArchivoWeb.mnpy", fileContent);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al descargar el archivo: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("El enlace debe terminar en .mnpy.");
            }
        }

        private void RunCode_Click(object sender, RoutedEventArgs e)
        {
            TabItem selectedTab = TabControl.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                Grid grid = selectedTab.Content as Grid;
                TextBox codeEditor = grid.Children[1] as TextBox;
                string code = codeEditor.Text;

                try
                {
                    // Limpia los errores antes de ejecutar
                    _errorListener.ErrorMsgs.Clear(); // Limpiamos los errores anteriores
                    ConsoleOutput.Text = ""; // Limpiamos la consola antes de la ejecución

                    var lexer = new MiniPythonLexer(new AntlrInputStream(code));
                    var tokens = new CommonTokenStream(lexer);
                    var parser = new MiniPythonParser(tokens);

                    parser.RemoveErrorListeners(); // Quitamos los listeners por defecto
                    lexer.RemoveErrorListeners();
                    parser.AddErrorListener(_errorListener); // Agregamos nuestro CustomErrorListener
                    lexer.AddErrorListener(_errorListener);

                    var result = parser.program(); // Ejecutamos el parser

                    if (_errorListener.HasErrors())
                    {
                        ShowInConsole(_errorListener.ToString()); // Mostramos los errores en la consola
                    }
                    else
                    {
                        ShowInConsole("Código ejecutado correctamente.");
                    }

                    // Mostrar la consola y mover el botón cuando se ejecute el código
                    if (ConsolePanel.Visibility == Visibility.Collapsed)
                    {
                        ToggleConsoleVisibility(null, null); // Mostrar la consola si está oculta
                    }
                }
                catch (Exception ex)
                {
                    ShowInConsole($"Error al ejecutar el código: {ex.Message}");
                }
            }
        }

        private void ShowInConsole(string message)
        {
            ConsoleOutput.Text += message + Environment.NewLine;
            if (ConsolePanel.Visibility == Visibility.Collapsed)
            {
                ConsolePanel.Visibility = Visibility.Visible; // Mostrar la consola si está oculta
                ToggleConsoleButton.Content = "▼"; // Actualiza el botón
                Grid.SetRow(ToggleConsoleButton, 2); // Mover el botón justo encima de la consola
            }
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

    }
}
