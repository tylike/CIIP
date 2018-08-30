using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace CIIP.Module.Win.Editors
{
    /// Implements AvalonEdit ICompletionData interface to provide the entries in the
    /// completion drop down.
    public class CompletionData : ICompletionData
    {
        public TokenType TokenType { get; set; }
        public CompletionData(string text, ImageSource bmp,string description,TokenType tokenType)
        {
            this.Text = text;
            this.Image = bmp;
            this.Description = description;
            this.TokenType = tokenType;
        }

        public ImageSource Image
        {
            get;
        }

        public string Text { get; private set; }

        // Use this property if you want to show a fancy UIElement in the list.
        public object Content
        {
            get { return this.Text; }
        }

        public object Description { get; }

        public double Priority
        {
            get { return 1; }
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}