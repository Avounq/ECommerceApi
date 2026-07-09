import InfoRow from './InfoRow'

function UserPanel({
  user,
  mode = 'profile',
  profileForm,
  passwordForm,
  isProfileLoading,
  isPasswordLoading,
  onProfileInputChange,
  onProfileUpdate,
  onPasswordInputChange,
  onPasswordChange,
  onLogout,
}) {
  return (
    <section className="rounded-lg border border-slate-800/80 bg-slate-900/90 p-5 shadow-xl">
      <h2 className="text-lg font-bold">Kullanıcı Bilgileri</h2>

      {!user && (
        <p className="mt-4 text-slate-400">
          Giriş başarılı olunca /api/auth/me cevabı burada görünecek.
        </p>
      )}

      {user && (
        <div className="mt-5 space-y-4">
          <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
            <InfoRow label="Id" value={user.id} />
            <InfoRow label="Kullanıcı Adı" value={user.username} />
            <InfoRow label="Email" value={user.email} />
            <InfoRow label="Ad" value={user.firstName} />
            <InfoRow label="Soyad" value={user.lastName} />
            <InfoRow label="Rol" value={user.role} />
          </div>

          {mode === 'profile' && (
            <>
              <div className="space-y-3 rounded-lg border border-slate-800 bg-slate-950 p-4">
                <h3 className="font-semibold text-slate-100">Profil Bilgileri</h3>
                <p className="text-sm text-slate-500">
                  Bu bölüm salt okunur. Kullanıcı bilgilerini hızlıca kontrol etmek için duruyor.
                </p>

                <ReadOnlyField label="Email" value={user.email} />
                <ReadOnlyField label="Ad" value={user.firstName} />
                <ReadOnlyField label="Soyad" value={user.lastName} />
              </div>

              <form
                onSubmit={onProfileUpdate}
                className="space-y-3 rounded-lg border border-slate-800 bg-slate-950 p-4"
              >
                <h3 className="font-semibold text-slate-100">Profil Düzenle</h3>
                <p className="text-sm text-slate-500">
                  Email, ad ve soyad değiştirilebilir. Kullanıcı adı ve rol değişmez.
                </p>

                <label className="block text-sm text-slate-300">Email</label>
                <input
                  name="email"
                  type="email"
                  value={profileForm.email}
                  onChange={onProfileInputChange}
                  className="w-full rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
                  required
                />

                <label className="block text-sm text-slate-300">Ad</label>
                <input
                  name="firstName"
                  value={profileForm.firstName}
                  onChange={onProfileInputChange}
                  className="w-full rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
                  required
                />

                <label className="block text-sm text-slate-300">Soyad</label>
                <input
                  name="lastName"
                  value={profileForm.lastName}
                  onChange={onProfileInputChange}
                  className="w-full rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
                  required
                />

                <button
                  type="submit"
                  disabled={isProfileLoading}
                  className="w-full rounded-md bg-orange-500 px-4 py-2 text-sm font-bold text-slate-950 transition hover:bg-orange-400 disabled:cursor-not-allowed disabled:bg-slate-600"
                >
                  {isProfileLoading ? 'Güncelleniyor...' : 'Profili Güncelle'}
                </button>
              </form>
            </>
          )}

          {mode === 'password' && (
            <form
              onSubmit={onPasswordChange}
              className="space-y-3 rounded-lg border border-slate-800 bg-slate-950 p-4"
            >
              <h3 className="font-semibold text-slate-100">Şifre Değiştir</h3>

              <label className="block text-sm text-slate-300">Mevcut şifre</label>
              <input
                name="currentPassword"
                type="password"
                value={passwordForm.currentPassword}
                onChange={onPasswordInputChange}
                className="w-full rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
                required
              />

              <label className="block text-sm text-slate-300">Yeni şifre</label>
              <input
                name="newPassword"
                type="password"
                value={passwordForm.newPassword}
                onChange={onPasswordInputChange}
                minLength={8}
                maxLength={12}
                className="w-full rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
                required
              />

              <label className="block text-sm text-slate-300">
                Yeni şifre tekrar
              </label>
              <input
                name="newPasswordAgain"
                type="password"
                value={passwordForm.newPasswordAgain}
                onChange={onPasswordInputChange}
                minLength={8}
                maxLength={12}
                className="w-full rounded-md border border-slate-700 bg-slate-900 px-3 py-2 text-white outline-none transition focus:border-orange-500 focus:ring-2 focus:ring-orange-500/20"
                required
              />

              <button
                type="submit"
                disabled={isPasswordLoading}
                className="w-full rounded-md bg-orange-500 px-4 py-2 text-sm font-bold text-slate-950 transition hover:bg-orange-400 disabled:cursor-not-allowed disabled:bg-slate-600"
              >
                {isPasswordLoading ? 'Değiştiriliyor...' : 'Şifreyi Değiştir'}
              </button>
            </form>
          )}

          <button
            type="button"
            onClick={onLogout}
            className="w-full rounded-md border border-slate-700 px-4 py-2 text-sm font-semibold text-slate-200 hover:border-red-400 hover:text-red-300"
          >
            Çıkış Yap
          </button>
        </div>
      )}
    </section>
  )
}

function ReadOnlyField({ label, value }) {
  return (
    <label className="block text-sm text-slate-300">
      {label}
      <input
        value={value}
        readOnly
        className="mt-1 w-full cursor-not-allowed rounded-md border border-slate-800 bg-slate-900/60 px-3 py-2 text-slate-400 outline-none"
      />
    </label>
  )
}

export default UserPanel
