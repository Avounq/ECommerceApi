# ECommerceApi

ASP.NET Core Web API, PostgreSQL, Docker Compose ve React/Vite frontend ile hazırlanmış demo e-ticaret projesi.

## Özellikler

- JWT access token ve refresh token authentication
- Role-based authorization (`User`, `Admin`)
- Register, login, refresh token, logout, profil ve şifre değiştirme
- Ürün listeleme, arama, kategori filtresi, stok filtresi ve sıralama
- Admin ürün ekleme, düzenleme, silme
- Ürünlerde ana görsel ve ek görsel galerisi
- Ürün detay sayfası, yorum ve puanlama
- Sepet yönetimi
- Checkout akışı, adres ve demo ödeme tipi seçimi
- Kullanıcı siparişleri ve sipariş iptali
- Admin sipariş yönetimi
- Background worker ile sipariş durum ilerletme
- Global exception middleware
- Request logging middleware
- CORS ayarı
- Swagger/OpenAPI
- Docker Compose ile API + PostgreSQL

## Teknolojiler

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker Compose
- React
- Vite
- Tailwind CSS

## Portlar

- API: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger/index.html`
- PostgreSQL: `localhost:5434`
- Frontend: `http://localhost:5173`

## Docker ile Çalıştırma

Kök dizinde:

```powershell
docker compose up --build
```

Veritabanı volume ile korunur. Verileri silmek istemiyorsan şunu kullanma:

```powershell
docker compose down -v
```

## Frontend Çalıştırma

```powershell
cd ecommerce-frontend
& "C:\Program Files\nodejs\npm.cmd" run dev
```

## Build Kontrolü

Backend:

```powershell
dotnet build .\ECommerceApi.slnx
```

Frontend:

```powershell
cd ecommerce-frontend
& "C:\Program Files\nodejs\npm.cmd" run build
```

## Önemli Akışlar

### Auth

- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/refresh`
- `POST /api/auth/logout`
- `GET /api/auth/me`
- `PUT /api/auth/updateprofile`
- `PUT /api/auth/sifre-degistir`

### Ürünler

- Kullanıcı ürünleri görebilir.
- Admin ürün ekleyebilir, düzenleyebilir ve silebilir.
- Ürünlerde stok, kategori, indirim, ana görsel ve ek görseller tutulur.
- Ürün detayında yorum ve puanlama bulunur.

### Sepet ve Sipariş

- Kullanıcı sepete ürün ekleyebilir.
- Stok sınırı kontrol edilir.
- Checkout sonrası sepet boşaltılır ve sipariş oluşur.
- Sipariş durumları otomatik ilerler:
  `Hazırlanıyor -> Kargoya Verildi -> Teslimata Çıktı -> Teslim Edildi`
- Kullanıcı teslim edilmemiş siparişi iptal edebilir.
- Admin iptal edilen siparişi silebilir.

## Notlar

- Migration’lar API başlarken otomatik uygulanır.
- ProductSeeder mevcut ürün varsa veritabanını ezmez.
- Docker build sonrası ürün/görsel değişikliklerinin kalıcı olması için PostgreSQL volume korunmalıdır.
