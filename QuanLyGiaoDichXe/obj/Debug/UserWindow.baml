   M S B A M L   `   `   `  ����K  GQuanLyGiaoDichXe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:QuanLyGiaoDichXeQuanLyGiaoDichXe  5   	   U/clr-namespace:QuanLyGiaoDichXe.UserControlLogin!QuanLyGiaoDichXe.UserControlLogin  5	   	   !    QuanLyGiaoDichXe.UserWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:QuanLyGiaoDichXe     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   8uc/clr-namespace:QuanLyGiaoDichXe.UserControlLogin  5	   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5
   	   -   5        $��window��5   	     � 
ResizeMode$  NoResize=�5   	    � WindowStyle$
 None=�5   	   $��Arial)�5   	    
  UserVM � DataContext# [  5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$
 User��5   	   $	��550��6   $	��800��6#   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      6   .+�5   
   � $��0��6   .+�5      4   .QuanLyGiaoDichXe.UserControlLogin.ControlBarUC  
 � Tag 6   �� *Title+ �� ElementName$ window��6K   5      � 5   
   $��1��6    5      �� 5      $
��auto �6    60   �� 5      $
��auto �6    60   �� 5      6"   .+�5!      #  MaterialDesignThemes.Wpf.Card  $��0��6"   $��10q�6/   .��5"      � $��0 0 0 10q�6   .+�5#      �� $��5 5 15 5q�68   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Tên đăng nhập��6J    % !MaterialDesignFloatingHintTextBox#��[ 6y   ��6   �� *
Username+6y   �� 5$      	 �� DisplayMemberPath$	 DisplayName��6   $ Quyền��6�   
 �� 
IsEditable
 .  & "MaterialDesignFloatingHintComboBox#��[ 6�   ��6?   �� *
UserRole+ �� SelectedItem 6`   �� *SelectedUserRole+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�63  �� 5%      $��5 5 15 5q�6;   $ Tên hiển thị��6M   #��[ 6{   ��6   �� *DisplayName+6{   �� 5&      $��5 5 15 5q�68   $ Mật khẩu��6J   #��[ 6t   ��6   �� *
Password+6t   �� 5'      $��5 5 15 5q�6:   $ Nhập Lại Mật khẩu��6L   #��[ 6   ��6   �� *
RePassword+6   5(      5)        5*      $��1��6"   $��5q�6/   .��5+      � .+�5,      �� $��Sửa��6=   $��5q�6K   $	��100��6V   ��6   �� *EditCommand+6d   �� 5-      $
��Xóa��6?   $��5q�6M   $	��100��6X   ��6   �� *DeleteCommand+6f   �� 5.      $��Đổi mật khẩu��6G   $��5q�6^   $	��150��6i   ��6   �� *ChangePasswordCommand+6w   5/      50        52      $��2��6"   $��10q�6/   $��Stretch=�6;   .��53      � .+�54      �� ��6   �� *List+ 6<   �� *SelectedItem+$ OneWayToSource=� �� View 55      �� 56      .P�57   "   �� $��Id��61    �� DisplayMemberBinding 6=   �� *Id+�� 58   "   $��Tên đăng nhập��61    6H   �� *
Username+�� 59   "   $��Chức vụ��61    6B   �� *UserRole.DisplayName+�� 5:   "   $��Tên hiển thị��61    6G   �� *DisplayName+5;      5=      5>      5?      5@      5A      5B      