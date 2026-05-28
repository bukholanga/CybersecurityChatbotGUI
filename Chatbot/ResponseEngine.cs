namespace CybersecurityChatbotGUI.Chatbot
{
    /// <summary>
    /// Stores and retrieves all chatbot responses.
    /// Supports keyword recognition, random responses, sentiment detection,
    /// memory recall, and conversation flow.
    /// </summary>
    public class ResponseEngine
    {
        // ── Memory ────────────────────────────────────────────────────────
        private string _userName = string.Empty;
        private string _favouriteTopic = string.Empty;
        private string _lastTopic = string.Empty;

        // ── Delegates ─────────────────────────────────────────────────────
        public delegate string ResponseSelector(string[] options);
        private readonly ResponseSelector _randomSelector;

        // ── Random instance ───────────────────────────────────────────────
        private readonly Random _random = new();

        // ── Sentiment keywords ────────────────────────────────────────────
        private readonly Dictionary<string, string> _sentimentResponses = new(StringComparer.OrdinalIgnoreCase)
        {
            ["worried"] = "It's completely understandable to feel worried. Cyber threats are real, but knowledge is your best defence. ",
            ["scared"] = "Don't be scared — being aware is the first step to staying safe. ",
            ["frustrated"] = "I understand your frustration. Cybersecurity can feel overwhelming, but let's take it one step at a time. ",
            ["confused"] = "No worries — let me explain that more clearly. ",
            ["curious"] = "Great to hear you're curious! Curiosity is the best way to learn about cybersecurity. ",
            ["angry"] = "I hear you. Let's channel that energy into learning how to protect yourself better. ",
            ["anxious"] = "It's natural to feel anxious about online threats. Let me help put your mind at ease. ",
        };

        // ── Follow-up keywords ────────────────────────────────────────────
        private readonly List<string> _followUpKeywords = new()
        {
            "tell me more", "more", "explain more", "another tip",
            "give me another", "continue", "go on", "elaborate", "expand"
        };

        // ── Topic responses (keyword → multiple responses) ────────────────
        private readonly Dictionary<string, string[]> _topicResponses = new(StringComparer.OrdinalIgnoreCase)
        {
            ["password"] = new[]
            {
                "Use at least 12 characters combining uppercase, lowercase, numbers and symbols. Never reuse passwords across accounts.",
                "A passphrase like 'Coffee&Rain@CapeTown!' is both strong and memorable. Avoid using your name or birthdate.",
                "Enable Two-Factor Authentication (2FA) on all accounts. Even if your password is stolen, 2FA blocks access.",
                "Consider using a trusted password manager like Bitwarden to generate and store strong unique passwords."
            },
            ["phishing"] = new[]
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations like SARS or your bank.",
                "Always check the sender's actual email address — not just the display name. Fake domains are a huge red flag.",
                "Never click suspicious links. Rather go directly to the website by typing the address in your browser.",
                "South African banks will NEVER ask for your PIN or OTP via email or SMS. Report suspicious messages to SABRIC."
            },
            ["scam"] = new[]
            {
                "Common SA scams include fake SARS refund SMSes, prize winner calls, and courier delivery links. Never share your banking details.",
                "If something sounds too good to be true, it usually is. Verify any offer independently before responding.",
                "Romance scams are very common on Facebook and Instagram in South Africa. Never send money to someone you haven't met in person.",
                "Report scams to the South African Police Service (SAPS) and the South African Banking Risk Information Centre (SABRIC)."
            },
            ["privacy"] = new[]
            {
                "Review your social media privacy settings regularly. Limit who can see your posts, location, and personal details.",
                "South Africa's POPIA Act gives you the right to know how your personal information is used. You can request companies to delete your data.",
                "Avoid sharing your ID number, home address, or daily routine on social media — this information can be used for identity theft.",
                "Use a VPN when connecting to public Wi-Fi to protect your browsing data from being intercepted."
            },
            ["malware"] = new[]
            {
                "Install reputable antivirus software like Windows Defender or Kaspersky and keep it updated at all times.",
                "Never download software from unknown or unofficial sources. Stick to official app stores and verified websites.",
                "Ransomware encrypts your files and demands payment. Regular backups to an external drive or cloud storage are your best protection.",
                "Be careful with USB drives from unknown sources — they can silently install malware on your device when plugged in."
            },
            ["safe browsing"] = new[]
            {
                "Always check for HTTPS and a padlock icon in your browser before entering any personal or payment information.",
                "Keep your browser and all extensions updated. Outdated software has security vulnerabilities that hackers exploit.",
                "Use an ad blocker like uBlock Origin to block malicious advertisements that can install malware without you clicking anything.",
                "Clear your browser cookies and cache regularly, especially on shared or public computers."
            },
            ["2fa"] = new[]
            {
                "Two-Factor Authentication adds a second layer of security. Even if someone steals your password, they cannot log in without the second factor.",
                "Use an authenticator app like Google Authenticator or Microsoft Authenticator instead of SMS-based 2FA when possible — it's more secure.",
                "Enable 2FA on your email first — it's the master key to all your other accounts. If email is compromised, everything else is at risk."
            },
            ["social media"] = new[]
            {
                "Set your profiles to Private. Strangers don't need to see your photos, location check-ins, or daily routine.",
                "Think before you post — once information is online, it is very difficult to permanently remove it.",
                "Be wary of friend requests from people you don't know. Fake profiles are used to gather personal information for scams.",
                "Romance scams are rising in South Africa. Never send money to someone you have only met online, no matter how convincing they seem."
            },
            ["email"] = new[]
            {
                "Enable spam filters on your email provider to automatically block suspicious messages.",
                "Never open attachments from unknown senders — especially .exe, .zip, or .doc files. They may contain malware.",
                "Use a separate email address for online shopping and newsletters to protect your main inbox.",
                "Enable 2FA on Gmail, Outlook, and Yahoo. Your email is the gateway to all your other accounts."
            },
        };

        public ResponseEngine()
        {
            // Delegate for random response selection
            _randomSelector = options => options[_random.Next(options.Length)];
        }

        /// <summary>Sets the user's name in memory.</summary>
        public void SetUserName(string name)
        {
            _userName = name;
        }

        /// <summary>Returns the stored user name.</summary>
        public string GetUserName() => _userName;

        /// <summary>
        /// Main method — processes user input and returns a response.
        /// </summary>
        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "It looks like you didn't type anything. Type a topic like 'password safety' or 'phishing'.";

            // ── Check for name introduction ──────────────────────────────
            if (input.StartsWith("my name is", StringComparison.OrdinalIgnoreCase))
            {
                string name = input.Substring(10).Trim();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    _userName = name;
                    return $"Nice to meet you, {_userName}! I'll remember your name. What cybersecurity topic would you like to learn about?";
                }
            }

            // ── Check for favourite topic ────────────────────────────────
            if (input.Contains("interested in", StringComparison.OrdinalIgnoreCase) ||
                input.Contains("i like", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var topic in _topicResponses.Keys)
                {
                    if (input.Contains(topic, StringComparison.OrdinalIgnoreCase))
                    {
                        _favouriteTopic = topic;
                        string namePrefix = !string.IsNullOrEmpty(_userName) ? $"{_userName}, " : "";
                        return $"Great! I'll remember that {namePrefix}you're interested in {topic}. " +
                               $"Here's a tip: {_randomSelector(_topicResponses[topic])}";
                    }
                }
            }

            // ── Check for sentiment ──────────────────────────────────────
            string sentimentPrefix = string.Empty;
            foreach (var sentiment in _sentimentResponses)
            {
                if (input.Contains(sentiment.Key, StringComparison.OrdinalIgnoreCase))
                {
                    sentimentPrefix = sentiment.Value;
                    break;
                }
            }

            // ── Check for follow-up ──────────────────────────────────────
            bool isFollowUp = _followUpKeywords.Any(k =>
                input.Contains(k, StringComparison.OrdinalIgnoreCase));

            if (isFollowUp && !string.IsNullOrEmpty(_lastTopic) &&
                _topicResponses.ContainsKey(_lastTopic))
            {
                string namePrefix = !string.IsNullOrEmpty(_userName) ? $"{_userName}, " : "";
                return $"{namePrefix}here's another tip on {_lastTopic}: " +
                       _randomSelector(_topicResponses[_lastTopic]);
            }

            // ── Check for topic keywords ─────────────────────────────────
            foreach (var topic in _topicResponses)
            {
                if (input.Contains(topic.Key, StringComparison.OrdinalIgnoreCase))
                {
                    _lastTopic = topic.Key;

                    // Memory recall — reference favourite topic if different
                    string memoryNote = string.Empty;
                    if (!string.IsNullOrEmpty(_favouriteTopic) &&
                        !_favouriteTopic.Equals(topic.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        memoryNote = $" (As someone interested in {_favouriteTopic}, this is also relevant to you.)";
                    }

                    string response = _randomSelector(topic.Value);
                    return sentimentPrefix + response + memoryNote;
                }
            }

            // ── Check for general questions ──────────────────────────────
            if (input.Contains("how are you", StringComparison.OrdinalIgnoreCase))
                return "I'm fully operational and ready to help you stay safe online! What would you like to know?";

            if (input.Contains("what is your purpose", StringComparison.OrdinalIgnoreCase) ||
                input.Contains("what do you do", StringComparison.OrdinalIgnoreCase))
                return "I'm a Cybersecurity Awareness Chatbot designed to educate South African citizens about online safety. " +
                       "Ask me about passwords, phishing, scams, privacy, malware, safe browsing, 2FA, social media, or email safety!";

            if (input.Contains("what can i ask", StringComparison.OrdinalIgnoreCase) ||
                input.Contains("help", StringComparison.OrdinalIgnoreCase))
                return "You can ask me about:\n• Password safety\n• Phishing\n• Scams\n• Privacy\n• Malware\n" +
                       "• Safe browsing\n• 2FA\n• Social media\n• Email safety\n\nYou can also say 'tell me more' for extra tips!";

            // ── Sentiment only (no topic) ────────────────────────────────
            if (!string.IsNullOrEmpty(sentimentPrefix))
                return sentimentPrefix + "Tell me which cybersecurity topic you'd like help with and I'll guide you.";

            // ── Default fallback ─────────────────────────────────────────
            string fallbackName = !string.IsNullOrEmpty(_userName) ? $", {_userName}" : "";
            return $"I didn't quite understand that{fallbackName}. Could you rephrase? " +
                   "Try asking about: password safety, phishing, scams, privacy, malware, safe browsing, 2FA, social media, or email safety.";
        }
    }
}