﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.50826.0
// 
namespace WCFClient.SilverlightResourceWCFService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SilverlightResource.ResourceInfo", Namespace="http://schemas.datacontract.org/2004/07/WCFClient.Web")]
    public partial struct SilverlightResourceResourceInfo : System.ComponentModel.INotifyPropertyChanged {
        
        private string AuthorField;
        
        private string ImageField;
        
        private string TitleField;
        
        private string TypeField;
        
        private string URLField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Author {
            get {
                return this.AuthorField;
            }
            set {
                if ((object.ReferenceEquals(this.AuthorField, value) != true)) {
                    this.AuthorField = value;
                    this.RaisePropertyChanged("Author");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Image {
            get {
                return this.ImageField;
            }
            set {
                if ((object.ReferenceEquals(this.ImageField, value) != true)) {
                    this.ImageField = value;
                    this.RaisePropertyChanged("Image");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title {
            get {
                return this.TitleField;
            }
            set {
                if ((object.ReferenceEquals(this.TitleField, value) != true)) {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Type {
            get {
                return this.TypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TypeField, value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string URL {
            get {
                return this.URLField;
            }
            set {
                if ((object.ReferenceEquals(this.URLField, value) != true)) {
                    this.URLField = value;
                    this.RaisePropertyChanged("URL");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="SilverlightResourceWCFService.SilverlightResource")]
    public interface SilverlightResource {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:SilverlightResource/GetResource", ReplyAction="urn:SilverlightResource/GetResourceResponse")]
        System.IAsyncResult BeginGetResource(int id, System.AsyncCallback callback, object asyncState);
        
        WCFClient.SilverlightResourceWCFService.SilverlightResourceResourceInfo EndGetResource(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SilverlightResourceChannel : WCFClient.SilverlightResourceWCFService.SilverlightResource, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetResourceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetResourceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public WCFClient.SilverlightResourceWCFService.SilverlightResourceResourceInfo Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((WCFClient.SilverlightResourceWCFService.SilverlightResourceResourceInfo)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SilverlightResourceClient : System.ServiceModel.ClientBase<WCFClient.SilverlightResourceWCFService.SilverlightResource>, WCFClient.SilverlightResourceWCFService.SilverlightResource {
        
        private BeginOperationDelegate onBeginGetResourceDelegate;
        
        private EndOperationDelegate onEndGetResourceDelegate;
        
        private System.Threading.SendOrPostCallback onGetResourceCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public SilverlightResourceClient() {
        }
        
        public SilverlightResourceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SilverlightResourceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SilverlightResourceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SilverlightResourceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetResourceCompletedEventArgs> GetResourceCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult WCFClient.SilverlightResourceWCFService.SilverlightResource.BeginGetResource(int id, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetResource(id, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WCFClient.SilverlightResourceWCFService.SilverlightResourceResourceInfo WCFClient.SilverlightResourceWCFService.SilverlightResource.EndGetResource(System.IAsyncResult result) {
            return base.Channel.EndGetResource(result);
        }
        
        private System.IAsyncResult OnBeginGetResource(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int id = ((int)(inValues[0]));
            return ((WCFClient.SilverlightResourceWCFService.SilverlightResource)(this)).BeginGetResource(id, callback, asyncState);
        }
        
        private object[] OnEndGetResource(System.IAsyncResult result) {
            WCFClient.SilverlightResourceWCFService.SilverlightResourceResourceInfo retVal = ((WCFClient.SilverlightResourceWCFService.SilverlightResource)(this)).EndGetResource(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetResourceCompleted(object state) {
            if ((this.GetResourceCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetResourceCompleted(this, new GetResourceCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetResourceAsync(int id) {
            this.GetResourceAsync(id, null);
        }
        
        public void GetResourceAsync(int id, object userState) {
            if ((this.onBeginGetResourceDelegate == null)) {
                this.onBeginGetResourceDelegate = new BeginOperationDelegate(this.OnBeginGetResource);
            }
            if ((this.onEndGetResourceDelegate == null)) {
                this.onEndGetResourceDelegate = new EndOperationDelegate(this.OnEndGetResource);
            }
            if ((this.onGetResourceCompletedDelegate == null)) {
                this.onGetResourceCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetResourceCompleted);
            }
            base.InvokeAsync(this.onBeginGetResourceDelegate, new object[] {
                        id}, this.onEndGetResourceDelegate, this.onGetResourceCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override WCFClient.SilverlightResourceWCFService.SilverlightResource CreateChannel() {
            return new SilverlightResourceClientChannel(this);
        }
        
        private class SilverlightResourceClientChannel : ChannelBase<WCFClient.SilverlightResourceWCFService.SilverlightResource>, WCFClient.SilverlightResourceWCFService.SilverlightResource {
            
            public SilverlightResourceClientChannel(System.ServiceModel.ClientBase<WCFClient.SilverlightResourceWCFService.SilverlightResource> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetResource(int id, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = id;
                System.IAsyncResult _result = base.BeginInvoke("GetResource", _args, callback, asyncState);
                return _result;
            }
            
            public WCFClient.SilverlightResourceWCFService.SilverlightResourceResourceInfo EndGetResource(System.IAsyncResult result) {
                object[] _args = new object[0];
                WCFClient.SilverlightResourceWCFService.SilverlightResourceResourceInfo _result = ((WCFClient.SilverlightResourceWCFService.SilverlightResourceResourceInfo)(base.EndInvoke("GetResource", _args, result)));
                return _result;
            }
        }
    }
}
