# MacroMan - Modern Ultima Online Makro Yöneticisi

.NET 9.0 ile geliştirilmiş, tam özellikli UO makro uygulaması.

## 🚀 Özellikler

### ✅ **Temel Özellikler**
- **GUI Tabanlı** - Script yazmaya gerek yok
- **Background Input** - Arka planda çalışır, client odaklı olması gerekmez
- **Multi-Client Scanner** - Classic/Enhanced/Orion/Assistant desteği
- **Hotkey Desteği** - F1-F12, yön tuşları, A-Z, 0-9
- **3 İşlem Tipi**: TuşaBas, Click, Yaz

### ✅ **Gelişmiş Özellikler**
- **Modifier Keys** - Ctrl+F1, Alt+A, Shift+Tab gibi kombinasyonlar
- **Tekrar Sayısı** - 0-999999 (0 = Sonsuz)
- **Zamanlanmış Bitirme** - Belirli tarih/saatte otomatik dur
- **Bitirme Eylemleri** - Client kapat / Program kapat / PC kapat
- **Real-time Progress** - Her satırın ilerlemesini görün
- **System Tray** - Sistem tepsisine küçült
- **Window Title** - Pencere başlığını değiştir

## 📋 Gereksinimler

- Windows 10/11
- .NET 9.0 Runtime
- **Yönetici hakları gerekli** (input injection için)
- UO Client (Classic/Enhanced/Orion) veya Assistant (Razor/AssistUO/UOSteam)

## 🔧 Kurulum

```bash
# Zip'i aç
unzip MacroMan_v3.1_FINAL.zip

# Derle
cd MacroMan
dotnet build -c Release

# Yönetici olarak çalıştır
dotnet run
```

## 📖 Kullanım Kılavuzu

### 1. **Client Seçimi**
```
1. UO'yu aç
2. MacroMan'i yönetici olarak çalıştır
3. "🔍 Scan" butonuna tıkla
4. Bulduğu clientlerden birini seç
```

**Desteklenen Clientler:**
- `[Classic]` - client.exe
- `[Enhanced]` - UOSA.exe
- `[Orion]` - OrionUO.exe
- `[Assistant]` - AssistUO/Razor/UOSteam window'u

### 2. **Makro Ayarları**
```
Tekrar Sayısı: 10        (0 = Sonsuz)
Bitince: Client'ı Kapat
☑ Zamanlanmış: 18.11.2024 07:00
```

### 3. **Action Ekleme**

#### Örnek 1: Basit Tuş
```
Hotkey: F1
Modifier: [Yok]
Bekleme: 1000ms
İşlem: TuşaBas
```

#### Örnek 2: Kombinasyon Tuş
```
Hotkey: F1
Modifier: ☑ Ctrl
Bekleme: 500ms
İşlem: TuşaBas
→ Sonuç: Ctrl+F1 basılır
```

#### Örnek 3: Mouse Click
```
İşlem: Click
X: 500, Y: 300
Mouse: Sol, Tek
Bekleme: 2000ms
```

#### Örnek 4: Metin Yazma
```
İşlem: Yaz
Metin: "Bank guards!"
Bekleme: 1000ms
```

## 💡 Örnek Senaryolar

### Senaryo 1: AFK Mining (Ctrl+F1 ile)
```
Client: [Classic] Ultima Online - Atlantic
Tekrar: 0 (Sonsuz)
Zamanlanmış: 07:00 (sabah durur)
Bitince: Client'ı Kapat

Actions:
1. F1      | 1000ms | TuşaBas
2. F1      | 2000ms | Click: (500,400) Sol Tek
3. F1      | 5000ms | Bekle
4. Ctrl+F1 | 1000ms | TuşaBas (skill menu)
```

### Senaryo 2: Vendor Check Loop
```
Tekrar: 100
Bitince: Hiçbir Şey Yapma

1. Tab     | 3000ms | TuşaBas (vendor window)
2. F2      | 1000ms | Click: (600,300)
3. Esc     | 2000ms | TuşaBas (close)
4. Alt+F1  | 1000ms | TuşaBas (hidden)
```

### Senaryo 3: Bank Guards Spam
```
Tekrar: 50
Bitince: Programı Kapat

1. Enter      | 500ms  | TuşaBas
2. F1         | 300ms  | Yaz: "Bank guards!"
3. Enter      | 500ms  | TuşaBas
4. Shift+F2   | 2000ms | TuşaBas
```

## 🎯 Özellik Detayları

### **Background Input** 🔥
- Client odaklı olmasa da çalışır
- Chrome'da gezinirken bile UO'da makro çalışır
- PostMessage/SendMessage API kullanır

### **Modifier Keys** ⌨️
```
☑ Ctrl  + F1  = Ctrl+F1
☑ Alt   + A   = Alt+A  
☑ Shift + Tab = Shift+Tab
☑ Ctrl ☑ Alt + F5 = Ctrl+Alt+F5
```

