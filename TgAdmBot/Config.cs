﻿using TgAdmBot.Logger;

namespace TgAdmBot
{
    public class Config
    {
        public Dictionary<string, string> env = new Dictionary<string, string>();
        public Config()
        {
            var dotenv = Path.Combine(Directory.GetCurrentDirectory(), ".env");
            new Log(dotenv);
            this.Load(dotenv);
        }

        private void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                env.Add(parts[0], parts[1]);
            }
        }

    }
}
