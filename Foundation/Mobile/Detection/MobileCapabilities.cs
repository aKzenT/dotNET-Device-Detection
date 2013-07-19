/* *********************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0.
 * 
 * If a copy of the MPL was not distributed with this file, You can obtain
 * one at http://mozilla.org/MPL/2.0/.
 * 
 * This Source Code Form is “Incompatible With Secondary Licenses”, as
 * defined by the Mozilla Public License, v. 2.0.
 * ********************************************************************* */

#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using FiftyOne.Foundation.Mobile.Detection.Matchers;

#if VER4

using System.Linq;


#endif

#endregion

namespace FiftyOne.Foundation.Mobile.Detection
{
    /// <summary>
    /// Enhanced mobile capabilities assigned to mobile devices.
    /// </summary>
    internal class MobileCapabilities
    {
        #region String Index Values

        // Gets the indexes of all the key capability strings as static readonly
        // values during static construction to avoid needing to look them up every time.
        private readonly int AjaxRequestType;
        private readonly int AjaxRequestTypeNotSupported;
        private readonly int Javascript;
        private readonly int JavascriptVersion;
        private readonly int CookiesCapable;
        private readonly int BrowserVersion;
        private readonly int BrowserName;
        private readonly int PlatformName;
        private readonly int Adapters;
        private readonly int ScreenPixelsHeight;
        private readonly int ScreenPixelsWidth;
        private readonly int BitsPerPixel;
        private readonly int HardwareName;
        private readonly int HardwareModel;
        private readonly int HardwareVendor;
        private readonly int HtmlVersion;
        private readonly int IsMobile;
        private readonly int[] True = new int[2];
        private readonly int[] False = new int[2];
        private readonly int IsCrawler;
        private readonly int CcppAccept;
        private readonly int[] ImagePng;
        private readonly int[] ImageJpeg;
        private readonly int[] ImageGif;
        private readonly int XHtmlVersion;
        private readonly int TablesCapable;

        #endregion
        
        #region Fields

