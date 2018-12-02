using LD43.Gameplay.Models;
using Newtonsoft.Json;
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
            levelWindow2.RVM = model;
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

            spawnGroupListBox.Items.Clear();
            spawnGroupListBox.Items.AddRange(Enumerable.Range(0, 100).Cast<object>().ToArray());
            spawnGroupListBox.SelectedIndex = 0;

            inanimatesListBox.Items.Clear();
            inanimatesListBox.Items.AddRange(typeof(InanimateType).GetEnumNames());
            inanimatesListBox.SelectedIndex = 0;
            model.InanimateType = inanimatesListBox.SelectedItem as string;

            enemiesListBox.Items.Clear();
            enemiesListBox.Items.AddRange(typeof(EnemyType).GetEnumNames());
            enemiesListBox.SelectedIndex = 0;
            model.EnemyType = enemiesListBox.SelectedItem as string;

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
                "PlayerStart",
                "Inanimate",
                "Enemy",
                "SpawnGroup",
            });
            modeComboBox.SelectedIndex = modeComboBox.Items.IndexOf(model.Mode);

            levelWindow2.TexturesToLoad = files;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.Mode = modeComboBox.Items[modeComboBox.SelectedIndex] as string;
        }

        private void inanimatesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            modeComboBox.SelectedIndex = modeComboBox.Items.IndexOf("Inanimate");
            model.Mode = "Inanimate";
            model.InanimateType = inanimatesListBox.SelectedItem as string;
        }

        private void enemiesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model.EnemyType = enemiesListBox.SelectedItem as string;
        }

        private void modeComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            model.Mode = modeComboBox.Items[modeComboBox.SelectedIndex] as string;
        }

        private void spawnGroupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            modeComboBox.SelectedIndex = modeComboBox.Items.IndexOf("SpawnGroup");
            model.Mode = "SpawnGroup";
            model.SpawnGroup = spawnGroupListBox.SelectedIndex;
        }

        private void paintComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            modeComboBox.SelectedIndex = modeComboBox.Items.IndexOf("Tile");
            model.Mode = "Tile";
            model.LeftClickTexture = paintComboBox.Items[paintComboBox.SelectedIndex] as string;
        }

        private void altPaintComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            modeComboBox.SelectedIndex = modeComboBox.Items.IndexOf("Tile");
            model.Mode = "Tile";
            model.RightClickTexture = altPaintComboBox.Items[altPaintComboBox.SelectedIndex] as string;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var r = openFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                model.Load(JsonConvert.DeserializeObject<RoomConfig>((File.ReadAllText(openFileDialog1.FileName))));
            }
        }
    }
}
