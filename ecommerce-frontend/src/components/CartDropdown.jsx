function CartDropdown({
  products,
  basketItems,
  isOpen,
  onToggle,
  onGoToBasket,
}) {
  const totalQuantity = basketItems.reduce((sum, item) => sum + item.quantity, 0)

  return (
    <div className="relative">
      <button
        type="button"
        onClick={onToggle}
        className="rounded-md border border-slate-700 px-4 py-2 text-sm font-semibold text-slate-200 hover:border-emerald-400 hover:text-emerald-300"
      >
        Sepetim ({totalQuantity})
      </button>

      {isOpen && (
        <div className="absolute right-0 z-10 mt-2 w-80 rounded-lg border border-slate-800 bg-slate-900 p-4 shadow-xl">
          <div className="flex items-center justify-between">
            <h3 className="font-semibold text-slate-100">Sepet Özeti</h3>
            <span className="text-sm text-slate-400">{basketItems.length} kalem</span>
          </div>

          {basketItems.length === 0 && (
            <p className="mt-4 text-sm text-slate-400">Sepet boş.</p>
          )}

          {basketItems.length > 0 && (
            <div className="mt-4 max-h-64 space-y-2 overflow-auto">
              {basketItems.map((item) => {
                const product = products.find((current) => current.id === item.productId)

                return (
                  <div
                    key={item.id}
                    className="rounded-md border border-slate-800 bg-slate-950 p-3"
                  >
                    <p className="font-semibold text-slate-100">
                      {product?.name ?? `Ürün #${item.productId}`}
                    </p>
                    <p className="mt-1 text-sm text-slate-400">
                      Adet: {item.quantity}
                    </p>
                  </div>
                )
              })}
            </div>
          )}

          <button
            type="button"
            onClick={onGoToBasket}
            className="mt-4 w-full rounded-md bg-emerald-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-emerald-400"
          >
            Sepete Git
          </button>
        </div>
      )}
    </div>
  )
}

export default CartDropdown