        /// <summary>
        /// Instance of the provider to use with this class.
        /// </summary>
        private Provider _provider = null;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the instance of the provider being used.
        /// </summary>
        internal Provider Provider
        {
            get { return _provider; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of <see cref="MobileCapabilities"/>.
        /// </summary>
        /// <param name="provider">Data provider to use with this capabilities provider.</param>
        internal MobileCapabilities(Provider provider)
        {
            _provider = provider;
            True[0] = _provider.Strings.Add("True");
            True[1] = _provider.Strings.Add("true");
            False[0] = _provider.Strings.Add("False");
            False[1] = _provider.Strings.Add("false");
            AjaxRequestType = _provider.Strings.Add("AjaxRequestType");
            AjaxRequestTypeNotSupported = _provider.Strings.Add("AjaxRequestTypeNotSupported");
            Javascript = _provider.Strings.Add("Javascript");
            JavascriptVersion = _provider.Strings.Add("JavascriptVersion");
            CookiesCapable = _provider.Strings.Add("CookiesCapable");
            BrowserVersion = _provider.Strings.Add("BrowserVersion");
            BrowserName = _provider.Strings.Add("BrowserName");
            PlatformName = _provider.Strings.Add("PlatformName");
            Adapters = _provider.Strings.Add("Adapters");
            ScreenPixelsHeight = _provider.Strings.Add("ScreenPixelsHeight");
            ScreenPixelsWidth = _provider.Strings.Add("ScreenPixelsWidth");
            BitsPerPixel = _provider.Strings.Add("BitsPerPixel");
            HardwareName = _provider.Strings.Add("HardwareName");
            HardwareModel = _provider.Strings.Add("HardwareModel");
            HardwareVendor = _provider.Strings.Add("HardwareVendor");
            HtmlVersion = _provider.Strings.Add("HtmlVersion");
            XHtmlVersion = _provider.Strings.Add("XHtmlVersion");
            IsMobile = _provider.Strings.Add("IsMobile");
            IsCrawler = _provider.Strings.Add("IsCrawler");
            TablesCapable = _provider.Strings.Add("TablesCapable");
            CcppAccept = _provider.Strings.Add("CcppAccept");
            ImageGif = new int[] { 
                _provider.Strings.Add("image/gif") };
            ImagePng = new int[] { 
                _provider.Strings.Add("image/png") };
            ImageJpeg = new int[] { 
                _provider.Strings.Add("image/jpeg"),
                _provider.Strings.Add("image/jpg") };
        }

        #endregion

        #region Create Methods

        /// <summary>
        /// Creates a dictionary of capabilites for the requesting device.
        /// </summary>
        /// <param name="headers">A collection of Http headers from the device.</param>
        /// <param name="currentCapabilities">The current capabilities assigned by .NET.</param>
        /// <returns>A dictionary of capabilities for the request.</returns>
        internal IDictionary Create(NameValueCollection headers, IDictionary currentCapabilities)
        {
            // Use the base class to create the initial list of capabilities.
            IDictionary capabilities = new Hashtable();

            // Get the device.
            int start = Environment.TickCount;
            Result result = _provider.GetResult(headers);
            int detectionTime = Environment.TickCount - start + 1;

            // Add the capabilities for the device.
            Create(result, capabilities, currentCapabilities);

            if (capabilities[Constants.FiftyOneDegreesProperties] is SortedList<string, List<string>>)
            {
                // Add the detection time to the list of properties.
                ((SortedList<string, List<string>>)capabilities[Constants.FiftyOneDegreesProperties])
                    .Add(Constants.DetectionTimeProperty,
                    new List<string>(new string[] {
                    detectionTime.ToString() 
                }));

                // Add the handler confidence to the list of properties.
                ((SortedList<string, List<string>>)capabilities[Constants.FiftyOneDegreesProperties])
                    .Add(Constants.ConfidenceProperty,
                    new List<string>(new string[] {
                    result.Confidence.ToString()
                }));

                // Add the difference to the list of properties.
                ((SortedList<string, List<string>>)capabilities[Constants.FiftyOneDegreesProperties])
                    .Add(Constants.DifferenceProperty,
                    new List<string>(new string[] {
                    String.Format(
                        "{0:#.###}",
                        result.Difference)
                }));
            }

            // Initialise any capability values that rely on the settings
            // from the device data source.
            Init(capabilities);

            return capabilities;
        }

        /// <summary>
        /// Creates a dictionary of capabilites for the useragent string.
        /// </summary>
        /// <param name="userAgent">The useragent string associated with the device.</param>
        /// <returns>A dictionary of capabilities for the request.</returns>
        internal IDictionary Create(string userAgent)
        {
            // Create the mobile capabilities hashtable.
            IDictionary capabilities = new Hashtable();

            // As we can't tell from the headers if javascript is supported assume
            // it can be and that the inheriting class will provide the additional details.
            SetJavaScript(capabilities, true);

            // Set the headers and return the new capabilities collection.
            NameValueCollection headers = new NameValueCollection();
            headers.Add("User-Agent", userAgent);
            return Create(headers, capabilities);
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Initialises the IDictionary of capabilities.
        /// </summary>
        protected virtual void Init(IDictionary capabilities)
        {
            // Set the tagwriter.
            capabilities["tagwriter"] = GetTagWriter(capabilities);
        }

        #endregion

        #region Private Methods

        private void Create(Result result, IDictionary properties, IDictionary currentProperties)
        {
            // Enhance with the capabilities from the device data.
            if (result != null)
            {
                
                // Enhance the default capabilities collection based on the device.
                Enhance(properties, currentProperties, result);

                // Add the 51Degrees.mobi device properties to the collection.
                properties.Add(Constants.FiftyOneDegreesProperties, result.GetAllProperties());

                // If an adapters patch file has been loaded then include this
                // capability in the exposed list of capabilities.
                string adapters = GetAdapters(result);
                if (String.IsNullOrEmpty(adapters) == false)
                    SetValue(properties, "adapters", adapters);
            }
        }

        /// <summary>
        /// Sets static capabilities used by mobile controls.
        /// </summary>
        /// <param name="capabilities">Dictionary of capabilities to be changed.</param>
        private static void SetStaticValues(IDictionary capabilities)
        {
            SetValue(capabilities, "requiresSpecialViewStateEncoding", "true");
            SetValue(capabilities, "requiresUniqueFilePathSuffix", "true");
            SetValue(capabilities, "requiresUniqueHtmlCheckboxNames", "true");
            SetValue(capabilities, "requiresUniqueHtmlInputNames", "true");
            SetValue(capabilities, "requiresUrlEncodedPostfieldValues", "true");
            SetValue(capabilities, "requiresOutputOptimization", "true");
            SetValue(capabilities, "requiresControlStateInSession", "true");
        }

        /// <summary>
        /// Updates the capabilities used by Microsoft's implementation of the
        /// HttpBrowserCapabilities class to control the property values it
        /// returns. Only properties exposed by FiftyOneBrowserCapabilities are overriden
        /// by this method.
        /// </summary>
        /// <param name="capabilities">Dictionary of capabilities to be enhanced.</param>
        /// <param name="currentCapabilities">Dictionary of existing capabilities for the device.</param>
        /// <param name="result">The match result to use for the enhancement.</param>
        private void Enhance(IDictionary capabilities, IDictionary currentCapabilities, Result result)
        {
            // Set base capabilities for all mobile devices.
            SetStaticValues(capabilities);

            SetValue(capabilities, "isMobileDevice", GetIsMobileDevice(result));
            SetValue(capabilities, "crawler", GetIsCrawler(result));
            SetValue(capabilities, "mobileDeviceModel", GetMobileDeviceModel(result));
            SetValue(capabilities, "mobileDeviceManufacturer", GetMobileDeviceManufacturer(result));
            SetValue(capabilities, "platform", GetPlatform(result));
            // property enhancement can be removed with this compiler flag
#if !REMOVE_OVERRIDE_BROWSER
            SetValue(capabilities, "browser", GetBrowser(result));
#endif
            SetValue(capabilities, "type", capabilities["mobileDeviceManufacturer"]);
            SetValue(capabilities, "screenPixelsHeight", GetScreenPixelsHeight(result) ??
                GetDefaultValue("screenPixelsHeight", currentCapabilities));
            SetValue(capabilities, "screenPixelsWidth", GetScreenPixelsWidth(result) ??
                GetDefaultValue("screenPixelsWidth", currentCapabilities));
            SetValue(capabilities, "screenBitDepth", GetBitsPerPixel(result));
            SetValue(capabilities, "preferredImageMime", GetPreferredImageMime(result));
            SetValue(capabilities, "isColor", GetIsColor(result));
            SetValue(capabilities, "supportsCallback", GetSupportsCallback(result));
            SetValue(capabilities, "SupportsCallback", GetSupportsCallback(result));
            SetValue(capabilities, "canInitiateVoiceCall", GetIsMobileDevice(result));
            SetValue(capabilities, "jscriptversion", GetJavascriptVersion(result));

            // The following values are set to prevent exceptions being thrown in
            // the standard .NET base classes if the property is accessed.
            SetValue(capabilities, "screenCharactersHeight", 
                GetDefaultValue("screenCharactersHeight", currentCapabilities));
            SetValue(capabilities, "screenCharactersWidth",
                GetDefaultValue("screenCharactersWidth", currentCapabilities));

            // Use the Version class to find the version. If this fails use the 1st two
            // decimal segments of the string.
            string versionString = _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(BrowserVersion));
            if (String.IsNullOrEmpty(versionString) == false)
            {
                try
                {
                    Version version = new Version(versionString);
                    SetValue(capabilities, "majorversion", version.Major.ToString());
                    SetValue(capabilities, "minorversion", String.Format(".{0}", version.Minor));
                    SetValue(capabilities, "version", version.ToString());
                }
                catch (FormatException)
                {
                    SetVersion(capabilities, versionString);
                }
                catch (ArgumentException)
                {
                    SetVersion(capabilities, versionString);
                }
            }
            else
            {
                // Transfer the current version capabilities to the new capabilities.
                SetValue(capabilities, "majorversion", currentCapabilities != null ? currentCapabilities["majorversion"] : null);
                SetValue(capabilities, "minorversion", currentCapabilities != null ? currentCapabilities["minorversion"] : null);
                SetValue(capabilities, "version", currentCapabilities != null ? currentCapabilities["version"] : null);

                // Ensure the version values are not null to prevent null arguement exceptions
                // with some controls.
                versionString = currentCapabilities != null ? currentCapabilities["version"] as string : "0.0";
                SetVersion(capabilities, versionString);
            }

            // All we can determine from the device database is if javascript is supported as a boolean.
            // If the value is not provided then null is returned and the capabilities won't be altered.
            object javaScript = GetJavascriptSupport(result);
            if (javaScript is bool)
            {
                SetJavaScript(capabilities, (bool)javaScript);
                SetValue(capabilities, "ecmascriptversion",
                         (bool)javaScript ? "3.0" : "0.0");
            }

            // Sets the W3C DOM version.
            SetValue(capabilities, "w3cdomversion",
                GetW3CDOMVersion(result,
                    currentCapabilities != null
                        ? (string)currentCapabilities["w3cdomversion"]
                        : String.Empty));

            // Update the cookies value if we have additional information.
            SetValue(capabilities, "cookies",
                    GetCookieSupport(result,
                                     currentCapabilities != null
                                         ? (string)currentCapabilities["cookies"]
                                         : String.Empty));

            // Only set these values from 51Degrees.mobi if they've not already been set from
            // the Http request header, or the .NET solution.
            if (capabilities.Contains("preferredRenderingType") == false)
            {
                // Set the rendering type for the response.
                SetValue(capabilities, "preferredRenderingType", GetPreferredHtmlVersion(result));

                // Set the Mime type of the response.
                SetValue(capabilities, "preferredRenderingMime", "text/html");
            }
        }

        /// <summary>
        /// Returns true if the device supports tables.
        /// </summary>
        /// <param name="result">The match result for the current request.</param>
        /// <returns></returns>
        private object GetTablesCapable(Result result)
        {
            int value = result.GetFirstPropertyValueStringIndex(TablesCapable);
            if (value < 0)
                return null;

            if (value == this.True[0] || value == this.True[1])
                return bool.TrueString.ToLowerInvariant();
            return bool.FalseString.ToLowerInvariant(); 
        }

        private string GetPreferredHtmlVersion(Result result)
        {
            // Working out ASP.NET will support HTML5. Return 4 for the moment.
            return "html4";

            /*

            // Get the list of values.
            var values = new List<double>();
            var versions = device.GetPropertyValueStringIndexes(HtmlVersion);
            if (versions != null)
            {
                foreach (var index in versions)
                {
                    double value;
                    if (double.TryParse(_provider.Strings.Get(index), out value))
                        values.Add(value);
                }
            }
            values.Sort();
            values.Reverse();

            // Find the highest version of HTML supported.
            foreach(double value in values)
            {
                switch (value.ToString())
                {
                    default:
                    case "4":
                        return "html4";
                    case "3.2":
                        return "html32";
                    case "5":
                        return "html5";
                }
            }
 
            // Couldn't find anything return html 4.
            return "html4";
            */
        }

        /// <summary>
        /// Sets the version using a regular expression to find numeric segments of
        /// the provided version string. If the version already exists in the
        /// new dictionary of capabilities a new value will not be written.
        /// </summary>
        /// <param name="capabilities"></param>
        /// <param name="version"></param>
        private static void SetVersion(IDictionary capabilities, string version)
        {
            if (version != null)
            {
                MatchCollection segments = Regex.Matches(version, @"\d+");
                string majorVersion = segments.Count > 0 ? segments[0].Value : "0";
                string minorVersion = segments.Count > 1 ? segments[1].Value : "0";
                if (String.IsNullOrEmpty(capabilities["majorversion"] as string))
                    SetValue(capabilities, "majorversion", majorVersion);
                if (String.IsNullOrEmpty(capabilities["minorversion"] as string))
                    SetValue(capabilities, "minorversion", minorVersion);
                if (String.IsNullOrEmpty(capabilities["version"] as string))
                    SetValue(capabilities, "version", String.Format("{0}.{1}", majorVersion, minorVersion));
            }
        }

        private string GetCookieSupport(Result result, string current)
        {
            bool value = false;
            // Return either the capability or the current value as a boolean string.
            if (bool.TryParse(_provider.Strings.Get(result.GetFirstPropertyValueStringIndex(CookiesCapable)), out value) == false)
                bool.TryParse(current, out value);
            return value.ToString();
        }

        /// <summary>
        /// Returns true if the device supports callbacks from the browser.
        /// </summary>
        /// <param name="result">The match result for the current request.</param>
        /// <returns>True if callback is supported.</returns>
        private string GetSupportsCallback(Result result)
        {
            List<int> values = result.GetPropertyValueStringIndexes(AjaxRequestType);
            if (values != null && values.Contains(AjaxRequestTypeNotSupported))
                return bool.FalseString.ToLowerInvariant();
            return bool.TrueString.ToLowerInvariant();
        }

        /// <summary>
        /// Returns version 1.0 if DOM is supported based on Ajax
        /// being supported, otherwise returns false.
        /// </summary>
        /// <param name="result">The match result for the current request.</param>
        /// <param name="current">The current value of the property.</param>
        /// <returns>1.0, 0.0 or the current value.</returns>
        private string GetW3CDOMVersion(Result result, string current)
        {
            Version version = new Version(0, 0);

            // Set the version to the current version.
            try
            {
                version = new Version(current);
            }
            catch (ArgumentException)
            {
                // Do nothing and let the default value be returned.
            }

            // Try and set version 1.0 if ajax is supported.
            List<int> values = result.GetPropertyValueStringIndexes(AjaxRequestType);
            if (values != null && values.Contains(AjaxRequestTypeNotSupported) == false)
                version = new Version("2.0.0.0");

            return version.ToString(2);
        }

        /// <summary>
        /// If the device indicates javascript support then return true.
        /// </summary>
        /// <param name="result">The match result for the current request.</param>
        /// <returns>True if javascript is supported.</returns>
        private object GetJavascriptSupport(Result result)
        {
            int value = result.GetFirstPropertyValueStringIndex(Javascript);
            if (value < 0)
                return null;
            return value == this.True[0] || value == this.True[1];
        }

        /// <summary>
        /// Get the javascript version or null if not provided or invalid.
        /// </summary>
        /// <param name="result">The match result for the current request.</param>
        /// <returns></returns>
        private string GetJavascriptVersion(Result result)
        {
            int index = result.GetFirstPropertyValueStringIndex(JavascriptVersion);
            if (index < 0)
                return null;

            string value = _provider.Strings.Get(index);

            // Check if the version value is valid in the version
            // class. If not then return null.
#if VER4
            Version version;
            if (Version.TryParse(value, out version))
                return value;
            return null;
#else
            try
            {
                new Version(value);
                return value;
            }
            catch
            {
                return null;
            }
#endif
        }

        private string GetPlatform(Result result)
        {
            return _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(PlatformName));
        }

        private string GetBrowser(Result result)
        {
            return _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(BrowserName));
        }

