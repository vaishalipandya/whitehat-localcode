using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHatSec.Shared
{
   public class Enums
    {
        public enum Severity
        {
            /// <summary>
            ///     All.
            /// </summary>
            All = 0,

            /// <summary>
            ///     The informational.
            /// </summary>
            Note = 1,

            /// <summary>
            ///     The low.
            /// </summary>
            Low = 2,

            /// <summary>
            ///     The medium.
            /// </summary>
            Medium = 3,

            /// <summary>
            ///     The high.
            /// </summary>
            High = 4,

            /// <summary>
            ///     The critical.
            /// </summary>
            Critical = 5
        }
    }
}
