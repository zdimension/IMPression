   M S B A M L   `   `   `  ����p  lSystem.Windows.Controls.Input.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35pTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.ToolkitSystem.Windows.Controls  5       
IMPressive"   IMPressive.PointsGraphWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      icontrolsTclr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Input.Toolkit     5      X TXceed.Wpf.Toolkit, Version=2.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      W SOxyPlot.Wpf, Version=2015.1.710.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  5      -   5          �Name$  pointsGraphWindow��5   9   $	��500��5      $	��735��6    � Icon$ icon.ico��6S    � WindowStartupLocation$ CenterOwner=�5	      .��5
      �    MainGrid#���   6   .+�5      � 
�������$��	0,52,0,25q�6   .+�5        OxyPlot.Wpf.PlotView  -   5          Name$ chartCanvas��5      $��	222,0,0,0q�6'      Model 6:   �� *
GraphModel+ �� ElementName$ pointsGraphWindow���� 5      -   5         ��Name$ listBox��5      $��0q�6   $	��222��6)   $
��Left=�65   ��5      �� 5      .R�5      � .+�5   	   �� $
��X : ��6   �� 5   	   $
��Bold"�6&   ��6   �� *X+6&   �� 5   	   $��; ��6   �� 5   	   $
��Y : ��6   �� 5   	   $
��Bold"�6&   ��6   �� *Y+6&   �� 5   	    ��Name$ btnDelPt��6   -   5        $��X��5   "   $��Right=�6.   6J   5   	   5      5      5      � 5      $��	0,10,84,0q�6	   $��26��6   $	��Top=�6(    � ColumnDefinitions 5      �� 5      $
��Auto �6   �� 5      $��* �6   �� 5       $
��Auto �6   �� 5!      $��* �6   .+�5$      �� -   5        	 ��Name$	 label��5$      $	��X :��6   $
��Left=�6(   $	��Top=�6C   $��0��6[   $��13.333(�6k   �� 5%      $��1q�6   
 �� Padding$
 5q�6    ���	#FF8696AA �� CornerRadius$ 2��6D   $��26��6U    J� SnapsToDevicePixels .  $	��Top=�5&   "   $��1��6:   ._�5'      -   'System.Windows.Controls.AutoCompleteBox  -   5          Name$
 txtX��5'      $��24��6-   $��-5q�69   $	��Top=�6E   $��Cambria Math)�5(      $��18(�6!   ���	#FF333333$��0,2,3,0q�6F   $��0q�5)         Text$  ��62   $��Stretch=�5*      ��5)      Z� ��5+      �� 5,       �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6-   .��5-   	   �  � Color$ 	#FFE5EEFB��6    � Offset$ 0Q�6(   � 5.   	   $ White��6   $ 1Q�6$   5/   	   51      52      �� 54      -   5        $	 label1��54      $	��Y :��6   $
��Left=�6)   $	��Top=�6D   $��2��6\   $��13.333(�6l   �� 55      $��1q�6   $
 5q�6    ���	#FF8696AA$ 2��6D   $��26��6U    .  $	��Top=�56   "   $��3��6:   ._�57        -   5        $
 txtY��57      $��24��6-   $��-5q�69   $	��Top=�6E   $��Cambria Math)�58      $��18(�6!   ���	#FF333333$��0,2,3,0q�6F   $��0q�59      $  ��62   $��Stretch=�5:      ��59      Z� ��5;      �� 5<      $ 0.5,1&�6   $ 0.5,0&�6-   .��5=   	   � $ 	#FFE5EEFB��6   $ 0Q�6(   � 5>   	   $ White��6   $ 1Q�6$   5?   	   5A      5B      5C      �� 5D      -	   5        $ btnLoad��5D      $��OK��6   $��Right=�6)   $��0,10,3,0q�6E   $	��Top=�6W   $��76��5E      ���	#FF8696AA  NormalButton#���  6(   $��26��6O   $
��Left=�5F      $	��Top=�6'   ���	#FF1E395B6F   �� 5G      -
   5         ��Name$ cbxInterpolate��5G      $$��Afficher la courbe interpolée��6%   $
��Left=�6M   $��	217,0,0,5q�5H      $��Bottom=�6   5I      �� 5J      -   5        $ 	cbxNormal��5J      $ ��Afficher la courbe de base��6    $
��Left=�6E   $��10,0,0,5q�6`   $��Bottom=�5K       d� 	IsChecked$
 TrueY�6^   5L      5M      