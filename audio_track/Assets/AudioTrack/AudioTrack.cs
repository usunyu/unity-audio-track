using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    using Debug = UnityEngine.Debug;

    public class AudioTrackInfo
    {
        public int Index { get; private set; }
        public string InlineInfo { get; private set; }

        public AudioTrackInfo(int index, string info)
        {
            Index = index;
            InlineInfo = info;
        }

        public override string ToString()
        {
            return string.Concat("Audio Stream Index: ", Index, ", Inline Info: ", InlineInfo);
        }
    }

    public class AudioTrack
    {
        private const string TAG = "[AudioTrack]";

        private static AudioTrack _instance = null;
        private Dictionary<string, AudioTrackInfo[]> _cache = new Dictionary<string, AudioTrackInfo[]>();
        private List<string> _ffmpegOutput = new List<string>();

        private string FFmpegPath
        {
            get { return Path.Combine(Application.streamingAssetsPath, "ffmpeg"); }
        }

        private AudioTrack()
        {
        }

        public static AudioTrack Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AudioTrack();
                }

                return _instance;
            }
        }

        public AudioTrackInfo[] QueryAudioTrack(string file)
        {
            if (!File.Exists(FFmpegPath))
            {
                Debug.LogWarning(TAG + " FFmpeg not found!");
                return null;
            }
            if (!File.Exists(file))
            {
                Debug.LogWarning(TAG + " Media file not found!");
            }
            if (_cache.ContainsKey(file))
            {
                return _cache[file];
            }
            _ffmpegOutput.Clear();
            // Build ffmpeg process with command.
            string command = string.Concat(" -i ", file, " -hide_banner");
            Process ffmpeg = new Process();
            ffmpeg.StartInfo.Arguments = command;
            ffmpeg.StartInfo.FileName = FFmpegPath;
            ffmpeg.StartInfo.UseShellExecute = false;
            ffmpeg.StartInfo.RedirectStandardOutput = true;
            ffmpeg.StartInfo.RedirectStandardError = true;
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.ErrorDataReceived += ProcessFFmpegOutput;
            ffmpeg.OutputDataReceived += ProcessFFmpegOutput;
            ffmpeg.EnableRaisingEvents = true;
            ffmpeg.Start();
            ffmpeg.BeginOutputReadLine();
            ffmpeg.BeginErrorReadLine();
            ffmpeg.WaitForExit();
            // Build audio track info.
            List<AudioTrackInfo> tracks = new List<AudioTrackInfo>();
            int index = 0;
            foreach (string output in _ffmpegOutput)
            {
                // Extract audio stream message.
                if (output.Contains("Stream") && output.Contains("Audio"))
                {
                    AudioTrackInfo track = new AudioTrackInfo(index++, output.Trim());
                    tracks.Add(track);
                }
            }
            return tracks.ToArray();
        }

        private void ProcessFFmpegOutput(object sender, DataReceivedEventArgs e)
        {
            string message = e.Data;
            if (_ffmpegOutput != null && message != null && message.Length > 0)
            {
                _ffmpegOutput.Add(string.Concat(message));
            }
        }
    }
}