using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LD43.LevelEditor
{
    public partial class Form1 : Form
    {
        private RoomViewModel model;

        public Form1()
        {
            InitializeComponent();
            model = new RoomViewModel();
            levelWindow1.RVM = model;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var r = saveFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                model.Save(saveFileDialog1.FileName);
            }
        }

        private void levelWindow1_MouseClick(object sender, MouseEventArgs e)
        {
            if (model.SelectedTile != null)
            {
                tileLabel.Text = $"Last Changed Tile: ({model.SelectedTile.Position.X},{model.SelectedTile.Position.Y}):";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var r = folderBrowserDialog1.ShowDialog();
            if (r != DialogResult.OK)
            {
                Application.Exit();
                return;
            }

            var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath)
                .Where(f => Path.GetExtension(f).ToLower() == ".png")
                .Select(f => Path.GetFileNameWithoutExtension(f));

            altPaintComboBox.Items.Clear();            
            altPaintComboBox.Items.AddRange(files.ToArray());
            altPaintComboBox.SelectedIndex = altPaintComboBox.Items.IndexOf(model.RightClickTexture);

            paintComboBox.Items.Clear();
            paintComboBox.Items.AddRange(files.ToArray());
            paintComboBox.SelectedIndex = paintComboBox.Items.IndexOf(model.LeftClickTexture);

            modeComboBox.Items.Clear();
            modeComboBox.Items.AddRange(new[]
            {
                "Tile",
                "PlayerStart"
            });
            modeComboBox.SelectedIndex = modeComboBox.Items.IndexOf(model.Mode);

            levelWindow1.TexturesToLoad = files;
        }

        private void tileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.RightClickTexture = altPaintComboBox.Items[altPaintComboBox.SelectedIndex] as string;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void paintComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.LeftClickTexture = paintComboBox.Items[paintComboBox.SelectedIndex] as string;
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.Mode = modeComboBox.Items[modeComboBox.SelectedIndex] as string;
        }
    }
}
