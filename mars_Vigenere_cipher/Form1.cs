using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mars_shifr_visionera
{
    public partial class Form1 : Form
    {
        private Button encrypt_btn, decrypt_btn, load_file_btn;
        private ComboBox alphabet_cmb;
        private TextBox encrypt_key_txt, decrypt_key_txt;
        private RichTextBox encrypt_message_rich_txt, decrypt_message_rich_textbox, encrypted_message_rich_txt, decrypted_message_rich_textbox;
        private OpenFileDialog openFileDialog;
        private TableLayoutPanel tableLayoutPanel;
        private static readonly string RUS = "АБВГҐДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЭЮЯ", 
            ENG = "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
            SPECIAL_CHARACTERS = "1234567890!@#$%^&*()_+=-`~[]{}|;':\",./<>?";

        private const int EM_SETCUEBANNER = 0x1501;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        public Form1()
        {
            InitializeComponent();
            INIT();
        }

        private void INIT()
        {
            this.Text = "Matt Mars: The Vigener Cipher";
            this.Size = new Size(800, 600);
            this.MinimumSize = new Size(800, 600);

            this.BackColor = Color.Black;
            this.ForeColor = Color.White;

            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.RowCount = 8;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.AutoSize = false;
            tableLayoutPanel.Padding = new Padding(10);
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

            Font my_font = new Font("Calbri", 8, FontStyle.Bold);

            encrypt_btn = new Button();
            encrypt_btn.Text = "Encrypt";
            encrypt_btn.Font = my_font;
            encrypt_btn.Dock = DockStyle.Fill;
            encrypt_btn.Click += ENCRYPT_BTN_CLICK;
            SET_CONTROL_COLOR(encrypt_btn);

            decrypt_btn = new Button();
            decrypt_btn.Text = "Decrypt";
            decrypt_btn.Font = my_font;
            decrypt_btn.Dock = DockStyle.Fill;
            decrypt_btn.Click += DECRYPT_BTN_Click;
            SET_CONTROL_COLOR(decrypt_btn);

            load_file_btn = new Button();
            load_file_btn.Text = "Load File";
            load_file_btn.Font = my_font;
            load_file_btn.Dock = DockStyle.Fill;
            load_file_btn.Click += LOAD_FILE_BTN_CLICK;
            SET_CONTROL_COLOR(load_file_btn);

            alphabet_cmb = new ComboBox();
            alphabet_cmb.Items.Add("Rus");
            alphabet_cmb.Items.Add("Latin");
            alphabet_cmb.Items.Add("Symbols");
            alphabet_cmb.SelectedIndex = 0;
            alphabet_cmb.Font = my_font;
            alphabet_cmb.Dock = DockStyle.Fill;
            alphabet_cmb.SelectedIndexChanged += ALPHABET_CMB_SELECTED_INDEX_CHANGE;
            SET_CONTROL_COLOR(alphabet_cmb);

            encrypt_key_txt = new TextBox();
            encrypt_key_txt.Font = my_font;
            encrypt_key_txt.Dock = DockStyle.Fill;
            SendMessage(encrypt_key_txt.Handle, EM_SETCUEBANNER, 0, "Key for encryption");
            SET_CONTROL_COLOR(encrypt_key_txt);

            decrypt_key_txt = new TextBox();
            decrypt_key_txt.Font = my_font;
            decrypt_key_txt.Dock = DockStyle.Fill;
            SendMessage(decrypt_key_txt.Handle, EM_SETCUEBANNER, 0, "Key for decryption");
            SET_CONTROL_COLOR(decrypt_key_txt);

            encrypt_message_rich_txt = new RichTextBox();
            encrypt_message_rich_txt.Font = my_font;
            encrypt_message_rich_txt.Dock = DockStyle.Fill;
            SendMessage(encrypt_message_rich_txt.Handle, EM_SETCUEBANNER, 0, "Enter message to encrypt");
            SET_CONTROL_COLOR(encrypt_message_rich_txt);

            encrypted_message_rich_txt = new RichTextBox();
            encrypted_message_rich_txt.Font = my_font;
            encrypted_message_rich_txt.ReadOnly = true;
            encrypted_message_rich_txt.Dock = DockStyle.Fill;
            SendMessage(encrypted_message_rich_txt.Handle, EM_SETCUEBANNER, 0, "Encrypted message");
            SET_CONTROL_COLOR(encrypted_message_rich_txt);

            decrypt_message_rich_textbox = new RichTextBox();
            decrypt_message_rich_textbox.Font = my_font;
            decrypt_message_rich_textbox.Dock = DockStyle.Fill;
            SendMessage(decrypt_message_rich_textbox.Handle, EM_SETCUEBANNER, 0, "Enter message to decrypt");
            SET_CONTROL_COLOR(decrypt_message_rich_textbox);

            decrypted_message_rich_textbox = new RichTextBox();
            decrypted_message_rich_textbox.Font = my_font;
            decrypted_message_rich_textbox.ReadOnly = true;
            decrypted_message_rich_textbox.Dock = DockStyle.Fill;
            SendMessage(decrypted_message_rich_textbox.Handle, EM_SETCUEBANNER, 0, "Decrypted message");
            SET_CONTROL_COLOR(decrypted_message_rich_textbox);

            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            tableLayoutPanel.Controls.Add(encrypt_btn, 0, 0);
            tableLayoutPanel.Controls.Add(decrypt_btn, 1, 0);
            tableLayoutPanel.Controls.Add(load_file_btn, 0, 1);
            tableLayoutPanel.Controls.Add(alphabet_cmb, 1, 1);
            tableLayoutPanel.Controls.Add(encrypt_key_txt, 0, 2);
            tableLayoutPanel.Controls.Add(decrypt_key_txt, 1, 2);
            tableLayoutPanel.Controls.Add(encrypt_message_rich_txt, 0, 3);
            tableLayoutPanel.Controls.Add(encrypted_message_rich_txt, 0, 4);
            tableLayoutPanel.Controls.Add(decrypt_message_rich_textbox, 1, 3);
            tableLayoutPanel.Controls.Add(decrypted_message_rich_textbox, 1, 4);

            this.Controls.Add(tableLayoutPanel);
        }

        private void ENCRYPT_BTN_CLICK(object sender, EventArgs e)
        {
            string message = encrypt_message_rich_txt.Text;
            string key = encrypt_key_txt.Text;
            string selected_alphabet_name = alphabet_cmb.SelectedItem.ToString();
            string selected_alphabet = GET_ALPHABET(selected_alphabet_name);

            encrypted_message_rich_txt.Text = ENCRYPT(message, key, selected_alphabet);
        }

        private void DECRYPT_BTN_Click(object sender, EventArgs e)
        {
            string message = decrypt_message_rich_textbox.Text;
            string key = decrypt_key_txt.Text;
            string selected_alphabet_name = alphabet_cmb.SelectedItem.ToString();
            string selected_alphabet = GET_ALPHABET(selected_alphabet_name);

            decrypted_message_rich_textbox.Text = DECRYPT(message, key, selected_alphabet);
        }

        private string GET_ALPHABET(string ALPHABET_NAME)
        {
            switch (ALPHABET_NAME)
            {
                case "Кириллица":
                    return RUS;
                case "Латиница":
                    return ENG;
                case "Символы":
                    return SPECIAL_CHARACTERS;
                default:
                    return ENG;
            }
        }

        private void LOAD_FILE_BTN_CLICK(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string file_path = openFileDialog.FileName;
                string file_content = File.ReadAllText(file_path);

                encrypt_message_rich_txt.Text = file_content;
            }
        }

        private string ENCRYPT(string input, string key, string alphabet)
        {
            StringBuilder result = new StringBuilder();
            int alphabet_length = alphabet.Length;

            input = input.ToUpper();
            key = key.ToUpper();

            string upper_alphabet = alphabet.ToUpper();

            for (int i = 0, j = 0; i < input.Length; i++)
            {
                char current_char = input[i];

                if (upper_alphabet.Contains(current_char) && upper_alphabet.Contains(alphabet))
                {
                    int char_index = upper_alphabet.IndexOf(current_char);
                    int key_index = upper_alphabet.IndexOf(key[j % key.Length]);
                    int new_int = (char_index + key_index) % alphabet_length;
                    result.Append(upper_alphabet[new_int]);
                    j++;
                }
                else
                {
                    result.Append(current_char);
                }
            }

            return result.ToString();
        }

        private string DECRYPT(string input, string key, string alphabet)
        {
            StringBuilder result = new StringBuilder();
            int alphabet_lenght = alphabet.Length;

            input = input.ToUpper();
            key = key.ToUpper();

            string upper_alphabet = alphabet.ToUpper();

            for (int i = 0, j = 0; i < input.Length; i++)
            {
                char current_char = input[i];

                if (upper_alphabet.Contains(current_char) && upper_alphabet.Contains(alphabet))
                {
                    int char_index = upper_alphabet.IndexOf(current_char);
                    int key_index = upper_alphabet.IndexOf(key[j % key.Length]);
                    int new_int = (char_index - key_index + alphabet_lenght) % alphabet_lenght;
                    result.Append(upper_alphabet[new_int]);
                    j++;
                }
                else
                {
                    result.Append(current_char);
                }
            }

            return result.ToString();
        }

        private void ALPHABET_CMB_SELECTED_INDEX_CHANGE(object sender, EventArgs e)
        {
            encrypt_key_txt.Clear();
            decrypt_key_txt.Clear();
            encrypt_message_rich_txt.Clear();
            decrypt_message_rich_textbox.Clear();
            encrypted_message_rich_txt.Clear();
            decrypted_message_rich_textbox.Clear();
        }

        private void SET_CONTROL_COLOR(Control control)
        {
            control.BackColor = Color.Black;
            control.ForeColor = Color.White;
        }
    }
}
