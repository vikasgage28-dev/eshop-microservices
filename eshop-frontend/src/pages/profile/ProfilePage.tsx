import { useNavigate } from 'react-router-dom'
import { LogOut, ShieldCheck, User, Mail, Hash, ClipboardList } from 'lucide-react'
import { useAuth } from '@/hooks/useAuth'
import { useGetOrdersByCustomerQuery, useGetOrdersQuery } from '@/api/orderingApi'
import { Button } from '@/components/ui/button'
import { formatCurrency } from '@/lib/utils'

function InfoRow({ icon, label, value }: { icon: React.ReactNode; label: string; value: string }) {
  return (
    <div className="flex items-center gap-3 py-3 border-b border-[#e8e8e8] dark:border-[#3a3a3a] last:border-0">
      <span className="text-gray-400 dark:text-gray-500 flex-shrink-0">{icon}</span>
      <span className="text-xs text-gray-500 dark:text-gray-400 w-20 flex-shrink-0">{label}</span>
      <span className="text-sm font-medium text-gray-900 dark:text-gray-100 truncate">{value}</span>
    </div>
  )
}

export default function ProfilePage() {
  const { fullName, email, userId, roles, isAdmin, logout } = useAuth()
  const navigate = useNavigate()

  const { data: allOrders }      = useGetOrdersQuery(undefined,         { skip: !isAdmin })
  const { data: customerOrders } = useGetOrdersByCustomerQuery(userId!, { skip: isAdmin || !userId })

  const orders  = isAdmin ? allOrders : customerOrders
  const totalSpend = customerOrders?.reduce((s, o) => s + o.totalAmount, 0) ?? 0
  const initials = fullName ? fullName.split(' ').map((n) => n[0]).join('').slice(0, 2).toUpperCase() : 'U'

  const handleLogout = () => { logout(); navigate('/login') }

  return (
    <div className="max-w-lg space-y-5">
      {/* Avatar card */}
      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-6 flex items-center gap-5">
        <div className="w-16 h-16 bg-blue-600 rounded-full flex items-center justify-center flex-shrink-0">
          <span className="text-white text-2xl font-bold">{initials}</span>
        </div>
        <div className="min-w-0">
          <h2 className="text-base font-semibold text-gray-900 dark:text-gray-100 truncate">{fullName ?? 'User'}</h2>
          <p className="text-sm text-gray-500 dark:text-gray-400 truncate">{email}</p>
          <div className="flex flex-wrap gap-1.5 mt-2">
            {roles.map((r) => (
              <span key={r} className="inline-flex items-center gap-1 text-xs font-medium px-2 py-0.5 rounded bg-blue-50 dark:bg-blue-900/30 text-blue-600 dark:text-blue-400">
                <ShieldCheck size={10} /> {r}
              </span>
            ))}
          </div>
        </div>
      </div>

      {/* Account info */}
      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] px-4">
        <InfoRow icon={<User size={15} />}       label="Full name" value={fullName ?? '—'} />
        <InfoRow icon={<Mail size={15} />}        label="Email"     value={email ?? '—'} />
        <InfoRow icon={<Hash size={15} />}        label="User ID"   value={userId?.slice(0, 16) + '…' ?? '—'} />
      </div>

      {/* Stats */}
      {!isAdmin && (
        <div className="grid grid-cols-2 gap-4">
          <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-4 text-center">
            <p className="text-2xl font-bold text-gray-900 dark:text-gray-100">{orders?.length ?? 0}</p>
            <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5 uppercase tracking-wide">Total Orders</p>
          </div>
          <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-4 text-center">
            <p className="text-2xl font-bold text-blue-600 dark:text-blue-400">{formatCurrency(totalSpend)}</p>
            <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5 uppercase tracking-wide">Total Spent</p>
          </div>
        </div>
      )}

      {/* Actions */}
      <div className="flex gap-3">
        <Button
          variant="outline"
          className="flex-1 gap-2 text-sm border-[#e8e8e8] dark:border-[#444] dark:bg-transparent dark:text-gray-300 dark:hover:bg-[#333]"
          onClick={() => navigate('/orders')}
        >
          <ClipboardList size={15} /> My Orders
        </Button>
        <Button
          variant="outline"
          className="flex-1 gap-2 text-sm text-red-600 dark:text-red-400 border-[#e8e8e8] dark:border-[#444] dark:bg-transparent hover:bg-red-50 dark:hover:bg-red-900/20 hover:border-red-200"
          onClick={handleLogout}
        >
          <LogOut size={15} /> Sign Out
        </Button>
      </div>
    </div>
  )
}
