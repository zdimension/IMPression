   M S B A M L   `   `   `  ���� d  `System.Windows.Controls.Input.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=nullpTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.ToolkitSystem.Windows.Controls   
IMPressive   IMPressive.GraphWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     icontrolsTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.Toolkit     X TXceed.Wpf.Toolkit, Version=2.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  -   $	��500��$	��725��  � 
ResizeMode$  CanMinimize=� � Icon$ icon.ico�� � WindowStartupLocation$ CenterOwner=� � 	Resources    FormCanvasGridTile j� Stretch j� TileMode j� Viewport j� ViewportUnits L� Drawing	 � Geometry
 �� 
StartPoint �� EndPoint
 � Pen >� 	Thickness >� Brush � Brush��%�   &	        L� $
 None=�$
 Tile=�$ 	0,0,30,30�$ Absolute=� � 	 � .���� $	
 0,0&�$
 30,0&��� $

 30,0&�$ 30,30&��� $
 30,30&�$
 0,30&��� $

 0,30&�$	 0,0&� >� $ 1Q�
 ����� ��	White��.��� .+��� -    ��Name$ chartCanvas��$��0,52,0,0q�#��[  z��� -   st���� -    ��Name$ btnLoad��$��OK��$
��Left=�$��
556,10,0,0q�$	��Top=�$��76�����	#FF8696AA  NormalButton#��[ $��26��$
��Left=�$	��Top=����	#FF1E395B�� $��1q�$��	10,10,0,0q� �� Padding$ 5q����	#FF8696AA �� CornerRadius$ 2��$��26��
�������$	��455�� J� SnapsToDevicePixels .  $
��Left=�$	��Top=�._�-   'System.Windows.Controls.AutoCompleteBox  -     Name$ txtOperation��$
��Left=�$��24��$��-5q�$	��Top=�$	��454��$��Cambria Math)�$��18(����	#FF333333$��0,2,3,0q�$��0q�   Text$ cos(x)����Z� ����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FFE5EEFB�� � Offset$ 0Q�� $ White��$ 1Q��� -   $ btnClear��$��Effacer��$
��Left=�$��
637,10,0,0q�$	��Top=�$��76�����	#FF8696AA#��[ $��26��$
��Left=�$	��Top=����	#FF1E395B�� $��1q�$��
470,10,0,0q�$ 0q����	#FF8696AA$ 2��$��26��$��81�� .  $
��Left=�$	��Top=�._��� -    ��Name$	 bg2��$��1q�
�������$ 1��._�#  Xceed.Wpf.Toolkit.ColorPicker  -     Name$ cpxColor��$
��Left=�$	��Top=�$��24��$��80��$��-1,-1,-2,-1q�   SelectedColor$	 Red��   AvailableColorsHeader$ Couleurs disponibles��   StandardColorsHeader$ Couleurs standards��    RecentColorsHeader$  Couleurs récentes��!   ShowAdvancedButton! .  ���	#FF1E395B��Z� ���� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFF0F6FB��$ 0Q�� $ 	#FFD7E2EF��$ 1Q�� $ 	#FFE2EAF3��$	 0.5Q�� $ 	#FFD5E0ED��$	 0.5Q