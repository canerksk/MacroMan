using System;

namespace MacroMan.Models
{
    [Serializable]
    public class MacroAction
    {
        public string HotkeyName { get; set; }          // F1, F2, A, B, Sol, Sag, vs.
        public int WaitTimeMs { get; set; }             // Bekleme süresi (ms)
        public ActionType ActionType { get; set; }       // TusaBas, Click, Yaz
        
        // Modifier keys
        public bool UseCtrl { get; set; }
        public bool UseAlt { get; set; }
        public bool UseShift { get; set; }
        
        // Click
        public int? ClickX { get; set; }
        public int? ClickY { get; set; }
        public MouseButton? MouseButton { get; set; }
        public ClickType? ClickType { get; set; }
        
        // Yaz 
        public string TextToWrite { get; set; }

        public MacroAction()
        {
            HotkeyName = "F1";
            WaitTimeMs = 1000;
            ActionType = ActionType.TusaBas;
            UseCtrl = false;
            UseAlt = false;
            UseShift = false;
        }

        public override string ToString()
        {
            string modifiers = "";
            if (UseCtrl) modifiers += "Ctrl+";
            if (UseAlt) modifiers += "Alt+";
            if (UseShift) modifiers += "Shift+";

            string actionDesc = ActionType switch
            {
                ActionType.TusaBas => $"Tuşa bas: {modifiers}{HotkeyName}",
                ActionType.Click => $"Click: ({ClickX},{ClickY}) {MouseButton} {ClickType}",
                ActionType.Yaz => $"Yaz: {TextToWrite}",
                _ => "Bilinmeyen"
            };

            return $"[{HotkeyName}] {WaitTimeMs}ms -> {actionDesc}";
        }
    }

    [Serializable]
    public class MacroSettings
    {
        public int RepeatCount { get; set; }                    // 0 = Sonsuz
        public CompletionAction CompletionAction { get; set; }
        public string SelectedClientHandle { get; set; }        // Client window handle
        public DateTime? ScheduledCompletionTime { get; set; }  // Belirli bir tarih/saatte bitir (null = tekrar bitince)

        public MacroSettings()
        {
            RepeatCount = 1;
            CompletionAction = CompletionAction.DoNothing;
            ScheduledCompletionTime = null;
        }

        public bool IsScheduled => ScheduledCompletionTime.HasValue;
        
        public bool ShouldCompleteNow()
        {
            if (!ScheduledCompletionTime.HasValue)
                return false;
                
            return DateTime.Now >= ScheduledCompletionTime.Value;
        }
    }

    public enum CompletionAction
    {
        DoNothing,      // Hiçbir şey yapma
        CloseClient,    // Client'ı kapat
        CloseProgram,   // Programı kapat
        ShutdownPC      // PC'yi kapat
    }
}
