using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace CIIP.Module.Win.Editors
{
    public class ColorizeAvalonEdit : DocumentColorizingTransformer
    {
        CompletionData[] types;
        Regex reg;
        public ColorizeAvalonEdit(CompletionData[] types)
        {
            this.types = types;
            reg = new Regex(string.Join("|", types.Select(x => x.Text).ToArray()));

        }
        protected override void ColorizeLine(DocumentLine line)
        {
            try
            {
                if (line.Length == 0)
                    return;
                
                int lineStartOffset = line.Offset;
                string text = CurrentContext.Document.GetText(line);
                int start = 0;
                int index;
                var maches = reg.Matches(text);
                foreach (var item in maches)
                {
                    
                }
                while ((index = text.IndexOf("Microsoft", start)) >= 0)
                {
                    base.ChangeLinePart(
                        lineStartOffset + index, // startOffset
                        lineStartOffset + index + 9, // endOffset
                        (VisualLineElement element) =>
                        {
                            // This lambda gets called once for every VisualLineElement
                            // between the specified offsets.
                            Typeface tf = element.TextRunProperties.Typeface;
                            // Replace the typeface with a modified version of
                            // the same typeface
                            element.TextRunProperties.SetTypeface(new Typeface(
                                tf.FontFamily,
                                FontStyles.Italic,
                                FontWeights.Bold,
                                tf.Stretch
                                ));
                        });
                    start = index + 1; // search for next occurrence
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}