   M S B A M L   `   `   `  ����D  @TodoLista, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:TodoLista	TodoLista  5   	   U QSystem.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3aO8clr-namespace:System.Collections;assembly=System.RuntimeSystem.Collections 5   	       TodoLista.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:TodoLista     5   	   PCollections8clr-namespace:System.Collections;assembly=System.Runtime     5   	     � Title$  	TodoLista��5	   	   $	��450��6   $	��800��6(   
������.��5      �  � ColumnDefinitions 5   
   �� 5      $
��115* �6   �� 5      $	��63* �6   �� 5      $
��533* �6   �� 5      $	��89* �6   .+�5   
   �� -   5         ��Name$ TasksListBox��5      ���#FFFFFF���#FFFFFF$��2��6S   $��10,50,10,10q�6c   $��20(�6x   �� 5   
   -   5         ��Name$ 	AddButton��5      $��3��6$   $��+��64   $	��Top=�6@   $��Center=�6_   $��Center=�6�   $	��Top=�6�   $��75��6�   $��75��6�   $��	0,349,0,0q�6�   $��75(�6�   6�   �� 5   
   -   5         ��Name$ TopNameTextBox��5      ���#FFFFFF���#FFFFFF �� 
IsReadOnly$
 True��6U   $��2��6g   $
��Left=�6w   $��	10,10,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $��Dzisiaj��6�   $	��Top=�6�   $��25(�6�   �� 5   
   -   5         ��Name$ 	UserImage��5      $
��Left=�6#   $��	10,10,0,0q�6>   $	��Top=�6Q   $��60��6i   $��60��6t   �� 5   
   -   5        $ UserNameTextBox��5      ���#FFFFFF���#FFFFFF$��Witaj, ��6V   $
 True��6e   $��Center=�6w   $��0,10,0,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $��12(�6�   $��60��6�   $��2��6�    �� TextAlignment$ Center=�6  $	��158��6  �� 5   
   -   5        	 ��Name$	 TasksListsItemsControl��5      $��10,80,10,10q�67   $��15(�6L   ���#FFFFFF���#FFFFFF$��2��6�   ��6�   �� *
tasksLists+��5      �� 5      .R�5      �� -   5        
������ 
������ 

 J� Uid
 5      �� *Id+��6y   �� *Name+5      5      �� 5   
   -   5        $ OpenLoginWindowBtn��5      $��3��6-   $	��Top=�6=   $��Center=�6\   $	��Top=�6y   $��0,10,0,0q�6�   $��40��6�   .��5      �� -	   5         ��Name$ OpenLoginWindowBtnTextBox��5      $��Logowanie/Rejestracja��6;   $
��Wrap=�6X   5       �� 5!   
   -
   5        $ UserNameTextBox_Kopiuj��5!      ���#FFFFFF���#FFFFFF$��Twoje listy zadań��6]   $
 True��6v   $��Center=�6�   $��0,41,0,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $��12(�6�   $ Center=�6�   $��60��6  $��2��6  $	��158��6+  5"      5#      