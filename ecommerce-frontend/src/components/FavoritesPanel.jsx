function FavoritesPanel({
  user,
  products,
  favoriteProductIds,
  addingProductId,
  onAddToBasket,
  onToggleFavorite,
}) {
  const favoriteProducts = products.filter((product) =>
    favoriteProductIds.includes(product.id),
  )

  const priceFormatter = new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY',
  })

  function getDiscountedPrice(product) {
    return product.price * (1 - (product.discountRate ?? 0) / 100)
  }

  function isOutOfStock(product) {
    return (product.stock ?? 0) <= 0
  }

  return (
    <section className="rounded-lg border border-slate-800/80 bg-slate-900/90 p-5 shadow-xl">
      <div>
        <h2 className="text-lg font-bold">Favorilerim</h2>
        <p className="mt-1 text-xs text-slate-400">
          Beğendiğin ürünleri burada toplarsın
        </p>
      </div>

      {!user && (
        <p className="mt-4 text-slate-400">
          Favorileri görmek için önce giriş yap.
        </p>
      )}

      {user && favoriteProducts.length === 0 && (
        <p className="mt-4 rounded-lg border border-slate-800 bg-slate-950 p-4 text-sm text-slate-400">
          Henüz favori ürün yok. Ürün kartlarındaki Favori butonuyla ekleyebilirsin.
        </p>
      )}

      {favoriteProducts.length > 0 && (
        <div className="mt-5 grid gap-3 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          {favoriteProducts.map((product) => (
            <article
              key={product.id}
              className="rounded-lg border border-slate-800 bg-slate-950 p-3"
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
                <div className="mb-3 grid aspect-[4/3] place-items-center rounded-md bg-gradient-to-br from-orange-500/20 to-slate-950">
                  <span className="text-3xl font-black text-orange-300">
                    {product.name.charAt(0).toUpperCase()}
                  </span>
                </div>
              )}
              <p className="text-xs font-semibold uppercase tracking-wide text-orange-300">
                {product.category}
              </p>
              <h3 className="mt-2 min-h-10 text-sm font-semibold text-slate-100">
                {product.name}
              </h3>
              <p className="mt-2 text-lg font-bold text-orange-300">
                {priceFormatter.format(getDiscountedPrice(product))}
              </p>
              <p className="mt-1 text-xs text-slate-500">
                Stok: {product.stock ?? 0}
              </p>

              <div className="mt-3 grid grid-cols-2 gap-2">
                <button
                  type="button"
                  onClick={() => onAddToBasket(product.id)}
                  disabled={addingProductId === product.id || isOutOfStock(product)}
                  className="rounded-md bg-orange-500 px-3 py-2 text-xs font-bold text-slate-950 hover:bg-orange-400 disabled:bg-slate-600"
                >
                  {isOutOfStock(product) ? 'Tükendi' : 'Sepete Ekle'}
                </button>
                <button
                  type="button"
                  onClick={() => onToggleFavorite(product.id)}
                  className="rounded-md border border-slate-700 px-3 py-2 text-xs font-semibold text-slate-200 hover:border-red-400 hover:text-red-300"
                >
                  Kaldır
                </button>
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  )
}

export default FavoritesPanel
