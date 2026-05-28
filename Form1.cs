using CybersecurityChatbotGUI.Audio;
using CybersecurityChatbotGUI.Chatbot;

namespace CybersecurityChatbotGUI
{
    /// <summary>
    /// Main GUI form for the Cybersecurity Awareness Chatbot.
    /// Handles all user interaction, display, and chatbot communication.
    /// </summary>
    public partial class Form1 : Form
    {
        // ── Fields ────────────────────────────────────────────────────────
        private readonly ResponseEngine _engine;
        private bool _nameAsked = false;

        public Form1()
        {
            InitializeComponent();
            _engine = new ResponseEngine();
        }

        /// <summary>
        /// Runs when the form loads — plays greeting, shows welcome message.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Play voice greeting
            AudioPlayer.PlayGreeting();

            // Welcome messages
            AppendMessage("CYBERBOT", "════════════════════════════════════════════════", Color.DarkCyan);
            AppendMessage("CYBERBOT", "  🛡  CYBERSECURITY AWARENESS CHATBOT  🛡", Color.Cyan);
            AppendMessage("CYBERBOT", "       Protecting South African Citizens", Color.Cyan);
            AppendMessage("CYBERBOT", "════════════════════════════════════════════════", Color.DarkCyan);
            AppendMessage("CYBERBOT", "", Color.Black);
            AppendMessage("CYBERBOT", "Welcome! I'm here to help you stay safe online.", Color.Cyan);
            AppendMessage("CYBERBOT", "You can ask me about: passwords, phishing, scams,", Color.Cyan);
            AppendMessage("CYBERBOT", "privacy, malware, safe browsing, 2FA, social media,", Color.Cyan);
            AppendMessage("CYBERBOT", "and email safety.", Color.Cyan);
            AppendMessage("CYBERBOT", "", Color.Black);
            AppendMessage("CYBERBOT", "What is your name?", Color.Yellow);

            _nameAsked = true;

            // Allow Enter key to send message
            txtInput.KeyDown += TxtInput_KeyDown;

            // Wire up send button
            btnSend.Click += BtnSend_Click;

            // Focus input box
            txtInput.Focus();
        }

        /// <summary>
        /// Handles Send button click.
        /// </summary>
        private void BtnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        /// <summary>
        /// Allows pressing Enter to send a message.
        /// </summary>
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendMessage();
            }
        }

        /// <summary>
        /// Processes the user's input and displays the bot's response.
        /// </summary>
        private void SendMessage()
        {
            string input = txtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            // Display user message
            AppendMessage("YOU", input, Color.White);
            txtInput.Clear();

            // Handle name input first
            if (_nameAsked && string.IsNullOrEmpty(_engine.GetUserName()))
            {
                _engine.SetUserName(input);
                _nameAsked = false;
                AppendMessage("CYBERBOT",
                    $"Nice to meet you, {_engine.GetUserName()}! 👋",
                    Color.Cyan);
                AppendMessage("CYBERBOT",
                    "What cybersecurity topic would you like to learn about?",
                    Color.Cyan);
                AppendMessage("CYBERBOT",
                    "Type 'help' to see all available topics.",
                    Color.Yellow);
                return;
            }

            // Handle exit
            if (input.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("bye", StringComparison.OrdinalIgnoreCase))
            {
                string name = _engine.GetUserName();
                AppendMessage("CYBERBOT",
                    $"Goodbye{(string.IsNullOrEmpty(name) ? "" : ", " + name)}! Stay safe online. 🛡",
                    Color.Cyan);
                btnSend.Enabled = false;
                txtInput.Enabled = false;
                return;
            }

            // Get and display bot response
            string response = _engine.GetResponse(input);
            AppendMessage("CYBERBOT", response, Color.Cyan);
        }

        /// <summary>
        /// Appends a formatted message to the chat display.
        /// </summary>
        private void AppendMessage(string sender, string message, Color color)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;

            if (string.IsNullOrEmpty(message))
            {
                rtbChat.AppendText(Environment.NewLine);
                return;
            }

            // Sender label
            rtbChat.SelectionColor = sender == "YOU" ? Color.Yellow : Color.DarkCyan;
            rtbChat.SelectionFont = new Font("Consolas", 9, FontStyle.Bold);
            rtbChat.AppendText($"[{sender}] ");

            // Message text
            rtbChat.SelectionColor = color;
            rtbChat.SelectionFont = new Font("Consolas", 10, FontStyle.Regular);
            rtbChat.AppendText(message + Environment.NewLine);

            // Auto-scroll to bottom
            rtbChat.ScrollToCaret();
        }
    }
}