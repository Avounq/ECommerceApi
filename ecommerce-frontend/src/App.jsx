import { useEffect, useState } from 'react'
import {
  addToBasket,
  addProductReview,
  cancelMyOrder,
  changePassword,
  checkoutBasket,
  createProduct,
  deleteOrder,
  deleteProduct,
  deleteBasketItem,
  getBasket,
  getAllOrders,
  getCurrentUser,
  getMyOrders,
  getProducts,
  getProductReviews,
  loginUser,
  logoutUser,
  registerUser,
  updateBasketItem,
  updateOrder,
  updateProduct,
  updateProfile,
} from './api'
import AdminPanel from './components/AdminPanel'
import AuthForm from './components/AuthForm'
import BasketPanel from './components/BasketPanel'
import FavoritesPanel from './components/FavoritesPanel'
import HomePage from './components/HomePage'
import OrdersPanel from './components/OrdersPanel'
import ProductDetailPanel from './components/ProductDetailPanel'
import ProductsPanel from './components/ProductsPanel'
import UserMenuDropdown from './components/UserMenuDropdown'
import UserPanel from './components/UserPanel'

function App() {
  const [authMode, setAuthMode] = useState('login')
  const [activeView, setActiveView] = useState('home')
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false)
  const [selectedProductId, setSelectedProductId] = useState(null)
  const [selectedCategory, setSelectedCategory] = useState('')
  const [stockFilter, setStockFilter] = useState('all')
  const [productSearch, setProductSearch] = useState('')
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [registerForm, setRegisterForm] = useState({
    username: '',
    email: '',
    firstName: '',
    lastName: '',
    password: '',
    confirmPassword: '',
  })
  const [profileForm, setProfileForm] = useState({
    email: '',
    firstName: '',
    lastName: '',
  })
  const [passwordForm, setPasswordForm] = useState({
    currentPassword: '',
    newPassword: '',
    newPasswordAgain: '',
  })
  const [message, setMessage] = useState('')
  const [user, setUser] = useState(null)
  const [products, setProducts] = useState([])
  const [basketItems, setBasketItems] = useState([])
  const [orders, setOrders] = useState([])
  const [adminOrders, setAdminOrders] = useState([])
  const [productReviews, setProductReviews] = useState([])
  const [favoriteProductIds, setFavoriteProductIds] = useState(() => {
    const savedFavorites = localStorage.getItem('favoriteProductIds')

    return savedFavorites ? JSON.parse(savedFavorites) : []
  })
  const [productsMessage, setProductsMessage] = useState('')
  const [basketMessage, setBasketMessage] = useState('')
  const [ordersMessage, setOrdersMessage] = useState('')
  const [adminMessage, setAdminMessage] = useState('')
  const [reviewMessage, setReviewMessage] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const [isProductsLoading, setIsProductsLoading] = useState(false)
  const [isBasketLoading, setIsBasketLoading] = useState(false)
  const [isOrdersLoading, setIsOrdersLoading] = useState(false)
  const [isRegisterLoading, setIsRegisterLoading] = useState(false)
  const [isProfileLoading, setIsProfileLoading] = useState(false)
  const [isPasswordLoading, setIsPasswordLoading] = useState(false)
  const [isCheckoutLoading, setIsCheckoutLoading] = useState(false)
  const [isAdminLoading, setIsAdminLoading] = useState(false)
  const [isReviewsLoading, setIsReviewsLoading] = useState(false)
  const [isReviewSubmitting, setIsReviewSubmitting] = useState(false)
  const [cancellingOrderId, setCancellingOrderId] = useState(null)
  const [updatingOrderId, setUpdatingOrderId] = useState(null)
  const [deletingOrderId, setDeletingOrderId] = useState(null)
  const [addingProductId, setAddingProductId] = useState(null)
  const [deletingBasketItemId, setDeletingBasketItemId] = useState(null)
  const [updatingBasketItemId, setUpdatingBasketItemId] = useState(null)
  const [checkoutInfo, setCheckoutInfo] = useState({
    address: '',
    paymentMethod: 'Kapıda Ödeme',
    cardHolder: '',
    cardNumber: '',
    cardExpiry: '',
    cardCvv: '',
  })

  useEffect(() => {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      return
    }

    async function loadCurrentSession() {
      try {
        const currentUser = await getCurrentUser(accessToken)
        setUser(currentUser)
        setProfileFormFromUser(currentUser)
        setMessage('Oturum açık.')
        await loadProducts(accessToken, selectedCategory, productSearch, stockFilter)
        await loadBasket(accessToken)
        await loadOrders(accessToken)
        if (currentUser.role === 'Admin') {
          await loadAdminOrders(accessToken)
        }
      } catch {
        localStorage.removeItem('accessToken')
        localStorage.removeItem('refreshToken')
      }
    }

    loadCurrentSession()
  }, [])

  function handleAuthModeChange(nextAuthMode) {
    setAuthMode(nextAuthMode)
    setMessage('')
  }

  async function handleLogin(event) {
    event.preventDefault()
    setIsLoading(true)
    setMessage('')

    try {
      const tokenResponse = await loginUser({
        username,
        password,
      })

      localStorage.setItem('accessToken', tokenResponse.accessToken)
      localStorage.setItem('refreshToken', tokenResponse.refreshToken)

      const currentUser = await getCurrentUser(tokenResponse.accessToken)

      setUser(currentUser)
      setProfileFormFromUser(currentUser)
      setMessage(`Hoş geldiniz ${currentUser.firstName || currentUser.username}.`)
      setActiveView('home')
      setPassword('')
      await loadProducts(tokenResponse.accessToken, selectedCategory, productSearch, stockFilter)
      await loadBasket(tokenResponse.accessToken)
      await loadOrders(tokenResponse.accessToken)
      if (currentUser.role === 'Admin') {
        await loadAdminOrders(tokenResponse.accessToken)
      }
    } catch (error) {
      setMessage(error.message)
      setUser(null)
      setProducts([])
      setBasketItems([])
      setOrders([])
    } finally {
      setIsLoading(false)
    }
  }

  function handleRegisterInputChange(event) {
    const { name, value } = event.target

    setRegisterForm((currentForm) => ({
      ...currentForm,
      [name]: value,
    }))
  }

  async function handleRegister(event) {
    event.preventDefault()
    setIsRegisterLoading(true)
    setMessage('')

    try {
      await registerUser(registerForm)
      setUsername(registerForm.username)
      setPassword('')
      setRegisterForm({
        username: '',
        email: '',
        firstName: '',
        lastName: '',
        password: '',
        confirmPassword: '',
      })
      setAuthMode('login')
      setMessage('Kayıt başarılı. Şimdi giriş yapabilirsin.')
    } catch (error) {
      setMessage(error.message)
    } finally {
      setIsRegisterLoading(false)
    }
  }

  function setProfileFormFromUser(currentUser) {
    setProfileForm({
      email: currentUser.email ?? '',
      firstName: currentUser.firstName ?? '',
      lastName: currentUser.lastName ?? '',
    })
  }

  function handleProfileInputChange(event) {
    const { name, value } = event.target

    setProfileForm((currentForm) => ({
      ...currentForm,
      [name]: value,
    }))
  }

  async function handleProfileUpdate(event) {
    event.preventDefault()

    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setMessage('Profil güncellemek için önce giriş yap.')
      return
    }

    setIsProfileLoading(true)
    setMessage('')

    try {
      const updatedUser = await updateProfile(accessToken, profileForm)
      setUser(updatedUser)
      setProfileFormFromUser(updatedUser)
      setMessage('Profil güncellendi.')
    } catch (error) {
      setMessage(error.message)
    } finally {
      setIsProfileLoading(false)
    }
  }

  function handlePasswordInputChange(event) {
    const { name, value } = event.target

    setPasswordForm((currentForm) => ({
      ...currentForm,
      [name]: value,
    }))
  }

  async function handlePasswordChange(event) {
    event.preventDefault()

    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setMessage('Şifre değiştirmek için önce giriş yap.')
      return
    }

    if (passwordForm.newPassword !== passwordForm.newPasswordAgain) {
      setMessage('Yeni şifreler aynı değil.')
      return
    }

    setIsPasswordLoading(true)
    setMessage('')

    try {
      await changePassword(accessToken, {
        currentPassword: passwordForm.currentPassword,
        newPassword: passwordForm.newPassword,
      })

      setPasswordForm({
        currentPassword: '',
        newPassword: '',
        newPasswordAgain: '',
      })
      setMessage('Şifre başarıyla değiştirildi.')
    } catch (error) {
      setMessage(error.message)
    } finally {
      setIsPasswordLoading(false)
    }
  }

  async function loadProducts(
    accessToken,
    category = selectedCategory,
    search = productSearch,
    nextStockFilter = stockFilter,
  ) {
    setIsProductsLoading(true)
    setProductsMessage('')

    try {
      const productsResponse = await getProducts(
        accessToken,
        category,
        search.trim(),
        nextStockFilter,
      )
      setProducts(productsResponse.data ?? [])
    } catch (error) {
      setProducts([])
      setProductsMessage(error.message)
    } finally {
      setIsProductsLoading(false)
    }
  }

  async function loadBasket(accessToken) {
    setIsBasketLoading(true)
    setBasketMessage('')

    try {
      const basketResponse = await getBasket(accessToken)
      setBasketItems(basketResponse.data ?? [])
    } catch (error) {
      setBasketItems([])
      setBasketMessage(error.message)
    } finally {
      setIsBasketLoading(false)
    }
  }

  async function loadOrders(accessToken) {
    setIsOrdersLoading(true)
    setOrdersMessage('')

    try {
      const ordersResponse = await getMyOrders(accessToken)
      setOrders(ordersResponse ?? [])
    } catch (error) {
      setOrders([])
      setOrdersMessage(error.message)
    } finally {
      setIsOrdersLoading(false)
    }
  }

  async function loadAdminOrders(accessToken) {
    setAdminMessage('')

    try {
      const ordersResponse = await getAllOrders(accessToken)
      setAdminOrders(ordersResponse.data ?? [])
    } catch (error) {
      setAdminOrders([])
      setAdminMessage(error.message)
    }
  }

  async function handleAddToBasket(productId) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setBasketMessage('Sepete eklemek için önce giriş yap.')
      return
    }

    setAddingProductId(productId)
    setBasketMessage('')

    try {
      await addToBasket(accessToken, {
        productId,
        quantity: 1,
      })

      setBasketMessage('Ürün sepete eklendi.')
      await loadBasket(accessToken)
      setIsUserMenuOpen(true)
    } catch (error) {
      setBasketMessage(error.message)
    } finally {
      setAddingProductId(null)
    }
  }

  async function handleCategoryChange(category) {
    setSelectedCategory(category)

    const accessToken = localStorage.getItem('accessToken')

    if (accessToken) {
      await loadProducts(accessToken, category, productSearch, stockFilter)
    }
  }

  async function handleProductSearchChange(event) {
    const nextSearch = event.target.value
    setProductSearch(nextSearch)

    const accessToken = localStorage.getItem('accessToken')

    if (accessToken) {
      await loadProducts(accessToken, selectedCategory, nextSearch, stockFilter)
    }
  }

  async function handleProductSearchClear() {
    setProductSearch('')

    const accessToken = localStorage.getItem('accessToken')

    if (accessToken) {
      await loadProducts(accessToken, selectedCategory, '', stockFilter)
    }
  }

  function handleToggleFavorite(productId) {
    setFavoriteProductIds((currentFavorites) => {
      const nextFavorites = currentFavorites.includes(productId)
        ? currentFavorites.filter((favoriteProductId) => favoriteProductId !== productId)
        : [...currentFavorites, productId]

      localStorage.setItem('favoriteProductIds', JSON.stringify(nextFavorites))

      return nextFavorites
    })
  }

  function handleCheckoutInfoChange(event) {
    const { name, value } = event.target

    setCheckoutInfo((currentInfo) => ({
      ...currentInfo,
      [name]: value,
    }))
  }

  function handleOpenProduct(productId) {
    setSelectedProductId(productId)
    setActiveView('productDetail')
    loadProductReviews(productId)
  }

  async function handleStockFilterChange(nextStockFilter) {
    setStockFilter(nextStockFilter)

    const accessToken = localStorage.getItem('accessToken')

    if (accessToken) {
      await loadProducts(accessToken, selectedCategory, productSearch, nextStockFilter)
    }
  }

  function handleGoHome() {
    setSelectedProductId(null)
    setActiveView('home')
  }

  async function loadProductReviews(productId) {
    setIsReviewsLoading(true)
    setReviewMessage('')

    try {
      const reviews = await getProductReviews(productId)
      setProductReviews(reviews)
    } catch (error) {
      setProductReviews([])
      setReviewMessage(error.message)
    } finally {
      setIsReviewsLoading(false)
    }
  }

  async function handleAddProductReview(productId, review) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setReviewMessage('Yorum yazmak için giriş yap.')
      return false
    }

    setIsReviewSubmitting(true)
    setReviewMessage('')

    try {
      await addProductReview(accessToken, productId, review)
      setReviewMessage('Yorum eklendi.')
      await loadProductReviews(productId)
      return true
    } catch (error) {
      setReviewMessage(error.message)
      return false
    } finally {
      setIsReviewSubmitting(false)
    }
  }

  async function handleDeleteBasketItem(basketItemId) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setBasketMessage('Sepetten çıkarmak için önce giriş yap.')
      return
    }

    setDeletingBasketItemId(basketItemId)
    setBasketMessage('')

    try {
      await deleteBasketItem(accessToken, basketItemId)
      setBasketMessage('Ürün sepetten çıkarıldı.')
      await loadBasket(accessToken)
    } catch (error) {
      setBasketMessage(error.message)
    } finally {
      setDeletingBasketItemId(null)
    }
  }

  async function handleUpdateBasketQuantity(basketItem, nextQuantity) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setBasketMessage('Sepeti güncellemek için önce giriş yap.')
      return
    }

    if (nextQuantity < 1) {
      await handleDeleteBasketItem(basketItem.id)
      return
    }

    setUpdatingBasketItemId(basketItem.id)
    setBasketMessage('')

    try {
      await updateBasketItem(accessToken, basketItem.id, {
        productId: basketItem.productId,
        quantity: nextQuantity,
      })

      setBasketMessage('Sepet güncellendi.')
      await loadBasket(accessToken)
    } catch (error) {
      setBasketMessage(error.message)
    } finally {
      setUpdatingBasketItemId(null)
    }
  }

  async function handleCheckout() {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setOrdersMessage('Sipariş vermek için önce giriş yap.')
      return
    }

    if (!checkoutInfo.address.trim()) {
      setBasketMessage('Siparişi tamamlamak için adres gir.')
      return
    }

    if (checkoutInfo.paymentMethod === 'Kredi Kartı Demo') {
      if (
        !checkoutInfo.cardHolder.trim() ||
        !checkoutInfo.cardNumber.trim() ||
        !checkoutInfo.cardExpiry.trim() ||
        !checkoutInfo.cardCvv.trim()
      ) {
        setBasketMessage('Kart ile ödeme için kart bilgilerini doldur.')
        return
      }
    }

    setIsCheckoutLoading(true)
    setOrdersMessage('')

    try {
      await checkoutBasket(accessToken, checkoutInfo)
      setBasketItems([])
      setBasketMessage('Sepet boşaltıldı.')
      setOrdersMessage(
        `Sipariş başarıyla oluşturuldu. Ödeme: ${checkoutInfo.paymentMethod}.`,
      )
      setCheckoutInfo({
        address: '',
        paymentMethod: 'Kapıda Ödeme',
        cardHolder: '',
        cardNumber: '',
        cardExpiry: '',
        cardCvv: '',
      })
      await loadBasket(accessToken)
      await loadOrders(accessToken)
      if (user?.role === 'Admin') {
        await loadAdminOrders(accessToken)
      }
    } catch (error) {
      setOrdersMessage(error.message)
    } finally {
      setIsCheckoutLoading(false)
    }
  }

  async function handleCancelMyOrder(orderId) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setOrdersMessage('Siparişi iptal etmek için giriş yap.')
      return
    }

    setCancellingOrderId(orderId)
    setOrdersMessage('')

    try {
      await cancelMyOrder(accessToken, orderId)
      setOrdersMessage('Sipariş iptal edildi.')
      await loadOrders(accessToken)
      if (user?.role === 'Admin') {
        await loadAdminOrders(accessToken)
      }
    } catch (error) {
      setOrdersMessage(error.message)
    } finally {
      setCancellingOrderId(null)
    }
  }

  async function handleCreateProduct(product) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setAdminMessage('Ürün eklemek için giriş yap.')
      return false
    }

    setIsAdminLoading(true)
    setAdminMessage('')

    try {
      await createProduct(accessToken, product)
      setAdminMessage('Ürün eklendi.')
      await loadProducts(accessToken, selectedCategory, productSearch, stockFilter)
      return true
    } catch (error) {
      setAdminMessage(error.message)
      return false
    } finally {
      setIsAdminLoading(false)
    }
  }

  async function handleUpdateProduct(productId, product) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setAdminMessage('Ürün güncellemek için giriş yap.')
      return false
    }

    setIsAdminLoading(true)
    setAdminMessage('')

    try {
      await updateProduct(accessToken, productId, product)
      setAdminMessage('Ürün güncellendi.')
      await loadProducts(accessToken, selectedCategory, productSearch, stockFilter)
      return true
    } catch (error) {
      setAdminMessage(error.message)
      return false
    } finally {
      setIsAdminLoading(false)
    }
  }

  async function handleDeleteProduct(productId) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setAdminMessage('Ürün silmek için giriş yap.')
      return
    }

    setIsAdminLoading(true)
    setAdminMessage('')

    try {
      await deleteProduct(accessToken, productId)
      setAdminMessage('Ürün silindi.')
      setFavoriteProductIds((currentFavorites) => {
        const nextFavorites = currentFavorites.filter((id) => id !== productId)
        localStorage.setItem('favoriteProductIds', JSON.stringify(nextFavorites))
        return nextFavorites
      })
      await loadProducts(accessToken, selectedCategory, productSearch, stockFilter)
      await loadBasket(accessToken)
    } catch (error) {
      setAdminMessage(error.message)
    } finally {
      setIsAdminLoading(false)
    }
  }

  async function handleUpdateOrderStatus(order, status) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setAdminMessage('Sipariş güncellemek için giriş yap.')
      return
    }

    setUpdatingOrderId(order.id)
    setAdminMessage('')

    try {
      await updateOrder(accessToken, order.id, {
        customerId: order.customerId,
        productId: order.productId,
        quantity: order.quantity,
        status,
      })
      setAdminMessage('Sipariş durumu güncellendi.')
      await loadAdminOrders(accessToken)
      await loadOrders(accessToken)
    } catch (error) {
      setAdminMessage(error.message)
    } finally {
      setUpdatingOrderId(null)
    }
  }

  async function handleDeleteOrder(order) {
    const accessToken = localStorage.getItem('accessToken')

    if (!accessToken) {
      setAdminMessage('Sipariş silmek için giriş yap.')
      return
    }

    if (order.status !== 'İptal Edildi') {
      setAdminMessage('Sadece iptal edilen siparişler silinebilir.')
      return
    }

    setDeletingOrderId(order.id)
    setAdminMessage('')

    try {
      await deleteOrder(accessToken, order.id)
      setAdminMessage('İptal edilen sipariş silindi.')
      await loadAdminOrders(accessToken)
      await loadOrders(accessToken)
    } catch (error) {
      setAdminMessage(error.message)
    } finally {
      setDeletingOrderId(null)
    }
  }

  async function handleLogout() {
    const accessToken = localStorage.getItem('accessToken')

    try {
      if (accessToken) {
        await logoutUser(accessToken)
      }
      setMessage('Çıkış yapıldı.')
    } catch (error) {
      setMessage(error.message)
    }

    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
    setUser(null)
    setProducts([])
    setBasketItems([])
    setOrders([])
    setAdminOrders([])
    setProfileForm({
      email: '',
      firstName: '',
      lastName: '',
    })
    setPasswordForm({
      currentPassword: '',
      newPassword: '',
      newPasswordAgain: '',
    })
    setProductsMessage('')
    setBasketMessage('')
    setOrdersMessage('')
    setIsUserMenuOpen(false)
  }

  return (
    <main className="min-h-screen bg-[radial-gradient(circle_at_top_left,#1f2937_0,#020617_34rem)] px-4 py-6 text-white">
      <div
        className={`mx-auto grid w-full max-w-7xl gap-6 ${
          user ? 'lg:grid-cols-1' : 'lg:grid-cols-[360px_1fr]'
        }`}
      >
        {!user && (
          <aside className="space-y-6">
            <AuthForm
              authMode={authMode}
              username={username}
              password={password}
              registerForm={registerForm}
              message={message}
              isLoading={isLoading}
              isRegisterLoading={isRegisterLoading}
              onAuthModeChange={handleAuthModeChange}
              onLogin={handleLogin}
              onRegister={handleRegister}
              onUsernameChange={(event) => setUsername(event.target.value)}
              onPasswordChange={(event) => setPassword(event.target.value)}
              onRegisterInputChange={handleRegisterInputChange}
            />
          </aside>
        )}

        <section className="space-y-6">
          {user && (
            <div className="sticky top-3 z-20 rounded-lg border border-slate-800/80 bg-slate-950/90 p-3 shadow-2xl shadow-slate-950/40 backdrop-blur">
              <div className="flex flex-wrap items-center justify-between gap-3">
                <div className="flex items-center gap-4">
                  <div>
                    <button
                      type="button"
                      onClick={handleGoHome}
                      className="text-left text-lg font-black tracking-tight text-orange-300 hover:text-orange-200"
                    >
                      e-commerce
                    </button>
                    <p className="text-[11px] text-slate-500">demo marketplace</p>
                  </div>
                </div>

                <div className="flex flex-wrap justify-center gap-2">
                  <NavButton
                    isActive={activeView === 'home'}
                    onClick={handleGoHome}
                  >
                    Ana Menü
                  </NavButton>
                  <NavButton
                    isActive={activeView === 'products'}
                    onClick={() => {
                      setSelectedProductId(null)
                      setActiveView('products')
                    }}
                  >
                    Ürünler
                  </NavButton>
                  <NavButton
                    isActive={activeView === 'basket'}
                    onClick={() => setActiveView('basket')}
                  >
                    Sepetim
                  </NavButton>
                  <NavButton
                    isActive={activeView === 'favorites'}
                    onClick={() => setActiveView('favorites')}
                  >
                    Favorilerim
                  </NavButton>
                  <NavButton
                    isActive={activeView === 'orders'}
                    onClick={() => setActiveView('orders')}
                  >
                    Siparişlerim
                  </NavButton>
                  <NavButton
                    isActive={activeView === 'profile'}
                    onClick={() => setActiveView('profile')}
                  >
                    Profilim
                  </NavButton>
                  <NavButton
                    isActive={activeView === 'password'}
                    onClick={() => setActiveView('password')}
                  >
                    Şifre Değiştir
                  </NavButton>
                  {user.role === 'Admin' && (
                    <NavButton
                      isActive={activeView === 'admin'}
                      onClick={() => setActiveView('admin')}
                    >
                      Admin
                    </NavButton>
                  )}
                </div>

                <UserMenuDropdown
                  user={user}
                  products={products}
                  basketItems={basketItems}
                  isOpen={isUserMenuOpen}
                  onOpen={() => setIsUserMenuOpen(true)}
                  onClose={() => setIsUserMenuOpen(false)}
                  onGoToBasket={() => {
                    setActiveView('basket')
                    setIsUserMenuOpen(false)
                  }}
                  onGoToFavorites={() => {
                    setActiveView('favorites')
                    setIsUserMenuOpen(false)
                  }}
                  onGoToProfile={() => {
                    setActiveView('profile')
                    setIsUserMenuOpen(false)
                  }}
                  onGoToOrders={() => {
                    setActiveView('orders')
                    setIsUserMenuOpen(false)
                  }}
                  onLogout={handleLogout}
                />
              </div>
            </div>
          )}

          {user && activeView === 'home' && (
            <HomePage
              user={user}
              products={products}
              onGoToProducts={() => setActiveView('products')}
              onGoToBasket={() => setActiveView('basket')}
              onGoToOrders={() => setActiveView('orders')}
              onOpenProduct={handleOpenProduct}
            />
          )}

          {(!user || activeView === 'products') && (
            <ProductsPanel
              user={user}
              products={products}
              message={productsMessage}
              isLoading={isProductsLoading}
              addingProductId={addingProductId}
              selectedCategory={selectedCategory}
              stockFilter={stockFilter}
              search={productSearch}
              favoriteProductIds={favoriteProductIds}
              onSearchChange={handleProductSearchChange}
              onSearchClear={handleProductSearchClear}
              onCategoryChange={handleCategoryChange}
              onStockFilterChange={handleStockFilterChange}
              onOpenProduct={handleOpenProduct}
              onAddToBasket={handleAddToBasket}
              onToggleFavorite={handleToggleFavorite}
            />
          )}

          {activeView === 'productDetail' && (
            <ProductDetailPanel
              product={products.find((product) => product.id === selectedProductId)}
              addingProductId={addingProductId}
              favoriteProductIds={favoriteProductIds}
              reviews={productReviews}
              reviewMessage={reviewMessage}
              isReviewsLoading={isReviewsLoading}
              isReviewSubmitting={isReviewSubmitting}
              onBack={() => setActiveView('products')}
              onAddToBasket={handleAddToBasket}
              onToggleFavorite={handleToggleFavorite}
              onAddReview={handleAddProductReview}
            />
          )}

          {activeView === 'basket' && (
            <BasketPanel
              user={user}
              products={products}
              basketItems={basketItems}
              message={basketMessage}
              isLoading={isBasketLoading}
              deletingBasketItemId={deletingBasketItemId}
              updatingBasketItemId={updatingBasketItemId}
              onDeleteBasketItem={handleDeleteBasketItem}
              onUpdateBasketQuantity={handleUpdateBasketQuantity}
              isCheckoutLoading={isCheckoutLoading}
              checkoutInfo={checkoutInfo}
              onCheckoutInfoChange={handleCheckoutInfoChange}
              onCheckout={handleCheckout}
            />
          )}

          {activeView === 'favorites' && (
            <FavoritesPanel
              user={user}
              products={products}
              favoriteProductIds={favoriteProductIds}
              addingProductId={addingProductId}
              onAddToBasket={handleAddToBasket}
              onToggleFavorite={handleToggleFavorite}
            />
          )}

          {activeView === 'orders' && (
            <OrdersPanel
              user={user}
              orders={orders}
              message={ordersMessage}
              isLoading={isOrdersLoading}
              cancellingOrderId={cancellingOrderId}
              onCancelOrder={handleCancelMyOrder}
            />
          )}

          {activeView === 'profile' && user && (
            <UserPanel
              user={user}
              mode="profile"
              profileForm={profileForm}
              passwordForm={passwordForm}
              isProfileLoading={isProfileLoading}
              isPasswordLoading={isPasswordLoading}
              onProfileInputChange={handleProfileInputChange}
              onProfileUpdate={handleProfileUpdate}
              onPasswordInputChange={handlePasswordInputChange}
              onPasswordChange={handlePasswordChange}
              onLogout={handleLogout}
            />
          )}

          {activeView === 'password' && user && (
            <UserPanel
              user={user}
              mode="password"
              profileForm={profileForm}
              passwordForm={passwordForm}
              isProfileLoading={isProfileLoading}
              isPasswordLoading={isPasswordLoading}
              onProfileInputChange={handleProfileInputChange}
              onProfileUpdate={handleProfileUpdate}
              onPasswordInputChange={handlePasswordInputChange}
              onPasswordChange={handlePasswordChange}
              onLogout={handleLogout}
            />
          )}

          {activeView === 'admin' && user?.role === 'Admin' && (
            <AdminPanel
              products={products}
              orders={adminOrders}
              message={adminMessage}
              isLoading={isAdminLoading}
              updatingOrderId={updatingOrderId}
              deletingOrderId={deletingOrderId}
              onCreateProduct={handleCreateProduct}
              onUpdateProduct={handleUpdateProduct}
              onDeleteProduct={handleDeleteProduct}
              onUpdateOrderStatus={handleUpdateOrderStatus}
              onDeleteOrder={handleDeleteOrder}
            />
          )}
        </section>
      </div>
    </main>
  )
}

function NavButton({ isActive, onClick, children }) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={`rounded-full px-4 py-2 text-sm font-semibold transition ${
        isActive
          ? 'bg-orange-500 text-slate-950 shadow-lg shadow-orange-950/40'
          : 'border border-slate-700/80 bg-slate-900/70 text-slate-200 hover:border-orange-400 hover:text-orange-300'
      }`}
    >
      {children}
    </button>
  )
}

export default App
