function AuthForm({
  authMode,
  username,
  password,
  registerForm,
  message,
  isLoading,
  isRegisterLoading,
  onAuthModeChange,
  onLogin,
  onRegister,
  onUsernameChange,
  onPasswordChange,
  onRegisterInputChange,
}) {
  return (
    <form
      onSubmit={authMode === 'login' ? onLogin : onRegister}
      className="rounded-lg border border-slate-800/80 bg-slate-900/90 p-6 shadow-2xl shadow-slate-950/50 backdrop-blur"
    >
      <p className="text-sm font-semibold uppercase tracking-wide text-orange-300">
        ECommerce
      </p>
      <h1 className="mt-2 text-3xl font-black tracking-tight">
        {authMode === 'login' ? 'Giriş Yap' : 'Kayıt Ol'}
      </h1>
      <p className="mt-2 text-sm text-slate-400">
        Ürünleri gez, sepete ekle ve siparişini tek panelden takip et.
      </p>

      <div className="mt-6 grid grid-cols-2 rounded-full border border-slate-700 bg-slate-950 p-1">
        <button
          type="button"
          onClick={() => onAuthModeChange('login')}
          className={`rounded-full px-3 py-2 text-sm font-semibold transition ${
            authMode === 'login'
              ? 'bg-orange-500 text-slate-950'
              : 'text-slate-300 hover:text-white'
          }`}
        >
          Giriş Yap
        </button>
        <button
          type="button"
          onClick={() => onAuthModeChange('register')}
          className={`rounded-full px-3 py-2 text-sm font-semibold transition ${
            authMode === 'register'
              ? 'bg-orange-500 text-slate-950'
              : 'text-slate-300 hover:text-white'
          }`}
        >
          Kayıt Ol
        </button>
      </div>

      {authMode === 'login' && (
        <>
          <label className="mt-6 block text-sm text-slate-300">
            Kullanıcı adı
          </label>
          <input
            value={username}
            onChange={onUsernameChange}
            className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            required
          />

          <label className="mt-4 block text-sm text-slate-300">Şifre</label>
          <input
            type="password"
            value={password}
            onChange={onPasswordChange}
            className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            required
          />
        </>
      )}

      {authMode === 'register' && (
        <>
          <label className="mt-6 block text-sm text-slate-300">
            Kullanıcı adı
          </label>
          <input
            name="username"
            value={registerForm.username}
            onChange={onRegisterInputChange}
            minLength={6}
            maxLength={50}
            className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            required
          />

          <label className="mt-4 block text-sm text-slate-300">Email</label>
          <input
            name="email"
            type="email"
            value={registerForm.email}
            onChange={onRegisterInputChange}
            minLength={20}
            maxLength={50}
            className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            required
          />

          <label className="mt-4 block text-sm text-slate-300">Ad</label>
          <input
            name="firstName"
            value={registerForm.firstName}
            onChange={onRegisterInputChange}
            minLength={3}
            maxLength={20}
            className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            required
          />

          <label className="mt-4 block text-sm text-slate-300">Soyad</label>
          <input
            name="lastName"
            value={registerForm.lastName}
            onChange={onRegisterInputChange}
            minLength={3}
            maxLength={20}
            className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            required
          />

          <label className="mt-4 block text-sm text-slate-300">Şifre</label>
          <input
            name="password"
            type="password"
            value={registerForm.password}
            onChange={onRegisterInputChange}
            minLength={8}
            maxLength={12}
            className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            required
          />

          <label className="mt-4 block text-sm text-slate-300">
            Şifre tekrar
          </label>
          <input
            name="confirmPassword"
            type="password"
            value={registerForm.confirmPassword}
            onChange={onRegisterInputChange}
            minLength={8}
            maxLength={12}
            className="mt-2 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
            required
          />
        </>
      )}

      <button
        type="submit"
        disabled={authMode === 'login' ? isLoading : isRegisterLoading}
        className="mt-6 w-full rounded-md bg-orange-500 px-4 py-2 font-bold text-slate-950 shadow-lg shadow-orange-950/30 transition hover:bg-orange-400 disabled:cursor-not-allowed disabled:bg-slate-600"
      >
        {authMode === 'login' &&
          (isLoading ? 'Giriş yapılıyor...' : 'Giriş Yap')}
        {authMode === 'register' &&
          (isRegisterLoading ? 'Kayıt yapılıyor...' : 'Kayıt Ol')}
      </button>

      {message && (
        <p className="mt-4 rounded-md border border-slate-800 bg-slate-950 p-3 text-sm text-slate-300">
          {message}
        </p>
      )}
    </form>
  )
}

export default AuthForm
