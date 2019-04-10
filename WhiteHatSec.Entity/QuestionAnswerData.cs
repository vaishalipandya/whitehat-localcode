using System;
using System.Collections.Generic;

namespace WhiteHatSec.Entity
{
    /// <summary>
    /// Question & Answer Info
    /// </summary>
    public class QuestionAnswerInfo
    {
        /// <summary>
        ///     Entity class for Question and Answer.
        /// </summary>
        public class QuestionAnswerData
        {
            /// <summary>
            ///     Gets or sets the Collection for Question.
            /// </summary>
            /// <value>
            ///     The Collection.
            /// </value>
            public List<QuestionAnswerCollection> Collection { get; set; }
        }

        /// <summary>
        ///     Question Answer Collection.
        /// </summary>
        public class QuestionAnswerCollection
        {
            /// <summary>
            ///     Gets or sets the Created date.
            /// </summary>
            /// <value>
            ///     The Created.
            /// </value>
            public DateTime Created { get; set; }

            /// <summary>
            ///     Gets or sets the Author Name.
            /// </summary>
            /// <value>
            ///     The Author.
            /// </value>
            public string Author { get; set; }

            /// <summary>
            ///     Gets or sets the Topic for question.
            /// </summary>
            /// <value>
            ///     The Topic.
            /// </value>
            public string Topic { get; set; }

            /// <summary>
            ///     Gets or sets the Responses.
            /// </summary>
            /// <value>
            ///     The Responses.
            /// </value>
            public AnswerResponse Responses { get; set; }
        }

        /// <summary>
        ///     Response Of Answer.
        /// </summary>
        public class AnswerResponse
        {
            /// <summary>
            ///     Gets or sets the Collection.
            /// </summary>
            /// <value>
            ///     The Collection.
            /// </value>
            public List<ResponseCollection> Collection { get; set; }
        }

        /// <summary>
        ///     Response Collection.
        /// </summary>
        public class ResponseCollection
        {
            /// <summary>
            ///     Gets or sets the Author.
            /// </summary>
            /// <value>
            ///     The author.
            /// </value>
            public string Author { get; set; }

            /// <summary>
            ///     Gets or sets the Content.
            /// </summary>
            /// <value>
            ///     The Content.
            /// </value>
            public string Content { get; set; }

            /// <summary>
            ///     Gets or sets the Created.
            /// </summary>
            /// <value>
            ///     The Created.
            /// </value>
            public string Created { get; set; }
        }
    }
}