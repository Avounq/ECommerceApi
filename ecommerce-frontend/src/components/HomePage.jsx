import './HomePage.css'

function HomePage({
  user,
  products,
  onGoToProducts,
  onGoToBasket,
  onGoToOrders,
  onOpenProduct,
}) {
  const featuredProducts = products.slice(0, 4)

  return (
    <section className="homepage-shell">
      <div className="homepage-hero">
        <div>
          <p className="homepage-kicker">demo marketplace</p>
          <h1>Alışveriş ana menüsü</h1>
          <p className="homepage-copy">
            Ürünleri gez, sepete ekle, siparişlerini takip et. Her şey tek panelde,
            hızlı ve net.
          </p>
          <div className="homepage-actions">
            <button type="button" onClick={onGoToProducts}>
              Ürünlere Git
            </button>
            {user && (
              <>
                <button type="button" onClick={onGoToBasket}>
                  Sepetim
                </button>
                <button type="button" onClick={onGoToOrders}>
                  Siparişlerim
                </button>
              </>
            )}
          </div>
        </div>

        <div className="homepage-stats">
          <div>
            <span>{products.length}</span>
            <p>aktif ürün</p>
          </div>
          <div>
            <span>12</span>
            <p>kategori</p>
          </div>
          <div>
            <span>24/7</span>
            <p>demo vitrin</p>
          </div>
        </div>
      </div>

      {featuredProducts.length > 0 && (
        <div className="homepage-featured">
          <div className="homepage-section-title">
            <h2>Öne çıkanlar</h2>
            <button type="button" onClick={onGoToProducts}>
              Tümünü gör
            </button>
          </div>

          <div className="homepage-product-grid">
            {featuredProducts.map((product) => (
              <article
                key={product.id}
                role="button"
                tabIndex={0}
                onClick={() => onOpenProduct(product.id)}
                onKeyDown={(event) => {
                  if (event.key === 'Enter' || event.key === ' ') {
                    onOpenProduct(product.id)
                  }
                }}
              >
                {product.imageUrl ? (
                  <img src={product.imageUrl} alt={product.name} />
                ) : (
                  <div className="homepage-product-fallback">
                    {product.name.charAt(0).toUpperCase()}
                  </div>
                )}
                <p>{product.category}</p>
                <h3>{product.name}</h3>
              </article>
            ))}
          </div>
        </div>
      )}
    </section>
  )
}

export default HomePage
