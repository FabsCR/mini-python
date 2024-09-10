using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using generated;

namespace MiniPython
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized; // Pantalla completa por defecto
        }

        private void CodeEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLineNumbers();
        }

        // Método para actualizar los números de línea para la pestaña activa
        private void UpdateLineNumbers()
        {
            TabItem selectedTab = TabControl.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                Grid grid = selectedTab.Content as Grid;

                // Obtener el ScrollViewer que contiene los números de línea
                ScrollViewer lineNumberScrollViewer = grid.Children[0] as ScrollViewer;
                TextBlock lineNumbers = lineNumberScrollViewer.Content as TextBlock;  // Acceder al TextBlock dentro del ScrollViewer

                TextBox codeEditor = grid.Children[1] as TextBox;  // El editor de código está en la columna 1

                // Limpiar los números de línea actuales
                lineNumbers.Text = "";
                int totalLines = codeEditor.LineCount;

                // Generar números de línea según la cantidad de líneas en el editor
                for (int i = 1; i <= totalLines; i++)
                {
                    lineNumbers.Text += i + Environment.NewLine;
                }

                // Sincronizar el scroll vertical del ScrollViewer de los números de línea con el del editor de código
                var scrollViewer = GetScrollViewer(codeEditor);
                if (scrollViewer != null)
                {
                    lineNumberScrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
                }
            }
        }


        // Método para agregar una nueva pestaña
        private void AddNewTab_Click(object sender, MouseButtonEventArgs e)
        {
            TabItem newTab = new TabItem
            {
                Header = "Sin Titulo.mnpy"
            };

            // Crear Grid con editor y números de línea
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // ScrollViewer para los números de línea
            ScrollViewer lineNumberScrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            };
            
            // Números de línea
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

            // Editor de texto
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
                TextWrapping = TextWrapping.Wrap
            };
            codeEditor.TextChanged += CodeEditor_TextChanged;
            grid.Children.Add(codeEditor);
            Grid.SetColumn(codeEditor, 1);

            // Vincular el evento ScrollChanged del ScrollViewer del editor de código
            var scrollViewer = GetScrollViewer(codeEditor);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += (s, ev) =>
                {
                    lineNumberScrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
                };
            }

            newTab.Content = grid;
            TabControl.Items.Insert(TabControl.Items.Count - 1, newTab); // Insertar antes del botón "+"
            TabControl.SelectedItem = newTab;
        }
        
        private ScrollViewer GetScrollViewer(DependencyObject o)
        {
            if (o is ScrollViewer) return (ScrollViewer)o;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);
                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }


        // Método para abrir un archivo .mnpy desde el explorador de archivos
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MiniPython Files (*.mnpy)|*.mnpy"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);
                AddTabWithContent(Path.GetFileName(openFileDialog.FileName), fileContent);
            }
        }

        // Método para abrir un archivo .mnpy desde un enlace directo
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

        // Método para agregar una nueva pestaña con contenido
        private void AddTabWithContent(string fileName, string content)
        {
            TabItem newTab = new TabItem
            {
                Header = fileName
            };

            // Crear Grid con editor y números de línea
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Números de línea
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
            grid.Children.Add(lineNumbers);
            Grid.SetColumn(lineNumbers, 0);

            // Editor de texto con contenido
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

            newTab.Content = grid;
            TabControl.Items.Insert(TabControl.Items.Count - 1, newTab); // Insertar antes del botón "+"

            TabControl.SelectedItem = newTab;
        }

        // Método para ejecutar el código de la pestaña activa
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
                    var lexer = new MiniPythonLexer(new Antlr4.Runtime.AntlrInputStream(code));
                    var parser = new MiniPythonParser(new Antlr4.Runtime.CommonTokenStream(lexer));
                    var result = parser.program(); // Ejecutamos el parser

                    MessageBox.Show("Código ejecutado correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al ejecutar el código: {ex.Message}");
                }
            }
        }

        // Método para guardar un archivo .mnpy
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
                    Filter = "MiniPython Files (*.mnpy)|*.mnpy"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, code);
                }
            }
        }
    }
}