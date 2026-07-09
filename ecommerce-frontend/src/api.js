const API_BASE_URL = 'http://localhost:8080/api'

let refreshTokenPromise = null

async function createErrorMessage(response, fallbackMessage) {
  const errorText = await response.text()

  if (!errorText) {
    return `${fallbackMessage}. Status: ${response.status}`
  }

  return `${fallbackMessage}. Status: ${response.status} ${errorText}`
}

async function fetchWithAutoRefresh(url, options = {}) {
  const response = await fetch(url, withLatestAccessToken(options))

  if (response.status !== 401) {
    return response
  }

  try {
    const newAccessToken = await refreshTokensOnce()

    return fetch(url, {
      ...withLatestAccessToken(options),
      headers: {
        ...(withLatestAccessToken(options).headers ?? {}),
        Authorization: `Bearer ${newAccessToken}`,
      },
    })
  } catch {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')

    return response
  }
}

async function refreshTokensOnce() {
  if (!refreshTokenPromise) {
    refreshTokenPromise = refreshTokens().finally(() => {
      refreshTokenPromise = null
    })
  }

  return refreshTokenPromise
}

function withLatestAccessToken(options) {
  const headers = options.headers ?? {}
  const hasAuthorizationHeader = Object.hasOwn(headers, 'Authorization')
  const latestAccessToken = localStorage.getItem('accessToken')

  if (!hasAuthorizationHeader || !latestAccessToken) {
    return options
  }

  return {
    ...options,
    headers: {
      ...headers,
      Authorization: `Bearer ${latestAccessToken}`,
    },
  }
}

