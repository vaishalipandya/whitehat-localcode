using System.Collections.Generic;

namespace WhiteHatSec.Entity
{
    /// <summary>
    /// Application detail
    /// </summary>
    public class ApplicationDetail
    {
        /// <summary>
        ///     class for Appolication Info.
        /// </summary>
        public class ApplicationInfo
        {
            /// <summary>
            ///     Gets or sets the collection.
            /// </summary>
            /// <value>
            ///     The collection.
            /// </value>
            public List<AppsCollection> Collection { get; set; }
            /// <summary>
            ///  Gets or sets the Response Message
            /// </summary>
            public string ResponseMessage { get; set; }
        }

        /// <summary>
        ///     class for Apps Collection.
        /// </summary>
        public class AppsCollection
        {
            /// <summary>
            ///     Gets or sets the Id.
            /// </summary>
            /// <value>
            ///     The id.
            /// </value>
            public int Id { get; set; }

            /// <summary>
            ///     Gets or sets the Label.
            /// </summary>
            /// <value>
            ///     The label.
            /// </value>
            public string Label { get; set; }

            /// <summary>
            ///     Gets or sets the Href.
            /// </summary>
            /// <value>
            ///     The href.
            /// </value>
            public string Href { get; set; }
        }
    }
}