function InfoRow({ label, value }) {
  return (
    <div className="rounded-lg border border-slate-800 bg-slate-950 p-3 transition hover:border-orange-500/50">
      <p className="text-xs uppercase tracking-wide text-slate-500">{label}</p>
      <p className="mt-1 break-all text-sm text-slate-100">{value ?? '-'}</p>
    </div>
  )
}

export default InfoRow
