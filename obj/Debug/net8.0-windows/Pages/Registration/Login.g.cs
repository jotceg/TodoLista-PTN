﻿#pragma checksum "..\..\..\..\..\Pages\Registration\Login.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D8A08540D635EE6C5A79AFF5EB2EF2A3E2260D48"
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
        
        
        #line 16 "..\..\..\..\..\Pages\Registration\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LoginTextBox;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\..\Pages\Registration\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordTextBox;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\..\Pages\Registration\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PasswordTextBoxVisible;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\..\Pages\Registration\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ShowPasswordCheckBox;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\..\Pages\Registration\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox RememberMyDataCheckBox;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\Pages\Registration\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LoginBtn;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\..\Pages\Registration\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RegisterBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TodoLista;component/pages/registration/login.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\Registration\Login.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
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
            this.PasswordTextBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 3:
            this.PasswordTextBoxVisible = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.ShowPasswordCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 28 "..\..\..\..\..\Pages\Registration\Login.xaml"
            this.ShowPasswordCheckBox.Checked += new System.Windows.RoutedEventHandler(this.ShowPasswordCheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 28 "..\..\..\..\..\Pages\Registration\Login.xaml"
            this.ShowPasswordCheckBox.Unchecked += new System.Windows.RoutedEventHandler(this.ShowPasswordCheckBox_Unchecked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RememberMyDataCheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.LoginBtn = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\..\..\Pages\Registration\Login.xaml"
            this.LoginBtn.Click += new System.Windows.RoutedEventHandler(this.LoginBtn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.RegisterBtn = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\..\..\Pages\Registration\Login.xaml"
            this.RegisterBtn.Click += new System.Windows.RoutedEventHandler(this.RegisterBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
