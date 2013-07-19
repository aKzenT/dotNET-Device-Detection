﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18047
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18047.
// 
#pragma warning disable 1591

namespace Detector.WebService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MobileDeviceSoap", Namespace="http://mobiledevice.51degrees.mobi/")]
    public partial class MobileDevice : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetPropertyOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetPropertiesOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetAllPropertiesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MobileDevice() {
            this.Url = global::Detector.Properties.Settings.Default.Detector_WebService_MobileDevice;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetPropertyCompletedEventHandler GetPropertyCompleted;
        
        /// <remarks/>
        public event GetPropertiesCompletedEventHandler GetPropertiesCompleted;
        
        /// <remarks/>
        public event GetAllPropertiesCompletedEventHandler GetAllPropertiesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://mobiledevice.51degrees.mobi/GetProperty", RequestNamespace="http://mobiledevice.51degrees.mobi/", ResponseNamespace="http://mobiledevice.51degrees.mobi/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetProperty(string propertyName) {
            object[] results = this.Invoke("GetProperty", new object[] {
                        propertyName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetPropertyAsync(string propertyName) {
            this.GetPropertyAsync(propertyName, null);
        }
        
        /// <remarks/>
        public void GetPropertyAsync(string propertyName, object userState) {
            if ((this.GetPropertyOperationCompleted == null)) {
                this.GetPropertyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetPropertyOperationCompleted);
            }
            this.InvokeAsync("GetProperty", new object[] {
                        propertyName}, this.GetPropertyOperationCompleted, userState);
        }
        
        private void OnGetPropertyOperationCompleted(object arg) {
            if ((this.GetPropertyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetPropertyCompleted(this, new GetPropertyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://mobiledevice.51degrees.mobi/GetProperties", RequestNamespace="http://mobiledevice.51degrees.mobi/", ResponseNamespace="http://mobiledevice.51degrees.mobi/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] GetProperties(string[] propertyNames) {
            object[] results = this.Invoke("GetProperties", new object[] {
                        propertyNames});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void GetPropertiesAsync(string[] propertyNames) {
            this.GetPropertiesAsync(propertyNames, null);
        }
        
        /// <remarks/>
        public void GetPropertiesAsync(string[] propertyNames, object userState) {
            if ((this.GetPropertiesOperationCompleted == null)) {
                this.GetPropertiesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetPropertiesOperationCompleted);
            }
            this.InvokeAsync("GetProperties", new object[] {
                        propertyNames}, this.GetPropertiesOperationCompleted, userState);
        }
        
        private void OnGetPropertiesOperationCompleted(object arg) {
            if ((this.GetPropertiesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetPropertiesCompleted(this, new GetPropertiesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://mobiledevice.51degrees.mobi/GetAllProperties", RequestNamespace="http://mobiledevice.51degrees.mobi/", ResponseNamespace="http://mobiledevice.51degrees.mobi/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Property[] GetAllProperties() {
            object[] results = this.Invoke("GetAllProperties", new object[0]);
            return ((Property[])(results[0]));
        }
        
        /// <remarks/>
        public void GetAllPropertiesAsync() {
            this.GetAllPropertiesAsync(null);
        }
        
        /// <remarks/>
        public void GetAllPropertiesAsync(object userState) {
            if ((this.GetAllPropertiesOperationCompleted == null)) {
                this.GetAllPropertiesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllPropertiesOperationCompleted);
            }
            this.InvokeAsync("GetAllProperties", new object[0], this.GetAllPropertiesOperationCompleted, userState);
        }
        
        private void OnGetAllPropertiesOperationCompleted(object arg) {
            if ((this.GetAllPropertiesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllPropertiesCompleted(this, new GetAllPropertiesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18047")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mobiledevice.51degrees.mobi/")]
    public partial class Property {
        
        private string nameField;
        
        private string[] valuesField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string[] Values {
            get {
                return this.valuesField;
            }
            set {
                this.valuesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetPropertyCompletedEventHandler(object sender, GetPropertyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetPropertyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetPropertyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetPropertiesCompletedEventHandler(object sender, GetPropertiesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetPropertiesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetPropertiesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetAllPropertiesCompletedEventHandler(object sender, GetAllPropertiesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllPropertiesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAllPropertiesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Property[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Property[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591