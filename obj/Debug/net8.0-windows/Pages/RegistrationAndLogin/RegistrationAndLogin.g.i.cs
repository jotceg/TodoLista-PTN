﻿#pragma checksum "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0E39FF3CF79B069D93CD67B61696F85EBB99D4D5"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TodoLista.Pages.Home;


namespace TodoLista.Pages.RegistrationAndLogin {
    
    
    /// <summary>
    /// RegistrationAndLogin
    /// </summary>
    public partial class RegistrationAndLogin : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LoginTextBox;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dwgCustomers;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordBox;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RegisterBtn;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LoginBtn;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ClearDataBtn;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox RememberMyDataCheckBox;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ClearDataBtn_Copy;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TodoLista;component/pages/registrationandlogin/registrationandlogin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LoginTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.dwgCustomers = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.PasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 4:
            this.RegisterBtn = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
            this.RegisterBtn.Click += new System.Windows.RoutedEventHandler(this.RegisterBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.LoginBtn = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
            this.LoginBtn.Click += new System.Windows.RoutedEventHandler(this.LoginBtn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ClearDataBtn = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
            this.ClearDataBtn.Click += new System.Windows.RoutedEventHandler(this.ClearDataBtn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.RememberMyDataCheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.ClearDataBtn_Copy = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\..\..\Pages\RegistrationAndLogin\RegistrationAndLogin.xaml"
            this.ClearDataBtn_Copy.Click += new System.Windows.RoutedEventHandler(this.ClearLoginStateDataBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

