using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MacroMan.Models;

namespace MacroMan.Services
{
    public class MacroExecutor
    {
        private readonly InputSimulator _inputSimulator;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;

        public event EventHandler<MacroProgressEventArgs> ProgressChanged;
        public event EventHandler<MacroCompletedEventArgs> MacroCompleted;
        public event EventHandler<string> ErrorOccurred;

        public bool IsRunning => _isRunning;


        public MacroExecutor(InputSimulator inputSimulator)
        {
            _inputSimulator = inputSimulator ?? throw new ArgumentNullException(nameof(inputSimulator));
        }


        public async Task ExecuteMacroAsync(List<MacroAction> actions, MacroSettings settings)
        {
            if (_isRunning)
            {
                ErrorOccurred?.Invoke(this, "Makro zaten çalışıyor!");
                return;
            }

            _isRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();

            // Target window'u ayarla
            if (!string.IsNullOrEmpty(settings.SelectedClientHandle))
            {
                try
                {
                    IntPtr targetWindow = new IntPtr(long.Parse(settings.SelectedClientHandle));
                    _inputSimulator.SetTargetWindow(targetWindow);
                }
                catch
                {
                    ErrorOccurred?.Invoke(this, "Geçersiz client handle!");
                    _isRunning = false;
                    return;
                }
            }

            try
            {
                await Task.Run(async () =>
                {
                    int repeatCount = settings.RepeatCount;
                    int currentRepeat = 0;
                    bool infinite = repeatCount == 0;
                    bool scheduled = settings.IsScheduled;

                    while (infinite || currentRepeat < repeatCount)
                    {
                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                            break;

                        // Zamanlanmış tamamlanma kontrolü
                        if (scheduled && settings.ShouldCompleteNow())
                        {
                            OnProgressChanged(new MacroProgressEventArgs
                            {
                                ScheduledTimeReached = true
                            });
                            break;
                        }

                        currentRepeat++;

                        for (int i = 0; i < actions.Count; i++)
                        {
                            if (_cancellationTokenSource.Token.IsCancellationRequested)
                                break;

                            // Her action öncesi zamanlama kontrolü
                            if (scheduled && settings.ShouldCompleteNow())
                            {
                                OnProgressChanged(new MacroProgressEventArgs
                                {
                                    ScheduledTimeReached = true
                                });
                                break;
                            }

                            var action = actions[i];

                            // Action'ı çalıştır
                            ExecuteAction(action);

                            // Progress event'i
                            OnProgressChanged(new MacroProgressEventArgs
                            {
                                CurrentIndex = i,
                                TotalActions = actions.Count,
                                CurrentAction = action,
                                WaitTimeMs = action.WaitTimeMs,
                                CurrentRepeat = currentRepeat,
                                TotalRepeats = infinite ? 0 : repeatCount,
                                ScheduledTime = settings.ScheduledCompletionTime
                            });

                            // Bekleme süresi ile progressbar güncellemesi
                            await WaitWithProgress(action.WaitTimeMs, _cancellationTokenSource.Token);
                        }

                        // Loop sonunda zamanlama kontrolü
                        if (scheduled && settings.ShouldCompleteNow())
                            break;

                        if (!infinite && currentRepeat >= repeatCount)
                            break;
                    }
                }, _cancellationTokenSource.Token);

                MacroCompleted?.Invoke(this, new MacroCompletedEventArgs 
                { 
                    CompletionAction = settings.CompletionAction,
                    ClientHandle = string.IsNullOrEmpty(settings.SelectedClientHandle) ? 
                        IntPtr.Zero : 
                        new IntPtr(long.Parse(settings.SelectedClientHandle)),
                    WasScheduled = settings.IsScheduled && settings.ShouldCompleteNow()
                });
            }
            catch (OperationCanceledException)
            {
                // Kullanıcı iptal etti
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Hata: {ex.Message}");
            }
            finally
            {
                _isRunning = false;
                _inputSimulator.SetTargetWindow(IntPtr.Zero); // Reset
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private void ExecuteAction(MacroAction action)
        {
            try
            {
                switch (action.ActionType)
                {
                    case ActionType.TusaBas:

                        _inputSimulator.SendKeyPress(
                            action.HotkeyName, 
                            action.UseCtrl, 
                            action.UseAlt, 
                            action.UseShift
                        );

                        break;

                    case ActionType.Click:
                        if (action.ClickX.HasValue && action.ClickY.HasValue &&
                            action.MouseButton.HasValue && action.ClickType.HasValue)
                        {
                            _inputSimulator.SendMouseClick(
                                action.ClickX.Value,
                                action.ClickY.Value,
                                action.MouseButton.Value,
                                action.ClickType.Value
                            );
                        }
                        break;

                    case ActionType.Yaz:
                        if (!string.IsNullOrEmpty(action.TextToWrite))
                        {
                            _inputSimulator.SendText(action.TextToWrite);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Action çalıştırma hatası: {ex.Message}");
            }
        }

        private async Task WaitWithProgress(int waitTimeMs, CancellationToken cancellationToken)
        {
            const int updateInterval = 100; // 100ms'de bir güncelle
            int elapsed = 0;

            while (elapsed < waitTimeMs)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await Task.Delay(Math.Min(updateInterval, waitTimeMs - elapsed), cancellationToken);
                elapsed += updateInterval;

                // Progress güncelleme için event
                OnProgressChanged(new MacroProgressEventArgs
                {
                    WaitElapsed = elapsed,
                    WaitTimeMs = waitTimeMs
                });
            }
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }

        protected virtual void OnProgressChanged(MacroProgressEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
    }

    public class MacroProgressEventArgs : EventArgs
    {
        public int CurrentIndex { get; set; }
        public int TotalActions { get; set; }
        public MacroAction CurrentAction { get; set; }
        public int WaitTimeMs { get; set; }
        public int WaitElapsed { get; set; }
        public int CurrentRepeat { get; set; }
        public int TotalRepeats { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public bool ScheduledTimeReached { get; set; }
    }

    public class MacroCompletedEventArgs : EventArgs
    {
        public CompletionAction CompletionAction { get; set; }
        public IntPtr ClientHandle { get; set; }
        public bool WasScheduled { get; set; }
    }
}