        /// <summary>
        /// If the data set does not contain the IsCrawler property null is returned.
        /// If it is present and contains the value true or false then a value
        /// is returned.
        /// </summary>
        /// <param name="result">The match result for the current request.</param>
        /// <returns></returns>
        private string GetIsCrawler(Result result)
        {
            int value = result.GetFirstPropertyValueStringIndex(IsCrawler);
            if (value == this.True[0] || value == this.True[1])
                return bool.TrueString.ToLowerInvariant();
            if (value == this.False[0] || value == this.False[1])
                return bool.FalseString.ToLowerInvariant();
            return null;
        }

        private string GetAdapters(Result result)
        {
            return _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(Adapters));
        }

        private string GetIsMobileDevice(Result result)
        {
            int value = result.GetFirstPropertyValueStringIndex(IsMobile);
            if (value == this.True[0] || value == this.True[1])
                return bool.TrueString.ToLowerInvariant();
            return bool.FalseString.ToLowerInvariant();
        }

        private string GetScreenPixelsHeight(Result result)
        {
            int size;
            string value = _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(ScreenPixelsHeight));
            if (int.TryParse(value, out size))
                return value;
            return null;
        }

        private string GetScreenPixelsWidth(Result result)
        {
            int size;
            string value = _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(ScreenPixelsWidth));
            if (int.TryParse(value, out size))
                return value;
            return null;
        }

        private string GetIsColor(Result result)
        {
            long bitsPerPixel = GetBitsPerPixel(result);
            if (bitsPerPixel >= 4)
                return bool.TrueString.ToLowerInvariant();
            return bool.FalseString.ToLowerInvariant();
        }

        private string GetMobileDeviceModel(Result result)
        {
            string value = _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(HardwareModel));
            if (String.IsNullOrEmpty(value))
                value = _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(HardwareName));
            return value;
        }

        private string GetMobileDeviceManufacturer(Result result)
        {
            return _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(HardwareVendor));
        }

        private string GetPreferredImageMime(Result result)
        {
            List<int> mimeTypes = result.GetPropertyValueStringIndexes(CcppAccept);
            // Look at the database and return the 1st one that matches in order
            // of preference.
            if (Contains(mimeTypes, ImagePng))
                return "image/png";
            if (Contains(mimeTypes, ImageJpeg))
                return "image/jpeg";
            if (Contains(mimeTypes, ImageGif))
                return "image/gif";
            return null;
        }

        /// <summary>
        /// Compares two lists to see if they contain at least one value that is the same.
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        private static bool Contains(IList<int> list1, IList<int> list2)
        {
            if (list1 == null || list2 == null)
                return false;

            foreach (int a in list1)
                foreach (int b in list2)
                    if (a == b)
                        return true;
            return false;
        }

        /// <summary>
        /// Returns the number of bits per pixel as a long, or 16 if not found.
        /// </summary>
        /// <param name="result">The match result for the current request.</param>
        /// <returns></returns>
        private long GetBitsPerPixel(Result result)
        {
            long bitsPerPixel = 1;
            if (long.TryParse(
                _provider.Strings.Get(result.GetFirstPropertyValueStringIndex(BitsPerPixel)), 
                out bitsPerPixel))
                return bitsPerPixel;
            return 16;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns the current capabilities value if it exists, otherwise
        /// uses the provided default values.
        /// </summary>
        /// <param name="key">The property key to be returned.</param>
        /// <param name="currentCapabilities">The current capabilities found by .NET.</param>
        /// <returns>A default value.</returns>
        protected static string GetDefaultValue(string key, IDictionary currentCapabilities)
        {
            string currentValue = currentCapabilities[key] as string;
            if (currentValue != null)
                return currentValue;
            return GetDefaultValue(key);
        }

        /// <summary>
        /// Returns the default value for the key to use if one can not be 
        /// found.
        /// </summary>
        /// <param name="key">The property key to be returned.</param>
        /// <returns>The hardcoded default value.</returns>
        protected static string GetDefaultValue(string key)
        {
            for (int i = 0; i < Constants.DefaultPropertyValues.Length; i++)
                if (Constants.DefaultPropertyValues[i, 0] == key)
                    return Constants.DefaultPropertyValues[i, 1];
            return null;
        }

        /// <summary>
        /// Sets the javascript boolean string in the capabilities dictionary.
        /// </summary>
        /// <param name="capabilities">Capabilities dictionary.</param>
        /// <param name="javaScript">The value of the jaavscript keys.</param>
        protected static void SetJavaScript(IDictionary capabilities, bool javaScript)
        {
            SetValue(capabilities, "javascript", javaScript.ToString().ToLowerInvariant());
            SetValue(capabilities, "Javascript", javaScript.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Sets the key in the capabilities dictionary to the object provided. If the key 
        /// already exists the previous value is replaced. If not a new entry is added
        /// to the Dictionary.
        /// </summary>
        /// <param name="capabilities">Dictionary of capabilities to be changed.</param>
        /// <param name="key">Key to be changed or added.</param>
        /// <param name="value">New entry value.</param>
        internal static void SetValue(IDictionary capabilities, string key, object value)
        {
            // Ignore new values that are empty strings.
            if (value == null ||
                String.IsNullOrEmpty(value as string))
                return;

            // Change or add the new capability.
            if (capabilities.Contains(key) == false)
            {
                capabilities.Add(key, value);
            }
            else
            {
                capabilities[key] = value;
            }
        }

        /// <summary>
        /// Returns the class to use as a text writer for the output stream.
        /// </summary>
        /// <param name="capabilities">Dictionary of device capabilities.</param>
        /// <returns>A string containing the text writer class name.</returns>
        private static string GetTagWriter(IDictionary capabilities)
        {
            switch (capabilities["preferredRenderingType"] as string)
            {
                case "xhtml-mp":
                case "xhtml-basic":
                    return "System.Web.UI.XhtmlTextWriter";

                case "chtml10":
                    return "System.Web.UI.ChtmlTextWriter";

                case "html4":
                    return "System.Web.UI.HtmlTextWriter";

                case "html32":
                    return "System.Web.UI.Html32TextWriter";

                default:
                    return "System.Web.UI.Html32TextWriter";
            }
        }

        #endregion
    }
}