### **Zamanlanmış Bitirme** ⏰
```
Senaryo: Gece farmı
Tekrar: 0 (Sonsuz)
☑ Zamanlanmış: 18.11.2024 07:00

→ Makro sabah 7'ye kadar çalışır
→ 07:00'da otomatik durur
→ "Bitince" eylemini gerçekleştirir
```

### **System Tray** 📋
```
Minimize edildiğinde sistem tepsisine gider
• Çift tıkla: Geri getir
• Sağ tık: Menü
• Balon bildirimi: "Program sistem tepsisinde"
```

**Kısayollar:**
- `Ctrl+M`: Sistem tepsisine küçült
- `Ctrl+S`: Kaydet
- `Ctrl+O`: Yükle

### **Window Title Değiştir** 📝
```
Araçlar > Window Title Değiştir
→ "MacroMan" yerine "Notepad" yaz
→ Task Manager'da Notepad gibi görünür
```

## 🎨 Arayüz Özellikleri

```
Menu Bar:
├── Dosya
│   ├── Kaydet (Ctrl+S)
│   ├── Yükle (Ctrl+O)
│   └── Çıkış
├── Görünüm
│   └── Sistem Tepsisine Küçült (Ctrl+M)
└── Araçlar
    └── Window Title Değiştir...

┌─────────────────────────────────────────────┐
│  UO Client Seçimi                           │
│  [Enhanced] UO - Atlantic  [🔍 Scan]        │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Makro Ayarları                             │
│  Tekrar: [10]  Bitince: [Client Kapat ▼]   │
│  ☑ Zamanlanmış: [18.11.2024] [07:00]       │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  # | Tuş        | Bekl | Tip   | Detay      │
│  1 | F1         | 1s   | Tuşa  | ...        │
│  2 | Ctrl+F1    | 2s   | Tuşa  | ...        │
│  3 | Alt+A      | 500ms| Tuşa  | ...        │
└─────────────────────────────────────────────┘

Status: Tekrar: 5/10 | İşlem 2/4 | Kalan: 02:15:30
[████████████░░░░░░░░] 60%
```

## 🔒 Güvenlik & Önemli Notlar

### ⚠️ Yönetici Hakları
```
MacroMan.exe → Sağ tık → Yönetici olarak çalıştır
```
Gerekli sebep: `PostMessage` API'si elevated haklar gerektirir

### 🎮 UO ToS
- Makro kullanımı oyun kurallarını ihlal edebilir
- Kendi sorumluluğunuzda kullanın
- PvP serverler genelde yasaktır

### 🔐 Background Mode
- Input direkt window handle'a gönderilir
- Memory editing YOK
- Sadece keyboard/mouse message'ları

## 🐛 Sorun Giderme

### Client Bulunamıyor?
```
1. UO açık mı kontrol et
2. Process ismine bak:
   • client.exe
   • UOSA.exe  
   • OrionUO.exe
3. Assistant kullanıyorsan kontrol et:
   • Razor
   • AssistUO
   • UOSteam
```

### Input Gitmiyor?
```
1. Yönetici olarak çalıştır
2. Client doğru seçilmiş mi kontrol et
```

### Modifier Keys Çalışmıyor?
```
1. Checkbox işaretli mi kontrol et:
   ☑ Ctrl ☑ Alt ☑ Shift
2. Grid'de "Ctrl+F1" görünüyor mu?
3. UO client bazı tuşları engelleyebilir
```

## 🛠️ Teknik Detaylar

### Kullanılan API'ler
```csharp
PostMessage()     // Background keyboard input
SendMessage()     // Background mouse input
EnumWindows()     // Client detection
GetWindowText()   // Window title
MapVirtualKey()   // Scan code conversion
```

### Client Detection
```csharp
// Method 1: Process name
Process.GetProcessesByName("client")
Process.GetProcessesByName("UOSA")
Process.GetProcessesByName("OrionUO")

// Method 2: Assistant window
FindWindowsWithText("UOASSIST-TP-MSG-WND")

// Method 3: EnumWindows fallback
Class: "Ultima Online"
Title: "Ultima Online - "
```

## 📊 Performans

- **CPU**: %1-2 (idle)
- **RAM**: ~50MB
- **Input Latency**: <10ms
- **Max Actions**: Sınırsız
- **Max Repeat**: 999,999

## 📄 Lisans

MIT License - Özgürce kullanabilirsiniz.

## ⚠️ Sorumluluk Reddi

Bu yazılım eğitim amaçlıdır. Ultima Online veya diğer oyunlarda kullanımdan doğacak her türlü sorumluluk kullanıcıya aittir. Geliştiriciler hiçbir yasal sorumluluğu kabul etmemektedir.

---

**Geliştirici:** Caner  
**Versiyon:** 3.1 FINAL  
**Özellikler:** Background Input | Modifier Keys | System Tray | Scheduled Time
