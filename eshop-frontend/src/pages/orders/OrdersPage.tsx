import { useNavigate } from 'react-router-dom'
import { useGetOrdersQuery, useGetOrdersByCustomerQuery } from '@/api/orderingApi'
import { useGetCustomerByEmailQuery } from '@/api/customerApi'
import { formatCurrency, orderStatusColor } from '@/lib/utils'
import { useAuth } from '@/hooks/useAuth'
import type { OrderStatus } from '@/types/ordering.types'

function statusBadge(status: OrderStatus, statusName: string) {
  const color = orderStatusColor[status] ?? 'bg-gray-100 text-gray-700'
  return <span className={`inline-flex px-2 py-0.5 rounded text-xs font-medium ${color}`}>{statusName}</span>
}

export default function OrdersPage() {
  const { isAdmin, email } = useAuth()
  const navigate = useNavigate()

  // Resolve the Customer profile ID from email (orders are keyed by Customer ID, not Identity user ID)
  const { data: customerProfile } = useGetCustomerByEmailQuery(email ?? '', { skip: isAdmin || !email })

  // Admin sees ALL orders — Customer sees only their own orders (by Customer profile ID)
  const { data: allOrders,      isLoading: loadingAll      } = useGetOrdersQuery(undefined,                      { skip: !isAdmin })
  const { data: customerOrders, isLoading: loadingCustomer } = useGetOrdersByCustomerQuery(customerProfile?.id ?? '', { skip: isAdmin || !customerProfile?.id })

  const orders    = isAdmin ? allOrders : customerOrders
  const isLoading = isAdmin ? loadingAll : loadingCustomer

  return (
    <div className="space-y-4">
      <div>
        <h1 className="text-xl font-bold text-gray-900 dark:text-gray-100">Orders</h1>
        <p className="text-sm text-gray-500 dark:text-gray-400 mt-0.5">
          {isAdmin
            ? `${orders?.length ?? 0} total orders across all customers`
            : `${orders?.length ?? 0} orders placed by you`}
        </p>
      </div>

      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] overflow-hidden">
        <table className="w-full text-sm">
          <thead>
            <tr className="bg-[#f8f8f8] dark:bg-[#222]">
              <th className="text-left px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Order</th>
              <th className="text-left px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Customer</th>
              <th className="text-left px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Date</th>
              <th className="text-left px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Status</th>
              <th className="text-right px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Total</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#f0f0f0] dark:divide-[#333]">
            {isLoading && Array.from({ length: 5 }).map((_, i) => (
              <tr key={i}>
                {Array.from({ length: 5 }).map((_, j) => (
                  <td key={j} className="px-4 py-3">
                    <div className="h-3.5 bg-gray-100 dark:bg-[#333] rounded animate-pulse" />
                  </td>
                ))}
              </tr>
            ))}
            {orders?.map((order) => (
              <tr
                key={order.id}
                className="hover:bg-[#fafafa] dark:hover:bg-[#222] cursor-pointer transition-colors"
                onClick={() => navigate(`/orders/${order.id}`)}
              >
                <td className="px-4 py-2.5 font-mono text-xs text-gray-500 dark:text-gray-400">{order.id.slice(0, 8)}…</td>
                <td className="px-4 py-2.5 text-gray-700 dark:text-gray-300">{order.customerEmail}</td>
                <td className="px-4 py-2.5 text-gray-500 dark:text-gray-400">
                  {new Date(order.orderDate).toLocaleDateString()}
                </td>
                <td className="px-4 py-2.5">
                  {statusBadge(order.status, order.statusName ?? order.status)}
                </td>
                <td className="px-4 py-2.5 text-right font-semibold text-gray-900 dark:text-gray-100">
                  {formatCurrency(order.totalAmount)}
                </td>
              </tr>
            ))}
            {!isLoading && orders?.length === 0 && (
              <tr>
                <td colSpan={5} className="px-4 py-12 text-center text-gray-400">No orders found</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  )
}
