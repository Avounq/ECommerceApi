# ECommerceApi API Contract

## Base URL

Local Docker:

```text
http://localhost:8080
http://localhost:8080/swagger

Auth Header
Authorization: Bearer {accessToken}

Auth
Register
Auth: Yok
Request:
{
  "username": "kagan",
  "email": "kagan@example.com",
  "password": "123456",
  "firstName": "KaÄŸan Arda",
  "lastName": "Kandak"
}
Response:
{
  "id": 1,
  "username": "kagan",
  "email": "kagan@example.com",
  "firstName": "KaÄŸan Arda",
  "lastName": "Kandak",
  "role": "User",
  "createdAt": "2026-07-06T..."
}

Login
POST /api/auth/login
Auth: Yok
Request:
{
  "username": "kagan",
  "password": "123456"
}
Response:
{
  "accessToken": "...",
  "refreshToken": "...",
  "accessTokenExpiration": "2026-07-06T...",
  "refreshTokenExpiration": "2026-07-13T..."
}
Refresh Token
POST /api/auth/refresh
Auth: Yok
Request:
{
  "accessToken": "...",
  "refreshToken": "..."
}
Response:
{
  "accessToken": "...",
  "refreshToken": "...",
  "accessTokenExpiration": "2026-07-06T...",
  "refreshTokenExpiration": "2026-07-13T..."
}
Logout
POST /api/auth/logout
Auth: Gerekli
Request: Body yok
Response:
{
  "message": "Ã‡Ä±kÄ±ÅŸ baÅŸarÄ±lÄ±."
}

Me
GET /api/auth/me
Auth: Gerekli
Response:
{
  "id": 1,
  "username": "kagan",
  "email": "kagan@example.com",
  "firstName": "KaÄŸan Arda",
  "lastName": "Kandak",
  "role": "User",
  "createdAt": "2026-07-06T..."
}
Profile Update
PUT /api/auth/updateprofile
Auth: Gerekli
Request:
{
  "email": "new@example.com",
  "firstName": "KaÄŸan Arda",
  "lastName": "Kandak"
}
Response:
{
  "id": 1,
  "username": "kagan",
  "email": "new@example.com",
  "firstName": "KaÄŸan Arda",
  "lastName": "Kandak",
  "role": "User",
  "createdAt": "2026-07-06T..."
}
Change Password
PUT /api/auth/sifre-degistir
Auth: Gerekli
Request:
{
  "currentPassword": "123456",
  "newPassword": "654321"
}
Response:
{
  "message": "Åifre BaÅŸarÄ±yla GÃ¼ncellendi."
}
Products
Get Products
GET /api/products
Auth: Admin
Query Ã¶rnekleri:
/api/products?pageNumber=1&pageSize=10
/api/products?search=phone
/api/products?sortBy=price

Response:
{
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "data": [
    {
      "id": 1,
      "name": "Telefon",
      "price": 25000,
      "stock": 10
    }
  ]
}
Get Product By Id
GET /api/products/{id}
Auth: Admin
Response:
{
  "id": 1,
  "name": "Telefon",
  "price": 25000,
  "stock": 10
}
Create Product
POST /api/products
Auth: Admin
Content-Type: multipart/form-data
Request alanlarÄ±:
name
price
stock

Update Product
PUT /api/products/{id}
Auth: Admin
Content-Type: multipart/form-data
Request alanlarÄ±:
name
price
stock

Delete Product
DELETE /api/products/{id}
Auth: Admin
Response:
{
  "message": "ÃœrÃ¼n silindi."
}
Baskets
Get My Basket
GET /api/baskets
Auth: Gerekli
Response:
{
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "data": [
    {
      "id": 7,
      "productId": 1,
      "quantity": 2
    }
  ]
}
Add Basket Item
POST /api/baskets
Auth: Gerekli
Content-Type: multipart/form-data
Request alanlarÄ±:
productId
quantity
Response:
{
  "id": 7,
  "productId": 1,
  "quantity": 2
}
Update Basket Item
PUT /api/baskets/{id}
Auth: Gerekli
Content-Type: multipart/form-data
Request alanlarÄ±:
productId
quantity
Response:
"Sepet BaÅŸarÄ±yla GÃ¼ncellendi"

Delete Basket Item
DELETE /api/baskets/{id}
Auth: Gerekli
Response:
"Girilen Ä°d'li sepet baÅŸarÄ±yla silindi."

Orders
Checkout
POST /api/orders/checkout
Auth: Gerekli
Request: Body yok
Response:
[
  {
    "id": 10,
    "customerId": 3,
    "customerName": "KaÄŸan Arda Kandak",
    "productId": 1,
    "productName": "Telefon",
    "productPrice": 25000,
    "quantity": 2
  }
]

Not:
Sepetteki Ã¼rÃ¼nleri sipariÅŸe Ã§evirir.
Checkout sonrasÄ± sepet temizlenir.
KullanÄ±cÄ± iÃ§in customer kaydÄ± yoksa otomatik oluÅŸturulur.

My Orders
GET /api/orders/my-orders
Auth: Gerekli
Response:
[
  {
    "id": 10,
    "customerId": 3,
    "customerName": "KaÄŸan Arda Kandak",
    "productId": 1,
    "productName": "Telefon",
    "productPrice": 25000,
    "quantity": 2
  }
]
Get All Orders
GET /api/orders
Auth: Admin
Response:
{
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "data": [
    {
      "id": 10,
      "customerId": 3,
      "customerName": "KaÄŸan Arda Kandak",
      "productId": 1,
      "productName": "Telefon",
      "productPrice": 25000,
      "quantity": 2
    }
  ]
}
Get Order By Id
GET /api/orders/{id}
Auth: Admin
Response:
{
  "id": 10,
  "customerId": 3,
  "customerName": "KaÄŸan Arda Kandak",
  "productId": 1,
  "productName": "Telefon",
  "productPrice": 25000,
  "quantity": 2
}
Create Order Manually
POST /api/orders
Auth: Admin
Content-Type: multipart/form-data
Request alanlarÄ±:
customerId
productId
quantity

Update Order
PUT /api/orders/{id}
Auth: Admin
Content-Type: multipart/form-data
Request alanlarÄ±:
customerId
productId
quantity

Delete Order
DELETE /api/orders/{id}
Auth: Admin
Response:
"Bu id'ye sahip sipariÅŸ baÅŸarÄ±yla silinmiÅŸtir."

Error Format
Global error response:
{
  "success": false,
  "statusCode": 404,
  "message": "SipariÅŸ bulunamadÄ±."
}
YaygÄ±n status kodlarÄ±:
200 OK
201 Created
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
500 Internal Server Error



Daha basit Ã¶zeti ÅŸu:

- `Auth`: giriÅŸ, kayÄ±t, profil, ÅŸifre
- `Products`: Ã¼rÃ¼nler, admin tarafÄ±
- `Baskets`: kullanÄ±cÄ± sepeti
- `Orders`: checkout ve sipariÅŸler
- `Error Format`: hata geldiÄŸinde frontend ne bekleyecek
