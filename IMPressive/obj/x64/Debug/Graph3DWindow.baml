   M S B A M L   `   `   `  ����d  `System.Windows.Controls.Input.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=nullpTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.ToolkitSystem.Windows.Controls  5   	    
IMPressive   IMPressive.Graph3DWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   icontrolsTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.Toolkit     5   	   X TXceed.Wpf.Toolkit, Version=2.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   -   5        $	��500��5   	   $	��725��6     � Icon$  icon.ico��6Z    � WindowStartupLocation$ CenterOwner=�6j    � 	Resources 5         FormCanvasGridTile j� Stretch j� TileMode j� Viewport j� ViewportUnits L� Drawing � Geometry	 �� 
StartPoint
 �� EndPoint
 � Pen >� 	Thickness >� Brush � Brush��%�  &	        L� 5   
   $
 None=�62   $
 Tile=�6A   $ 	0,0,30,30�5	      $ Absolute=�6(    5
      � 5       5      � 5      .��5      �� $		 0,0&�6+   $

 30,0&�6<   �� 5      $
	 30,0&�6+   $
 30,30&�6=   �� 5      $	 30,30&�6+   $

 0,30&�6>   �� 5      $
	 0,30&�6+   $	
 0,0&�6=   5       5      >� 5      $ 1Q�6   
 �����6,    5      ��5      	White��6'   5      5      5      .��5      � .+�5    
   '� -   5        $��mainViewport��5       $��0,52,0,0q�6)    '� Camera 5!      P� 5"      -   5        
camera�� � FarPlaneDistance$ 10Q�5#       � NearPlaneDistance$ 1Q�5$       � LookDirection$ 0,0,-15�5%       � UpDirection$ 0,1,05�5&       � Position$ 0,0,21�5'      x�5)      g� 5*      -   5        
Light1�� g� Content 5+      t� 5,       �� Color$ White��6+    t� 	Direction$ 1,1,-15�69   5.      50      �� 51   
   -   5         ��Name$ btnLoad��51      $��OK��6"   $��Right=�6/   $��	0,10,85,0q�6K   $	��Top=�6^   $��76��6v   ���	#FF8696AA  NormalButton#��[ 6�   $��26��6�   $
��Left=�6�   $	��Top=�6  ���	#FF1E395B6"  �� 52   
   $��1q�6   $��10,10,252,0q�6%    �� Padding$ 5q�6:   ���	#FF8696AA �� CornerRadius$ 2��6^   $��26��6o   
������� J� SnapsToDevicePixels .  $	��Top=�6�   ._�53      -   'System.Windows.Controls.AutoCompleteBox  -   5          Name$ txtOperation��53   '   $��24��6=   $��-5q�6I   $	��Top=�6U   $��Cambria Math)�6m   $��18(�6�   ���	#FF333333$��0,2,3,0q�6�   $��0q�6�      Text$ sin(4xy)��6  $��Stretch=�6  ��6�   Z� ��54      �� 55       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��56      �   � Color$  	#FFE5EEFB��6'   ! � Offset$! 0Q�69   � 57      $  White��6'   $! 1Q�65   58      5:      5;      �� 5<   
   -   5        $ btnClear��5<      $��Effacer��6#   $��Right=�65   $��0,10,4,0q�6Q   $	��Top=�6c   $��76��6{   ���	#FF8696AA#��[ 6�   $��26��6�   $
��Left=�6�   $	��Top=�6�   ���	#FF1E395B6(  �� 5=   
   $��1q�6   $��
0,10,166,0q�6%   $ 0q�69   ���	#FF8696AA$ 2��6]   $��26��6n   $��81��6z    .  $��Right=�6�   $	��Top=�6�   ._�5>      �� -   5        " ��Name$	" bg2��5>      $��1q�6"   
�������$ 1��6J   ._�5?      #  Xceed.Wpf.Toolkit.ColorPicker  -	   5        #  Name$# cpxColor��5?   #   $
��Left=�65   $	��Top=�6Q   $��24��6i   $��80��6u   $��-1,-1,-2,-1q�6�   $   SelectedColor$	$ Red��6�   %   AvailableColorsHeader$% Couleurs disponibles��6�   &   StandardColorsHeader$& Couleurs standards��6�   '   RecentColorsHeader$' Couleurs récentes��6   (   ShowAdvancedButton( .  ���	#FF1E395B��6B  Z� ��5@      �� 5A      $ 0.5,1&�6.   $ 0.5,0&�6?   .��5B      � $  	#FFF0F6FB��6+   $! 0Q�6=   � 5C      $  	#FFD7E2EF��6+   $! 1Q�6=   � 5D      $  	#FFE2EAF3��6+   $	! 0.5Q�6=   � 5E      $  	#FFD5E0ED��6+   $	! 0.5Q�6=   5F      5H      5I      5J      5K      5L      