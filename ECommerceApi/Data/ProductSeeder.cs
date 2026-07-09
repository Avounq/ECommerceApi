using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Data
{
    public static class ProductSeeder
    {
        private static readonly ProductSeedItem[] SeedProducts =
        [
            new("Akilli Telefon", 39999.90m, "Elektronik", ["Telefon", "Cep Telefonu"]),
            new("Dizustu Bilgisayar", 54999.90m, "Elektronik", ["Laptop", "Bilgisayar"]),
            new("Kablosuz Kulaklik", 3499.90m, "Elektronik", ["Kulaklik"]),
            new("Akilli Saat", 7999.90m, "Elektronik", ["Smart Watch"]),
            new("4K Smart TV", 44999.90m, "Elektronik", ["Televizyon", "TV"]),
            new("Tablet", 18999.90m, "Elektronik", []),
            new("Oyun Konsolu", 27999.90m, "Elektronik", []),
            new("Mekanik Klavye", 2499.90m, "Elektronik", []),
            new("Kablosuz Mouse", 1499.90m, "Elektronik", []),
            new("Monitor 27 Inc", 10999.90m, "Elektronik", []),
            new("Powerbank 20000 mAh", 1299.90m, "Elektronik", []),
            new("Bluetooth Hoparlor", 1899.90m, "Elektronik", []),

            new("Kadin Elbise", 1199.90m, "Kadin", []),
            new("Kadin Bluz", 549.90m, "Kadin", []),
            new("Kadin Jean Pantolon", 1299.90m, "Kadin", []),
            new("Kadin Trenckot", 2499.90m, "Kadin", []),
            new("Kadin Kazak", 899.90m, "Kadin", []),
            new("Kadin Etek", 699.90m, "Kadin", []),
            new("Kadin Sneaker", 2199.90m, "Kadin", []),
            new("Kadin Gomlek", 799.90m, "Kadin", []),
            new("Kadin Mont", 3499.90m, "Kadin", []),
            new("Kadin Cizme", 2799.90m, "Kadin", ["Cizme"]),

            new("Erkek Basic Tisort", 399.90m, "Erkek", ["Basic Tisort", "Tisort"]),
            new("Erkek Jean Pantolon", 1099.90m, "Erkek", ["Jean Pantolon", "Pantolon"]),
            new("Kapusonlu Sweatshirt", 899.90m, "Erkek", ["Sweatshirt"]),
            new("Mevsimlik Ceket", 2999.90m, "Erkek", ["Ceket"]),
            new("Erkek Polo Yaka", 649.90m, "Erkek", []),
            new("Erkek Chino Pantolon", 1199.90m, "Erkek", []),
            new("Erkek Gomlek", 849.90m, "Erkek", []),
            new("Erkek Mont", 3799.90m, "Erkek", []),
            new("Erkek Sneaker", 2399.90m, "Erkek", []),

            new("Bebek Tulum", 449.90m, "Anne & Cocuk", []),
            new("Cocuk Mont", 1399.90m, "Anne & Cocuk", []),
            new("Oyuncak Blok Seti", 699.90m, "Anne & Cocuk", []),
            new("Bebek Arabasi", 8999.90m, "Anne & Cocuk", []),
            new("Cocuk Ayakkabi", 899.90m, "Anne & Cocuk", []),
            new("Bebek Bezi Paketi", 399.90m, "Anne & Cocuk", []),
            new("Oyuncak Araba", 349.90m, "Anne & Cocuk", []),
            new("Cocuk Sirt Cantasi", 599.90m, "Anne & Cocuk", []),
            new("Mama Sandalyesi", 2499.90m, "Anne & Cocuk", []),

            new("Kahve Makinesi", 5999.90m, "Ev & Yasam", []),
            new("Nevresim Takimi", 1499.90m, "Ev & Yasam", []),
            new("Masa Lambasi", 799.90m, "Ev & Yasam", []),
            new("Sandalye", 1899.90m, "Ev & Yasam", []),
            new("Robot Supurge", 14999.90m, "Ev & Yasam", []),
            new("Tencere Seti", 3299.90m, "Ev & Yasam", []),
            new("Yemek Takimi", 4499.90m, "Ev & Yasam", []),
            new("Hali", 2999.90m, "Ev & Yasam", []),
            new("Kitaplik", 2599.90m, "Ev & Yasam", []),

            new("Filtre Kahve", 249.90m, "Supermarket", []),
            new("Zeytinyagi 1L", 429.90m, "Supermarket", []),
            new("Camasir Deterjani", 399.90m, "Supermarket", []),
            new("Kedi Mamasi", 899.90m, "Supermarket", []),
            new("Pirinc 5 Kg", 329.90m, "Supermarket", []),
            new("Makarna Paketi", 39.90m, "Supermarket", []),
            new("Tuvalet Kagidi", 249.90m, "Supermarket", []),
            new("Bulasik Tableti", 399.90m, "Supermarket", []),
            new("Cikolata Paketi", 89.90m, "Supermarket", []),

            new("Parfum", 1799.90m, "Kozmetik", []),
            new("Gunes Kremi", 499.90m, "Kozmetik", []),
            new("Sac Bakim Seti", 699.90m, "Kozmetik", []),
            new("Ruj", 299.90m, "Kozmetik", []),
            new("Maskara", 349.90m, "Kozmetik", []),
            new("Yuz Temizleme Jeli", 249.90m, "Kozmetik", []),
            new("Nemlendirici Krem", 399.90m, "Kozmetik", []),
            new("Sampuan", 189.90m, "Kozmetik", []),
            new("Tiras Makinesi", 2499.90m, "Kozmetik", []),

            new("Spor Ayakkabi", 2499.90m, "Ayakkabi & Canta", ["Ayakkabi"]),
            new("Deri Bot", 3199.90m, "Ayakkabi & Canta", []),
            new("Sirt Cantasi", 1299.90m, "Ayakkabi & Canta", ["Canta"]),
            new("Omuz Cantasi", 1499.90m, "Ayakkabi & Canta", []),
            new("Laptop Cantasi", 1199.90m, "Ayakkabi & Canta", []),
            new("Valiz Orta Boy", 2999.90m, "Ayakkabi & Canta", []),
            new("Terlik", 499.90m, "Ayakkabi & Canta", []),
            new("Topuklu Ayakkabi", 1899.90m, "Ayakkabi & Canta", []),
            new("Cuzdanli Canta", 1699.90m, "Ayakkabi & Canta", []),

            new("Kol Saati", 1499.90m, "Saat & Aksesuar", ["Saat"]),
            new("Gunes Gozlugu", 899.90m, "Saat & Aksesuar", ["Gozluk"]),
            new("Deri Cuzdan", 649.90m, "Saat & Aksesuar", ["Cuzdan"]),
            new("Kemer", 499.90m, "Saat & Aksesuar", []),
            new("Bileklik", 299.90m, "Saat & Aksesuar", []),
            new("Kolye", 449.90m, "Saat & Aksesuar", []),
            new("Sapka", 349.90m, "Saat & Aksesuar", []),
            new("Atki", 299.90m, "Saat & Aksesuar", []),
            new("Celik Saat", 2999.90m, "Saat & Aksesuar", []),

            new("Kosu Bandi", 18999.90m, "Spor & Outdoor", []),
            new("Kamp Cadiri", 3499.90m, "Spor & Outdoor", []),
            new("Dambil Seti", 1299.90m, "Spor & Outdoor", []),
            new("Termos", 599.90m, "Spor & Outdoor", []),
            new("Yoga Mati", 499.90m, "Spor & Outdoor", []),
            new("Bisiklet Kaski", 899.90m, "Spor & Outdoor", []),
            new("Kamp Sandalyesi", 799.90m, "Spor & Outdoor", []),
            new("Spor Canta", 699.90m, "Spor & Outdoor", []),
            new("Protein Tozu", 1499.90m, "Spor & Outdoor", []),

            new("Firsat Bluetooth Hoparlor", 999.90m, "Flas Urunler", []),
            new("Firsat Spor Ayakkabi", 1799.90m, "Flas Urunler", []),
            new("Firsat Tencere Seti", 2499.90m, "Flas Urunler", []),
            new("Firsat Akilli Saat", 4999.90m, "Flas Urunler", []),
            new("Firsat Kahve Makinesi", 3999.90m, "Flas Urunler", []),
            new("Firsat Mont", 1999.90m, "Flas Urunler", []),
            new("Firsat Canta", 899.90m, "Flas Urunler", []),

            new("Telefon Kilifi", 299.90m, "Cok Satanlar", ["Cok Satan Telefon Kilifi"]),
            new("Airfryer", 4999.90m, "Cok Satanlar", ["Cok Satan Airfryer"]),
            new("Tisort", 349.90m, "Cok Satanlar", ["Cok Satan Tisort"]),
            new("Kablosuz Sarj Cihazi", 799.90m, "Cok Satanlar", []),
            new("Termal Tayt", 499.90m, "Cok Satanlar", []),
            new("Mini Blender", 1499.90m, "Cok Satanlar", []),
            new("Organizer Kutu", 249.90m, "Cok Satanlar", []),

            new("iPhone 15 Pro Kilifi", 549.90m, "Elektronik", ["Telefon Kilifi"]),
            new("Android Hızlı Şarj Adaptörü", 699.90m, "Elektronik", ["Sarj Adaptoru"]),
            new("USB-C Kablo 2 Metre", 249.90m, "Elektronik", ["Sarj Kablosu"]),
            new("Gaming Kulaklik", 2499.90m, "Elektronik", ["Kulaklik"]),
            new("Oyuncu Mouse Pad", 399.90m, "Elektronik", ["Mouse Pad"]),
            new("Webcam Full HD", 1399.90m, "Elektronik", ["Webcam"]),
            new("Wi-Fi Router", 2299.90m, "Elektronik", ["Router"]),
            new("Harici SSD 1 TB", 3499.90m, "Elektronik", ["SSD"]),
            new("Akilli Priz", 599.90m, "Elektronik", ["Smart Plug"]),
            new("Dikey Supurge", 9999.90m, "Elektronik", ["Supurge"]),

            new("Kadın Günlük Tişört", 449.90m, "Kadin", ["Tisort"]),
            new("Kadın Oversize Sweatshirt", 999.90m, "Kadin", ["Sweatshirt"]),
            new("Kadın Kumaş Pantolon", 1199.90m, "Kadin", ["Pantolon"]),
            new("Kadın Deri Ceket", 3999.90m, "Kadin", ["Ceket"]),
            new("Kadın Şal", 299.90m, "Kadin", ["Sal"]),
            new("Kadın Pijama Takımı", 999.90m, "Kadin", ["Pijama"]),
            new("Kadın Spor Tayt", 799.90m, "Kadin", ["Tayt"]),
            new("Kadın Yağmurluk", 1899.90m, "Kadin", ["Yagmurluk"]),
            new("Kadın Babet", 1299.90m, "Kadin", ["Babet"]),
            new("Kadın Çanta Askısı", 349.90m, "Kadin", ["Canta Askisi"]),

            new("Erkek Oversize Tişört", 499.90m, "Erkek", ["Tisort"]),
            new("Erkek Jogger Pantolon", 999.90m, "Erkek", ["Jogger"]),
            new("Erkek Deri Ceket", 4299.90m, "Erkek", ["Ceket"]),
            new("Erkek Eşofman Takımı", 1799.90m, "Erkek", ["Esofman"]),
            new("Erkek Spor Şort", 599.90m, "Erkek", ["Sort"]),
            new("Erkek Klasik Ayakkabı", 2499.90m, "Erkek", ["Ayakkabi"]),
            new("Erkek Bere", 299.90m, "Erkek", ["Bere"]),
            new("Erkek Yağmurluk", 1999.90m, "Erkek", ["Yagmurluk"]),
            new("Erkek Keten Gömlek", 999.90m, "Erkek", ["Gomlek"]),
            new("Erkek Spor Çorap Seti", 249.90m, "Erkek", ["Corap"]),

            new("Bebek Islak Mendil Paketi", 149.90m, "Anne & Cocuk", ["Islak Mendil"]),
            new("Bebek Mama Seti", 799.90m, "Anne & Cocuk", ["Mama"]),
            new("Çocuk Lego Seti", 1299.90m, "Anne & Cocuk", ["Lego"]),
            new("Çocuk Scooter", 1899.90m, "Anne & Cocuk", ["Scooter"]),
            new("Bebek Oyun Halısı", 1199.90m, "Anne & Cocuk", ["Oyun Halisi"]),
            new("Çocuk Yağmurluk", 799.90m, "Anne & Cocuk", ["Yagmurluk"]),
            new("Bebek Biberon", 299.90m, "Anne & Cocuk", ["Biberon"]),
            new("Çocuk Kitap Seti", 499.90m, "Anne & Cocuk", ["Kitap"]),
            new("Oyuncak Bebek", 699.90m, "Anne & Cocuk", ["Oyuncak"]),
            new("Çocuk Uyku Tulumu", 899.90m, "Anne & Cocuk", ["Uyku Tulumu"]),

            new("Airfryer XL", 6999.90m, "Ev & Yasam", ["Airfryer"]),
            new("Blender Seti", 2499.90m, "Ev & Yasam", ["Blender"]),
            new("Çay Makinesi", 1999.90m, "Ev & Yasam", ["Cay Makinesi"]),
            new("Ütü", 1799.90m, "Ev & Yasam", ["Utu"]),
            new("Çamaşır Kurutmalık", 799.90m, "Ev & Yasam", ["Kurutmalik"]),
            new("Banyo Havlu Seti", 999.90m, "Ev & Yasam", ["Havlu"]),
            new("Baharatlık Seti", 449.90m, "Ev & Yasam", ["Baharatlik"]),
            new("Duvar Saati", 699.90m, "Ev & Yasam", ["Saat"]),
            new("Dekoratif Ayna", 1499.90m, "Ev & Yasam", ["Ayna"]),
            new("Çalışma Masası", 3999.90m, "Ev & Yasam", ["Masa"]),

            new("Türk Kahvesi", 199.90m, "Supermarket", ["Kahve"]),
            new("Toz Şeker 5 Kg", 249.90m, "Supermarket", ["Seker"]),
            new("Ayçiçek Yağı 5L", 699.90m, "Supermarket", ["Yag"]),
            new("Un 5 Kg", 189.90m, "Supermarket", ["Un"]),
            new("Çay 1 Kg", 249.90m, "Supermarket", ["Cay"]),
            new("Süt 12'li Paket", 399.90m, "Supermarket", ["Sut"]),
            new("Kağıt Havlu", 229.90m, "Supermarket", ["Kagit Havlu"]),
            new("Bulaşık Deterjanı", 189.90m, "Supermarket", ["Deterjan"]),
            new("Köpek Maması", 999.90m, "Supermarket", ["Kopek Mamasi"]),
            new("Maden Suyu 24'lü", 199.90m, "Supermarket", ["Maden Suyu"]),

            new("BB Krem", 399.90m, "Kozmetik", ["Krem"]),
            new("Fondöten", 549.90m, "Kozmetik", ["Fondoten"]),
            new("Allık", 299.90m, "Kozmetik", ["Allik"]),
            new("Eyeliner", 249.90m, "Kozmetik", ["Eyeliner"]),
            new("Dudak Balmı", 149.90m, "Kozmetik", ["Balm"]),
            new("Saç Kurutma Makinesi", 1799.90m, "Kozmetik", ["Sac Kurutma"]),
            new("Vücut Losyonu", 349.90m, "Kozmetik", ["Losyon"]),
            new("El Kremi", 129.90m, "Kozmetik", ["Krem"]),
            new("Diş Fırçası Seti", 199.90m, "Kozmetik", ["Dis Fircasi"]),
            new("Tıraş Köpüğü", 179.90m, "Kozmetik", ["Tiras"]),

            new("Kabin Boy Valiz", 2499.90m, "Ayakkabi & Canta", ["Valiz"]),
            new("Büyük Boy Valiz", 4499.90m, "Ayakkabi & Canta", ["Valiz"]),
            new("Bel Çantası", 699.90m, "Ayakkabi & Canta", ["Canta"]),
            new("Bez Çanta", 249.90m, "Ayakkabi & Canta", ["Canta"]),
            new("Kadın Çizme Siyah", 2999.90m, "Ayakkabi & Canta", ["Cizme"]),
            new("Koşu Ayakkabısı", 2799.90m, "Ayakkabi & Canta", ["Ayakkabi"]),
            new("Outdoor Bot", 3799.90m, "Ayakkabi & Canta", ["Bot"]),
            new("Okul Çantası", 899.90m, "Ayakkabi & Canta", ["Canta"]),
            new("Kartlık", 349.90m, "Ayakkabi & Canta", ["Cuzdan"]),
            new("Seyahat Organizeri", 499.90m, "Ayakkabi & Canta", ["Organizer"]),

            new("Akıllı Bileklik", 1499.90m, "Saat & Aksesuar", ["Bileklik"]),
            new("Gümüş Kolye", 899.90m, "Saat & Aksesuar", ["Kolye"]),
            new("Çelik Bileklik", 549.90m, "Saat & Aksesuar", ["Bileklik"]),
            new("Kadın Güneş Gözlüğü", 1199.90m, "Saat & Aksesuar", ["Gozluk"]),
            new("Erkek Güneş Gözlüğü", 1299.90m, "Saat & Aksesuar", ["Gozluk"]),
            new("Deri Kartlık", 499.90m, "Saat & Aksesuar", ["Cuzdan"]),
            new("Minimal Kol Saati", 2199.90m, "Saat & Aksesuar", ["Saat"]),
            new("Saç Tokası Seti", 199.90m, "Saat & Aksesuar", ["Toka"]),
            new("Kışlık Bere", 349.90m, "Saat & Aksesuar", ["Bere"]),
            new("İpek Fular", 699.90m, "Saat & Aksesuar", ["Fular"]),

            new("Pilates Topu", 399.90m, "Spor & Outdoor", ["Pilates"]),
            new("Direnç Lastiği Seti", 299.90m, "Spor & Outdoor", ["Direnc Lastigi"]),
            new("Kamp Matı", 599.90m, "Spor & Outdoor", ["Mat"]),
            new("Uyku Tulumu", 1999.90m, "Spor & Outdoor", ["Uyku Tulumu"]),
            new("Balıkçı Sandalyesi", 899.90m, "Spor & Outdoor", ["Sandalye"]),
            new("Futbol Topu", 699.90m, "Spor & Outdoor", ["Top"]),
            new("Basketbol Topu", 749.90m, "Spor & Outdoor", ["Top"]),
            new("Suluk", 249.90m, "Spor & Outdoor", ["Suluk"]),
            new("Fitness Eldiveni", 349.90m, "Spor & Outdoor", ["Eldiven"]),
            new("Sırt Kamp Çantası", 1899.90m, "Spor & Outdoor", ["Canta"]),

            new("Fırsat Gaming Kulaklık", 1599.90m, "Flas Urunler", ["Kulaklik"]),
            new("Fırsat Airfryer XL", 4999.90m, "Flas Urunler", ["Airfryer"]),
            new("Fırsat Kadın Çizme", 1999.90m, "Flas Urunler", ["Cizme"]),
            new("Fırsat Büyük Boy Valiz", 2999.90m, "Flas Urunler", ["Valiz"]),
            new("Fırsat Dikey Süpürge", 6999.90m, "Flas Urunler", ["Supurge"]),
            new("Fırsat Parfüm Seti", 1299.90m, "Flas Urunler", ["Parfum"]),
            new("Fırsat Nevresim Takımı", 999.90m, "Flas Urunler", ["Nevresim"]),
            new("Fırsat Koşu Ayakkabısı", 1899.90m, "Flas Urunler", ["Ayakkabi"]),
            new("Fırsat Akıllı Bileklik", 999.90m, "Flas Urunler", ["Bileklik"]),
            new("Fırsat Protein Tozu", 999.90m, "Flas Urunler", ["Protein"]),

            new("Çok Satan Robot Süpürge", 11999.90m, "Cok Satanlar", ["Robot Supurge"]),
            new("Çok Satan Akıllı Saat", 5999.90m, "Cok Satanlar", ["Saat"]),
            new("Çok Satan Kablosuz Kulaklık", 2499.90m, "Cok Satanlar", ["Kulaklik"]),
            new("Çok Satan Kahve Makinesi", 4499.90m, "Cok Satanlar", ["Kahve Makinesi"]),
            new("Çok Satan Kadın Sneaker", 1799.90m, "Cok Satanlar", ["Sneaker"]),
            new("Çok Satan Erkek Mont", 2999.90m, "Cok Satanlar", ["Mont"]),
            new("Çok Satan Bebek Bezi", 349.90m, "Cok Satanlar", ["Bebek Bezi"]),
            new("Çok Satan Güneş Kremi", 399.90m, "Cok Satanlar", ["Gunes Kremi"]),
            new("Çok Satan Kamp Çadırı", 2799.90m, "Cok Satanlar", ["Kamp Cadiri"]),
            new("Çok Satan Sırt Çantası", 999.90m, "Cok Satanlar", ["Canta"])
        ];

        private static readonly Dictionary<string, string> ProductImages = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Akilli Telefon"] = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?auto=format&fit=crop&w=800&q=80",
            ["Dizustu Bilgisayar"] = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=800&q=80",
            ["Kablosuz Kulaklik"] = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=800&q=80",
            ["Akilli Saat"] = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=800&q=80",
            ["4K Smart TV"] = "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?auto=format&fit=crop&w=800&q=80",
            ["Tablet"] = "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?auto=format&fit=crop&w=800&q=80",
            ["Oyun Konsolu"] = "https://images.unsplash.com/photo-1605901309584-818e25960a8f?auto=format&fit=crop&w=800&q=80",
            ["Mekanik Klavye"] = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?auto=format&fit=crop&w=800&q=80",
            ["Kablosuz Mouse"] = "https://images.unsplash.com/photo-1527814050087-3793815479db?auto=format&fit=crop&w=800&q=80",
            ["Monitor 27 Inc"] = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?auto=format&fit=crop&w=800&q=80",
            ["Powerbank 20000 mAh"] = "https://images.unsplash.com/photo-1609091839311-d5365f9ff1c5?auto=format&fit=crop&w=800&q=80",
            ["Bluetooth Hoparlor"] = "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?auto=format&fit=crop&w=800&q=80",

            ["Kadin Elbise"] = "https://images.unsplash.com/photo-1595777457583-95e059d581b8?auto=format&fit=crop&w=800&q=80",
            ["Kadin Bluz"] = "https://images.unsplash.com/photo-1564257631407-3deb25e9c8e2?auto=format&fit=crop&w=800&q=80",
            ["Kadin Jean Pantolon"] = "https://images.unsplash.com/photo-1542272604-787c3835535d?auto=format&fit=crop&w=800&q=80",
            ["Kadin Trenckot"] = "https://images.unsplash.com/photo-1544022613-e87ca75a784a?auto=format&fit=crop&w=800&q=80",
            ["Kadin Kazak"] = "https://images.unsplash.com/photo-1576871337622-98d48d1cf531?auto=format&fit=crop&w=800&q=80",
            ["Kadin Etek"] = "https://images.unsplash.com/photo-1583496661160-fb5886a13d77?auto=format&fit=crop&w=800&q=80",
            ["Kadin Sneaker"] = "https://images.unsplash.com/photo-1549298916-b41d501d3772?auto=format&fit=crop&w=800&q=80",
            ["Kadin Gomlek"] = "https://images.unsplash.com/photo-1598554747436-c9293d6a588f?auto=format&fit=crop&w=800&q=80",
            ["Kadin Mont"] = "https://images.unsplash.com/photo-1544022613-e87ca75a784a?auto=format&fit=crop&w=800&q=80",
            ["Kadin Cizme"] = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?auto=format&fit=crop&w=800&q=80",

            ["Erkek Basic Tisort"] = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&q=80",
            ["Erkek Jean Pantolon"] = "https://images.unsplash.com/photo-1542272604-787c3835535d?auto=format&fit=crop&w=800&q=80",
            ["Kapusonlu Sweatshirt"] = "https://images.unsplash.com/photo-1576871337622-98d48d1cf531?auto=format&fit=crop&w=800&q=80",
            ["Mevsimlik Ceket"] = "https://images.unsplash.com/photo-1551028719-00167b16eac5?auto=format&fit=crop&w=800&q=80",
            ["Erkek Polo Yaka"] = "https://images.unsplash.com/photo-1586363104862-3a5e2ab60d99?auto=format&fit=crop&w=800&q=80",
            ["Erkek Chino Pantolon"] = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=800&q=80",
            ["Erkek Gomlek"] = "https://images.unsplash.com/photo-1598032895397-b9472444bf93?auto=format&fit=crop&w=800&q=80",
            ["Erkek Mont"] = "https://images.unsplash.com/photo-1551028719-00167b16eac5?auto=format&fit=crop&w=800&q=80",
            ["Erkek Sneaker"] = "https://images.unsplash.com/photo-1549298916-b41d501d3772?auto=format&fit=crop&w=800&q=80",

            ["Bebek Tulum"] = "https://images.unsplash.com/photo-1515488764276-beab7607c1e6?auto=format&fit=crop&w=800&q=80",
            ["Cocuk Mont"] = "https://images.unsplash.com/photo-1519238263530-99bdd11df2ea?auto=format&fit=crop&w=800&q=80",
            ["Oyuncak Blok Seti"] = "https://images.unsplash.com/photo-1558060370-d644479cb6f7?auto=format&fit=crop&w=800&q=80",
            ["Bebek Arabasi"] = "https://images.unsplash.com/photo-1595975914625-2f8a1f07994b?auto=format&fit=crop&w=800&q=80",
            ["Cocuk Ayakkabi"] = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=800&q=80",
            ["Bebek Bezi Paketi"] = "https://i5.walmartimages.com/seo/Ditto-Baby-Diapers-Size-4-160-Count-Disposable-Diapers-Super-Pack-Perfect-Day-time-Night-time-Chemical-Smell-Super-Absorbent-Flexible_937d32cb-8bdb-4cbd-9c1e-94c2cd97a7d7.9fb8ea33a3d9ade7a2aac3a175e1ca30.jpeg",
            ["Oyuncak Araba"] = "https://images.unsplash.com/photo-1594787318286-3d835c1d207f?auto=format&fit=crop&w=800&q=80",
            ["Cocuk Sirt Cantasi"] = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?auto=format&fit=crop&w=800&q=80",
            ["Mama Sandalyesi"] = "https://images.unsplash.com/photo-1586000792898-8aa6305f3217?auto=format&fit=crop&w=800&q=80",

            ["Kahve Makinesi"] = "https://images.unsplash.com/photo-1517668808822-9ebb02f2a0e6?auto=format&fit=crop&w=800&q=80",
            ["Nevresim Takimi"] = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?auto=format&fit=crop&w=800&q=80",
            ["Masa Lambasi"] = "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?auto=format&fit=crop&w=800&q=80",
            ["Sandalye"] = "https://images.unsplash.com/photo-1503602642458-232111445657?auto=format&fit=crop&w=800&q=80",
            ["Robot Supurge"] = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?auto=format&fit=crop&w=800&q=80",
            ["Tencere Seti"] = "https://images.unsplash.com/photo-1556911220-bff31c812dba?auto=format&fit=crop&w=800&q=80",
            ["Yemek Takimi"] = "https://images.unsplash.com/photo-1526318472351-c75fcf070305?auto=format&fit=crop&w=800&q=80",
            ["Hali"] = "https://images.unsplash.com/photo-1516455207990-7a41ce80f7ee?auto=format&fit=crop&w=800&q=80",
            ["Kitaplik"] = "https://images.unsplash.com/photo-1521587760476-6c12a4b040da?auto=format&fit=crop&w=800&q=80",

            ["Filtre Kahve"] = "https://images.unsplash.com/photo-1447933601403-0c6688de566e?auto=format&fit=crop&w=800&q=80",
            ["Zeytinyagi 1L"] = "https://images.unsplash.com/photo-1474979266404-7eaacbcd87c5?auto=format&fit=crop&w=800&q=80",
            ["Camasir Deterjani"] = "https://images.unsplash.com/photo-1585421514738-01798e348b17?auto=format&fit=crop&w=800&q=80",
            ["Kedi Mamasi"] = "https://images.unsplash.com/photo-1589924691995-400dc9ecc119?auto=format&fit=crop&w=800&q=80",
            ["Pirinc 5 Kg"] = "https://images.unsplash.com/photo-1586201375761-83865001e31c?auto=format&fit=crop&w=800&q=80",
            ["Makarna Paketi"] = "https://images.unsplash.com/photo-1551462147-37885acc36f1?auto=format&fit=crop&w=800&q=80",
            ["Tuvalet Kagidi"] = "https://images.unsplash.com/photo-1584556812952-905ffd0c611a?auto=format&fit=crop&w=800&q=80",
            ["Bulasik Tableti"] = "https://images.unsplash.com/photo-1585421514738-01798e348b17?auto=format&fit=crop&w=800&q=80",
            ["Cikolata Paketi"] = "https://images.unsplash.com/photo-1511381939415-e44015466834?auto=format&fit=crop&w=800&q=80",

            ["Parfum"] = "https://images.unsplash.com/photo-1541643600914-78b084683601?auto=format&fit=crop&w=800&q=80",
            ["Gunes Kremi"] = "https://images.unsplash.com/photo-1620916566398-39f1143ab7be?auto=format&fit=crop&w=800&q=80",
            ["Sac Bakim Seti"] = "https://images.unsplash.com/photo-1522338242992-e1a54906a8da?auto=format&fit=crop&w=800&q=80",
            ["Ruj"] = "https://images.unsplash.com/photo-1586495777744-4413f21062fa?auto=format&fit=crop&w=800&q=80",
            ["Maskara"] = "https://images.unsplash.com/photo-1631214524020-2574c227b4bb?auto=format&fit=crop&w=800&q=80",
            ["Yuz Temizleme Jeli"] = "https://images.unsplash.com/photo-1556228720-195a672e8a03?auto=format&fit=crop&w=800&q=80",
            ["Nemlendirici Krem"] = "https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=800&q=80",
            ["Sampuan"] = "https://images.unsplash.com/photo-1522338242992-e1a54906a8da?auto=format&fit=crop&w=800&q=80",
            ["Tiras Makinesi"] = "https://images.unsplash.com/photo-1621605815971-fbc98d665033?auto=format&fit=crop&w=800&q=80",

            ["Spor Ayakkabi"] = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=800&q=80",
            ["Deri Bot"] = "https://images.unsplash.com/photo-1520639888713-7851133b1ed0?auto=format&fit=crop&w=800&q=80",
            ["Sirt Cantasi"] = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?auto=format&fit=crop&w=800&q=80",
            ["Omuz Cantasi"] = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=800&q=80",
            ["Laptop Cantasi"] = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?auto=format&fit=crop&w=800&q=80",
            ["Valiz Orta Boy"] = "https://images.unsplash.com/photo-1553531384-cc64ac80f931?auto=format&fit=crop&w=800&q=80",
            ["Terlik"] = "https://images.unsplash.com/photo-1603487742131-4160ec999306?auto=format&fit=crop&w=800&q=80",
            ["Topuklu Ayakkabi"] = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?auto=format&fit=crop&w=800&q=80",
            ["Cuzdanli Canta"] = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=800&q=80",

            ["Kol Saati"] = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=800&q=80",
            ["Gunes Gozlugu"] = "https://images.unsplash.com/photo-1511499767150-a48a237f0083?auto=format&fit=crop&w=800&q=80",
            ["Deri Cuzdan"] = "https://images.unsplash.com/photo-1627123424574-724758594e93?auto=format&fit=crop&w=800&q=80",
            ["Kemer"] = "https://images.unsplash.com/photo-1627123424574-724758594e93?auto=format&fit=crop&w=800&q=80",
            ["Bileklik"] = "https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=800&q=80",
            ["Kolye"] = "https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=800&q=80",
            ["Sapka"] = "https://images.unsplash.com/photo-1521369909029-2afed882baee?auto=format&fit=crop&w=800&q=80",
            ["Atki"] = "https://images.unsplash.com/photo-1520903920243-00d872a2d1c9?auto=format&fit=crop&w=800&q=80",
            ["Celik Saat"] = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=800&q=80",

            ["Kosu Bandi"] = "https://images.unsplash.com/photo-1576678927484-cc907957088c?auto=format&fit=crop&w=800&q=80",
            ["Kamp Cadiri"] = "https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?auto=format&fit=crop&w=800&q=80",
            ["Dambil Seti"] = "https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=800&q=80",
            ["Termos"] = "https://images.unsplash.com/photo-1523362628745-0c100150b504?auto=format&fit=crop&w=800&q=80",
            ["Yoga Mati"] = "https://images.unsplash.com/photo-1599901860904-17e6ed7083a0?auto=format&fit=crop&w=800&q=80",
            ["Bisiklet Kaski"] = "https://images.unsplash.com/photo-1507035895480-2b3156c31fc8?auto=format&fit=crop&w=800&q=80",
            ["Kamp Sandalyesi"] = "https://images.unsplash.com/photo-1478131143081-80f7f84ca84d?auto=format&fit=crop&w=800&q=80",
            ["Spor Canta"] = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?auto=format&fit=crop&w=800&q=80",
            ["Protein Tozu"] = "https://images.unsplash.com/photo-1593095948071-474c5cc2989d?auto=format&fit=crop&w=800&q=80",

            ["Firsat Bluetooth Hoparlor"] = "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?auto=format&fit=crop&w=800&q=80",
            ["Firsat Spor Ayakkabi"] = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=800&q=80",
            ["Firsat Tencere Seti"] = "https://images.unsplash.com/photo-1556911220-bff31c812dba?auto=format&fit=crop&w=800&q=80",
            ["Firsat Akilli Saat"] = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=800&q=80",
            ["Firsat Kahve Makinesi"] = "https://images.unsplash.com/photo-1517668808822-9ebb02f2a0e6?auto=format&fit=crop&w=800&q=80",
            ["Firsat Mont"] = "https://images.unsplash.com/photo-1544022613-e87ca75a784a?auto=format&fit=crop&w=800&q=80",
            ["Firsat Canta"] = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=800&q=80",

            ["Telefon Kilifi"] = "https://images.unsplash.com/photo-1609081219090-a6d81d3085bf?auto=format&fit=crop&w=800&q=80",
            ["Airfryer"] = "https://images.unsplash.com/photo-1556911220-e15b29be8c8f?auto=format&fit=crop&w=800&q=80",
            ["Tisort"] = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&q=80",
            ["Kablosuz Sarj Cihazi"] = "https://images.unsplash.com/photo-1609081219090-a6d81d3085bf?auto=format&fit=crop&w=800&q=80",
            ["Termal Tayt"] = "https://images.unsplash.com/photo-1506629905607-d9f297dcb955?auto=format&fit=crop&w=800&q=80",
            ["Mini Blender"] = "https://images.unsplash.com/photo-1556911220-e15b29be8c8f?auto=format&fit=crop&w=800&q=80",
            ["Organizer Kutu"] = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?auto=format&fit=crop&w=800&q=80"
        };

        public static async Task SeedAsync(AppDbContext dbContext)
        {
            await ResetProductIdentityAsync(dbContext);

            var existingProducts = await dbContext.Products.ToListAsync();

            foreach (var seedProduct in SeedProducts)
            {
                var product = existingProducts.FirstOrDefault(existingProduct =>
                    seedProduct.Matches(existingProduct.Name));

                if (product is null)
                {
                    product = new Product();
                    dbContext.Products.Add(product);
                    existingProducts.Add(product);
                }
                else
                {
                    continue;
                }

                product.Name = seedProduct.Name;
                product.Price = seedProduct.Price;
                product.Category = seedProduct.Category;
                product.ImageUrl = seedProduct.ImageUrl ?? GetImageUrl(seedProduct.Category, seedProduct.Name);
                product.Stock = seedProduct.Stock ?? GetStock(seedProduct.Category, seedProduct.Name);
                product.DiscountRate = seedProduct.DiscountRate ?? GetDiscountRate(seedProduct.Category, seedProduct.Name);
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task ResetProductIdentityAsync(AppDbContext dbContext)
        {
            await dbContext.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"Products\"', 'Id'), COALESCE((SELECT MAX(\"Id\") FROM \"Products\"), 0) + 1, false);");
        }

        private sealed record ProductSeedItem(
            string Name,
            decimal Price,
            string Category,
            string[] Aliases,
            string? ImageUrl = null,
            int? Stock = null,
            decimal? DiscountRate = null)
        {
            public bool Matches(string productName)
            {
                if (string.Equals(productName, Name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return Aliases.Any(alias =>
                    string.Equals(productName, alias, StringComparison.OrdinalIgnoreCase));
            }
        }

        private static string GetImageUrl(string category, string productName)
        {
            if (ProductImages.TryGetValue(productName, out var exactImageUrl))
            {
                return exactImageUrl;
            }

            if (productName.Contains("Telefon", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Dizustu", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Laptop", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kulaklik", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Saat", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("TV", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Televizyon", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Tablet", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Klavye", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1587829741301-dc798b83add3?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Mouse", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1527814050087-3793815479db?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Monitor", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Powerbank", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1609091839311-d5365f9ff1c5?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Hoparlor", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Oyun Konsolu", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1605901309584-818e25960a8f?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Elbise", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1595777457583-95e059d581b8?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Bluz", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Gomlek", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Tisort", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Polo", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Jean", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Pantolon", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1542272604-787c3835535d?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kazak", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Sweatshirt", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1576871337622-98d48d1cf531?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Etek", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1583496661160-fb5886a13d77?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Mont", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Ceket", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Trenckot", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1544022613-e87ca75a784a?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Cizme", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Topuklu", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Terlik", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1603487742131-4160ec999306?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Sneaker", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Ayakkabi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Bot", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1520639888713-7851133b1ed0?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Canta", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Valiz", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1553531384-cc64ac80f931?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Bebek Arabasi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1595975914625-2f8a1f07994b?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Bebek", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Cocuk", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1519238263530-99bdd11df2ea?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Oyuncak", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1558060370-d644479cb6f7?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Mama Sandalyesi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1586000792898-8aa6305f3217?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kahve Makinesi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1517668808822-9ebb02f2a0e6?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kahve", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1447933601403-0c6688de566e?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Nevresim", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Hali", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Lamba", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kitaplik", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1521587760476-6c12a4b040da?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Sandalye", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1503602642458-232111445657?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Robot Supurge", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Tencere", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Yemek Takimi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1556911220-bff31c812dba?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Zeytinyagi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1474979266404-7eaacbcd87c5?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Makarna", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Pirinc", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1551462147-37885acc36f1?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Deterjan", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Bulasik", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1585421514738-01798e348b17?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kedi Mamasi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1589924691995-400dc9ecc119?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Tuvalet Kagidi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1584556812952-905ffd0c611a?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Cikolata", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1511381939415-e44015466834?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Parfum", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1541643600914-78b084683601?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Gunes Kremi", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1620916566398-39f1143ab7be?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Sac", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Sampuan", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1522338242992-e1a54906a8da?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Temizleme", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1556228720-195a672e8a03?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Tiras", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1621605815971-fbc98d665033?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Ruj", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Maskara", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Krem", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Gozluk", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1511499767150-a48a237f0083?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Cuzdan", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Kemer", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1627123424574-724758594e93?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kolye", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Bileklik", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Sapka", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Atki", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1521369909029-2afed882baee?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kosu Bandi", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Dambil", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Yoga", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Bisiklet Kaski", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1507035895480-2b3156c31fc8?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Protein", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1593095948071-474c5cc2989d?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Kamp", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Termos", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Airfryer", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Blender", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1556911220-e15b29be8c8f?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Sarj", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Kilif", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1609081219090-a6d81d3085bf?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Tayt", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1506629905607-d9f297dcb955?auto=format&fit=crop&w=800&q=80";
            }

            if (productName.Contains("Organizer", StringComparison.OrdinalIgnoreCase))
            {
                return "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?auto=format&fit=crop&w=800&q=80";
            }

            return category switch
            {
                "Elektronik" => "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?auto=format&fit=crop&w=800&q=80",
                "Kadin" => "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=800&q=80",
                "Erkek" => "https://images.unsplash.com/photo-1516257984-b1b4d707412e?auto=format&fit=crop&w=800&q=80",
                "Anne & Cocuk" => "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=800&q=80",
                "Ev & Yasam" => "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=800&q=80",
                "Supermarket" => "https://images.unsplash.com/photo-1542838132-92c53300491e?auto=format&fit=crop&w=800&q=80",
                "Kozmetik" => "https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=800&q=80",
                "Ayakkabi & Canta" => "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=800&q=80",
                "Saat & Aksesuar" => "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=800&q=80",
                "Spor & Outdoor" => "https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=800&q=80",
                "Flas Urunler" => "https://images.unsplash.com/photo-1607082349566-187342175e2f?auto=format&fit=crop&w=800&q=80",
                "Cok Satanlar" => "https://images.unsplash.com/photo-1607083206869-4c7672e72a8a?auto=format&fit=crop&w=800&q=80",
                _ => "https://images.unsplash.com/photo-1472851294608-062f824d29cc?auto=format&fit=crop&w=800&q=80"
            };
        }

        private static int GetStock(string category, string productName)
        {
            if (category == "Flas Urunler")
            {
                return 12;
            }

            if (productName.Contains("Telefon", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Laptop", StringComparison.OrdinalIgnoreCase)
                || productName.Contains("Dizustu", StringComparison.OrdinalIgnoreCase))
            {
                return 18;
            }

            return category switch
            {
                "Elektronik" => 35,
                "Supermarket" => 120,
                "Kozmetik" => 80,
                "Cok Satanlar" => 65,
                _ => 45
            };
        }

        private static decimal GetDiscountRate(string category, string productName)
        {
            if (category == "Flas Urunler")
            {
                return 25;
            }

            if (category == "Cok Satanlar")
            {
                return 15;
            }

            if (productName.Contains("Firsat", StringComparison.OrdinalIgnoreCase))
            {
                return 30;
            }

            return category switch
            {
                "Elektronik" => 8,
                "Kadin" => 12,
                "Erkek" => 10,
                "Ayakkabi & Canta" => 14,
                _ => 0
            };
        }
    }
}
