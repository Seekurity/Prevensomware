using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Prevensomware.WPFGUI
{
    public static class RichTextBoxExtension
    {
        public static void AppendText(this RichTextBox box, string text, string color)
        {
            var brushConverter = new BrushConverter();
            var textRange = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd) {Text = text};
            try
            {
                textRange.ApplyPropertyValue(TextElement.ForegroundProperty,
                    brushConverter.ConvertFromString(color));
            }
            catch (FormatException) { }
        }
    }
}
