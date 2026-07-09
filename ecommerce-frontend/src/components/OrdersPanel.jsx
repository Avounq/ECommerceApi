const CANCELLED_STATUS = '\u0130ptal Edildi'
const DELIVERED_STATUS = 'Teslim Edildi'

function OrdersPanel({
  user,
  orders,
  message,
  isLoading,
  cancellingOrderId,
  onCancelOrder,
}) {
  const totalPrice = orders.reduce(
    (sum, order) => sum + order.productPrice * order.quantity,
    0,
  )
  const priceFormatter = new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY',
  })

  function canCancel(order) {
    return order.status !== DELIVERED_STATUS && order.status !== CANCELLED_STATUS
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

  return (
    <section className="rounded-lg border border-slate-800/80 bg-slate-900/90 p-5 shadow-xl">
      <div className="flex items-center justify-between gap-4">
        <div>
          <h2 className="text-lg font-bold">Siparişlerim</h2>
          <p className="mt-1 text-xs text-slate-400">
            {orders.length > 0 ? `${orders.length} sipariş bulundu` : 'Sipariş geçmişin'}
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
          Siparişleri görmek için önce giriş yap.
        </p>
      )}

      {message && (
        <p className="mt-4 rounded-md bg-slate-950 p-3 text-sm text-slate-300">
          {message}
        </p>
      )}

      {user && orders.length === 0 && !isLoading && (
        <p className="mt-4 text-slate-400">Henüz sipariş yok.</p>
      )}

      {orders.length > 0 && (
        <div className="mt-5 space-y-3">
          {orders.map((order) => (
            <article
              key={order.id}
              className="grid gap-4 rounded-lg border border-slate-800 bg-slate-950 p-4 transition hover:border-orange-500/70 sm:grid-cols-[1fr_auto]"
            >
              <div>
                <p className="text-xs font-semibold uppercase tracking-wide text-orange-300">
                  Sipariş #{order.id}
                </p>
                <span className="mt-2 inline-flex rounded-full border border-orange-500/40 bg-orange-500/10 px-2.5 py-1 text-xs font-bold text-orange-300">
                  {order.status || 'Hazırlanıyor'}
                </span>
                <h3 className="mt-2 font-semibold text-slate-100">
                  {order.productName}
                </h3>
                <p className="mt-1 text-sm text-slate-400">
                  {order.customerName || 'Müşteri bilgisi yok'}
                </p>
                <div className="mt-3 grid gap-1 text-xs text-slate-500">
                  <p>Adres: {order.shippingAddress || 'Adres bilgisi yok'}</p>
                  <p>Ödeme: {getPaymentText(order)}</p>
                </div>
              </div>

              <div className="text-left sm:text-right">
                <p className="text-sm text-slate-400">Adet: {order.quantity}</p>
                <p className="mt-1 font-bold text-orange-300">
                  {priceFormatter.format(order.productPrice * order.quantity)}
                </p>

                {canCancel(order) && (
                  <button
                    type="button"
                    onClick={() => onCancelOrder(order.id)}
                    disabled={cancellingOrderId === order.id}
                    className="mt-3 rounded-md border border-red-500/60 px-3 py-1.5 text-xs font-bold text-red-200 hover:border-red-300 disabled:cursor-not-allowed disabled:border-slate-700 disabled:text-slate-500"
                  >
                    {cancellingOrderId === order.id ? 'İptal ediliyor' : 'Siparişi İptal Et'}
                  </button>
                )}
              </div>
            </article>
          ))}

          <div className="rounded-lg border border-orange-500/30 bg-slate-950 p-4 text-sm text-slate-300">
            Toplam sipariş tutarı:{' '}
            <span className="font-bold text-orange-300">
              {priceFormatter.format(totalPrice)}
            </span>
          </div>
        </div>
      )}
    </section>
  )
}

export default OrdersPanel
