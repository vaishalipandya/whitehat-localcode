using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    /// Common functions
    /// </summary>
    public static class Common
    {
        /// <summary>
        ///     for bind combobox by enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<T, string>> Of<T>()
        {
            return Enum.GetValues(typeof (T))
                .Cast<T>()
                .Select(p => new KeyValuePair<T, string>(p, p.ToString()))
                .ToList();
        }

        /// <summary>
        ///     Adds the item to the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="type">The type.</param>
        /// <param name="valueMember">The value member.</param>
        /// <param name="displayMember">The display member.</param>
        /// <param name="displayText">The display text.</param>
        public static void AddItem(
            IList list,
            Type type,
            string valueMember,
            string displayMember,
            string displayText)
        {
            // Creates an instance of the specified type 
            // using the constructor that best matches the specified parameters. 
            object obj = Activator.CreateInstance(type);

            // Gets the Display Property Information
            PropertyInfo displayProperty = type.GetProperty(displayMember);

            // Sets the required text into the display property
            displayProperty.SetValue(obj, displayText, null);

            // Gets the Value Property Information
            PropertyInfo valueProperty = type.GetProperty(valueMember);

            // Sets the required value into the value property
            valueProperty.SetValue(obj, -1, null);

            // Insert the new object on the list
            list.Insert(0, obj);
        }

        /// <summary>
        ///     Gets the line.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="lineNumber">The line no.</param>
        /// <returns></returns>
        public static string GetLine(string text, int lineNumber)
        {
            string[] lines = text.Replace("\\r", string.Empty).Split('\n');
            return lines[lineNumber];
        }      

        /// <summary>
        ///     Extracts from string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="startString">The start string.</param>
        /// <param name="endString">The end string.</param>
        /// <returns></returns>
        public static List<string> ExtractMatchText(string text, string startString, string endString)
        {
            List<string> matchedString = new List<string>();
            bool exit = false;
            while (!exit)
            {
                int indexStart = text.IndexOf(startString);
                int indexEnd = text.IndexOf(endString);
                if (indexStart != -1 && indexEnd != -1)
                {
                    matchedString.Add(text.Substring(indexStart + startString.Length,
                        indexEnd - indexStart - startString.Length));
                    text = text.Substring(indexEnd + endString.Length);
                }
                else
                    exit = true;
            }

            return matchedString;
        }

        /// <summary>
        ///     Gets the name of the input element by.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="webBrowser">The web browser.</param>
        /// <returns></returns>
        public static HtmlElement GetInputElementByName(
            string fieldName,
            WebBrowser webBrowser)
        {
            HtmlElementCollection allInput =
                webBrowser.Document.GetElementsByTagName("input");
            return
                allInput.Cast<HtmlElement>()
                    .FirstOrDefault(
                        htmlElement => htmlElement.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        ///     Gets the input element by value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="webBrowser">The web browser.</param>
        /// <returns></returns>
        public static HtmlElement GetInputElementByValue(
            string fieldName,
            WebBrowser webBrowser)
        {
            HtmlElementCollection allInput =
                webBrowser.Document.GetElementsByTagName("input");
            return
                allInput.Cast<HtmlElement>()
                    .FirstOrDefault(
                        htmlElement =>
                            htmlElement.GetAttribute("value")
                                .Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}