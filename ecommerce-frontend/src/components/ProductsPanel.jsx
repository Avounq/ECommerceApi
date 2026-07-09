function ProductsPanel({
  user,
  products,
  message,
  isLoading,
  addingProductId,
  selectedCategory,
  stockFilter,
  search,
  favoriteProductIds,
  onSearchChange,
  onSearchClear,
  onCategoryChange,
  onStockFilterChange,
  onOpenProduct,
  onAddToBasket,
  onToggleFavorite,
}) {
  const categories = [
    { label: 'Kadın', value: 'Kadin', isNew: true },
    { label: 'Erkek', value: 'Erkek' },
    { label: 'Anne & Çocuk', value: 'Anne & Cocuk' },
    { label: 'Ev & Yaşam', value: 'Ev & Yasam' },
    { label: 'Süpermarket', value: 'Supermarket' },
    { label: 'Kozmetik', value: 'Kozmetik' },
    { label: 'Ayakkabı & Çanta', value: 'Ayakkabi & Canta' },
    { label: 'Elektronik', value: 'Elektronik' },
    { label: 'Saat & Aksesuar', value: 'Saat & Aksesuar' },
    { label: 'Spor & Outdoor', value: 'Spor & Outdoor' },
    { label: 'Flaş Ürünler', value: 'Flas Urunler', isNew: true },
    { label: 'Çok Satanlar', value: 'Cok Satanlar', isNew: true },
  ]

  const activeCategoryLabel =
    selectedCategory === ''
      ? 'Tüm Ürünler'
      : categories.find((category) => category.value === selectedCategory)?.label ?? 'Ürünler'

  const priceFormatter = new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY',
  })

  function getDiscountedPrice(product) {
    return product.price * (1 - (product.discountRate ?? 0) / 100)
  }

  function hasDiscount(product) {
    return (product.discountRate ?? 0) > 0
  }

  function isOutOfStock(product) {
    return (product.stock ?? 0) <= 0
  }

  function isLowStock(product) {
    const stock = product.stock ?? 0

    return stock > 0 && stock <= 5
  }

  const visibleProducts = products.filter((product) => {
    if (stockFilter === 'inStock') {
      return !isOutOfStock(product)
    }

    if (stockFilter === 'lowStock') {
      return isLowStock(product)
    }

    return true
  })

  return (
    <section className="rounded-lg border border-slate-800 bg-slate-900/90 p-5 shadow-xl">
      <div className="flex items-center justify-between gap-4">
        <div>
          <h2 className="text-lg font-bold">{activeCategoryLabel}</h2>
          <p className="mt-1 text-xs text-slate-400">
            {visibleProducts.length > 0
              ? `${visibleProducts.length} ürün listeleniyor`
              : 'Ürün vitrini'}
          </p>
        </div>

        {isLoading && (
          <span className="rounded-full bg-slate-950 px-3 py-1 text-xs text-slate-300">
            Yükleniyor
          </span>
        )}
      </div>

      {user && (
        <div className="mt-4 flex flex-col gap-2 sm:flex-row">
          <label className="relative flex-1">
            <span className="sr-only">Ürün ara</span>
            <input
              value={search}
              onChange={onSearchChange}
              placeholder="Ürün, kategori veya marka ara"
              className="w-full rounded-full border border-slate-700 bg-slate-950 px-4 py-2.5 pr-24 text-sm text-slate-100 outline-none transition placeholder:text-slate-500 focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            />
            {search && (
              <button
                type="button"
                onClick={onSearchClear}
                className="absolute right-2 top-1/2 -translate-y-1/2 rounded-full px-3 py-1 text-xs font-semibold text-slate-400 hover:bg-slate-800 hover:text-orange-300"
              >
                Temizle
              </button>
            )}
          </label>
        </div>
      )}

      {user && (
        <div className="mt-3 flex flex-wrap gap-2">
          <StockFilterButton
            isActive={stockFilter === 'all'}
            onClick={() => onStockFilterChange('all')}
          >
            Tüm stoklar
          </StockFilterButton>
          <StockFilterButton
            isActive={stockFilter === 'inStock'}
            onClick={() => onStockFilterChange('inStock')}
          >
            Stokta olanlar
          </StockFilterButton>
          <StockFilterButton
            isActive={stockFilter === 'lowStock'}
            onClick={() => onStockFilterChange('lowStock')}
          >
            Düşük stok
          </StockFilterButton>
        </div>
      )}

      {!user && (
        <p className="mt-4 text-slate-400">
          Ürünleri görmek için önce giriş yap.
        </p>
      )}

      {user && (
        <div className="mt-4 rounded-md border border-slate-800 bg-slate-950/95 p-2">
          <div className="flex flex-wrap items-center gap-1.5">
            <span className="mr-1 flex items-center gap-1.5 px-2 py-1.5 text-[11px] font-black text-slate-300">
              <span className="text-base leading-none">=</span>
              Kategoriler
            </span>
            <button
              type="button"
              onClick={() => onCategoryChange('')}
              className={`flex items-center gap-1.5 rounded-full border px-2.5 py-1.5 text-[11px] font-semibold ${
                selectedCategory === ''
                  ? 'border-orange-500 bg-orange-500 text-slate-950'
                  : 'border-slate-800 text-slate-200 hover:border-orange-500 hover:text-orange-300'
              }`}
            >
              Tüm Ürünler
            </button>
            {categories.map((category) => (
              <button
                key={category.label}
                type="button"
                onClick={() => onCategoryChange(category.value)}
                className={`flex items-center gap-1.5 rounded-full border px-2.5 py-1.5 text-[11px] font-semibold ${
                  selectedCategory === category.value
                    ? 'border-orange-500 bg-orange-500 text-slate-950'
                    : 'border-slate-800 text-slate-200 hover:border-orange-500 hover:text-orange-300'
                }`}
              >
                {category.label}
                {category.isNew && (
                  <span className="rounded-full bg-red-600 px-1.5 py-0.5 text-[9px] font-bold leading-none text-white">
                    Yeni
                  </span>
                )}
              </button>
            ))}
          </div>
        </div>
      )}

      {message && (
        <p className="mt-4 rounded-md bg-red-950/40 p-3 text-sm text-red-200">
          {message}
        </p>
      )}

      {user && !message && visibleProducts.length === 0 && !isLoading && (
        <p className="mt-4 text-slate-400">Bu filtreye uygun ürün bulunamadı.</p>
      )}

      {visibleProducts.length > 0 && (
        <div className="mt-5 grid gap-3 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          {visibleProducts.map((product) => (
            <article
              key={product.id}
              onClick={() => onOpenProduct(product.id)}
              className="cursor-pointer rounded-lg border border-slate-800 bg-slate-950 p-3 shadow-sm transition hover:-translate-y-0.5 hover:border-orange-500/70 hover:shadow-orange-950/30"
            >
              <button
                type="button"
                onClick={(event) => {
                  event.stopPropagation()
                  onToggleFavorite(product.id)
                }}
                className={`float-right rounded-full px-2 py-1 text-xs font-bold ${
                  favoriteProductIds.includes(product.id)
                    ? 'bg-red-500 text-white'
                    : 'border border-slate-700 text-slate-300 hover:border-red-400 hover:text-red-300'
                }`}
              >
                Favori
              </button>
              <div className="relative mb-3 overflow-hidden rounded-md bg-slate-900">
                {hasDiscount(product) && (
                  <span className="absolute left-2 top-2 rounded-full bg-red-600 px-2 py-1 text-[10px] font-black text-white">
                    %{product.discountRate} indirim
                  </span>
                )}
                {isOutOfStock(product) && (
                  <span className="absolute right-2 top-2 rounded-full bg-slate-950/90 px-2 py-1 text-[10px] font-black text-slate-200">
                    Tükendi
                  </span>
                )}
                {isLowStock(product) && (
                  <span className="absolute right-2 top-2 rounded-full bg-yellow-500 px-2 py-1 text-[10px] font-black text-slate-950">
                    Son {product.stock}
                  </span>
                )}
                {product.imageUrl ? (
                  <img
                    src={product.imageUrl}
                    alt={product.name}
                    onError={(event) => {
                      event.currentTarget.style.display = 'none'
                    }}
                    className="aspect-[4/3] w-full bg-slate-950 object-contain p-2"
                  />
                ) : (
                  <div className="grid aspect-[4/3] place-items-center bg-gradient-to-br from-slate-800 to-slate-950">
                    <span className="text-3xl font-black text-orange-500/80">
                      {product.name.charAt(0).toUpperCase()}
                    </span>
                  </div>
                )}
              </div>
              <p className="inline-flex rounded-full bg-slate-900 px-2 py-1 text-[10px] font-semibold uppercase tracking-wide text-slate-400">
                {product.category ?? 'Kategori yok'}
              </p>
              <h3 className="mt-3 min-h-10 text-sm font-semibold text-slate-100">
                {product.name}
              </h3>
              <div className="mt-3">
                {hasDiscount(product) && (
                  <p className="text-xs text-slate-500 line-through">
                    {priceFormatter.format(product.price)}
                  </p>
                )}
                <p className="text-lg font-bold text-orange-300">
                  {priceFormatter.format(getDiscountedPrice(product))}
                </p>
                <p className="mt-1 text-xs text-slate-500">
                  Stok: {product.stock ?? 0}
                </p>
              </div>

              <button
                type="button"
                onClick={(event) => {
                  event.stopPropagation()
                  onAddToBasket(product.id)
                }}
                disabled={addingProductId === product.id || isOutOfStock(product)}
                className="mt-3 w-full rounded-md bg-orange-500 px-3 py-2 text-xs font-bold text-slate-950 hover:bg-orange-400 disabled:cursor-not-allowed disabled:bg-slate-600"
              >
                {isOutOfStock(product)
                  ? 'Tükendi'
                  : addingProductId === product.id
                    ? 'Ekleniyor...'
                    : 'Sepete Ekle'}
              </button>
            </article>
          ))}
        </div>
      )}
    </section>
  )
}

function StockFilterButton({ isActive, onClick, children }) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`rounded-full border px-3 py-1.5 text-xs font-semibold ${
        isActive
          ? 'border-orange-500 bg-orange-500 text-slate-950'
          : 'border-slate-800 bg-slate-950 text-slate-300 hover:border-orange-500 hover:text-orange-300'
      }`}
    >
      {children}
    </button>
  )
}

export default ProductsPanel
