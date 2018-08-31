using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using DevExpress.ExpressApp;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.Net;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Xml;
using CIIP.Module.BusinessObjects.SYS;
using CIIP.Module.BusinessObjects.SYS.BOBuilder;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.XtraBars.Docking;

namespace CIIP.Module.Win.Editors
{
    //public class CodeEditor : Panel
    //{
    //    public ICSharpCode.AvalonEdit.TextEditor Editor { get; set; }

    //    IList<BusinessObject> businessObjects;
    //    AssemblyBuilder builder;
    //    MethodDefine method;
    //    private CsharpCode codeObject;

    //    public CodeEditor(IObjectSpace os, object currentObject, CsharpCode value)
    //    {
    //        this.codeObject = value;
    //        method = currentObject as MethodDefine;

    //        Editor = new ICSharpCode.AvalonEdit.TextEditor();

    //        Editor.TextArea.TextEntering += TextArea_TextEntering;
    //        Editor.TextArea.TextEntered += TextArea_TextEntered;
    //        //Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
    //        Editor.ShowLineNumbers = true;

    //        using (StreamReader s =
    //            new StreamReader(AdmiralEnvironment.ApplicationPath + @"\\VSCSharp.xshd"))
    //        {
    //            using (XmlTextReader reader = new XmlTextReader(s))
    //            {
    //                Editor.SyntaxHighlighting =
    //                    ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(
    //                        reader,
    //                        HighlightingManager.Instance);
    //            }
    //        }

    //        Editor.FontFamily = new FontFamily("Consolas");
    //        Editor.FontSize = 12;
    //        //Editor.SyntaxHighlighting.MainRuleSet.Rules

    //        Editor.TextArea.IndentationStrategy =
    //            new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(Editor.Options);
    //        var foldingManager = FoldingManager.Install(Editor.TextArea);
    //        var foldingStrategy = new BraceFoldingStrategy();
    //        Editor.TextArea.MouseDoubleClick += TextArea_MouseDoubleClick;
    //        var holder = new ElementHost();
    //        holder.Child = Editor;
    //        holder.Dock = DockStyle.Fill;
    //        this.Controls.Add(holder);

    //        if (currentObject != null)
    //        {
    //            var code = value?.Code + "";
    //            methodDocumentHelper = new MethodDocumentHelper(method, SmartIDEWorkspace.GetIDE(os));
    //            IList<ICompletionData> list = new List<ICompletionData>();
    //            methodDocumentHelper.UpdateAutoCompleteItems(0, true, code, null, list);
    //            var t =
    //                list.OfType<CompletionData>()
    //                    .Where(x => x.TokenType >= TokenType.iClass && x.TokenType <= TokenType.iClassShortCut)
    //                    .Select(x => x.Text)
    //                    .Distinct()
    //                    .ToArray();

    //            var rule = new HighlightingRule();
    //            rule.Color = Editor.SyntaxHighlighting.GetNamedColor("TypeName");
    //            //Editor.SyntaxHighlighting.NamedHighlightingColors;
    //            //\b(?> this | base)\b
    //            rule.Regex = new Regex("\\b(?>" + string.Join("|", t) + ")\\b",
    //                RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace);

    //            //Editor.SyntaxHighlighting.MainRuleSet.Rules.Add(rule);

    //            //Editor.TextArea.TextView.LineTransformers.Add(new ColorizeAvalonEdit(t));
    //        }
    //        else
    //        {
    //            Editor.IsEnabled = false;
    //        }

    //        #region statusbar

    //        Label lblStatus = new Label();
    //        lblStatus.Text = SetStatusBarText();
    //        lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
    //        lblStatus.Dock = DockStyle.Bottom;
    //        this.Controls.Add(lblStatus);

    //        #endregion

    //        #region ErrorListView

    //        var listview = new System.Windows.Forms.ListView();
    //        listview.DoubleClick += Listview_DoubleClick;
    //        listview.MouseClick += Listview_MouseClick;
    //        listview.Columns.Add("消息");
    //        listview.View = System.Windows.Forms.View.Details;
    //        listview.Dock = DockStyle.Bottom;
    //        this.ErrorListView = listview;
    //        this.Controls.Add(listview);

    //        #endregion


    //    }

    //    private void Listview_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
    //    {
    //        if (e.Button == MouseButtons.Right && ErrorListView.SelectedItems.Count > 0)
    //        {
    //            var selected = ErrorListView.SelectedItems[0];
    //            var item = selected.Tag as Diagnostic;
    //            GoogleTranslator.TranslateGoogleString(selected.Text);
    //            //client.Translate("", selected.Text, "en", "zh-CHS", "text/plan", "general", "");
    //        }
    //    }

