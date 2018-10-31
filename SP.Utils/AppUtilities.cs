using SP.Containers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;

namespace SP
{
    public static class AppUtilities
    {
        public static StringTable ParseArgs(string[] args, params string[] acceptableFlags)
        {
            bool IsFlag(string arg) => (arg.Length == 2 && (arg[0] == '/' || arg[0] == '-')) || (arg.Length > 2 && arg.StartsWith("--"));

            var result = new StringTable();
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (IsFlag(arg))
                {
                    var flag = arg.Length == 2 ? arg.Substring(1) : arg.Substring(2);
                    if (Array.IndexOf(acceptableFlags, flag) < 0)
                        throw new ArgumentOutOfRangeException(flag);
                    var val = (i >= args.Length - 1) || IsFlag(args[i + 1]) ? "" : args[++i];
                    result[flag] = val;
                }
                else
                    //assume that one argument (typically the last) without a switch is acceptable
                    result["default"] = arg;
            }
            return result;
        }

        public static string RunExecutable(string path, string args, out string errors)
        {
            Process p = new Process();
            ProcessStartInfo psi = p.StartInfo;
            psi.UseShellExecute = false;
            psi.FileName = path;
            psi.Arguments = args;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            p.Start();
            errors = p.StandardError.ReadToEnd();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }

    public class Logger
    {
        private readonly string filePath;
        public SmtpClient Mailer { get; private set; } = new SmtpClient();
        public long MaxLogFileSize { get; set; } = Int64.MaxValue;

        public Logger(string path)
        {
            filePath = path;
        }

        public void LogError(Exception error, string message = null)
        {
            LogMessage($"{DateTime.Now:g} Unandled {error.GetType().Name} occurred - {message ?? error.Message}\r\n{error.StackTrace}");
        }

        public void LogMessage(string msg)
        {
            using (FileStream file = new FileStream(this.filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                var totalMsg = msg + "\r\n";
                var remaining = String.Empty;
                if ((file.Length + totalMsg.Length) > this.MaxLogFileSize) //2 extra characters for the crlf
                {
                    var reader = new StreamReader(file);
                    file.Position = Math.Max(file.Length - (this.MaxLogFileSize - totalMsg.Length), 0);
                    remaining = reader.ReadToEnd();
                    file.Position = 0;
                }
                else
                    file.Seek(0, SeekOrigin.End);
                var writer = new StreamWriter(file);
                writer.Write(remaining);
                if (totalMsg.Length > this.MaxLogFileSize)
                    writer.Write(totalMsg.Substring(0, Convert.ToInt32(this.MaxLogFileSize)));
                else
                    writer.Write(totalMsg);
                writer.Flush();
            }
        }

        public void SendEmailAlert(string from, string to, string subject, string body)
        {
            try
            {
                MailMessage email = new MailMessage(from, to, subject, body) { IsBodyHtml = true };
                Mailer.Send(email);
            }
            catch (Exception ex)
            {
                LogMessage("Error sending email message: " + ex.Message);
            }
        }
    }

    public class Counter
    {
        private static Counter instance;
        private static int progressFrequency = 1;
        public int Iterations { get; private set; } = 0;
        public DateTime StartTime { get; private set; } = DateTime.Now;
        public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;
        public double Speed { get; private set; } = 0.0;

        protected Counter() { }

        public static void Increment(Action<Counter> progressHandler)
        {
            if (instance != null)
            {
                instance.Step();
                if (progressHandler != null && instance.Iterations % progressFrequency == 0)
                    progressHandler(instance);
            }
        }

        public static Counter Start(int frequency)
        {
            progressFrequency = frequency;
            instance = new Counter();
            return instance;
        }

        protected void Step()
        {
            this.Iterations += 1;
            this.ElapsedTime = DateTime.Now - this.StartTime;
            if (this.ElapsedTime > TimeSpan.Zero)
                this.Speed = this.Iterations / this.ElapsedTime.TotalSeconds;
        }

        public static Counter Stop() => instance;
    }
}