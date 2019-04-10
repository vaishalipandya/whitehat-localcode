using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WhiteHatSec.Entity
{
    /// <summary>
    /// Application Vulnerability Information
    /// </summary>
    public class ApplicationVulnerabilityInfo
    {
        /// <summary>
        ///     Application Vulnerability Data.
        /// </summary>
        public class AppsVulnsData
        {
            /// <summary>
            ///     Gets or sets the Application Vuln Data Collection.
            /// </summary>
            /// <value>
            ///     The collection.
            /// </value>
            public List<AppsVulnsDataCollection> Collection { get; set; }
        }

        /// <summary>
        ///     Application Vuln Collection.
        /// </summary>
        public class AppsVulnsDataCollection
        {
            /// <summary>
            ///     Gets or sets the Id.
            /// </summary>
            /// <value>
            ///     The Id.
            /// </value>
            public string Id { get; set; }

            public string Class_Readable {get;set;}
            /// <summary>
            ///     Gets or sets the Risk.
            /// </summary>
            /// <value>
            ///     The Risk.
            /// </value>
            private string _risk;
            public string Risk { get { return char.ToUpper(_risk[0]) + _risk.Substring(1); } set { _risk = value; } }

            /// <summary>
            ///     Gets or sets the Status.
            /// </summary>
            /// <value>
            ///     The Status.
            /// </value>
            private string _status;
            public string Status { get { return char.ToUpper(_status[0]) + _status.Substring(1); } set { _status = value; } }

            /// <summary>
            ///     Gets or sets the application class.
            /// </summary>
            /// <value>
            ///     The application class.
            /// </value>
            [JsonProperty(PropertyName = "class")]
            public string AppClass { get; set; }

            /// <summary>
            ///     Gets or sets the opened.
            /// </summary>
            /// <value>
            ///     The opened.
            /// </value>
            public DateTime Opened { get; set; }

            /// <summary>
            ///     Gets or sets the opened date.
            /// </summary>
            /// <value>
            ///     The opened date.
            /// </value>
            public DateTime OpenedDate { get; set; }

            /// <summary>
            ///     Gets or sets the application.
            /// </summary>
            /// <value>
            ///     The application.
            /// </value>
            public ApplicationVulns Application { get; set; }

            /// <summary>
            ///     Gets or sets the label.
            /// </summary>
            /// <value>
            ///     The label.
            /// </value>
            public string Label { get; set; }

            [JsonProperty(PropertyName = "location")]
            public string Location { get; set; }
        }

        /// <summary>
        ///     Application Vulns.
        /// </summary>
        public class ApplicationVulns
        {
            /// <summary>
            ///     Gets or sets the label.
            /// </summary>
            /// <value>
            ///     The label.
            /// </value>
            public string Label { get; set; }
        }
    }
}