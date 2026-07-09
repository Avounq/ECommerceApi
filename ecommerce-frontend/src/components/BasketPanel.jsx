function BasketPanel({
  user,
  products,
  basketItems,
  message,
  isLoading,
  deletingBasketItemId,
  updatingBasketItemId,
  onDeleteBasketItem,
  onUpdateBasketQuantity,
  isCheckoutLoading,
  checkoutInfo,
  onCheckoutInfoChange,
  onCheckout,
}) {
  const totalQuantity = basketItems.reduce((sum, item) => sum + item.quantity, 0)
  const totalPrice = basketItems.reduce((sum, item) => {
    const product = products.find((current) => current.id === item.productId)

    return sum + getFinalPrice(product) * item.quantity
  }, 0)

  const priceFormatter = new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY',
  })

  function getFinalPrice(product) {
    if (!product) {
      return 0
    }

    return product.price * (1 - (product.discountRate ?? 0) / 100)
  }

  return (
    <section className="rounded-lg border border-slate-800/80 bg-slate-900/90 p-5 shadow-xl">
      <div className="flex items-center justify-between gap-4">
        <div>
          <h2 className="text-lg font-bold">Sepetim</h2>
          <p className="mt-1 text-xs text-slate-400">
            {totalQuantity > 0 ? `${totalQuantity} ürün sepette` : 'Sepetin boş'}
          </p>
        </div>

        {isLoading && (
          <span className="rounded-full bg-slate-950 px-3 py-1 text-xs text-slate-300">
            Yükleniyor
          </span>
        )}
      </div>

      {!user && (
        <p className="mt-4 text-slate-400">
          Sepeti görmek için önce giriş yap.
        </p>
      )}

      {message && (
        <p className="mt-4 rounded-md bg-slate-950 p-3 text-sm text-slate-300">
          {message}
        </p>
      )}

      {user && basketItems.length === 0 && !isLoading && (
        <p className="mt-4 text-slate-400">Sepet boş.</p>
      )}

      {basketItems.length > 0 && (
        <div className="mt-5 grid gap-4 lg:grid-cols-[1fr_320px]">
          <div className="space-y-3">
            {basketItems.map((item) => {
              const product = products.find((current) => current.id === item.productId)
              const hasReachedStockLimit = product
                ? item.quantity >= (product.stock ?? 0)
                : false

              return (
                <article
                  key={item.id}
                  className="grid gap-4 rounded-lg border border-slate-800 bg-slate-950 p-4 transition hover:border-orange-500/70 sm:grid-cols-[1fr_auto]"
                >
                  <div>
                    <p className="text-xs font-semibold uppercase tracking-wide text-orange-300">
                      {product?.category ?? 'Ürün'}
                    </p>
                    <h3 className="mt-1 font-semibold text-slate-100">
                      {product?.name ?? `Ürün #${item.productId}`}
                    </h3>
                    <p className="mt-1 text-sm text-slate-400">
                      {product ? priceFormatter.format(getFinalPrice(product)) : `ProductId: ${item.productId}`}
                    </p>
                  </div>

                  <div className="flex items-center gap-3 sm:flex-col sm:items-end">
                    <div className="flex items-center rounded-full border border-slate-800 bg-slate-900 p-1">
                      <button
                        type="button"
                        onClick={() => onUpdateBasketQuantity(item, item.quantity - 1)}
                        disabled={updatingBasketItemId === item.id}
                        className="grid h-7 w-7 place-items-center rounded-full text-sm font-bold text-slate-300 hover:bg-slate-800 hover:text-orange-300 disabled:cursor-not-allowed disabled:text-slate-600"
                      >
                        -
                      </button>
                      <span className="min-w-8 text-center text-sm font-bold text-orange-300">
                        {updatingBasketItemId === item.id ? '...' : item.quantity}
                      </span>
                      <button
                        type="button"
                        onClick={() => onUpdateBasketQuantity(item, item.quantity + 1)}
                        disabled={updatingBasketItemId === item.id || hasReachedStockLimit}
                        className="grid h-7 w-7 place-items-center rounded-full text-sm font-bold text-slate-300 hover:bg-slate-800 hover:text-orange-300 disabled:cursor-not-allowed disabled:text-slate-600"
                      >
                        +
                      </button>
                    </div>
                    {hasReachedStockLimit && (
                      <p className="text-xs text-yellow-300">
                        Stok sınırına ulaşıldı
                      </p>
                    )}

                    <button
                      type="button"
                      onClick={() => onDeleteBasketItem(item.id)}
                      disabled={deletingBasketItemId === item.id}
                      className="rounded-md border border-red-500/60 px-3 py-1 text-sm font-semibold text-red-200 hover:border-red-300 hover:text-red-100 disabled:cursor-not-allowed disabled:border-slate-700 disabled:text-slate-500"
                    >
                      {deletingBasketItemId === item.id
                        ? 'Çıkarılıyor...'
                        : 'Sepetten Çıkar'}
                    </button>
                  </div>
                </article>
              )
            })}
          </div>

          <aside className="h-fit rounded-lg border border-orange-500/30 bg-slate-950 p-4 shadow-lg shadow-orange-950/20">
            <p className="text-sm font-semibold text-slate-200">Sipariş Özeti</p>
            <div className="mt-4 space-y-2 text-sm text-slate-400">
              <div className="flex justify-between">
                <span>Ürün adedi</span>
                <span className="font-semibold text-slate-100">{totalQuantity}</span>
              </div>
              <div className="flex justify-between">
                <span>Ara toplam</span>
                <span className="font-semibold text-orange-300">
                  {priceFormatter.format(totalPrice)}
                </span>
              </div>
            </div>

            <label className="mt-4 block text-xs font-semibold uppercase tracking-wide text-slate-500">
              Teslimat adresi
            </label>
            <textarea
              name="address"
              value={checkoutInfo.address}
              onChange={onCheckoutInfoChange}
              rows={3}
              placeholder="Mahalle, sokak, bina no..."
              className="mt-2 w-full resize-none rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
            />

            <label className="mt-3 block text-xs font-semibold uppercase tracking-wide text-slate-500">
              Ödeme tipi
            </label>
            <select
              name="paymentMethod"
              value={checkoutInfo.paymentMethod}
              onChange={onCheckoutInfoChange}
              className="mt-2 w-full rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
            >
              <option>Kapıda Ödeme</option>
              <option>Kredi Kartı Demo</option>
              <option>Havale/EFT Demo</option>
            </select>

            {checkoutInfo.paymentMethod === 'Kredi Kartı Demo' && (
              <div className="mt-3 rounded-lg border border-slate-800 bg-slate-900/70 p-3">
                <p className="text-xs font-semibold uppercase tracking-wide text-slate-500">
                  Kart Bilgileri
                </p>
                <input
                  name="cardHolder"
                  value={checkoutInfo.cardHolder}
                  onChange={onCheckoutInfoChange}
                  placeholder="Kart üzerindeki isim"
                  className="mt-3 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
                />
                <input
                  name="cardNumber"
                  value={checkoutInfo.cardNumber}
                  onChange={onCheckoutInfoChange}
                  inputMode="numeric"
                  placeholder="Kart numarası"
                  className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
                />
                <div className="mt-2 grid grid-cols-2 gap-2">
                  <input
                    name="cardExpiry"
                    value={checkoutInfo.cardExpiry}
                    onChange={onCheckoutInfoChange}
                    placeholder="AA/YY"
                    className="rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
                  />
                  <input
                    name="cardCvv"
                    value={checkoutInfo.cardCvv}
                    onChange={onCheckoutInfoChange}
                    inputMode="numeric"
                    placeholder="CVV"
                    className="rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-white outline-none focus:border-orange-500"
                  />
                </div>
              </div>
            )}

            <button
              type="button"
              onClick={onCheckout}
              disabled={isCheckoutLoading}
              className="mt-4 w-full rounded-md bg-orange-500 px-4 py-2 font-bold text-slate-950 transition hover:bg-orange-400 disabled:cursor-not-allowed disabled:bg-slate-600"
            >
              {isCheckoutLoading ? 'Sipariş oluşturuluyor...' : 'Siparişi Tamamla'}
            </button>
          </aside>
        </div>
      )}
    </section>
  )
}

export default BasketPanel
