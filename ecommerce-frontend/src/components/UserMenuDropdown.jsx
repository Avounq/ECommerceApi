function UserMenuDropdown({
  user,
  products,
  basketItems,
  isOpen,
  onOpen,
  onClose,
  onGoToBasket,
  onGoToFavorites,
  onGoToProfile,
  onGoToOrders,
  onLogout,
}) {
  const totalQuantity = basketItems.reduce((sum, item) => sum + item.quantity, 0)

  return (
    <div className="relative" onMouseEnter={onOpen} onMouseLeave={onClose}>
      <button
        type="button"
        onClick={isOpen ? onClose : onOpen}
        className="flex items-center gap-2 rounded-full border border-slate-700/80 bg-slate-900/80 px-4 py-2 text-sm font-semibold text-slate-200 transition hover:border-orange-400 hover:text-orange-300"
      >
        <span className="grid h-6 w-6 place-items-center rounded-full bg-orange-500 text-xs font-black text-slate-950">
          {(user.firstName || user.username).charAt(0).toUpperCase()}
        </span>
        {user.firstName || user.username}
      </button>

      {isOpen && (
        <div className="absolute right-0 top-full z-10 w-80 pt-2">
          <div className="rounded-lg border border-slate-800 bg-slate-900/95 p-4 shadow-2xl shadow-slate-950/60 backdrop-blur">
            <p className="text-sm text-slate-400">Hoş geldiniz</p>
            <h3 className="mt-1 font-semibold text-slate-100">
              {user.firstName} {user.lastName}
            </h3>

            <div className="mt-4 grid gap-2">
              <MenuButton onClick={onGoToBasket}>Sepetim ({totalQuantity})</MenuButton>
              <MenuButton onClick={onGoToFavorites}>Favorilerim</MenuButton>
              <MenuButton onClick={onGoToProfile}>Profilim</MenuButton>
              <MenuButton onClick={onGoToOrders}>Siparişlerim</MenuButton>
              <MenuButton onClick={onLogout}>Çıkış Yap</MenuButton>
            </div>

            <div className="mt-4 border-t border-slate-800 pt-4">
              <p className="text-sm font-semibold text-slate-200">Sepet Özeti</p>

              {basketItems.length === 0 && (
                <p className="mt-2 text-sm text-slate-400">Sepet boş.</p>
              )}

              {basketItems.length > 0 && (
                <div className="scrollbar-hidden mt-3 max-h-48 space-y-2 overflow-auto">
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
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

function MenuButton({ onClick, children }) {
  return (
    <button
      type="button"
      onClick={onClick}
      className="rounded-md border border-slate-800 bg-slate-950 px-3 py-2 text-left text-sm font-semibold text-slate-200 transition hover:border-orange-400 hover:text-orange-300"
    >
      {children}
    </button>
  )
}

export default UserMenuDropdown
