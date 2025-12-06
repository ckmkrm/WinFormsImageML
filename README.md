# 🖼️ WinForms ML.NET Image Classification App  
Bu proje, **C# + WinForms + ML.NET (ImageClassification)** kullanarak resimleri **kedi / köpek** olarak sınıflandıran bir masaüstü uygulamasıdır.  
.NET 8 + ML.NET 5 + TensorFlow backend kullanır.

---

## 📌 Özellikler
- ✔️ ML.NET Image Classification (ResNet50 tabanlı)
- ✔️ Kedi / Köpek sınıflandırması
- ✔️ WinForms grafik arayüz
- ✔️ Görsel yükleme
- ✔️ Eğitim + Test modeli
- ✔️ Modeli ZIP olarak kaydetme (`output/model.zip`)

---

## 📂 Proje Klasör Yapısı

<ProjectRoot>/
├── data/
│ ├── train/
│ │ ├── cats/
│ │ └── dogs/
│ └── test/
│ ├── cats/
│ └── dogs/
├── output/
│ └── model.zip
├── Form1.cs
├── Form1.Designer.cs
├── ImageData.cs
├── ImagePrediction.cs
├── Program.cs
├── WinFormsImageML.csproj
└── README.md

---

## 📥 Dataset (Kedi / Köpek Resimleri)

Resmi Microsoft dataset:

🔗 https://www.microsoft.com/en-us/download/details.aspx?id=54765  
**Dogs vs Cats – Kaggle subset**

İndirdikten sonra:

- `cats` klasörünü `data/train/cats` içine koy
- `dogs` klasörünü `data/train/dogs` içine koy
- Aynı şekilde test klasörlerini doldur

---

## 🔧 Kullanılan NuGet Paketleri

```xml
<PackageReference Include="Microsoft.ML" Version="5.0.0" />
<PackageReference Include="Microsoft.ML.ImageAnalytics" Version="5.0.0" />
<PackageReference Include="Microsoft.ML.Vision" Version="5.0.0" />
<PackageReference Include="SciSharp.TensorFlow.Redist" Version="2.16.0" />
