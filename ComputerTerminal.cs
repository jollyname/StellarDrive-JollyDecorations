using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FishNet;

namespace JollyDecorations
{
    public class ComputerTerminal : MonoBehaviour
    {
        public TMP_Text TerminalText;

        public float CharDelay = 0.15f;       // delay between typed characters
        public float PostCommandDelay = 0.6f; // pause after command before output appears
        public float BetweenCommandsDelay = 2.5f; // pause after output before next command starts
        public float CursorBlinkRate = 0.5f;

        public int MaxVisibleLines = 14;

        private const string Prompt = "C:\\>";

        private readonly string _header =
    @"═══════════════════════════════
     STELLAR OPERATING SYSTEM v3.14
     Build 2034.11
     
     Copyright (C) Stellar Industries
     
     Type HELP for a list of commands.
     
";

        private readonly List<string> _fakeCommands = new List<string>
        {
            "HELP",
            "DIR",
            "STATUS",
            "SCAN",
            "CHEATS",
            "WARP",
            "SUDO",
            "CREDITS",
            "CLS",
            "LOGOFF",
        };

        private readonly Dictionary<string, string> _commandOutputs = new Dictionary<string, string>
        {
            { "HELP", "Available commands: HELP, DIR, STATUS, SCAN, CHEATS, WARP, CREDITS, CLS" },
            { "DIR", "  LOG.SYS      2 KB\n  CORE.BIN   482 KB\n  README.TXT   1 KB\n  MANIFEST.DAT 9 KB" },
            { "STATUS", "All systems nominal. No active alerts." },
            { "SCAN", "Nearby signatures: 2 planets, 1 moon, 1 ring system.\nNo hostile contacts detected." },
            { "CHEATS", "DEV TOOLS: F1 - fly mode / F2 - free resources.\nScroll wheel adjusts fly speed. Use responsibly, Captain." },
            { "WARP", "ERROR: Warp drive not installed.\nThis vessel is rated for local system travel only." },
            { "SUDO", "Nice try. This isn't that kind of terminal." },
            { "CREDITS", "STELLAR OS - a StellarDrive terminal prop\nGame by Curious Owl" },
            { "CLS", "" },
            { "LOGOFF", "ERROR: LOGOFF.EXE not found." },
        };

        private string _buffer;
        private bool _isTyping;
        private bool _cursorVisible = true;
        private int _longestCommandLength;

        private void Start()
        {
            _buffer = _header + Prompt;
            RefreshDisplay();

            foreach (string cmd in _fakeCommands)
            {
                if (cmd.Length > _longestCommandLength)
                {
                    _longestCommandLength = cmd.Length;
                }
            }

            StartCoroutine(CursorBlink());
            StartCoroutine(RunCommandSequence());
        }

        private void TrimBuffer()
        {
            string[] lines = _buffer.Split('\n');
            if (lines.Length > MaxVisibleLines)
            {
                int startIndex = lines.Length - MaxVisibleLines;
                _buffer = string.Join("\n", lines, startIndex, MaxVisibleLines);
            }
        }

        private void RefreshDisplay(string suffix = "")
        {
            TrimBuffer();
            TerminalText.text = _buffer + suffix;
        }

        private IEnumerator RunCommandSequence()
        {
            string previousCommand = null;

            float cycleSeconds = (CharDelay * _longestCommandLength) + PostCommandDelay + BetweenCommandsDelay;

            while (true)
            {
                float cycleStartTime = Time.unscaledTime;

                ulong cycleIndex = GetCurrentCycleIndex(cycleSeconds);
                string command = PickCommandForCycle(cycleIndex, previousCommand);
                previousCommand = command;

                yield return TypeCommand(command);

                yield return new WaitForSeconds(PostCommandDelay);

                string output = _commandOutputs.TryGetValue(command, out var result)
                    ? result
                    : $"'{command}' is not recognized as an internal or external command.";

                if (command == "CLS")
                {
                    _buffer = _header + Prompt;
                }
                else
                {
                    if (!string.IsNullOrEmpty(output))
                    {
                        _buffer += "\n" + output;
                    }
                    _buffer += "\n\n" + Prompt;
                }

                RefreshDisplay();

                float elapsed = Time.unscaledTime - cycleStartTime;
                float remaining = cycleSeconds - elapsed;
                if (remaining > 0f)
                {
                    yield return new WaitForSeconds(remaining);
                }
            }
        }

        private ulong GetCurrentCycleIndex(float cycleSeconds)
        {
            var timeManager = InstanceFinder.TimeManager;
            ulong ticksPerCycle = (ulong)Mathf.Max(1, Mathf.RoundToInt(timeManager.TickRate * cycleSeconds));
            return timeManager.Tick / ticksPerCycle;
        }

        // Deterministic pick, same cycleIndex shouldd always produces the same command
        private string PickCommandForCycle(ulong cycleIndex, string previousCommand)
        {
            var rng = new System.Random(unchecked((int)cycleIndex));
            string command = _fakeCommands[rng.Next(_fakeCommands.Count)];

            if (command == previousCommand && _fakeCommands.Count > 1)
            {
                command = _fakeCommands[rng.Next(_fakeCommands.Count)];
            }

            return command;
        }

        private IEnumerator TypeCommand(string command)
        {
            _isTyping = true;

            for (int i = 0; i < command.Length; i++)
            {
                _buffer += command[i];
                RefreshDisplay();
                yield return new WaitForSeconds(CharDelay);
            }

            _isTyping = false;
        }

        private IEnumerator CursorBlink()
        {
            while (true)
            {
                if (!_isTyping)
                {
                    _cursorVisible = !_cursorVisible;
                    RefreshDisplay(_cursorVisible ? "_" : "");
                }
                yield return new WaitForSeconds(CursorBlinkRate);
            }
        }
    }
}