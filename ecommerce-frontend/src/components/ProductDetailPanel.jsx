import { useEffect, useState } from 'react'

function ProductDetailPanel({
  product,
  addingProductId,
  favoriteProductIds,
  reviews,
  reviewMessage,
  isReviewsLoading,
  isReviewSubmitting,
  onBack,
  onAddToBasket,
  onToggleFavorite,
  onAddReview,
}) {
  const priceFormatter = new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY',
  })
  const productImages = product ? getProductImages(product) : []
  const [selectedImageUrl, setSelectedImageUrl] = useState(productImages[0] ?? '')
  const [reviewForm, setReviewForm] = useState({
    rating: '5',
    comment: '',
  })

  useEffect(() => {
    setSelectedImageUrl(productImages[0] ?? '')
    setReviewForm({
      rating: '5',
      comment: '',
    })
  }, [product?.id, productImages[0]])

  if (!product) {
    return (
      <section className="rounded-lg border border-slate-800 bg-slate-900/90 p-5">
        <p className="text-slate-300">Ürün bulunamadı.</p>
        <button
          type="button"
          onClick={onBack}
          className="mt-4 rounded-md bg-orange-500 px-4 py-2 text-sm font-bold text-slate-950"
        >
          Geri Dön
        </button>
      </section>
    )
  }

  const hasDiscount = (product.discountRate ?? 0) > 0
  const finalPrice = product.price * (1 - (product.discountRate ?? 0) / 100)
  const isOutOfStock = (product.stock ?? 0) <= 0
  const averageRating = getAverageRating(reviews)

  async function handleReviewSubmit(event) {
    event.preventDefault()

    const isSuccessful = await onAddReview(product.id, {
      rating: Number(reviewForm.rating),
      comment: reviewForm.comment,
    })

    if (isSuccessful) {
      setReviewForm({
        rating: '5',
        comment: '',
      })
    }
  }

  return (
    <section className="rounded-lg border border-slate-800 bg-slate-900/90 p-5 shadow-xl">
      <button
        type="button"
        onClick={onBack}
        className="rounded-md border border-slate-700 px-3 py-2 text-sm font-semibold text-slate-200 hover:border-orange-400 hover:text-orange-300"
      >
        Ürünlere Dön
      </button>

      <div className="mt-5 grid gap-6 lg:grid-cols-[420px_1fr]">
        <div className="overflow-hidden rounded-lg border border-slate-800 bg-slate-950">
          {selectedImageUrl ? (
            <img
              src={selectedImageUrl}
              alt={product.name}
              onError={(event) => {
                event.currentTarget.style.display = 'none'
              }}
              className="aspect-square w-full bg-slate-950 object-contain p-4"
            />
          ) : (
            <div className="grid aspect-square place-items-center text-6xl font-black text-orange-400">
              {product.name.charAt(0).toUpperCase()}
            </div>
          )}
          {productImages.length > 1 && (
            <div className="grid grid-cols-4 gap-2 border-t border-slate-800 p-3">
              {productImages.map((imageUrl) => (
                <button
                  key={imageUrl}
                  type="button"
                  onClick={() => setSelectedImageUrl(imageUrl)}
                  className={`overflow-hidden rounded-md border bg-slate-950 p-1 ${
                    selectedImageUrl === imageUrl
                      ? 'border-orange-400'
                      : 'border-slate-800 hover:border-orange-500/70'
                  }`}
                >
                  <img
                    src={imageUrl}
                    alt={`${product.name} görseli`}
                    className="aspect-square w-full object-contain"
                  />
                </button>
              ))}
            </div>
          )}
        </div>

        <div>
          <p className="inline-flex rounded-full bg-slate-950 px-3 py-1 text-xs font-bold uppercase text-orange-300">
            {product.category}
          </p>
          <h2 className="mt-4 text-3xl font-black text-slate-100">{product.name}</h2>
          <p className="mt-2 text-sm text-slate-400">
            Ürün detayları, teknik bilgiler, kullanıcı yorumları ve puanlamalar.
          </p>

          <div className="mt-5 flex flex-wrap items-end gap-3">
            {hasDiscount && (
              <span className="text-sm text-slate-500 line-through">
                {priceFormatter.format(product.price)}
              </span>
            )}
            <span className="text-3xl font-black text-orange-300">
              {priceFormatter.format(finalPrice)}
            </span>
            {hasDiscount && (
              <span className="rounded-full bg-red-600 px-2 py-1 text-xs font-black">
                %{product.discountRate} indirim
              </span>
            )}
          </div>

          <div className="mt-5 grid gap-3 sm:grid-cols-3">
            <InfoCard label="Puan" value={averageRating} />
            <InfoCard label="Stok" value={product.stock ?? 0} />
            <InfoCard label="Ürün Kodu" value={`PRD-${product.id}`} />
          </div>

          <div className="mt-5 flex flex-wrap gap-2">
            <button
              type="button"
              onClick={() => onAddToBasket(product.id)}
              disabled={addingProductId === product.id || isOutOfStock}
              className="rounded-md bg-orange-500 px-5 py-2 text-sm font-bold text-slate-950 hover:bg-orange-400 disabled:cursor-not-allowed disabled:bg-slate-600"
            >
              {isOutOfStock
                ? 'Tükendi'
                : addingProductId === product.id
                  ? 'Ekleniyor...'
                  : 'Sepete Ekle'}
            </button>
            <button
              type="button"
              onClick={() => onToggleFavorite(product.id)}
              className={`rounded-md border px-5 py-2 text-sm font-semibold ${
                favoriteProductIds.includes(product.id)
                  ? 'border-red-400 text-red-300'
                  : 'border-slate-700 text-slate-200 hover:border-red-400 hover:text-red-300'
              }`}
            >
              {favoriteProductIds.includes(product.id)
                ? 'Favoriden Çıkar'
                : 'Favoriye Ekle'}
            </button>
          </div>
        </div>
      </div>

      <div className="mt-6 grid gap-4 lg:grid-cols-[1fr_420px]">
        <div className="rounded-lg border border-slate-800 bg-slate-950 p-4">
          <h3 className="font-bold text-slate-100">Teknik Bilgiler</h3>
          <div className="mt-4 grid gap-2 text-sm">
            <SpecRow label="Kategori" value={product.category} />
            <SpecRow label="Stok Durumu" value={isOutOfStock ? 'Tükendi' : 'Stokta var'} />
            <SpecRow label="İndirim Oranı" value={`%${product.discountRate ?? 0}`} />
            <SpecRow label="Liste Fiyatı" value={priceFormatter.format(product.price)} />
            <SpecRow label="Satış Fiyatı" value={priceFormatter.format(finalPrice)} />
          </div>
        </div>

        <div className="rounded-lg border border-slate-800 bg-slate-950 p-4">
          <h3 className="font-bold text-slate-100">Yorumlar ve Puanlamalar</h3>

          <form onSubmit={handleReviewSubmit} className="mt-4 rounded-md bg-slate-900 p-3">
            <label className="block text-xs font-semibold text-slate-400">
              Puan
            </label>
            <select
              value={reviewForm.rating}
              onChange={(event) =>
                setReviewForm((currentForm) => ({
                  ...currentForm,
                  rating: event.target.value,
                }))
              }
              className="mt-1 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
            >
              <option value="5">5 - Çok iyi</option>
              <option value="4">4 - İyi</option>
              <option value="3">3 - Orta</option>
              <option value="2">2 - Zayıf</option>
              <option value="1">1 - Kötü</option>
            </select>

            <label className="mt-3 block text-xs font-semibold text-slate-400">
              Yorum
            </label>
            <textarea
              value={reviewForm.comment}
              onChange={(event) =>
                setReviewForm((currentForm) => ({
                  ...currentForm,
                  comment: event.target.value,
                }))
              }
              rows={3}
              placeholder="Ürün hakkında yorum yaz..."
              className="mt-1 w-full resize-none rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
              required
            />

            <button
              type="submit"
              disabled={isReviewSubmitting}
              className="mt-3 rounded-md bg-orange-500 px-4 py-2 text-sm font-bold text-slate-950 hover:bg-orange-400 disabled:cursor-not-allowed disabled:bg-slate-600"
            >
              {isReviewSubmitting ? 'Gönderiliyor' : 'Yorum Ekle'}
            </button>
          </form>

          {reviewMessage && (
            <p className="mt-3 rounded-md bg-slate-900 p-3 text-sm text-slate-300">
              {reviewMessage}
            </p>
          )}

          <div className="mt-4 space-y-3">
            {isReviewsLoading && (
              <p className="text-sm text-slate-500">Yorumlar yükleniyor.</p>
            )}

            {!isReviewsLoading && reviews.length === 0 && (
              <p className="text-sm text-slate-500">Henüz yorum yok.</p>
            )}

            {reviews.map((review) => (
              <article key={review.id} className="rounded-md bg-slate-900 p-3">
                <div className="flex items-center justify-between gap-2">
                  <p className="text-sm font-bold text-slate-100">
                    {review.userName || 'Kullanıcı'}
                  </p>
                  <span className="text-xs font-bold text-orange-300">
                    {review.rating}/5
                  </span>
                </div>
                <p className="mt-1 text-sm text-slate-400">{review.comment}</p>
              </article>
            ))}
          </div>
        </div>
      </div>
    </section>
  )
}

function InfoCard({ label, value }) {
  return (
    <div className="rounded-lg border border-slate-800 bg-slate-950 p-3">
      <p className="text-xs uppercase tracking-wide text-slate-500">{label}</p>
      <p className="mt-1 font-black text-slate-100">{value}</p>
    </div>
  )
}

function SpecRow({ label, value }) {
  return (
    <div className="flex justify-between gap-4 border-b border-slate-900 pb-2 text-slate-300">
      <span className="text-slate-500">{label}</span>
      <span className="font-semibold text-slate-100">{value}</span>
    </div>
  )
}

function getAverageRating(reviews) {
  if (reviews.length === 0) {
    return '-'
  }

  const totalRating = reviews.reduce((sum, review) => sum + review.rating, 0)

  return `${(totalRating / reviews.length).toFixed(1)}/5`
}

function getProductImages(product) {
  const imageUrls = Array.isArray(product.imageUrls) ? product.imageUrls : []

  if (product.imageUrl && !imageUrls.includes(product.imageUrl)) {
    return [product.imageUrl, ...imageUrls]
  }

  return imageUrls
}

export default ProductDetailPanel
