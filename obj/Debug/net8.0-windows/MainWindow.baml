   M S B A M L   `   `   `  ����  
MiniPython    MiniPython.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � 
ResizeMode$  NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ 
MiniPython��5	   	    � WindowState$ 	Maximized=�6   ���#1e1e1e.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $	��200 �6   �� 5      $
��Auto �6   .+�5   
   o� $��0��6   
����������#1e1e1e.@�5      m� $��_File��6   .@�5      m� -   5        $��Abrir archivo local��5      68   m� 5      -   5        $��Abrir desde Web��5      64   m� 5      -   5        $��Guardar archivo��5      64   5      5      �� 5   
   -   5         ��Name$ 
TabControl��5      $��1��6)   ���#2d2d2d
����������#2d2d2d.@�5      �� -   5        $��+��5      $
{�True��6!   ���#2d2d2d
�������6Z   5       � 5#   
   -   5         �Name$ ConsolePanel��5#      $��3��6   ���#252526$y�	Collapsed=�6G   .+�5$      �� -   5         ��Name$ ConsoleOutput��5$      ���#1e1e1e���#ff0000$��Consolas)�5'      $��14(�5(       �� 
IsReadOnly$
 True��5)      "	 �� VerticalScrollBarVisibility$
	 Auto=�5*      $��5q�5+      5,      �� 5/   
   -   5        
 ��Name$
 ToggleConsoleButton��5/      $��4��50      $	��▲��51      $��40��52      $��20��53      ���#2d2d2d
�������$��Right=�56      $��5q�57      58      59      5:      