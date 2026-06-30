using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL;

namespace GlInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void UpdateList()
        {
            listView1.Items.Clear();
            List<string> ext = new List<string>();
            ext.AddRange(GL.GetString(StringName.Extensions).Split(new char[] { ' ', ';', '\n' }).ToArray());
            richTextBox1.Text = "";
            richTextBox1.Text += GL.GetString(StringName.ShadingLanguageVersion);

            foreach (var s in ext)
            {
                if (!s.ToLower().Contains(textBox1.Text.ToLower()))
                    continue;

                listView1.Items.Add(new ListViewItem(new string[] { s }));
            }

            toolStripStatusLabel1.Text = $"ext count: {ext.Count}";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in listView1.Items)
            {
                var lvi = v as ListViewItem;
                sb.AppendLine(lvi.Text);
            }
            Clipboard.SetText(sb.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var txt = Clipboard.GetText();
            var arr = txt.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            foreach (var s in arr)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    var txt0 = listView1.Items[i].Text;
                    if (txt0.ToLower() == s.ToLower())
                    {
                        listView1.Items[i].BackColor = Color.Violet;

                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in listView1.Items)
            {
                var lvi = v as ListViewItem;
                if (lvi.BackColor != Color.Violet) continue;
                sb.AppendLine(lvi.Text);
            }
            Clipboard.SetText(sb.ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var gl = new GLControl(new GLControlSettings()
            {

            });
            gl.SwapBuffers();

            UpdateList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var gl = new GLControl(new GLControlSettings()
            {
                APIVersion = new Version(4, 6),
                Flags = OpenTK.Windowing.Common.ContextFlags.Debug,
                Profile = OpenTK.Windowing.Common.ContextProfile.Compatability
            });
            gl.SwapBuffers();

            UpdateList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var gl = new GLControl(new GLControlSettings()
            {
                APIVersion = new Version(4, 6),
                Flags = OpenTK.Windowing.Common.ContextFlags.Debug,

            });
            gl.SwapBuffers();

            UpdateList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var gl = new GLControl(new GLControlSettings()
            {
                Profile = OpenTK.Windowing.Common.ContextProfile.Compatability
            });
            gl.SwapBuffers();

            UpdateList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddInt("major", "Major", 4);
            d.AddInt("minor", "Minor", 6);
            d.AddBoolField("overrideApiVersion", "Override API version", false);
            d.AddBoolField("comp", "Compatability profile", false);
            d.AddBoolField("forw", "Forward compatabible");
            d.AddBoolField("debug", "Debug");

            d.SetWidth(400);

            if (!d.ShowDialog())
                return;

            var settings = new GLControlSettings()
            {

            };

            if (d.GetBoolField("overrideApiVersion"))
                settings.APIVersion = new Version(d.GetInt("major"), d.GetInt("minor"));

            if (d.GetBoolField("comp"))
                settings.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;

            if (d.GetBoolField("forw"))
                settings.Flags = OpenTK.Windowing.Common.ContextFlags.ForwardCompatible;

            if (d.GetBoolField("debug"))
                settings.Flags |= OpenTK.Windowing.Common.ContextFlags.Default;

            var gl = new GLControl(settings);
            gl.SwapBuffers();

            UpdateList();
        }
    }
}
