   M S B A M L   `   `   `  ����H  DUdpClinetTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:UdpClinetTestUdpClinetTest  5   	       UdpClinetTest.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:UdpClinetTest     5   	     � Title$  
MainWindow��5   	   $��509.804��6   $��760.294��6-    � 	Resources 5	         
FontStyle1��%(   &	        %�5
   
   
Italic$�6/   5      .��5      � -   5         �Name$ btStart��5      $��
0,-23,2,-2q�6    � ColumnDefinitions 5   
   �� 5      $
��271* �6   �� 5      $
��244* �6   .+�5   
   �� -   5         ��Name$ btStop��5      $��	Stop Dump��6!   $
��Left=�65   $��141,196,0,0q�6P   $	��Top=�6e   $	��105��6}   $��35��6�   $��20(�6�   $��Italic$�6�   6�   �� 5   
   -   5        $ 
btDiscover��5      $��Device Discover��6%   $
��Left=�6?   $��	21,93,0,0q�6Z   $	��Top=�6m   $	��160��6�   $��20��6�   �� 5   
   -   5         ��Name$ 	lstDevice��5      $
��Left=�6%   $	��300��6@   $��	44,85,0,0q�6M   $	��Top=�6`   $	��201��6x   $��1��6�   �� 5   
   -   5        $ 
btReadDump��5      $��Read Dump From Device��6%   $
��Left=�6E   $��
10,314,0,0q�6`   $	��Top=�6t   $	��237��6�   $��20(�6�   #���   6�   $��31��6�   �� 5   
   -   5        $��	FileWrite��5      $
��Left=�6%   $��
44,433,0,0q�6@   $	��Top=�6T   $��75��6l   $��20��6�   $��1��6�   �� 5   
   -   5        $ btFRead��5      $��	File Read��6"   $
��Left=�66   $��169,433,0,0q�6Q   $	��Top=�6f   $��75��6~   $��20��6�   $��1��6�   �� 5   
   -   5         ��Name$ txtData��5      $
��Left=�6#   $��23��6>   $��
44,412,0,0q�6J    �� TextWrapping$
 Wrap=�6^   $	��Top=�6r   $	��120��6�   $��1��6�   �� 5   
   -	   5        $ tbStart��5      $��
Start Dump��6"   $
��Left=�67   $��
21,196,0,0q�6R   $	��Top=�6f   $��95��6~   $��35��6�   6�   5      5      