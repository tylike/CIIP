using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CIIP.Module.BusinessObjects.SYS;
using System.Windows.Input;
using System.Diagnostics;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Windows.Forms.Integration;
using ICSharpCode.AvalonEdit.Folding;
using Microsoft.CodeAnalysis;
using CIIP.Module.BusinessObjects.SYS.Logic;
using DevExpress.ExpressApp;
using System.IO;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Text.RegularExpressions;

namespace CIIP.Module.Win.Editors
{
    public partial class SmartVisualStudio : UserControl
    {
        protected SmartVisualStudio()
        {
            InitializeComponent();
        }

        readonly SmartIDEWorkspace _workspace;

        public ICSharpCode.AvalonEdit.TextEditor Editor { get; set; }

        //IList<BusinessObject> businessObjects;
        //MethodDefine method;
        //private BusinessObjectPartialLogic logic;
        private CsharpCode _codeObject;
        readonly IDocumentProvider _document;

        public SmartVisualStudio(IObjectSpace os, CsharpCode value) : this()
        {
            this._codeObject = value;

            CreateEditor();
            if (value != null)
            {
                if (value.Workspace != null)
                {
                    this._workspace = (SmartIDEWorkspace) value.Workspace;
                }
                else
                {
                    this._workspace = SmartIDEWorkspace.GetIDE(os);
                }
                this._document = value.Provider;

            }

            #region 预设置智能感知项目,如果是一个方法，就需要先看一下方法中可以用的智能感知条目列表

            if (_document is IPartCodeProvider && _document!=null)
            {
                var code = value?.Code + "";
                IList<ICompletionData> list = new List<ICompletionData>();
                _workspace.GetIntellisenseItems(_document, 0, true, code, null, list);
            }

            if (value == null)
            {
                Editor.IsEnabled = false;
            }

            #endregion

            #region 设置环境

            if (value != null)
            {
                tabSolution.Visibility = value.ShowSolutionFiles
                    ? DevExpress.XtraBars.Docking.DockVisibility.Visible
                    : DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                if (value.ShowSolutionFiles)
                {
                    solutionTreeView.Nodes.Clear();
                    var solution = solutionTreeView.Nodes.Add("Solution");

                    foreach (var item in _workspace.Workspace.CurrentSolution.Projects)
                    {
                        var projectNode = solution.Nodes.Add(item.Name, item.Name);
                        projectNode.Tag = item;

                        var references = projectNode.Nodes.Add("引用", "引用");
                        foreach (var refence in item.MetadataReferences)
                        {
                            references.Nodes.Add(refence.Display);
                        }

                        foreach (var doc in item.Documents.OrderBy(x=>x.Name))
                        {
                            var docNode = projectNode.Nodes.Add(doc.Name, doc.Name);
                            docNode.Tag = doc;
                        }
                    }
                }
                if (value.Diagnostics != null && value.Diagnostics.Count > 0)
                {
                    this.SetDiagnosticMessage(value.Diagnostics);
                }
            }

            #endregion


        }

        private void CreateEditor()
        {
            Editor = new ICSharpCode.AvalonEdit.TextEditor();

            Editor.TextArea.TextEntering += TextArea_TextEntering;
            Editor.TextArea.TextEntered += TextArea_TextEntered;
            //Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            Editor.ShowLineNumbers = true;

            using (StreamReader s =
                new StreamReader(AdmiralEnvironment.ApplicationPath + @"\\VSCSharp.xshd"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    Editor.SyntaxHighlighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(
                            reader,
                            HighlightingManager.Instance);
                }
            }

            Editor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            Editor.FontSize = 12;
            //Editor.SyntaxHighlighting.MainRuleSet.Rules

            Editor.TextArea.IndentationStrategy =
                new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(Editor.Options);
            var foldingManager = FoldingManager.Install(Editor.TextArea);
            var foldingStrategy = new BraceFoldingStrategy();

            this.elementHost1.Child = Editor;
        }

        private void Listview_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ErrorListView.SelectedItems.Count > 0)
            {
                var selected = ErrorListView.SelectedItems[0];
                var item = selected.Tag as Diagnostic;
                GoogleTranslator.TranslateGoogleString(selected.Text);
                //client.Translate("", selected.Text, "en", "zh-CHS", "text/plan", "general", "");
            }
        }

        private void Listview_DoubleClick(object sender, EventArgs e)
        {
            if (ErrorListView.SelectedItems.Count > 0)
            {
                var selected = ErrorListView.SelectedItems[0];
                var item = selected.Tag as Diagnostic;
                if (_document is IPartCodeProvider)
                {
                    var part = _document as IPartCodeProvider;
                    var code = _workspace.GetText(_document);
                    var begin = code.IndexOf(part.DefaultLocation);
                    var l = item.Location.SourceSpan;
                    if (l.Start > begin)
                    {
                        Editor.Select(l.Start - begin - part.DefaultLocation.Length - 1, l.Length > 0 ? l.Length : 1);
                    }
                }
                else
                {
                    var line = item.Location.GetLineSpan();
                    var path =line.Path;
                    var doc = _workspace.Workspace.CurrentSolution.Projects.First().Documents.First(x => x.Name == path);
                    OpenFile(doc);
                    Editor.Select(item.Location.SourceSpan.Start, item.Location.SourceSpan.Length);
                    Editor.ScrollToLine(line.StartLinePosition.Line);
                }
            }
        }

        public bool Validated()
        {
            var diags = _workspace.GetDiagnostics(Editor.Text, this._document);            
            SetDiagnosticMessage(diags);
            return ErrorListView.Items.Count == 0;
        }

        private void SetDiagnosticMessage(IEnumerable<Diagnostic> diags)
        {
            ErrorListView.Items.Clear();
            if (diags != null)
            {
                foreach (var item in diags.Where(x => x.DefaultSeverity != DiagnosticSeverity.Hidden))
                {
                    var line = item.Location.GetLineSpan().ToString();

                    var lvi = new ListViewItem(new string[] { item.Severity.ToString(), item.GetMessage(), line }, 0);
                    lvi.Tag = item;
                    ErrorListView.Items.Add(lvi);
                }
            }
            
        }

        private string SetStatusBarText()
        {
            return "行:" + Editor.TextArea.Caret.Position.Line + " 列:" + Editor.TextArea.Caret.Position.Column;
        }
        
        //默认为没有输入
        private int startTokenPosition = -1;
        private int inputedLen = 0;

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine("TextArea_TextEntered:" + e.Text);

            if (completionWindow == null &&
                (e.Text == "." || e.Text == " " || e.Text == "\t" || e.Text == "(" || e.Text == "["))
            {
                // Open code completion after the user has pressed dot:
                completionWindow = new CompletionWindow(Editor.TextArea);
                completionWindow.Width = 300;
                var data = completionWindow.CompletionList.CompletionData;
                _workspace.GetIntellisenseItems(this._document, Editor.CaretOffset, e.Text != ".", Editor.Text, null, data);
                completionWindow.Show();
                completionWindow.Closed += delegate
                {
                    completionWindow = null;
                };
            }

            if (e.Text == "\n")
            {
                this.Validated();
                this.OnValueChanged?.Invoke(this, null);
            }
        }

        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine("TextArea_TextEntering:" + e.Text);
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }



        public event EventHandler OnValueChanged;
        CompletionWindow completionWindow;

        public CsharpCode Code
        {
            get
            {
                if (this._codeObject != null)
                    this._codeObject.Code = Editor.Text;
                return _codeObject;
            }
            set
            {
                _codeObject = value;
                if (value == null)
                {
                    Editor.Text = "";
                }
                else
                {
                    Editor.Text = value.Code;
                }
            }
        }

        private void solutionTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (solutionTreeView.SelectedNode != null)
            {
                var s = solutionTreeView.SelectedNode.Tag as Document;
                if (s != null)
                {
                    OpenFile(s);
                }
            }
        }

        private void OpenFile(Document s)
        {
            Editor.Text = _workspace.Workspace.CurrentSolution.Projects.Single(x => x.Id == s.Project.Id).Documents.Single(x => x.Name == s.Name).GetTextAsync().Result.ToString();
        }
    }
}
