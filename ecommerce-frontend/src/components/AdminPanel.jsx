import { useRef, useState } from 'react'

const categories = [
  'Kadin',
  'Erkek',
  'Anne & Cocuk',
  'Ev & Yasam',
  'Supermarket',
  'Kozmetik',
  'Ayakkabi & Canta',
  'Elektronik',
  'Saat & Aksesuar',
  'Spor & Outdoor',
  'Flas Urunler',
  'Cok Satanlar',
]

const orderStatuses = [
  'Hazırlanıyor',
  'Kargoya Verildi',
  'Teslimata Çıktı',
  'Teslim Edildi',
  'İptal Edildi',
]

function AdminPanel({
  products,
  orders = [],
  message,
  isLoading,
  updatingOrderId,
  deletingOrderId,
  onCreateProduct,
  onUpdateProduct,
  onDeleteProduct,
  onUpdateOrderStatus,
  onDeleteOrder,
}) {
  const formRef = useRef(null)
  const [form, setForm] = useState({
    id: null,
    name: '',
    price: '',
    category: 'Elektronik',
    imageUrl: '',
    imageUrlsText: '',
    stock: '50',
    discountRate: '0',
  })

  const isEditing = form.id !== null
  const imagePreviewUrl = form.imageUrl.trim()
  const extraImageUrls = getExtraImageUrls(form.imageUrlsText)
  const dashboardStats = getDashboardStats(products, orders)
  const lowStockProducts = products.filter((product) => {
    const stock = product.stock ?? 0

    return stock > 0 && stock <= 5
  })

  function handleChange(event) {
    const { name, value } = event.target

    setForm((currentForm) => ({
      ...currentForm,
      [name]: value,
    }))
  }

  function handleEdit(product) {
    setForm({
      id: product.id,
      name: product.name,
      price: String(product.price),
      category: product.category,
      imageUrl: product.imageUrl ?? '',
      imageUrlsText: (product.imageUrls ?? [])
        .filter((imageUrl) => imageUrl !== product.imageUrl)
        .join('\n'),
      stock: String(product.stock ?? 50),
      discountRate: String(product.discountRate ?? 0),
    })

    formRef.current?.scrollIntoView({
      behavior: 'smooth',
      block: 'center',
    })
  }

  function handleCancel() {
    setForm({
      id: null,
      name: '',
      price: '',
      category: 'Elektronik',
      imageUrl: '',
      imageUrlsText: '',
      stock: '50',
      discountRate: '0',
    })
  }

  function handleRemoveExtraImage(imageUrlToRemove) {
    setForm((currentForm) => ({
      ...currentForm,
      imageUrlsText: getExtraImageUrls(currentForm.imageUrlsText)
        .filter((imageUrl) => imageUrl !== imageUrlToRemove)
        .join('\n'),
    }))
  }

  async function handleSubmit(event) {
    event.preventDefault()

    let isSuccessful = false

    if (isEditing) {
      isSuccessful = await onUpdateProduct(form.id, form)
    } else {
      isSuccessful = await onCreateProduct(form)
    }

    if (isSuccessful) {
      handleCancel()
    }
  }

  return (
    <section className="rounded-lg border border-slate-800/80 bg-slate-900/90 p-5 shadow-xl">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <div>
          <h2 className="text-lg font-bold">Admin Panel</h2>
          <p className="mt-1 text-xs text-slate-400">
            Ürün ekle, güncelle, sil ve sipariş yönet
          </p>
        </div>

        {isLoading && (
          <span className="rounded-full bg-slate-950 px-3 py-1 text-xs text-slate-300">
            İşlem yapılıyor
          </span>
        )}
      </div>

      {message && (
        <p className="mt-4 rounded-md border border-slate-800 bg-slate-950 p-3 text-sm text-slate-300">
          {message}
        </p>
      )}

      <div className="mt-5 grid gap-3 sm:grid-cols-2 xl:grid-cols-5">
        <DashboardCard label="Toplam Ürün" value={dashboardStats.totalProducts} />
        <DashboardCard label="Toplam Sipariş" value={dashboardStats.totalOrders} />
        <DashboardCard label="İptal Sipariş" value={dashboardStats.cancelledOrders} />
        <DashboardCard label="Tahmini Gelir" value={dashboardStats.totalRevenue} />
        <DashboardCard label="Düşük Stok" value={dashboardStats.lowStockProducts} />
      </div>

      {lowStockProducts.length > 0 && (
        <div className="mt-5 rounded-lg border border-yellow-500/40 bg-yellow-500/10 p-4">
          <div className="flex flex-wrap items-center justify-between gap-3">
            <div>
              <h3 className="text-sm font-bold text-yellow-200">Düşük Stok Uyarısı</h3>
              <p className="mt-1 text-xs text-yellow-100/80">
                Stoğu 5 veya altında olan ürünler. Düzenle ile hızlıca stok artırabilirsin.
              </p>
            </div>
            <span className="rounded-full bg-yellow-500 px-3 py-1 text-xs font-black text-slate-950">
              {lowStockProducts.length} ürün
            </span>
          </div>

          <div className="mt-3 grid gap-2 md:grid-cols-2 xl:grid-cols-3">
            {lowStockProducts.map((product) => (
              <button
                key={product.id}
                type="button"
                onClick={() => handleEdit(product)}
                className="flex items-center justify-between gap-3 rounded-md border border-yellow-500/30 bg-slate-950/70 px-3 py-2 text-left hover:border-yellow-300"
              >
                <span>
                  <span className="block text-sm font-semibold text-slate-100">
                    {product.name}
                  </span>
                  <span className="text-xs text-slate-400">{product.category}</span>
                </span>
                <span className="rounded-full bg-yellow-500 px-2 py-1 text-xs font-black text-slate-950">
                  Stok {product.stock ?? 0}
                </span>
              </button>
            ))}
          </div>
        </div>
      )}

      <form
        ref={formRef}
        onSubmit={handleSubmit}
        className={`mt-5 rounded-lg border p-4 ${
          isEditing
            ? 'border-orange-500/60 bg-orange-950/20'
            : 'border-slate-800 bg-slate-950'
        }`}
      >
        <div className="mb-3 flex flex-wrap items-center justify-between gap-2">
          <div>
            <h3 className="text-sm font-bold text-slate-100">
              {isEditing ? 'Ürün düzenleniyor' : 'Yeni ürün ekle'}
            </h3>
            {isEditing && (
              <p className="mt-1 text-xs text-orange-300">
                #{form.id} id'li ürün seçildi. Değişiklikleri yapıp Güncelle'ye bas.
              </p>
            )}
          </div>
          {isEditing && (
            <button
              type="button"
              onClick={handleCancel}
              className="rounded-md border border-slate-700 px-3 py-1.5 text-xs font-semibold text-slate-200 hover:border-slate-500"
            >
              Vazgeç
            </button>
          )}
        </div>

        <div className="grid gap-3 lg:grid-cols-[1fr_160px_220px]">
          <input
            name="name"
            value={form.name}
            onChange={handleChange}
            placeholder="Ürün adı"
            className="rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
            required
          />
          <input
            name="price"
            type="number"
            min="0.01"
            step="0.01"
            value={form.price}
            onChange={handleChange}
            placeholder="Fiyat"
            className="rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
            required
          />
          <select
            name="category"
            value={form.category}
            onChange={handleChange}
            className="rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
          >
            {categories.map((category) => (
              <option key={category}>{category}</option>
            ))}
          </select>

          <div className="grid gap-3 rounded-md border border-slate-800 bg-slate-900/60 p-3 lg:col-span-3 md:grid-cols-[1fr_180px]">
            <label className="block">
              <span className="mb-1 block text-xs font-semibold text-slate-300">
                Görsel URL
              </span>
              <input
                name="imageUrl"
                value={form.imageUrl}
                onChange={handleChange}
                placeholder="https://...jpg / .png / webp veya direkt görsel linki"
                className="w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
              />
              <p className="mt-1 text-[11px] text-slate-500">
                Ürün foto linkini buraya yapıştır. Link direkt görsel açıyorsa kartta da görünür.
              </p>
            </label>

            <div className="overflow-hidden rounded-md border border-slate-800 bg-slate-950">
              {imagePreviewUrl ? (
                <img
                  src={imagePreviewUrl}
                  alt="Görsel önizleme"
                  onError={(event) => {
                    event.currentTarget.style.display = 'none'
                  }}
                  className="h-32 w-full bg-slate-950 object-contain p-2"
                />
              ) : (
                <div className="grid h-32 place-items-center px-4 text-center text-xs text-slate-500">
                  URL girince önizleme burada görünür
                </div>
              )}
            </div>
          </div>

          <div className="rounded-md border border-slate-800 bg-slate-900/60 p-3 lg:col-span-3">
            <label className="block">
              <span className="mb-1 block text-xs font-semibold text-slate-300">
                Ek Görsel URL'leri
              </span>
              <textarea
                name="imageUrlsText"
                value={form.imageUrlsText}
                onChange={handleChange}
                rows={4}
                placeholder="Her satıra bir görsel URL'si yaz"
                className="w-full resize-y rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
              />
            </label>
            <p className="mt-1 text-[11px] text-slate-500">
              İlk görsel ana görsel alanında kalır. Buradakiler ürün detay galerisinde görünür.
            </p>

            {extraImageUrls.length > 0 && (
              <div className="mt-3 grid gap-2 sm:grid-cols-2 xl:grid-cols-4">
                {extraImageUrls.map((imageUrl) => (
                  <div
                    key={imageUrl}
                    className="overflow-hidden rounded-md border border-slate-800 bg-slate-950"
                  >
                    <img
                      src={imageUrl}
                      alt="Ek görsel önizleme"
                      onError={(event) => {
                        event.currentTarget.style.display = 'none'
                      }}
                      className="h-24 w-full object-contain p-2"
                    />
                    <div className="border-t border-slate-800 p-2">
                      <p className="truncate text-[11px] text-slate-500">{imageUrl}</p>
                      <button
                        type="button"
                        onClick={() => handleRemoveExtraImage(imageUrl)}
                        className="mt-2 w-full rounded-md border border-red-500/60 px-2 py-1 text-xs font-semibold text-red-200 hover:border-red-300"
                      >
                        Görseli Sil
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>

          <input
            name="stock"
            type="number"
            min="0"
            value={form.stock}
            onChange={handleChange}
            placeholder="Stok"
            className="rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
            required
          />
          <input
            name="discountRate"
            type="number"
            min="0"
            max="90"
            step="0.01"
            value={form.discountRate}
            onChange={handleChange}
            placeholder="İndirim %"
            className="rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
            required
          />
          <button
            type="submit"
            disabled={isLoading}
            className="rounded-md bg-orange-500 px-4 py-2 text-sm font-bold text-slate-950 hover:bg-orange-400 disabled:cursor-not-allowed disabled:bg-slate-600"
          >
            {isEditing ? 'Güncelle' : 'Ekle'}
          </button>
        </div>
      </form>

      <div className="mt-5 grid gap-3 sm:grid-cols-2 xl:grid-cols-3">
        {products.map((product) => (
          <article
            key={product.id}
            className="rounded-lg border border-slate-800 bg-slate-950 p-4"
          >
            {product.imageUrl ? (
              <img
                src={product.imageUrl}
                alt={product.name}
                onError={(event) => {
                  event.currentTarget.style.display = 'none'
                }}
                className="mb-3 aspect-[4/3] w-full rounded-md bg-slate-950 object-contain p-2"
              />
            ) : (
              <div className="mb-3 grid aspect-[4/3] place-items-center rounded-md bg-slate-900 text-xs text-slate-500">
                Görsel yok
              </div>
            )}
            <p className="text-xs font-semibold uppercase tracking-wide text-orange-300">
              {product.category}
            </p>
            <h3 className="mt-2 font-semibold text-slate-100">{product.name}</h3>
            <p className="mt-1 text-sm text-slate-400">{product.price} TL</p>
            <p className="mt-1 text-xs text-slate-500">
              Stok: {product.stock ?? 0} | İndirim: %{product.discountRate ?? 0}
            </p>

            <div className="mt-4 flex gap-2">
              <button
                type="button"
                onClick={() => handleEdit(product)}
                className={`flex-1 rounded-md border px-3 py-2 text-sm font-semibold ${
                  form.id === product.id
                    ? 'border-orange-400 text-orange-300'
                    : 'border-slate-700 text-slate-200 hover:border-orange-400 hover:text-orange-300'
                }`}
              >
                {form.id === product.id ? 'Seçili' : 'Düzenle'}
              </button>
              <button
                type="button"
                onClick={() => onDeleteProduct(product.id)}
                disabled={isLoading}
                className="flex-1 rounded-md border border-red-500/60 px-3 py-2 text-sm font-semibold text-red-200 hover:border-red-300"
              >
                Sil
              </button>
            </div>
          </article>
        ))}
      </div>

      <div className="mt-8 rounded-lg border border-slate-800 bg-slate-950 p-4">
        <div className="flex flex-wrap items-center justify-between gap-3">
          <div>
            <h3 className="text-base font-bold text-slate-100">Sipariş Yönetimi</h3>
            <p className="mt-1 text-xs text-slate-500">
              Sipariş durumunu buradan güncelleyebilirsin.
            </p>
          </div>
          <span className="rounded-full bg-slate-900 px-3 py-1 text-xs text-slate-300">
            {orders.length} sipariş
          </span>
        </div>

        {orders.length === 0 && (
          <p className="mt-4 text-sm text-slate-500">Henüz sipariş yok.</p>
        )}

        {orders.length > 0 && (
          <div className="mt-4 space-y-3">
            {orders.map((order) => (
              <article
                key={order.id}
                className="grid gap-3 rounded-lg border border-slate-800 bg-slate-900 p-3 lg:grid-cols-[1fr_220px_auto_auto]"
              >
                <div>
                  <p className="text-xs font-bold uppercase tracking-wide text-orange-300">
                    Sipariş #{order.id}
                  </p>
                  <h4 className="mt-1 font-semibold text-slate-100">
                    {order.productName}
                  </h4>
                  <p className="mt-1 text-sm text-slate-400">
                    {order.customerName || 'Müşteri bilgisi yok'} - Adet: {order.quantity}
                  </p>
                  <p className="mt-1 text-xs text-slate-500">
                    Adres: {order.shippingAddress || 'Adres bilgisi yok'}
                  </p>
                  <p className="mt-1 text-xs text-slate-500">
                    Ödeme: {getPaymentText(order)}
                  </p>
                </div>

                <select
                  value={order.status || 'Hazırlanıyor'}
                  onChange={(event) => onUpdateOrderStatus(order, event.target.value)}
                  disabled={updatingOrderId === order.id}
                  className="h-10 rounded-md border border-slate-700 bg-slate-950 px-3 text-sm text-white outline-none focus:border-orange-500"
                >
                  {orderStatuses.map((status) => (
                    <option key={status}>{status}</option>
                  ))}
                </select>

                <span className="inline-flex h-10 items-center justify-center rounded-md border border-orange-500/40 bg-orange-500/10 px-3 text-xs font-bold text-orange-300">
                  {updatingOrderId === order.id
                    ? 'Güncelleniyor'
                    : order.status || 'Hazırlanıyor'}
                </span>

                {order.status === 'İptal Edildi' && (
                  <button
                    type="button"
                    onClick={() => onDeleteOrder(order)}
                    disabled={deletingOrderId === order.id}
                    className="h-10 rounded-md border border-red-500/60 px-3 text-xs font-bold text-red-200 hover:border-red-300 disabled:cursor-not-allowed disabled:border-slate-700 disabled:text-slate-500"
                  >
                    {deletingOrderId === order.id ? 'Siliniyor' : 'Sil'}
                  </button>
                )}
              </article>
            ))}
          </div>
        )}
      </div>
    </section>
  )
}

function getExtraImageUrls(imageUrlsText) {
  return String(imageUrlsText ?? '')
    .split('\n')
    .map((imageUrl) => imageUrl.trim())
    .filter(Boolean)
}

function getDashboardStats(products, orders) {
  const priceFormatter = new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY',
  })
  const activeOrders = orders.filter((order) => order.status !== 'İptal Edildi')
  const totalRevenue = activeOrders.reduce(
    (sum, order) => sum + order.productPrice * order.quantity,
    0,
  )

  return {
    totalProducts: products.length,
    totalOrders: orders.length,
    cancelledOrders: orders.filter((order) => order.status === 'İptal Edildi').length,
    totalRevenue: priceFormatter.format(totalRevenue),
    lowStockProducts: products.filter((product) => {
      const stock = product.stock ?? 0

      return stock > 0 && stock <= 5
    }).length,
  }
}

function getPaymentText(order) {
  if (!order.paymentMethod) {
    return 'Ödeme bilgisi yok'
  }

  if (order.cardLastFourDigits) {
    return `${order.paymentMethod} - **** ${order.cardLastFourDigits}`
  }

  return order.paymentMethod
}

function DashboardCard({ label, value }) {
  return (
    <div className="rounded-lg border border-slate-800 bg-slate-950 p-4">
      <p className="text-xs font-semibold uppercase tracking-wide text-slate-500">
        {label}
      </p>
      <p className="mt-2 text-lg font-black text-orange-300">{value}</p>
    </div>
  )
}

export default AdminPanel
