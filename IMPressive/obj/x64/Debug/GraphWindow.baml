   M S B A M L   `   `   `  ����d  `System.Windows.Controls.Input.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=nullpTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.ToolkitSystem.Windows.Controls  5   	    
IMPressive   IMPressive.GraphWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   icontrolsTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.Toolkit     5   	   X TXceed.Wpf.Toolkit, Version=2.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   -   5        $	��500��5   	   $	��725��6     � 
ResizeMode$  CanMinimize=�6Z    � Icon$ icon.ico��6s    � WindowStartupLocation$ CenterOwner=�6�    � 	Resources 5         FormCanvasGridTile j� Stretch j� TileMode j� Viewport j� ViewportUnits L� Drawing	 � Geometry
 �� 
StartPoint �� EndPoint
 � Pen >� 	Thickness >� Brush � Brush��%�  &	        L� 5   
   $
 None=�62   $
 Tile=�6A   $ 	0,0,30,30�5	      $ Absolute=�6(    5
      � 5      	 5      � 5      .��5      �� $	
 0,0&�6+   $
 30,0&�6<   �� 5      $

 30,0&�6+   $ 30,30&�6=   �� 5      $
 30,30&�6+   $
 0,30&�6>   �� 5      $

 0,30&�6+   $	 0,0&�6=   5       5      >� 5      $ 1Q�6   
 �����6,    5      ��5      	White��6'   5      5      5      .��5      � .+�5   
   �� -   5         ��Name$ chartCanvas��5      $��0,52,0,0q�6&   #��[  68   z�5       �� 5!      -   5        st��5!   !   5$      �� 5%   
   -   5         ��Name$ btnLoad��5%      $��OK��6"   $
��Left=�6/   $��
556,10,0,0q�6J   $	��Top=�6^   $��76��6v   ���	#FF8696AA  NormalButton#��[ 6�   $��26��6�   $
��Left=�6�   $	��Top=�6  ���	#FF1E395B6"  �� 5&   
   $��1q�6   $��	10,10,0,0q�6%    �� Padding$ 5q�68   ���	#FF8696AA �� CornerRadius$ 2��6\   $��26��6m   
�������$	��455��6�    J� SnapsToDevicePixels .  $
��Left=�6�   $	��Top=�6�   ._�5'      -   'System.Windows.Controls.AutoCompleteBox  -   5          Name$ txtOperation��5'   '   $
��Left=�6=   $��24��6X   $��-5q�6d   $	��Top=�6p   $	��454��6�   $��Cambria Math)�6�   $��18(�6�   ���	#FF333333$��0,2,3,0q�6�   $��0q�6�      Text$ cos(x)��6+  ��6�   Z� ��5(      �� 5)       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5*      �  � Color$ 	#FFE5EEFB��6'    � Offset$ 0Q�69   � 5+      $ White��6'   $ 1Q�65   5,      5.      5/      �� 50   
   -   5        $ btnClear��50      $��Effacer��6#   $
��Left=�65   $��
637,10,0,0q�6P   $	��Top=�6d   $��76��6|   ���	#FF8696AA#��[ 6�   $��26��6�   $
��Left=�6�   $	��Top=�6�   ���	#FF1E395B6)  �� 51   
   $��1q�6   $��
470,10,0,0q�6%   $ 0q�69   ���	#FF8696AA$ 2��6]   $��26��6n   $��81��6z    .  $
��Left=�6�   $	��Top=�6�   ._�52      �� -   5         ��Name$	 bg2��52      $��1q�6"   
�������$ 1��6J   ._�53      #  Xceed.Wpf.Toolkit.ColorPicker  -   5          Name$ cpxColor��53   #   $
��Left=�65   $	��Top=�6Q   $��24��6i   $��80��6u   $��-1,-1,-2,-1q�6�      SelectedColor$	 Red��6�      AvailableColorsHeader$ Couleurs disponibles��6�      StandardColorsHeader$ Couleurs standards��6�       RecentColorsHeader$  Couleurs récentes��6   !   ShowAdvancedButton! .  ���	#FF1E395B��6B  Z� ��54      �� 55      $ 0.5,1&�6.   $ 0.5,0&�6?   .��56      � $ 	#FFF0F6FB��6+   $ 0Q�6=   � 57      $ 	#FFD7E2EF��6+   $ 1Q�6=   � 58      $ 	#FFE2EAF3��6+   $	 0.5Q�6=   � 59      $ 	#FFD5E0ED��6+   $	 0.5Q�6=   5:      5<      5=      5>      5?      5@      