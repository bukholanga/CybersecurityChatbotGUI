namespace CybersecurityChatbotGUI.Audio
{
    /// <summary>
    /// Handles playback of the voice greeting WAV file on application startup.
    /// </summary>
    public static class AudioPlayer
    {
        private const string WavFileName = "assets/greeting.wav";

        public static void PlayGreeting()
        {
            try
            {
                string wavPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, WavFileName);

                if (!File.Exists(wavPath))
                    return;

                if (OperatingSystem.IsWindows())
                {
                    using var player = new System.Media.SoundPlayer(wavPath);
                    player.PlaySync();
                }
            }
            catch { }
        }
    }
}