export async function loginUser(loginData) {
  const response = await fetch(`${API_BASE_URL}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(loginData),
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Giriş başarısız'))
  }

  return response.json()
}

export async function registerUser(registerData) {
  const response = await fetch(`${API_BASE_URL}/auth/register`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(registerData),
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Kayıt başarısız'))
  }

  return response.json()
}

export async function getCurrentUser(accessToken) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/auth/me`, {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Kullanıcı bilgisi alınamadı'))
  }

  return response.json()
}

export async function logoutUser(accessToken) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/auth/logout`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Çıkış yapılamadı'))
  }

  return response.json()
}

export async function refreshTokens() {
  const accessToken = localStorage.getItem('accessToken')
  const refreshToken = localStorage.getItem('refreshToken')

  if (!accessToken || !refreshToken) {
    throw new Error('Yenilenecek token bulunamadı.')
  }

  const response = await fetch(`${API_BASE_URL}/auth/refresh`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      accessToken,
      refreshToken,
    }),
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Token yenilenemedi'))
  }

  const tokenResponse = await response.json()

  localStorage.setItem('accessToken', tokenResponse.accessToken)
  localStorage.setItem('refreshToken', tokenResponse.refreshToken)

  return tokenResponse.accessToken
}

export async function updateProfile(accessToken, profileData) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/auth/updateprofile`, {
    method: 'PUT',
    headers: {
      Authorization: `Bearer ${accessToken}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(profileData),
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Profil güncellenemedi'))
  }

  return response.json()
}

export async function changePassword(accessToken, passwordData) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/auth/sifre-degistir`, {
    method: 'PUT',
    headers: {
      Authorization: `Bearer ${accessToken}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(passwordData),
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Şifre değiştirilemedi'))
  }

  return response.json()
}

export async function getProducts(accessToken, category = '', search = '', stockFilter = 'all') {
  const queryParams = new URLSearchParams({
    pageNumber: '1',
    pageSize: '150',
    sortBy: 'name',
  })

  if (category) {
    queryParams.set('category', category)
  }

  if (search) {
    queryParams.set('search', search)
  }

  if (stockFilter === 'inStock') {
    queryParams.set('inStockOnly', 'true')
  }

  if (stockFilter === 'lowStock') {
    queryParams.set('lowStockOnly', 'true')
  }

  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/products?${queryParams.toString()}`, {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Ürünler alınamadı'))
  }

  return response.json()
}

export async function getProductReviews(productId) {
  const response = await fetch(`${API_BASE_URL}/products/${productId}/reviews`)

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Yorumlar alınamadı'))
  }

  return response.json()
}

export async function addProductReview(accessToken, productId, review) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/products/${productId}/reviews`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${accessToken}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(review),
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Yorum eklenemedi'))
  }

  return response.json()
}

export async function getBasket(accessToken) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/baskets?pageNumber=1&pageSize=50`, {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Sepet alınamadı'))
  }

  return response.json()
}

export async function addToBasket(accessToken, basketItem) {
  const formData = new FormData()
  formData.append('productId', basketItem.productId)
  formData.append('quantity', basketItem.quantity)

  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/baskets`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
    body: formData,
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Sepete eklenemedi'))
  }

  return response.json()
}

export async function deleteBasketItem(accessToken, basketItemId) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/baskets/${basketItemId}`, {
    method: 'DELETE',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Sepetten çıkarılamadı'))
  }

  return response.text()
}

export async function updateBasketItem(accessToken, basketItemId, basketItem) {
  const formData = new FormData()
  formData.append('productId', basketItem.productId)
  formData.append('quantity', basketItem.quantity)

  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/baskets/${basketItemId}`, {
    method: 'PUT',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
    body: formData,
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Sepet güncellenemedi'))
  }

  return response.text()
}

export async function createProduct(accessToken, product) {
  const formData = new FormData()
  formData.append('name', product.name)
  formData.append('price', normalizePrice(product.price))
  formData.append('category', product.category)
  formData.append('imageUrl', product.imageUrl ?? '')
  getImageUrls(product).forEach((imageUrl) => {
    formData.append('imageUrls', imageUrl)
  })
  formData.append('stock', product.stock ?? 50)
  formData.append('discountRate', normalizePrice(product.discountRate ?? 0))

  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/products`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
    body: formData,
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Ürün eklenemedi'))
  }

  return response.json()
}

export async function updateProduct(accessToken, productId, product) {
  const formData = new FormData()
  formData.append('name', product.name)
  formData.append('price', normalizePrice(product.price))
  formData.append('category', product.category)
  formData.append('imageUrl', product.imageUrl ?? '')
  getImageUrls(product).forEach((imageUrl) => {
    formData.append('imageUrls', imageUrl)
  })
  formData.append('stock', product.stock ?? 50)
  formData.append('discountRate', normalizePrice(product.discountRate ?? 0))

  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/products/${productId}`, {
    method: 'PUT',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
    body: formData,
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Ürün güncellenemedi'))
  }

  return response.text()
}

function normalizePrice(price) {
  return String(price).replace(',', '.')
}

function getImageUrls(product) {
  if (Array.isArray(product.imageUrls)) {
    return product.imageUrls
  }

  return String(product.imageUrlsText ?? '')
    .split('\n')
    .map((imageUrl) => imageUrl.trim())
    .filter(Boolean)
}

export async function deleteProduct(accessToken, productId) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/products/${productId}`, {
    method: 'DELETE',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Ürün silinemedi'))
  }

  return response.text()
}

export async function getAllOrders(accessToken) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/orders?pageNumber=1&pageSize=150`, {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Siparişler alınamadı'))
  }

  return response.json()
}

export async function updateOrder(accessToken, orderId, order) {
  const formData = new FormData()
  formData.append('customerId', order.customerId ?? '')
  formData.append('productId', order.productId)
  formData.append('quantity', order.quantity)
  formData.append('status', order.status)

  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/orders/${orderId}`, {
    method: 'PUT',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
    body: formData,
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Sipariş güncellenemedi'))
  }

  return response.text()
}

export async function deleteOrder(accessToken, orderId) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/orders/${orderId}`, {
    method: 'DELETE',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Sipariş silinemedi'))
  }

  return response.text()
}

export async function checkoutBasket(accessToken, checkoutInfo) {
  const cardDigits = String(checkoutInfo.cardNumber ?? '').replace(/\D/g, '')

  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/orders/checkout`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${accessToken}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      address: checkoutInfo.address,
      paymentMethod: checkoutInfo.paymentMethod,
      cardLastFourDigits: cardDigits ? cardDigits.slice(-4) : null,
    }),
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Sipariş tamamlanamadı'))
  }

  return response.json()
}

export async function cancelMyOrder(accessToken, orderId) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/orders/${orderId}/cancel`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Sipariş iptal edilemedi'))
  }

  return response.json()
}

export async function getMyOrders(accessToken) {
  const response = await fetchWithAutoRefresh(`${API_BASE_URL}/orders/my-orders`, {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  })

  if (!response.ok) {
    throw new Error(await createErrorMessage(response, 'Siparişler alınamadı'))
  }

  return response.json()
}