    //    private void Listview_DoubleClick(object sender, EventArgs e)
    //    {
    //        if (ErrorListView.SelectedItems.Count > 0)
    //        {
    //            var selected = ErrorListView.SelectedItems[0];
    //            var item = selected.Tag as Diagnostic;
    //            var code = methodDocumentHelper.GetCode();
    //            var begin = code.IndexOf(method.DefaultLocation);
    //            var l = item.Location.SourceSpan;
    //            if (l.Start > begin)
    //            {
    //                Editor.Select(l.Start - begin - method.DefaultLocation.Length - 1, l.Length);
    //            }
    //        }
    //    }

    //    public bool Validated()
    //    {
    //        var diags = methodDocumentHelper.GetError(Editor.Text);
    //        var header = ErrorListView.Columns[0];

    //        header.Width = ErrorListView.Width;

    //        ErrorListView.Items.Clear();
    //        foreach (var item in diags)
    //        {
    //            if (item.Severity == DiagnosticSeverity.Error)
    //            {
    //                var lvi = new ListViewItem(item.GetMessage(), 0);
    //                lvi.Tag = item;
    //                ErrorListView.Items.Add(lvi);
    //            }
    //        }
    //        return ErrorListView.Items.Count == 0;
    //    }

    //    System.Windows.Forms.ListView ErrorListView;

    //    private string SetStatusBarText()
    //    {
    //        return "行:" + Editor.TextArea.Caret.Position.Line + " 列:" + Editor.TextArea.Caret.Position.Column;
    //    }

    //    private void TextArea_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    //    {
    //        var form = new Form();
    //        var propertyGrid = new ElementHost();

    //        var editor = new ICSharpCode.AvalonEdit.TextEditor();
    //        editor.Text = methodDocumentHelper.Workspace.GetAllCode();

    //        editor.SyntaxHighlighting = Editor.SyntaxHighlighting;
    //        editor.ShowLineNumbers = true;

    //        editor.TextArea.IndentationStrategy = Editor.TextArea.IndentationStrategy;
    //        var foldingManager = FoldingManager.Install(editor.TextArea);
    //        var foldingStrategy = new BraceFoldingStrategy();

    //        editor.FontFamily = new FontFamily("Consolas");
    //        editor.FontSize = 12;

    //        propertyGrid.Child = editor;

    //        propertyGrid.Dock = DockStyle.Fill;
    //        form.Controls.Add(propertyGrid);
    //        form.Show();
    //    }

    //    //默认为没有输入
    //    private int startTokenPosition = -1;
    //    private int inputedLen = 0;

    //    private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
    //    {
    //        Debug.WriteLine("TextArea_TextEntered:" + e.Text);

    //        if (completionWindow == null &&
    //            (e.Text == "." || e.Text == " " || e.Text == "\t" || e.Text == "(" || e.Text == "["))
    //        {
    //            // Open code completion after the user has pressed dot:
    //            completionWindow = new CompletionWindow(Editor.TextArea);
    //            completionWindow.Width = 300;

    //            var data = completionWindow.CompletionList.CompletionData;
    //            methodDocumentHelper.UpdateAutoCompleteItems(Editor.CaretOffset, e.Text != ".", Editor.Text, null, data);

    //            //if (char.IsLetter(e.Text[0]) && completionWindow.CompletionList.SelectedItem == null)
    //            //{
    //            //    completionWindow.CompletionList.SelectItem(e.Text);
    //            //}

    //            completionWindow.Show();
    //            completionWindow.Closed += delegate
    //            {
    //                completionWindow = null;
    //            };
    //        }

    //        if (e.Text == "\n")
    //        {
    //            this.Validated();
    //            this.OnValueChanged?.Invoke(this, null);
    //        }
    //    }

    //    private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
    //    {
    //        Debug.WriteLine("TextArea_TextEntering:" + e.Text);
    //        if (e.Text.Length > 0 && completionWindow != null)
    //        {
    //            if (!char.IsLetterOrDigit(e.Text[0]))
    //            {
    //                completionWindow.CompletionList.RequestInsertion(e);
    //            }
    //        }
    //    }


    //    private MethodDocumentHelper methodDocumentHelper;

    //    public event EventHandler OnValueChanged;
    //    CompletionWindow completionWindow;

    //    public CsharpCode Code
    //    {
    //        get
    //        {
    //            if (this.codeObject != null)
    //                this.codeObject.Code = Editor.Text;
    //            return codeObject;
    //        }
    //        set
    //        {
    //            codeObject = value;
    //            if (value == null)
    //            {
    //                Editor.Text = "";
    //            }
    //            else
    //            {
    //                Editor.Text = value.Code;
    //            }
    //        }
    //    }

    //}

    public static class GoogleTranslator
    {

         public static  void TranslateGoogleString(string strToTranslate)
        {
            string encodedStr = HttpUtility.UrlEncode(strToTranslate);
            string googleTransUrl = $"http://www.bing.com/translator/?h_text=msn_ctxt&text={strToTranslate}&setlang=zh-cn&from=en&to=zh-CHS";
            Process.Start(googleTransUrl);
        }
    }
}