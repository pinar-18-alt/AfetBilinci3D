# Afet Bilinci 3D: Güvenli Oda Macerası

Afet Bilinci 3D, ilkokul çağındaki çocuklar için geliştirilmiş 3D, hikâye temelli ve oyun tabanlı öğrenme yaklaşımını kullanan bir afet bilinci oyunudur. Projenin temel amacı, çocuklara deprem anında ve sonrasında uygulanması gereken güvenli davranışları korkutmadan, yaş düzeylerine uygun bir oyun deneyimi içinde öğretmektir.

Oyuncu; ev ortamında acil durum çantası hazırlar, sarsıntı sırasında güvenli bir alana yönelir, binadan doğru şekilde tahliye olur, merdiven ve asansör seçeneklerini değerlendirir, açık alandaki güvenli toplanma bölgesine ulaşır ve kısa bir mini sınavla öğrendiklerini pekiştirir.

## Eğitim Amacı

Bu proje, afet bilinci eğitimini çocukların bilişsel gelişim düzeyine uygun, kısa yönergelerle desteklenen ve güvenli davranış pratiği sunan bir oyun deneyimine dönüştürür.

Tasarımda öne çıkan temel ilkeler:

- Travmatik, şiddet içeren, kanlı veya aşırı tetikleyici görsellere yer verilmez.
- Deprem ve tahliye süreci korku duygusu üzerinden değil, doğru davranışları öğretme amacıyla ele alınır.
- Ekrandaki metinler kısa, okunaklı ve çocukların kolayca takip edebileceği biçimdedir.
- Görevlerde `E tuşuna bas` ve `E tuşuna basılı tut` gibi açık ve doğrudan eylem yönergeleri kullanılır.
- Yanlış seçimler cezalandırıcı şekilde değil, öğretici geri bildirimlerle açıklanır.
- Son bölümde yer alan mini sınav ile 112 Acil Çağrı Merkezi, güvenli alan seçimi ve afet sonrası davranışlar pekiştirilir.

## Oyun Akışı

1. Oyuncu adını girer ve maceraya başlar.
2. Evde acil durum çantası için gerekli eşyaları toplar.
3. Sarsıntı başladığında güvenli masaya yönelir ve çök-kapan-tutun davranışını uygular.
4. Evden çıkış noktasına ilerler.
5. Apartman koridorunda asansör ve merdiven seçeneklerini değerlendirir.
6. Dış alanda tabelaları ve yönlendirme işaretlerini takip ederek güvenli toplanma alanına gider.
7. Toplanma alanında çadır kurma görevini tamamlar.
8. Üç soruluk mini sınavla öğrendiklerini pekiştirir.
9. Skor ekranında toplam puan, süre ve tamamlanan görev sayısı görüntülenir.

## Ekran Görüntüleri

### 1. Giriş Ekranı
![Giriş Ekranı](docs/screenshots/01-giris-ekrani.png)

### 2. Acil Durum Çantası Hazırlama
![Çanta Hazırlama](docs/screenshots/02-canta-hazirlama.png)

### 3. Sarsıntı ve Güvenli Alan
![Sarsıntı ve Güvenli Alan](docs/screenshots/03-sarsinti-guvenli-alan.png)

### 4. Evden Çıkış
![Evden Çıkış](docs/screenshots/04-evden-cikis.png)

### 5. Apartman Koridoru
![Apartman Koridoru](docs/screenshots/05-apartman-koridoru.png)

### 6. Güvenli Alana Yönlendirme
![Güvenli Alana Yönlendirme](docs/screenshots/06-guvenli-alana-yonlendirme.png)

### 7. Çadır Kurma Görevi
![Çadır Kurma](docs/screenshots/07-cadir-kurma.png)

### 8. Mini Sınav
![Mini Sınav](docs/screenshots/08-mini-sinav.png)

### 9. Skor Ekranı
![Skor Ekranı](docs/screenshots/09-skor-ekrani.png)

## Temel Özellikler

- 3D çocuk karakteri ve kamera takip sistemi
- Çocuk dostu, kısa ve anlaşılır görev metinleri
- Acil durum çantası için kontrol listesi
- Etkileşimli eşya toplama sistemi
- Güvenli alanı görünür kılan yönlendirme ve vurgu sistemi
- Çök-kapan-tutun görevi
- Merdiven ve asansör kullanımı üzerine farkındalık
- Güvenli toplanma alanına yönlendirme
- Çadır kurma görevi
- 112 odaklı mini sınav
- Puan, süre ve görev sayısını gösteren sonuç ekranı

## Teknik Bilgiler

- Oyun motoru: Unity 6.3 LTS
- Render Pipeline: URP
- Hedef platform: Windows, Mac ve Linux Standalone
- Programlama dili: C#

Projeyi açmak için:

1. Unity Hub üzerinden projeyi açın.
2. Unity sürümü olarak `6000.3.10f1` veya uyumlu bir Unity 6.3 LTS sürümü kullanın.
3. İlk açılışta Unity'nin dosyaları içe aktarmasını bekleyin.
4. Başlangıç sahnesi: `Assets/Scenes/GirisEkrani.unity`

## GitHub Notu

Bu projede üçüncü parti Unity asset paketleri kullanılmıştır. Asset lisansları ve dosya boyutları nedeniyle büyük 3D model, doku ve paket içerikleri GitHub reposuna eklenmemiştir. Bu dosyalar yerel Unity projesinde korunur.

Bu repo; proje tanıtımını, ekran görüntülerini, Unity ayarlarını, sahne dosyalarını ve C# kaynak kodlarını paylaşmak için hazırlanmıştır. Projeyi farklı bir bilgisayarda birebir çalıştırmak için kullanılan üçüncü parti asset paketlerinin ayrıca içe aktarılması gerekebilir.

## Eğitimsel Yaklaşım

Afet Bilinci 3D, çocukların afet sırasında ve sonrasında temel güvenlik davranışlarını oyun içinde deneyimleyerek öğrenmesini amaçlar. Oyun; korkutucu sahneler yerine rehberlik eden görevler, olumlu pekiştirme, kısa yönergeler ve güvenli geri bildirimler kullanır.

Bu nedenle proje, travmatik bir afet simülasyonu olarak değil; çocuk dostu, öğretici ve güvenli bir farkındalık deneyimi olarak tasarlanmıştır.
